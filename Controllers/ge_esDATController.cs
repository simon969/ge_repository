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

    public class ge_ESdatController: Controller  {     
   
        protected readonly IServiceScopeFactory _serviceScopeFactory;
        protected readonly IDataESdatFileService _dataESdatService;
        protected readonly IDataAGSService _dataAGSService;
        protected readonly IUserOpsService _userService;
        protected readonly IOptions<ge_config> _ge_config;
		protected readonly IHostingEnvironment _env;
        public ge_ESdatController(
            IServiceScopeFactory ServiceScopeFactory,
            IDataESdatFileService ESdatDataService,
            IDataAGSService AGSDataService,
            IUserOpsService UserOpsService,
            IHostingEnvironment HostEnvironment,
            IOptions<ge_config> ge_config)
            : base()
        {
           _serviceScopeFactory = ServiceScopeFactory;
           _dataESdatService = ESdatDataService;
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
        Boolean save_esdat = true;
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

        if (options.Contains("read_esdat_only")) {
            save_esdat = false;    
        }

        var calc_resp = await ReadFile (Id,templateId.Value,table, sheet, "", save_esdat);

        var okResult = calc_resp as OkObjectResult;
    
        if (okResult == null) {
                
                var vwResult  = calc_resp as ViewResult;
                
                if (vwResult == null) {
                return Json(calc_resp);
                }
                
                return calc_resp;
                // return View ("OperationRequest",calc_resp);
            
        }

        ge_esdat_file _esdat_file  = okResult.Value as ge_esdat_file;
     
        if (options.Contains("view_esdat")) {
            if (format == "view") {
            return View ("ReadData", _esdat_file);
            }
            
            if (format=="json") {
             return Json(_esdat_file);
            }
        
            return Ok (_esdat_file);
        } 
    
        if (save_esdat == false) {
            IESdatAGSService _esdatAGSService = new ESdatAGSService();
            ge_table_map _tm = await _dataESdatService.GetFileAsClass<ge_table_map>(tablemapId.Value);
            
            if (_tm == null) {
                return Json($"table map for field not found for id={tablemapId.Value}");
            }

            IAGSGroupTables _ags_file = _esdatAGSService.CreateAGS(_esdat_file,_tm, agstables, options);
            
            if (agslibraryId!=null) {
                await _dataAGSService.SetLibraryAGSData(agslibraryId.Value,null);
            }
            
            await _dataAGSService.AddLibraryAGSData(_ags_file, null);

            if (save_ags == true) {
                var _data = await _dataESdatService.GetDataByIdWithAll(Id);
                var _user = await GetUserAsync();
                string filename = _data.filename.remove_ext() + ".ags";  
                await _dataAGSService.NewAGSData(_data.projectId, _user.Id, _ags_file, filename, "");
            }
            
            if (format=="view") {
            return View (_ags_file);
            }
            
            if (format=="json") {
             return Json(_ags_file);
            }
            
            return Ok(_ags_file);
        // return await View  (mond, format);
        }  

        return await CreateAGS (Id, tablemapId.Value, agstables, options, format, save_ags);

        return Ok (_esdat_file);
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
           
            var _data = await _dataESdatService.GetDataByIdWithAll(Id);

            if (_data == null) {
                return NotFound();
            }
            
            var _template = await _dataESdatService.GetDataById(templateId);
            
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
            
            IESdatFileService _esdatService = new ESdatFileService();

            ge_esdat_file esdat_file = await _esdatService.NewFile(Id,templateId,table,sheet,_dataESdatService);

            if (esdat_file==null){
                return Json($"Unable to locate table ({table}) from template file ({_template.filename}) in data file ({_data.filename})");
            }

            if (save==true) {





            }

            if (format=="view") {
                return View (esdat_file);
            }

            return Ok(esdat_file);

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

            var _data = await _dataESdatService.GetDataByIdWithAll(Id);
            var _tablemapId = await _dataESdatService.GetDataById(tablemapId);
            var _user = await GetUserAsync();

            if (_data == null || _tablemapId == null) {
                return NotFound();
            }
                                             
            IESdatAGSService _esdatAGSService = new ESdatAGSService();
            
            IAGSGroupTables _ags_tables = await _esdatAGSService.CreateAGS(Id,tablemapId,agstables,options,_dataESdatService);

            if (save==true) {
                string filename = _data.filename + ".ags";  
                await _dataAGSService.NewAGSData(_data.projectId, _user.Id, _ags_tables, filename, "");
            }

            if (format=="view") {
            return View (_ags_tables);
            }

            return Ok(_ags_tables);
    
    }

}
}


