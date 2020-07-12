using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ge_repository.Models;
using ge_repository.Authorization;
using static ge_repository.Authorization.Constants;
using ge_repository.Extensions;
using System.Data.SqlClient;
using System.Data;
using ge_repository.OtherDatabase;
using Newtonsoft.Json;
using ge_repository.spatial;


namespace ge_repository.Controllers
{

    public class ge_logController: ge_Controller  {     
   
       
        public ge_log_file ge_file {get;set;}
        public string[] tables_wdepth {get;} = new string[] {"",""};
        public string[] tables_wquality {get;}= new string[] {"",""};
        public List<MOND> MOND {get;set;}
        public  List<MONG> MONG {get;set;}
        public  List<POINT> POINT {get;set;}
        private int NOT_FOUND = -1;
        
        private string[] READ_STOPS = {"\"\"",""};


        private static string DATE_FORMAT_AGS = "yyyy-MM-dd HH:mm:ss";
        private static string DP3 = "0.000";
        private static string DP2 = "0.00";
        private static string DP1 = "0.0";

        private static int MAX_SEARCH_LINES = 100;
     
         public ge_logController(

            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,  
            IHostingEnvironment env ,
            IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
           
        }
    private ge_search findSearchTerms (ge_search dic, string name, ge_log_workbook wb) {
    
        ge_search new_dic = new ge_search();

        new_dic.name = "logger header created:" + DateTime.Now;
       
        foreach (search_table st in dic.search_tables.Where(e=>e.name==name)) {

                        wb.setWorksheet(st.id);
                        
                        if (wb.worksheet==null) {
                        continue;
                        }
        
            foreach  (search_item si in dic.search_items) {
                si.value = wb.matchReturnValue(si.search_text, si.start_offset, si.row_offset);
                // ge_log_workbook is zero based array of worksheet so will match string[] 
                si.row = wb.matchReturnRow(si.search_text);
                new_dic.search_items.Add (si);
            }
        
            Boolean colNotFound=false;
            
            search_range sh = st.header;
            
            int header_row = 0;
            int header_offset = 0;
            search_item si2 = null;
            colNotFound = false;
            
            if (!String.IsNullOrEmpty(sh.search_item)) {
                si2 =  new_dic.search_items.Find(e=>e.name==sh.search_item);
                // ge_log_workbook is zero based array of worksheet so will match[]
                header_row = si2.row ;
                header_offset = si2.row_offset;

            }

            foreach (value_header vh in st.headers) { 
     
                        int i = wb.matchReturnColumn(vh.search_text,header_row,header_offset);
                        
                        if (i == NOT_FOUND) {
                            new_dic.status = $"column search text [{vh.search_text}] of table [{name}] not found";
                            colNotFound=true;
                            break;
                        } else {
                            // ge_log_workbook is zero based array of worksheet so 
                            // add 1 so column is correctly located in the csv file
                            vh.found = i + vh.col_offset;
                            vh.source = ge_log_constants.SOURCE_ACTUAL;
                        }
            }

            if (colNotFound==false) {
                if (name == null | name == "" | name == st.name) {
                    new_dic.search_tables.Add (st);
                    new_dic.setFoundTableValues (st);
                    new_dic.status =$"all columns of table {name} found";

                    break; 
                }
            }

        }
        
       
        
        
        return new_dic;
    }

    private ge_search findSearchTerms(ge_search dic, string name, string[] lines) {
        
        ge_search new_dic = new ge_search();

        new_dic.name = "logger header created:" + DateTime.Now;
        int max_line = lines.Count()-1;
        int max_search_lines = Math.Min(max_line, MAX_SEARCH_LINES);

        foreach  (search_item si in dic.search_items) {
            for (int i = 0; i<max_search_lines; i++) {
                if (lines[i].Contains(si.search_text)) {
                     si.row = i;
                     si.row_text = lines[i + si.row_offset];
                     si.value_string();
                     new_dic.search_items.Add (si);
                     break;
                }
            }

        }
        
        search_item end_line = dic.search_items.Find(i=>i.name=="data_end");
        
        if (end_line!=null) {
            if (lines[max_line].Contains(end_line.search_text))
            end_line.row_text = lines[max_line];
            end_line.row = max_line;
            new_dic.search_items.Add (end_line);
        }
        
        new_dic.array_items = new_dic.getSplitItems();
        
        Boolean colNotFound=false;
        
        foreach (search_table st in dic.search_tables) {

            search_range sh = st.header;
            string header = "";
            search_item si = null;
            colNotFound = false;
            if (sh.row>0) {header = lines[sh.row];}
            if (!String.IsNullOrEmpty(sh.search_item)) {
                si =  new_dic.search_items.Find(e=>e.name==sh.search_item);
                if (si==null) { continue;}
                header = new_dic.search_items.Find(e=>e.name==sh.search_item).row_text;
            }
            
            string[] columns =  header.Split(",");
                foreach (value_header vh in st.headers) { 
                            if (vh.found == NOT_FOUND) {
                                int i = columns.findFirstIndexContains(vh.search_text);
                                if (i == NOT_FOUND) {
                                    new_dic.status = $"column search text [{vh.search_text}] of table [{name}] not found";
                                    colNotFound=true;
                                    break;
                                } else {
                                vh.found = i + vh.col_offset;
                                vh.source = ge_log_constants.SOURCE_ACTUAL;
                                }
                            }
                }

                if (colNotFound==false) {
                    if (name == null | name == "" | name == st.name) {
                        new_dic.search_tables.Add (st);
                        new_dic.setFoundTableValues (st);
                        new_dic.status =$"all columns of table {name} found";

                        break; 
                    }
                }

        }
        
       
        
        
        return new_dic;
    }
private async Task<ge_log_file> ReadFile(Guid Id,
                                          Guid templateId,
                                          string table = "") {
            
            var template = await new ge_dataController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getDataAsClass<ge_search>(templateId);
            
            var file = await new ge_dataController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Get(Id);

            if (template==null || file==null) {
                return null;
            }            

            string[] lines = null;
            ge_search template_loaded = null;

            if (file.fileext == ".csv") {
                lines = await new ge_dataController( _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getDataByLines(Id);
                template_loaded = findSearchTerms(template,table, lines);
                if (template_loaded.search_tables.Count==0) {
                    return null; //     return Json (dic_loaded);
                }
            }

            if (file.fileext == ".xlsx") {
                using (MemoryStream ms = await new ge_dataController(  _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).GetMemoryStream(Id)) {
                    ge_log_workbook wb = new ge_log_workbook(ms);  
                    template_loaded  =  findSearchTerms (template, table, wb);
                    if (template_loaded.search_tables.Count==0) {
                        return null; //     return Json (dic_loaded);
                    }
                    lines = wb.getTable(template_loaded.search_tables[0].id);
                    wb.close();

                }
            }

            ge_file = getNewLoggerFile (template_loaded, lines);

            if (ge_file==null) {
            //    dic_loaded.status = $"Unable to process logger file {_data.filename} please check the search template for correctly finding the required fields";
            //    return Json (dic_loaded);
            return null;
            }

            ge_file.readingAggregates = getAggregates(ge_file);
            ge_file.dataId = Id;
            ge_file.templateId = templateId;
            return ge_file;
}
[HttpPost]
public async Task<IActionResult>  setFileHeaderValue (Guid Id, 
                                                      string name, 
                                                      string value) {
            
            var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            if (_data == null) {
                return NotFound();
            }
            







            

            return Json($" FileHeader value set name={name} value= {value}");

}

[HttpPost]
 public async Task<IActionResult> Read(Guid Id,
                                          Guid templateId,
                                          string table = "",
                                          string format = "view", 
                                          Boolean save = false
                                            ) {
            return await ReadFile(Id,templateId, table,format, save);                                
}

public async Task<IActionResult> ReadFile(Guid Id,
                                          Guid templateId,
                                          string table = "",
                                          string format = "view", 
                                          Boolean save = false
                                            ) {

            if (Id == Guid.Empty) {
                return NotFound();
            }
            
            if (templateId == Guid.Empty) {
                return NotFound();
            }

           
            var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            if (_data == null)
            {
                return NotFound();
            }
            var _template = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == templateId);
            
            if (_template == null)
            {
                return NotFound();
            }

            var empty_data = new ge_data();
            
            var user = GetUserAsync();
            
            if (user==null) {
              return RedirectToPageMessage (msgCODE.USER_NOTFOUND);
            } 

            int IsDownloadAllowed = _context.IsOperationAllowed(Constants.DownloadOperationName, _data.project, _data);
            Boolean CanUserDownload = _context.DoesUserHaveOperation(Constants.DownloadOperationName,_data.project, user.Result.Id);
            
            int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, _data.project, empty_data);
            Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName,_data.project, user.Result.Id);

            if (IsDownloadAllowed!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_PROHIBITED);
            }
            
            if (!CanUserDownload) {
            return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_USER_PROHIBITED);
            }

            if (IsCreateAllowed!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
            }
            if (!CanUserCreate) {
            return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
            }
          
           
            var ge_file = await ReadFile(Id, templateId, table);

            if (ge_file==null){
                return Json($"Unable to locate table ({table}) from template file ({_template.filename}) in data file ({_data.filename})");
            }

            if (ge_file.channel!=table) {
                table = ge_file.channel;
            }

            var exist_ge_file = await GetFile(Id, table,false);
                    
                    if (exist_ge_file==null) {
                        ViewData["fileStatus"] = "File records not written";
                    } else {
                        if (exist_ge_file.readingAggregates==ge_file.readingAggregates) {
                            ViewData["fileStatus"] = "File records written match";
                        } else {
                            ViewData["fileStatus"] = "File records written do not match";
                        }
                    }
                    
                    if (save==true) {
                        if (exist_ge_file !=null) {
                            
                           // int del = await DeleteFile(Id);
                           // ViewData["fileStatus"] = $"Existing records deleted ({del})";
                           ge_file.Id  = exist_ge_file.Id;
                           int updated = await UpdateFile(ge_file,true);
                           ViewData["fileStatus"] = $"File records updated ({updated})";
                        } else {
                        int add = await AddNewFile(ge_file);
                            if (add>0) { 
                                ViewData["fileStatus"] = $"File records written ({add})";
                            }
                        }
                    }
                  
            //      return Ok();

            return View("ReadData", ge_file);
 }

