using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.AGS;
using ge_repository.OtherDatabase;
using ge_repository.Extensions;

namespace ge_repository.Controllers 
{
       public class ge_agsController : ge_Controller
        {
            public IOptions<ags_config> _agsConfig { get; }
            public ge_data data;
            public string userId;
            public List<MOND> MOND {get; set;}
            public List<MONV> MONV {get; set;}
            public List<MONG> MONG {get;set;}          
            public List<POINT> POINT {get;set;}
            public List<ABBR> ABBR {get;set;}
            public List<TRAN> TRAN {get;set;}
            public List<TYPE> TYPE {get;set;}
            public List<UNIT> UNIT {get;set;}
            public List<DICT> DICT {get;set;}
            public List<PROJ> PROJ {get;set;}
            public static int NOT_FOUND = -1;
       public ge_agsController(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IOptions<ags_config> agsConfig,
            IHostingEnvironment env,
		 	IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
             _agsConfig = agsConfig;
             context.Database.SetCommandTimeout(1200);
        }
         public ge_agsController(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IHostingEnvironment env,
		 	IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
          
        } 
         public async Task<IActionResult> CreateXML(Guid Id, string dictionary_file, string data_structure)
        {
            
            if (Id==null) {
                return NotFound(); 
            }
            
            data = _context.ge_data
                                    .Include (d => d.project)
                                    .FirstOrDefault(d =>d.Id==Id);

            if (data==null) {
                return NotFound();
            }
            
            if (data.fileext != FileExtension.AGS) {
             return RedirectToPageMessage (msgCODE.AGS_UNKNOWN_FILE);
            }

            if (data.pflag ==pflagCODE.PROCESSING) {
             return RedirectToPageMessage (msgCODE.AGS_PROCESSING_FILE);
            }

            userId = _userManager.GetUserId(User);

            var new_data = new ge_data();

            int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, data.project, new_data);
            Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName,data.project,userId);
            
            if (IsCreateAllowed!=geOPSResp.Allowed) {
               return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
            }
            if (!CanUserCreate) {
               return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
            }
            
            if (data_structure!=null) {
            _agsConfig.Value.data_structure = data_structure;
            }
            
            if (dictionary_file!=null) {
            _agsConfig.Value.dictionary_file = dictionary_file;
            }
            var b = _context.ge_data_big.FirstOrDefault(m => m.Id == Id);

            if (b == null) {
                return NotFound();
            }

            data.data = b;


            ge_AGS_Client.enumStatus resp = await runAGSClientAsync();
            
            if (resp == ge_AGS_Client.enumStatus.NotConnected) {
                
                return RedirectToPageMessage (msgCODE.AGS_NOTCONNECTED);
            }
            if (resp == ge_AGS_Client.enumStatus.XMLReceiveFailed) {
                
                return RedirectToPageMessage (msgCODE.XML_NOTRECEIVED);
            }
             if (resp == ge_AGS_Client.enumStatus.AGSSendFailed) {
                
                return RedirectToPageMessage (msgCODE.AGS_SENDFAILED);
            }
            
