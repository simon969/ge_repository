using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.OtherDatabase;
using ge_repository.LowerThamesCrossing;
using ge_repository.services;
using ge_repository.repositories;
using ge_repository.interfaces;
using Newtonsoft.Json;
using ge_repository.DAL;
using ge_repository.Extensions;
using ge_repository.ESRI;

namespace ge_repository.Controllers
{
       public class ge_LTC2ServiceController: _ge_LTCController  {     
      
       protected readonly IServiceScopeFactory _serviceScopeFactory;
       protected readonly IDataService _dataService;
       protected readonly IUserOpsService _userService;
    
       public List<LTM_Survey_Data2> Survey_Data;
       public List<LTM_Geometry> Survey_Geom;
       public List<LTM_Survey_Data_Repeat2> Survey_Repeat_Data;
       public Dictionary<string, string> map_hole_id =  new Dictionary<string, string>();
       public Dictionary<string, string> map_mong_id =  new Dictionary<string, string>();
       public Dictionary<string, int> map_mong_id_gintrecid = new Dictionary<string, int>();
       public ge_project _project;

       public ge_LTC2ServiceController(
            ge_DbContext context,
            IServiceScopeFactory serviceScopeFactory,
            IDataService dataService,
            IUserOpsService userOpsService,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,  
            IHostingEnvironment env ,
            IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
           // LTM Package A
           map_hole_id.Add ("BH2316-S","BH2316");
           map_hole_id.Add ("BH2316-D","BH2316");
           map_mong_id.Add ("BH2316-SPipe 1","Pipe 2");
           map_mong_id.Add ("BH2316-DPipe 1","Pipe 1");
           
           // LTM Package NGGE
           map_hole_id.Add ("005-BH001 (S)","005-BH001");
           map_hole_id.Add ("005-BH002 (S)","005-BH002");
           map_hole_id.Add ("005-BH003 (S)","005-BH003");
           map_hole_id.Add ("005-BH004 (S)","005-BH004");
           map_hole_id.Add ("005-BH005 (S)","005-BH005");
           map_hole_id.Add ("005-BH006 (S)","005-BH006");
           map_hole_id.Add ("005-BH010 (S)","005-BH010");
           map_hole_id.Add ("005-BH011 (S)","005-BH011");
           map_hole_id.Add ("005-BH012 (S)","005-BH012");
           map_hole_id.Add ("005-BH013 (S)","005-BH013");
          
           // LTM Package Phase 1A Phase1B
           map_mong_id_gintrecid.Add ("BH2612Pipe 1",295);

           // LTM Package SU
           map_hole_id.Add ("CG-BH10-S","CG-BH10");
           map_hole_id.Add ("CG-BH10-D","CG-BH10"); 
           map_mong_id.Add ("CG-BH10-SShallow","2");
           map_mong_id.Add ("CG-BH10-DDeep","1");
           
           map_hole_id.Add ("CG-BH13-S","CG-BH13");
           map_hole_id.Add ("CG-BH13-D","CG-BH13"); 
           map_mong_id.Add ("CG-BH13-SShallow","2");
           map_mong_id.Add ("CG-BH13-DDeep","1");
           
           map_hole_id.Add ("CG-BH15-S","CG-BH15");
           map_hole_id.Add ("CG-BH15-D","CG-BH15"); 
           map_mong_id.Add ("CG-BH15-SShallow","2");
           map_mong_id.Add ("CG-BH15-DDeep","1");
           
           map_hole_id.Add ("CG-BH17-S","CG-BH17");
           map_hole_id.Add ("CG-BH17-D","CG-BH17"); 
           map_mong_id.Add ("CG-BH17-SShallow","2");
           map_mong_id.Add ("CG-BH17-DDeep","1");
           
           map_hole_id.Add ("CG-BH18-S","CG-BH18");
           map_hole_id.Add ("CG-BH18-D","CG-BH18"); 
           map_mong_id.Add ("CG-BH18-SShallow","2");
           map_mong_id.Add ("CG-BH18-DDeep","1");
           
           map_hole_id.Add ("CG-BH38-S","CG-BH38");
           map_hole_id.Add ("CG-BH38-D","CG-BH38"); 
           map_mong_id.Add ("CG-BH38-SShallow","2");
           map_mong_id.Add ("CG-BH38-DDeep","1");

            
            _serviceScopeFactory = serviceScopeFactory;
            _dataService = dataService;
            _userService = userOpsService;
        }

public async Task<IActionResult> ViewSurvey123 (Guid projectId,
                                                    string hole_id)
                                                {
    string where  = "";
    string format = "xml";
    string table  = "LTM_Survey_Data_R06";

    if (!String.IsNullOrEmpty(hole_id)) {
     //   where = "hole_id='" + hole_id + "'";
        where = "hole_id like '%" + hole_id + "%'";
    }

   


    return await ViewFeature (projectId, table, where, format);

}
[HttpPost]
public async Task<IActionResult> UpdateSurvey123 (Guid projectId,
                                                    Guid globalid,
                                                    string QA_status) {
  
    string where  = "globalid='" + globalid + "'";
    string format = "json";
    string table  = "LTM_Survey_Data_R06";
    DateTime localDate = DateTime.Now;
    var user = await GetUserAsync();
    string editor = user.Email;

    var resp = await ViewFeature (projectId, table, where, format);
    
    if (Survey_Data!=null) {
        if (Survey_Data[0].globalid == globalid) {
            Survey_Data[0].QA_status = QA_status;
            Survey_Data[0].EditDate_setDT(localDate);
            Survey_Data[0].Editor = editor;
            return await UpdateFeature(projectId, Survey_Data, Survey_Geom);
        }
    }

    return BadRequest("No Survey123 records returned for updating");

}

[AllowAnonymous]   
public async Task<IActionResult> ViewFeature(Guid projectId,
                                                    string table,
                                                    string where,
                                                    string format ="json"
                                                   ) {
            Boolean GeometryOnly = false;

            if (table.Contains(",GeometryOnly")) {
                GeometryOnly = true;
                table = table.Replace(",GeometryOnly","");
            }

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
           
            var user = GetUserAsync().Result;
            
            if (user==null) {
              return NotFound();
            } 
            
            var _OperationRequest = await _userService.GetOperationRequest(user.Id, _project);
            
            if (_OperationRequest.AreProjectOperationsAllowed("Download;Create") == false) {
                if (format == "view") {
                    return View ("OperationRequest",_OperationRequest);
                } 
                return NotFound();
            }

            if (_project.otherDbConnectId==null) {
                return NotFound();
            }

            var cs = await _dataService.GetEsriConnectionSettingsByProjectId(_project.otherDbConnectId.Value);

            LTCUnitOfWork uw = new LTCUnitOfWork(cs, "LTM_Survey_Data_R06", "gas_repeat_R06");
            
            ILTCEsriService _esriService = new LTCEsriService(uw);

            var t2 = await _esriService.ReadFeaturesToMONDList(where);
            
            var t3 = await _esriService.ReadParentFeatureToJson(where);

            var t1 = await new ge_esriController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getFeatures(projectId,
                                                                table,
                                                                where
                                                                );
            if (GeometryOnly==true) {
                foreach (string s1 in (string[]) t1.Value) {
                var survey_data  = JsonConvert.DeserializeObject<esriFeatureLayer<LTM_Survey_Data2>>(s1);
                    if (survey_data.features==null) {
                    return NotFound();
                    }
                    await LoadFeature(survey_data.features); 
                }
                if (format=="xml") {
                 string xml = XmlSerializeToString<LTM_Geometry>(Survey_Geom);
                 return new XmlActionResult("<root>" + xml + "</root>");
                }
                return Json(Survey_Geom);
            } 
            
            if (table == "LTM_Survey_Data_R06") {
                foreach (string s1 in (string[]) t1.Value) {
                    var survey_data  = JsonConvert.DeserializeObject<esriFeature<LTM_Survey_Data2>>(s1);
                        if (survey_data.features==null) {
                            return NotFound();
                        }
                    await LoadFeature(survey_data.features); 
                }
                if (format=="xml") {
                 string xml = XmlSerializeToString<LTM_Survey_Data2>(Survey_Data);
                 return new XmlActionResult("<root>" + xml + "</root>");
                }
                return Json(Survey_Data);
            } 
           
            if (table == "gas_repeat_R06") {
                foreach (string s1 in (string[]) t1.Value) {
                    var survey_repeat  = JsonConvert.DeserializeObject<esriFeature<LTM_Survey_Data_Repeat2>>(s1);
                    if (survey_repeat.features==null) {
                    return NotFound();
                    }
                    await LoadFeature(survey_repeat.features);
                }
                if (format=="xml") {
                 string xml = XmlSerializeToString<LTM_Survey_Data_Repeat2>(Survey_Repeat_Data);
                 return new XmlActionResult("<root>" + xml + "</root>");
                }
                var output = JsonConvert.SerializeObject(Survey_Repeat_Data);
                return Ok(output);
            } 
                    
            return NotFound();
}
private async Task<IActionResult> UpdateMONG(List<MONG> list) {
                var saveMONG_resp = await new ge_gINTController (_context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config
                                                        ).Upload (_project.Id, list);
                ViewData["FeatureStatus"] = $"Features attributes({saveMONG_resp}) written to MONG table";
                
                return View (list);
}
private async Task<IActionResult> UpdateMONV(List<MONV> list) {
                var saveMONV_resp = await new ge_gINTController (_context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config
                                                        ).Upload (_project.Id, list);
                ViewData["FeatureStatus"] = $"Features attributes({saveMONV_resp}) written to MONV table";
                
                return View (list);
}
private async Task<IActionResult> UpdateMOND(List<MOND> list) {

                string[] selectOtherId= list.Select (m=>m.ge_otherId).Distinct().ToArray();
                
                string where2 = $"ge_source in ('esri_survey2','esri_survey2_repeat') and ge_otherid in ({selectOtherId.ToDelimString(",","'")})";
                List<MOND> existingMOND; 
                var resp = await new ge_gINTController ( _context,
                                                        _authorizationService,
                                                        _userManager,
                                                        _env ,
                                                        _ge_config
                                                       ).getMOND (  _project.Id,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    where2,
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
                                                       ).Upload (_project.Id, list, where2);

                // ViewData["FeatureStatus"] = $"Features attributes({saveMOND_resp}) written to MOND table";

               return Ok(saveMOND_resp);
                
}    
 [HttpPost]
 public async Task<IActionResult> PostReadFeature( Guid projectId,
                                                    string dataset,
                                                    string where = "",
                                                    int page_size = 250,
                                                    string pages = "",
                                                    int orderby = Esri.OrderBy.None,
                                                    string format = "view", 
                                                    Boolean save = false ) {
          // https://webmasters.stackexchange.com/questions/109117/usage-of-comma-in-url-encoded-or-not-encoded
            int[] pages2 = null;
            
            if (!String.IsNullOrEmpty(pages)) {
                pages2 = Array.ConvertAll<string, int>(pages.Split(','), Convert.ToInt32);
            } 
            
            return await ReadFeature (projectId,
                                dataset,
                                where,
                                page_size,
                                pages2,
                                orderby,
                                format, 
                                save);
}
public async Task<IActionResult> ReadFeature( Guid projectId,
                                                    string dataset,
                                                    string where = "",
                                                    int page_size = 250,
                                                    int[] pages = null,
                                                    int orderby = Esri.OrderBy.None,
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

            _project = await _context.ge_project
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
            
           
            
            var t1 = await new ge_esriController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getFeatures(projectId,
                                                                table1,
                                                                where,
                                                                page_size,
                                                                pages,
                                                                orderby
                                                                );
            //Initialise list containers
            if (t1.Value==null) {
                return  NotFound();
            }
            
            List<MOND> MOND_All = new List<MOND>();
            List<MONV> MONV_All =  new List<MONV>();

            foreach (string s1 in (string[]) t1.Value) {
                if (s1 == null) { 
                    continue;
                }
                
                Survey_Data = new List<LTM_Survey_Data2>();
                Survey_Geom = new List<LTM_Geometry>();
                Survey_Repeat_Data = new List<LTM_Survey_Data_Repeat2>();
                
                var survey_data  = JsonConvert.DeserializeObject<esriFeature<LTM_Survey_Data2>>(s1);

                var survey_resp = LoadFeature(survey_data.features);
            
                Guid[] globalid = survey_data.features.Select (m=>m.attributes.globalid).Distinct().ToArray();
                // if (globalid.Count()>100 ) {
                //     return Json($"The parent record count({globalid.Count()}) will result in exceeding table limit of 1000 in the child table");
                // }
                string repeat_where = $"parentglobalid in ({globalid.ToDelimString(",","'")})";
                
                var t2 = await new ge_esriController(  _context,
                                                        _authorizationService,
                                                        _userManager,
                                                        _env ,
                                                        _ge_config).getFeatures(projectId,
                                                                    table2,
                                                                    repeat_where,
                                                                    page_size,
                                                                    null,
                                                                    2
                                                                    );
                foreach (string s2 in (string[]) t2.Value) {
                    if (s2 == null) { 
                        continue;
                    }
                    var survey_repeat  = JsonConvert.DeserializeObject<esriFeature<LTM_Survey_Data_Repeat2>>(s2);
                    var repeat_resp = LoadFeature(survey_repeat.features);
                }

                var mond_resp = await AddSurveyData(_project);
                
                // if (mond_resp < 0 ) {
                //     return Json("No records in MOND table");
                // }

                if (save == true) {
                    var resp_mond = await UpdateMOND(MOND);

                    var resp_monv = await UpdateMONV(MONV);
                }
                
                MOND_All.AddRange (MOND);
                MONV_All.AddRange (MONV);
            }

                      
            List<MOND> ordered = MOND_All.OrderBy(e=>e.DateTime).ToList();
            MOND_All = ordered;
            
            if (format=="json") {
            return Json(MOND_All);
            }

            if (format == "view") {
            return View("ViewMOND", MOND_All);
            }

            return Ok (MOND_All);

 }
//  private async Task<int> AddMONDAsync() // No async because the method does not need await
//         {
//             return await Task.Run(() =>
//             {
//                 var resp = await AddMOND(_project);
//                 return resp;
//             });
//         }
private string IfOther(string s1, string other) {
    
    if (s1==null && other==null) {
        return "";
    }
    
    if (s1==null && other!=null) {
        return other.Replace ("_"," ");
    }
    
    if (s1!="Other") {
        return s1.Replace("_"," ");
    } 

    return other.Replace("_"," ");
    
}
// private async Task<int> ReadFeature (List<items<EsriGeometry>>  geom_data) {
        
//         Survey_Data = new List<LTM_Survey_Data2>();

//         foreach (items<EsriGeometry> geom_items in geom_data) {
            
//             EsriGeometry geom = geom_items.geometry;
            
//             if (survey==null) {
//                continue; 
//             }
            
//             Geometry.Add (geom);
            
//         }

//         return Survey_Data.Count();
// }
private async Task<int> LoadFeature (List<items<LTM_Survey_Data2>>  survey_data) {
        
        if (Survey_Data == null) Survey_Data = new List<LTM_Survey_Data2>();
        if (Survey_Geom == null) Survey_Geom = new List<LTM_Geometry>();
        
        foreach (items<LTM_Survey_Data2> survey_items in survey_data) {
            
            LTM_Survey_Data2 survey = survey_items.attributes;
            if (survey==null) {
               continue; 
            }

            EsriGeometry geom =  survey_items.geometry;
            LTM_Geometry geom2 = getLTMGeometry(geom, survey, Esri.esriWGS84);
            
            
            Survey_Data.Add (survey);
            Survey_Geom.Add (geom2);
        }

        return Survey_Data.Count();
}


private LTM_Geometry getLTMGeometry(EsriGeometry geom, LTM_Survey_Data2 survey, int wkid) {
    
        LTM_Geometry geom2 = new LTM_Geometry();

        geom2.hole_id = survey.hole_id;
        geom2.objectid = survey.objectid;
        geom2.x = geom.x;
        geom2.y = geom.y;
        
        ge_data g =  new ge_data(); 
        
        if (geom.x>=0.001 & geom.y > 0.001) {
        
        if (wkid ==  Esri.esriOSGB36) {
            ge_projectionOSGB36 p =  new ge_projectionOSGB36(g);
            g.locLongitude = geom.x;
            g.locLatitude = geom.y;
            if (p.calcEN_fromLatLong()) {;
                geom2.East = g.locEast.Value;
                geom2.North = g.locNorth.Value;
            }
        }

        if (wkid ==  Esri.esriWGS84) {
            double height = 100;
            
            if (survey.surv_g_level != null) {
            height = survey.surv_g_level.Value;
            }

            Transform_WGS84_and_OSGB t = new Transform_WGS84_and_OSGB();
            g = t.WGS84_Lat_Long_Height_to_OSGB_East_North( geom.y,
                                                            geom.x,
                                                            height);
            if (g != null) {
                geom2.East = g.locEast.Value;
                geom2.North = g.locNorth.Value;
            }

        }
        }

        // Transform_WGS84_and_OSGB t2 = new Transform_WGS84_and_OSGB();
        // ge_data g4 = t2.WGS84_Lat_Long_Height_to_OSGB_East_North(  52.65797861194,
        //                                                     1.71605201472,
        //                                                     69.391);
        // ge_data g2 =  new ge_data(); 
        // g2.locLongitude =  0.3958734;
        // g2.locLatitude = 51.4894179;
        // ge_projectionOSGB36 p2 =  new ge_projectionOSGB36(g2);
        // p2.calcEN_fromLatLong();
        // string res = $"Lat:{g2.locLatitude} Long:{g2.locLongitude} N:{g2.locNorth} E:{g2.locEast}";

    return geom2;

}
private EsriGeometry getEsriGeometry(LTM_Geometry geom) {
    
    EsriGeometry geom2 = new EsriGeometry();
        
        geom2.x = geom.x;
        geom2.y = geom.y;

    return geom2;

}
private async Task<int> LoadFeature (List<items<LTM_Survey_Data_Repeat2>> survey_data_repeat) {
        
            if (Survey_Repeat_Data == null) Survey_Repeat_Data = new List<LTM_Survey_Data_Repeat2>();
            
            foreach (items<LTM_Survey_Data_Repeat2> repeat_items in survey_data_repeat) {
                
                LTM_Survey_Data_Repeat2 survey2 = repeat_items.attributes;
                
                if (survey2 == null) {
                    continue; 
                }

                Survey_Repeat_Data.Add (survey2);
            }

        return Survey_Repeat_Data.Count();
}


private async Task<int> xReadFeatureMOND(List<items<LTM_Survey_Data2>>  survey_data, 
                                         List<items<LTM_Survey_Data_Repeat2>> survey_data_repeat,  
                                         ge_project project) {
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

            Survey_Data = new List<LTM_Survey_Data2>();

        foreach (items<LTM_Survey_Data2> survey_items in survey_data) {
            
            LTM_Survey_Data2 survey = survey_items.attributes;
            
            if (survey==null) {
               continue; 
            }
            
            Survey_Data.Add (survey);
            
            string point_id = survey.hole_id;

            if (map_hole_id.ContainsKey(survey.hole_id)) {
                point_id = map_hole_id[survey.hole_id];
            } 

            POINT pt = POINT.Find(p=>p.PointID==point_id);
            
            if (pt==null) {
                if ((survey.pack.Contains("A") && projectId==1) || 
                    (survey.pack.Contains("B") && projectId==2) ||
                    (survey.pack.Contains("C") && projectId==3) ||
                    (survey.pack.Contains("D") && projectId==4)) {
                Console.WriteLine ("Package {0} survey.hole_id [{1}] not found", survey.pack, survey.hole_id);
                } 
                continue; 
               
            }

            List<MONG> pt_mg = MONG.Where(m=>m.PointID==pt.PointID).ToList();

            if (pt_mg.Count == 0) {
                Console.WriteLine ("Package {0} monitoring point for survey.hole_id {1} not found", survey.pack, survey.hole_id);
                continue;
            }
            
            string mong_id  = survey.mong_ID;
            MONG mg = null;

            if (map_mong_id.ContainsKey(survey.hole_id + survey.mong_ID)) {
                mong_id = map_mong_id[survey.hole_id + survey.mong_ID];
            } 
            
            mg =   pt_mg.Find(m=>m.ItemKey == mong_id);

            if (map_mong_id_gintrecid.ContainsKey(survey.hole_id + survey.mong_ID)) {
                int mong_gintrecid = map_mong_id_gintrecid[survey.hole_id + survey.mong_ID];
                mg = pt_mg.Find(m=>m.GintRecID == mong_gintrecid);
            } 

            if (mg == null) {
                mg = pt_mg.FirstOrDefault();
            } 
      
            DateTime? survey_start = gINTDateTime(survey.date1_getDT());

            if (survey_start==null) continue;

            DateTime survey_startDT = gINTDateTime(survey.date1_getDT()).Value;
            DateTime survey_endDT = gINTDateTime(survey.time2_getDT()).Value;
          
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
                
                mv.DIP_SRLN = IfOther(survey.dip_instr,survey.dip_instr_other);
                mv.DIP_CLBD = null;
                
                mv.FLO_SRLN = IfOther(survey.purg_meter, survey.purg_meter_other);
                mv.FLO_CLBD = gINTDateTime(survey.purg_meter_cali_d_getDT());
                
                mv.GAS_SRLN = IfOther(survey.gas_instr, survey.gas_instr_other);
                mv.GAS_CLBD = gINTDateTime(survey.gas_cali_d_getDT());
                
                mv.PID_SRLN = IfOther(survey.PID_instr, survey.PID_instr_other);
                mv.PID_CLBD = gINTDateTime(survey.PID_cali_d_getDT());
                
                mv.RND_REF = survey.mon_rd_nb;
                mv.MONV_DATM = survey.dip_datum;

                mv.AIR_PRESS = survey.atmo_pressure;
                mv.AIR_TEMP = survey.atmo_temp;
                
                mv.PIPE_DIA = survey.pipe_diam;


                if(survey.dip_datum_offset != null) {
                   // mv.MONV_DIS = ((float) survey.dip_datum_offset.Value) / 100f;
                    mv.MONV_DIS = ((float) survey.dip_datum_offset.Value);
                }

                mv.MONV_MENG = survey.Creator;
                MONV.Add(mv);

            // Water depth below gl (m)
            if (survey.depth_gwl_bgl != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "WDEP";
                md.MOND_RDNG = Convert.ToString(survey.depth_gwl_bgl);
                md.MOND_UNIT = "m";
                md.MOND_INST = "Dipmeter: " + IfOther(survey.dip_instr, survey.dip_instr_other);
                md.MOND_REM = survey.dip_com;
                if (survey.dip_time!=null) {
                md.DateTime  = gINTDateTime(survey.dip_time_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // Water depth below gl (m)
            if (survey.dip_check == "dry") {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "WDEP";
                md.MOND_RDNG = "Dry";
                md.MOND_UNIT = "m";
                md.MOND_INST = "Dipmeter: " + IfOther(survey.dip_instr, survey.dip_instr_other);
                md.MOND_REM = survey.dip_com;
                if (survey.dip_time!=null) {
                md.DateTime  = gINTDateTime(survey.dip_time_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // Water depth below gl (m)
            if (survey.depth_gwl_bgl == null && survey.dip_req == "yes" && survey.dip_datum_offset!=null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "WDEP";
                md.MOND_RDNG = "Dry";
                md.MOND_UNIT = "m";
                md.MOND_INST = "Dipmeter: " + IfOther(survey.dip_instr, survey.dip_instr_other);
                md.MOND_REM = survey.dip_com;
                if (survey.dip_time!=null) {
                md.DateTime  = gINTDateTime(survey.dip_time_getDT()).Value;
                }
                MOND.Add(md);
            }    

            // Depth to base of instalation (m)
            if (survey.depth_install_bgl != null && survey.dip_datum_offset !=null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DBSE";
                md.MOND_RDNG = Convert.ToString(survey.depth_install_bgl);
                md.MOND_NAME = "Depth to base of installation";
                md.MOND_UNIT = "m";
                md.MOND_INST = IfOther(survey.dip_instr, survey.dip_instr_other);
                if (survey.dip_time!=null) {
                md.DateTime  = gINTDateTime(survey.dip_time_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // PH
            if (survey.ph != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "PH";
                md.MOND_RDNG = Convert.ToString(survey.ph);
                md.MOND_UNIT = "PH";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

            //Redox Potential
            if (survey.redox_potential != null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "RDX";
                md.MOND_RDNG = Convert.ToString(survey.redox_potential);
                md.MOND_UNIT = "mV";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

            // Electrical Conductivity
            if (survey.conductivity != null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "EC";
                md.MOND_RDNG = Convert.ToString(survey.conductivity);
                md.MOND_NAME = "Electrical Conductivity";
                md.MOND_UNIT = "uS/cm";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

            //Temperature (°C)
            if (survey.temperature != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DOWNTEMP";
                md.MOND_RDNG = Convert.ToString(survey.temperature);
                md.MOND_NAME = "Downhole temperature";
                md.MOND_UNIT = "Deg C";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

            //Dissolved oxygen (mg/l)
            if (survey.dissolved_oxy != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DO";
                md.MOND_RDNG = Convert.ToString(survey.dissolved_oxy);
                md.MOND_NAME = "Dissolved Oxygen";
                md.MOND_UNIT = "mg/l";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

            // Atmosperic Temperature (°C)
            if (survey.atmo_temp != null ) {
               MOND md = NewMOND(mg, survey);  
                md.MOND_TYPE = "TEMP";
                md.MOND_RDNG = Convert.ToString(survey.atmo_temp);
                md.MOND_NAME = "Atmospheric temperature";
                md.MOND_UNIT = "Deg C";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

            // Atmospheric pressure (mbar)
            if (survey.atmo_pressure != null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "BAR";
                md.MOND_RDNG = Convert.ToString(survey.atmo_pressure);
                md.MOND_NAME = "Atmospheric pressure";
                md.MOND_UNIT = "mbar";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

            // Peak Gas flow
            if (survey.gas_flow_peak != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GFLOP";
                md.MOND_RDNG = Convert.ToString(survey.gas_flow_peak);
                md.MOND_NAME = "Peak gas flow rate";
                md.MOND_UNIT = "l/h";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
               MOND.Add(md);
            }

            // Steady Gas flow
            if (survey.gas_flow_steady != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GFLOS";
                md.MOND_RDNG = Convert.ToString(survey.gas_flow_steady);
                md.MOND_NAME = "Steady gas flow rate";
                md.MOND_UNIT = "l/h";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }

            // Peak diff barometric pressure (mbar?)
            if (survey.BH_pressure_peak != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GPRP";
                md.MOND_RDNG = Convert.ToString(survey.BH_pressure_peak);
                md.MOND_NAME = "Peak diff barometric pressure";
                md.MOND_UNIT = Convert.ToString(survey.BH_pressure_units);
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }

            // Steady diff barometric pressure (mbar?) 
            if (survey.BH_pressure_steady != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GPRS";
                md.MOND_RDNG = Convert.ToString(survey.BH_pressure_steady);
                md.MOND_NAME = "Steady diff barometric pressure";
                md.MOND_UNIT = Convert.ToString(survey.BH_pressure_units);;
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // Ambient Methane concentration 
            if (survey.ambi1_CH4 != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "TGMA";
                md.MOND_RDNG = Convert.ToString(survey.ambi1_CH4);
                md.MOND_NAME = "Ambient methane concentration";
                md.MOND_UNIT = "%vol";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
          
            // Ambient Methane concentration 
            if (survey.ambi1_CH4_lel != null) {
            MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GMA";
                md.MOND_RDNG = Convert.ToString(survey.ambi1_CH4_lel);
                md.MOND_NAME = "Ambient methane concentration";
                md.MOND_UNIT = "%LEL";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
               if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
           
            // Ambient Oxygen concentration 
            if (survey.ambi1_O2 != null) {
            MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GOXA";
                md.MOND_RDNG = Convert.ToString(survey.ambi1_O2);
                md.MOND_NAME = "Ambient oxygen concentration";
                md.MOND_UNIT = "%vol";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
               if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
           
            // Ambient Carbon Dioxide concentration 
            if (survey.ambi1_CO2 != null) {
            MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GCDA";
                md.MOND_RDNG = Convert.ToString(survey.ambi1_CO2);
                md.MOND_NAME = "Ambient carbon dioxide concentration";
                md.MOND_UNIT = "%vol";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
               if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // Ambient Carbon Dioxide concentration 
            if (survey.PID_t != null) {
            MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "VOC";
                md.MOND_RDNG = Convert.ToString(survey.PID_t);
                md.MOND_NAME = "Volatile organic compounds";
                md.MOND_UNIT = "ppm";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }

            List<items<LTM_Survey_Data_Repeat2>> repeat = survey_data_repeat.FindAll(r=>r.attributes.parentglobalid==survey.globalid); 
          
            foreach (items<LTM_Survey_Data_Repeat2> repeat_items in repeat) {
                
                LTM_Survey_Data_Repeat2 survey2 = repeat_items.attributes;
                
                if (survey2.elapse_t == null) continue;
                
                int elapsed = survey2.elapse_t.Value;
                
                //DateTime dt = survey_start.Value.AddSeconds(elapsed);
                DateTime dt = survey_startDT.AddSeconds(elapsed);
                
                if (survey.gas_repeat_tstart!=null) {
                dt  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value.AddSeconds(elapsed);
                }

                // Gas flow (l/h)
                // if (survey2.gas_flow_t != null) {
                //     MOND md = NewMOND(mg, survey, survey2);
                //     md.MOND_TYPE = "GFLO";
                //     md.MOND_RDNG = Convert.ToString(survey2.gas_flow_t);
                //     md.MOND_NAME = "Gas flow rate";
                //     md.MOND_UNIT = "l/h";
                //     md.MOND_INST = survey.gas_instr;
                //     md.DateTime = dt; 
                //     MOND.Add(md);  
                // }
                
                // Methane reading Limit CH4 LEL (%)
                if (survey2.CH4_lel_t != null) {
                    MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "GM";
                    md.MOND_RDNG = Convert.ToString(survey2.CH4_lel_t);
                    md.MOND_NAME = "Methane as percentage of LEL (Lower Explosive Limit)";
                    md.MOND_UNIT = "%vol";
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
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
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
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
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
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
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
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
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
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
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                    md.DateTime = dt;  
                    MOND.Add(md);  
                }
                
                //Photo Ionisations PID (ppm)
                // if (survey2.PID_t != null) {
                //     MOND md = NewMOND(mg, survey, survey2);
                //     md.MOND_TYPE = "PID";
                //     md.MOND_RDNG = Convert.ToString(survey2.PID_t);
                //     md.MOND_NAME = "Photoionization Detector";
                //     md.MOND_UNIT = "ppm";
                //     md.MOND_INST = survey.PID_instr;
                //     md.DateTime = dt; 
                //     MOND.Add(md);  
                // }

                // VOC (ppm)
                // if (survey2.voc_t != null) {
                //     MOND md = NewMOND(mg, survey, survey2);
                //     md.MOND_TYPE = "VOC";
                //     md.MOND_RDNG = Convert.ToString(survey2.voc_t);
                //     md.MOND_NAME = "Volatile Organic Compounds";
                //     md.MOND_UNIT = "ppm";
                //     md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                //     md.DateTime = dt; 
                //     MOND.Add(md);  
                // }

          }
    }

    return MOND.Count();
     
}


private async Task<int> AddSurveyData(ge_project project) {
            
            
            string[] AllPoints = new string[] {""};
            
            MOND = new List<MOND>();
            MONV = new List<MONV>(); 
            
            var resp = await new ge_gINTController (_context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config
                                                        ).getMONG(project.Id,AllPoints);
            var okResult = resp as OkObjectResult;

            if (okResult == null) {
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

           
            
            int projectId = POINT.FirstOrDefault().gINTProjectID;


        foreach (LTM_Survey_Data2 survey in Survey_Data) {
                       
            if (survey==null) {
               continue; 
            }
            
            string point_id = survey.hole_id;

            if (map_hole_id.ContainsKey(survey.hole_id)) {
                point_id = map_hole_id[survey.hole_id];
            } 

            POINT pt = POINT.Find(p=>p.PointID==point_id);
            
            if (pt==null) {
                if ((survey.pack.Contains("A") && projectId==1) || 
                    (survey.pack.Contains("B") && projectId==2) ||
                    (survey.pack.Contains("C") && projectId==3) ||
                    (survey.pack.Contains("D") && projectId==4)) {
                Console.WriteLine ("Package {0} survey.hole_id [{1}] not found", survey.pack, survey.hole_id);
                } 
                continue; 
               
            }

            List<MONG> pt_mg = MONG.Where(m=>m.PointID==pt.PointID).ToList();

            if (pt_mg.Count == 0) {
                Console.WriteLine ("Package {0} monitoring point for survey.hole_id {1} not found", survey.pack, survey.hole_id);
                continue;
            }
            
            string mong_id  = survey.mong_ID;
            
            if (mong_id != null) {
                mong_id = mong_id.Trim ();
            }

            MONG mg = null;
            MONG mg_topo =  null;

            if (map_mong_id.ContainsKey(survey.hole_id + survey.mong_ID)) {
                mong_id = map_mong_id[survey.hole_id + survey.mong_ID];
            } 
            
            mg =   pt_mg.Find(m=>m.ItemKey == mong_id);
            
            if (map_mong_id_gintrecid.ContainsKey(survey.hole_id + survey.mong_ID)) {
                int mong_gintrecid = map_mong_id_gintrecid[survey.hole_id + survey.mong_ID];
                mg = pt_mg.Find(m=>m.GintRecID == mong_gintrecid);
            } 

            if (mg == null) {
                mg = pt_mg.FirstOrDefault();
            } 
            
            

            DateTime? survey_start = gINTDateTime(survey.date1_getDT());

            if (survey_start==null) continue;
                    AddVisit (pt, survey);
           
            if (survey.QA_status.Contains("Dip_Approved")) {
                    AddDip(mg, survey);
            }
            
            if (survey.QA_status.Contains("Purge_Approved")) {
                    AddPurge(mg, survey);
            }
            
            if (survey.QA_status.Contains("Gas_Approved")) {
                    AddGas(mg, survey);
            }

            if (survey.QA_status.Contains("Topo_Approved") && 
                (survey.surv_g_level != null || 
                survey.meas_ToC != null)) { 

                mg_topo = pt_mg.Find(m=>m.MONG_DIS == 0);
                
                if (mg_topo == null) {
                    mg_topo = new MONG();
                    mg_topo.PointID = pt.PointID;
                    mg_topo.PIPE_REF = "Pipe 1";
                    mg_topo.ItemKey = "SM1";
                    mg_topo.MONG_DIS = 0;
                    mg_topo.MONG_DETL = "Ground Survey Monitoring Point";
                    mg_topo.MONG_TYPE = "GNSS";
                    pt_mg.Add (mg_topo);
                    var resp_mong = await UpdateMONG (pt_mg);
                    //if update fails set mg_topo=null;

                }

                if (mg_topo != null) {
                    AddTopo(mg_topo, survey);
                }
            }

        }

        return MOND.Count();
     
}
private int AddVisit(POINT pt, LTM_Survey_Data2 survey){
            
          
            MONV mv = NewMONV(pt,survey);
               
                mv.MONV_STAR = gINTDateTime(survey.date1_getDT()).Value;
                mv.MONV_ENDD = gINTDateTime(survey.time2_getDT()).Value;
                
                mv.MONV_DIPR = survey.dip_req;
                mv.MONV_GASR = survey.gas_mon;
                mv.MONV_LOGR = survey.logger_downld_req ;
                mv.MONV_TOPR = survey.surv_req;
                
                mv.MONV_REMG = survey.gas_fail + " " + survey.gas_com;
                mv.MONV_REMD = survey.dip_fail + " " + survey.dip_com;
                mv.MONV_REML = survey.logger_fail + " " + survey.logger_com;
                mv.MONV_REMS = survey.samp_fail + " " + survey.samp_com;
                mv.MONV_REMT = survey.surv_fail + " " + survey.surv_com;
                
                mv.PUMP_TYPE = survey.purg_pump + " " + survey.purg_pump_oth;

                mv.MONV_WEAT = survey.weath;
                mv.MONV_TEMP = survey.temp;
                mv.MONV_WIND = survey.wind;
                
                mv.DIP_SRLN = IfOther(survey.dip_instr,survey.dip_instr_other);
                mv.DIP_CLBD = null;
                
                mv.FLO_SRLN = IfOther(survey.purg_meter, survey.purg_meter_other);
                mv.FLO_CLBD = gINTDateTime(survey.purg_meter_cali_d_getDT());
                
                mv.GAS_SRLN = IfOther(survey.gas_instr, survey.gas_instr_other);
                mv.GAS_CLBD = gINTDateTime(survey.gas_cali_d_getDT());
                
                mv.PID_SRLN = IfOther(survey.PID_instr, survey.PID_instr_other);
                mv.PID_CLBD = gINTDateTime(survey.PID_cali_d_getDT());
                
                mv.RND_REF = survey.mon_rd_nb;
                mv.MONV_DATM = survey.dip_datum;

                mv.AIR_PRESS = survey.atmo_pressure;
                mv.AIR_TEMP = survey.atmo_temp;
                
                mv.PIPE_DIA = survey.pipe_diam;

                if(survey.dip_datum_offset != null) {
                   // mv.MONV_DIS = ((float) survey.dip_datum_offset.Value) / 100f;
                    mv.MONV_DIS = ((float) survey.dip_datum_offset.Value);
                }

                mv.MONV_MENG = survey.Creator;
                MONV.Add(mv);

    return 0;

}


private int AddPurge(MONG mg, LTM_Survey_Data2 survey) {
            
            // PH
            if (survey.ph != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "PH";
                md.MOND_RDNG = Convert.ToString(survey.ph);
                md.MOND_UNIT = "PH";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

            //Redox Potential
            if (survey.redox_potential != null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "RDX";
                md.MOND_RDNG = Convert.ToString(survey.redox_potential);
                md.MOND_UNIT = "mV";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

            // Electrical Conductivity
            if (survey.conductivity != null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "EC";
                md.MOND_RDNG = Convert.ToString(survey.conductivity);
                md.MOND_NAME = "Electrical Conductivity";
                md.MOND_UNIT = "uS/cm";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

            //Temperature (°C)
            if (survey.temperature != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DOWNTEMP";
                md.MOND_RDNG = Convert.ToString(survey.temperature);
                md.MOND_NAME = "Downhole temperature";
                md.MOND_UNIT = "Deg C";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

            //Dissolved oxygen (mg/l)
            if (survey.dissolved_oxy != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DO";
                md.MOND_RDNG = Convert.ToString(survey.dissolved_oxy);
                md.MOND_NAME = "Dissolved Oxygen";
                md.MOND_UNIT = "mg/l";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

         
              //Turbidity
            if (survey.turbitity != null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "TURB";
                md.MOND_RDNG = Convert.ToString(survey.turbitity);
                md.MOND_NAME = "Turbidity";
                md.MOND_UNIT = "NTU";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }
return 0;

}
private int AddTopo(MONG mg, LTM_Survey_Data2 survey) {
            
            // surv_g_level	Surveyed Ground Level (mAOD)	Surveyed level of ground
            // surv_ToC	Surveyed Top of Cover (mAOD)	surveyed level of Top of Cover


            if (survey.surv_g_level != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "LEV";
                md.MOND_RDNG = Convert.ToString(survey.surv_g_level);
                md.MOND_NAME = "Ground Level";
                md.MOND_UNIT = "m";
                md.MOND_INST = "GNSS Instrument";
                md.MOND_REM = survey.surv_com;
                if (survey.surv_time!=null) {
                md.DateTime  = gINTDateTime(survey.surv_time_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // meas_ToC	Measured Top of Cover - A(m)	Tape measured height difference (distance) between ground level and Top of Cover
            // meas_ToI	Measured Top of Installed pipe - B (m)	Tape measured height difference (distance) between ground level and Top of Installed pipe

            if (survey.meas_ToC != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DSTL";
                md.MOND_RDNG = Convert.ToString(survey.meas_ToC);
                md.MOND_NAME = "Distance from GL to Top of Cover";
                md.MOND_UNIT = "m";
                md.MOND_REM = survey.surv_com;
                if (survey.surv_time!=null) {
                md.DateTime  = gINTDateTime(survey.surv_time_getDT()).Value;
                }
                MOND.Add(md);
            }

        return 0;
}

private int AddDip(MONG mg, LTM_Survey_Data2 survey) {

            // Water depth below gl (m)
            
            if (survey.depth_gwl_bgl != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "WDEP";
                md.MOND_RDNG = Convert.ToString(survey.depth_gwl_bgl);
                md.MOND_UNIT = "m";
                md.MOND_INST = "Dipmeter: " + IfOther(survey.dip_instr, survey.dip_instr_other);
                md.MOND_REM = survey.dip_com;
                if (survey.dip_time!=null) {
                md.DateTime  = gINTDateTime(survey.dip_time_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // Water depth below gl (m)
            if (survey.dip_check == "dry") {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "WDEP";
                md.MOND_RDNG = "Dry";
                md.MOND_UNIT = "m";
                md.MOND_INST = "Dipmeter: " + IfOther(survey.dip_instr, survey.dip_instr_other);
                md.MOND_REM = survey.dip_com;
                if (survey.dip_time!=null) {
                md.DateTime  = gINTDateTime(survey.dip_time_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // Water depth below gl (m)
            if (survey.depth_gwl_bgl == null && survey.dip_req == "yes" && survey.dip_datum_offset!=null && survey.dip_or_pressure != "PressureGauge") {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "WDEP";
                md.MOND_RDNG = "Dry";
                md.MOND_UNIT = "m";
                md.MOND_INST = "Dipmeter: " + IfOther(survey.dip_instr, survey.dip_instr_other);
                md.MOND_REM = survey.dip_com;
                if (survey.dip_time!=null) {
                md.DateTime  = gINTDateTime(survey.dip_time_getDT()).Value;
                }
                MOND.Add(md);
            }    

            // Depth to base of instalation (m)
            if (survey.depth_install_bgl != null && survey.dip_datum_offset !=null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DBSE";
                md.MOND_RDNG = Convert.ToString(survey.depth_install_bgl);
                md.MOND_NAME = "Depth to base of installation";
                md.MOND_UNIT = "m";
                md.MOND_INST = IfOther(survey.dip_instr, survey.dip_instr_other);
                if (survey.dip_time!=null) {
                md.DateTime  = gINTDateTime(survey.dip_time_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // Sub aquifer Water pressure (bar)
            if (survey.water_pressure != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "PRES";
                md.MOND_RDNG = Convert.ToString(survey.water_pressure);
                md.MOND_NAME = "Water pressure";
                md.MOND_UNIT = "bar";
                md.MOND_INST = "Pressure gauge on borehole";
                if (survey.dip_time!=null) {
                md.DateTime  = gINTDateTime(survey.dip_time_getDT()).Value;
                }
                MOND.Add(md);
            }
            return 0;
}
private int AddGas(MONG mg, LTM_Survey_Data2 survey) {

         //   DateTime survey_startDT = gINTDateTime(survey.date1_getDT()).Value;
            
         //   DateTime survey_endDT = gINTDateTime(survey.time2_getDT()).Value;
          
                        // Peak Gas flow
            if (survey.gas_flow_peak != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GFLOP";
                md.MOND_RDNG = Convert.ToString(survey.gas_flow_peak);
                md.MOND_NAME = "Peak gas flow rate";
                md.MOND_UNIT = "l/h";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
               MOND.Add(md);
            }

            // Steady Gas flow
            if (survey.gas_flow_steady != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GFLOS";
                md.MOND_RDNG = Convert.ToString(survey.gas_flow_steady);
                md.MOND_NAME = "Steady gas flow rate";
                md.MOND_UNIT = "l/h";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }

            // Peak diff barometric pressure (mbar?)
            if (survey.BH_pressure_peak != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GPRP";
                md.MOND_RDNG = Convert.ToString(survey.BH_pressure_peak);
                md.MOND_NAME = "Peak diff barometric pressure";
                md.MOND_UNIT = Convert.ToString(survey.BH_pressure_units);
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }

            // Steady diff barometric pressure (mbar?) 
            if (survey.BH_pressure_steady != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GPRS";
                md.MOND_RDNG = Convert.ToString(survey.BH_pressure_steady);
                md.MOND_NAME = "Steady diff barometric pressure";
                md.MOND_UNIT = Convert.ToString(survey.BH_pressure_units);;
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // Ambient Methane concentration 
            if (survey.ambi1_CH4 != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "TGMA";
                md.MOND_RDNG = Convert.ToString(survey.ambi1_CH4);
                md.MOND_NAME = "Ambient methane concentration";
                md.MOND_UNIT = "%vol";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
          
            // Ambient Methane concentration 
            if (survey.ambi1_CH4_lel != null) {
            MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GMA";
                md.MOND_RDNG = Convert.ToString(survey.ambi1_CH4_lel);
                md.MOND_NAME = "Ambient methane concentration";
                md.MOND_UNIT = "%LEL";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
               if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
           
            // Ambient Oxygen concentration 
            if (survey.ambi1_O2 != null) {
            MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GOXA";
                md.MOND_RDNG = Convert.ToString(survey.ambi1_O2);
                md.MOND_NAME = "Ambient oxygen concentration";
                md.MOND_UNIT = "%vol";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
               if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
           
            // Ambient Carbon Dioxide concentration 
            if (survey.ambi1_CO2 != null) {
            MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GCDA";
                md.MOND_RDNG = Convert.ToString(survey.ambi1_CO2);
                md.MOND_NAME = "Ambient carbon dioxide concentration";
                md.MOND_UNIT = "%vol";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
               if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // Photo Ionisation Detector concentration 
            if (survey.PID_t != null) {
            MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "VOC";
                md.MOND_RDNG = Convert.ToString(survey.PID_t);
                md.MOND_NAME = "Volatile organic compounds";
                md.MOND_UNIT = "ppm";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }

            // Atmosperic Temperature (°C)
            if (survey.atmo_temp != null ) {
               MOND md = NewMOND(mg, survey);  
                md.MOND_TYPE = "TEMP";
                md.MOND_RDNG = Convert.ToString(survey.atmo_temp);
                md.MOND_NAME = "Atmospheric temperature";
                md.MOND_UNIT = "Deg C";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }

            // Atmospheric pressure (mbar)
            if (survey.atmo_pressure != null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "BAR";
                md.MOND_RDNG = Convert.ToString(survey.atmo_pressure);
                md.MOND_NAME = "Atmospheric pressure";
                md.MOND_UNIT = "mbar";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            DateTime survey_startDT = gINTDateTime(survey.date1_getDT()).Value;

            List<LTM_Survey_Data_Repeat2> repeat = Survey_Repeat_Data.FindAll(r=>r.parentglobalid==survey.globalid); 
          
            foreach (LTM_Survey_Data_Repeat2 survey2 in repeat) {
                
                if (survey2.elapse_t == null) continue;
                
                int elapsed = survey2.elapse_t.Value;
                
                //DateTime dt = survey_start.Value.AddSeconds(elapsed);
                DateTime dt = survey_startDT.AddSeconds(elapsed);
                
                if (survey.gas_repeat_tstart!=null) {
                dt  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value.AddSeconds(elapsed);
                }

                // Gas flow (l/h)
                // if (survey2.gas_flow_t != null) {
                //     MOND md = NewMOND(mg, survey, survey2);
                //     md.MOND_TYPE = "GFLO";
                //     md.MOND_RDNG = Convert.ToString(survey2.gas_flow_t);
                //     md.MOND_NAME = "Gas flow rate";
                //     md.MOND_UNIT = "l/h";
                //     md.MOND_INST = survey.gas_instr;
                //     md.DateTime = dt; 
                //     MOND.Add(md);  
                // }
                
                // Methane reading Limit CH4 LEL (%)
                if (survey2.CH4_lel_t != null) {
                    MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "GM";
                    md.MOND_RDNG = Convert.ToString(survey2.CH4_lel_t);
                    md.MOND_NAME = "Methane as percentage of LEL (Lower Explosive Limit)";
                    md.MOND_UNIT = "%vol";
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
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
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
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
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
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
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
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
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
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
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                    md.DateTime = dt;  
                    MOND.Add(md);  
                }
                
                // Volatile Organic Compounds from Gas Analyser 
                if (survey2.voc_t != null) {
                    MOND md = NewMOND(mg, survey, survey2); 
                    md.MOND_TYPE = "VOC";
                    md.MOND_RDNG = Convert.ToString(survey2.voc_t);
                    md.MOND_NAME = "Volatile organic compounds";
                    md.MOND_UNIT = "ppm";
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                    md.DateTime  = dt;
                    MOND.Add(md);
                }


            }

            return 0;
}


private MOND NewMOND (MONG mg, LTM_Survey_Data2 survey, LTM_Survey_Data_Repeat2 repeat) {
        
        int round = convertToInt16(survey.mon_rd_nb,"R",-999);

        MOND md = new MOND {
                        ge_source ="esri_survey2_repeat",
                        ge_otherId = Convert.ToString(repeat.globalid),
                        gINTProjectID = mg.gINTProjectID,
                        PointID = mg.PointID,
                        ItemKey = mg.ItemKey,
                        MONG_DIS = mg.MONG_DIS,
                        RND_REF = String.Format("{0:00}", round),
                        MOND_REF = String.Format("Round {0:00} Seconds {1:00}" ,round,repeat.elapse_t),
                        };
        return md;    
 }
 
 
 private MONV NewMONV (POINT pt, LTM_Survey_Data2 survey) {
      
        int round = convertToInt16(survey.mon_rd_nb,"R", -999);
        
        MONV mv = new MONV {
                        ge_source ="esri_survey2",
                        ge_otherId = Convert.ToString(survey.globalid),
                        gINTProjectID = pt.gINTProjectID,
                        PointID = pt.PointID,
                        DateTime = gINTDateTime(survey.date1_getDT()),
                        RND_REF = String.Format("{0:00}", round),
                        MONV_REF = String.Format("Round {0:00}", round)
                        };
        return mv;    
 }




private MOND NewMOND (MONG mg, LTM_Survey_Data2 survey ) {
        
        int round = convertToInt16(survey.mon_rd_nb,"R",-999);

        MOND md = new MOND {
                        ge_source ="esri_survey2",
                        ge_otherId = Convert.ToString(survey.globalid),
                        gINTProjectID = mg.gINTProjectID,
                        PointID = mg.PointID,
                        ItemKey = mg.ItemKey,
                        MONG_DIS = mg.MONG_DIS,
                        RND_REF = String.Format("{0:00}", round),
                        MOND_REF = String.Format("Round {0:00}", round),
                        DateTime =  gINTDateTime(survey.date1_getDT())
                        };
        return md;    

 }
 
 public async Task<IActionResult> UpdateFeature(Guid projectId,
                                                List<LTM_Survey_Data2>  survey_data, 
                                                List<LTM_Geometry> geometry
                                                )
                                                {
        // https://services9.arcgis.com/4MYxhHBDmXiGXKqw/ArcGIS/rest/services/service_f52f08750ca04914bfc7c90a3be64ff1/FeatureServer

        //  https://developers.arcgis.com/rest/services-reference/update-features.htm                              
        
        StringBuilder sb = new StringBuilder();
        sb.Append ("[");

        for (int i=0;i<survey_data.Count();i++) {
            string sd = JsonConvert.SerializeObject(survey_data[i]);
            string geom = JsonConvert.SerializeObject(getEsriGeometry(geometry[i]));
            if (i>0) sb.Append (",");
            sb.Append("{");
            sb.Append ("\"attributes\":" + sd);
            sb.Append (",");
            sb.Append ("\"geometry\":" + geom);
            sb.Append("}");
        }

        sb.Append ("]");
        
        string update = sb.ToString();
                                          
        var t1 = await new ge_esriController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).updateFeatures (projectId,
                                                                "LTM_Survey_Data_R06",
                                                                update
                                                                );
        return t1;
  }

public async Task<IActionResult> UpdateTable(Guid projectId,
                                                List<items<LTM_Survey_Data_Repeat2>>  repeat_data
                                                )
                                                {
        // https://services9.arcgis.com/4MYxhHBDmXiGXKqw/ArcGIS/rest/services/service_f52f08750ca04914bfc7c90a3be64ff1/FeatureServer

        //  https://developers.arcgis.com/rest/services-reference/update-features.htm                                              
    
        StringBuilder sb = new StringBuilder();
    
        string update = sb.ToString();

                   
                                                  
        var t1 = await new ge_esriController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).updateFeatures (projectId,
                                                                "LTM_Survey_Data_R06",
                                                                update
                                                                );
        return t1;                                               

  }


public async Task<IActionResult> ReadFeature    (   Guid[] projectId, 
                                                    string dataset,
                                                    string where = "",
                                                    int page_size = 250,
                                                    int[] pages = null,
                                                    int orderby = Esri.OrderBy.None,
                                                    string format = "view", 
                                                    Boolean save = false) {

            List<MOND> MOND_All = new List<MOND>();
            List<ge_project> processed = new List<ge_project>();

            foreach (Guid Id in projectId) {

                var _project = await _context.ge_project
                                        .SingleOrDefaultAsync(m => m.Id == Id);
                
                var resp = await ReadFeature ( Id,
                                                dataset,
                                                where,
                                                page_size,
                                                pages,
                                                orderby,
                                                "", 
                                                save
                                                );
                
                var okResult = resp as OkObjectResult;   
                
                if (okResult==null) {
                    continue;
                }

                List<MOND> projMOND = okResult.Value as List<MOND>;  
                MOND_All.AddRange (projMOND); 
                _project.description = $"{projMOND.Count} MOND records found";
                processed.Add (_project);
            } 

            if (format=="view") {
                return View (processed);
            }

            if (format=="json") {
                return Json (MOND_All);
            }

            return Ok (MOND_All);

            }
 [HttpPost]
public async Task<IActionResult> PostReadFeatureByGroup(Guid groupId, 
                                                    string dataset,
                                                    string where = "",
                                                    int page_size = 250,
                                                    string pages = "",
                                                    int orderby = Esri.OrderBy.None,
                                                    string format = "view", 
                                                    Boolean save = false) {
        // https://webmasters.stackexchange.com/questions/109117/usage-of-comma-in-url-encoded-or-not-encoded
            int[] pages2 = null;

            if (pages.Length>0) {
                pages2 = Array.ConvertAll<string, int>(pages.Split(','), Convert.ToInt32);
            } 
    
    return await ReadFeatureByGroup ( groupId, 
                                        dataset,
                                        where,
                                        page_size,
                                        pages2,
                                        orderby,
                                        format, 
                                        save );
}
[AllowAnonymous]
public async Task<IActionResult> ReadFeatureByGroup(Guid groupId, 
                                                    string dataset,
                                                    string where = "",
                                                    int page_size = 250,
                                                    int[] pages = null,
                                                    int orderby = Esri.OrderBy.None,
                                                    string format = "view", 
                                                    Boolean save = false) {

            if (groupId == null)
            {
                return NotFound();
            }

            if (String.IsNullOrEmpty(dataset)) {

                return NotFound();
            }

            ge_group _group = await _context.ge_group
                                    .Include(p=>p.projects)
                                    .SingleOrDefaultAsync(m => m.Id == groupId);
            
            if (_group == null)
            {
                return NotFound();
            }
            
            Guid[] projects = _group.projects.Select (m=>m.Id).Distinct().ToArray();
            
            return await ReadFeature (  projects, 
                                        dataset,
                                        where,
                                        page_size,
                                        pages,
                                        orderby,
                                        format, 
                                        save );

    }
}

}

       