[HttpPost]
public async Task<IActionResult> ReadFile(Guid Id,
                                          Guid templateId,
                                          string table,
                                          string bh_ref,
                                          float? probe_depth,
                                          string format = "view", 
                                          Boolean save = false
                                            )  {
     return await ReadFileWith(Id,templateId, table, bh_ref,probe_depth,format,save);
 }

 public async Task<IActionResult> ReadFileWith(Guid Id,
                                          Guid templateId,
                                          string table,
                                          string bh_ref,
                                          float? probe_depth,
                                          string format = "view", 
                                          Boolean save = false
                                            ) {

            if (Id == Guid.Empty) {
                return NotFound();
            }
            
            if (templateId == Guid.Empty) {
                return NotFound();
            }

           
            var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            if (_data == null)
            {
                return NotFound();
            }
            var _template = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == templateId);
            
            if (_template == null)
            {
                return NotFound();
            }

            var empty_data = new ge_data();
            
            var user = GetUserAsync();
            
            if (user==null) {
              return RedirectToPageMessage (msgCODE.USER_NOTFOUND);
            } 

            int IsDownloadAllowed = _context.IsOperationAllowed(Constants.DownloadOperationName, _data.project, _data);
            Boolean CanUserDownload = _context.DoesUserHaveOperation(Constants.DownloadOperationName,_data.project, user.Result.Id);
            
            int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, _data.project, empty_data);
            Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName,_data.project, user.Result.Id);

            if (IsDownloadAllowed!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_PROHIBITED);
            }
            
            if (!CanUserDownload) {
            return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_USER_PROHIBITED);
            }

            if (IsCreateAllowed!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
            }
            if (!CanUserCreate) {
            return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
            }
          
           
            var ge_file = await ReadFile(Id, templateId, table);

            if (ge_file==null){
                return Json($"Unable to locate table ({table}) from template file ({_template.filename}) in data file ({_data.filename})");
            }

            if (ge_file.channel!=table) {
                table = ge_file.channel;
            }


           ge_log_helper gf = new ge_log_helper();

           gf.log_file = ge_file;

           gf.AddOverrides (probe_depth, bh_ref);
           
           var exist_ge_file = await GetFile(Id, table, false);
                    
                    if (exist_ge_file==null) {
                        ViewData["fileStatus"] = "File records not written";
                    } else {
                        if (exist_ge_file.readingAggregates==ge_file.readingAggregates) {
                            ViewData["fileStatus"] = "File records written match";
                        } else {
                            ViewData["fileStatus"] = "File records written do not match";
                        }
                    }
                    
                    if (save==true) {
                        if (exist_ge_file !=null) {
                            
                           // int del = await DeleteFile(Id);
                           // ViewData["fileStatus"] = $"Existing records deleted ({del})";
                           ge_file.Id  = exist_ge_file.Id;
                           int updated = await UpdateFile(ge_file,true);
                           ViewData["fileStatus"] = $"File records updated ({updated})";
                        } else {
                        int add = await AddNewFile(ge_file);
                            if (add>0) { 
                                ViewData["fileStatus"] = $"File records written ({add})";
                            }
                        }
                    }
                  
            //      return Ok();
            if (format == "view") {
            return View("ReadData", ge_file);
            }

            return Ok(ge_file);
    
 }
