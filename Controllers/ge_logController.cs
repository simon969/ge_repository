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

    public class ge_logController: Controller  {     
   
        protected readonly IServiceScopeFactory _serviceScopeFactory;
        protected readonly IDataLoggerFileService _dataService;
        protected readonly IUserOpsService _userService;
        protected readonly IOptions<ge_config> _ge_config;
		protected readonly IHostingEnvironment _env;
        protected ILoggerFileService _logService;
        protected IMONDLogService _mondService;
        public ge_logController(
            IServiceScopeFactory ServiceScopeFactory,
            IDataLoggerFileService DataService,
            IUserOpsService UserOpsService,
            IHostingEnvironment HostEnvironment,
            IOptions<ge_config> ge_config)
            : base()
        {
           _serviceScopeFactory = ServiceScopeFactory;
           _dataService = DataService;
           _userService = UserOpsService;
           _env = HostEnvironment;
           _ge_config = ge_config;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessFileBackground( Guid Id, 
                                              Guid? templateId, 
                                              string table, 
                                              string sheet,
                                              string bh_ref, 
                                              float? probe_depth, 
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

           
            var _data = await _dataService.GetDataByIdWithAll(Id);

            if (_data == null) {
                return NotFound();
            }
            
            var _template = await _dataService.GetDataById(templateId);
            
            if (_template == null) {
                return NotFound();
            }

            var empty_data = new ge_data();

            var user = await GetUserAsync();

            if (user==null) {
              return NotFound();
            } 
            
            var _AllowedDataOperations = await _userService.GetAllowedOperations(user.Id, _data);
            var _AllowedProjectOperations = await _userService.GetAllowedOperations(user.Id, _data.project); 
            var _AllowedGroupOperations = await _userService.GetAllowedOperations(user.Id, _data.project.group); 

            var _IsProjectAdmin = await _userService.IsUserProjectAdmin(user.Id, _data.project);
            var _IsGroupAdmin = await _userService.IsUserGroupAdmin(user.Id, _data.project.group);
            var _OperationRequest = await _userService.GetOperationRequest(user.Id, _data);
            
            if (_OperationRequest.AreProjectOperationsAllowed("Download;Create") == false) {
                if (format == "view") {
                    return View ("OperationRequest",_OperationRequest);
                } 
                return NotFound();
            }
            
            _logService = await getLoggerFileServiceFromDataId(Id);

            ge_log_file log_file = await _logService.NewLogFile(Id,templateId,table,sheet,_dataService);

            if (log_file==null){
                return Json($"Unable to locate table ({table}) from template file ({_template.filename}) in data file ({_data.filename})");
            }

            if (log_file.channel!=table) {
                table = log_file.channel;
            }

            ge_log_file exist_log_file = await  _logService.GetByDataId(Id, table, true);

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
                    int updated = await  _logService.UpdateLogFile(log_file,true);
                    ViewData["fileStatus"] = $"File records updated ({updated})";
                } else {
                int add = await _logService.CreateLogFile(log_file);
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
 private async Task<ILoggerFileService> getLoggerFileServiceFromDataId(Guid Id) {

    OtherDbConnections _dbConnections = await _dataService.GetOtherDbConnectionsByDataId(Id);
    dbConnectDetails _connectLogger = _dbConnections.getConnectType("logger");
    ILoggerFileUnitOfWork _unitOfWork = new LogUnitOfWork(_connectLogger);
    _logService = new LoggerFileService(_unitOfWork);
    
    return _logService;


 } 
 private async Task<IMONDLogService> getMONDLogServiceFromDataId(Guid Id) {

    OtherDbConnections _dbConnections = await _dataService.GetOtherDbConnectionsByDataId(Id);
    dbConnectDetails _connectLogger = _dbConnections.getConnectType("gINT");
    IGintUnitOfWork _unitOfWork = new GintUnitOfWork(_connectLogger);
    _mondService = new MONDLogService(_unitOfWork);
    
    return _mondService;


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
            
            ge_log_file log_file = null;
            
            await getLoggerFileServiceFromDataId (Id);
            
            ge_log_file exist_log_file = await  _logService.GetByDataId(Id,table);

            if (templateId!=null) {
                log_file = await _logService.NewLogFile (Id,templateId.Value,table,sheet,_dataService);
            }

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
                           int updated = await _logService.UpdateLogFile(log_file,true);
                           ViewData["fileStatus"] = $"File records updated ({updated})";
                        } else {
                        int add = await _logService.CreateLogFile(log_file);
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
            
            var _data = await _dataService.GetDataById(Id);

            if (_data == null)
            {
                return NotFound();
            }

            await getLoggerFileServiceFromDataId (Id);
           
            ge_log_file log_file = null;
            ge_log_file exist_log_file = await _logService.GetByDataId(Id,table,true);
            
                       
            if (exist_log_file==null && templateId==null) {
            return Json($"A logger file has not been found for data file: {_data.filename} table: {table} please provide templateId to read this data file");
            }

            if (templateId!=null) {
                log_file = await _logService.NewLogFile(Id,templateId.Value,table,"",_dataService);
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
                            var log_added = await _logService.CreateLogFile(log_file);
                            ViewData["fileStatus"] = $"Records created({log_added})";
                        } else {             
                            var log_updated = await _logService.UpdateLogFile(log_file, true);
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

            await getLoggerFileServiceFromDataId (Id);
            
            await getMONDLogServiceFromDataId (Id);

            ge_log_file log_file  = await _logService.GetByDataId(Id, table,true);

 
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
                    DateTime = read.ReadingDateTime,
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


[HttpPost]
public async Task<IActionResult> ProcessFile(   Guid Id, 
                                                Guid? templateId, 
                                                string table, 
                                                string sheet,
                                                string bh_ref, 
                                                float? probe_depth, 
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
            
            var vwResult  = calc_resp as ViewResult;
            
            if (vwResult == null) {
            return Json(calc_resp);
            }
            
            return calc_resp;
            // return View ("OperationRequest",calc_resp);
           
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
            
            var _data = await _dataService.GetDataById(Id);

            if (_data == null)
            {
                return NotFound();
            }
            
            await getLoggerFileServiceFromDataId (Id);
            
            ge_log_file log_file = null;
            ge_log_file exist_log_file = await _logService.GetByDataId(Id,table,true);
            
                       
            if (exist_log_file==null && templateId==null) {
            return Json($"A logger file has not been found for data file: {_data.filename} table: {table} please provide templateId to read this data file");
            }

            if (templateId!=null) {

                log_file =  await _logService.NewLogFile(Id, templateId.Value,table,"",_dataService);
                      
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
                            var log_added = await _logService.CreateLogFile(log_file);
                            ViewData["fileStatus"] = $"Records created({log_added})";
                        } else {             
                            var log_updated = await _logService.UpdateLogFile(log_file, true);
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
            
            var _data = await _dataService.GetDataById(Id);

            if (_data == null)
            {
                return NotFound();
            }

            var user = GetUserAsync().Result;

            // if (user == null) {
            //     return RedirectToPageMessage (msgCODE.USER_NOTFOUND);
            // }

            // ge_data empty_data= new ge_data();
          

            // int IsDownloadAllowed = _context.IsOperationAllowed(Constants.DownloadOperationName, _data.project, _data);
            // Boolean CanUserDownload = _context.DoesUserHaveOperation(Constants.DownloadOperationName,_data.project,user.Id);
            
            // int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, _data.project, empty_data);
            // Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName,_data.project,user.Id);

            // if (IsDownloadAllowed!=geOPSResp.Allowed) {
            //     return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_PROHIBITED);
            // }
            
            // if (!CanUserDownload) {
            // return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_USER_PROHIBITED);
            // }

            // if (IsCreateAllowed!=geOPSResp.Allowed) {
            //     return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
            // }
            // if (!CanUserCreate) {
            // return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
            // }

            await getLoggerFileServiceFromDataId (Id);

            ge_log_file log_file = null;
            ge_log_file exist_log_file = await _logService.GetByDataId (Id, table, true);
           
            if (exist_log_file==null && templateId==null) {
               return UnprocessableEntity( new {status="error", message=$"A logger file has not been found for data file: {_data.filename} table: {table} please provide templateId to read this data file"});
            }

            if (templateId!=null) {
                log_file = await _logService.NewLogFile (Id,templateId.Value,table,"",_dataService);
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
                            var log_added = await _logService.CreateLogFile(ge_wq.log_file);
                            ViewData["fileStatus"] = $"Records created({log_added})";
                        } else {
                        var log_updated = await _logService.UpdateLogFile(ge_wq.log_file, true);
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
            
            var _data = await _dataService.GetDataById(Id);

            if (_data == null)
            {
                return NotFound();
            }
            
            await getLoggerFileServiceFromDataId (Id);
            
            ge_log_file log_file = null;      
            ge_log_file exist_log_file = await _logService.GetByDataId(Id, table,true);

            if (exist_log_file==null && templateId==null) {
            return Json($"A logger file has not been found for data file: {_data.filename} table: {table} please provide templateId to read this data file");
            }

            if (templateId!=null) {
               log_file = await _logService.NewLogFile (Id,templateId.Value,table,"",_dataService);
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
                            var log_added = await _logService.CreateLogFile(log_file);
                            ViewData["fileStatus"] = $"Records created({log_added})";
                        } else {             
                            var log_updated = await _logService.UpdateLogFile(log_file, true);
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
            
            await getLoggerFileServiceFromDataId (Id);
            
            ge_log_file log_file = await _logService.GetByDataId(Id, table);

           if (log_file==null) {
            return Json($"There no associated logger records for {_data.filename}");

           }

           if (format=="json") {
           return Json(log_file);
           }

           return View ("ReadData", log_file);
            
}


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



public async Task<IActionResult> Copy(Guid Id, string filename = "", Boolean Overwrite = false) {

    ge_data data = await _dataService.GetDataById(Id);

    var user  = await GetUserAsync();
    
    await getLoggerFileServiceFromDataId (Id);
    
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
   
    }

}




