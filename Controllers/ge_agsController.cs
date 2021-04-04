using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.AGS;
using ge_repository.OtherDatabase;
using ge_repository.Extensions;
using ge_repository.services;
using ge_repository.repositories;
using ge_repository.interfaces;


namespace ge_repository.Controllers 
{
       public class ge_agsController : Controller
        {
            protected readonly IOptions<ags_config> _agsConfig;
            protected readonly IHostingEnvironment _env;
            protected readonly IOptions<ge_config> _ge_config;
            protected readonly IServiceScopeFactory _serviceScopeFactory;
            protected readonly IDataAGSService _dataService;
            protected readonly IUserOpsService _userService;

        public ge_agsController(
            IServiceScopeFactory ServiceScopeFactory,
            IDataAGSService DataService,
            IUserOpsService UserOpsService,
            IOptions<ags_config> agsConfig,
            IHostingEnvironment env,
		 	IOptions<ge_config> ge_config)
            : base()
        {
             _serviceScopeFactory = ServiceScopeFactory;
             _dataService = DataService;
             _userService = UserOpsService;
             _env = env;
             _ge_config = ge_config;
             _agsConfig = agsConfig;
            
        }
            
         public async Task<IActionResult> CreateXML(Guid Id, string dictionary_file, string data_structure)
        {
            
            if (Id==null) {
                return NotFound(); 
            }

            ge_data data = await _dataService.GetDataByIdWithAll(Id);

         
            if (data==null) {
                return NotFound();
            }
            
            if (data.fileext != FileExtension.AGS) {
             return RedirectToPageMessage (msgCODE.AGS_UNKNOWN_FILE);
            }

            if (data.pflag ==pflagCODE.PROCESSING) {
             return RedirectToPageMessage (msgCODE.AGS_PROCESSING_FILE);
            }
            
            if (_agsConfig == null) {
               return NotFound();
            }

            ags_config config = _agsConfig.Value;

            if (data_structure!=null) {
            config.data_structure = data_structure;
            }
            
            if (dictionary_file!=null) {
            config.dictionary_file = dictionary_file;
            }
            
            ge_user user = await GetUserAsync();

            var _OperationRequest = await _userService.GetOperationRequest(user.Id, data);
            
            if (_OperationRequest.AreProjectOperationsAllowed("Download;Create") == false) {
                return View ("OperationRequest",_OperationRequest);
            }
             
            IAGSConvertXMLService _agsConvertXml = new AGSConvertXMLService(_serviceScopeFactory);

            var resp =  _agsConvertXml.NewAGSClientAsync(config, Id, user.Id);

            return Ok($"AGS Processing File {data.filename} ({data.filesize} bytes), the pflag status will remain as 'Processing' until this workflow is complete");
       
        }

        public ActionResult Index()
        {
          //  ViewData["Message"] = "Welcome to ASP.NET MVC!";

          //  return View();
          return Ok();
        }

        public ActionResult About()
        {
          //  return View();
          return Ok();
        }
        public async Task<List<PROJ>> ReadPROJ(Guid Id) {
            string[] table = new string[] {"PROJ"};
            var resp = await Read(Id,table,"list");
            
            var respOk = resp as OkObjectResult;
            
            if (respOk==null) { 
                    return null;
            }
            var ags_tables = resp as AGS404GroupTables;
            return ags_tables.PROJ.values;
            
        }
        public async Task<List<ABBR>> ReadABBR(Guid Id) {
            string[] table = new string[] {"ABBR"};
            var resp = await Read(Id,table,"list");
            var respOk = resp as OkObjectResult;
            
            if (respOk==null) { 
                    return null;
            }
            var ags_tables = resp as AGS404GroupTables;
            return ags_tables.ABBR.values;
        }
        public async Task<List<UNIT>> ReadUNIT(Guid Id) {
            string[] table = new string[] {"UNIT"};
            var resp = await Read(Id,table,"list");
            var respOk = resp as OkObjectResult;
            
            if (respOk==null) { 
                    return null;
            }
            var ags_tables = resp as AGS404GroupTables;
            return ags_tables.UNIT.values;
        }
        public async Task<List<DICT>> ReadDICT(Guid Id) {
            string[] table = new string[] {"DICT"};
            var resp = await Read(Id,table,"list");
            var respOk = resp as OkObjectResult;
            
            if (respOk==null) { 
                    return null;
            }
            var ags_tables = resp as AGS404GroupTables;
            return ags_tables.DICT.values;
        }
        public async Task<IActionResult> ReadFile(Guid Id,
                                             string[] tables,
                                             string format = "view", 
                                             Boolean save = false ) {
         

            return await Read (Id, tables, format, save);
        }

       public async Task<IActionResult> Read(Guid Id,
                                             string[] tables,
                                             string format = "view", 
                                             Boolean save = false ) {

            if (Id == null)
            {
                return NotFound();
            }
            
            var _data = await _dataService.GetDataById(Id);

            if (_data == null)
            {
                return NotFound();
            }
            
            AGS404GroupTables ags_tables = await _dataService.GetAGS404GroupTables (Id, tables);
            
            if (save == true) { 

            }
            
            if (format=="view") {
                return View (ags_tables);
            }

            if (format=="json") {
                return Json(ags_tables);
            }
               
            
            return Ok(ags_tables);
           
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



