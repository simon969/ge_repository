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
using ge_repository.Services;
using Newtonsoft.Json;

namespace ge_repository.Controllers
{

     public class ge_LTCController: ge_Controller  {     
     private static string DATE_FORMAT = "yyyy-MM-dd hh:mm:ss";
     private static string FILE_NAME_DATE_FORMAT = "yyyy_MM_dd";
     private static string DATE_FORMAT_AGS = "yyyy-MM-ddThh:mm:ss";
     private static string READING_FORMAT = "{0:#.00}";
     private static float FACTOR_Pa_To_mb = 100F;
     public List<LTM_Survey_Data> Survey_Data {get;set;}
     public List<MOND> MOND {get;set;}
     public List<MONG> MONG {get;set;}
     public List<MONV> MONV {get; set;}
     public List<POINT> POINT {get;set;} 
         public ge_LTCController(

            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,  
            IHostingEnvironment env ,
            IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
           
        }
public async Task<IActionResult> ViewFeature(Guid projectId,
                                                    string table,
                                                    string where
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
            
            var t1 = await new ge_esriController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getFeatures(projectId,
                                                                table,
                                                                where
                                                                );
            return Json(t1);

}
public async Task<IActionResult> ReadFeature( Guid projectId,
                                                    string dataset,
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

            var t1 = await new ge_esriController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getFeatures(projectId,
                                                                table1,
                                                                where
                                                                );
            var survey_data  = JsonConvert.DeserializeObject<esriFeature<LTM_Survey_Data>>( (string) t1.Value);

            var t2 = await new ge_esriController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getFeatures(projectId,
                                                                table2,
                                                                where
                                                                );
            var survey_repeat  = JsonConvert.DeserializeObject<esriFeature<LTM_Survey_Data_Repeat>>((string) t2.Value);
            
            var mond_resp = await ReadFeature (survey_data.features, survey_repeat.features, _project);

            ViewData["FeatureStatus"] = "Features not written to MOND table";
            
            if (save == true) {

               var existingMOND = await new ge_gINTController (_context,
                                                   _authorizationService,
                                                   _userManager,
                                                   _env ,
                                                   _ge_config
                                                       ).getMOND (_project.Id,"ge_source like '%esri%'");
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
               var saveMOND_resp = await new ge_gINTController (_context,
                                                   _authorizationService,
                                                   _userManager,
                                                   _env ,
                                                   _ge_config
                                                       ).UploadMOND (_project.Id, MOND,"esri");

                ViewData["FeatureStatus"] = $"Features attributes({saveMOND_resp}) written to MOND table";
                var saveMONV_resp = await new ge_gINTController (_context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config
                                                        ).UploadMONV (_project.Id, MONV);
                ViewData["FeatureStatus"] = $"Features attributes({saveMONV_resp}) written to MONV table";
            }  
            
            List<MOND> ordered = MOND.OrderBy(e=>e.DateTime).ToList();
            MOND = ordered;
            
            if (format=="json") {
            return Json(MOND);
            }
            
            return View("ViewMOND", MOND);
            



 }
 private List<MOND> getMONDForDeletion(List<MOND> existingMOND, List<MOND> newMOND ) {
     List<MOND> deleteMOND = new List<MOND>();
     
     foreach (MOND existM in existingMOND) {
        var newM = newMOND.Where(m=>m.PointID == existM.PointID &&
                                m.MONG_DIS == existM.MONG_DIS &&
                                m.DateTime == existM.DateTime &&
                                m.MOND_TYPE == existM.MOND_TYPE);
        if (newM==null) {
            deleteMOND.Add (existM);
        }

     }
    
    if (deleteMOND.Count()>0) {
        return deleteMOND;
    }

    return null;
 }
 
private async Task<int> ReadFeature(List<items<LTM_Survey_Data>>  survey_data, 
                                         List<items<LTM_Survey_Data_Repeat>> survey_data_repeat,  
                                         ge_project project) {
            string[] AllPoints = new string[] {""};

            MONG = await new ge_gINTController (_context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config
                                                        ).getMONG(project.Id,AllPoints);
            POINT = await new ge_gINTController (_context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config
                                                        ).getPOINT(project.Id,AllPoints);

            MOND = new List<MOND>();
            MONV = new List<MONV>(); 
            
            int projectId = POINT.FirstOrDefault().gINTProjectID;

            Survey_Data = new List<LTM_Survey_Data>();

        foreach (items<LTM_Survey_Data> survey_items in survey_data) {
            
            LTM_Survey_Data survey = survey_items.attributes;
            
            if (survey==null) {
               continue; 
            }
            
            Survey_Data.Add (survey);
          
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
      
            DateTime? survey_start = survey.date1_getDT();

            if (survey_start==null) continue;

            DateTime survey_startDT = survey.time1_getDT().Value;
            DateTime survey_endDT = survey.time2_getDT().Value;

            MONV mv = NewMONV(pt,survey);
               
                mv.MONV_STAR =  survey_startDT;
                mv.MONV_ENDD = survey_endDT;
                
                mv.MONV_DIPR = survey.dip_req;
                mv.MONV_GASR = survey.gas_mon;
                mv.MONV_LOGR = survey.logger_downld_req ;

                //mv.MONV_DIPR = survey.dip_fail;
                
               // mv.MONV_LOGR = survey.logger_fail;
                mv.MONV_REMG = survey.gas_fail + " " + survey.gas_com;
                mv.MONV_REMD = survey.dip_fail + " " + survey.dip_com;
                mv.MONV_REML = survey.logger_fail + " " + survey.logger_com;
                // if (mv.MONV_REMG.Length > 255 || mv.MONV_REMD.Length>255 || mv.MONV_REML.Length>255) {
                //     Console.Write(mv.MONV_REMG);
                //        Console.Write(mv.MONV_REMD);
                //           Console.Write(mv.MONV_REML);
                // }   
                mv.MONV_WEAT = survey.weath;
                mv.MONV_TEMP = survey.temp;
                mv.MONV_WIND = survey.wind;
                
                mv.DIP_SRLN = survey.dip_instr;
                mv.DIP_CLBD = survey.dip_cali_d_getDT();
                
                mv.FLO_SRLN = survey.purg_meter;
                mv.FLO_CLBD = survey.purg_meter_cali_d_getDT();
                
                mv.GAS_SRLN = survey.gas_instr;
                mv.GAS_CLBD = survey.gas_cali_d_getDT();
                
                mv.PID_SRLN = survey.PID_instr;
                mv.PID_CLBD = survey.PID_cali_d_getDT();
                
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
            
            // Water depth below gl (m)
            if (survey.dip_check == "dry") {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "WDEP";
                md.MOND_RDNG = "Dry";
                md.MOND_UNIT = "m";
                md.MOND_INST = "Dipmeter:" + survey.dip_instr;
                md.MOND_REM = survey.dip_com;
                MOND.Add(md);
            }
            
            // Water depth below gl (m)
            if (survey.depth_gwl_bgl == null && survey.dip_req == "yes" && survey.dip_datum_offset!=null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "WDEP";
                md.MOND_RDNG = "Dry";
                md.MOND_UNIT = "m";
                md.MOND_INST = "Dipmeter:" + survey.dip_instr;
                md.MOND_REM = survey.dip_com;
                MOND.Add(md);
            }
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

            // Electrical Conductivity
            if (survey.conductivity != null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "EC";
                md.MOND_RDNG = Convert.ToString(survey.conductivity);
                md.MOND_UNIT = "uS/cm";
                md.MOND_INST = survey.gas_instr;
                MOND.Add(md);
            }

            //Temperature (°C)
            if (survey.temperature != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DOWNTEMP";
                md.MOND_RDNG = Convert.ToString(survey.temperature);
                md.MOND_UNIT = "Deg C";
                md.MOND_INST = survey.gas_instr;
                MOND.Add(md);
            }

            //Dissolved oxygen (mg/l)
            if (survey.dissolved_oxy != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DO";
                md.MOND_RDNG = Convert.ToString(survey.dissolved_oxy);
                md.MOND_UNIT = "mg/l";
                md.MOND_INST = survey.gas_instr;
                MOND.Add(md);
            }

            // Atmosperic Temperature (°C)
            if (survey.atmo_temp != null ) {
               MOND md = NewMOND(mg, survey);  
                md.MOND_TYPE = "TEMP";
                md.MOND_RDNG = Convert.ToString(survey.atmo_temp);
                md.MOND_UNIT = "Deg C";
                md.MOND_INST = survey.gas_instr;
                MOND.Add(md);
            }

            // Atmospheric pressure (mbar)
            if (survey.atmo_pressure != null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "BAR";
                md.MOND_RDNG = Convert.ToString(survey.atmo_pressure);
                md.MOND_UNIT = "mbar";
                md.MOND_INST = survey.gas_instr;
                MOND.Add(md);
            }

           // Differential borehole pressure (mbar)
            if (survey.Diff_BH_pressure != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GPRS";
                md.MOND_RDNG = Convert.ToString(survey.Diff_BH_pressure);
                md.MOND_UNIT = "mbar";
                md.MOND_INST = survey.gas_instr;
                MOND.Add(md);
            }
            // Depth to base of installation (m)
            if (survey.depth_install_bgl != null && survey.dip_datum_offset !=null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DBSE";
                md.MOND_RDNG = Convert.ToString(survey.depth_install_bgl);
                md.MOND_UNIT = "m";
                md.MOND_INST = survey.dip_instr;
                MOND.Add(md);
            }
          
            // Peak Gas flow
            if (survey.gas_flow_peak != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GFLOP";
                md.MOND_RDNG = Convert.ToString(survey.gas_flow_peak);
                md.MOND_UNIT = "l/h";
                md.MOND_INST = survey.dip_instr;
                MOND.Add(md);
            }

            // Steady Gas flow
            if (survey.gas_flow_steady != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GFLOS";
                md.MOND_RDNG = Convert.ToString(survey.gas_flow_steady);
                md.MOND_UNIT = "l/h";
                md.MOND_INST = survey.dip_instr;
                MOND.Add(md);
            }

            List<items<LTM_Survey_Data_Repeat>> repeat = survey_data_repeat.FindAll(r=>r.attributes.parentglobalid==survey.globalid); 
          
            foreach (items<LTM_Survey_Data_Repeat> survey2_items in repeat) {
                
                LTM_Survey_Data_Repeat survey2 = survey2_items.attributes;
                
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
                    md.DateTime = dt ;
                    MOND.Add(md);  
                }
                
                if (mg.PointID == "BH4019") {
                    Console.Write (survey2);
                }

                // Methane reading Limit CH4 LEL (%)
                if (survey2.CH4_lel_t != null) {
                     MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "LEL";
                    md.MOND_RDNG = Convert.ToString(survey2.CH4_lel_t);
                    md.MOND_NAME = "LEL (Lower Explosive Limit)";
                    md.MOND_UNIT = "%vol";
                    md.MOND_INST = survey.gas_instr;
                    md.DateTime = dt ;
                    MOND.Add(md);
                }

                // Methane reading CH4 (% v/v)
                if (survey2.CH4_t != null) {
                      MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "TGM";
                    md.MOND_RDNG = Convert.ToString(survey2.CH4_t);
                    md.MOND_NAME = "Methane";
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
                    md.DateTime = dt ;
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
    }

    return MOND.Count();
     
}

private MOND NewMOND (MONG mg, LTM_Survey_Data survey, LTM_Survey_Data_Repeat repeat) {
        
        int round = convertToInt16(survey.mon_rd_nb,"R",-999);

        MOND md = new MOND {
                        ge_source ="esri_survey_repeat",
                        ge_otherid = Convert.ToString(repeat.globalid),
                        gINTProjectID = mg.gINTProjectID,
                        PointID = mg.PointID,
                        ItemKey = mg.ItemKey,
                        MONG_DIS = mg.MONG_DIS,
                        RND_REF = survey.mon_rd_nb,
                        MOND_REF = String.Format("Round {0:00} Seconds {1:00}" ,round,repeat.elapse_t),
                        };
        return md;    
 }
 
 private int convertToInt16( string numericStr,
        string Remove, int ErrValue )
    {
        String s1="";

        if (!String.IsNullOrEmpty(Remove)) {
        s1 = numericStr.Replace(Remove,"");
        } else {
        s1 = numericStr;
        }

        try
        {
            return  Convert.ToInt16( s1 );
        }
        catch( Exception ex )
        {
            return ErrValue;
        }

}
 private MONV NewMONV (POINT pt, LTM_Survey_Data survey) {
      
        int round = convertToInt16(survey.mon_rd_nb,"R", -999);
        
        MONV mv = new MONV {
                        ge_source ="esri_survey",
                        ge_otherid = Convert.ToString(survey.globalid),
                        gINTProjectID = pt.gINTProjectID,
                        PointID = pt.PointID,
                        DateTime = survey.time1_getDT(),
                        RND_REF = survey.mon_rd_nb,
                        MONV_REF = String.Format("Round {0:00}", round)
                        };
        return mv;    
 }
private MOND NewMOND (MONG mg, LTM_Survey_Data survey ) {
        
        int round = convertToInt16(survey.mon_rd_nb,"R",-999);

        MOND md = new MOND {
                        ge_source ="esri_survey",
                        ge_otherid = Convert.ToString(survey.globalid),
                        gINTProjectID = mg.gINTProjectID,
                        PointID = mg.PointID,
                        ItemKey = mg.ItemKey,
                        MONG_DIS = mg.MONG_DIS,
                        RND_REF = survey.mon_rd_nb,
                        MOND_REF = String.Format("Round {0:00}", round),
                        DateTime =  survey.time1_getDT()
                        };
        return md;    

 }
  private void DeleteAllEsriDataInMOND(Guid Id) {

 }  
 
 }


}