// 
[HttpPost]
public async Task<IActionResult> CalculateVWT(  Guid Id,
                                                Guid? templateId,
                                                string table,
                                                Guid?[] baroIds,
                                                float? level_offset,
                                                int? baro_buffer_mins,
                                                int? atmos_m,
                                                float? probe_depth,
                                                float? dry_depth,
                                                string bh_ref,
                                                float? zero_reading,
                                                float? zero_temp,
                                                float? linear_factor,
                                                float? baro_pressure_m,
                                                float? temp_at_calib,
                                                float? const_a,
                                                float? const_b,
                                                float? const_c,
                                                float? const_t,
                                                string format = "view", 
                                                Boolean save = false) {
            if (Id == Guid.Empty)
            {
                return NotFound();
            }
            
            var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            if (_data == null)
            {
                return NotFound();
            }
            
            var empty_data = new ge_data();
            var user = GetUserAsync().Result;
            
            if (user != null) {

                    int IsDownloadAllowed = _context.IsOperationAllowed(Constants.DownloadOperationName, _data.project, _data);
                    Boolean CanUserDownload = _context.DoesUserHaveOperation(Constants.DownloadOperationName,_data.project,user.Id);
                    
                    int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, _data.project, empty_data);
                    Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName,_data.project,user.Id);

                    if (IsDownloadAllowed!=geOPSResp.Allowed) {
                        return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_PROHIBITED);
                    }
                    
                    if (!CanUserDownload) {
                    return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_USER_PROHIBITED);
                    }

                    if (IsCreateAllowed!=geOPSResp.Allowed) {
                        return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
                    }
                    if (!CanUserCreate) {
                    return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
                    }
            }

            ge_log_file exist_log_file = await GetFile(Id, table);
            ge_log_file log_file = null;
            
            if (exist_log_file==null && templateId==null) {
            return Json($"A logger file has not been found for data file: {_data.filename} table: {table} please provide templateId to read this data file");
            }

            if (templateId!=null) {
            log_file = await ReadFile (Id, templateId.Value, table);
            } else {
            log_file = exist_log_file;
            }
            
            if (log_file==null) {
                return Json($"The data file: {_data.filename} table: {table}) cannot be read with templateId provided");
            } else {
              if (exist_log_file !=null) {
                 log_file.Id = exist_log_file.Id; 
              }
            }
                        
            log_file.data = _data;

            ge_log_calculateVWT ge_calculateVWT = new ge_log_calculateVWT() ;

            ge_calculateVWT.log_file = log_file;

            if (baroIds!=null) {
                    foreach (Guid? baroId in baroIds) {  
                        if (baroId!=null) {
                            var _baro_data = await _context.ge_data
                                        .Include(d =>d.project)
                                        .SingleOrDefaultAsync(m => m.Id == baroId);
                            ge_log_file baro_file = await GetFile(baroId.Value);
                            
                            if (baro_file==null) {
                                return Json($"Baro logger file records has not been found for data file ({_baro_data.filename}), please create baro logger file, before calculating wdepth");   
                            }
                            
                            baro_file.data = _baro_data;
                            ge_calculateVWT.baro_files.Add(baro_file);
                        }
                    }

                }


            
            ge_calculateVWT.Calculate(  baro_buffer_mins,
                                        atmos_m, 
                                        level_offset,
                                        probe_depth, 
                                        bh_ref, 
                                        dry_depth,
                                        zero_reading,
                                        zero_temp,
                                        linear_factor,
                                        baro_pressure_m,
                                        temp_at_calib,
                                        const_a,
                                        const_b,
                                        const_c,
                                        const_t);
            
             if (save==true) {
                        if (log_file.Id == Guid.Empty) {
                            var log_added = await AddNewFile(log_file);
                            ViewData["fileStatus"] = $"Records created({log_added})";
                        } else {             
                            var log_updated = await UpdateFile(log_file, true);
                            ViewData["fileStatus"] = $"Records updated({log_updated})";
                        }
            }
            
            return View ("ReadData", ge_calculateVWT.log_file);

}
[HttpPost]
 public async Task<IActionResult> createMOND(   Guid Id,
                                                    string table, 
                                                    DateTime? fromDT,
                                                    DateTime? toDT,
                                                    string round_ref,
                                                    string format = "view", 
                                                    Boolean save = false ) 
 {

            if (Id == null)
            {
                return NotFound();
            }
            
            var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            if (_data == null)
            {
                return NotFound();
            }

            var user = GetUserAsync().Result;
            
            ge_data empty_data = new ge_data();

            if (user != null) {
                    int IsDownloadAllowed = _context.IsOperationAllowed(Constants.DownloadOperationName, _data.project, _data);
                    Boolean CanUserDownload = _context.DoesUserHaveOperation(Constants.DownloadOperationName,_data.project,user.Id);
                    
                    int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, _data.project, empty_data);
                    Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName,_data.project,user.Id);

                    if (IsDownloadAllowed!=geOPSResp.Allowed) {
                        return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_PROHIBITED);
                    }
                    
                    if (!CanUserDownload) {
                    return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_USER_PROHIBITED);
                    }

                    if (IsCreateAllowed!=geOPSResp.Allowed) {
                        return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
                    }
                    if (!CanUserCreate) {
                    return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
                    }
            }
            
            var log_file = await GetFile(Id, table);
          
            if (table.Contains("waterquality") || 
                table.Contains("wq") ) {
                
                var resp = await createMOND_WQ(log_file, 
                                            fromDT, 
                                            toDT,
                                            round_ref,
                                            true 
                                            );
            
                if (resp < 0) {
                    return Json ($"There is an issue converting water quality logger data file {_data.filename} to MOND records");
                }
                if (save == true) { 
                    var saveMOND_resp = await new ge_gINTController (_context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config
                                                        ).UploadMOND (_data.projectId, MOND , "ge_source='ge_flow'");
                }
           } 
           
           if (table.Contains("depth") || 
                table.Contains("head") || 
                table.Contains("pressure") || 
                table.Contains("channel") || 
                table.Contains("r0") ||
                table.Contains("r1")
                ) {
                var resp = await createMOND_WDEP(log_file, 
                                            fromDT, 
                                            toDT,
                                            round_ref,
                                            true 
                                            );
            
                if (resp < 0) {
                    return Json ($"There is an issue converting water depth logger data file {_data.filename} to MOND records");
                }

            List<MOND> ordered = MOND.OrderBy(e=>e.DateTime).ToList();
            MOND = ordered;

                if (save == true) { 
                    var saveMOND_resp = await new ge_gINTController (_context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config
                                                        ).UploadMOND (_data.projectId, MOND , "ge_source='ge_logger'");
                }

            }
           
            if (format == "view") {
                return View("ViewMOND", MOND);
            } 

            if (format == "json") {
                return Json(MOND);
            }

            return Ok();
 }

 private async Task<int> createMOND_WQ (ge_log_file log_file, 
                                            DateTime? fromDT,
                                            DateTime? toDT,
                                            string round_ref,
                                            Boolean addWLEV = true ) {


        if (log_file==null) {
            return -1;
        }

        var data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == log_file.dataId);
                
        ge_project project = data.project; 

        // Find borehole in point table of gint database
        string holeId = log_file.getBoreHoleId();
        if (holeId=="") {
            return -1;
        }

        string[] SelectPoint = new string[] {holeId};

        POINT = await new ge_gINTController (_context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config
                                                    ).getPOINT(project.Id, SelectPoint);

        if (POINT == null) {
            return -1;
        }
        POINT pt =  POINT.FirstOrDefault();

        if (pt==null) {
            return -1;
        }

        MONG = await new ge_gINTController (_context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config
                                                    ).getMONG(project.Id, SelectPoint);


        // Find monitoring point in mong table of gint database
        float probe_depth = log_file.getProbeDepth();
        if (probe_depth==0) {
            return -1;
        }

        MONG mg = null;
        List<MONG> PointInstalls = MONG.FindAll(m=>m.PointID==pt.PointID);
        
        string formatMATCH ="{0:00.0}";

       if (PointInstalls.Count==1) {
           mg = PointInstalls.FirstOrDefault();
       } else {
            foreach (MONG m in PointInstalls) {
                if (m.MONG_DIS!=null) {
                    if (String.Format(formatMATCH, m.MONG_DIS.Value) == String.Format(formatMATCH,probe_depth)) {
                        mg = m;
                        break;
                    }
                }
            }
       }

        if (mg==null) {
            return -1;
        }
        
        // Add all readings to new items in List<MOND> 
        MOND = new List<MOND>();
               
        string device_name = log_file.getDeviceName();
        
        float? gl = null;
        
        if (pt.Elevation!=null) {
            gl = Convert.ToSingle(pt.Elevation.Value);
        }

        if (gl==null && pt.LOCA_GL!=null) {
            gl = Convert.ToSingle(pt.LOCA_GL.Value);
        }

        int round_no = Convert.ToInt16(round_ref);
        
        List<ge_log_reading> readings2 = log_file.getIncludeReadings(fromDT,toDT);
        
            foreach (ge_log_reading reading in readings2) {
                
                foreach (value_header vh in log_file.field_headers) {
                    
                    string mond_ref = String.Format("Round {0:00} Seconds {1:00}",round_no,reading.Duration);
                    
                    if (vh.id == "WDEPTH" && vh.units=="m") {
                        // Add MOND WDEP record
                       
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "WDEP", mg.MONG_TYPE + " flow meter reading", mond_ref, vh.db_name, "Water Depth", vh.units,vh.format, null,"ge_flow");
                        MOND.Add (md);
                
                        if (gl!=null && addWLEV==true) {           
                        // Add MOND WLEV record
                        MOND md2 = NewMOND (mg, reading, device_name, round_ref, "WLEV", mg.MONG_TYPE + " flow meter reading", mond_ref, vh.db_name,"Water Level", vh.units, vh.format, gl,"ge_flow");
                        MOND.Add (md2);
                        }
                    }
                    
                    if (vh.id == "PH" ) {
                        // Add MOND Potential Hydrogen
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "PH", mg.MONG_TYPE + " flow meter reading", mond_ref, vh.db_name, "", vh.units, vh.format, null,"ge_flow");
                        MOND.Add (md);
                    }
                    
                    if (vh.id == "DO" && vh.units == "mg/L") {
                        // Add MOND Disolved Oxygen
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "DO", mg.MONG_TYPE + " flow meter reading", mond_ref, vh.db_name, "Dissolved Oxygen", vh.units, vh.format, null,"ge_flow");
                        MOND.Add (md);
                    }
   
                    if (vh.id == "EC" && vh.units == "uS/cm") {
                        // Add MOND Electrical Conductivity 
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "EC", mg.MONG_TYPE + " flow meter reading", mond_ref, vh.db_name, "Electrical Conductivity", vh.units, vh.format, null,"ge_flow");
                        MOND.Add (md);
                    }
                    
                    if (vh.id == "SAL" && vh.units == "g/cm3") {
                        // Add MOND Salinity record 
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "SAL", mg.MONG_TYPE + " flow meter reading",mond_ref,  vh.db_name, "Salinity", vh.units, vh.format, null,"ge_flow");
                        MOND.Add (md);
                    }
                    
                    if (vh.id == "TEMP" && vh.units == "Deg C") {
                        // Add MOND Salinity record 
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "DOWNTEMP", mg.MONG_TYPE + " flow meter reading", mond_ref, vh.db_name, "Downhole Temperature", vh.units, vh.format, null,"ge_flow");
                        MOND.Add (md);
                    }
                    
                    if (vh.id == "RDX" && vh.units == "mV") {
                        // Add MOND Redox Salinity record 
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "RDX", mg.MONG_TYPE + " flow meter reading", mond_ref, vh.db_name, "Redox Potential", vh.units, vh.format, null,"ge_flow");
                        MOND.Add (md);
                    }

                }
            }

        return 0;
}
private async Task<int> createMOND_WDEP (ge_log_file log_file, 
                                            DateTime? fromDT,
                                            DateTime? toDT,
                                            string round_ref,
                                            Boolean addWLEV = true ) {


        if (log_file==null) {
            return -1;
        }

        var data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == log_file.dataId);
                
        ge_project project = data.project; 

        // Find borehole in point table of gint database
        string holeId = log_file.getBoreHoleId();
        if (holeId=="") {
            return -1;
        }

        string[] SelectPoint = new string[] {holeId};

        POINT = await new ge_gINTController (_context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config
                                                    ).getPOINT(project.Id, SelectPoint);

        if (POINT == null) {
            return -1;
        }
        POINT pt =  POINT.FirstOrDefault();

        if (pt==null) {
            return -1;
        }

        MONG = await new ge_gINTController (_context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config
                                                    ).getMONG(project.Id, SelectPoint);


        // Find monitoring point in mong table of gint database
        float probe_depth = log_file.getProbeDepth();
        if (probe_depth==0) {
            return -1;
        }

        MONG mg = null;
        List<MONG> PointInstalls = MONG.FindAll(m=>m.PointID==pt.PointID);
        
        string formatMATCH ="{0:00.0}";

       if (PointInstalls.Count==1) {
           mg = PointInstalls.FirstOrDefault();
       } else {
            foreach (MONG m in PointInstalls) {
                if (m.MONG_DIS!=null) {
                    if (String.Format(formatMATCH, m.MONG_DIS.Value) == String.Format(formatMATCH,probe_depth)) {
                        mg = m;
                        break;
                    }
                }
            }
       }

        if (mg==null) {
            return -1;
        }
        
        // Add all readings to new items in List<MOND> 
        MOND = new List<MOND>();
        value_header  log_wdepth =  log_file.getHeaderByIdUnits(ge_log_constants.WDEPTH,"m");
        
        if (log_wdepth==null) {
            return -1;
        }
       
        string device_name = log_file.getDeviceName();
        
        float? gl = null;
        
        if (pt.Elevation!=null) {
            gl = Convert.ToSingle(pt.Elevation.Value);
        }

        if (gl==null && pt.LOCA_GL!=null) {
            gl = Convert.ToSingle(pt.LOCA_GL.Value);
        }
        
        List<ge_log_reading> readings2 = log_file.getIncludeReadings(fromDT,toDT);
        
        foreach (ge_log_reading reading in readings2) {
            
            if (reading.getValue(log_wdepth.db_name) == null && reading.NotDry!=-1) {
                continue;
            }
            
            // Add MOND WDEP record
            MOND md = NewMOND (mg, reading, device_name, round_ref, "WDEP", mg.MONG_TYPE + " datalogger reading","", log_wdepth.db_name, "Water Depth", "m", DP3, null,"ge_logger");
            MOND.Add (md);
            
            if (gl!=null && addWLEV==true) {           
            // Add MOND WLEV record
            MOND md2 = NewMOND (mg, reading, device_name, round_ref, "WLEV", mg.MONG_TYPE + " datalogger reading","", log_wdepth.db_name,"Water Level", "m", DP3, gl,"ge_logger");
            MOND.Add (md2);
            }

        }

        return 0;
}
private void AddRoundRef(string ROUND_REF) {
    
    if (!String.IsNullOrEmpty(ROUND_REF)) {
            search_item round_ref = new search_item {
                                name = "round_ref",
                                value = ROUND_REF,
                                units ="",
                                comments = "Monitoring Round Reference (MOND_REF)",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                ge_file.AddReplaceSearchItem (round_ref);
            }
}

private MOND NewMOND (MONG mg, ge_log_reading read,
                        string instrument_name, 
                        string round_ref, 
                        string mond_type, 
                        string mond_rem,
                        string mond_ref, 
                        string value_name, 
                        string mond_name, 
                        string units,
                        string format, 
                        float? GL, 
                        string ge_source) {
        
        string value = null; 
        string format2 = "{0:" + format + "}";

        if (read.NotDry==ge_log_constants.ISNOTDRY) {
            float? reading = read.getValue(value_name);
            if (reading!=null) {
                value = String.Format(format2,reading);
                if (mond_type=="WLEV") value = String.Format(format2,GL.Value - reading);
            }
        }

        if (read.NotDry==ge_log_constants.ISDRY) {
            value = "Dry";
        }

        if (!String.IsNullOrEmpty(read.Remark)) {
            mond_rem += " " + read.Remark;
        }
    
        MOND md =  new MOND {
                    gINTProjectID = mg.gINTProjectID,
                    PointID = mg.PointID,
                    ItemKey = mg.ItemKey,
                    MONG_DIS = mg.MONG_DIS,
                    MOND_TYPE = mond_type,
                    MOND_REF = mond_ref,
                    DateTime = read.ReadingDatetime,
                    MOND_UNIT = units,
                    MOND_RDNG = value,
                    MOND_INST = instrument_name,
                    MOND_NAME = mond_name,
                    MOND_REM = mond_rem,
                    RND_REF = round_ref,
                    ge_source = ge_source,
                    ge_otherid = read.Id.ToString()                    
        };

        return md;

}

[HttpPost]
public async Task<IActionResult> ProcessWQ( string[] process) {

    for (int i=0; i<process.Count(); i++) {
  
        string[] line = process[i].Split (",");
        
        Guid Id = Guid.Parse(line[0]);
        Guid templateId = Guid.Parse(line[1]);
        string bh_ref = line[2];
        float probe_depth = Single.Parse(line[3]);
        string round_ref = line[4];

        await ProcessWQ (Id,templateId,bh_ref,probe_depth, round_ref, "");

    }

    return Ok();

}

public async Task<IActionResult> ProcessWQ( Guid Id, Guid? templateId, string bh_ref, float probe_depth, string round_ref, string response) { 
   
    var calc_resp = await CalculateWQ (Id,templateId,"data_waterquality",probe_depth, bh_ref, "",true);
    
    var okResult = calc_resp as OkObjectResult;   
    
    if (okResult == null) { 
        return Json(calc_resp);
    } 

    if (okResult.StatusCode!=200) {
        return Json(calc_resp);
    }

    return await createMOND (Id,"data_waterquality",null,null,round_ref,response,true);
    
}

// public async Task<IActionResult> ProcessWorkbook( Guid Id, Guid? templateId, string table, string bh_ref, float probe_depth, string round_ref, string response) {
//     return await ProcessWorkbook (Id,templateId,table,bh_ref,)
// } 

[HttpPost]
public async Task<IActionResult> ProcessFile( Guid Id, 
                                              Guid templateId, 
                                              string table, 
                                              string bh_ref, 
                                              float probe_depth, 
                                              string round_ref, 
                                              string format = "view", 
                                              Boolean save = false ) { 
   
    var calc_resp = await ReadFileWith (Id,templateId,table, bh_ref, probe_depth, "", true);
    
    var okResult = calc_resp as OkObjectResult;   
    
    if (okResult == null) { 
        return Json(calc_resp);
    } 

    if (okResult.StatusCode!=200) {
        return Json(calc_resp);
    }

    return await createMOND (Id,table, null, null, round_ref, format, save);
    
}

[HttpPost]
public async Task<IActionResult> Calculate(Guid Id,
                                            string log_type, 
                                            Guid? templateId,
                                            string table,
                                            Guid? baroId,
                                            float? level_offset,
                                            int? baro_buffer_mins,
                                            int? atmos_m,
                                            float? probe_depth,
                                            float? dry_depth,
                                            string bh_ref,
                                            string format = "view", 
                                            Boolean save = false) {
    Guid[] baro_Ids = null;

    if (baroId!=null) {
        baro_Ids  = new Guid[] {baroId.Value};
    }

    return  await this.Calculate2 (Id,
                        log_type, 
                        templateId,
                        table,
                        baro_Ids, 
                        level_offset,
                        baro_buffer_mins,
                        atmos_m,
                        probe_depth,
                        dry_depth,
                        bh_ref,
                        format, 
                        save);


}

[HttpPost]
public async Task<IActionResult> Calculate2(Guid Id,
                                            string log_type, 
                                            Guid? templateId,
                                            string table,
                                            Guid[] baroIds,
                                            float? level_offset,
                                            int? baro_buffer_mins,
                                            int? atmos_m,
                                            float? probe_depth,
                                            float? dry_depth,
                                            string bh_ref,
                                            string format = "view", 
                                            Boolean save = false) {

            if (Id == null)
            {
                return NotFound();
            }
            
            var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            if (_data == null)
            {
                return NotFound();
            }

            var user = GetUserAsync().Result;
            
            ge_data empty_data= new ge_data();

            if (user != null) {

                    int IsDownloadAllowed = _context.IsOperationAllowed(Constants.DownloadOperationName, _data.project, _data);
                    Boolean CanUserDownload = _context.DoesUserHaveOperation(Constants.DownloadOperationName,_data.project,user.Id);
                    
                    int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, _data.project, empty_data);
                    Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName,_data.project,user.Id);

                    if (IsDownloadAllowed!=geOPSResp.Allowed) {
                        return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_PROHIBITED);
                    }
                    
                    if (!CanUserDownload) {
                    return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_USER_PROHIBITED);
                    }

                    if (IsCreateAllowed!=geOPSResp.Allowed) {
                        return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
                    }
                    if (!CanUserCreate) {
                    return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
                    }
            }
            
            ge_log_file exist_log_file = await GetFile(Id, table);
            ge_log_file log_file = null;
            
            if (exist_log_file==null && templateId==null) {
            return Json($"A logger file has not been found for data file: {_data.filename} table: {table} please provide templateId to read this data file");
            }

            if (templateId!=null) {
            log_file = await ReadFile (Id, templateId.Value, table);
            } else {
            log_file = exist_log_file;
            }
            
            if (log_file==null) {
                return Json($"The data file: {_data.filename} table: {table}) cannot be read with templateId provided");
            } else {
              if (exist_log_file !=null) {
                 log_file.Id = exist_log_file.Id; 
              }
            }
                        
            log_file.data = _data;

            if (log_type==ge_log_constants.LOG_DIVER || log_type==ge_log_constants.LOG_VWIRE ) {

                _log_calculate calculate = null;
                
                if (log_type==ge_log_constants.LOG_DIVER) {
                    calculate = new ge_log_calculateDiver() ;
                }
            
                if (log_type==ge_log_constants.LOG_VWIRE) {
                    calculate = new ge_log_calculateVWT() ;
                }
            
                calculate.log_file = log_file;
                       
                if (baroIds!=null) {
                    foreach (Guid? baroId in baroIds) {   
                        if (baroId!=null) {
                            var _baro_data = await _context.ge_data
                                        .Include(d =>d.project)
                                        .SingleOrDefaultAsync(m => m.Id == baroId);
                            ge_log_file baro_file = await GetFile(baroId.Value);
                            if (baro_file==null) {
                            return Json($"Baro logger file records has not been found for data file ({_baro_data.filename}), please create baro logger file, before calculating wdepth");   
                            }
                            baro_file.data = _baro_data;
                            calculate.baro_files.Add(baro_file);
                        }
                    }

                }
                        
                calculate.Calculate( baro_buffer_mins,
                                    atmos_m, 
                                    level_offset,
                                    probe_depth, 
                                    bh_ref, 
                                    dry_depth);

            }
            
            if (log_type==ge_log_constants.LOG_WQ) {

                ge_log_calculateWQ  calculate = new ge_log_calculateWQ() ;

                calculate.log_file = log_file;
                
                calculate.Calculate( probe_depth, 
                                        bh_ref);
            
            }

            if (save==true) {
                        if (log_file.Id == Guid.Empty) {
                            var log_added = await AddNewFile(log_file);
                            ViewData["fileStatus"] = $"Records created({log_added})";
                        } else {             
                            var log_updated = await UpdateFile(log_file, true);
                            ViewData["fileStatus"] = $"Records updated({log_updated})";
                        }
            }
            
            if (format=="view") {
            return View ("ReadData", log_file);
            }

            if (format=="json") {
            return Json(log_file);
            }

            return Ok();
    }
