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

    public class ge_logController: ge_Controller  {     
   
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ge_log_file log_file {get;set;}
        public string[] tables_wdepth {get;} = new string[] {"",""};
        public string[] tables_wquality {get;}= new string[] {"",""};
        public List<MOND> MOND {get;set;}
        public  List<MONG> MONG {get;set;}
        public  List<POINT> POINT {get;set;}
        private int NOT_FOUND = -1;
       
        
        private string[] READ_STOPS = {"\"\"",""};

        public static string DATETIME_FORMAT {get;} = "yyyy-MM-ddTHH:mm:ss";
        public static string DATE_FORMAT {get;} = "yyyy-MM-dd";
       
        private static string DP3 = "0.000";
        private static string DP2 = "0.00";
        private static string DP1 = "0.0";

       
         public ge_logController(
            ge_DbContext context,
            IServiceScopeFactory serviceScopeFactory,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,  
            IHostingEnvironment env ,
            IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
           _serviceScopeFactory = serviceScopeFactory;
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

            IUnitOfWork _unit = new UnitOfWork(_context);
            IDataService _dataservice = new DataService(_unit);
            OtherDbConnections _dbConnections = await _dataservice.GetOtherDbConnectionsByDataId(Id);
            
            // check current process status
            ge_data data = await _dataservice.GetDataById(Id);

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
            ac.ge_source = get_ge_source(table);
            ac.probe_depth = probe_depth;

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
private async Task<IActionResult> ReadFile(Guid Id,
                                          Guid templateId,
                                          string table = "",
                                          string sheet = "") {
            
            var template = await new ge_dataController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getDataAsClass<ge_search>(templateId);
            if (template==null) {
            return BadRequest ($"Unable to load templateId {templateId} as a ge_search object");
            }   

            var resp_file = await new ge_dataController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Get(Id);
            
            var okResult = resp_file as OkObjectResult;    

            if (okResult==null) {
                return BadRequest ($"Unable to load ge_logger file Id {Id}");
            }
                        
            ge_data file  = okResult.Value as ge_data;
            
            string[] lines = null;
            ge_search template_loaded = null;

            if (file.fileext == ".csv") {
                lines = await new ge_dataController( _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getDataByLines(Id);
                SearchTerms st = new SearchTerms();
                template_loaded = st.findSearchTerms(template,table, lines);
                if (template_loaded.search_tables.Count==0) {
                    return BadRequest(template_loaded);
                }
            }

            if (file.fileext == ".xlsx") {
                using (MemoryStream ms = await new ge_dataController(  _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).GetMemoryStream(Id)) {
                    ge_log_workbook wb = new ge_log_workbook(ms);
                    SearchTerms st = new SearchTerms();  
                    if (sheet.Contains(",")) {
                    string[] sheets = sheet.Split (",");
                    template_loaded =  st.findSearchTerms (template, table, wb, sheets);  
                    } else {
                    template_loaded  =  st.findSearchTerms (template, table, wb, sheet);
                    }
                    if (template_loaded.search_tables.Count==0) {
                        return BadRequest(template_loaded);
                    }

                    wb.setWorksheet(template_loaded.search_tables[0].sheet);
                    lines = wb.WorksheetToTable();
                    wb.close();
                }
            }

            log_file = getNewLoggerFile (template_loaded, lines);

            if (log_file==null) {
                template_loaded.status = $"Unable to process logger file {file.filename} please check the ge_search template for finding the required header fields";
                return BadRequest(template_loaded);
            }

            log_file.calcReadingAggregates();
            log_file.dataId = Id;
            log_file.templateId = templateId;

            return Ok(log_file);

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

            var exist_resp = await  new ge_logdbController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Get(Id, table, false);

            okResult = exist_resp as OkObjectResult;    

            if (okResult.StatusCode==200) {
                exist_log_file  = okResult.Value as ge_log_file;
            }

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

            ge_log_file exist_log_file = null;
            
            var exist_resp = await  new ge_logdbController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Get(Id, table, true);

            var exist_okResult = exist_resp as OkObjectResult;

            if (exist_okResult != null) {
                exist_log_file  = exist_okResult.Value as ge_log_file;
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

            ge_log_file exist_log_file = null;
            
            var exist_resp = await  new ge_logdbController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Get(Id, table, false);

            var okResult = exist_resp as OkObjectResult;    

            if (okResult.StatusCode==200) {
                exist_log_file  = okResult.Value as ge_log_file;
            }
            
            if (exist_log_file==null && templateId==null) {
            return Json($"A logger file has not been found for data file: {_data.filename} table: {table} please provide templateId to read this data file");
            }

            if (templateId!=null) {
                var read_resp = await ReadFile (Id, templateId.Value, table,"");
                okResult = read_resp as OkObjectResult;   
    
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
                            var _baro_data = await _context.ge_data
                                        .Include(d =>d.project)
                                        .SingleOrDefaultAsync(m => m.Id == baroId);
                            ge_log_file baro_file = null;
                            var baro_resp = await  new ge_logdbController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Get(baroId.Value, "data_pressure", true);
                            okResult = exist_resp as OkObjectResult;    

                            if (okResult.StatusCode==200) {
                                baro_file  = okResult.Value as ge_log_file;
                            }
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
            
            var resp_get = await  new ge_logdbController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Get(Id, table);

            var okResult = resp_get as OkObjectResult;    

            if (okResult.StatusCode==200) {
                log_file  = okResult.Value as ge_log_file;
            }

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

                    
                    var resp = await createMOND(log_file, 
                                                    fromDT, 
                                                    toDT,
                                                    page_size,
                                                    page,
                                                    round_ref,
                                                    ge_source,
                                                    true);

                    okResult = resp as OkObjectResult;   
                        
                    if (okResult == null) {
                            return resp;
                        //    return Json ($"There is an issue converting {ge_source} data file {_data.filename} to MOND records");
                    }
                    
                    if (save == true) { 

                        // string[] selectOtherId= MOND.Select (m=>m.ge_otherId).Distinct().ToArray();
                        // string where2 = $"ge_source='{ge_source}' and ge_otherid in ({selectOtherId.ToDelimString(",","'")})";
                        
                        string where2 = $"ge_source='{ge_source}'"; 

                        var saveMOND_resp = await new ge_gINTController (_context,
                                                            _authorizationService,
                                                            _userManager,
                                                            _env ,
                                                            _ge_config
                                                                ).Upload (_data.projectId, MOND , where2 );
                    }
            
            ordered.AddRange(this.MOND.OrderBy(e=>e.DateTime).ToList());

            }

             if (format == "view") {
                return View("ViewMOND", ordered);
            } 

            if (format == "json") {
                return Json(ordered);
            }

            return Ok(ordered);
 }
private async Task<IActionResult> createMOND (ge_log_file log_file, 
                                            DateTime? fromDT,
                                            DateTime? toDT,
                                            int page_size,
                                            int page,
                                            string round_ref,
                                            string ge_source="ge_flow",
                                            Boolean addWLEV = true) {


        if (log_file==null) {
            return BadRequest("log_file not found");
        }

        var data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == log_file.dataId);
                
        ge_project project = data.project; 

        // Find borehole in point table of gint database
        string holeId = log_file.getBoreHoleId();

        if (holeId=="") {
            return BadRequest ($"Borehole ref not provided");
        }

        string[] SelectPoint = new string[] {holeId};

        var resp = await new ge_gINTController (_context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config
                                                    ).getPOINT(project.Id, SelectPoint);
        
        var okResult = resp as OkObjectResult;   
        if (okResult == null) {
           return resp;
        } 
        
        POINT = okResult.Value as List<POINT>;
        if (POINT == null) {
            return BadRequest ($"Borehole ref {holeId} not found in {project.name}");
            //return -1;
        }
        POINT pt =  POINT.FirstOrDefault();

        if (pt==null) {
            return BadRequest ($"Borehole ref {holeId} not found in {project.name}");
            //return -1;
        }

        resp = await new ge_gINTController (_context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config
                                                    ).getMONG(project.Id, SelectPoint);

        okResult = resp as OkObjectResult;   
        
        if (okResult.StatusCode!=200) {
            return BadRequest ($"No monitoring instalations found for borehole ref {holeId} not found in {project.name}"); 
        //return -1;
        } 
        
        MONG = okResult.Value as List<MONG>;
        if (MONG==null) { 
            return BadRequest ($"No monitoring instalations found for borehole ref {holeId} not found in {project.name}"); 
        //return -1;
        }

        // Find monitoring point in mong table of gint database
        float probe_depth = log_file.getProbeDepth();
        if (probe_depth==0) {
            return BadRequest ($"No probe depth provided for borehole ref {holeId} not found in {project.name}"); 
           // return -1;
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
            return BadRequest ($"No installations in borehole ref {holeId} have a probe depth of {probe_depth} in {project.name}"); 
            // return -1;
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

        return Ok (MOND);
}
//  private async Task<IActionResult> createMOND_WQ (ge_log_file log_file, 
//                                             DateTime? fromDT,
//                                             DateTime? toDT,
//                                             string round_ref,
//                                             Boolean addWLEV = true ) {


//         if (log_file==null) {
//             return BadRequest("log_file not found");
//         }

//         var data = await _context.ge_data
//                                     .Include(d =>d.project)
//                                     .SingleOrDefaultAsync(m => m.Id == log_file.dataId);
                
//         ge_project project = data.project; 

//         // Find borehole in point table of gint database
//         string holeId = log_file.getBoreHoleId();

//         if (holeId=="") {
//             return BadRequest ($"Borehole ref not provided");
//         }

//         string[] SelectPoint = new string[] {holeId};

//         var resp = await new ge_gINTController (_context,
//                                                 _authorizationService,
//                                                 _userManager,
//                                                 _env ,
//                                                 _ge_config
//                                                     ).getPOINT(project.Id, SelectPoint);
        
//         var okResult = resp as OkObjectResult;   
//         if (okResult == null) {
//            return resp;
//         } 
        
//         POINT = okResult.Value as List<POINT>;
//         if (POINT == null) {
//             return BadRequest ($"Borehole ref {holeId} not found in {project.name}");
//             //return -1;
//         }
//         POINT pt =  POINT.FirstOrDefault();

//         if (pt==null) {
//             return BadRequest ($"Borehole ref {holeId} not found in {project.name}");
//             //return -1;
//         }

//         resp = await new ge_gINTController (_context,
//                                                 _authorizationService,
//                                                 _userManager,
//                                                 _env ,
//                                                 _ge_config
//                                                     ).getMONG(project.Id, SelectPoint);

//         okResult = resp as OkObjectResult;   
        
//         if (okResult.StatusCode!=200) {
//             return BadRequest ($"No monitoring instalations found for borehole ref {holeId} not found in {project.name}"); 
//         //return -1;
//         } 
        
//         MONG = okResult.Value as List<MONG>;
//         if (MONG==null) { 
//             return BadRequest ($"No monitoring instalations found for borehole ref {holeId} not found in {project.name}"); 
//         //return -1;
//         }

//         // Find monitoring point in mong table of gint database
//         float probe_depth = log_file.getProbeDepth();
//         if (probe_depth==0) {
//             return BadRequest ($"No probe depth provided for borehole ref {holeId} not found in {project.name}"); 
//            // return -1;
//         }

//         MONG mg = null;
//         List<MONG> PointInstalls = MONG.FindAll(m=>m.PointID==pt.PointID);
        
//         string formatMATCH ="{0:00.0}";

//        if (PointInstalls.Count==1) {
//            mg = PointInstalls.FirstOrDefault();
//        } else {
//             foreach (MONG m in PointInstalls) {
//                 if (m.MONG_DIS!=null) {
//                     if (String.Format(formatMATCH, m.MONG_DIS.Value) == String.Format(formatMATCH,probe_depth)) {
//                         mg = m;
//                         break;
//                     }
//                 }
//             }
//        }

//         if (mg==null) {
//             return BadRequest ($"No installations in borehole ref {holeId} have a probe depth of {probe_depth} in {project.name}"); 
//             // return -1;
//         }
        
//         // Add all readings to new items in List<MOND> 
//         MOND = new List<MOND>();
               
//         string device_name = log_file.getDeviceName();
        
//         float? gl = null;
        
//         if (pt.Elevation!=null) {
//             gl = Convert.ToSingle(pt.Elevation.Value);
//         }

//         if (gl==null && pt.LOCA_GL!=null) {
//             gl = Convert.ToSingle(pt.LOCA_GL.Value);
//         }

//         int round_no = Convert.ToInt16(round_ref);
        
//         List<ge_log_reading> readings2 = log_file.getIncludeReadings(fromDT,toDT);
        
//             foreach (ge_log_reading reading in readings2) {
                
//                 foreach (value_header vh in log_file.field_headers) {
                    
//                     string mond_ref = String.Format("Round {0:00} Seconds {1:00}",round_no,reading.Duration);
                    
//                     if (vh.id == "WDEPTH" && vh.units=="m") {
//                         // Add MOND WDEP record
                       
//                         MOND md = NewMOND (mg, reading, device_name, round_ref, "WDEP", mg.MONG_TYPE + " flow meter reading", mond_ref, vh.db_name, "Water Depth", vh.units,vh.format, null,"ge_flow");
//                         if (md!=null) MOND.Add (md);
                
//                         if (gl!=null && addWLEV==true) {           
//                         // Add MOND WLEV record
//                         MOND md2 = NewMOND (mg, reading, device_name, round_ref, "WLEV", mg.MONG_TYPE + " flow meter reading", mond_ref, vh.db_name,"Water Level", vh.units, vh.format, gl,"ge_flow");
//                         if (md2!=null) MOND.Add (md2);
//                         }
//                     }
                    
//                     if (vh.id == "PH" ) {
//                         // Add MOND Potential Hydrogen
//                         MOND md = NewMOND (mg, reading, device_name, round_ref, "PH", mg.MONG_TYPE + " flow meter reading", mond_ref, vh.db_name, "", vh.units, vh.format, null,"ge_flow");
//                         if (md!=null) MOND.Add (md);
//                     }
                    
//                     if (vh.id == "DO" && vh.units == "mg/l") {
//                         // Add MOND Disolved Oxygen
//                         MOND md = NewMOND (mg, reading, device_name, round_ref, "DO", mg.MONG_TYPE + " flow meter reading", mond_ref, vh.db_name, "Dissolved Oxygen", vh.units, vh.format, null,"ge_flow");
//                         if (md!=null)  MOND.Add (md);
//                     }
   
//                     if (vh.id == "EC" && vh.units == "μS/cm") {
//                         // Add MOND Electrical Conductivity 
//                         MOND md = NewMOND (mg, reading, device_name, round_ref, "EC", mg.MONG_TYPE + " flow meter reading", mond_ref, vh.db_name, "Electrical Conductivity", vh.units, vh.format, null,"ge_flow");
//                         if (md!=null) MOND.Add (md);
//                     }
                    
//                     if (vh.id == "SAL" && vh.units == "g/cm3") {
//                         // Add MOND Salinity record 
//                         MOND md = NewMOND (mg, reading, device_name, round_ref, "SAL", mg.MONG_TYPE + " flow meter reading",mond_ref,  vh.db_name, "Salinity", vh.units, vh.format, null,"ge_flow");
//                         if (md!=null) MOND.Add (md);
//                     }
                    
//                     if (vh.id == "TEMP" && vh.units == "Deg C") {
//                         // Add MOND Temp record 
//                         MOND md = NewMOND (mg, reading, device_name, round_ref, "DOWNTEMP", mg.MONG_TYPE + " flow meter reading", mond_ref, vh.db_name, "Downhole Temperature", vh.units, vh.format, null,"ge_flow");
//                         MOND.Add (md);
//                     }
                    
//                     if (vh.id == "RDX" && vh.units == "mV") {
//                         // Add MOND Redox Salinity record 
//                         MOND md = NewMOND (mg, reading, device_name, round_ref, "RDX", mg.MONG_TYPE + " flow meter reading", mond_ref, vh.db_name, "Redox Potential", vh.units, vh.format, null,"ge_flow");
//                         if (md!=null) MOND.Add (md);
//                     }

//                     if (vh.id == "TURB" && vh.units == "NTU") {
//                         // Add MOND Salinity record 
//                         MOND md = NewMOND (mg, reading, device_name, round_ref, "TURB", mg.MONG_TYPE + " flow meter reading",mond_ref,  vh.db_name, "Turbity", vh.units, vh.format, null,"ge_flow");
//                         if (md!=null) MOND.Add (md);
//                     }

//                 }
//             }

//         return Ok (MOND);
// }
// private async Task<int> createMOND_WDEP (ge_log_file log_file, 
//                                             DateTime? fromDT,
//                                             DateTime? toDT,
//                                             string round_ref,
//                                             Boolean addWLEV = true ) {


//         if (log_file==null) {
//             return -1;
//         }

//         var data = await _context.ge_data
//                                     .Include(d =>d.project)
//                                     .SingleOrDefaultAsync(m => m.Id == log_file.dataId);
                
//         ge_project project = data.project; 

//         // Find borehole in point table of gint database
//         string holeId = log_file.getBoreHoleId();
//         if (holeId=="") {
//             return -1;
//         }

//         string[] SelectPoint = new string[] {holeId};

//         var resp_point = await new ge_gINTController (_context,
//                                                 _authorizationService,
//                                                 _userManager,
//                                                 _env ,
//                                                 _ge_config
//                                                     ).getPOINT(project.Id, SelectPoint);
//         var okResult_point = resp_point as OkObjectResult;   
        
//         if (okResult_point.StatusCode!=200) {
//         return -1;
//         } 
        
//         POINT = okResult_point.Value as List<POINT>;
//         if (POINT == null) {
//             return -1;
//         }

//         POINT pt =  POINT.FirstOrDefault();

//         if (pt==null) {
//             return -1;
//         }

//         var resp_mong = await new ge_gINTController (_context,
//                                                 _authorizationService,
//                                                 _userManager,
//                                                 _env ,
//                                                 _ge_config
//                                                     ).getMONG(project.Id, SelectPoint);
//         var okResult_mong = resp_mong as OkObjectResult;   
        
//         if (okResult_mong.StatusCode!=200) {
//         return -1;
//         } 
        
//         MONG = okResult_mong.Value as List<MONG>;
//         if (MONG==null) { 
//         return -1;
//         }

//         // Find monitoring point in mong table of gint database
//         float probe_depth = log_file.getProbeDepth();
//         if (probe_depth==0) {
//             return -1;
//         }

//         MONG mg = null;
//         List<MONG> PointInstalls = MONG.FindAll(m=>m.PointID==pt.PointID);
        
//         string formatMATCH ="{0:00.0}";

//        if (PointInstalls.Count==1) {
//            mg = PointInstalls.FirstOrDefault();
//        } else {
//             foreach (MONG m in PointInstalls) {
//                 if (m.MONG_DIS!=null) {
//                     if (String.Format(formatMATCH, m.MONG_DIS.Value) == String.Format(formatMATCH,probe_depth)) {
//                         mg = m;
//                         break;
//                     }
//                 }
//             }
//        }

//         if (mg==null) {
//             return -1;
//         }
        
//         // Add all readings to new items in List<MOND> 
//         MOND = new List<MOND>();
//         value_header  log_wdepth =  log_file.getHeaderByIdUnits(ge_log_constants.WDEPTH,"m");
        
//         if (log_wdepth==null) {
//             return -1;
//         }
       
//         string device_name = log_file.getDeviceName();
        
//         float? gl = null;
        
//         if (pt.Elevation!=null) {
//             gl = Convert.ToSingle(pt.Elevation.Value);
//         }

//         if (gl==null && pt.LOCA_GL!=null) {
//             gl = Convert.ToSingle(pt.LOCA_GL.Value);
//         }
        
//         List<ge_log_reading> readings2 = log_file.getIncludeReadings(fromDT,toDT);
        
//         foreach (ge_log_reading reading in readings2) {
            
//             if (reading.getValue(log_wdepth.db_name) == null && reading.NotDry!=-1) {
//                 continue;
//             }
            
//             // Add MOND WDEP record
//             MOND md = NewMOND (mg, reading, device_name, round_ref, "WDEP", mg.MONG_TYPE + " datalogger reading","", log_wdepth.db_name, "Water Depth", "m", DP3, null,"ge_logger");
//             if (md!=null) MOND.Add (md);
            
//             if (gl!=null && addWLEV==true) {           
//             // Add MOND WLEV record
//             MOND md2 = NewMOND (mg, reading, device_name, round_ref, "WLEV", mg.MONG_TYPE + " datalogger reading","", log_wdepth.db_name,"Water Level", "m", DP3, gl,"ge_logger");
//             if (md2!=null) MOND.Add (md2);
//             }

//         }

//         return 0;
// }


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

     if (options.Contains("view_logger")) {
         ge_log_file log_file  = okResult.Value as ge_log_file;
         if (format == "view") {
            return View ("ReadData", log_file);
         }
         if (format=="json") {
             return Json(log_file);
         }
         return Ok (log_file);
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
            
            ge_log_file exist_log_file = null;
            
            var exist_resp = await  new ge_logdbController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Get(Id, table, false);

            var okResult = exist_resp as OkObjectResult;    

            if (okResult.StatusCode==200) {
                exist_log_file  = okResult.Value as ge_log_file;
            }
            
            if (exist_log_file==null && templateId==null) {
            return Json($"A logger file has not been found for data file: {_data.filename} table: {table} please provide templateId to read this data file");
            }

            if (templateId!=null) {
                var read_resp = await ReadFile (Id, templateId.Value, table,"");
                okResult = read_resp as OkObjectResult;   
    
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
                ge_log_file baro_file = null;

                if (baroIds!=null) {
                    foreach (Guid? baroId in baroIds) {   
                        if (baroId!=null) {
                            var _baro_data = await _context.ge_data
                                        .Include(d =>d.project)
                                        .SingleOrDefaultAsync(m => m.Id == baroId);
                                        
                            var baro_resp = await  new ge_logdbController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Get(baroId.Value);

                            okResult = baro_resp as OkObjectResult;    

                            if (okResult.StatusCode==200) {
                                baro_file  = okResult.Value as ge_log_file;
                            }

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
            
            ge_log_file exist_log_file =null;

            var exist_resp = await  new ge_logdbController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Get(Id, table, false);

            var okResult = exist_resp as OkObjectResult;    

            if (okResult.StatusCode==200) {
                exist_log_file  = okResult.Value as ge_log_file;
            }
           
            if (exist_log_file==null && templateId==null) {
               return UnprocessableEntity( new {status="error", message=$"A logger file has not been found for data file: {_data.filename} table: {table} please provide templateId to read this data file"});
            }

            if (templateId!=null) {
                var read_resp = await ReadFile (Id, templateId.Value, table,"");
                okResult = read_resp as OkObjectResult;   
    
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
            
            ge_log_file exist_log_file = null;

            var exist_resp = await  new ge_logdbController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Get(Id, table);

            var okResult = exist_resp as OkObjectResult;    

            if (okResult.StatusCode==200) {
                exist_log_file  = okResult.Value as ge_log_file;
            }

            
            if (exist_log_file==null && templateId==null) {
            return Json($"A logger file has not been found for data file: {_data.filename} table: {table} please provide templateId to read this data file");
            }

           if (templateId!=null) {
                var read_resp = await ReadFile (Id, templateId.Value, table,"");
                okResult = read_resp as OkObjectResult;   
    
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
                            var _baro_data = await _context.ge_data
                                        .Include(d =>d.project)
                                        .SingleOrDefaultAsync(m => m.Id == baroId);
                            ge_log_file baro_file = null;
                            var resp = await  new ge_logdbController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Get(baroId.Value);

                            okResult = resp as OkObjectResult;    

                            if (okResult.StatusCode==200) {
                                baro_file  = okResult.Value as ge_log_file;
                            }
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

            return Ok(ge_diver.log_file);
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

            //   ge_log_file log_file = await GetFile(Id, table);
            var resp = await  new ge_logdbController( _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).Get(Id, table);

            var okResult = resp as OkObjectResult;    

            if (okResult.StatusCode==200) {
                log_file  = okResult.Value as ge_log_file;
            }
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
        ge_log_file glf = null; // await GetFile(g.Id,"", false);
        
        var resp = await  new ge_logdbController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Get(g.Id, "", false);

        var okResult = resp as OkObjectResult;    

        if (okResult.StatusCode==200) {
            glf  = okResult.Value as ge_log_file;
        }
        
        string value_found = glf.file_headers.Find(s=>s.name==name).value;
        
        if (value==value_found) {
            return g.Id;
        }
    }
    
    return (Guid?) null;
}
private async Task<Guid?> findFirstWhereHeaderContains(geXML list, string name, string[] value) {

    foreach (data g in list.projects.First().data_list) {
        ge_log_file glf = null; // await GetFile(g.Id,"", false);

        var resp = await  new ge_logdbController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Get(g.Id, "", false);

        var okResult = resp as OkObjectResult;    

        if (okResult.StatusCode==200) {
            glf  = okResult.Value as ge_log_file;
        }

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


// public async Task<ge_log_file> xGetFile (Guid dataId, 
//                                         string table = "data_pressure", 
//                                         Boolean IncludeReadings=true) {

//         if (dataId == null) {
//                 return null;
//         }
        
//         var _logger = await _context.ge_data
//                                     .Include(d =>d.project)
//                                     .SingleOrDefaultAsync(m => m.Id == dataId);

//         dbConnectDetails cd = await getConnectDetails(_logger.projectId,logTables.DB_DATA_TYPE);
      
//         if (cd==null) {
//             return null;
//         }

//         string dbConnectStr = cd.AsConnectionString();
        
//         return await Task.Run(() =>
        
//         {
//                     using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
//                     {
//                         cnn.Open();
//                         dsTable ds_readings = new logTables().reading;
//                         dsTable ds_file = new logTables().file;
//                         ds_file.setConnection (cnn);        
//                         ds_readings.setConnection (cnn);
                        
//                         //Multichannel transducer have upto 8 tables which will all have the same dataId

//                         if (string.IsNullOrEmpty(table)) {
//                         ds_file.sqlWhere("dataId='" + dataId.ToString() + "' and (channel is null or channel='')");
//                         } else {
//                         ds_file.sqlWhere("dataId='" + dataId.ToString() + "' and channel='" + table + "'" );    
//                         }
                        
//                         ds_file.getDataSet();
//                         DataTable dt_file = ds_file.getDataTable();
    
//                         if (dt_file==null) {
//                             return null;
//                         } 
                        
//                         if (dt_file.Rows.Count==0) {
//                             return null;
//                         }

//                         ge_log_file file = new ge_log_file();
                        
//                         DataRow row = dt_file.Rows[0];
//                         get_log_file_values(row, file);

                       
//                         if (IncludeReadings) {
//                             ds_readings.sqlWhere("FileId='" + file.Id.ToString() + "'");
//                             ds_readings.getDataSet();
//                             DataTable dt_readings = ds_readings.getDataTable();
//                             file.readings = new List<ge_log_reading>();

//                             foreach(DataRow rrow in dt_readings.Rows)
//                             {    
//                                 ge_log_reading r =  new ge_log_reading();
//                                 get_log_reading_values(rrow, r);
//                                 file.readings.Add(r);
//                             }  
//                         file.OrderReadings();
//                         }

//                         file.unpack_exist_file();
                                            
//                         return file;
//                     }   
//             });

// }
// public async Task<int> xUpdateReadings (
//                                     DateTime? fromDT,
//                                     DateTime? toDT,
//                                     string table,
//                                     Guid dataId,
//                                     int? Valid,
//                                     int? Include,
//                                     int? pflag,
//                                     int? NotDry,
//                                     string Remark) {

//         int NOT_OK = -1;
//         int ret = 0;
        
//         if (dataId == null) {
//             return NOT_OK;
//         }

//         var _logger = await _context.ge_data
//                                     .Include(d =>d.project)
//                                     .SingleOrDefaultAsync(m => m.Id == dataId);
//         if (_logger == null) {
//             return NOT_OK;
//         }

//         dbConnectDetails cd = await getConnectDetails(_logger.projectId,logTables.DB_DATA_TYPE);
      
//         if (cd==null) {
//             return NOT_OK;
//         }

//         // ge_log_file log_file =  await GetFile(dataId, table, false);
//         var resp = await  new ge_logdbController( _context,
//                                                 _authorizationService,
//                                                 _userManager,
//                                                 _env ,
//                                                 _ge_config).Get(dataId, table, false);

//         var okResult = resp as OkObjectResult;    

//         if (okResult.StatusCode==200) {
//             log_file  = okResult.Value as ge_log_file;
//         }
//         if (log_file==null) {
//             return NOT_OK;
//         }

//         string sql_where =  "fileId='" + log_file.Id.ToString() + "'";

//         if (fromDT != null) {
//         sql_where += $" and ReadingDateTime >= '{String.Format("{0:yyyy-MM-dd HH:mm:ss}'",fromDT)}"; 
//         }

//         if (toDT != null) {
//         sql_where += $" and ReadingDateTime <= '{String.Format("{0:yyyy-MM-dd HH:mm:ss}'",toDT)}"; 
//         }

//         string dbConnectStr = cd.AsConnectionString();
//         return await Task.Run(() =>
        
//         {
//             using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
//             {
//                 dsTable ds_readings = new logTables().reading;
//                 cnn.Open();
//                 ds_readings.setConnection (cnn);
//                 ds_readings.getDataTable();
//                 ds_readings.sqlWhere(sql_where);
//                 ds_readings.getDataSet();
//                 DataTable dt_readings = ds_readings.getDataTable();
//                 foreach (DataRow row in dt_readings.Rows) {
//                     if (Valid!=null) row["Valid"] = Valid;
//                     if (Include!=null) row["Include"] = Include;
//                     if (pflag!=null)  row["pflag"] =  pflag;
//                     if (NotDry!=null) row["NotDry"] = NotDry;
//                     if (Remark!=null)  row["Remark"] = Remark;
//                 }
                
//                 ret = ds_readings.Update();
//                 return ret;  
//             }

//         });

// }
// public async Task<int> xUpdateChannel (Guid[] Id, Guid dataId, string header, float [] values) {

// //value_header vh = Json.Convert<value_header>(header);

//         int NOT_OK = -1;
//         int ret = 0;
        
//         if (dataId == null) {
//             return NOT_OK;
//         }

//         var _logger = await _context.ge_data
//                                     .Include(d =>d.project)
//                                     .SingleOrDefaultAsync(m => m.Id == dataId);
//         if (_logger == null) {
//             return NOT_OK;
//         }

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
//                 cnn.Open();
//                 ds_readings.setConnection (cnn);
//                 ds_readings.getDataTable();
//                 ds_readings.sqlWhere("Id='" + Id.ToString() + "'");
//                 ds_readings.getDataSet();
//                 DataTable dt_readings = ds_readings.getDataTable();

//                 DataRow row = dt_readings.Rows[0];
//                 // if (Valid!=null) row["Valid"] = Valid;
//                 // if (Include!=null) row["Include"] = Include;
//                 // if (pflag!=null)  row["pflag"] =  pflag;
//                 // if (NotDry!=null) row["NotDry"] = NotDry;
//                 // if (Remark!=null)  row["Remark"] = Remark;
//                 ret = ds_readings.Update();
//                 return ret;  
//             }

//         });

// }


// public async Task<int> xUpdateReading (Guid Id,
//                                     Guid dataId,
//                                     int? Valid,
//                                     int? Include,
//                                     int? pflag,
//                                     int? NotDry,
//                                     string Remark) {

        
//         int NOT_OK = -1;
//         int ret = 0;
        
//         if (dataId == null) {
//             return NOT_OK;
//         }

//         var _logger = await _context.ge_data
//                                     .Include(d =>d.project)
//                                     .SingleOrDefaultAsync(m => m.Id == dataId);
//         if (_logger == null) {
//             return NOT_OK;
//         }

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
//                 cnn.Open();
//                 ds_readings.setConnection (cnn);
//                 ds_readings.getDataTable();
//                 ds_readings.sqlWhere("Id='" + Id.ToString() + "'");
//                 ds_readings.getDataSet();
//                 DataTable dt_readings = ds_readings.getDataTable();

//                 DataRow row = dt_readings.Rows[0];
//                 if (Valid!=null) row["Valid"] = Valid;
//                 if (Include!=null) row["Include"] = Include;
//                 if (pflag!=null)  row["pflag"] =  pflag;
//                 if (NotDry!=null) row["NotDry"] = NotDry;
//                 if (Remark!=null)  row["Remark"] = Remark;
//                 ret = ds_readings.Update();
//                 return ret;  
//             }

//         });

// }

private async Task<int> AddNewFile (ge_log_file file, string source ="db") {

        string s1 = file.SerializeToXmlString<ge_log_file>();
        
        if (source == "db") {
            var resp = await  new ge_logdbController( _context,
                                                        _authorizationService,
                                                        _userManager,
                                                        _env ,
                                                        _ge_config).Post(s1, "xml");

            
        }

        if (source == "file") {



        }


        return -1;
}

private async Task<ge_log_file> GetFile(Guid dataId, 
                                        string table = "data_pressure", 
                                        Boolean IncludeReadings=true) {


    var resp_get = await  new ge_logdbController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Get(dataId, table);

    var okResult = resp_get as OkObjectResult;    

    if (okResult.StatusCode==200) {
                log_file  = okResult.Value as ge_log_file;
                return log_file;
    }

    return null;

}
private async Task<int>  UpdateFile (ge_log_file file, Boolean IncludeReadings, string source ="db") 
{

        string s1 = file.SerializeToXmlString<ge_log_file>();

        
        if (source == "db") {
            var resp = await  new ge_logdbController( _context,
                                                        _authorizationService,
                                                        _userManager,
                                                        _env ,
                                                        _ge_config).Put(s1,true,"xml");


        }

        if (source == "file") {



        }


        return -1;



}

public async Task<IActionResult> Copy(Guid Id, string filename = "", Boolean Overwrite = false) {

    var resp_getdata = await new ge_dataController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Get(Id);
    var okResult_data = resp_getdata as OkObjectResult;
    
    if (okResult_data==null) {
        return BadRequest(resp_getdata);
        
    }

    var user = GetUserAsync();

    var resp_getlog = await  new ge_logdbController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).GetAll(Id, true);

    var okResult_log = resp_getlog as OkObjectResult;    

    if (okResult_log==null) {
        return BadRequest(resp_getlog);
        
    }

    ge_data data = okResult_data.Value as ge_data; 
    
    //to prevent circular xml serialisation set parent project object to null
    data.project = null;
    
    if (filename != null && filename != "") {
        data.filename = filename;
    }

    string s2 =  data.SerializeToXmlString<ge_data>();

    List<ge_log_file> files  = okResult_log.Value as List<ge_log_file>;

    List<ge_data> resp = new List<ge_data>();

    foreach (ge_log_file file in files) {
        
        string s1 = file.SerializeToXmlString<ge_log_file>();
       
        
        if (Overwrite) {
            var resp_put = await  new ge_logdataController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env,
                                                _ge_config).Put(file.Id, s1, s2, "xml");
            var okResult_put = resp_put as OkObjectResult;    
            if (okResult_log!=null) {
                ge_data data_put = okResult_put.Value as ge_data;
                resp.Add (data_put);
            }

            continue;
        }
            var resp_post = await  new ge_logdataController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).Post(s1, s2, "xml");
            var okResult_post = resp_post as OkObjectResult;    
            if (okResult_log!=null) {
                ge_data data_post = okResult_post.Value as ge_data;
                resp.Add (data_post);
            }
    }

    return Json(resp);


}

// private async Task<int>  xUpdateFile (ge_log_file file, Boolean IncludeReadings) {

//         int NOT_OK = -1;
//         int ret = 0;
        
//         if (file.dataId == null) {
//                 return NOT_OK;
//         }
        
//         file.packFieldHeaders();
//         file.packFileHeader();

//         var _logger = await _context.ge_data
//                                     .Include(d =>d.project)
//                                     .SingleOrDefaultAsync(m => m.Id == file.dataId);

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
//                 ds_file.sqlWhere("Id='" + file.Id + "'");
//                 ds_file.getDataSet();
//                 DataTable dt_file = ds_file.getDataTable();

//                 if (dt_file==null) {
//                     return NOT_OK;
//                 } 
                
//                 if (dt_file.Rows.Count==0) {
//                     return NOT_OK;
//                 }

//                 DataRow file_row = dt_file.Rows[0];
//                 set_log_file_values (file, file_row);
//                 ret = ds_file.Update();
                
//                 if (IncludeReadings) {  
//                     ds_readings.sqlWhere("FileId='" + file.Id.ToString() + "'");
//                     ds_readings.getDataSet();
//                     DataTable dt_readings = ds_readings.getDataTable();
//                     Boolean checkExisting = false;
                    
//                     if (dt_readings.Rows.Count>0) {
//                         checkExisting=true;
//                     }

//                     foreach (ge_log_reading reading in file.readings) {
                        
//                         DataRow row = null;
//                         if (checkExisting==true) {
//                             if (reading.Id != Guid.Empty) {
//                             row = dt_readings.Select ($"Id='{reading.Id}'").SingleOrDefault();
//                             }

//                             if (row==null) {
//                             row =  dt_readings.Select ($"ReadingDateTime='{String.Format("{0:yyyy-MM-dd HH:mm:ss.ffff}",reading.ReadingDatetime)}'").SingleOrDefault();
//                             }
//                         }

//                         if (row==null) {
//                             row = ds_readings.NewRow();
//                             reading.Id = Guid.NewGuid();
//                             reading.fileId = file.Id;
//                             ds_readings.addRow (row); 
//                         } else {
//                             reading.Id = (Guid) row["Id"];
//                             reading.fileId = file.Id;
//                         }
                       
//                         set_log_reading_values (reading,row);
//                     }

//                     //what if there are other records (more) in dt_readings from a previous version of the ge_log_file? 
//                     // mark for deletion all records not 'new' or 'updated'
//                     if (file.readings.Count() < dt_readings.Rows.Count) {
//                         foreach (DataRow row in dt_readings.Rows) {
//                         if (row.RowState == DataRowState.Added | 
//                             row.RowState != DataRowState.Modified) {
//                                 row.Delete();
//                             }

//                         } 
//                     }

                    
//                     ret = ret + ds_readings.Update();
//                     return ret;
//                 } 
//             return ret;  
//             }
//         });

// } 
// private void get_log_reading_values(DataRow row, ge_log_reading reading) {

//                 reading.Id = (Guid) row ["Id"];
//                 reading.fileId = (Guid) row["fileId"];     
//                 reading.ReadingDatetime = (DateTime) row["ReadingDateTime"];
//                 if (row["Duration"] != DBNull.Value) reading.Duration= Convert.ToInt64(row["Duration"].ToString());
//                 if (row["Value1"] != DBNull.Value) reading.Value1 =Convert.ToSingle( row["Value1"].ToString());
//                 if (row["Value2"] != DBNull.Value) reading.Value2= Convert.ToSingle(row["Value2"].ToString());
//                 if (row["Value3"] != DBNull.Value) reading.Value3 =Convert.ToSingle( row["Value3"].ToString());
//                 if (row["Value4"] != DBNull.Value) reading.Value4 = Convert.ToSingle(row["Value4"].ToString());
//                 if (row["Value5"] != DBNull.Value)  reading.Value5 =Convert.ToSingle(row["Value5"].ToString());
//                 if (row["Value6"] != DBNull.Value)  reading.Value6 =Convert.ToSingle(row["Value6"].ToString());
//                 if (row["Value7"] != DBNull.Value)  reading.Value7 =Convert.ToSingle(row["Value7"].ToString());
//                 if (row["Value8"] != DBNull.Value) reading.Value8= Convert.ToSingle(row["Value8"].ToString());
//                 if (row["Value9"] != DBNull.Value) reading.Value9 =Convert.ToSingle( row["Value9"].ToString());
//                 if (row["Value10"] != DBNull.Value) reading.Value10 = Convert.ToSingle(row["Value10"].ToString());
//                 if (row["Value11"] != DBNull.Value)  reading.Value11 =Convert.ToSingle(row["Value11"].ToString());
//                 if (row["Value12"] != DBNull.Value)  reading.Value12 =Convert.ToSingle(row["Value12"].ToString());
//                 if (row["Value13"] != DBNull.Value)  reading.Value13 =Convert.ToSingle(row["Value13"].ToString());
//                 if (row["Value14"] != DBNull.Value) reading.Value14 =Convert.ToSingle( row["Value14"].ToString());
//                 if (row["Value15"] != DBNull.Value) reading.Value15 = Convert.ToSingle(row["Value15"].ToString());
//                 if (row["Value16"] != DBNull.Value)  reading.Value16 =Convert.ToSingle(row["Value16"].ToString());
//                 if (row["Value17"] != DBNull.Value)  reading.Value17 =Convert.ToSingle(row["Value17"].ToString());
//                 if (row["Value18"] != DBNull.Value)  reading.Value18 =Convert.ToSingle(row["Value18"].ToString());
//                 if (row["Remark"] != DBNull.Value) reading.Remark = (String) row["Remark"]; 
//                 reading.Valid = (int) row["Valid"];
//                 reading.Include = (int) row["Include"];
//                 reading.pflag = (int) row["pflag"];
//                 reading.NotDry = (int) row["NotDry"];
// }
// private void set_log_reading_values(ge_log_reading reading, DataRow row) {

//                 row["Id"] = reading.Id;
//                 row["fileId"] = reading.fileId;
//                 row["ReadingDateTime"] = reading.ReadingDatetime;
//                 if (reading.Duration==null) { row["Duration"] = DBNull.Value;} else {row["Duration"] = reading.Duration;} 
//                 if (reading.Value1==null) { row["Value1"] = DBNull.Value;} else {row["Value1"] = reading.Value1;} 
//                 if (reading.Value2==null) { row["Value2"] = DBNull.Value;} else {row["Value2"] = reading.Value2;} 
//                 if (reading.Value3==null) { row["Value3"] = DBNull.Value;}  else {row["Value3"] = reading.Value3;}
//                 if (reading.Value4==null) { row["Value4"] = DBNull.Value;} else {row["Value4"] = reading.Value4;} 
//                 if (reading.Value5==null) { row["Value5"] = DBNull.Value;} else {row["Value5"] = reading.Value5;} 
//                 if (reading.Value6==null) { row["Value6"] =  DBNull.Value;} else {row["Value6"] =reading.Value6;}
//                 if (reading.Value7==null) { row["Value7"] = DBNull.Value;} else {row["Value7"] = reading.Value7;} 
//                 if (reading.Value8==null) { row["Value8"] = DBNull.Value;} else {row["Value8"] = reading.Value8;} 
//                 if (reading.Value9==null) { row["Value9"] = DBNull.Value;}  else {row["Value9"] = reading.Value9;}
//                 if (reading.Value10==null) { row["Value10"] = DBNull.Value;} else {row["Value10"] = reading.Value10;} 
//                 if (reading.Value11==null) { row["Value11"] = DBNull.Value;} else {row["Value11"] = reading.Value11;} 
//                 if (reading.Value12==null) { row["Value12"] =  DBNull.Value;} else {row["Value12"] =reading.Value12;}
//                 if (reading.Value13==null) { row["Value13"] = DBNull.Value;} else {row["Value13"] = reading.Value13;} 
//                 if (reading.Value14==null) { row["Value14"] = DBNull.Value;} else {row["Value14"] = reading.Value14;} 
//                 if (reading.Value15==null) { row["Value15"] = DBNull.Value;} else {row["Value15"] = reading.Value15;} 
//                 if (reading.Value16==null) { row["Value16"] =  DBNull.Value;} else {row["Value16"] =reading.Value16;}
//                 if (reading.Value17==null) { row["Value17"] = DBNull.Value;} else {row["Value17"] = reading.Value17;} 
//                 if (reading.Value18==null) { row["Value18"] = DBNull.Value;} else {row["Value18"] = reading.Value18;} 
//                 row["Remark"] = reading.Remark;
//                 row["Valid"] = reading.Valid;
//                 row["Include"] = reading.Include;
//                 row["pflag"] = reading.pflag;
//                 row["NotDry"] = reading.NotDry;

// }
// private void get_log_file_values(DataRow row, ge_log_file file) {

//                 file.Id = (Guid) row["Id"];
//                 file.dataId = (Guid) row["dataId"]; 
//                 if (row["ReadingAggregates"] != DBNull.Value) file.readingAggregates = (String) row["ReadingAggregates"]; 
//                 if (row["FieldHeader"] != DBNull.Value) file.fieldHeader= (String) row["FieldHeader"]; 
//                 if (row["FileHeader"] != DBNull.Value) file.fileHeader= (String) row["FileHeader"]; 
//                 if (row["Comments"] != DBNull.Value) file.Comments= (String) row["Comments"]; 
//                 if (row["channel"] != DBNull.Value) file.channel = (String) row["channel"]; 
//                 if (row["SearchTemplate"] != DBNull.Value) file.SearchTemplate = (String) row["SearchTemplate"];
//                 if (row["templateId"] !=DBNull.Value) file.templateId = (Guid) row["templateId"]; 

// }

// private void set_log_file_values(ge_log_file file, DataRow row) {

//                 row["Id"] = file.Id;
//                 row["dataId"] = file.dataId;
//                 row["fieldHeader"] = file.fieldHeader;
//                 row["ReadingAggregates"] = file.readingAggregates;
//                 row["FileHeader"] = file.fileHeader;
//                 row["Comments"] = file.Comments;
//                 row["channel"] = file.channel;
//                 row["templateId"] = file.templateId;
//                 row["SearchTemplate"] = file.SearchTemplate;

// }

// private DateTime? getDateTime(string s, string format) {
    
//     string[] formats = { "yyyy h:mm:ss tt", "M/dd/yyyy" };
    
//     DateTime dt;
    
//     try {
//         if (DateTime.TryParseExact(s, format, CultureInfo.CurrentCulture,
//                             DateTimeStyles.None, out dt)
//                             ) return dt;

//         // if (DateTime.TryParseExact(s, formats, CultureInfo.CurrentCulture,
//         //                     DateTimeStyles.None, out dt)
//         //                     ) return dt;
//         dt = DateTime.Parse(s);
//         return dt;
//     } catch {
//     return null;
//     }

//     }
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
                    string dateformat) {
    
    
    string[] dateformats = SplitDateFormats(dateformat);


    for (int i = line_start; i<line_end; i++) {
                string line = lines[i];
                if (line.Length>0) {
                    string[] values = line.Split(",");
                    
                    if (values[0].Contains("\"")) {
                        values = line.QuoteSplit();
                    }
                    if (values[intReadTime] == "") {
                        break;
                    }
                   
                    ge_log_reading r= new ge_log_reading();
                    
                    if (intReadTime != NOT_FOUND) {
                        if (ContainsError(values[intReadTime])) {continue;}
                        r.ReadingDatetime = getDateTime(values[intReadTime],dateformats);
                    }
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

 private string[] SplitDateFormats(string dateformat="") {

    string[] dateformats;

    if (dateformat == null) {
        dateformat = "";
    }

    if (dateformat.Length > 0) {
        dateformat = dateformat + "," + DATETIME_FORMAT;
    } else {
        dateformat = DATETIME_FORMAT;
    }

    if (dateformat.Contains(",")) {
        dateformats = dateformat.Split(",");
    } else { 
    dateformats = new string[] {dateformat};
    }

    return dateformats;

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
 private DateTime getDateTime(string s1, string[] dateformats) {

                        if (dateformats!=null) {
                            foreach (string dateformat in dateformats) {
                                DateTime dateTime;
                                Boolean formatOK = DateTime.TryParseExact(s1, dateformat, CultureInfo.CurrentCulture,DateTimeStyles.AllowInnerWhite, out dateTime);
                                if (formatOK) {
                                    return dateTime;
                                }
                            }
                        }

                        return DateTime.Parse(s1);
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
 private Boolean ContainsError( string s1) {

     if (s1.Contains("#VALUE")) return true;
     if (s1.Contains("#ERROR")) return true;
     if (s1.Contains("#REF")) return true;
     if (s1.Contains("#N/A")) return true;

     return false;
 
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
//   private Boolean IsTableStandardFormat(search_table st) {


//                 int max_id = -1;

//                 foreach (value_header vh in st.headers) {
//                     if (max_id < vh.found) {
//                         max_id = vh.found;
//                     }

//                 }

//                 if (max_id==st.headers.Count) {
//                     return true;
//                 } 
//                 return false;
// }


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
        
        int line_start = dic.data_start_row(NOT_FOUND);
        
        int line_end = dic.data_end_row(lines.Count());

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
                                    intCheckValueForDry,
                                    DateTimeReading.format                         
                                    );
        if (readlines <= 0) {
            return null;
        }

        file.init_new_file();

    return file;
    
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




