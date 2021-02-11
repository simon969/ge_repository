using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Extensions;
using ge_repository.OtherDatabase;
using ge_repository.interfaces;
using ge_repository.spatial;
using ge_repository.services;
using ge_repository.repositories;

namespace ge_repository.Controllers
{

    public class ge_log2Controller: Controller  {     
   
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ge_DbContext _context;
        private readonly IUnitOfWork _unit;
        private readonly IDataLoggerFileService _dataService;
        private readonly IUserOpsService _userService;
        private IMONDService _mondService {get;set;}
        private ILoggerFileService _logService;
        private IOptions<ge_config> _ge_config {get;}
		private IHostingEnvironment _env {get;}
        public ge_data data_file {get;set;}
        public ge_log_file log_file {get;set;}

        private int NOT_FOUND = -1;
        private string[] READ_STOPS = {"\"\"",""};

        public static string DATETIME_FORMAT {get;} = "yyyy-MM-ddTHH:mm:ss";
        public static string DATE_FORMAT {get;} = "yyyy-MM-dd";
       
        private static string DP3 = "0.000";
        private static string DP2 = "0.00";
        private static string DP1 = "0.0";

       
         public ge_log2Controller(
            IServiceScopeFactory ServiceScopeFactory,
            ge_DbContext Context,
            IHostingEnvironment env,
            IOptions<ge_config> ge_config)
            : base()
        {
           _serviceScopeFactory = ServiceScopeFactory;
            IUnitOfWork _unit = new UnitOfWork(_context);
            _dataService = new DataLoggerFileService(_unit);
            _userService = new UserOpsService(_unit);
        }
        [HttpPost]
        // public async Task<IActionResult> ProcessFile( Guid Id, 
        //                                       Guid? templateId, 
        //                                       string table, 
        //                                       string sheet,
        //                                       string bh_ref, 
        //                                       float probe_depth, 
        //                                       string round_ref,
        //                                       string process,
        //                                       string options,
        //                                       string format = "view",
        //                                       Boolean save_logger = true,
        //                                       Boolean save_mond = true) {
        
        
        // if (process == "Foreground") {
        //         return await ProcessFile (Id, 
        //                             templateId, 
        //                             table, 
        //                             sheet,
        //                             bh_ref, 
        //                             probe_depth, 
        //                             round_ref,
        //                             options,
        //                             format, 
        //                             save_mond);
        // }
        
        // if (process == "Background") {
        //         return await ProcessFileBackground (Id, 
        //                                       templateId, 
        //                                       table, 
        //                                       sheet,
        //                                       bh_ref, 
        //                                       probe_depth, 
        //                                       round_ref,
        //                                       options);
        // }


        //}

        [HttpPost]
        public async Task<IActionResult> ProcessFileBackground( Guid Id, 
                                              Guid? templateId, 
                                              string table, 
                                              string sheet,
                                              string bh_ref, 
                                              float probe_depth, 
                                              string round_ref,
                                              string options = ""
                                              ) { 
            Boolean read_logger = true;
            Boolean save_logger = true; 
            Boolean save_mond = true;
            Boolean ignore_pflag = false;

            read_logger = options.Contains("read_logger");
            save_logger = ! options.Contains("read_logger_ONLY");
            save_logger = options.Contains("save_logger");
            ignore_pflag = options.Contains("ignore_pflag");
            save_mond = options.Contains("save_mond");

            OtherDbConnections _dbConnections = await _dataService.GetOtherDbConnectionsByDataId(Id);
            
            // check current process status
            ge_data data = await _dataService.GetDataById(Id);

            if (ignore_pflag == false && data.pflag != pflagCODE.NORMAL) {
                 return UnprocessableEntity($"{data.filename} is currently being processed. Some large logger files can take more than 20 mins, please wait until pflag on this file is set to normal before re-running the process.");
            }  

            if (_dbConnections ==null) {
                return BadRequest($"Cannot load OtherDbConnection file for {data.filename}, please check project");
            }

            dbConnectDetails _connectGint = _dbConnections.getConnectType("gINT");
            dbConnectDetails _connectLogger = _dbConnections.getConnectType("logger");
            
            if (_connectGint ==  null || _connectLogger ==null) {
                return BadRequest($"Cannot load gINT or logger connection details for {data.filename}, please check project");
            }
            

            var resp = await runLogClientAsync(
                              _serviceScopeFactory,
                              Id,
                              templateId,
                              table,
                              sheet,
                              bh_ref,
                              probe_depth,
                              round_ref,
                              _connectGint,
                              _connectLogger,
                              read_logger,
                              save_logger,
                              save_mond);

            return Ok($"Processing File {data.filename} ({data.filesize} bytes), the pflag status will remain as 'Processing' until this workflow is complete");
            }

