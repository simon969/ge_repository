using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ge_repository.interfaces;
using ge_repository.ESdat;
using ge_repository.OtherDatabase;
using ge_repository.Models;
using ge_repository.AGS;
using static ge_repository.Extensions.Extensions;
namespace ge_repository.services
{
    public class ESdatFileService : IESdatFileService
    {
          private static int NOT_FOUND = -1;
        
    public int loadTemplateAndLines (ge_data data_file, 
                                            ge_search template, 
                                            string table, 
                                            string sheet, 
                                            IDataService _dataService,  
                                            out ge_search template_loaded, 
                                            out string[] lines ) {

            lines = null;
            template_loaded = null;
            
            if (data_file.fileext == ".csv") {
                    var resp = _dataService.GetFileAsLines(data_file.Id);
                    lines = (string[]) resp.Result;
                    SearchTerms st = new SearchTerms();
                    template_loaded = st.findSearchTerms(template,table, lines);
                    if (template_loaded.search_tables.Count==0) {
                        return -1;
                    }
            }

            if (data_file.fileext == ".xlsx") {
                var resp = _dataService.GetFileAsMemoryStream(data_file.Id);
                using (MemoryStream ms = (MemoryStream) resp.Result) {
                    ge_log_workbook wb = new ge_log_workbook(ms);
                    
                    SearchTerms st = new SearchTerms();  
                    if (sheet.Contains(",")) {
                    string[] sheets = sheet.Split (",");
                    template_loaded =  st.findSearchTerms (template, table, wb, sheets);  
                    } else {
                    template_loaded  =  st.findSearchTerms (template, table, wb, sheet);
                    }
                    
                    if (template_loaded.search_tables.Count==0) {
                        lines = null;
                        return -1;
                    }
                    
                    wb.setWorksheet(template_loaded.search_tables[0].sheet);
                    wb.evaluateSheet();
                    lines = wb.WorksheetToTable(50,true);
                    wb.close();
                    
                    return 1;
                }
            }
            
            return -1;
            
        }
        public async Task<ge_esdat_file> NewFile(Guid Id, 
                                     Guid templateId, 
                                     string table, 
                                     string sheet, 
                                     IDataService _dataService) {
                                        
        string[] lines = null;
        ge_search template_loaded = null;

        ge_search template = await _dataService.GetFileAsClass<ge_search>(templateId);
        ge_data data_file = await _dataService.GetDataById(Id);

        int resp = loadTemplateAndLines (data_file,template,table,sheet,_dataService, out template_loaded, out lines);
        
        if (resp == -1) {
            return null;
        }

        return NewFile ( template_loaded, 
                            lines, 
                            Id, 
                            templateId);
    }   
     public ge_esdat_file NewFile( ge_search dic, 
                                string[] lines,
                                Guid dataId,
                                Guid templateId) {
            
            
           
            search_range sr =  dic.search_tables[0].header;
            string header_row = dic.search_items.Find(e=>e.name==sr.search_item_name).row_text;
            string[] columns =  Split(header_row); 
            int line_start = dic.data_start_row(NOT_FOUND);
            int line_end = dic.data_end_row(lines.Count());

            ge_esdat_file  _esdat = new ge_esdat_file(columns);
            
            for (int i = line_start; i<line_end; i++) {
                string line = lines[i];
                if (line.Length>0) {
                string[] values = Split(line);
                DataRow row = _esdat.dt.NewRow();
                set_values(columns, values, row);
                _esdat.dt.Rows.Add(row);
                }
            }

            return  _esdat;
    }     
    private string[] Split(string line) {

        if (line.Contains("\",\"")) {
            string trimmed = line.Substring(1,line.Length-2); 
            string[] values = trimmed.Split("\",\"");
            return values;
        } else {
             string[] values = line.Split(",");
             return values;
        }

    }
    private void set_values(string[] header, string[] values, DataRow row) {

           for (int i=0; i<header.Count();i++) {
           row[header[i]] = values[i]; 
           }
    }
    }

