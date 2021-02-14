using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
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
using ge_repository.LowerThamesCrossing;
using ge_repository.services;
using Newtonsoft.Json;

namespace ge_repository.Controllers
{
    public class ge_LTCController: _ge_LTCController  {    
        public List<EsriGeometry> Survey_Geom;
        public List<LTM_Survey_Data> Survey_Data;
        public List<LTM_Survey_Data_Add> Survey_Data_Add;
       
        public List<LTM_Survey_Data_Repeat> Survey_Repeat_Data;

        // public Guid[] IgnoreDataRepeat_GlobalId =  new Guid[]  {new Guid("bf8e8e5f-7394-4363-bfc2-9bfe876b048a"),
        //                                                         new Guid("ec65345c-72fb-4c38-aca9-ca6c6c770f82"),
        //                                                         new Guid("ec65345c-72fb-4c38-aca9-ca6c6c770f82"),
        //                                                         new Guid("9d515e60-8fd8-4102-83ff-dfae6701d5e6"),
        //                                                         new Guid("c44f4585-53bb-4ac4-b9d5-c96807db3ad2"),
        //                                                         new Guid("be08d83a-d962-4b70-a7bb-ea859afded55"),
        //                                                         new Guid("64e1335e-cd49-48d0-88e0-36d4c0100330"),
        //                                                         new Guid("a4438ad3-9e87-4afd-851d-74c4e05e1da2"),
        //                                                         new Guid("0c2fd345-b0c4-488e-950f-a79cfaa0328a"),
        //                                                         new Guid("1ac157f9-9b6c-4800-ab53-9b30052be81d"),
        //                                                         new Guid("b46e37b5-4b8b-4f4b-91af-67a29b8b2c14"),
        //                                                         new Guid("bb2b4acc-5f9d-4e16-aaeb-974336626bf5"),
        //                                                         new Guid("b8b99893-87d5-425c-9bd0-437510049873"),
        //                                                         new Guid("4903b36b-0d2a-4237-b86d-03b15118f038")
        //                                                         };
         public ge_LTCController(

            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,  
            IHostingEnvironment env ,
            IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
           
        }
[AllowAnonymous]   
public async Task<IActionResult> ViewFeature(Guid projectId,
                                                    string table,
                                                    string where,
                                                    string addId = "",
                                                    string format ="json"
                                                   ) {
            if (projectId == null)
            {
                return NotFound();
            }

            if (String.IsNullOrEmpty(table)) {
                return NotFound();
            }

            var _project = await _context.ge_project
                                        .Include(p=>p.group)
                                    .SingleOrDefaultAsync(m => m.Id == projectId);
            
            if (_project == null)
            {
                return NotFound();
            }

            if (_project.esriConnectId==null) {
                return RedirectToPageMessage (msgCODE.ESRI_NO_VALID_CONNECTION);
            }
           
            ge_data _data = new ge_data();
            _data.project = _project;

             var user = GetUserAsync().Result;;
            
            if (user != null) {
                    int IsDownloadAllowed = _context.IsOperationAllowed(Constants.DownloadOperationName, _project.group, _project, _data);
                    Boolean CanUserDownload = _context.DoesUserHaveOperation(Constants.DownloadOperationName,_project,user.Id);
                    
                    int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, _project.group, _project, _data);
                    Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName,_project,user.Id);

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
            