[HttpPost]
public async Task<IActionResult> CalculateWQ(Guid Id,
                                            Guid? templateId,
                                            string table,
                                            float? probe_depth,
                                            string bh_ref,
                                            string format = "view", 
                                            Boolean save = false) {

            if (Id == null)
            {
                return NotFound();
            }
            
            var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            if (_data == null)
            {
                return NotFound();
            }

            var user = GetUserAsync().Result;

            if (user == null) {
                return RedirectToPageMessage (msgCODE.USER_NOTFOUND);
            }

            ge_data empty_data= new ge_data();
          

            int IsDownloadAllowed = _context.IsOperationAllowed(Constants.DownloadOperationName, _data.project, _data);
            Boolean CanUserDownload = _context.DoesUserHaveOperation(Constants.DownloadOperationName,_data.project,user.Id);
            
            int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, _data.project, empty_data);
            Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName,_data.project,user.Id);

            if (IsDownloadAllowed!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_PROHIBITED);
            }
            
            if (!CanUserDownload) {
            return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_USER_PROHIBITED);
            }

            if (IsCreateAllowed!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
            }
            if (!CanUserCreate) {
            return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
            }
            
            
            ge_log_file exist_log_file = await GetFile(Id, table);
            ge_log_file log_file = null;
            
            if (exist_log_file==null && templateId==null) {
                return Json( new {status="error", message=$"A logger file has not been found for data file: {_data.filename} table: {table} please provide templateId to read this data file"});
            }

            if (templateId!=null) {
            log_file = await ReadFile (Id, templateId.Value, table);
            } else {
            log_file = exist_log_file;
            }
            
            if (log_file==null) {
                return Json( new {status="error", message=$"The data file: {_data.filename} table: {table}) cannot be read with templateId provided"});
            } else {
              if (exist_log_file !=null) {
                 log_file.Id = exist_log_file.Id; 
              }
            }
                        
            log_file.data = _data;
            
            ge_log_calculateWQ ge_wq = new ge_log_calculateWQ() ;
            
            ge_wq.log_file = log_file;
            
            ge_wq.Calculate(probe_depth, 
                                bh_ref 
                                );
            
            if (save==true) {
                        if (log_file.Id == Guid.Empty) {
                            var log_added = await AddNewFile(ge_wq.log_file);
                            ViewData["fileStatus"] = $"Records created({log_added})";
                        } else {
                        var log_updated = await UpdateFile(ge_wq.log_file, true);
                        ViewData["fileStatus"] = $"Records updated({log_updated})";
                        }
            }
            if (format=="view") {
            return View ("ReadData", ge_wq.log_file);
            }

            if (format=="json") {
            return Json(ge_wq.log_file);
            }
            
            return Ok(ge_wq.log_file);
    }

[HttpPost]
public async Task<IActionResult> CalculateDiver(Guid Id,
                                            Guid? templateId,
                                            string table,
                                            Guid?[] baroIds,
                                            float? level_offset,
                                            int? baro_buffer_mins,
                                            int? atmos_m,
                                            float? probe_depth,
                                            float? dry_depth,
                                            string bh_ref,
                                            string format = "view", 
                                            Boolean save = false) {

            if (Id == null)
            {
                return NotFound();
            }
            
            var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            if (_data == null)
            {
                return NotFound();
            }

            var user = GetUserAsync().Result;

            if (user == null) {
                return RedirectToPageMessage (msgCODE.USER_NOTFOUND);
            }

            ge_data empty_data= new ge_data();
          

            int IsDownloadAllowed = _context.IsOperationAllowed(Constants.DownloadOperationName, _data.project, _data);
            Boolean CanUserDownload = _context.DoesUserHaveOperation(Constants.DownloadOperationName,_data.project,user.Id);
            
            int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, _data.project, empty_data);
            Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName,_data.project,user.Id);

            if (IsDownloadAllowed!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_PROHIBITED);
            }
            
            if (!CanUserDownload) {
            return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_USER_PROHIBITED);
            }

            if (IsCreateAllowed!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
            }
            if (!CanUserCreate) {
            return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
            }
            
            
            ge_log_file exist_log_file = await GetFile(Id, table);
            ge_log_file log_file = null;
            
            if (exist_log_file==null && templateId==null) {
            return Json($"A logger file has not been found for data file: {_data.filename} table: {table} please provide templateId to read this data file");
            }

            if (templateId!=null) {
            log_file = await ReadFile (Id, templateId.Value, table);
            } else {
            log_file = exist_log_file;
            }
            
            if (log_file==null) {
                return Json($"The data file: {_data.filename} table: {table}) cannot be read with templateId provided");
            } else {
              if (exist_log_file !=null) {
                 log_file.Id = exist_log_file.Id; 
              }
            }
                        
            log_file.data = _data;
            
            ge_log_calculateDiver ge_diver = new ge_log_calculateDiver() ;
            
            ge_diver.log_file = log_file;
            
                if (baroIds!=null) {
                    foreach (Guid? baroId in baroIds) {  
                        if (baroId!=null) {
                            var _baro_data = await _context.ge_data
                                        .Include(d =>d.project)
                                        .SingleOrDefaultAsync(m => m.Id == baroId);
                            ge_log_file baro_file = await GetFile(baroId.Value);
                            
                            if (baro_file==null) {
                                return Json($"Baro logger file records has not been found for data file ({_baro_data.filename}), please create baro logger file, before calculating wdepth");   
                            }
                            
                            baro_file.data = _baro_data;
                            ge_diver.baro_files.Add(baro_file);
                        }
                    }

                }

            
           
            ge_diver.Calculate( baro_buffer_mins,
                                atmos_m, 
                                level_offset,
                                probe_depth, 
                                bh_ref, 
                                dry_depth);
            
            if (save==true) {
                        if (log_file.Id == Guid.Empty) {
                            var log_added = await AddNewFile(log_file);
                            ViewData["fileStatus"] = $"Records created({log_added})";
                        } else {             
                            var log_updated = await UpdateFile(log_file, true);
                            ViewData["fileStatus"] = $"Records updated({log_updated})";
                        }
            }
            
            if (format=="view") {
            return View ("ReadData", ge_diver.log_file);
            }

            if (format=="json") {
            return Json(ge_diver.log_file);
            }

            return Ok();
    }


public async Task<IActionResult> View (Guid Id, 
                                       string table,
                                       string format="view") {

            if (Id == null)
            {
                return NotFound();
            }
            var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            if (_data == null)
            {
                return NotFound();
            }
            
            ge_data empty_data = new ge_data();

            var userId = _userManager.GetUserId(User);
            
            if (userId != null) {

                    int IsDownloadAllowed = _context.IsOperationAllowed(Constants.DownloadOperationName, _data.project, _data);
                    Boolean CanUserDownload = _context.DoesUserHaveOperation(Constants.DownloadOperationName,_data.project,userId);
                    
                    int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, _data.project, empty_data);
                    Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName,_data.project,userId);

                    if (IsDownloadAllowed!=geOPSResp.Allowed) {
                        return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_PROHIBITED);
                    }
                    
                    if (!CanUserDownload) {
                    return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_USER_PROHIBITED);
                    }

                    if (IsCreateAllowed!=geOPSResp.Allowed) {
                        return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
                    }
                    if (!CanUserCreate) {
                    return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
                    }
            }

           ge_log_file log_file = await GetFile(Id, table);
           
           if (log_file==null) {
            return Json($"There no associated logger records for {_data.filename}");

           }

           if (format=="json") {
           return Json(log_file);
           }

           return View ("ReadData", log_file);
            
}

private async Task<dbConnectDetails> getConnectDetails (Guid projectId, string dbType) {
            
            if (projectId==null) {
            return  null;
            }
        
            var project = await _context.ge_project
                        .Include(p =>p.group)
                        .FirstOrDefaultAsync(m => m.Id == projectId);

            if (project == null) {
                return  null;
            }

            if (project.otherDbConnectId==null) {
            return  null;
            }

            var cs = await new ge_dataController(  _context,
                                                        _authorizationService,
                                                        _userManager,
                                                        _env ,
                                                        _ge_config).getDataAsClass<OtherDbConnections>(project.otherDbConnectId.Value); 

            if (cs==null) {    
                return null;
            }
     
            dbConnectDetails cd = cs.getConnectType(dbType);

            return cd;
    }

// public Task<ge_log_file> GetFileTask (Guid dataId) {
//         return Task.Run(() =>
//         {
//         return GetFile(dataId);       //YOUR CODE HERE
//         });