        private async Task<ge_log_client.enumStatus> runLogClientAsync(
                                          IServiceScopeFactory serviceScopeFactory,
                                          Guid Id,
                                          Guid? templateId,
                                          string table,
                                          string sheet,
                                          string bh_ref,
                                          float? probe_depth,
                                          string round_ref,
                                          dbConnectDetails connectGint,
                                          dbConnectDetails connectLogger,
                                          Boolean read_logger = false,
                                          Boolean save_logger = false,
                                          Boolean save_MOND = false)    {
        
                return await Task.Run(()=> runLogClient (
                                          serviceScopeFactory,
                                          Id,
                                          templateId,
                                          table,
                                          sheet,
                                          bh_ref,
                                          probe_depth,
                                          round_ref,
                                          connectGint,
                                          connectLogger,
                                          read_logger,
                                          save_logger,
                                          save_MOND));
        }
        
        private ge_log_client.enumStatus runLogClient(
                                          IServiceScopeFactory serviceScopeFactory,
                                          Guid Id,
                                          Guid? templateId,
                                          string table,
                                          string sheet,
                                          string bh_ref,
                                          float? probe_depth,
                                          string round_ref,
                                          dbConnectDetails connectGint,
                                          dbConnectDetails connectLogger,
                                          Boolean read_logger = false,
                                          Boolean save_logger = false,
                                          Boolean save_MOND = false
                                          ) {
              
            ge_log_client ac = new ge_log_client(serviceScopeFactory, 
                                                connectLogger, 
                                                connectGint);
            
            
            
            ac.Id = Id;
            ac.templateId = templateId;
            ac.table = table;
            ac.sheet = sheet;
            ac.bh_ref = bh_ref;
            ac.probe_depth = probe_depth;
            ac.round_ref = round_ref;
            ac.read_logger = read_logger;
            ac.save_logger = save_logger;
            ac.save_MOND = save_MOND;
            ac.start(); 

            return ge_log_client.enumStatus.Start; 

        }

        private string get_ge_source(string table) {

            if (table.Contains("waterquality") || 
                table.Contains("wq") ) {
                return "ge_flow";
            }
 
            if (table.Contains("depth") || 
                table.Contains("head") || 
                table.Contains("pressure") || 
                table.Contains("channel") || 
                table.Contains("r0") ||
                table.Contains("r1")
                ) {
                return "ge_logger";
            }

            return "";

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
            return await ReadFile(Id, templateId, table, "", format, save);                                
}

public async Task<IActionResult> ReadFile(Guid Id,
                                          Guid templateId,
                                          string table = "",
                                          string sheet = "",
                                          string format = "view", 
                                          Boolean save = false
                                            ) {

            if (Id == Guid.Empty) {
                return NotFound();
            }
            
            if (templateId == Guid.Empty) {
                return NotFound();
            }

           
            var _data = await _dataService.GetDataById(Id);

            if (_data == null)
            {
                return NotFound();
            }
            
            var _template = await _dataService.GetDataById(templateId);
            
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
          
           // ge_log_file log_file = null;
           ge_log_file exist_log_file = null;

            var read_resp = await ReadFile(Id, templateId, table, "");
            
            var okResult = read_resp as OkObjectResult;   
            
            if (okResult == null) { 
                return Json(read_resp);
            } 

            if (okResult.StatusCode!=200) {
                return Json(read_resp);
            }
           
            if (okResult.StatusCode == 200) {
                return Json(read_resp);
            }
            
            if (log_file==null){
                return Json($"Unable to locate table ({table}) from template file ({_template.filename}) in data file ({_data.filename})");
            }

            if (log_file.channel!=table) {
                table = log_file.channel;
            }

            var exist_resp = await  _logService.GetByDataId(Id, table, true);

            if (exist_log_file==null) {
                ViewData["fileStatus"] = "File records not written";
            } else {
                if (exist_log_file.readingAggregates==log_file.readingAggregates) {
                    ViewData["fileStatus"] = "File records written match";
                } else {
                    ViewData["fileStatus"] = "File records written do not match";
                }
            }
            
            if (save==true) {
                if (exist_log_file !=null) {
                    
                    // int del = await DeleteFile(Id);
                    // ViewData["fileStatus"] = $"Existing records deleted ({del})";
                    log_file.Id  = exist_log_file.Id;
                    int updated = await  UpdateFile(log_file,true);
                    ViewData["fileStatus"] = $"File records updated ({updated})";
                } else {
                int add = await AddNewFile(log_file);
                    if (add>0) { 
                        ViewData["fileStatus"] = $"File records written ({add})";
                    }
                }
            }
            
    //      return Ok();
            
            if (format=="view") {
            return View("ReadData", log_file);
            }

            return Ok(log_file);

 }

[HttpPost]
public async Task<IActionResult> ReadFile(Guid Id,
                                          Guid? templateId,
                                          string table,
                                          string bh_ref,
                                          float? probe_depth,
                                          string format = "view", 
                                          Boolean save = false
                                            )  {
     return await ReadFileWith(Id,templateId, table, null, bh_ref,probe_depth,format,save);
 }