            return RedirectToPage("/Data/Index",new {projectId=data.projectId});
        }

        public async Task<ge_AGS_Client.enumStatus> runAGSClientAsync(){
        
            return await Task.Run(()=> runAGSClient ());
        }
        
        public ge_AGS_Client.enumStatus runAGSClient() {
            if (_agsConfig == null) {
                return ge_AGS_Client.enumStatus.AGSStartFailed;
            }
            string host = _agsConfig.Value.host;
            int port = _agsConfig.Value.port;
            string dic = _agsConfig.Value.dictionary_file;
            string ds = _agsConfig.Value.data_structure;

            ge_AGS_Client ac = new ge_AGS_Client(host, port,_context, userId);
            ac.datastructure = ds;
            ac.dictionaryfile = dic;
            ac.data_ags = data;
            return ac.start();
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
            var list = await Read(Id,table,"list");
            return PROJ;
        }
        public async Task<List<ABBR>> ReadABBR(Guid Id) {
            string[] table = new string[] {"ABBR"};
            var list = await Read(Id,table,"list");
            return ABBR;
        }
        public async Task<List<UNIT>> ReadUNIT(Guid Id) {
            string[] table = new string[] {"UNIT"};
            var list = await Read(Id,table,"list");
            return UNIT;
        }
        public async Task<List<DICT>> ReadDICT(Guid Id) {
            string[] table = new string[] {"DICT"};
            var list = await Read(Id,table,"list");
            return DICT;
        }
        private int addUNIT(string unit_unit, string unit_desc, string unit_rem){
            try {
                
                UNIT u = new UNIT {
                    UNIT_UNIT= unit_unit,
                    UNIT_DESC = unit_desc
                };
                
                UNIT.Add (u);
                return 0;
            } catch {
                return -1;
            } 
            
           
        }
        private int addTYPES (string[] s1) {
            try {
                for (int i=0;i<s1.Length;i++) {
                    addTYPE(s1[i],"");
                }
            return 0;
            } catch {
                return -1;
            }
        }
        private int addUNITS (string[] s1) {
            try {
                for (int i=0;i<s1.Length;i++) {
                    addUNIT(s1[i],"","");
                }
            return 0;
            } catch {
                return -1;
            }
        }
        private int addTYPE (string type_type, string type_desc) {
            try {
                TYPE t = new TYPE {
                        TYPE_TYPE = type_type,
                        TYPE_DESC = type_desc
                }; 

                TYPE.Add (t);
                return 0;
            } 
            catch { 
                return -1;
            }
        }
       public async Task<IActionResult> Read(Guid Id,
                                             string[] tables,
                                             string format = "view", 
                                             Boolean save = false ) {

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
            
            if (tables[0] == null) {
                tables = new string[] {
                            "PROJ",
                            "POINT",
                            "ABBR",
                            "UNIT",
                            "DICT"
                        };
            }
            
            var lines = await new ge_dataController( _context,
                                                     _authorizationService,
                                                     _userManager,
                                                     _env ,
                                                     _ge_config).getDataByLines(Id);

            if (tables.Contains("PROJ")) {
                PROJ = new List<PROJ>();
                readGroup(lines, PROJ);
            }    
            if (tables.Contains("POINT")) {
                POINT = new List<POINT>();
                readGroup(lines, POINT);
            }          
            if (tables.Contains("ABBR")) {
                ABBR = new List<ABBR>();
                readGroup(lines, ABBR);
            }         
            if (tables.Contains("UNIT")) {
                UNIT = new List<UNIT>();
                readGroup(lines, UNIT);
            }         
            if (tables.Contains("DICT")) {
                DICT = new List<DICT>();
                readGroup(lines, DICT);
            }

            return Ok();
           }

        private int find_line(string[] lines, string value) {
            for (int i=0; i<lines.Count();i++) {
                if (lines[i]==value){
                return i;
                };
            }
            return -1;
        }
        private int readGroup(string[] lines, List<UNIT> list) {

            int group_start = find_line (lines, "\"GROUP\",\"UNIT\"");

            if (group_start==NOT_FOUND) {
                return -1;
            }

            string[] header = lines[group_start+1].QuoteSplit();
            string[] units = lines[group_start+2].QuoteSplit();
            string[] type = lines[group_start+3].QuoteSplit();

            addUNITS(units);
            addTYPES(type);
            
            for (int i=group_start + 4; i<lines.Count();i++) {
                string line = lines[i];
                if (line.Length==0) {
                    return i;
                }
                string[] values = line.QuoteSplit();
                UNIT p = new UNIT();
                setValues(header, values, p);
                list.Add (p);
            }
            return 0;
        }

        private int readGroup(string[] lines, List<DICT> list) {

            int group_start = find_line (lines, "\"GROUP\",\"DICT\"");

            if (group_start==NOT_FOUND) {
                return -1;
            }

            string[] header = lines[group_start+1].QuoteSplit();
            string[] units = lines[group_start+2].QuoteSplit();
            string[] type = lines[group_start+3].QuoteSplit();
            
            for (int i=group_start + 4; i<lines.Count();i++) {
                string line = lines[i];
                if (line.Length==0) {
                    return i;
                }
                string[] values = line.QuoteSplit();
                DICT p = new DICT();
                setValues(header, values, p);
                list.Add (p);
            }
            return 0;
        }
         private int readGroup(string[] lines, List<TYPE> list) {

            int group_start = find_line (lines, "\"GROUP\",\"TYPE\"");

            if (group_start==NOT_FOUND) {
                return -1;
            }

            string[] header = lines[group_start+1].QuoteSplit();
            string[] units = lines[group_start+2].QuoteSplit();
            string[] type = lines[group_start+3].QuoteSplit();
            
            for (int i=group_start + 4; i<lines.Count();i++) {
                string line = lines[i];
                if (line.Length==0) {
                    return i;
                }
                string[] values = line.QuoteSplit();
                TYPE p = new TYPE();
                setValues(header, values, p);
                list.Add (p);
            }
            return 0;
        }
       
        private int readGroup(string[] lines, List<ABBR> list) {

            int group_start = find_line (lines, "\"GROUP\",\"ABBR\"");

            if (group_start==NOT_FOUND) {
                return -1;
            }

            string[] header = lines[group_start+1].QuoteSplit();
            string[] units = lines[group_start+2].QuoteSplit();
            string[] type = lines[group_start+3].QuoteSplit();

            for (int i=group_start + 4; i<lines.Count();i++) {
                string line = lines[i];
                if (line.Length==0) {
                    return i;
                }
                string[] values = line.QuoteSplit();
                ABBR p = new ABBR();
                setValues(header, values, p);
                list.Add (p);
            }
            return 0;
        }
        private int readGroup(string[] lines, List<PROJ> list) {

            int group_start = find_line (lines, "\"GROUP\",\"PROJ\"");

            if (group_start==NOT_FOUND) {
                return -1;
            }
           
            string[] header = lines[group_start+1].QuoteSplit();
            string[] units = lines[group_start+2].QuoteSplit();
            string[] type = lines[group_start+3].QuoteSplit();
            
            addUNITS(units);
            addTYPES(type);
            
            for (int i=group_start + 4; i<lines.Count();i++) {
                string line = lines[i];
                if (line.Length==0) {
                    return i;
                }
                string[] values = line.QuoteSplit();
                PROJ p = new PROJ();
                setValues(header, values, p);
                list.Add (p);
            }
            return 0;
        }
        private int readGroup(string[] lines, List<POINT> list) {

            int group_start = find_line (lines, "\"GROUP\",\"LOCA\"");

            if (group_start==NOT_FOUND) {
                return -1;
            }

            string[] header = lines[group_start+1].QuoteSplit();
            string[] units = lines[group_start+2].QuoteSplit();
            string[] type = lines[group_start+3].QuoteSplit();
            
            addUNITS(units);
            addTYPES(type);
            
            for (int i=group_start + 4; i<lines.Count();i++) {
                string line = lines[i];
                if (line.Length==0) {
                    return i;
                }
                string[] values = line.QuoteSplit();
                POINT p = new POINT();
                setValues(header, values, p);
                list.Add (p);
            }
            return 0;
        }
        private int setValues(string[] header, string[] values, PROJ p) {
         try {
            for (int i=0;i<header.Count();i++) {
                if (header[i] == "PROJ_CLNT") p.PROJ_CLNT = values[i];
                if (header[i] == "PROJ_CONT") p.PROJ_CONT = values[i];
                if (header[i] == "PROJ_ENG") p.PROJ_ENG = values[i];
                if (header[i] == "PROJ_ID") p.PROJ_ID = values[i];
                if (header[i] == "PROJ_GRID") p.PROJ_GRID = values[i];
                if (header[i] == "PROJ_LOC") p.PROJ_LOC = values[i];
                if (header[i] == "PROJ_MEMO") p.PROJ_MEMO = values[i];
                if (header[i] == "PROJ_NAME") p.PROJ_NAME = values[i];
            }
         } catch {
             return -1;
         }
         
         return 0;
     }

      private int setValues(string[] header, string[] values, POINT p) {
         try {
            for (int i=0;i<header.Count();i++) {
                if (header[i] == "LOCA_NATE" && values[i] != "") p.East = Convert.ToDouble(values[i]);
                if (header[i] == "LOCA_GL" && values[i] != "") p.Elevation = Convert.ToDouble(values[i]);
                if (header[i] == "FILE_FSET") p.FILE_FSET= values[i];
                if (header[i] == "LOCA_FDEP" && values[i]!= "") p.HoleDepth = Convert.ToDouble(values[i]);
                if (header[i] == "LOCA_ALID") p.LOCA_ALID = values[i];
                if (header[i] == "LOCA_CKBY") p.LOCA_CKBY = values[i];
                if (header[i] == "LOCA_CKDT" && values[i] != "") p.LOCA_CKDT = Convert.ToDateTime(values[i]);
                if (header[i] == "LOCA_CLST") p.LOCA_CLST = values[i];
                if (header[i] == "LOCA_CNGE") p.LOCA_CNGE = values[i];
                if (header[i] == "LOCA_DATM") p.LOCA_DATM = values[i];
                if (header[i] == "LOCA_ELAT" && values[i] != "") p.LOCA_ELAT = Convert.ToDouble(values[i]);
                if (header[i] == "LOCA_ELON" && values[i] != "") p.LOCA_ELON = Convert.ToDouble(values[i]);
                if (header[i] == "LOCA_ENDD" && values[i] != "") p.LOCA_ENDD = Convert.ToDateTime(values[i]);
                if (header[i] == "LOCA_ETRV" && values[i] != "") p.LOCA_ETRV = Convert.ToDouble(values[i]);
            }

         } catch {
             return -1;
         }
         
         return 0;
        }
        
    
      private int setValues(string[] header, string[] values, ABBR p) {
         try {
            for (int i=0;i<header.Count();i++) {
                if (header[i] == "ABBR_CODE") p.ABBR_CODE = values[i];
                if (header[i] == "ABBR_DESC") p.ABBR_DESC = values[i];
                if (header[i] == "ABBR_HDNG") p.ABBR_HDNG = values[i];
                if (header[i] == "ABBR_LIST") p.ABBR_LIST = values[i];
                if (header[i] == "ABBR_REM" ) p.ABBR_REM = values[i];
                if (header[i] == "FILE_FSET") p.FILE_FSET = values[i];
            }
         } catch {
             return -1;
         }

        return 0;
      }
        private int setValues(string[] header, string[] values, UNIT p) {
         try {
            for (int i=0;i<header.Count();i++) {
               if (header[i] == "FILE_FSET") p.FILE_FSET = values[i];
               if (header[i] == "UNIT_DESC") p.UNIT_DESC = values[i];
               if (header[i] == "UNIT_REM") p.UNIT_REM = values[i];
               if (header[i] == "UNIT_UNIT") p.UNIT_UNIT = values[i];
            }
         } catch {
             return -1;
         }
        return 0;
      }

      private int setValues(string[] header, string[] values, TYPE p) {
         try {
            for (int i=0;i<header.Count();i++) {
               if (header[i] == "FILE_FSET") p.FILE_FSET = values[i];
               if (header[i] == "TYPE_DESC") p.TYPE_DESC = values[i];
               if (header[i] == "TYPE_TYPE") p.TYPE_TYPE = values[i];
            }
         } catch {
             return -1;
         }
        return 0;
      }

      private int setValues(string[] header, string[] values, DICT p) {
         try {
            for (int i=0;i<header.Count();i++) { 
                if (header[i] == "DICT_DESC") p.DICT_DESC = values[i];
                if (header[i] == "DICT_DTYP") p.DICT_DTYP = values[i];
                if (header[i] == "DICT_HDNG") p.DICT_HDNG = values[i];
                if (header[i] == "DICT_PGRP") p.DICT_PGRP = values[i];
                if (header[i] == "DICT_REM") p.DICT_REM = values[i];
                if (header[i] == "DICT_STAT") p.DICT_STAT = values[i];
                if (header[i] == "DICT_TYPE") p.DICT_TYPE = values[i];
                if (header[i] == "DICT_UNIT") p.DICT_UNIT = values[i];
                if (header[i] == "FILE_FSET") p.FILE_FSET = values[i];
            }
         } catch {
             return -1;
         }
         
         return 0;
        }
    }
}