            var t1 = await new ge_esriController(   _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getFeatures(projectId,
                                                                table,
                                                                where
                                                                );
            
            if (addId!="") {
                Guid Id = new Guid (addId);
                var FeatureAdd = await new ge_dataController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getDataAsClass<Additional_LTM_Survey_Data>(Id,"json");
                if (FeatureAdd!=null) {
                Survey_Data_Add = FeatureAdd.Feature_Adds;
                }
            }

            if (table == "LTM_Survey_Data_R05") {
                foreach (string s1 in (string[]) t1.Value) {
                    var survey_data  = JsonConvert.DeserializeObject<esriFeature<LTM_Survey_Data>>(s1);
                        if (survey_data.features==null) {
                            return NotFound();
                        }
                    await LoadFeature(survey_data.features); 
                }
                Merge_Survey_Data_Add();
                if (format=="xml") {
                 string xml = XmlSerializeToString<LTM_Survey_Data_Add>(Survey_Data_Add);
                 return new XmlActionResult("<root>" + xml + "</root>");
                }
                return Json(Survey_Data_Add);
            } 

            if (table == "Geometry") {
                foreach (string s1 in (string[]) t1.Value) {
                var survey_data  = JsonConvert.DeserializeObject<esriFeature<LTM_Survey_Data>>(s1);
                    if (survey_data.features==null) {
                    return NotFound();
                    }
                    await LoadFeature(survey_data.features); 
                }
                if (format=="xml") {
                 string xml = XmlSerializeToString<EsriGeometry>(Survey_Geom);
                 return new XmlActionResult("<root>" + xml + "</root>");
                }
                return Json(Survey_Geom);
            } 
            if (table == "gas_repeat_R05") {
                foreach (string s1 in (string[]) t1.Value) {
                    var survey_repeat  = JsonConvert.DeserializeObject<esriFeature<LTM_Survey_Data_Repeat>>(s1);
                    if (survey_repeat.features==null) {
                    return NotFound();
                    }
                    await LoadFeature(survey_repeat.features);
                }
                if (format=="xml") {
                 string xml = XmlSerializeToString<LTM_Survey_Data_Repeat>(Survey_Repeat_Data);
                 return new XmlActionResult("<root>" + xml + "</root>");
                }
                var output = JsonConvert.SerializeObject(Survey_Repeat_Data);
                return Ok(output);
            } 
                    
            return NotFound();

}
private async Task<int> LoadFeature (List<items<LTM_Survey_Data>>  survey_data) {
        
        if (Survey_Data == null) Survey_Data = new List<LTM_Survey_Data>();
        if (Survey_Geom == null) Survey_Geom = new List<EsriGeometry>();
        
        foreach (items<LTM_Survey_Data> survey_items in survey_data) {
            
            LTM_Survey_Data survey = survey_items.attributes;
            EsriGeometry geom =  survey_items.geometry;

            if (survey==null) {
               continue; 
            }
            
            Survey_Data.Add (survey);
            Survey_Geom.Add (geom);
        }

        return Survey_Data.Count();
}

private  async Task<int> LoadFeature (List<items<LTM_Survey_Data_Repeat>> survey_data_repeat) {
        
           if (Survey_Repeat_Data == null) Survey_Repeat_Data = new List<LTM_Survey_Data_Repeat>();
            
            foreach (items<LTM_Survey_Data_Repeat> repeat_items in survey_data_repeat) {
                
                LTM_Survey_Data_Repeat survey2 = repeat_items.attributes;
                
                if (survey2 == null) {
                    continue; 
                }

                Survey_Repeat_Data.Add (survey2);
            }

        return Survey_Repeat_Data.Count();
}

public async Task<IActionResult> ReadFeature( Guid projectId,
                                                    string dataset,
                                                    string addId = "",
                                                    string format = "view", 
                                                    Boolean save = false ) 
 {

            if (projectId == null)
            {
                return NotFound();
            }

            if (String.IsNullOrEmpty(dataset)) {

                return NotFound();
            }

            var _project = await _context.ge_project
                                        .Include(p=>p.group)
                                    .SingleOrDefaultAsync(m => m.Id == projectId);
            
            if (_project == null)
            {
                return NotFound();
            }

            if (_project.esriConnectId==null) {
                return RedirectToPageMessage (msgCODE.ESRI_NO_VALID_CONNECTION);
            }
           
            ge_data _data = new ge_data();
            _data.project = _project;

            var user = GetUserAsync().Result;
            
            if (user != null) {
                    int IsDownloadAllowed = _context.IsOperationAllowed(Constants.DownloadOperationName, _project.group, _project, _data);
                    Boolean CanUserDownload = _context.DoesUserHaveOperation(Constants.DownloadOperationName,_project,user.Id);
                    
                    int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, _project.group, _project, _data);
                    Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName,_project,user.Id);

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
            

            var cs = await new ge_dataController(  _context,
                                                        _authorizationService,
                                                        _userManager,
                                                        _env ,
                                                        _ge_config).getDataAsClass<EsriConnectionSettings>(_project.esriConnectId.Value); 
            
            if (cs == null) {
                   return RedirectToPageMessage (msgCODE.ESRI_NO_VALID_CONNECTION);
            }

            EsriDataSet ds = cs.datasets.Find(d=>d.Name==dataset);