// }
private async Task<Guid?> findFirstWhereHeaderContains(List<ge_data> items, string name, string value) {

    foreach (ge_data g in items) {
        ge_log_file glf = await GetFile(g.Id,"", false);
        string value_found = glf.file_headers.Find(s=>s.name==name).value;
        if (value==value_found) {
            return g.Id;
        }
    }
    
    return (Guid?) null;
}
private async Task<Guid?> findFirstWhereHeaderContains(geXML list, string name, string[] value) {

    foreach (data g in list.projects.First().data_list) {
        ge_log_file glf = await GetFile(g.Id,"", false);
        if (glf!=null) {
            string value_found = glf.file_headers.Find(s=>s.name==name).value;
            if (value_found.Length>0) {
                Boolean found = false;
                foreach (string s in value) {
                    if (value_found.Contains(s)) {
                        found=true;
                    } else {
                        found=false;
                        break;
                    }
                }
                if (found){
                    return g.Id;
                }
            }
        }
    }
    
    return (Guid?) null;
}
public async Task<ge_log_file> GetFile (Guid dataId, 
                                        string table = "data_pressure", 
                                        Boolean IncludeReadings=true) {

        if (dataId == null) {
                return null;
        }
        
        var _logger = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == dataId);

        dbConnectDetails cd = await getConnectDetails(_logger.projectId,logTables.DB_DATA_TYPE);
      
        if (cd==null) {
            return null;
        }

        string dbConnectStr = cd.AsConnectionString();
        
        return await Task.Run(() =>
        
        {
                    using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
                    {
                        cnn.Open();
                        dsTable ds_readings = new logTables().reading;
                        dsTable ds_file = new logTables().file;
                        ds_file.setConnection (cnn);        
                        ds_readings.setConnection (cnn);
                        
                        //Multichannel transducer have upto 8 tables which will all have the same dataId

                        if (string.IsNullOrEmpty(table)) {
                        ds_file.sqlWhere("dataId='" + dataId.ToString() + "' and (channel is null or channel='')");
                        } else {
                        ds_file.sqlWhere("dataId='" + dataId.ToString() + "' and channel='" + table + "'" );    
                        }
                        
                        ds_file.getDataSet();
                        DataTable dt_file = ds_file.getDataTable();
    
                        if (dt_file==null) {
                            return null;
                        } 
                        
                        if (dt_file.Rows.Count==0) {
                            return null;
                        }

                        ge_log_file file = new ge_log_file();
                        
                        DataRow row = dt_file.Rows[0];
                        get_log_file_values(row, file);

                       
                        if (IncludeReadings) {
                            ds_readings.sqlWhere("FileId='" + file.Id.ToString() + "'");
                            ds_readings.getDataSet();
                            DataTable dt_readings = ds_readings.getDataTable();
                            file.readings = new List<ge_log_reading>();

                            foreach(DataRow rrow in dt_readings.Rows)
                            {    
                                ge_log_reading r =  new ge_log_reading();
                                get_logger_reading_values(rrow, r);
                                file.readings.Add(r);
                            }  
                        file.OrderReadings();
                        }

                        file.unpackSearchTemplate();
                        file.unpackFieldHeaders();
                        file.unpackFileHeader();
                       
                        return file;
                    }   
            });

}
public async Task<int> UpdateReadings (
                                    DateTime? fromDT,
                                    DateTime? toDT,
                                    string table,
                                    Guid dataId,
                                    int? Valid,
                                    int? Include,
                                    int? pflag,
                                    int? NotDry,
                                    string Remark) {

        int NOT_OK = -1;
        int ret = 0;
        
        if (dataId == null) {
            return NOT_OK;
        }

        var _logger = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == dataId);
        if (_logger == null) {
            return NOT_OK;
        }

        dbConnectDetails cd = await getConnectDetails(_logger.projectId,logTables.DB_DATA_TYPE);
      
        if (cd==null) {
            return NOT_OK;
        }

        ge_log_file log_file =  await GetFile(dataId, table, false);

        if (log_file==null) {
            return NOT_OK;
        }

        string sql_where =  "fileId='" + log_file.Id.ToString() + "'";

        if (fromDT != null) {
        sql_where += $" and ReadingDateTime >= '{String.Format("{0:yyyy-MM-dd HH:mm:ss}'",fromDT)}"; 
        }

        if (toDT != null) {
        sql_where += $" and ReadingDateTime <= '{String.Format("{0:yyyy-MM-dd HH:mm:ss}'",toDT)}"; 
        }

        string dbConnectStr = cd.AsConnectionString();
        return await Task.Run(() =>
        
        {
            using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
            {
                dsTable ds_readings = new logTables().reading;
                cnn.Open();
                ds_readings.setConnection (cnn);
                ds_readings.getDataTable();
                ds_readings.sqlWhere(sql_where);
                ds_readings.getDataSet();
                DataTable dt_readings = ds_readings.getDataTable();
                foreach (DataRow row in dt_readings.Rows) {
                    if (Valid!=null) row["Valid"] = Valid;
                    if (Include!=null) row["Include"] = Include;
                    if (pflag!=null)  row["pflag"] =  pflag;
                    if (NotDry!=null) row["NotDry"] = NotDry;
                    if (Remark!=null)  row["Remark"] = Remark;
                }
                
                ret = ds_readings.Update();
                return ret;  
            }

        });

}
public async Task<int> UpdateChannel (Guid[] Id, Guid dataId, string header, float [] values) {

//value_header vh = Json.Convert<value_header>(header);

        int NOT_OK = -1;
        int ret = 0;
        
        if (dataId == null) {
            return NOT_OK;
        }

        var _logger = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == dataId);
        if (_logger == null) {
            return NOT_OK;
        }

        dbConnectDetails cd = await getConnectDetails(_logger.projectId,logTables.DB_DATA_TYPE);
      
        if (cd==null) {
            return NOT_OK;
        }

        string dbConnectStr = cd.AsConnectionString();
        return await Task.Run(() =>
        
        {
            using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
            {
                dsTable ds_readings = new logTables().reading;
                cnn.Open();
                ds_readings.setConnection (cnn);
                ds_readings.getDataTable();
                ds_readings.sqlWhere("Id='" + Id.ToString() + "'");
                ds_readings.getDataSet();
                DataTable dt_readings = ds_readings.getDataTable();

                DataRow row = dt_readings.Rows[0];
                // if (Valid!=null) row["Valid"] = Valid;
                // if (Include!=null) row["Include"] = Include;
                // if (pflag!=null)  row["pflag"] =  pflag;
                // if (NotDry!=null) row["NotDry"] = NotDry;
                // if (Remark!=null)  row["Remark"] = Remark;
                ret = ds_readings.Update();
                return ret;  
            }

        });

}


public async Task<int> UpdateReading (Guid Id,
                                    Guid dataId,
                                    int? Valid,
                                    int? Include,
                                    int? pflag,
                                    int? NotDry,
                                    string Remark) {

        
        int NOT_OK = -1;
        int ret = 0;
        
        if (dataId == null) {
            return NOT_OK;
        }

        var _logger = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == dataId);
        if (_logger == null) {
            return NOT_OK;
        }

        dbConnectDetails cd = await getConnectDetails(_logger.projectId,logTables.DB_DATA_TYPE);
      
        if (cd==null) {
            return NOT_OK;
        }

        string dbConnectStr = cd.AsConnectionString();
        return await Task.Run(() =>
        
        {
            using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
            {
                dsTable ds_readings = new logTables().reading;
                cnn.Open();
                ds_readings.setConnection (cnn);
                ds_readings.getDataTable();
                ds_readings.sqlWhere("Id='" + Id.ToString() + "'");
                ds_readings.getDataSet();
                DataTable dt_readings = ds_readings.getDataTable();

                DataRow row = dt_readings.Rows[0];
                if (Valid!=null) row["Valid"] = Valid;
                if (Include!=null) row["Include"] = Include;
                if (pflag!=null)  row["pflag"] =  pflag;
                if (NotDry!=null) row["NotDry"] = NotDry;
                if (Remark!=null)  row["Remark"] = Remark;
                ret = ds_readings.Update();
                return ret;  
            }

        });

}



private async Task<int>  UpdateFile (ge_log_file file, Boolean IncludeReadings) {

        int NOT_OK = -1;
        int ret = 0;
        
        if (file.dataId == null) {
                return NOT_OK;
        }
        
        file.packFieldHeaders();
        file.packFileHeader();

        var _logger = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == file.dataId);

        dbConnectDetails cd = await getConnectDetails(_logger.projectId,logTables.DB_DATA_TYPE);
      
        if (cd==null) {
            return NOT_OK;
        }

        string dbConnectStr = cd.AsConnectionString();
        
        return await Task.Run(() =>
        
        {
            using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
            {
                dsTable ds_readings = new logTables().reading;
                dsTable ds_file = new logTables().file;
                cnn.Open();
                ds_file.setConnection (cnn);        
                ds_file.getDataTable ();  
                ds_readings.setConnection (cnn);
                ds_readings.getDataTable();
                ds_file.sqlWhere("Id='" + file.Id + "'");
                ds_file.getDataSet();
                DataTable dt_file = ds_file.getDataTable();

                if (dt_file==null) {
                    return NOT_OK;
                } 
                
                if (dt_file.Rows.Count==0) {
                    return NOT_OK;
                }

                DataRow file_row = dt_file.Rows[0];
                set_log_file_values (file, file_row);
                ret = ds_file.Update();
                
                if (IncludeReadings) {  
                    ds_readings.sqlWhere("FileId='" + file.Id.ToString() + "'");
                    ds_readings.getDataSet();
                    DataTable dt_readings = ds_readings.getDataTable();
                    Boolean checkExisting = false;
                    
                    if (dt_readings.Rows.Count>0) {
                        checkExisting=true;
                    }

                    foreach (ge_log_reading reading in file.readings) {
                        
                        DataRow row = null;
                        if (checkExisting==true) {
                            if (reading.Id != Guid.Empty) {
                            row = dt_readings.Select ($"Id='{reading.Id}'").SingleOrDefault();
                            }

                            if (row==null) {
                            row =  dt_readings.Select ($"ReadingDateTime='{String.Format("{0:yyyy-MM-dd HH:mm:ss.ffff}",reading.ReadingDatetime)}'").SingleOrDefault();
                            }
                        }

                        if (row==null) {
                            row = ds_readings.NewRow();
                            reading.Id = Guid.NewGuid();
                            reading.fileId = file.Id;
                            ds_readings.addRow (row); 
                        } else {
                            reading.Id = (Guid) row["Id"];
                            reading.fileId = file.Id;
                        }
                        //what if there are other records (more) in dt_readings from a previous version of the ge_log_file? 
                        // file.readings.count<dt_readings.count this will still be present
                        set_log_reading_values (reading,row);
                    }


                    ret = ret + ds_readings.Update();
                    return ret;
                } 
            return ret;  
            }
        });

} 
private void get_logger_reading_values(DataRow row, ge_log_reading reading) {

                reading.Id = (Guid) row ["Id"];
                reading.fileId = (Guid) row["fileId"];     
                reading.ReadingDatetime = (DateTime) row["ReadingDateTime"];
                if (row["Duration"] != DBNull.Value) reading.Duration= Convert.ToInt64(row["Duration"].ToString());
                if (row["Value1"] != DBNull.Value) reading.Value1 =Convert.ToSingle( row["Value1"].ToString());
                if (row["Value2"] != DBNull.Value) reading.Value2= Convert.ToSingle(row["Value2"].ToString());
                if (row["Value3"] != DBNull.Value) reading.Value3 =Convert.ToSingle( row["Value3"].ToString());
                if (row["Value4"] != DBNull.Value) reading.Value4 = Convert.ToSingle(row["Value4"].ToString());
                if (row["Value5"] != DBNull.Value)  reading.Value5 =Convert.ToSingle(row["Value5"].ToString());
                if (row["Value6"] != DBNull.Value)  reading.Value6 =Convert.ToSingle(row["Value6"].ToString());
                if (row["Value7"] != DBNull.Value)  reading.Value7 =Convert.ToSingle(row["Value7"].ToString());
                if (row["Value8"] != DBNull.Value) reading.Value8= Convert.ToSingle(row["Value8"].ToString());
                if (row["Value9"] != DBNull.Value) reading.Value9 =Convert.ToSingle( row["Value9"].ToString());
                if (row["Value10"] != DBNull.Value) reading.Value10 = Convert.ToSingle(row["Value10"].ToString());
                if (row["Value11"] != DBNull.Value)  reading.Value11 =Convert.ToSingle(row["Value11"].ToString());
                if (row["Value12"] != DBNull.Value)  reading.Value12 =Convert.ToSingle(row["Value12"].ToString());
                if (row["Value13"] != DBNull.Value)  reading.Value13 =Convert.ToSingle(row["Value13"].ToString());
                if (row["Value14"] != DBNull.Value) reading.Value14 =Convert.ToSingle( row["Value14"].ToString());
                if (row["Value15"] != DBNull.Value) reading.Value15 = Convert.ToSingle(row["Value15"].ToString());
                if (row["Value16"] != DBNull.Value)  reading.Value16 =Convert.ToSingle(row["Value16"].ToString());
                if (row["Value17"] != DBNull.Value)  reading.Value17 =Convert.ToSingle(row["Value17"].ToString());
                if (row["Value18"] != DBNull.Value)  reading.Value18 =Convert.ToSingle(row["Value18"].ToString());
                if (row["Remark"] != DBNull.Value) reading.Remark = (String) row["Remark"]; 
                reading.Valid = (int) row["Valid"];
                reading.Include = (int) row["Include"];
                reading.pflag = (int) row["pflag"];
                reading.NotDry = (int) row["NotDry"];
}
private void set_log_reading_values(ge_log_reading reading, DataRow row) {

                row["Id"] = reading.Id;
                row["fileId"] = reading.fileId;
                row["ReadingDateTime"] = reading.ReadingDatetime;
                if (reading.Duration==null) { row["Duration"] = DBNull.Value;} else {row["Duration"] = reading.Duration;} 
                if (reading.Value1==null) { row["Value1"] = DBNull.Value;} else {row["Value1"] = reading.Value1;} 
                if (reading.Value2==null) { row["Value2"] = DBNull.Value;} else {row["Value2"] = reading.Value2;} 
                if (reading.Value3==null) { row["Value3"] = DBNull.Value;}  else {row["Value3"] = reading.Value3;}
                if (reading.Value4==null) { row["Value4"] = DBNull.Value;} else {row["Value4"] = reading.Value4;} 
                if (reading.Value5==null) { row["Value5"] = DBNull.Value;} else {row["Value5"] = reading.Value5;} 
                if (reading.Value6==null) { row["Value6"] =  DBNull.Value;} else {row["Value6"] =reading.Value6;}
                if (reading.Value7==null) { row["Value7"] = DBNull.Value;} else {row["Value7"] = reading.Value7;} 
                if (reading.Value8==null) { row["Value8"] = DBNull.Value;} else {row["Value8"] = reading.Value8;} 
                if (reading.Value9==null) { row["Value9"] = DBNull.Value;}  else {row["Value9"] = reading.Value9;}
                if (reading.Value10==null) { row["Value10"] = DBNull.Value;} else {row["Value10"] = reading.Value10;} 
                if (reading.Value11==null) { row["Value11"] = DBNull.Value;} else {row["Value11"] = reading.Value11;} 
                if (reading.Value12==null) { row["Value12"] =  DBNull.Value;} else {row["Value12"] =reading.Value12;}
                if (reading.Value13==null) { row["Value13"] = DBNull.Value;} else {row["Value13"] = reading.Value13;} 
                if (reading.Value14==null) { row["Value14"] = DBNull.Value;} else {row["Value14"] = reading.Value14;} 
                if (reading.Value15==null) { row["Value15"] = DBNull.Value;} else {row["Value15"] = reading.Value15;} 
                if (reading.Value16==null) { row["Value16"] =  DBNull.Value;} else {row["Value16"] =reading.Value16;}
                if (reading.Value17==null) { row["Value17"] = DBNull.Value;} else {row["Value17"] = reading.Value17;} 
                if (reading.Value18==null) { row["Value18"] = DBNull.Value;} else {row["Value18"] = reading.Value18;} 
                row["Remark"] = reading.Remark;
                row["Valid"] = reading.Valid;
                row["Include"] = reading.Include;
                row["pflag"] = reading.pflag;
                row["NotDry"] = reading.NotDry;

}
private void get_log_file_values(DataRow row, ge_log_file file) {

                file.Id = (Guid) row["Id"];
                file.dataId = (Guid) row["dataId"]; 
                if (row["ReadingAggregates"] != DBNull.Value) file.readingAggregates = (String) row["ReadingAggregates"]; 
                if (row["FieldHeader"] != DBNull.Value) file.fieldHeader= (String) row["FieldHeader"]; 
                if (row["FileHeader"] != DBNull.Value) file.fileHeader= (String) row["FileHeader"]; 
                if (row["Comments"] != DBNull.Value) file.Comments= (String) row["Comments"]; 
                if (row["channel"] != DBNull.Value) file.channel = (String) row["channel"]; 
                if (row["SearchTemplate"] != DBNull.Value) file.SearchTemplate = (String) row["SearchTemplate"];
                if (row["templateId"] !=DBNull.Value) file.templateId = (Guid) row["templateId"]; 

}