    public class ESdatAGSService : IESdatAGSService {
        public async Task<IAGSGroupTables> CreateAGS (Guid Id,Guid tablemapId, string[] agstables,string options, IDataService _dataService) {

                ge_esdat_file es_file = await _dataService.GetFileAsClass<ge_esdat_file>(Id);
                ge_table_map map = await _dataService.GetFileAsClass<ge_table_map>(tablemapId);
                IAGSGroupTables ags_file = CreateAGS (es_file, map, agstables,options);

                return ags_file;
        }
        public IAGSGroupTables CreateAGS (ge_esdat_file es_file, ge_table_map map, string[] agstables,string options) {

                IAGSGroupTables ags_tables = new AGS404GroupTables();

                if (agstables.Contains("PROJ")) {
                    List<PROJ> proj = getPROJ();
                    ags_tables.AddTable (proj);
                }

                if (agstables.Contains("ERES")) {
                    foreach (table_map tm in map.table_maps.Where(m=>m.destination=="ERES")) {
                        if (tm!=null) {
                            List<ERES> list = ConvertDataTable<ERES>(es_file.dt, tm);
                            ags_tables.AddTable(list);
                            if (agstables.Contains("ABBR")) {
                                List<ABBR> abbr =  getABBR (list);
                                ags_tables.AddTable (abbr);
                            }
                            if (agstables.Contains("UNIT")) {
                                List<UNIT> unit =  getUNIT (list);
                                ags_tables.AddTable (unit);
                            }
                        }
                    }
                }

                if (agstables.Contains("TRAN")) {
                    List<TRAN> tran = getTRAN();
                    ags_tables.AddTable (tran);
                }

                if (agstables.Contains("POINT")) {
                    foreach (table_map tm in map.table_maps.Where(m=>m.destination=="LOCA")) {
                        List<POINT> list = ConvertDataTable<POINT>(es_file.dt, tm);
                        string[] distinct = list.Select (m=>m.PointID).Distinct().ToArray();
                        List<POINT> unique = getFirsts (list,distinct);
                        ags_tables.AddTable(unique);
                            if (agstables.Contains("ABBR")) {
                                List<ABBR> abbr =  getABBR (list);
                                ags_tables.AddTable (abbr);
                            }
                    }
                }
                
                if (agstables.Contains("SAMP")) {
                    foreach(table_map tm in map.table_maps.Where(m=>m.destination=="SAMP")) {
                        List<SAMP> list  = ConvertDataTable<SAMP>(es_file.dt, tm);
                        string[] distinct = list.Select (m=>m.SAMP_ID).Distinct().ToArray();
                        List<SAMP> unique = getFirsts (list,distinct);
                        ags_tables.AddTable(unique);
                    }
                }

                if (agstables.Contains("MOND")) {
                    foreach (table_map tm in map.table_maps.Where(m=>m.destination=="MOND")) {
                        if (tm!=null) {
                            List<MOND> list = ConvertDataTable<MOND>(es_file.dt, tm);
                            ags_tables.AddTable(list);
                            if (agstables.Contains("ABBR")) {
                                List<ABBR> abbr =  getABBR (list);
                                ags_tables.AddTable (abbr);
                            }
                            if (agstables.Contains("UNIT")) {
                                List<UNIT> unit =  getUNIT (list);
                                ags_tables.AddTable (unit);
                            }
                        }
                    }
                }

                if (agstables.Contains("MONG")) {
                    foreach (table_map tm in map.table_maps.Where(m=>m.destination=="MONG")) {
                        if (tm!=null) {
                            List<MONG> list = ConvertDataTable<MONG>(es_file.dt, tm);
                            ags_tables.AddTable(list);
                            if (agstables.Contains("ABBR")) {
                                List<ABBR> abbr =  getABBR (list);
                                ags_tables.AddTable (abbr);
                            }
                        }
                    }
                }

                if (agstables.Contains("ABBR")) {
                    foreach (table_map tm in map.table_maps.Where(m=>m.destination=="ABBR")) {
                        if (tm!=null) {
                            List<ABBR> list = ConvertDataTable<ABBR>(es_file.dt, tm);
                            ags_tables.AddTable(list);
                        }
                    }
                }
                
                if (agstables.Contains("UNIT")) {
                    foreach (table_map tm in map.table_maps.Where(m=>m.destination=="UNIT")) {
                        if (tm!=null) {
                            List<UNIT> list = ConvertDataTable<UNIT>(es_file.dt, tm);
                            ags_tables.AddTable(list);
                        }
                    }
                }
                
                if (agstables.Contains("TYPE")) {
                    foreach (table_map tm in map.table_maps.Where(m=>m.destination=="TYPE")) {
                        if (tm!=null) {
                            List<TYPE> list = ConvertDataTable<TYPE>(es_file.dt, tm);
                            ags_tables.AddTable(list);
                        }
                    }
                }
                
                if (agstables.Contains("DICT")) {
                    foreach (table_map tm in map.table_maps.Where(m=>m.destination=="DICT")) {
                        if (tm!=null) {
                            List<DICT> list = ConvertDataTable<DICT>(es_file.dt, tm);
                            ags_tables.AddTable(list);
                        }
                    }
                }

                return ags_tables;
        }
        private List<TRAN> getTRAN() {
            
            List<TRAN> list =  new List<TRAN>();
            
            TRAN t = new TRAN();
            t.TRAN_DATE = DateTime.Now;
            t.TRAN_AGS = "4.04";
            list.Add (t);

            return list;


        }
        private List<PROJ> getPROJ() {
            
            List<PROJ> list =  new List<PROJ>();
            
            PROJ p = new PROJ();
            p.PROJ_MEMO = $"ESdata data export {DateTime.Now}";
            list.Add (p);

            return list;


        }
        private List<ABBR> getABBR(List<MONG> list) {
                
                string[] mong_types = list.Select (m=>m.MONG_TYPE).Distinct().ToArray();
                
                List<ABBR> abbr = new List<ABBR>();

                foreach (string s in mong_types) {
                    ABBR ab =  new ABBR(); 
                    ab.ABBR_HDNG = "MONG_TYPE";
                    ab.ABBR_CODE = s;
                    ab.ABBR_DESC = "";
                    abbr.Add (ab);
                }

                return abbr;
        }
         private List<ABBR> getABBR(List<MOND> list) {
                
                string[] mond_types = list.Select (m=>m.MOND_TYPE).Distinct().ToArray();
                
                List<ABBR> abbr = new List<ABBR>();

                foreach (string s in mond_types) {
                    ABBR ab =  new ABBR(); 
                    ab.ABBR_HDNG = "MOND_TYPE";
                    ab.ABBR_CODE = s;
                    ab.ABBR_DESC = "";
                    abbr.Add (ab);
                }

                return abbr;
        }
         private List<ABBR> getABBR(List<POINT> list) {
                
                string[] loca_types = list.Select (m=>m.LOCA_TYPE).Distinct().ToArray();
                
                List<ABBR> abbr = new List<ABBR>();

                foreach (string s in loca_types) {
                    ABBR ab =  new ABBR(); 
                    ab.ABBR_HDNG = "LOCA_TYPE";
                    ab.ABBR_CODE = s;
                    ab.ABBR_DESC = "";
                    abbr.Add (ab);
                }

                return abbr;
        }
        private List<ABBR> getABBR(List<ERES> list) {
                
                //Abbreviations ERES_CODE
                string[] eres_codes = list.Select (m=>m.ItemKey).Distinct().ToArray();
                
                List<ERES> eres_codesList =  new List<ERES>();
                
                foreach (string s in eres_codes) {
                    ERES first = list.Where(el=>el.ItemKey==s).First();
                    eres_codesList.Add (first);
                }

                List<ABBR> abbr = new List<ABBR>();

                foreach (ERES eres in eres_codesList) {
                    ABBR ab =  new ABBR(); 
                    ab.ABBR_HDNG = "ERES_CODE";
                    ab.ABBR_CODE = eres.ItemKey;
                    ab.ABBR_DESC = eres.ERES_NAME;
                    abbr.Add (ab);
                }
                
                // Abbreviations ERES_MATX
                string[] eres_matx = list.Select (m=>m.ERES_MATX).Distinct().ToArray();
                foreach (string s in eres_matx) {
                    ABBR ab =  new ABBR(); 
                    ab.ABBR_HDNG = "ERES_MATX";
                    ab.ABBR_CODE = s;
                    ab.ABBR_DESC = "";
                    abbr.Add (ab);
                }

                return abbr;
        }
        private List<UNIT> getUNIT(List<ERES> list) {
                
                string[] distinct = list.Select (m=>m.ERES_DUNI).Distinct().ToArray();
                
                List<UNIT> uList =  new List<UNIT>();
                
                foreach (string s in distinct) {
                    UNIT u =  new UNIT(); 
                    u.UNIT_UNIT = s;
                    uList.Add (u);
                }

                return uList;
        }
        private List<UNIT> getUNIT(List<MOND> list) {
                
                string[] distinct = list.Select (m=>m.MOND_UNIT).Distinct().ToArray();
                
                List<UNIT> uList =  new List<UNIT>();
                
                foreach (string s in distinct) {
                    UNIT u =  new UNIT(); 
                    u.UNIT_UNIT = s;
                    uList.Add (u);
                }

                return uList;
        }
        private List<POINT> getFirsts(List<POINT> list, string[] distinct) {
                
                List<POINT> newList =  new List<POINT>();
                
                foreach (string s in distinct) {
                    POINT first = list.Where(el=>el.PointID==s).First();
                    newList.Add (first);
                }

                return newList;
        }
        private List<SAMP> getFirsts(List<SAMP> list, string[] distinct) {
                
                List<SAMP> newList =  new List<SAMP>();
                
                foreach (string s in distinct) {
                    SAMP first = list.Where(el=>el.SAMP_ID==s).First();
                    newList.Add (first);
                }

                return newList;
        }
    }
    public class DataESdatFileService : DataService, IDataESdatFileService {
    public DataESdatFileService(IUnitOfWork unitOfWork): base (unitOfWork) {}
    public Task<ge_esdat_file> NewData(Guid projectId, string UserId, ge_esdat_file esdat_file, string format) {

        return null;
    } 

    }

}