            String table1 = ds.tables[0];
            String table2 = ds.tables[1];
            string where = "";
            int page_size = 250;
            var t1 = await new ge_esriController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getFeatures(projectId,
                                                                table1,
                                                                where,
                                                                page_size
                                                                );
            
            Survey_Data = new List<LTM_Survey_Data>();
            Survey_Geom = new List<EsriGeometry>();
            
            foreach (string s1 in (string[]) t1.Value) {
            var survey_data  = JsonConvert.DeserializeObject<esriFeature<LTM_Survey_Data>>(s1);
            var survey_resp = LoadFeature(survey_data.features);
            }


            var t2 = await new ge_esriController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getFeatures(projectId,
                                                                table2,
                                                                where,
                                                                page_size
                                                                );
            Survey_Repeat_Data = new List<LTM_Survey_Data_Repeat>();
            
            foreach (string s1 in (string[]) t2.Value) {
            var survey_repeat  = JsonConvert.DeserializeObject<esriFeature<LTM_Survey_Data_Repeat>>(s1);
            var repeat_resp = LoadFeature(survey_repeat.features);
            }

            if (addId!="") {
                Guid Id = new Guid (addId);
                var FeatureAdd = await new ge_dataController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getDataAsClass<Additional_LTM_Survey_Data>(Id,"json");
                if (FeatureAdd!=null) {
                Survey_Data_Add = FeatureAdd.Feature_Adds;
                }
            }

            Merge_Survey_Data_Add();
            
            var mond_resp = await AddMOND(_project);

            ViewData["FeatureStatus"] = "Features not written to MOND table";
            
            if (save == true) {
                List<MOND> existingMOND;
                var resp = await new ge_gINTController (_context,
                                                   _authorizationService,
                                                   _userManager,
                                                   _env ,
                                                   _ge_config
                                                       ).getMOND (_project.Id,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    "ge_source in ('esri_survey','esri_survey_repeat')",
                                                                    "");
                var okResult = resp as OkObjectResult;   
                if (okResult.StatusCode==200) {
                    existingMOND = okResult.Value as List<MOND>;
                    if (existingMOND!=null) {
                    var deleteMOND =  getMONDForDeletion(existingMOND, MOND);
                        if (deleteMOND!=null) {
                        int[] s = deleteMOND.Select (m=>m.GintRecID).ToArray();
                            var delMOND_resp = await new ge_gINTController (_context,
                                                   _authorizationService,
                                                   _userManager,
                                                   _env ,
                                                   _ge_config
                                                       ).deleteMOND(_project.Id, s);
                        }
                    }
                }
                
               var saveMOND_resp = await new ge_gINTController (_context,
                                                   _authorizationService,
                                                   _userManager,
                                                   _env ,
                                                   _ge_config
                                                       ).Upload (_project.Id, MOND,"ge_source in ('esri_survey','esri_survey_repeat')");

                ViewData["FeatureStatus"] = $"Features attributes({saveMOND_resp}) written to MOND table";
                var saveMONV_resp = await new ge_gINTController (_context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config
                                                        ).Upload (_project.Id, MONV);
                ViewData["FeatureStatus"] = $"Features attributes({saveMONV_resp}) written to MONV table";
            }  
            
            List<MOND> ordered = MOND.OrderBy(e=>e.DateTime).ToList();
            MOND = ordered;
            
            if (format=="json") {
            return Json(MOND);
            }
            
