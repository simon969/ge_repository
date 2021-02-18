using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

using ge_repository.DAL;
using ge_repository.Extensions;
using ge_repository.interfaces;
using ge_repository.OtherDatabase;
using ge_repository.Models;
using ge_repository.ESRI;

namespace ge_repository.services {

    public class ESRIService<TParent, TChild, TGeom> : IEsriService where TParent: IEsriParent where TChild:IEsriChild where TGeom: IEsriGeometryWithAttributes {

    protected  readonly IEsriUnitOfWork<TParent,TChild,TGeom> _unitOfWork;
    
    public ESRIService(IEsriUnitOfWork<TParent,TChild,TGeom>  unitOfWork) {
            _unitOfWork = unitOfWork;
    }
    
    public async Task<List<MOND>> ReadFeature (     string where = "",
                                                    int page_size = 250,
                                                    int[] pages = null,
                                                    int orderby = Esri.OrderBy.None,
                                                    string format = "view", 
                                                    Boolean save = false) {

            var t1 = await _unitOfWork.ParentFeatureData.getFeatures(  where,
                                                                        page_size,
                                                                        pages,
                                                                        orderby
                                                                    );
            //Initialise list containers
            if (t1 == null) {
                return  null;
            }
            
            List<MOND> MOND_All = new List<MOND>();
            List<MONV> MONV_All =  new List<MONV>();

            foreach (string s1 in (string[]) t1) {
                if (s1 == null) { 
                    continue;
                }
                
                List<TParent> Survey_Data = new List<TParent>();
                List<TGeom> Survey_Geom = new List<TGeom>();
                List<TChild> Survey_Repeat_Data = new List<TChild>();
                
                var survey_data  = JsonConvert.DeserializeObject<esriFeature<TParent>>(s1);

                var survey_resp = LoadFeature(survey_data.features);
            
                Guid[] globalid = survey_data.features.Select (m=>m.attributes.globalid).Distinct().ToArray();
                // if (globalid.Count()>100 ) {
                //     return Json($"The parent record count({globalid.Count()}) will result in exceeding table limit of 1000 in the child table");
                // }
                string repeat_where = $"parentglobalid in ({globalid.ToDelimString(",","'")})";
                
                var t2 = await _unitOfWork.ChildFeatureData.getFeatures(   where,
                                                                            page_size,
                                                                            null,
                                                                            2
                                                                        );
                if (t2 == null) {
                    continue;
                }

                foreach (string s2 in (string[]) t2) {
                    if (s2 == null) { 
                        continue;
                    }
                    var survey_repeat  = JsonConvert.DeserializeObject<esriFeature<TChild>>(s2);
                    var repeat_resp = LoadFeature(survey_repeat.features);
                }

                // var mond_resp = await AddSurveyData(_project);
                
                // // if (mond_resp < 0 ) {
                // //     return Json("No records in MOND table");
                // // }

                // if (save == true) {
                //     var resp_mond = await UpdateMOND(MOND);

                //     var resp_monv = await UpdateMONV(MONV);
                // }
                
                // MOND_All.AddRange (MOND);
                // MONV_All.AddRange (MONV);
                
                // return MOND_All;

            }
            return null;
    }



private async Task<int> LoadFeature (List<items<TParent>>  survey_data) {
        
        if (_unitOfWork.ParentFeatureData.list == null) _unitOfWork.ParentFeatureData.list = new List<TParent>();
        if (_unitOfWork.GeomFeatureData.list == null) _unitOfWork.GeomFeatureData.list = new List<TGeom>();
        
        foreach (items<TParent> survey_items in survey_data) {
            
            TParent survey = survey_items.attributes;
            if (survey==null) {
               continue; 
            }

            EsriGeometry geom =  survey_items.geometry;
            TGeom geom2 = getLTMGeometry(geom, survey, Esri.esriWGS84);
            
            
           _unitOfWork.ParentFeatureData.list.Add (survey);
           _unitOfWork.GeomFeatureData.list.Add (geom2);
        }

        return _unitOfWork.ParentFeatureData.list.Count();
}


private TGeom getLTMGeometry(EsriGeometry geom, TParent survey, int wkid) {
    
        TGeom geom2 = (TGeom) Activator.CreateInstance(typeof(TGeom));

    //    geom2.hole_id = survey.hole_id;
    //    geom2.objectid = survey.objectid;
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
private EsriGeometry getEsriGeometry(TGeom geom) {
    
    EsriGeometry geom2 = new EsriGeometry();
        
        geom2.x = geom.x;
        geom2.y = geom.y;

    return geom2;

}
private  async Task<int> LoadFeature (List<items<TChild>> survey_data_repeat) {
        
            if (_unitOfWork.ChildFeatureData.list == null) _unitOfWork.ChildFeatureData.list = new List<TChild>();
            
            foreach (items<TChild> repeat_items in survey_data_repeat) {
                
                TChild survey2 = repeat_items.attributes;
                
                if (survey2 == null) {
                    continue; 
                }

                _unitOfWork.ChildFeatureData.list.Add (survey2);
            }

        return _unitOfWork.ChildFeatureData.list.Count();
}
private async Task<int> AddSurveyData(ge_project project) {
            
            
        //     string[] AllPoints = new string[] {""};
            
        //     MOND = new List<MOND>();
        //     MONV = new List<MONV>(); 
            
        //     var resp = await new ge_gINTController (_context,
        //                                             _authorizationService,
        //                                             _userManager,
        //                                             _env ,
        //                                             _ge_config
        //                                                 ).getMONG(project.Id,AllPoints);
        //     var okResult = resp as OkObjectResult;

        //     if (okResult == null) {
        //         return -1;
        //     } 
        
        //     MONG = okResult.Value as List<MONG>;
        //         if (MONG==null) { 
        //         return -1;
        //         }

        //     resp = await new ge_gINTController (_context,
        //                                             _authorizationService,
        //                                             _userManager,
        //                                             _env ,
        //                                             _ge_config
        //                                                 ).getPOINT(project.Id,AllPoints);

        //     okResult = resp as OkObjectResult;   
        //         if (okResult.StatusCode!=200) {
        //         return -1;
        //         } 
        
        //     POINT = okResult.Value as List<POINT>;
        //         if (POINT==null) { 
        //         return -1;
        //         }

           
            
        //     int projectId = POINT.FirstOrDefault().gINTProjectID;


        // foreach (LTM_Survey_Data2 survey in Survey_Data) {
                       
        //     if (survey==null) {
        //        continue; 
        //     }
            
        //     string point_id = survey.hole_id;

        //     if (map_hole_id.ContainsKey(survey.hole_id)) {
        //         point_id = map_hole_id[survey.hole_id];
        //     } 

        //     POINT pt = POINT.Find(p=>p.PointID==point_id);
            
        //     if (pt==null) {
        //         if ((survey.pack.Contains("A") && projectId==1) || 
        //             (survey.pack.Contains("B") && projectId==2) ||
        //             (survey.pack.Contains("C") && projectId==3) ||
        //             (survey.pack.Contains("D") && projectId==4)) {
        //         Console.WriteLine ("Package {0} survey.hole_id [{1}] not found", survey.pack, survey.hole_id);
        //         } 
        //         continue; 
               
        //     }

        //     List<MONG> pt_mg = MONG.Where(m=>m.PointID==pt.PointID).ToList();

        //     if (pt_mg.Count == 0) {
        //         Console.WriteLine ("Package {0} monitoring point for survey.hole_id {1} not found", survey.pack, survey.hole_id);
        //         continue;
        //     }
            
        //     string mong_id  = survey.mong_ID;

        //     MONG mg = null;
        //     MONG mg_topo =  null;

        //     if (map_mong_id.ContainsKey(survey.hole_id + survey.mong_ID)) {
        //         mong_id = map_mong_id[survey.hole_id + survey.mong_ID];
        //     } 
            
        //     mg =   pt_mg.Find(m=>m.ItemKey == mong_id);
            
        //     if (map_mong_id_gintrecid.ContainsKey(survey.hole_id + survey.mong_ID)) {
        //         int mong_gintrecid = map_mong_id_gintrecid[survey.hole_id + survey.mong_ID];
        //         mg = pt_mg.Find(m=>m.GintRecID == mong_gintrecid);
        //     } 

        //     if (mg == null) {
        //         mg = pt_mg.FirstOrDefault();
        //     } 
            
            

        //     DateTime? survey_start = gINTDateTime(survey.date1_getDT());

        //     if (survey_start==null) continue;
        //             AddVisit (pt, survey);
           
        //     if (survey.QA_status.Contains("Dip_Approved")) {
        //             AddDip(mg, survey);
        //     }
            
        //     if (survey.QA_status.Contains("Purge_Approved")) {
        //             AddPurge(mg, survey);
        //     }
            
        //     if (survey.QA_status.Contains("Gas_Approved")) {
        //             AddGas(mg, survey);
        //     }

        //     if (survey.QA_status.Contains("Topo_Approved") && 
        //         (survey.surv_g_level != null || 
        //         survey.meas_ToC != null)) { 

        //         mg_topo = pt_mg.Find(m=>m.MONG_DIS == 0);
                
        //         if (mg_topo == null) {
        //             mg_topo = new MONG();
        //             mg_topo.PointID = pt.PointID;
        //             mg_topo.PIPE_REF = "Pipe 1";
        //             mg_topo.ItemKey = "SM1";
        //             mg_topo.MONG_DIS = 0;
        //             mg_topo.MONG_DETL = "Ground Survey Monitoring Point";
        //             mg_topo.MONG_TYPE = "GNSS";
        //             pt_mg.Add (mg_topo);
        //             var resp_mong = await UpdateMONG (pt_mg);
        //             //if update fails set mg_topo=null;

        //         }

        //         if (mg_topo != null) {
        //             AddTopo(mg_topo, survey);
        //         }
        //     }

        // }

        // return MOND.Count();
     return 0;
}


}

}

