using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ge_repository.Models;
using ge_repository.interfaces;
using ge_repository.Authorization;
using ge_repository.services;
using ge_repository.repositories;
namespace ge_repository.OtherDatabase  { 

// https://blog.hildenco.com/2018/12/accessing-entity-framework-context-on.html

public class ge_log_client {

    public Guid Id {get;set;}
    public ge_data data_file {get;set;}
    public ge_log_file log_file {get;set;}
    public Guid? templateId {get;set;}
    public ge_search template {get;set;}
    public ge_search template_loaded {get;set;}
    public string[] lines {get;set;}
    public enumStatus status {get;set;}
    public IDataService _dataService {get;set;}
    public ILoggerFileService _logService {get;set;}
    public IGintTableService2<MONG, MOND> _gintService {get;set;}
    private ge_DbContext _context {get;}
    public string table {get;set;}
    public string sheet {get;set;}
    public string bh_ref  {get;set;}
    public float? probe_depth  {get;set;}
    public string round_ref {get;set;}  

    public DateTime? fromDT {get;set;}
    public DateTime? toDT {get;set;}
    public string userId {get;set;}
    private static int NOT_FOUND = -1;
    private static int OK = 1;

    public Boolean read_logger {get;set;} = true;
    public Boolean save_logger {get;set;} = true;
    public Boolean apply_overrides {get;set;} = true;

    public Boolean save_MOND {get;set;}= false;

    public ge_log_client (IDataService dataService, ILoggerFileService logService, IGintTableService2<MONG,MOND> gintservice) {
        _dataService = dataService;
        _logService = logService;
        _gintService = gintservice;
    }
    public ge_log_client (IServiceScopeFactory serviceScopeFactory, dbConnectDetails connectLogger, dbConnectDetails connectGint) {

            var scope = serviceScopeFactory.CreateScope() ;

            _context = scope.ServiceProvider.GetService<ge_DbContext>();

            IUnitOfWork _unit = new UnitOfWork(_context); 
            _dataService = new DataService(_unit);
           
            IGintUnitOfWork _gunit = new GintUnitOfWork(connectGint);
            _gintService = new MONDService (_gunit);
            
            ILoggerFileUnitOfWork _lunit = new LogUnitOfWork(connectLogger);
            _logService = new LoggerFileService (_lunit);
    }
    public enum enumStatus
        {  
            Start,
            DataFileLoaded,
            TemplateFileLoaded,
            TemplateAndLinesRead,
            LoggerFileSaved,
            LoggerFileLoaded,
            MONDcreated,
            MONDupdated,
            Stop
        }


    public async Task<ge_log_client.enumStatus> start() {

            status = enumStatus.Start;
            
            await _dataService.SetProcessFlag(Id,pflagCODE.PROCESSING);

            while (status != enumStatus.Stop) {
                
                if (status == enumStatus.Start) {
                    int resp =  await loadDataFile();
                    if (resp == OK) status = enumStatus.DataFileLoaded;
                }
                
                if (status == enumStatus.DataFileLoaded && read_logger==true ) {
                    int resp = await loadTemplateFile();
                    if (resp == OK) status = enumStatus.TemplateFileLoaded;
                }

                if (status == enumStatus.TemplateFileLoaded && read_logger==true) {
                    int resp =  await loadTemplateAndLines();
                    if (resp == OK) status = enumStatus.TemplateAndLinesRead;
                }

                if (status == enumStatus.TemplateAndLinesRead && read_logger==true ) {
                    int resp = createFileLogger();
                    if (resp == OK) status = enumStatus.LoggerFileLoaded;
                }
                
                if (status == enumStatus.LoggerFileLoaded && save_logger==true) {
                    int resp = await saveFileLogger();
                    if (resp == OK) status = enumStatus.LoggerFileSaved;
                }

                if (status == enumStatus.DataFileLoaded && read_logger==false) {
                    int resp = await loadFileLogger();
                    if (resp == OK) status = enumStatus.LoggerFileLoaded;
                }
                               
                if (status == enumStatus.LoggerFileLoaded || status == enumStatus.LoggerFileSaved) {
                    int resp = await createMOND();
                    if (resp == OK) status = enumStatus.MONDcreated;
                }

                if (status == enumStatus.MONDcreated) {
                    status = enumStatus.Stop;
                }
        
        }
        
        await _dataService.SetProcessFlag(Id,pflagCODE.NORMAL);
        
        return status;
    
 }
    


    
    private async Task<int> loadTemplateAndLines () {

        // string[] lines = null;
        // ge_search template_loaded = null;
        
        if (data_file.fileext == ".csv") {
                lines = await _dataService.GetFileAsLines(data_file.Id);
                SearchTerms st = new SearchTerms();
                template_loaded = st.findSearchTerms(template,table, lines);
        }

        if (data_file.fileext == ".xlsx") {
            using (MemoryStream ms = await _dataService.GetFileAsMemoryStream(data_file.Id)) {
                ge_log_workbook wb = new ge_log_workbook(ms);
                SearchTerms st = new SearchTerms();  
                if (sheet.Contains(",")) {
                string[] sheets = sheet.Split (",");
                template_loaded =  st.findSearchTerms (template, table, wb, sheets);  
                } else {
                template_loaded  =  st.findSearchTerms (template, table, wb, sheet);
                }
                wb.setWorksheet(template_loaded.search_tables[0].sheet);
                lines = wb.WorksheetToTable();
                wb.close();
            }
        }
        
        return 1;

  }
    
    
    private async Task<int> loadDataFile() {
            
            if (Id == null) {
            return -1;
            }
           
           data_file = await _dataService.GetDataById(Id);
           
           return 1; 
    
    
    }
    private async Task<int> loadTemplateFile() {
        
        if (templateId == null) {
            return -1;
        }

        template = await _dataService.GetFileAsClass<ge_search>(templateId.Value);
           
        return 1; 
    
    }
    private async Task<int> saveFileLogger() {

        ge_log_file exist = await _logService.GetByDataId(log_file.Id, table);
    
        if (exist != null) {
            await _logService.UpdateLogFile(exist, log_file);
        } else {
            await _logService.CreateLogFile (log_file);
        }

        return 1;
    }

      
    private int createFileLogger() {

            log_file = _logService.CreateLogFile( template_loaded,
                                                    lines,
                                                    Id,
                                                    templateId.Value);
            if (apply_overrides==true) {
            ge_log_helper gf = new ge_log_helper();
            gf.log_file = log_file;
            gf.AddOverrides (probe_depth, bh_ref);
            }

            return 1;

    }