private void set_log_file_values(ge_log_file file, DataRow row) {

                row["Id"] = file.Id;
                row["dataId"] = file.dataId;
                row["fieldHeader"] = file.fieldHeader;
                row["ReadingAggregates"] = file.readingAggregates;
                row["FileHeader"] = file.fileHeader;
                row["Comments"] = file.Comments;
                row["channel"] = file.channel;
                row["templateId"] = file.templateId;
                row["SearchTemplate"] = file.SearchTemplate;

}
private string getAggregates(ge_log_file file) {

    
    aggregate_reading ar = new aggregate_reading();
    ar.count = file.readings.Count();
    ar.maxReadingDate = file.readings.Max(r=>r.ReadingDatetime);
    ar.minReadingDate = file.readings.Min(r=>r.ReadingDatetime);
    
    if (file.getHeader1()!=null) {
        try {
            ar.Value1= new ValueRange {
            max = file.readings.Max(r=>r.Value1.Value),
            min = file.readings.Min(r=>r.Value1.Value),
            } ;
        } catch {};
    }
    if (file.getHeader2()!=null) {
        try {
            ar.Value2 = new ValueRange {
            max = file.readings.Max(r=>r.Value2.Value),
            min = file.readings.Min(r=>r.Value2.Value),
            } ;
        } catch {};
    }
    
    if (file.getHeader3()!=null) {
        try {
            ar.Value3 = new ValueRange {
            max = file.readings.Max(r=>r.Value3.Value),
            min = file.readings.Min(r=>r.Value3.Value),
            };
        } catch {};
    }
    
    if (file.getHeader4()!=null) {
        try {
            ar.Value4 = new ValueRange {
            max = file.readings.Max(r=>r.Value4.Value),
            min = file.readings.Min(r=>r.Value4.Value),
            };
        } catch {};
    }
    
    if (file.getHeader5()!=null) {
        try {
            ar.Value5 = new ValueRange {
            max = file.readings.Max(r=>r.Value5.Value),
            min = file.readings.Min(r=>r.Value5.Value),
            };
        } catch {};
    }
    if (file.getHeader6()!=null) {
        try {
            ar.Value6 = new ValueRange {
            max = file.readings.Max(r=>r.Value6.Value),
            min = file.readings.Min(r=>r.Value6.Value),
            };
        } catch{};
    }
return JsonConvert.SerializeObject(ar);

}