            return View("ViewMOND", MOND);
            



 }
private void Merge_Survey_Data_Add(){
        
        List<LTM_Survey_Data_Add> data_add =  new List<LTM_Survey_Data_Add>();

        foreach (LTM_Survey_Data df in Survey_Data) {

            LTM_Survey_Data_Add da_new = new LTM_Survey_Data_Add(df);

            if (Survey_Data_Add!=null) {
                LTM_Survey_Data_Add da =  Survey_Data_Add.FindLast(d=>d.globalid==df.globalid);
                if (da!=null) {
                    da_new.QA_status =da.QA_status;
                }
            }

            data_add.Add (da_new);
        }

        Survey_Data_Add = data_add;

}
 
private async Task<int> AddMOND(ge_project project) {
            string[] AllPoints = new string[] {""};

            var resp = await new ge_gINTController (_context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config
                                                        ).getMONG(project.Id,AllPoints);
            var okResult = resp as OkObjectResult;   
                if (okResult.StatusCode!=200) {
                return -1;
                } 
        
            MONG = okResult.Value as List<MONG>;
                if (MONG==null) { 
                return -1;
                }
            
            resp = await new ge_gINTController (_context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config
                                                        ).getPOINT(project.Id,AllPoints);
            okResult = resp as OkObjectResult;   
                if (okResult.StatusCode!=200) {
                return -1;
                } 
        
            POINT = okResult.Value as List<POINT>;
                if (POINT==null) { 
                return -1;
                }

            MOND = new List<MOND>();
            MONV = new List<MONV>(); 
            
            int projectId = POINT.FirstOrDefault().gINTProjectID;

            foreach (LTM_Survey_Data_Add survey in Survey_Data_Add) {
            
            if (survey==null) {
               continue; 
            }
            
            POINT pt = POINT.Find(p=>p.PointID==survey.hole_id);
            
            if (pt==null) {
                if ((survey.package.Contains("A") && projectId==1) || 
                    (survey.package.Contains("B") && projectId==2) ||
                    (survey.package.Contains("C") && projectId==3) ||
                    (survey.package.Contains("D") && projectId==4)) {
                Console.WriteLine ("Package {0} survey.hole_id [{1}] not found", survey.package, survey.hole_id);
                } 
                continue; 
               
            }

            List<MONG> pt_mg = MONG.Where(m=>m.PointID==survey.hole_id).ToList();

            if (pt_mg.Count == 0) {
                Console.WriteLine ("Package {0} monitoring point for survey.hole_id {1} not found", survey.package, survey.hole_id);
                continue;
            }

            MONG mg =   pt_mg.Find(m=>m.ItemKey==survey.mong_ID);
            
            if (mg == null) {
                mg = pt_mg.FirstOrDefault();
            } 
      
            DateTime? survey_start = gINTDateTime(survey.date1_getDT());
            
            if (survey_start==null) continue;

          

            DateTime survey_startDT = gINTDateTime(survey.time1_getDT()).Value;
            DateTime survey_endDT = gINTDateTime(survey.time2_getDT()).Value;

            // if (survey.globalid == new Guid("bf8e8e5f-7394-4363-bfc2-9bfe876b048a")) {
            //     Console.WriteLine ("{0}       {1}", survey_startDT, survey.time1_getDT());
            // }

            MONV mv = NewMONV(pt,survey);
               
                mv.MONV_STAR = survey_startDT;
                mv.MONV_ENDD = survey_endDT;
                
                mv.MONV_DIPR = survey.dip_req;
                mv.MONV_GASR = survey.gas_mon;
                mv.MONV_LOGR = survey.logger_downld_req ;

                mv.MONV_REMG = survey.gas_fail + " " + survey.gas_com;
                mv.MONV_REMD = survey.dip_fail + " " + survey.dip_com;
                mv.MONV_REML = survey.logger_fail + " " + survey.logger_com;
                mv.MONV_REMS = survey.samp_fail + " " + survey.samp_com;
                mv.PUMP_TYPE = survey.purg_pump + " " + survey.purg_pump_oth;

                mv.MONV_WEAT = survey.weath;
                mv.MONV_TEMP = survey.temp;
                mv.MONV_WIND = survey.wind;
                
                if (survey.dip_instr!=null) {
                mv.DIP_SRLN = survey.dip_instr;
                mv.DIP_CLBD = gINTDateTime(survey.dip_cali_d_getDT());
                }          
                
                if (survey.interface_instr !=null) {
                mv.DIP_SRLN = survey.interface_instr;
                mv.DIP_CLBD = gINTDateTime(survey.interface_cali_d_getDT());
                }

                mv.FLO_SRLN = survey.purg_meter;
                mv.FLO_CLBD = gINTDateTime(survey.purg_meter_cali_d_getDT());
                
                mv.GAS_SRLN = survey.gas_instr;
                mv.GAS_CLBD = gINTDateTime(survey.gas_cali_d_getDT());
                
                mv.PID_SRLN = survey.PID_instr;
                mv.PID_CLBD = gINTDateTime(survey.PID_cali_d_getDT());
                
                mv.RND_REF = survey.mon_rd_nb;
                mv.MONV_DATM = survey.dip_datum;

                mv.AIR_PRESS = survey.atmo_pressure;
                mv.AIR_TEMP = survey.atmo_temp;
                
                mv.PIPE_DIA = survey.pipe_diam;

                if(survey.dip_datum_offset != null) {
                    mv.MONV_DIS = ((float) survey.dip_datum_offset.Value) / 100f;
                }

                mv.MONV_MENG = survey.Creator;
                MONV.Add(mv);


            
        //    if (IgnoreDataRepeat_GlobalId.Contains(survey.globalid)) {
        //        continue;
        //    }

            if (survey.QA_status.Contains("Dip_Approved")) {
                    AddDip(mg, survey);
            }
            
            if (survey.QA_status.Contains("Purge_Approved")) {
                    AddPurge(mg, survey);
            }
            
            if (survey.QA_status.Contains("Gas_Approved")) {
                    AddGas(mg, survey);
            }
          




          }
    
    return MOND.Count();
     
}
private int AddDip(MONG mg, LTM_Survey_Data survey) {

            // Water depth below gl (m)
            if (survey.depth_gwl_bgl != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "WDEP";
                md.MOND_RDNG = Convert.ToString(survey.depth_gwl_bgl);
                md.MOND_UNIT = "m";
                md.MOND_INST = "Dipmeter:" + survey.dip_instr;
                md.MOND_REM = survey.dip_com;
                MOND.Add(md);
            }
            
            // Water depth if dry
            if (survey.dip_check == "dry") {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "WDEP";
                md.MOND_RDNG = "Dry";
                md.MOND_UNIT = "m";
                md.MOND_INST = "Dipmeter:" + survey.dip_instr;
                md.MOND_REM = survey.dip_com;
                MOND.Add(md);
            }
            
            // Water depth if null and dip required and offset
            if (survey.depth_gwl_bgl == null && survey.dip_req == "yes" && survey.dip_datum_offset!=null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "WDEP";
                md.MOND_RDNG = "Dry";
                md.MOND_UNIT = "m";
                md.MOND_INST = "Dipmeter:" + survey.dip_instr;
                md.MOND_REM = survey.dip_com;
                MOND.Add(md);
            }

            
            // Depth to base of installation (m)
            if (survey.depth_install_bgl != null && survey.dip_datum_offset !=null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DBSE";
                md.MOND_RDNG = Convert.ToString(survey.depth_install_bgl);
                md.MOND_NAME = "Depth to base of installation";
                md.MOND_UNIT = "m";
                md.MOND_INST = survey.dip_instr;
                MOND.Add(md);
            }

            return 0;

}

private int AddPurge(MONG mg, LTM_Survey_Data survey) {

            // PH
            if (survey.ph != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "PH";
                md.MOND_RDNG = Convert.ToString(survey.ph);
                md.MOND_UNIT = "PH";
                md.MOND_INST = survey.gas_instr;
                MOND.Add(md);
            }

            //Redox Potential
            if (survey.redox_potential != null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "RDX";
                md.MOND_RDNG = Convert.ToString(survey.redox_potential);
                md.MOND_UNIT = "mV";
                md.MOND_INST = survey.gas_instr;
                MOND.Add(md);
            }

            //Redox Potential from eh field
            if (survey.eh!= null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "RDX";
                md.MOND_RDNG = Convert.ToString(survey.eh);
                md.MOND_NAME = "Redox Potential";
                md.MOND_UNIT = "mV";
                md.MOND_INST = survey.gas_instr;
                MOND.Add(md);
            }

            // Electrical Conductivity
            if (survey.conductivity != null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "EC";
                md.MOND_RDNG = Convert.ToString(survey.conductivity);
                md.MOND_NAME = "Electrical Conductivity";
                md.MOND_UNIT = "uS/cm";
                md.MOND_INST = survey.gas_instr;
                MOND.Add(md);
            }

            //Temperature (°C)
            if (survey.temperature != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DOWNTEMP";
                md.MOND_RDNG = Convert.ToString(survey.temperature);
                md.MOND_NAME = "Downhole temperature";
                md.MOND_UNIT = "Deg C";
                md.MOND_INST = survey.gas_instr;
                MOND.Add(md);
            }

            //Dissolved oxygen (mg/l)
            if (survey.dissolved_oxy != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DO";
                md.MOND_RDNG = Convert.ToString(survey.dissolved_oxy);
                md.MOND_NAME = "Dissolved Oxygen";
                md.MOND_UNIT = "mg/l";
                md.MOND_INST = survey.gas_instr;
                MOND.Add(md);
            }


           
    return 0;
}
private int AddGas(MONG mg, LTM_Survey_Data survey) {

            // Peak Gas flow
            if (survey.gas_flow_peak != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GFLOP";
                md.MOND_RDNG = Convert.ToString(survey.gas_flow_peak);
                md.MOND_NAME = "Peak gas flow rate";
                md.MOND_UNIT = "l/h";
                md.MOND_INST = survey.dip_instr;
                MOND.Add(md);
            }

            // Steady Gas flow
            if (survey.gas_flow_steady != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GFLOS";
                md.MOND_RDNG = Convert.ToString(survey.gas_flow_steady);
                md.MOND_NAME = "Steady gas flow rate";
                md.MOND_UNIT = "l/h";
                md.MOND_INST = survey.dip_instr;
                MOND.Add(md);
            }
                        // Atmosperic Temperature (°C)
            if (survey.atmo_temp != null ) {
               MOND md = NewMOND(mg, survey);  
                md.MOND_TYPE = "TEMP";
                md.MOND_RDNG = Convert.ToString(survey.atmo_temp);
                md.MOND_NAME = "Atmospheric temperature";
                md.MOND_UNIT = "Deg C";
                md.MOND_INST = survey.gas_instr;
                MOND.Add(md);
            }

            // Atmospheric pressure (mbar)
            if (survey.atmo_pressure != null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "BAR";
                md.MOND_RDNG = Convert.ToString(survey.atmo_pressure);
                md.MOND_NAME = "Atmospheric pressure";
                md.MOND_UNIT = "mbar";
                md.MOND_INST = survey.gas_instr;
                MOND.Add(md);
            }

           // Differential borehole pressure (mbar)
            if (survey.Diff_BH_pressure != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GPRS";
                md.MOND_RDNG = Convert.ToString(survey.Diff_BH_pressure);
                md.MOND_NAME = "Differential pressure";
                md.MOND_UNIT = "mbar";
                md.MOND_INST = survey.gas_instr;
                MOND.Add(md);
            }
            
            DateTime survey_startDT= gINTDateTime(survey.time1_getDT()).Value;
            
            List<LTM_Survey_Data_Repeat> repeat = Survey_Repeat_Data.FindAll(r=>r.parentglobalid==survey.globalid); 
          
            foreach (LTM_Survey_Data_Repeat survey2 in repeat) {
                
                              
                if (survey2.elapse_t == null) continue;
                
                int elapsed = survey2.elapse_t.Value;
                //DateTime dt = survey_start.Value.AddSeconds(elapsed);
                DateTime dt = survey_startDT.AddSeconds(elapsed);

                // Gas flow (l/h)
                if (survey2.gas_flow_t != null) {
                    MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "GFLO";
                    md.MOND_RDNG = Convert.ToString(survey2.gas_flow_t);
                    md.MOND_NAME = "Gas flow rate";
                    md.MOND_UNIT = "l/h";
                    md.MOND_INST = survey.gas_instr;
                    md.DateTime = dt; 
                    MOND.Add(md);  
                }
                
                // Methane reading Limit CH4 LEL (%)
                if (survey2.CH4_lel_t != null) {
                     MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "GM";
                    md.MOND_RDNG = Convert.ToString(survey2.CH4_lel_t);
                    md.MOND_NAME = "Methane as percentage of LEL (Lower Explosive Limit)";
                    md.MOND_UNIT = "%vol";
                    md.MOND_INST = survey.gas_instr;
                    md.DateTime = dt; 
                    MOND.Add(md);
                }

                // Methane reading CH4 (% v/v)
                if (survey2.CH4_t != null) {
                      MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "TGM";
                    md.MOND_RDNG = Convert.ToString(survey2.CH4_t);
                    md.MOND_NAME = "Total Methane";
                    md.MOND_UNIT = "%vol";
                    md.MOND_INST = survey.gas_instr;
                    md.DateTime = dt; 
                    MOND.Add(md);  
                }

                // Carbon Dioxide reading CO2 (% v/v)
                if (survey2.CO2_t != null) {
                   MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "GCD";
                    md.MOND_RDNG = Convert.ToString(survey2.CO2_t);
                    md.MOND_NAME = "Carbon Dioxide";
                    md.MOND_UNIT = "%vol";
                    md.MOND_INST = survey.gas_instr;
                    md.DateTime = dt; 
                    MOND.Add(md);  
                }

                //Oxygen Reading O2 (% v/v)
                if (survey2.O2_t != null) {
                    MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "GOX";
                    md.MOND_RDNG = Convert.ToString(survey2.O2_t);
                    md.MOND_NAME = "Oxygen";
                    md.MOND_UNIT = "%vol";
                    md.MOND_INST = survey.gas_instr;
                    md.DateTime = dt;  
                    MOND.Add(md);  
                }

                //Hydrogen Sulphide H2S (ppm)
                if (survey2.H2S_t != null) {
                    MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "HYS";
                    md.MOND_RDNG = Convert.ToString(survey2.H2S_t);
                    md.MOND_NAME = "Hydrogen Sulphide";
                    md.MOND_UNIT = "ppm";
                    md.MOND_INST = survey.gas_instr;
                    md.DateTime = dt;  
                    MOND.Add(md);  
                }

                //Carbon Monoxide Readings CO (ppm)
                if (survey2.CO_t != null) {
                    MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "GCO";
                    md.MOND_RDNG = Convert.ToString(survey2.CO_t);
                    md.MOND_NAME = "Carbon Monoxide";
                    md.MOND_UNIT = "ppm";
                    md.MOND_INST = survey.gas_instr;
                    md.DateTime = dt;  
                    MOND.Add(md);  
                }
                
                //Photo Ionisations PID (ppm)
                if (survey2.PID_t != null) {
                    MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "PID";
                    md.MOND_RDNG = Convert.ToString(survey2.PID_t);
                    md.MOND_NAME = "Photoionization Detector";
                    md.MOND_UNIT = "ppm";
                    md.MOND_INST = survey.PID_instr;
                    md.DateTime = dt; 
                    MOND.Add(md);  
                }

                // VOC (ppm)
                if (survey2.voc_t != null) {
                    MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "VOC";
                    md.MOND_RDNG = Convert.ToString(survey2.voc_t);
                    md.MOND_NAME = "Volatile Organic Compounds";
                    md.MOND_UNIT = "ppm";
                    md.MOND_INST = survey.gas_instr;
                    md.DateTime = dt; 
                    MOND.Add(md);  
                }

            }

            return 0;

}

private MOND NewMOND (MONG mg, LTM_Survey_Data survey, LTM_Survey_Data_Repeat repeat) {
        
        int round = convertToInt16(survey.mon_rd_nb,"R",-999);

        MOND md = new MOND {
                        ge_source ="esri_survey_repeat",
                        ge_otherId = Convert.ToString(repeat.globalid),
                        gINTProjectID = mg.gINTProjectID,
                        PointID = mg.PointID,
                        ItemKey = mg.ItemKey,
                        MONG_DIS = mg.MONG_DIS,
                        RND_REF = survey.mon_rd_nb,
                        MOND_REF = String.Format("Round {0:00} Seconds {1:00}" ,round,repeat.elapse_t),
                        };
        return md;    
 }
 
 
 private MONV NewMONV (POINT pt, LTM_Survey_Data survey) {
      
        int round = convertToInt16(survey.mon_rd_nb,"R", -999);
        
        MONV mv = new MONV {
                        ge_source ="esri_survey",
                        ge_otherId = Convert.ToString(survey.globalid),
                        gINTProjectID = pt.gINTProjectID,
                        PointID = pt.PointID,
                        DateTime = gINTDateTime(survey.time1_getDT()),
                        RND_REF = survey.mon_rd_nb,
                        MONV_REF = String.Format("Round {0:00}", round)
                        };
        return mv;    
 }




private MOND NewMOND (MONG mg, LTM_Survey_Data survey ) {
        
        int round = convertToInt16(survey.mon_rd_nb,"R",-999);

        MOND md = new MOND {
                        ge_source ="esri_survey",
                        ge_otherId = Convert.ToString(survey.globalid),
                        gINTProjectID = mg.gINTProjectID,
                        PointID = mg.PointID,
                        ItemKey = mg.ItemKey,
                        MONG_DIS = mg.MONG_DIS,
                        RND_REF = survey.mon_rd_nb,
                        MOND_REF = String.Format("Round {0:00}", round),
                        DateTime =  gINTDateTime(survey.time1_getDT())
                        };
        return md;    

 }
 
 }

}