    private async Task<int> loadFileLogger() {

            log_file = await _logService.GetByDataId(Id, table);
           
            if (apply_overrides==true) {
                ge_log_helper gf = new ge_log_helper();
                gf.log_file = log_file;
                gf.AddOverrides (probe_depth, bh_ref);
            }

            return 1;
  
    }

    private async Task<int> createMOND() 
    {

            string ge_source = "";

            if (table.Contains("waterquality") || 
                table.Contains("wq") ) {
                ge_source = "ge_flow";
            }
            if (table.Contains("depth") || 
                table.Contains("head") || 
                table.Contains("pressure") || 
                table.Contains("channel") || 
                table.Contains("r0") ||
                table.Contains("r1")
                ) {
                ge_source = "ge_logger";
            }

            int page_size = 1000;
            int row_count = log_file.getIncludeReadings(fromDT, toDT).Count() ;
            int total_pages = Convert.ToInt32(row_count / page_size) + 1;
            
            List<MOND> ordered =  new List<MOND>();
            
            for (int page = 1; page <= total_pages; page++) {

                    
                List<MOND> MOND = await createMOND( page_size,
                                    page,
                                    ge_source,
                                    true);
                
               if (save_MOND == true) { 

                    string where2 = $"ge_source='{ge_source}'"; 
                    await _gintService.UpdateRange(MOND, where2);
                            
                }
                
                ordered.AddRange(MOND.OrderBy(e=>e.DateTime).ToList());

            }

        return 1;
    }
    private async Task<List<MOND>> createMOND ( int page_size,
                                            int page,
                                            string ge_source="ge_flow",
                                            Boolean addWLEV = true) {


        // Find borehole in point table of gint database
        
        string holeId = log_file.getBoreHoleId();

        if (holeId=="") {
            return null; // BadRequest ($"Borehole ref not provided");
        }

        POINT pt = await _gintService.GetPointByHoleId(holeId);

        if (pt == null) {
            return null;//         return BadRequest ($"Borehole ref {holeId} not found in {project.name}");
        }                
    
        List<MONG> mgs = await _gintService.GetParentsByHoleId(holeId);

        
         // Find monitoring point in mong table of gint database
        float probe_depth = log_file.getProbeDepth();
        
        if (probe_depth==0) {
             return null; // return BadRequest ($"No probe depth provided for borehole ref {holeId} not found in {project.name}"); 
        }


        MONG mg = null;

        List<MONG> PointInstalls = mgs.FindAll(m=>m.PointID==pt.PointID);
        
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
            return null; // return BadRequest ($"No installations in borehole ref {holeId} have a probe depth of {probe_depth} in {project.name}"); 
        }
        
        // Add all readings to new items in List<MOND> 
        List<MOND> MOND = new List<MOND>();
               
        string device_name = log_file.getDeviceName();
        
        float? gl = null;
        
        if (pt.Elevation!=null) {
            gl = Convert.ToSingle(pt.Elevation.Value);
        }