private DateTime? getDateTime(string s, string format) {
    
    string[] formats = { "yyyy h:mm:ss tt", "M/dd/yyyy" };
    
    DateTime dt;
    
    try {
        if (DateTime.TryParseExact(s, format, CultureInfo.CurrentCulture,
                            DateTimeStyles.None, out dt)
                            ) return dt;

        // if (DateTime.TryParseExact(s, formats, CultureInfo.CurrentCulture,
        //                     DateTimeStyles.None, out dt)
        //                     ) return dt;
        dt = DateTime.Parse(s);
        return dt;
    } catch {
    return null;
    }

    }
    private int addReadingsAny(List<ge_log_reading> list, 
                    string[] lines, 
                    int line_start,
                    int line_end, 
                    int intReadTime, 
                    int intDuration, 
                    int intValue1, 
                    int intValue2,
                    int intValue3,
                    int intValue4,
                    int intValue5,
                    int intValue6, 
                    int intValue7, 
                    int intValue8,
                    int intValue9,
                    int intValue10,
                    int intValue11,
                    int intValue12,
                    int intValue13, 
                    int intValue14,
                    int intValue15,
                    int intValue16,
                    int intValue17,
                    int intValue18,
                    int intRemark,
                    int intValueCheckForDry,
                    string dateformat = "") {

    for (int i = line_start; i<line_end; i++) {
                string line = lines[i];
                if (line.Length>0) {
                    string[] values = line.Split(",");
                    
                    if (values[0].Contains("\"")) {
                            values = line.QuoteSplit();
                    }
                    if (values[0] == "") {
                        break;
                    }
                   
                    ge_log_reading r= new ge_log_reading();

                    if (intReadTime != NOT_FOUND) {r.ReadingDatetime = DateTime.Parse(values[intReadTime]);}
                    if (intDuration!= NOT_FOUND) {r.Duration = getDuration(values[intDuration], null);}
                    if (intValue1 != NOT_FOUND) {r.Value1 = getFloat(values[intValue1],null);}
                    if (intValue2 != NOT_FOUND) {r.Value2 = getFloat(values[intValue2],null);}
                    if (intValue3 != NOT_FOUND) {r.Value3 = getFloat(values[intValue3],null);}
                    if (intValue4 != NOT_FOUND) {r.Value4 = getFloat(values[intValue4],null);}
                    if (intValue5!= NOT_FOUND) { r.Value5 = getFloat(values[intValue5],null);}
                    if (intValue6 != NOT_FOUND) {r.Value6 = getFloat(values[intValue6],null);}
                    if (intValue7 != NOT_FOUND) {r.Value7 = getFloat(values[intValue7],null);}
                    if (intValue8 != NOT_FOUND) {r.Value8 = getFloat(values[intValue8],null);}
                    if (intValue9 != NOT_FOUND) {r.Value9 = getFloat(values[intValue9],null);}
                    if (intValue10 != NOT_FOUND) {r.Value10 = getFloat(values[intValue10],null);}
                    if (intValue11 != NOT_FOUND) {r.Value11 = getFloat(values[intValue11],null);}
                    if (intValue12 != NOT_FOUND) {r.Value12 = getFloat(values[intValue12],null);}
                    if (intValue13 != NOT_FOUND) {r.Value13 = getFloat(values[intValue13],null);}
                    if (intValue14 != NOT_FOUND) {r.Value14 = getFloat(values[intValue14],null);}
                    if (intValue15 != NOT_FOUND) {r.Value15 = getFloat(values[intValue15],null);}
                    if (intValue16 != NOT_FOUND) {r.Value16 = getFloat(values[intValue16],null);}
                    if (intValue17 != NOT_FOUND) {r.Value17 = getFloat(values[intValue17],null);}
                    if (intValue18 != NOT_FOUND) {r.Value18 = getFloat(values[intValue18],null);}
                    if (intRemark != NOT_FOUND) {r.Remark = values[intRemark];}
                    if (intValueCheckForDry!=NOT_FOUND) {
                        if (values[intValueCheckForDry] == "Dry" || values[intValueCheckForDry] == "DRY") {
                            r.NotDry = -1;
                        }   
                    }

                    list.Add (r);
                }
    }

    return list.Count();

 }

 private int addReadingsOrdered (List<ge_log_reading> list, 
                    string[] lines, 
                    int line_start,
                    int line_end, 
                    int intReadTime, 
                    int intDuration, 
                    int intValue1, 
                    int intValue2,
                    int intValue3,
                    int intValue4,
                    int intValue5,
                    int intValue6, 
                    int intValue7, 
                    int intValue8,
                    int intValue9,
                    int intValue10,
                    int intValue11,
                    int intValue12,
                    int intValue13, 
                    int intValue14,
                    int intValue15,
                    int intValue16,
                    int intValue17,
                    int intValue18,
                    string dateformat="") {
    
    if (intReadTime>NOT_FOUND && intDuration>NOT_FOUND && 
        intValue1>NOT_FOUND && intValue2>NOT_FOUND && 
        intValue3>NOT_FOUND && intValue4>NOT_FOUND &&
        intValue5>NOT_FOUND && intValue6>NOT_FOUND && 
        intValue7>NOT_FOUND && intValue8>NOT_FOUND &&
        intValue9>NOT_FOUND && intValue10>NOT_FOUND && 
        intValue11>NOT_FOUND && intValue12>NOT_FOUND &&
        intValue13>NOT_FOUND && intValue14>NOT_FOUND) {
        for (int i = line_start; i<line_end; i++) {
            string line = lines[i];
            if (line.Length>0) {
                string[] values = line.Split(",");
                
                if (values[0].Contains("\"")) {
                        values = line.QuoteSplit();
                }
                if (values[0] == "") {
                    break;
                }
                ge_log_reading r= new ge_log_reading();
                r.ReadingDatetime = DateTime.Parse(values[intReadTime]);
                r.Duration = getDuration(values[intDuration],null);
                r.Value1 = Convert.ToSingle(values[intValue1]);
                r.Value2 = Convert.ToSingle(values[intValue2]);
                r.Value3 = Convert.ToSingle(values[intValue3]);
                r.Value4 = Convert.ToSingle(values[intValue4]);
                r.Value5 = Convert.ToSingle(values[intValue5]);
                r.Value6 = Convert.ToSingle(values[intValue6]);
                r.Value7 = Convert.ToSingle(values[intValue7]);
                r.Value8 = Convert.ToSingle(values[intValue8]);
                r.Value9 = Convert.ToSingle(values[intValue9]);
                r.Value10 = Convert.ToSingle(values[intValue10]);
                r.Value11 = Convert.ToSingle(values[intValue11]);
                r.Value12 = Convert.ToSingle(values[intValue12]);
                r.Value13 = Convert.ToSingle(values[intValue13]);
                r.Value14 = Convert.ToSingle(values[intValue14]);
                list.Add (r);
            }
        }
        return list.Count();
    }
    
    if (intReadTime>NOT_FOUND && intDuration>NOT_FOUND && 
        intValue1>NOT_FOUND && intValue2>NOT_FOUND && 
        intValue3>NOT_FOUND && intValue4>NOT_FOUND &&
        intValue5>NOT_FOUND && intValue6>NOT_FOUND) {
        for (int i = line_start; i<line_end; i++) {
            string line = lines[i];
            if (line.Length>0) {
                string[] values = line.Split(",");
                ge_log_reading r= new ge_log_reading();
                r.ReadingDatetime = DateTime.Parse(values[intReadTime]);
                r.Duration = (Int32) Convert.ToSingle(values[intDuration]);
                r.Value1 = Convert.ToSingle(values[intValue1]);
                r.Value2 = Convert.ToSingle(values[intValue2]);
                r.Value3 = Convert.ToSingle(values[intValue3]);
                r.Value4 = Convert.ToSingle(values[intValue4]);
                r.Value5 = Convert.ToSingle(values[intValue5]);
                r.Value6 = Convert.ToSingle(values[intValue6]);
                list.Add (r);
            }
        }
        return list.Count();
    }
    if (intReadTime>NOT_FOUND && intDuration>NOT_FOUND && 
        intValue1>NOT_FOUND && intValue2>NOT_FOUND && 
        intValue3>NOT_FOUND && intValue4>NOT_FOUND &&
        intValue5>NOT_FOUND)  {
        for (int i = line_start; i<line_end; i++) {
            string line = lines[i];
            if (line.Length>0) {
                string[] values = line.Split(",");
                ge_log_reading r= new ge_log_reading();
                r.ReadingDatetime = DateTime.Parse(values[intReadTime]);
                r.Duration = (Int32) Convert.ToSingle(values[intDuration]);
                r.Value1 = Convert.ToSingle(values[intValue1]);
                r.Value2 = Convert.ToSingle(values[intValue2]);
                r.Value3 = Convert.ToSingle(values[intValue3]);
                r.Value4 = Convert.ToSingle(values[intValue4]);
                r.Value5 = Convert.ToSingle(values[intValue5]);
                list.Add (r);
            }
        }
        return list.Count();
    }
    if (intReadTime>NOT_FOUND && intDuration>NOT_FOUND && 
        intValue1>NOT_FOUND && intValue2>NOT_FOUND && 
        intValue3>NOT_FOUND && intValue4>NOT_FOUND)  {
        for (int i = line_start; i<line_end; i++) {
            string line = lines[i];
            if (line.Length>0) {
                string[] values = line.Split(",");
                ge_log_reading r= new ge_log_reading();
                r.ReadingDatetime = DateTime.Parse(values[intReadTime]);
                r.Duration = (Int32) Convert.ToSingle(values[intDuration]);
                r.Value1 = Convert.ToSingle(values[intValue1]);
                r.Value2 = Convert.ToSingle(values[intValue2]);
                r.Value3 = Convert.ToSingle(values[intValue3]);
                r.Value4 = Convert.ToSingle(values[intValue4]);
                list.Add (r);
            }
        }
        return list.Count();
    }
    
    if (intReadTime>NOT_FOUND && intDuration>NOT_FOUND && 
        intValue1>NOT_FOUND && intValue2>NOT_FOUND && 
        intValue3>NOT_FOUND)  {
        for (int i = line_start; i<line_end; i++) {
            string line = lines[i];
            if (line.Length>0) {
                string[] values = line.Split(",");
                ge_log_reading r= new ge_log_reading();
                r.ReadingDatetime = DateTime.Parse(values[intReadTime]);
                r.Duration = (Int32) Convert.ToSingle(values[intDuration]);
                r.Value1 = Convert.ToSingle(values[intValue1]);
                r.Value2 = Convert.ToSingle(values[intValue2]);
                r.Value3 = Convert.ToSingle(values[intValue3]);
                list.Add (r);
            }
        }
        return list.Count();
    }
    if (intReadTime>NOT_FOUND && intDuration>NOT_FOUND && 
        intValue1>NOT_FOUND && intValue2>NOT_FOUND) {
        for (int i = line_start; i<line_end; i++) {
            string line = lines[i];
            if (line.Length>0) {
                string[] values = line.Split(",");
                ge_log_reading r= new ge_log_reading();
               r.ReadingDatetime = DateTime.Parse(values[intReadTime]);
                r.Duration = (Int32) Convert.ToSingle(values[intDuration]);
                r.Value1 = Convert.ToSingle(values[intValue1]);
                r.Value2 = Convert.ToSingle(values[intValue2]);
                list.Add (r);
            }
        }
        return list.Count();
    }
    if (intReadTime>NOT_FOUND && intDuration>NOT_FOUND && 
        intValue1>NOT_FOUND) {
        for (int i = line_start; i<line_end; i++) {
            string line = lines[i];
            if (line.Length>0) {
                string[] values = line.Split(",");
                ge_log_reading r= new ge_log_reading();
                r.ReadingDatetime = DateTime.Parse(values[intReadTime]);
                r.Duration = (Int32) Convert.ToSingle(values[intDuration]);
                r.Value1 = Convert.ToSingle(values[intValue1]);
                list.Add (r);
            }
        }
        return list.Count();
    }
    if (intReadTime > NOT_FOUND && intValue1 > NOT_FOUND && 
        intValue2 > NOT_FOUND && intValue3 > NOT_FOUND && 
        intValue4 > NOT_FOUND && intValue5 > NOT_FOUND &&
        intValue6 > NOT_FOUND) {
            for (int i = line_start; i<line_end; i++) {
                string line = lines[i];
                if (line.Length>0) {
                    string[] values = line.Split(",");
                    ge_log_reading r= new ge_log_reading();
                    r.ReadingDatetime = DateTime.Parse(values[intReadTime]);
                    if(!values[intValue1].IsFloat() ||
                        !values[intValue2].IsFloat() || 
                        !values[intValue3].IsFloat() || 
                        !values[intValue4].IsFloat() || 
                        !values[intValue5].IsFloat()|| 
                        !values[intValue6].IsFloat()) {
                        continue;
                    }
                    r.Value1 = Convert.ToSingle(values[intValue1]);
                    r.Value2 = Convert.ToSingle(values[intValue2]);
                    r.Value3 = Convert.ToSingle(values[intValue3]);
                    r.Value4 = Convert.ToSingle(values[intValue4]); 
                    r.Value5 = Convert.ToSingle(values[intValue5]);
                    r.Value6 = Convert.ToSingle(values[intValue6]);                         
                    list.Add (r);
                }
            }
            return list.Count();
    }
    if (intReadTime > NOT_FOUND && intValue1 > NOT_FOUND && 
        intValue2 > NOT_FOUND && intValue3 > NOT_FOUND && 
        intValue4 > NOT_FOUND && intValue5 > NOT_FOUND) {
            for (int i = line_start; i<line_end; i++) {
                string line = lines[i];
                if (line.Length>0) {
                    string[] values = line.Split(",");
                    ge_log_reading r= new ge_log_reading();
                    r.ReadingDatetime = DateTime.Parse(values[intReadTime]);
                    if(!values[intValue1].IsFloat() ||
                        !values[intValue2].IsFloat() || 
                        !values[intValue3].IsFloat() || 
                        !values[intValue4].IsFloat() || 
                        !values[intValue5].IsFloat()) {
                        continue;
                    }
                    r.Value1 = Convert.ToSingle(values[intValue1]);
                    r.Value2 = Convert.ToSingle(values[intValue2]);
                    r.Value3 = Convert.ToSingle(values[intValue3]);
                    r.Value4 = Convert.ToSingle(values[intValue4]); 
                    r.Value5 = Convert.ToSingle(values[intValue5]);                     
                    list.Add (r);
                }
            }
            return list.Count();
    }

    if (intReadTime>NOT_FOUND && intValue1>NOT_FOUND && 
            intValue2>NOT_FOUND && intValue3>NOT_FOUND &&
            intValue4>NOT_FOUND) {
            for (int i = line_start; i<line_end; i++) {
                string line = lines[i];
                if (line.Length>0) {
                    string[] values = line.Split(",");
                    ge_log_reading r= new ge_log_reading();
                    r.ReadingDatetime = DateTime.Parse(values[intReadTime]);
                     if(!values[intValue1].IsFloat() ||
                        !values[intValue2].IsFloat() || 
                        !values[intValue3].IsFloat() || 
                        !values[intValue4].IsFloat()) {
                        continue;
                    }
                    r.Value1 = Convert.ToSingle(values[intValue1]);
                    r.Value2 = Convert.ToSingle(values[intValue2]);
                    r.Value3 = Convert.ToSingle(values[intValue3]);
                    r.Value4 = Convert.ToSingle(values[intValue4]);                      
                    list.Add (r);
                }
            }
            return list.Count();
    }

    if (intReadTime>NOT_FOUND && intValue1>NOT_FOUND && 
            intValue2>NOT_FOUND && intValue3>NOT_FOUND) {
            for (int i = line_start; i<line_end; i++) {
                string line = lines[i];
                if (line.Length>0) {
                    string[] values = line.Split(",");
                    ge_log_reading r= new ge_log_reading();
                    r.ReadingDatetime = DateTime.Parse(values[intReadTime]); 
                    if (!values[intValue1].IsFloat() ||
                        !values[intValue2].IsFloat() || 
                        !values[intValue3].IsFloat()){
                        continue;
                    }
                    r.Value1 = Convert.ToSingle(values[intValue1]);
                    r.Value2 = Convert.ToSingle(values[intValue2]);
                    r.Value3 = Convert.ToSingle(values[intValue3]);  
                    list.Add (r);
                    Console.Write (i);
                }
            }
            return list.Count();
    }
    
    if (intReadTime>NOT_FOUND && intValue1>NOT_FOUND && 
            intValue2>NOT_FOUND) {
            for (int i = line_start; i<line_end; i++) {
                string line = lines[i];

                if (line.Length>0 ) {
                    if (READ_STOPS.Contains(line)){
                        break;
                    }
                    string[] values = line.Split(",");
                    
                    if (values[0].Contains("\"")) {
                        values = line.QuoteSplit();
                    }
                   
                    ge_log_reading r = new ge_log_reading();
                    if (dateformat=="") { 
                        r.ReadingDatetime = DateTime.Parse(values[intReadTime]);
                    } else {
                        r.ReadingDatetime = DateTime.ParseExact(values[intReadTime], dateformat, CultureInfo.CurrentCulture,DateTimeStyles.AllowInnerWhite);
                    }
                    if (!values[intValue1].IsFloat() || 
                        !values[intValue2].IsFloat()) {
                        continue;
                    }
                    r.Value1 = Convert.ToSingle(values[intValue1]);
                    r.Value2 = Convert.ToSingle(values[intValue2]);                    
                    list.Add (r);
                }
            }
            return list.Count();
     }

    if (intReadTime>NOT_FOUND && intValue1>NOT_FOUND) {
            for (int i = line_start; i<line_end; i++) {
                string line = lines[i];
                if (line.Length>0) {
                    
                    if (READ_STOPS.Contains(line)){
                        break;
                    }

                    string[] values = line.Split(",");
                    
                    if (values[0].Contains("\"")) {
                        values = line.QuoteSplit();
                    }
                    
                    ge_log_reading r= new ge_log_reading();
                    if (dateformat=="") { 
                        r.ReadingDatetime = DateTime.Parse(values[intReadTime]);
                    } else {
                        r.ReadingDatetime = DateTime.ParseExact(values[intReadTime], dateformat, CultureInfo.CurrentCulture,DateTimeStyles.AllowInnerWhite);
                    }
                     if (!values[intValue1].IsFloat()){
                        continue;
                    }
                    r.Value1 = Convert.ToSingle(values[intValue1]);
                    list.Add (r);
                }
            }
            return list.Count();
        }

    return 0;
 }

 private float? getFloat(string s1, float? retOnError) {
     float? fl;
     try {
         fl = Convert.ToSingle(s1);
         return fl;
     } catch {
         return retOnError;
     }


 }
 private long? getDuration(string duration, long? retIfError) {
    Int64 dur  = 0 ;
    try {
        TimeSpan ts = TimeSpan.Parse(duration);
        dur = Convert.ToInt64(ts.TotalSeconds);
        return dur;
    } catch {
        try {
            dur = (Int64) Convert.ToSingle(duration);
            return dur;
        } catch {
            return retIfError;
        }
    }
  }
  private Boolean IsTableStandardFormat(search_table st) {


                int max_id = -1;

                foreach (value_header vh in st.headers) {
                    if (max_id < vh.found) {
                        max_id = vh.found;
                    }

                }

                if (max_id==st.headers.Count) {
                    return true;
                } 
                return false;
}


   private ge_log_file getNewLoggerFile(ge_search dic, string[] lines) {
    
       
        ge_log_file file = new ge_log_file();
        file.search_template = dic;
        
        file.file_headers = dic.search_items;
        file.file_array = dic.array_items;
        
        search_table st = dic.search_tables.FirstOrDefault();
        file.search_table = st; 
        file.field_headers = st.headers;
        file.channel = st.name;
        
        value_header DateTimeReading = dic.getHeader(ge_log_constants.READINGDATETIME);
        int intReadTime = NOT_FOUND;
        if (DateTimeReading != null) {
            intReadTime = DateTimeReading.found;
        }

        value_header Duration = dic.getHeader(ge_log_constants.DURATION);
        int intDuration = NOT_FOUND;
        if (Duration!=null) {
            intDuration = Duration.found;    
        }

        value_header Header1 = dic.getHeader(ge_log_constants.VALUE1);
        int intValue1 = NOT_FOUND;
        if (Header1!=null) {
            intValue1 = Header1.found;
        }
        
        value_header Header2 = dic.getHeader(ge_log_constants.VALUE2); 
        int intValue2 = NOT_FOUND;
        if (Header2!=null) {
            intValue2 = Header2.found;
        }

        value_header Header3 = dic.getHeader(ge_log_constants.VALUE3);
        int intValue3 = NOT_FOUND;
        if (Header3!=null) {
            intValue3 = Header3.found;
        }

        value_header Header4 = dic.getHeader(ge_log_constants.VALUE4);
        int intValue4 = NOT_FOUND;
        if (Header4!=null) {
            intValue4 = Header4.found;
        }

        value_header Header5 = dic.getHeader(ge_log_constants.VALUE5);
        int intValue5 = NOT_FOUND;
        if (Header5!=null) {
            intValue5 = Header5.found;
        }

        value_header Header6 = dic.getHeader(ge_log_constants.VALUE6);
        int intValue6 = NOT_FOUND;
        if (Header6!=null) {
            intValue6 = Header6.found;
        }

        value_header Header7 = dic.getHeader(ge_log_constants.VALUE7);
        int intValue7 = NOT_FOUND;
        if (Header7!=null) {
            intValue7 = Header7.found;
        }

        value_header Header8 = dic.getHeader(ge_log_constants.VALUE8);
        int intValue8 = NOT_FOUND;
        if (Header8!=null) {
            intValue8 = Header8.found;
        }

        value_header Header9 = dic.getHeader(ge_log_constants.VALUE9);
        int intValue9 = NOT_FOUND;
        if (Header9!=null) {
            intValue9 = Header9.found;
        }
        
        value_header Header10 = dic.getHeader(ge_log_constants.VALUE10);       
        int intValue10 = NOT_FOUND;
        if (Header10!=null) {
            intValue10 = Header10.found;
        }

        value_header Header11 = dic.getHeader(ge_log_constants.VALUE11);
        int intValue11= NOT_FOUND;
        if (Header11!=null) {
            intValue11 = Header11.found;
        }

        value_header Header12 = dic.getHeader(ge_log_constants.VALUE12);
        int intValue12 = NOT_FOUND;
        if (Header12!=null) {
            intValue12 = Header12.found;
        }
        
        value_header Header13 = dic.getHeader(ge_log_constants.VALUE13);
        int intValue13 = NOT_FOUND;
        if (Header13!=null) {
            intValue13 = Header13.found;
        }

        value_header Header14 = dic.getHeader(ge_log_constants.VALUE14); 
        int intValue14 = NOT_FOUND;
        if (Header14!=null) {
            intValue14 = Header14.found;
        }
        
        value_header Header15 =dic.getHeader(ge_log_constants.VALUE15);
        int intValue15 = NOT_FOUND;
        if (Header15!=null) {
            intValue15 = Header15.found;
        }

        value_header Header16 =dic.getHeader(ge_log_constants.VALUE16);
        int intValue16= NOT_FOUND;
        if (Header16!=null) {
            intValue16 = Header16.found;
        }

        value_header Header17 = dic.getHeader(ge_log_constants.VALUE17);
        int intValue17 = NOT_FOUND;
        if (Header17!=null) {
            intValue17 = Header17.found;
        }

        value_header Header18 = dic.getHeader(ge_log_constants.VALUE18);
        int intValue18 = NOT_FOUND;
        if (Header18!=null) {
            intValue18 = Header18.found;
        }
        value_header HeaderRemark = dic.getHeader(ge_log_constants.REMARK);
        int intRemark = NOT_FOUND;
        if (HeaderRemark!=null) {
            intRemark = HeaderRemark.found;
        }

        value_header log_wdepthM = file.getHeaderByIdUnits(ge_log_constants.WDEPTH,"m");
        int intCheckValueForDry = NOT_FOUND;
        if (log_wdepthM!=null) {
            intCheckValueForDry = log_wdepthM.found;   
        }

        file.readings =  new List<ge_log_reading>();
        
        int line_start = find_row(dic.search_items,"data_start",NOT_FOUND);
        
        if (line_start==NOT_FOUND) {
            line_start = find_row(dic.search_items,"header",NOT_FOUND); 
        }
        
        if (line_start==NOT_FOUND) {
            line_start = find_row(dic.search_items,st.header.search_item,NOT_FOUND); 
        }
        
        line_start = line_start + 1;   
        
        int line_end = find_row(dic.search_items,"data_end",NOT_FOUND);

        if (line_end!=NOT_FOUND) {
            line_end= line_end - 1;
        } else {
            line_end = lines.Count();
        }
        

        int readlines = addReadingsAny(file.readings, 
                                    lines, 
                                    line_start, 
                                    line_end,
                                    intReadTime,
                                    intDuration, 
                                    intValue1, 
                                    intValue2, 
                                    intValue3,
                                    intValue4,
                                    intValue5,
                                    intValue6,
                                    intValue7, 
                                    intValue8, 
                                    intValue9,
                                    intValue10,
                                    intValue11,
                                    intValue12,
                                    intValue13, 
                                    intValue14, 
                                    intValue15,
                                    intValue16,
                                    intValue17,
                                    intValue18, 
                                    intRemark,
                                    intCheckValueForDry                             
                                    );
        if (readlines <= 0) {
            return null;
        }

        file.init_new_file();

    return file;
    
 }
 private int find_row(List<search_item> list, string name, int retIfNotFound) {
    
    search_item si = list.Find(e=>e.name==name);
    
    if (si==null) {
        return retIfNotFound;
    }
    return si.row;
 }