 private async Task<IActionResult> ReadFileWith(Guid Id,
                                          Guid? templateId,
                                          string table,
                                          string sheet,
                                          string bh_ref,
                                          float? probe_depth,
                                          string format = "view", 
                                          Boolean save = false
                                            ) {

           if (templateId!=null) {
            
                var read_resp = await ReadFile(Id, templateId.Value, table, sheet);
           
                var read_okResult = read_resp as OkObjectResult;   
            
                if (read_okResult == null) {
                    return (read_resp);
                }

                if (read_okResult.StatusCode != 200) {
                    return (read_resp);
                }

                if (read_okResult.StatusCode == 200) {
                    log_file = read_okResult.Value as ge_log_file;
                }
           }

            
            ge_log_file exist_log_file = await  _logService.GetByDataId(Id,table);


            if (log_file==null && exist_log_file==null) {
                return BadRequest($"The logger file {Id} for table {table} has not been read and there is no existing processed logger saved");
            }
            
            if (exist_log_file!=null && log_file!=null) {
                if (exist_log_file.readingAggregates == log_file.readingAggregates) {
                    ViewData["fileStatus"] = "File records written match";
                } else {
                    ViewData["fileStatus"] = "File records written do not match";
                }
            }

            if (log_file!=null && exist_log_file==null){
                ViewData["fileStatus"] = "File records not written";
            }
            
            if (log_file==null && exist_log_file!=null) {
                log_file = exist_log_file;
            }

            ge_log_helper gf = new ge_log_helper();
            gf.log_file = log_file;
            gf.AddOverrides (probe_depth, bh_ref);      
            
            if (save==true) {
                        if (exist_log_file !=null) {
                           log_file.Id  = exist_log_file.Id;
                           int updated = await UpdateFile(log_file,true);
                           ViewData["fileStatus"] = $"File records updated ({updated})";
                        } else {
                        int add = await AddNewFile(log_file);
                            if (add>0) { 
                                ViewData["fileStatus"] = $"File records written ({add})";
                            }
                        }
                    }
                  
            if (format == "view") {
            return View("ReadData", log_file);
            }

            return Ok(log_file);
    
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

            ge_log_file exist_log_file = await _logService.GetByDataId(Id,table,true);
            
                       
            if (exist_log_file==null && templateId==null) {
            return Json($"A logger file has not been found for data file: {_data.filename} table: {table} please provide templateId to read this data file");
            }

            if (templateId!=null) {
                var read_resp = await ReadFile (Id, templateId.Value, table,"");

                var okResult = read_resp as OkObjectResult;   
    
                if (okResult == null) { 
                    return Json(read_resp);
                }

                if (okResult.StatusCode!=200) {
                    return Json(read_resp);
                }
                
                if (okResult.StatusCode ==200) {
                   log_file = okResult.Value as ge_log_file;
                }
            
            }
            
            if (log_file==null && exist_log_file!=null) {
            log_file = exist_log_file; 
            log_file.Id = exist_log_file.Id;
            }
            
            if (log_file==null) {
                return Json($"The data file: {_data.filename} table: {table}) cannot be read with templateId provided");
            } 
                        
            log_file.data = _data;

            ge_log_calculateVWT ge_calculateVWT = new ge_log_calculateVWT() ;

            ge_calculateVWT.log_file = log_file;

            if (baroIds!=null) {
                    foreach (Guid? baroId in baroIds) {  
                        if (baroId!=null) {
                            ge_data _baro_data = await _dataService.GetDataById(baroId.Value);
                            
                            ge_log_file baro_file = await _logService.GetByDataId(baroId.Value,"data_pressure",true);

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
            
            var _data = await _dataService.GetDataById(Id);

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
            
            log_file  = await _logService.GetByDataId(Id, table,true);

 
            List<MOND> mond = await _mondService.CreateMOND (log_file,
                                                            table,
                                                            round_ref,
                                                            fromDT,
                                                            toDT,
                                                            save);

             if (format == "view") {
                return View("ViewMOND", mond);
            } 

            if (format == "json") {
                return Json(mond);
            }

            return Ok(mond);
 }

private async Task<IActionResult> createMOND(   ge_log_file log_file,
                                                    string table, 
                                                    DateTime? fromDT,
                                                    DateTime? toDT,
                                                    string round_ref,
                                                    string format = "view", 
                                                    Boolean save = false 
                                                    ) 
 {
            
            
            List<MOND> mond = await _mondService.CreateMOND (log_file,
                                                            table,
                                                            round_ref,
                                                            fromDT,
                                                            toDT,
                                                            save);

             if (format == "view") {
                return View("ViewMOND", mond);
            } 

            if (format == "json") {
                return Json(mond);
            }

            return Ok(mond);
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

[HttpPost]
public async Task<IActionResult> ProcessWQ( string[] process) {

    List<ge_log_file> processedOk;

    for (int i=0; i<process.Count(); i++) {
  
        string[] line = process[i].Split (",");
        
        Guid Id = Guid.Parse(line[0]);
        Guid templateId = Guid.Parse(line[1]);
        string bh_ref = line[2];
        float probe_depth = Single.Parse(line[3]);
        string round_ref = line[4];

        var process_rep = await ProcessWQ (Id,templateId,bh_ref,probe_depth, round_ref, "");
        

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
public async Task<IActionResult> ProcessFile(   Guid Id, 
                                                Guid? templateId, 
                                                string table, 
                                                string sheet,
                                                string bh_ref, 
                                                float probe_depth, 
                                                string round_ref,
                                                string options = "",
                                                string format = "view", 
                                                Boolean save = false ) { 
    Boolean save_logger = true;
    
    if (options==null) options = "";
    
    if (options.Contains("background")) {
        
        if (save==true && !options.Contains("save_mond")){
            options += ",save_mond";
        }

        return await ProcessFileBackground (Id, 
                                            templateId, 
                                            table, 
                                            sheet,
                                            bh_ref, 
                                            probe_depth, 
                                            round_ref,
                                            options);
    
    }

    if (options.Contains("read_logger_only")) {
        save_logger = false;    
    }

     var calc_resp = await ReadFileWith (Id,templateId,table, sheet, bh_ref, probe_depth, "", save_logger);
     var okResult = calc_resp as OkObjectResult;   
     if (okResult == null) { 
            return Json(calc_resp);
     }

    ge_log_file log_file  = okResult.Value as ge_log_file;
     
    if (options.Contains("view_logger")) {
         
        if (format == "view") {
            return View ("ReadData", log_file);
        }
        if (format=="json") {
             return Json(log_file);
        }
        return Ok (log_file);
    } 
    
    if (save_logger == false) {
        return await createMOND(log_file,table,null,null,round_ref,format,false);    
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
            
            ge_log_file exist_log_file = await _logService.GetByDataId(Id,table,true);
            
                       
            if (exist_log_file==null && templateId==null) {
            return Json($"A logger file has not been found for data file: {_data.filename} table: {table} please provide templateId to read this data file");
            }

            if (templateId!=null) {
                var read_resp = await ReadFile (Id, templateId.Value, table,"");
                var okResult = read_resp as OkObjectResult;   
    
                if (okResult == null) { 
                    return Json(read_resp);
                }

                if (okResult.StatusCode!=200) {
                    return Json(read_resp);
                }
                
                if (okResult.StatusCode ==200) {
                   log_file = okResult.Value as ge_log_file;
                }
            
            }
            
            if (log_file==null && exist_log_file!=null) {
            log_file = exist_log_file; 
            log_file.Id = exist_log_file.Id;
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
                        if (baroId != null) {
                            ge_log_file baro_file = await _logService.GetByDataId(baroId.Value,"pressure",true);
                            ge_data baro_data = await _dataService.GetDataById(baroId.Value);

                            if (baro_file==null) {
                            return Json($"Baro logger file records has not been found for data file ({baro_data.filename}), please create baro logger file, before calculating wdepth");   
                            }
                            baro_file.data = baro_data;
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

            return Ok(log_file);
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
            
            ge_log_file exist_log_file = await _logService.GetByDataId (Id, table, true);
           
            if (exist_log_file==null && templateId==null) {
               return UnprocessableEntity( new {status="error", message=$"A logger file has not been found for data file: {_data.filename} table: {table} please provide templateId to read this data file"});
            }

            if (templateId!=null) {
                var read_resp = await ReadFile (Id, templateId.Value, table,"");
                var okResult = read_resp as OkObjectResult;   
    
                if (okResult == null) { 
                    return UnprocessableEntity(read_resp);
                }

                if (okResult.StatusCode!=200) {
                    return BadRequest(read_resp);
                }
                
                log_file = okResult.Value as ge_log_file;
               
            
            }
            
            if (log_file==null && exist_log_file!=null) {
                log_file = exist_log_file; 
                log_file.Id = exist_log_file.Id;
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
            
            ge_log_file exist_log_file = await _logService.GetByDataId(Id, table,true);

            if (exist_log_file==null && templateId==null) {
            return Json($"A logger file has not been found for data file: {_data.filename} table: {table} please provide templateId to read this data file");
            }

           if (templateId!=null) {
                var read_resp = await ReadFile (Id, templateId.Value, table,"");
                var okResult = read_resp as OkObjectResult;   
    
                if (okResult == null) { 
                    return Json(read_resp);
                }

                if (okResult.StatusCode!=200) {
                    return Json(read_resp);
                }
                
                if (okResult.StatusCode ==200) {
                   log_file = okResult.Value as ge_log_file;
                }
            
            }
            
            if (log_file==null && exist_log_file!=null) {
            log_file = exist_log_file; 
            log_file.Id = exist_log_file.Id;
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
                            ge_data baro_data = await _dataService.GetDataById(baroId.Value);
                            ge_log_file baro_file = await _logService.GetByDataId(baroId.Value, table);
                           
                            if (baro_file==null) {
                                return Json($"Baro logger file records has not been found for data file ({baro_data.filename}), please create baro logger file, before calculating wdepth");   
                            }
                            
                            baro_file.data = baro_data;
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

            return Ok(ge_diver.log_file);
    }


public async Task<IActionResult> View (Guid Id, 
                                       string table,
                                       string format="view") {

            if (Id == null)
            {
                return NotFound();
            }
            
            var _data = await _dataService.GetDataById(Id);

            if (_data == null)
            {
                return NotFound();
            }
            
            ge_data empty_data = new ge_data();

           // var userId = _userManager.GetUserId(User);

            string userId = null;

            string UserAllowed =  await _userService.GetAllowedOperations(userId, _data);

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

            ge_log_file log_file = await _logService.GetByDataId(Id, table);

           if (log_file==null) {
            return Json($"There no associated logger records for {_data.filename}");

           }

           if (format=="json") {
           return Json(log_file);
           }

           return View ("ReadData", log_file);
            
}

// private async Task<dbConnectDetails> getConnectDetails (Guid projectId, string dbType) {
            
//             if (projectId==null) {
//             return  null;
//             }
        
//             var project = await _context.ge_project
//                         .Include(p =>p.group)
//                         .FirstOrDefaultAsync(m => m.Id == projectId);

//             if (project == null) {
//                 return  null;
//             }

//             if (project.otherDbConnectId==null) {
//             return  null;
//             }

//             var cs = await new ge_dataController(  _context,
//                                                         _authorizationService,
//                                                         _userManager,
//                                                         _env ,
//                                                         _ge_config).getDataAsClass<OtherDbConnections>(project.otherDbConnectId.Value); 

//             if (cs==null) {    
//                 return null;
//             }
     
//             dbConnectDetails cd = cs.getConnectType(dbType);

//             return cd;
//     }

// public Task<ge_log_file> GetFileTask (Guid dataId) {
//         return Task.Run(() =>
//         {
//         return GetFile(dataId);       //YOUR CODE HERE
//         });

// }
private async Task<Guid?> findFirstWhereHeaderContains(List<ge_data> items, string name, string value) {

    foreach (ge_data g in items) {
        ge_log_file glf = await _logService.GetByDataId(g.Id,"", false);
        
        string value_found = glf.file_headers.Find(s=>s.name==name).value;
        
        if (value==value_found) {
            return g.Id;
        }
    }
    
    return (Guid?) null;
}
private async Task<Guid?> findFirstWhereHeaderContains(geXML list, string name, string[] value) {

    foreach (data g in list.projects.First().data_list) {
        ge_log_file glf = await _logService.GetByDataId(g.Id,"", false);

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


private async Task<int> AddNewFile (ge_log_file file, string source ="db") {

        string s1 = file.SerializeToXmlString<ge_log_file>();
        
        if (source == "db") {
         //   var resp = await  new ge_logdbController( _context,
                                                        // _authorizationService,
                                                        // _userManager,
                                                        // _env ,
                                                        // _ge_config).Post(s1, "xml");

            
        }

        if (source == "file") {



        }


        return -1;
}

private async Task<ge_log_file> GetFile(Guid dataId, 
                                        string table = "data_pressure", 
                                        Boolean IncludeReadings=true) {


    var log_file  = await  _logService.GetByDataId(dataId, table, IncludeReadings);

    return log_file;
}
private async Task<int>  UpdateFile (ge_log_file file, Boolean IncludeReadings, string source ="db") 
{

        string s1 = file.SerializeToXmlString<ge_log_file>();

        
        if (source == "db") {
            // var resp = await  new ge_logdbController( _context,
            //                                             _authorizationService,
            //                                             _userManager,
            //                                             _env ,
            //                                             _ge_config).Put(s1,true,"xml");


        }

        if (source == "file") {



        }


        return -1;



}

public async Task<IActionResult> Copy(Guid Id, string filename = "", Boolean Overwrite = false) {

    ge_data data = await _dataService.GetDataById(Id);

    var user  = await GetUserAsync();

    var files = await _logService.GetAllByDataId(Id);

       
    //to prevent circular xml serialisation set parent project object to null
    data.project = null;
    
    if (filename != null && filename != "") {
        data.filename = filename;
    }

    List<ge_data> resp = new List<ge_data>();

    foreach (ge_log_file file in files) {
        
        ge_data exist_log_data = await _dataService.GetDataById(Id);
                
        string s1 = file.SerializeToXmlString<ge_log_file>();
       
        if (exist_log_data!=null && !Overwrite) {
            continue;
        }

        ge_data new_log_data = _dataService.NewData(data.projectId,user.Id,file,"text/xml");
       
        if (exist_log_data !=null) {
          await _dataService.UpdateData(exist_log_data, new_log_data);
        } else {
          await _dataService.CreateData (new_log_data)  ;
        }

    }

    return Json(resp);


}

   
 public  RedirectToPageResult RedirectToPageMessage(int msgCODE) {
        // RedirectToAction(string actionName, string ge_message, string fragment);
            return RedirectToPage ("/Shared/Message",new {MsgId = msgCODE});
           
        }
public async Task<ge_user>  GetUserAsync() {
          try {
                if (HttpContext!=null) {
                var claim = HttpContext.User.Claims.First(c => c.Type == "email");
                string emailAddress = claim.Value;
                ge_user user = await _userService.GetUserByEmailAddress(emailAddress);
                return user;
                }
                return null;
                
          } catch {
              return null;
          }
        }
//  private int find_row(List<search_item> list, string name, int retIfNotFound, Boolean Exact=true) {
    
//     search_item si = list.Find(e=>e.name==name);
    
//     if (si==null && Exact==false) {
//         si = list.Find(e=>e.name.Contains(name));
//     }

//     if (si==null) {
//         return retIfNotFound;
//     }
    
//     return si.row;

//  }

// private async Task<int> DeleteFile(Guid dataId, string channel = "") {

//         int NOT_OK = -1;
               
//         if (dataId == null) {
//                 return NOT_OK;
//         }
        
//         var _logger = await _context.ge_data
//                                     .Include(d =>d.project)
//                                     .SingleOrDefaultAsync(m => m.Id == dataId);

//         dbConnectDetails cd = await getConnectDetails(_logger.projectId,logTables.DB_DATA_TYPE);
      
//         if (cd==null) {
//             return NOT_OK;
//         }

//         string dbConnectStr = cd.AsConnectionString();
//         return await Task.Run(() =>
        
//         {
//             using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
//             {
//                 dsTable ds_readings = new logTables().reading;
//                 dsTable ds_file = new logTables().file;
//                 cnn.Open();
//                 ds_file.setConnection (cnn);        
//                 ds_file.getDataTable ();  
//                 ds_readings.setConnection (cnn);
//                 ds_readings.getDataTable();

//                 if (channel == "") {
//                 ds_file.sqlWhere("DataId='" + dataId.ToString() + "' and (channel is null or channel='')");
//                 } else {
//                 ds_file.sqlWhere("DataId='" + dataId.ToString() + "' and channel='" + channel + "'");
//                 }

//                 ds_file.getDataSet();
//                 return ds_file.Delete();
//             }
//         });
// }    



// private async Task<int> AddNewFile(ge_log_file file) {
    
//         int NOT_OK = -1;
//         int ret = 0;
        
//         if (file == null) {
//                 return NOT_OK;
//         }
        
//         file.packFieldHeaders();
//         file.packFileHeader();
        
//         var _data = await _context.ge_data
//                                     .Include(d =>d.project)
//                                     .SingleOrDefaultAsync(m => m.Id == file.dataId);

//         dbConnectDetails cd = await getConnectDetails(_data.projectId,logTables.DB_DATA_TYPE);
      
//         if (cd==null) {
//             return NOT_OK;
//         }

//         string dbConnectStr = cd.AsConnectionString();

//         return await Task.Run(() =>
//             {
//                 using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
//                 {
//                     dsTable ds_readings = new logTables().reading;
//                     dsTable ds_file = new logTables().file;
//                     cnn.Open();
//                     ds_file.setConnection (cnn);  
//                     ds_file.Reset(); 
                           
//                     ds_readings.setConnection (cnn);
//                     ds_readings.Reset();

//                     DataTable dt_file = ds_file.getDataTable();
//                     DataRow file_row = dt_file.NewRow();
                    
//                     file.Id = Guid.NewGuid();
//                     set_log_file_values (file, file_row);
//                     ds_file.addRow (file_row);

//                     ret = ds_file.Update();
                    
//                     DataTable dt_readings = ds_readings.getDataTable();
                    
//                     foreach (ge_log_reading reading in file.readings) {
//                         DataRow row = dt_readings.NewRow();
//                         reading.Id = Guid.NewGuid();
//                         reading.fileId = file.Id;
//                         set_log_reading_values (reading, row);
//                         ds_readings.addRow (row);
//                     }

//                     ret = ret + ds_readings.Update();
//                     return ret;

//                 }
//             });
//     }

//     // private async Task<List<ge_log_reading>> get_mond_log_readings(Guid projectId, List<MOND> mond) {
//     private async Task<IActionResult> get_mond_log_readings(Guid projectId, List<MOND> mond) {
        
//         dbConnectDetails cd = await getConnectDetails(projectId,logTables.DB_DATA_TYPE);
        
//         if (cd==null) {
//             return NotFound();
//         }

//         if (mond==null) {
//             return NotFound();
//         }
//         string []  selectOtherId = mond.Select (m=>m.ge_otherId).Distinct().ToArray();

//         string sql_where = "id in (" + selectOtherId.ToDelimString(",","'") +  ")";

//         string dbConnectStr = cd.AsConnectionString();

//         return await Task.Run(() =>
//             {
//                 using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
//                 {
//                     dsTable ds_readings = new logTables().reading;
//                     cnn.Open();
//                     ds_readings.setConnection (cnn);
//                     ds_readings.Reset();
//                     ds_readings.sqlWhere(sql_where);
//                     ds_readings.getDataSet();
//                     DataTable dt_readings = ds_readings.getDataTable(); 
//                     List<ge_log_reading> readings =  new List<ge_log_reading>();
//                     foreach(DataRow rrow in dt_readings.Rows)
//                     {    
//                         ge_log_reading r =  new ge_log_reading();
//                         get_log_reading_values(rrow, r);
//                         readings.Add(r);
//                     }
                    
//                     return Ok(readings);
                
//                 }
//             });
//     }


    // private async Task<IActionResult> get_mond_log_files (Guid projectId, List<MOND> mond) {

    //     var resp = await get_mond_log_readings(projectId, MOND);
        
    //     var okResult = resp as OkObjectResult;   
            
    //         if (okResult == null) {
    //                 return (resp);
    //         }

    //         if (okResult.StatusCode != 200) {
    //                 return (resp);
    //         }
        
    //     dbConnectDetails cd = await getConnectDetails(projectId,logTables.DB_DATA_TYPE);
        
    //     if (cd==null) {
    //         return null;
    //     }
        
    //     List<ge_log_reading> readings = okResult.Value as List<ge_log_reading>;

    //     Guid []  selectFileId = readings.Select (m=>m.fileId).Distinct().ToArray();
        
    //     string sql_where = "id in (" + selectFileId.ToDelimString(",","'") +  ")";

    //     string dbConnectStr = cd.AsConnectionString();

    //     return await Task.Run(() =>
    //         {
    //             using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
    //             {
    //                 dsTable ds_file = new logTables().file;
    //                 cnn.Open();
    //                 ds_file.setConnection (cnn);
    //                 ds_file.Reset();
    //                 ds_file.sqlWhere(sql_where);
    //                 ds_file.getDataSet();
    //                 DataTable dt_file = ds_file.getDataTable(); 
    //                 List<ge_log_file> files =  new List<ge_log_file>();
    //                 foreach(DataRow rrow in dt_file.Rows)
    //                 {    
    //                     ge_log_file r =  new ge_log_file();
    //                     get_log_file_values(rrow, r);
    //                     r.unpack_exist_file();
    //                     files.Add(r);
    //                 }
                    
    //                 return Ok(files);
                
    //             }
    //         });

    // }
    
    
    // public async Task<IActionResult> ReadMONDFlowFiles( Guid projectId, string round_ref, string format, bool save) {


    //     string where = $"ge_source='ge_flow' and rnd_ref='{round_ref}'";
        
    //     var resp = await new ge_gINTController (_context,
    //                                         _authorizationService,
    //                                         _userManager,
    //                                         _env ,
    //                                         _ge_config
    //                                         ).getMOND(projectId,
    //                                                    null,
    //                                                    null,
    //                                                    null, 
    //                                                    where,
    //                                                    "");
    //         var okResult = resp as OkObjectResult;   
            
    //         if (okResult == null) {
    //             BadRequest (resp);
    //         }

    //         if (okResult.StatusCode != 200) {
    //             BadRequest (resp);
    //         }
            
    //         MOND = okResult.Value as List<MOND>;

    //         if (MOND==null) {
    //         return BadRequest();
    //         }
        
            
    //         resp = await get_mond_log_files(projectId, MOND);

    //         okResult = resp as OkObjectResult;   
            
    //         if (okResult == null) {
    //                 return (resp);
    //         }

    //         if (okResult.StatusCode != 200) {
    //                 return (resp);
    //         }
        
    //     List<ge_log_file> log_files =  okResult.Value as List<ge_log_file>; 
        
    //     List<MOND> updatedMOND = new List<MOND>();

    //     foreach (ge_log_file log_file in log_files) {

    //         Guid dataId = log_file.dataId;
    //         Guid templateId = log_file.templateId;
    //         string bh_ref = log_file.getBoreHoleId();
    //         float probe_depth = log_file.getProbeDepth();

    //         var process_resp = await ProcessFile (dataId,
    //                                         templateId,
    //                                         "data_waterquality",
    //                                         "", 
    //                                         bh_ref, 
    //                                         probe_depth,
    //                                         round_ref,
    //                                         "",
    //                                         save);
    //         var okProcess = process_resp as OkObjectResult;   
            
    //         if (okProcess == null) {
    //                 return (process_resp);
    //         }

    //         if (okProcess.StatusCode != 200) {
    //                 return (process_resp);
    //         }
        
    //         List<MOND> process_files =  okProcess.Value as List<MOND>;
    //         updatedMOND.AddRange(process_files); 
                       
    //     }

    //     if (format == "view") {
    //        return View("ViewMOND", updatedMOND);
    //     }

    //     return Ok(updatedMOND);
    //     }


    
    
    }

}