        if (gl==null && pt.LOCA_GL!=null) {
            gl = Convert.ToSingle(pt.LOCA_GL.Value);
        }

        int round_no = Convert.ToInt16(round_ref);
        
        string mond_rem_suffix = "";
        string mond_ref = "";

        if (ge_source =="ge_flow") {
            mond_rem_suffix = " flow meter reading";
        }

        if (ge_source =="ge_logger") {
            mond_rem_suffix = " datalogger reading";
        }

        List<ge_log_reading> readings2 = log_file.getIncludeReadingsPage(fromDT, toDT, page_size, page);
                                          
            foreach (ge_log_reading reading in readings2) {
                
                foreach (value_header vh in log_file.field_headers) {
                    
                    if (ge_source =="ge_flow")  {
                        mond_ref = String.Format("Round {0:00} Seconds {1:00}",round_no,reading.Duration);
                    }

                    if (vh.id == "WDEPTH" && vh.units=="m") {
                        // Add MOND WDEP record
                       
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "WDEP", mg.MONG_TYPE + mond_rem_suffix, mond_ref, vh.db_name, "Water Depth", vh.units,vh.format, null, ge_source);
                        if (md!=null) MOND.Add (md);
                
                        if (gl!=null && addWLEV==true) {           
                        // Add MOND WLEV record
                        MOND md2 = NewMOND (mg, reading, device_name, round_ref, "WLEV", mg.MONG_TYPE + mond_rem_suffix, mond_ref, vh.db_name,"Water Level", vh.units, vh.format, gl, ge_source);
                        if (md2!=null) MOND.Add (md2);
                        }
                    }
                    
                    if (vh.id == "PH" ) {
                        // Add MOND Potential Hydrogen
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "PH", mg.MONG_TYPE + mond_rem_suffix, mond_ref, vh.db_name, "", vh.units, vh.format, null, ge_source);
                        if (md!=null) MOND.Add (md);
                    }
                    
                    if (vh.id == "DO" && vh.units == "mg/l") {
                        // Add MOND Disolved Oxygen
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "DO", mg.MONG_TYPE + mond_rem_suffix, mond_ref, vh.db_name, "Dissolved Oxygen", vh.units, vh.format, null, ge_source);
                        if (md!=null)  MOND.Add (md);
                    }
                    
                    if ((vh.id == "AEC" && vh.units == "μS/cm") || 
                        (vh.id == "AEC" && vh.units == "mS/cm")) {
                        // Add MOND Electrical Conductivity 
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "AEC", mg.MONG_TYPE + mond_rem_suffix, mond_ref, vh.db_name, "Actual Electrical Conductivity", vh.units, vh.format, null, ge_source);
                        if (md!=null) MOND.Add (md);
                    }
                    
                    if ((vh.id == "EC" && vh.units == "μS/cm") || 
                        (vh.id == "EC" && vh.units == "mS/cm")) {
                        // Add MOND Electrical Conductivity 
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "EC", mg.MONG_TYPE + mond_rem_suffix, mond_ref, vh.db_name, "Electrical Conductivity", vh.units, vh.format, null, ge_source);
                        if (md!=null) MOND.Add (md);
                    }
                    
                    if (vh.id == "SAL" && vh.units == "g/cm3") {
                        // Add MOND Salinity record 
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "SAL", mg.MONG_TYPE + mond_rem_suffix,mond_ref,  vh.db_name, "Salinity", vh.units, vh.format, null, ge_source);
                        if (md!=null) MOND.Add (md);
                    }
                    
                    if (vh.id == "TEMP" && vh.units == "Deg C") {
                        // Add MOND Temp record 
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "DOWNTEMP", mg.MONG_TYPE + mond_rem_suffix, mond_ref, vh.db_name, "Downhole Temperature", vh.units, vh.format, null, ge_source);
                        MOND.Add (md);
                    }
                    
                    if (vh.id == "RDX" && vh.units == "mV") {
                        // Add MOND Redox Salinity record 
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "RDX", mg.MONG_TYPE + mond_rem_suffix, mond_ref, vh.db_name, "Redox Potential", vh.units, vh.format, null, ge_source);
                        if (md!=null) MOND.Add (md);
                    }

                    if (vh.id == "TURB" && vh.units == "NTU") {
                        // Add MOND Salinity record 
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "TURB", mg.MONG_TYPE + mond_rem_suffix,mond_ref,  vh.db_name, "Turbity", vh.units, vh.format, null, ge_source);
                        if (md!=null) MOND.Add (md);
                    }

                }
            }

        return MOND;
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
        
        if (String.IsNullOrEmpty(value)) {
            return null;
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
                    ge_otherId = read.Id.ToString()                    
        };

        return md;

}     
}
}

 