private async Task<int> DeleteFile(Guid dataId, string channel = "") {

        int NOT_OK = -1;
               
        if (dataId == null) {
                return NOT_OK;
        }
        
        var _logger = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == dataId);

        dbConnectDetails cd = await getConnectDetails(_logger.projectId,logTables.DB_DATA_TYPE);
      
        if (cd==null) {
            return NOT_OK;
        }

        string dbConnectStr = cd.AsConnectionString();
        return await Task.Run(() =>
        
        {
            using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
            {
                dsTable ds_readings = new logTables().reading;
                dsTable ds_file = new logTables().file;
                cnn.Open();
                ds_file.setConnection (cnn);        
                ds_file.getDataTable ();  
                ds_readings.setConnection (cnn);
                ds_readings.getDataTable();

                if (channel == "") {
                ds_file.sqlWhere("DataId='" + dataId.ToString() + "' and (channel is null or channel='')");
                } else {
                ds_file.sqlWhere("DataId='" + dataId.ToString() + "' and channel='" + channel + "'");
                }

                ds_file.getDataSet();
                return ds_file.Delete();
            }
        });
}    



private async Task<int> AddNewFile(ge_log_file file) {
    
        int NOT_OK = -1;
        int ret = 0;
        
        if (file == null) {
                return NOT_OK;
        }
        
        file.packFieldHeaders();
        file.packFileHeader();
        
        var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == file.dataId);

        dbConnectDetails cd = await getConnectDetails(_data.projectId,logTables.DB_DATA_TYPE);
      
        if (cd==null) {
            return NOT_OK;
        }

        string dbConnectStr = cd.AsConnectionString();

        return await Task.Run(() =>
            {
                using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
                {
                    dsTable ds_readings = new logTables().reading;
                    dsTable ds_file = new logTables().file;
                    cnn.Open();
                    ds_file.setConnection (cnn);  
                    ds_file.Reset(); 
                           
                    ds_readings.setConnection (cnn);
                    ds_readings.Reset();

                    DataTable dt_file = ds_file.getDataTable();
                    DataRow file_row = dt_file.NewRow();
                    
                    file.Id = Guid.NewGuid();
                    set_log_file_values (file, file_row);
                    ds_file.addRow (file_row);

                    ret = ds_file.Update();
                    
                    DataTable dt_readings = ds_readings.getDataTable();
                    
                    foreach (ge_log_reading reading in file.readings) {
                        DataRow row = dt_readings.NewRow();
                        reading.Id = Guid.NewGuid();
                        reading.fileId = file.Id;
                        set_log_reading_values (reading, row);
                        ds_readings.addRow (row);
                    }

                    ret = ret + ds_readings.Update();
                    return ret;

                }
            });
}
    }
}


