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
using ge_repository.ESdat;
using ge_repository.AGS;

namespace ge_repository.Controllers
{

    public class ge_tablefileController: Controller  {     
   
        protected readonly IServiceScopeFactory _serviceScopeFactory;
        protected readonly IDataTableFileService _dataTableFileService;
        protected readonly IDataAGSService _dataAGSService;
        protected readonly IUserOpsService _userService;
        protected readonly IOptions<ge_config> _ge_config;
		protected readonly IHostingEnvironment _env;
        public ge_tablefileController(
            IServiceScopeFactory ServiceScopeFactory,
            IDataTableFileService DataTableFileService,
            IDataAGSService AGSDataService,
            IUserOpsService UserOpsService,
            IHostingEnvironment HostEnvironment,
            IOptions<ge_config> ge_config)
            : base()
        {
           _serviceScopeFactory = ServiceScopeFactory;
           _dataTableFileService = DataTableFileService;
           _dataAGSService = AGSDataService;
           _userService = UserOpsService;
           _env = HostEnvironment;
           _ge_config = ge_config;
        }
        public async Task<IActionResult> ProcessFile(   Guid Id, 
                                                        Guid? templateId, 
                                                        string table, 
                                                        string sheet,
                                                        Guid? tablemapId, 
                                                        string[] agstables,
                                                        Guid? agslibraryId, 
                                                        string options = "",
                                                        string format = "view", 
                                                        Boolean save = false ) { 
        Boolean save_file = true;
        Boolean save_ags = save;
        if (options==null) options = "";
        
        if (options.Contains("background")) {
            
            if (save==true && !options.Contains("save_ags")){
                options += ",save_ags";
            }

            return await ProcessFileBackground (Id, 
                                                templateId, 
                                                table, 
                                                sheet,
                                                tablemapId, 
                                                agstables, 
                                                options);
        
        }

        if (options.Contains("read_file_only")) {
            save_file = false;    
        }

        var calc_resp = await ReadFile (Id,templateId.Value,table, sheet, "", save_file);

        var okResult = calc_resp as OkObjectResult;
    
        if (okResult == null) {
                
                var vwResult  = calc_resp as ViewResult;
                
                if (vwResult == null) {
                return Json(calc_resp);
                }
                
                return calc_resp;
                // return View ("OperationRequest",calc_resp);
            
        }

        ge_data_table _dt_file  = okResult.Value as ge_data_table;
     
        if (options.Contains("view_file")) {
            if (format == "view") {
            return View ("ReadData", _dt_file);
            }
            
            if (format=="json") {
             return Json(_dt_file);
            }
        
            return Ok (_dt_file);
        } 
    
        if (save_file == false) {
            ITableFileAGSService _tableFileAGSService = new TableFileAGSService();
            ge_table_map _tm = await _dataTableFileService.GetFileAsClass<ge_table_map>(tablemapId.Value);
            
            if (_tm == null) {
                return Json($"table map for field not found for id={tablemapId.Value}");
            }

            IAGSGroupTables _ags_tables = _tableFileAGSService.CreateAGS(_dt_file,_tm, agstables, options);
            
            if (agslibraryId!=null) {
                await _dataAGSService.SetLibraryAGSData(agslibraryId.Value,null);
            }
            
            await _dataAGSService.AddLibraryAGSData(_ags_tables, null);

            if (save_ags == true) {
                var _data = await _dataTableFileService.GetDataByIdWithAll(Id);
                var _tablemap = await _dataTableFileService.GetDataByIdWithAll(tablemapId.Value);
                var _user = await GetUserAsync();
                await _dataAGSService.CreateData(_data.projectId, 
                                                _user.Id, 
                                                _ags_tables, 
                                                _data.FileNameNoExtention() + ".ags",
                                                $"AGS Conversion from data table {_data.filename} using table map {_tablemap.filename}",
                                                "ags");
                  }
            
            if (format=="view") {
            return View (_ags_tables);
            }
            
            if (format=="json") {
             return Json(_ags_tables);
            }
            
            return Ok(_ags_tables);
        // return await View  (mond, format);
        }  

        return await CreateAGS (Id, tablemapId.Value, agstables, options, format, save_ags);

    }


    public async Task<IActionResult> ProcessFileBackground(   Guid Id, 
                                                    Guid? templateId, 
                                                    string table, 
                                                    string sheet,
                                                    Guid? tablemapId, 
                                                    string[] agstables, 
                                                    string options
                                                   ) {
        
        return null;


    } 
    public async Task<IActionResult> ReadFile(   Guid Id, 
                                                    Guid templateId, 
                                                    string table, 
                                                    string sheet,
                                                    string format = "view", 
                                                    Boolean save = false) {
        
        
            if (Id == Guid.Empty) {
                return NotFound();
            }
            
            if (templateId == Guid.Empty) {
                return NotFound();
            }
           
            var _data = await _dataTableFileService.GetDataByIdWithAll(Id);

            if (_data == null) {
                return NotFound();
            }
            
            var _template = await _dataTableFileService.GetDataById(templateId);
            
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
            
            ITableFileService _tableFileService = new TableFileService();

            ge_data_table _tableFile = await _tableFileService.NewFile(Id,templateId,table,sheet,_dataTableFileService);

            if (_tableFile==null){
                return Json($"Unable to locate table ({table}) from template file ({_template.filename}) in data file ({_data.filename})");
            }

            if (save==true) {
                await _dataTableFileService.CreateData(_data.projectId,
                                                        user.Id,_tableFile,
                                                        _data.FileNameNoExtention() + ".xml", 
                                                        $"datatable read from {_data.filename}", 
                                                        "xml");
            }

            if (format=="view") {
                return View (_tableFile);
            }

            return Ok(_tableFile);

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
    
    public async Task<IActionResult> CreateAGS(   Guid Id, 
                                                    Guid tablemapId, 
                                                    string[] agstables, 
                                                    string options,
                                                    string format = "view", 
                                                    Boolean save = false) {

            if (Id == Guid.Empty || tablemapId == Guid.Empty) {
                return NotFound();
            }

            var _data = await _dataTableFileService.GetDataByIdWithAll(Id);
            var _tablemapId = await _dataTableFileService.GetDataByIdWithAll(tablemapId);
            var _user = await GetUserAsync();

            if (_data == null || _tablemapId == null) {
                return NotFound();
            }
                                             
            ITableFileAGSService _tableFileAGSService = new TableFileAGSService();
            
            IAGSGroupTables _ags_tables = await _tableFileAGSService.CreateAGS(Id,tablemapId,agstables,options,_dataTableFileService);

            if (save==true) {
                await _dataAGSService.CreateData(_data.projectId, 
                                                _user.Id, 
                                                _ags_tables, 
                                                _data.FileNameNoExtention() + ".ags",
                                                $"AGS Conversion from data table {_data.filename} using table map {_tablemapId.filename}",
                                                "ags");
            }

            if (format=="view") {
            return View (_ags_tables);
            }

            return Ok(_ags_tables);
    
    }

}
}


