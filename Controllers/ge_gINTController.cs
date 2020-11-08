using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
//    [Route("api/[controller]")]
//    [ApiController]
    public class ge_gINTController: ge_Controller  {     
     private static string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
     private static string FILE_NAME_DATE_FORMAT = "{0:yyyy_MM_dd}";
     private static string DATETIME_FORMAT_AGS = "{0:yyyy-MM-ddTHH:mm:ss}";

     private static string DP2_FORMAT_AGS = "{0:#.00}";
     private static string DATE_FORMAT_AGS = "{0:yyyy-MM-dd}";
     private static string READING_FORMAT = "{0:#.00}";

     public static string AGS404= "4.04";
     public static string AGS403= "4.03";
     public static int NOT_FOUND = -1;
     public static string AGS_EXT = ".ags";

        public List<MOND> MOND {get; set;}
        public List<MONV> MONV {get; set;}
        public List<MONG> MONG {get;set;}          
        public List<POINT> POINT {get;set;}
        public List<ABBR> ABBR {get;set;}
        public List<TRAN> TRAN {get;set;}
        public List<TYPE> TYPE {get;set;}
        public List<UNIT> UNIT {get;set;}
        public List<PROJ> PROJ {get;set;}
        public List<ERES> ERES {get;set;}
        public List<SPEC> SPEC {get;set;}
        public List<SAMP> SAMP {get;set;}
      
         public ge_gINTController(

            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,  
            IHostingEnvironment env ,
            IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
           
        }
     

public async Task<IActionResult> getERES(Guid projectId, 
                                        string[] points,  
                                        string[] othergINTProjectId =  null, 
                                        string where = "", 
                                        string format="") {
            
            if (projectId == null) {
                return BadRequest("No projectId provided");
            }
            
            var project = await _context.ge_project
                                        .Include(p=>p.group)
                                        .SingleOrDefaultAsync(m => m.Id == projectId);
            
            if (project == null)
            {
                return BadRequest("project not found");
            }


            dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

            if (cd==null) {
                return BadRequest($"There is a problem with {project.name} gINT connection file");
            }

            string dbConnectStr = cd.AsConnectionString();
            int? gINTProjectId = cd.ProjectId;
            string sql_where = "";
            
            if (gINTProjectId == null) {
                return UnprocessableEntity("There is a problem with the gINTProjectId for {project.name} in gINT connection file");
            }

            if (othergINTProjectId != null) {
                othergINTProjectId = othergINTProjectId.Concat(new [] { Convert.ToString(gINTProjectId.Value)}).ToArray();
            } else {
                othergINTProjectId = new string[] {Convert.ToString(gINTProjectId.Value)};
            }
           
            
            if (!String.IsNullOrEmpty(points [0])) {
                sql_where += " and PointID in (" + points.ToDelimString(",","'") + ")";
            }
            
            if (where!=null) {
                sql_where += " " + where;                
            }

           
            ERES = await Task.Run(() =>
                    {
                    using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
                    {
                         cnn.Open();
                        dsTable ds_ERES = new gINTTables().ERES;
                        ds_ERES.setConnection (cnn);        
                        ds_ERES.getDataTable ();  
                        ds_ERES.sqlWhere(sql_where);
                        ds_ERES.getDataSet();
                        DataTable dt_ERES = ds_ERES.getDataTable();
                        
                        if (dt_ERES==null) {
                            return null;
                        } 
                        
                        if (dt_ERES.Rows.Count==0) {
                            return null;
                        }
                        
                        return ConvertDataTable<ERES>(dt_ERES);
                    }
            }
            );

            if (format=="view") {
                return View(ERES);
            }

            return Ok(ERES);

 }

 
 public async Task<IActionResult> getMONG(Guid projectId, 
                                          string[] points,  
                                          string[] othergINTProjectId =  null,
                                          string format="") {
            
            if (projectId == null) {
                return BadRequest("No projectId provided");
            }
            
            var project = await _context.ge_project
                                        .Include(p=>p.group)
                                        .SingleOrDefaultAsync(m => m.Id == projectId);
            
            if (project == null)
            {
                return BadRequest("project not found");
            }


            dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

            if (cd==null) {
                return BadRequest($"There is a problem with {project.name} gINT connection file");
            }

            string dbConnectStr = cd.AsConnectionString();
            int? gINTProjectId = cd.ProjectId;
            string sql_where = "";
            
            if (gINTProjectId == null) {
                return UnprocessableEntity("There is a problem with the gINTProjectId for {project.name} in gINT connection file");
            }

            if (othergINTProjectId != null) {
                othergINTProjectId = othergINTProjectId.Concat(new [] { Convert.ToString(gINTProjectId.Value)}).ToArray();
            } else {
                othergINTProjectId = new string[] {Convert.ToString(gINTProjectId.Value)};
            }
            
            sql_where = "gINTProjectId in (" + othergINTProjectId.ToDelimString(",","") + ")";
            
            points = points ?? new string[0];

            if (points.Length > 0 && !String.IsNullOrEmpty(points [0])) {
               sql_where += " and PointID in (" + points.ToDelimString(",","'") + ")";
            }
            
                       
            // if (mongs.Count>0) {
            //     sql_where +=" and (";
            //         foreach (MONG m in mongs) {
            //             sql_where += $"(PointID='{m.PointID} and ItemKey='{m.ItemKey}' and MONG_DIS={m.MONG_DIS}) or "; 
            //         }
            //     //remove last ' or '
            //     sql_where = sql_where.Substring(0,sql_where.Length-4);
            //     sql_where += ")";
            // }

            MONG = await Task.Run(() =>
                    {
                    using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
                    {
                         cnn.Open();
                        dsTable dsMONG = new gINTTables().MONG;
                        dsMONG.setConnection (cnn);        
                        dsMONG.getDataTable ();  
                        dsMONG.sqlWhere(sql_where);
                        dsMONG.getDataSet();
                        DataTable dt_MONG = dsMONG.getDataTable();
                        
                        if (dt_MONG==null) {
                            return null;
                        } 
                        
                        if (dt_MONG.Rows.Count==0) {
                            return null;
                        }
                        
                        return ConvertDataTable<MONG>( dt_MONG);
                    }
            }
            );

            if (format=="view") {
                return View(MONG);
            }

            return Ok(MONG);

 }

 private async Task<dbConnectDetails> GetDbConnectDetails(Guid projectId, string dbType) {
     
     var cs = await getOtherDbConnections(projectId);
     
     if (cs==null) {
         return null;
     }
     
    dbConnectDetails cd = cs.getConnectType(dbType);

    return cd;
}

private async Task<OtherDbConnections> getOtherDbConnections (Guid projectId) {
            
            if (projectId==null) {
            return  null;
            }
        
            var project = await _context.ge_project
                        .Include(p =>p.group)
                        .FirstOrDefaultAsync(m => m.Id == projectId);

            if (project == null) {
                return  null;
            }

            if (project.otherDbConnectId==null) {
            return  null;
            }

            var cs = await new ge_dataController(  _context,
                                                        _authorizationService,
                                                        _userManager,
                                                        _env ,
                                                        _ge_config).getDataAsClass<OtherDbConnections>(project.otherDbConnectId.Value); 

            return cs; 

    }
public async Task<int> deleteMOND (Guid projectId,
                                   int[] Id) {
            if (projectId == null) {
                return -1;
            }
            
            var project = await _context.ge_project
                                        .Include(p=>p.group)
                                    .SingleOrDefaultAsync(m => m.Id == projectId);
            
            if (project == null) {
                return -1;
            }
            

            dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

            if (cd==null) {
                throw new Exception ($"There is a problem with {project.name} gINT connection file");
            }

            string dbConnectStr = cd.AsConnectionString();
            int? gINTProjectId = cd.ProjectId;
            
            string sql_where = "gINTProjectID=" + gINTProjectId.Value;
            
            if (Id!=null) {
                sql_where += " and GintRecID in ("  + Id.ToCSV() + ")";
            }
            
            return await Task.Run(() =>
                    {
                    using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
                    {
                        dsTable dsMOND = new gINTTables().MOND;
                        cnn.Open();
                        dsMOND.setConnection (cnn);        
                        dsMOND.getDataTable ();  
                        dsMOND.sqlWhere(sql_where);
                        dsMOND.getDataSet();
                        DataTable dt_MOND = dsMOND.getDataTable();
                        
                        if (dt_MOND==null) {
                            return -1;
                        } 
                        
                        if (dt_MOND.Rows.Count==0) {
                            return -1;
                        }
                        foreach (DataRow row in dt_MOND.Rows) {
                            row.Delete();
                        }
                        // return dsMOND.Update();
                        return  dsMOND.Delete();
                    }
            }
            );

 }



//  public async Task<List<MOND>> getMOND (Guid projectId, 
//                                         string where) {
//             if (projectId == null) {
//                 return null;
//             }
            
//             var project = await _context.ge_project
//                                         .Include(p=>p.group)
//                                     .SingleOrDefaultAsync(m => m.Id == projectId);
            
//             if (project == null) {
//                 return null;
//             }
            

//             dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

//             if (cd==null) {
//                 throw new Exception ($"There is a problem with {project.name} gINT connection file");
//             }

//             string dbConnectStr = cd.AsConnectionString();
//             int? gINTProjectId = cd.ProjectId;
            
//             string sql_where = "gINTProjectId=" + gINTProjectId.Value;
            
//             if (!String.IsNullOrEmpty(where)) {
//                 sql_where += " and "  + where;
//             }
            
//             return await Task.Run(() =>
//                     {
//                     using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
//                     {
//                         dsTable dsMOND = new gINTTables().MOND;
//                         cnn.Open();
//                         dsMOND.setConnection (cnn);        
//                         dsMOND.getDataTable ();  
//                         dsMOND.sqlWhere(sql_where);
//                         dsMOND.getDataSet();
//                         DataTable dt_MOND = dsMOND.getDataTable();
                        
//                         if (dt_MOND==null) {
//                             return null;
//                         } 
                        
//                         if (dt_MOND.Rows.Count==0) {
//                             return null;
//                         }
                        
//                         return  ConvertDataTable<MOND>( dt_MOND);
//                     }
//             }
//             );

//  }
public async Task<IActionResult> getMOND(Guid projectId, 
                                        DateTime? fromDT, 
                                        DateTime? toDT,
                                        string[] points = null,
                                        string where = "",
                                        string format = "") {
return await getMOND (  projectId, 
                        fromDT, 
                        toDT,
                        points,
                        null,
                        where,
                        format);
}
private async Task<IActionResult> getMOND(Guid projectId, 
                                        DateTime? fromDT, 
                                        DateTime? toDT,
                                        string[] points = null,
                                        string[] othergINTProjectId =  null,
                                        string where = "",
                                        string format = "") {
            
            
            if (projectId == null)
            {
                return BadRequest("No projectId provided");
            }
            
            var project = await _context.ge_project
                                        .Include(p=>p.group)
                                    .SingleOrDefaultAsync(m => m.Id == projectId);
            
            if (project == null)
            {
                return BadRequest("No project found for projectId provided");
            }
            

            dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

            if (cd==null) {
                return UnprocessableEntity("There is a problem with {project.name} gINT connection file");
            }

            string dbConnectStr = cd.AsConnectionString();
            int? gINTProjectId = cd.ProjectId;
            string sql_where = "";
            
            if (gINTProjectId == null) {
                return UnprocessableEntity("There is a problem with the gINTProjectId for {project.name} in gINT connection file");
            }

            if (othergINTProjectId != null) {
                othergINTProjectId = othergINTProjectId.Concat(new [] { Convert.ToString(gINTProjectId.Value)}).ToArray();
            } else {
                othergINTProjectId = new string[] {Convert.ToString(gINTProjectId.Value)};
            }
           
            sql_where = "gINTProjectId in (" + othergINTProjectId.ToDelimString(",","") + ")";
            

            points = points ?? new string[0];

            if (points.Length > 0 && !String.IsNullOrEmpty(points [0])) {
               sql_where += " and PointID in (" + points.ToDelimString(",","'") + ")";
            }
            
            if (fromDT!=null) {
                sql_where += " and " + String.Format("DateTime>='{0:yyyy-MM-dd HH:mm:ss}'",fromDT.Value);
            }
            
            if (toDT!=null) {
                sql_where += " and " + String.Format("DateTime<='{0:yyyy-MM-dd HH:mm:ss}'",toDT.Value);
            }

            if (where!=null) {
                sql_where += " and " + where;                
            }

            MOND = await Task.Run(() =>
                    {
                    using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
                    {
                        dsTable dsMOND = new gINTTables().MOND;
                        cnn.Open();
                        dsMOND.setConnection (cnn);        
                        dsMOND.getDataTable ();  
                        dsMOND.sqlWhere(sql_where);
                        dsMOND.getDataSet();
                        DataTable dt_MOND = dsMOND.getDataTable();
                        
                        if (dt_MOND==null) {
                            return null;
                        } 
                        
                        if (dt_MOND.Rows.Count==0) {
                            return null;
                        }
                        
                        return  ConvertDataTable<MOND>( dt_MOND);
                    }
            }
            );

            if (format=="view") {
                return View("ViewMOND",MOND);
            }

            return Ok(MOND);

 }
 public async Task<List<MONV>> getMONV(Guid projectId,
                                        string[] points ) {
            
            if (projectId == null)
            
            {
                return null;
            }
            
            var project = await _context.ge_project
                                        .Include(p=>p.group)
                                    .SingleOrDefaultAsync(m => m.Id == projectId);
            
            if (project == null)
            {
                return null;
            }

            dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

            if (cd==null) {
                throw new Exception ($"There is a problem with {project.name} gINT connection file");
            }

            string dbConnectStr = cd.AsConnectionString();
            int? gINTProjectId = cd.ProjectId;
            
            string sql_where = "gINTProjectId=" + gINTProjectId.Value;
            
          if (!String.IsNullOrEmpty(points [0])) {
                sql_where += " and PointID in (" + points.ToDelimString(",","'") + ")";
            }

            return await Task.Run(() =>
                    {
                    using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
                    {
                        dsTable dsMONV = new gINTTables().MONV;
                        cnn.Open();
                        dsMONV.setConnection (cnn);        
                        dsMONV.getDataTable ();  
                        dsMONV.sqlWhere("gINTProjectId=" + gINTProjectId.Value );
                        dsMONV.getDataSet();
                        DataTable dt_MONV = dsMONV.getDataTable();
                        
                        if (dt_MONV==null) {
                            return null;
                        } 
                        
                        if (dt_MONV.Rows.Count==0) {
                            return null;
                        }
                        
                        return  ConvertDataTable<MONV>( dt_MONV);
                    }
            }
            );

 }
public async Task<IActionResult> getTRAN(Guid projectId, string format = "") {
            
            if (projectId == null)
            {
                return BadRequest("No projectId provided");
            }

            var project = await _context.ge_project
                                        .Include(p=>p.group)
                                    .SingleOrDefaultAsync(m => m.Id == projectId);
           
            if (project == null)
            {
                return BadRequest("This project is not found");
            }


            dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

            if (cd==null) {
                return BadRequest("This project has no connection details");
            }

            string dbConnectStr = cd.AsConnectionString();
            int? gINTProjectId = cd.ProjectId;
            
            if (gINTProjectId==null) {
                return BadRequest("This project connection file has no gINTProjectId");
            }

            string sql_where = "gINTProjectId=" + gINTProjectId.Value;
           
            List<TRAN> TRAN = await Task.Run(() => {
                            using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
                            {
                                dsTable dst= new gINTTables().TRAN;
                                cnn.Open();
                                dst.setConnection (cnn);        
                                dst.getDataTable ();  
                                dst.sqlWhere( sql_where );
                                dst.getDataSet();
                                DataTable dt = dst.getDataTable();
                                
                                if (dt==null) {
                                    return null;
                                } 
                                
                                if (dt.Rows.Count==0) {
                                    return null;
                                }
                                
                                return  ConvertDataTable<TRAN>(dt);
                            }
                    }
            );

            if (format=="view") {
                return View(TRAN);
            }

            return Ok(TRAN);

 }
public async Task<List<PROJ>> getPROJ(Guid projectId) {
            
            if (projectId == null)
            {
                return null;
            }

            var project = await _context.ge_project
                                        .Include(p=>p.group)
                                    .SingleOrDefaultAsync(m => m.Id == projectId);
           
            if (project == null)
            {
                return null;
            }


            dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

            if (cd==null) {
                return null;
            }

            string dbConnectStr = cd.AsConnectionString();
            int? gINTProjectId = cd.ProjectId;
            
            if (gINTProjectId==null) {
                return null;
            }

            string sql_where = "gINTProjectId=" + gINTProjectId.Value;
           
            return await Task.Run(() =>
                    {
                    using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
                    {
                        dsTable dsPROJ = new gINTTables().PROJ;
                        cnn.Open();
                        dsPROJ.setConnection (cnn);        
                        dsPROJ.getDataTable ();  
                        dsPROJ.sqlWhere( sql_where );
                        dsPROJ.getDataSet();
                        DataTable dt_PROJ = dsPROJ.getDataTable();
                        
                        if (dt_PROJ==null) {
                            return null;
                        } 
                        
                        if (dt_PROJ.Rows.Count==0) {
                            return null;
                        }
                        
                        return  ConvertDataTable<PROJ>( dt_PROJ);
                    }
            }
            );

 }


  public async Task<IActionResult> getPOINT(Guid projectId, 
                                            string[] points = null, 
                                            string[] othergINTProjectId = null, 
                                            string format="") {
            
            if (projectId == null)
            {
                return null;
            }

            var project = await _context.ge_project
                                        .Include(p=>p.group)
                                        .SingleOrDefaultAsync(m => m.Id == projectId);
           
            if (project == null)
            {
                return null;
            }


            dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

            if (cd==null) {
                return null;
            }

            string dbConnectStr = cd.AsConnectionString();
            int? gINTProjectId = cd.ProjectId;
            string sql_where = "";
            
            if (gINTProjectId == null) {
                return UnprocessableEntity("There is a problem with the gINTProjectId for {project.name} in gINT connection file");
            }

            if (othergINTProjectId != null) {
                othergINTProjectId = othergINTProjectId.Concat(new [] { Convert.ToString(gINTProjectId.Value)}).ToArray();
            } else {
                othergINTProjectId = new string[] {Convert.ToString(gINTProjectId.Value)};
            }
            
            sql_where = "gINTProjectId in (" + othergINTProjectId.ToDelimString(",","") + ")";
                 
            points = points ?? new string[0];

            if (points.Length > 0 && !String.IsNullOrEmpty(points [0])) {
                sql_where += " and PointID in (" + points.ToDelimString(",","'") + ")";
            }

            POINT = await Task.Run(() =>
                    {
                    using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
                    {
                        dsTable dsPOINT = new gINTTables().POINT;
                        cnn.Open();
                        dsPOINT.setConnection (cnn);        
                        dsPOINT.getDataTable ();  
                        dsPOINT.sqlWhere( sql_where );
                        dsPOINT.getDataSet();
                        DataTable dt_POINT = dsPOINT.getDataTable();
                        
                        if (dt_POINT==null) {
                            return null;
                        } 
                        
                        if (dt_POINT.Rows.Count==0) {
                            return null;
                        }
                        
                        return  ConvertDataTable<POINT>( dt_POINT);
                    }
            }
            );

            if (format=="view") {
                return View(POINT);
            }

            return Ok(POINT);

 }

[HttpPost]  
  
public async Task<IActionResult> createAGS(Guid Id, 
                                           string[] holes,
                                           string filename,
                                           string[] tables,     
                                           DateTime? fromDT, 
                                           DateTime? toDT,  
                                           Guid? appendId,
                                           string[] othergINTProjectId = null,
                                           string where = null,
                                           string version = "4.04", 
                                           string format = "view", 
                                           Boolean save = false) {
            
            if (Id == Guid.Empty )
            {
                return NotFound();
            }
            
            var _project = await _context.ge_project
                                    .Include(p=>p.group)
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            if (_project == null)
            {
                return NotFound();
            }

            var _empty_data = new ge_data();
            _empty_data.project = _project;
            
            var user = GetUserAsync().Result;

            if (user != null) {
                    
                    int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, _project, _empty_data);
                    Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName, _project, user.Id);

                    if (IsCreateAllowed!=geOPSResp.Allowed) {
                        return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
                    }
                    if (!CanUserCreate) {
                    return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
                    }

            }
            
              
            if (tables.Contains("MOND")) {
            var resp = await getMOND (Id, fromDT, toDT, holes, othergINTProjectId, where,"");
            var okResult = resp as OkObjectResult;   
                if (okResult.StatusCode==200) {
                    MOND = okResult.Value as List<MOND>;
                }
            }

            string[] SelectPoints = null ;
            
            if (holes[0] == null && MOND != null) {
            SelectPoints = MOND.Select (m=>m.PointID).Distinct().ToArray();
            } else {
            SelectPoints = holes;
            }
            
            // if (holes != null) {
            //     bool allPointsInMOND = !holes.Except(MOND_Distinct).Any();
            //     if (allPointsInMOND) {
            //         SelectPoints  = holes;
            //         } else {
            //             return Json ("[" + MOND_Distinct  + "] does not contain all elements of  [" + holes + "]");
            //         }
            // } else {
            //     SelectPoints = MOND_Distinct;
            // }      

                      
            StringBuilder sb = new StringBuilder();

          
            if (tables.Contains("PROJ-min")) {
            PROJ = await getPROJ (Id) ;
                if (PROJ != null) {
                    sb.Append (getAGSTable(PROJ, version, true));
                sb.AppendLine();
                }
            }

            if (tables.Contains("PROJ")) {
            PROJ = await getPROJ (Id) ;
                if (PROJ != null) {
                    sb.Append (getAGSTable(PROJ, version, false));
                    sb.AppendLine();
                }
            }
            
            if (tables.Contains("POINT-min")) {
                var resp = await getPOINT (Id, SelectPoints, othergINTProjectId);  
                var okResult = resp as OkObjectResult;   
                if (okResult.StatusCode==200) {
                    POINT = okResult.Value as List<POINT>;
                    if (POINT != null) {
                        sb.Append (getAGSTable(POINT, version, true));
                        sb.AppendLine();
                    }
                }
            }
            
            if (tables.Contains("POINT")) {
                var resp = await getPOINT (Id, SelectPoints, othergINTProjectId);  
                var okResult = resp as OkObjectResult;   
                if (okResult.StatusCode==200) {
                    POINT = okResult.Value as List<POINT>;
                    if (POINT!=null) { 
                        sb.Append (getAGSTable(POINT, version, false));
                        sb.AppendLine();
                    }
                }
            }
            
            if (tables.Contains("TRAN")) {
                var resp = await getTRAN(Id);
                var okResult = resp as OkObjectResult;   
                if (okResult.StatusCode==200) {
                    TRAN = okResult.Value as List<TRAN>;
                    if (TRAN!=null) {
                    sb.Append (getAGSTable(TRAN, version, false));
                    sb.AppendLine();
                    }
                }
            }
            
            if (tables.Contains("MONG")) {
                var resp = await getMONG(Id, SelectPoints, othergINTProjectId);
                var okResult = resp as OkObjectResult;   
                if (okResult.StatusCode==200) { 
                    MONG = okResult.Value as List<MONG>;
                    if (MONG!=null) {
                    sb.Append (getAGSTable(MONG, version, true));
                    sb.AppendLine();
                    }
                }
            }
            
            if (tables.Contains("MOND")) {
                if (MOND!=null) {
                    sb.Append (getAGSTable(MOND, version));
                    sb.AppendLine();
                }
            }

            if (tables.Contains("ABBR")) {
                var abbr = await getUniqueABBR(appendId);
                if (ABBR!=null) {
                    sb.Append (getAGSTable(ABBR, version));
                    sb.AppendLine();
                }
            }

            if (appendId != null) {
            string add_file  = await new ge_dataController(  _context,
                                                        _authorizationService,
                                                        _userManager,
                                                        _env ,
                                                        _ge_config).getDataAsString(appendId.Value); 
            sb.Append(add_file);

            }

           
            string s1 = sb.ToString();

            if (s1.Length==0) {
                return Json ("No data found");
            }        

            if (filename == null) {
                filename = "AGS Export "  + String.Format(FILE_NAME_DATE_FORMAT,DateTime.Now) + AGS_EXT;
            }

            if (filename.LastIndexOf(AGS_EXT) == NOT_FOUND) {
                    filename += AGS_EXT;
            } 
           

                var _data =  new ge_data {
                            Id = Guid.NewGuid(),
                            createdId = user.Id,
                            createdDT = DateTime.Now,
                            editedDT = DateTime.Now,
                            editedId = user.Id,
                            filename = filename,
                            filesize = s1.Length,
                            fileext = ".ags",
                            filetype = "text/plain",
                            filedate = DateTime.Now,
                            encoding ="utf-16",
                            datumProjection = datumProjection.NONE,
                            pstatus = PublishStatus.Uncontrolled,
                            cstatus = ConfidentialityStatus.RequiresClientApproval,
                            version= "P01.1",
                            vstatus= VersionStatus.Intermediate,
                            qstatus = QualitativeStatus.AECOMFactual,
                            description= _project.name + " AGS data (" + String.Format(FILE_NAME_DATE_FORMAT,fromDT) + " to " + String.Format(FILE_NAME_DATE_FORMAT,toDT) + ") for Holes " + SelectPoints.ToDelimString(","),
                            operations ="Read;Download;Update;Delete",
                            data = new ge_data_big {
                                 data_string = s1
                                }
                            };
            if (save) {
                 _project.data.Add(_data);
                 _context.SaveChanges();
            }

            byte[] ags_utf16 = Encoding.Unicode.GetBytes(s1); 
                      
            if (format =="download") {
             // download to file can be utf-16  
            return File ( ags_utf16, "text/plain", _data.filename );
            }

            
            if (format == null || format=="view") {
            //viewing in browser can only be ASCII
            byte[] ags_ascii = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, ags_utf16);        
            return File (ags_ascii, "text/plain");
            }

            return NotFound();

}
private async Task<int> getUniqueABBR(Guid? fileId) {

// if (fileId != null) {
//     var abbr = await new ge_agsController(_context,
//                                         _authorizationService,
//                                         _userManager,
//                                         _env ,
//                                         _ge_config).ReadABBR(fileId.Value); 
// }

// string[] mond_type =  MOND.Select (m=>m.MOND_TYPE).Distinct().ToArray();
// string[] loca_type = POINT.Select (m=>m.LOCA_TYPE).Distinct().ToArray();



return 0;

}

private string getAGSTable(List<MONG> rows, 
                        string version, Boolean min = false) {
    
    if (version!=AGS404  && version!=AGS403) {
        return "";
    } 

    string table_name =     "\"GROUP\",\"MONG\"";
    string table_headings = "\"HEADING\",\"LOCA_ID\",\"MONG_ID\",\"MONG_DIS\",\"PIPE_REF\",\"MONG_DATE\",\"MONG_TYPE\",\"MONG_DETL\",\"MONG_TRZ\",\"MONG_BRZ\",\"MONG_BRGA\",\"MONG_BRGB\",\"MONG_BRGC\",\"MONG_INCA\",\"MONG_INCB\",\"MONG_INCC\",\"MONG_RSCA\",\"MONG_RSCB\",\"MONG_RSCC\",\"MONG_REM\",\"MONG_CONT\",\"FILE_FSET\"";
    string table_units =  "\"UNIT\",\"\",\"\",\"m\",\"\",\"yyyy-mm-dd\",\"\",\"\",\"m\",\"m\",\"deg\",\"deg\",\"deg\",\"deg\",\"deg\",\"deg\",\"\",\"\",\"\",\"\",\"\",\"\"";
    string table_types =    "\"TYPE\",\"ID\",\"X\",\"2DP\",\"X\",\"PA\",\"X\",\"X\",\"XN\",\"PU\",\"X\",\"U\",\"U\",\"X\",\"X\",\"X\",\"X\",\"X\",\"X\",\"X\",\"X\",\"X\"";
    
    StringBuilder sb = new StringBuilder();
    sb.Append (table_name);
    sb.AppendLine();
    sb.Append (table_headings);
    sb.AppendLine();
    sb.Append (table_units);
    sb.AppendLine();
    sb.Append (table_types);
    sb.AppendLine();

    foreach (MONG row in rows) {
        string line = $"\"DATA\",\"{row.PointID}\",\"{row.ItemKey}\",\"{String.Format(DP2_FORMAT_AGS,row.MONG_DIS)}\",\"{row.PIPE_REF}\",\"{String.Format(DATE_FORMAT_AGS,row.MONG_DATE)}\",\"{row.MONG_TYPE}\",\"{row.MONG_DETL}\",\"{row.MONG_TRZ}\",\"{row.MONG_BRZ}\",\"{row.MONG_BRGA}\",\"{row.MONG_BRGB}\",\"{row.MONG_BRGC}\",\"{row.MONG_INCA}\",\"{row.MONG_INCB}\",\"{row.MONG_INCC}\",\"{row.MONG_RSCA}\",\"{row.MONG_RSCB}\",\"{row.MONG_RSCC}\",\"{row.MONG_REM}\",\"{row.MONG_CONT}\",\"{row.FILE_FSET}\"";
        sb.Append(line);
        sb.AppendLine();
    }
    
    return sb.ToString();
}
private string getAGSTable(List<GRAT> rows, 
                        string version, Boolean min = false) {
    
    if (version!=AGS404  && version!=AGS403) {
        return "";
    } 
    
    //"GROUP","GRAT"
    //"HEADING","LOCA_ID","SAMP_TOP","SAMP_REF","SAMP_TYPE","SAMP_ID","SPEC_REF","SPEC_DPTH","GRAT_SIZE","GRAT_PERP","GRAT_TYPE","GRAT_REM","FILE_FSET"
    //"UNIT","","m","","","","","m","mm","%","","",""
    //"TYPE","ID","2DP","X","PA","ID","X","2DP","3SF","0DP","PA","X","X"

    string table_name =     "\"GROUP\",\"GRAT\"";
    string table_headings = "\"HEADING\",\"LOCA_ID\",\"SAMP_TOP\",\"SAMP_REF\",\"SAMP_TYPE\",\"SAMP_ID\",\"SPEC_REF\",\"SPEC_DPTH\",\"GRAT_SIZE\",\"GRAT_PERP\",\"GRAT_TYPE\",\"GRAT_REM\",\"FILE_FSET\"";
    string table_units =  "\"UNIT\",\"\",\"m\",\"\",\"\",\"\",\"\",\"m\",\"mm\",\"%\",\"\",\"\",\"\"";
    string table_types =    "\"TYPE\",\"ID\",\"2DP\",\"X\",\"PA\",\"ID\",\"X\",\"2DP\",\"3SF\",\"0DP\",\"PA\",\"X\",\"X\"";
    
    StringBuilder sb = new StringBuilder();
    sb.Append (table_name);
    sb.AppendLine();
    sb.Append (table_headings);
    sb.AppendLine();
    sb.Append (table_units);
    sb.AppendLine();
    sb.Append (table_types);
    sb.AppendLine();

    foreach (GRAT row in rows) {
        string line = $"\"DATA\",\"{row.PointID}\",\"{row.SAMP_Depth}\",\"{row.SAMP_REF}\",\"{row.SAMP_TYPE}\",\"{row.SAMP_ID}\",\"{row.SPEC_REF}\",\"{row.Depth}\",\"{row.Reading}\",\"{row.GRAT_PERP}\",\"{row.GRAT_TYPE}\",\"{row.GRAT_REM}\",\"{row.FILE_FSET}\"";
        sb.Append(line);
        sb.AppendLine();
    }
    
    return sb.ToString();
}
private string getAGSTable(List<GRAG> rows, 
                        string version, Boolean min = false) {
    
    if (version!=AGS404  && version!=AGS403) {
        return "";
    } 
    // "GROUP","GRAG"
    // "HEADING","LOCA_ID","SAMP_TOP","SAMP_REF","SAMP_TYPE","SAMP_ID","SPEC_REF","SPEC_DPTH","SPEC_DESC","SPEC_PREP","GRAG_UC","GRAG_VCRE","GRAG_GRAV","GRAG_SAND","GRAG_SILT","GRAG_CLAY","GRAG_FINE","GRAG_REM","GRAG_METH","GRAG_LAB","GRAG_CRED","TEST_STAT","FILE_FSET"
    // "UNIT","","m","","","","","m","","","","%","%","%","%","%","%","","","","","",""
    // "TYPE","ID","2DP","X","PA","ID","X","2DP","X","X","1SF","1DP","1DP","1DP","1DP","1DP","1DP","X","X","X","X","X","X"

    string table_name =     "\"GROUP\",\"GRAG\"";
    string table_headings = "\"HEADING\",\"LOCA_ID\",\"SAMP_TOP\",\"SAMP_REF\",\"SAMP_TYPE\",\"SAMP_ID\",\"SPEC_REF\",\"SPEC_DPTH\",\"SPEC_DESC\",\"SPEC_PREP\",\"GRAG_UC\",\"GRAG_VCRE\",\"GRAG_GRAV\",\"GRAG_SAND\",\"GRAG_SILT\",\"GRAG_CLAY\",\"GRAG_FINE\",\"GRAG_REM\",\"GRAG_METH\",\"GRAG_LAB\",\"GRAG_CRED\",\"TEST_STAT\",\"FILE_FSET\"";
    string table_units =  "\"UNIT\",\"\",\"m\",\"\",\"\",\"\",\"\",\"m\",\"\",\"\",\"\",\"%\",\"%\",\"%\",\"%\",\"%\",\"%\",\"\",\"\",\"\",\"\",\"\",\"\"";
    string table_types =    "\"TYPE\",\"ID\",\"2DP\",\"X\",\"PA\",\"ID\",\"X\",\"2DP\",\"X\",\"X\",\"1SF\",\"1DP\",\"1DP\",\"1DP\",\"1DP\",\"1DP\",\"1DP\",\"X\",\"X\",\"X\",\"X\",\"X\",\"X\"";
    
    StringBuilder sb = new StringBuilder();
    sb.Append (table_name);
    sb.AppendLine();
    sb.Append (table_headings);
    sb.AppendLine();
    sb.Append (table_units);
    sb.AppendLine();
    sb.Append (table_types);
    sb.AppendLine();

    foreach (GRAG row in rows) {
        string line = $"\"DATA\",\"{row.PointID}\",\"{row.SAMP_Depth}\",\"{row.SAMP_REF}\",\"{row.SAMP_TYPE}\",\"{row.SAMP_ID}\",\"{row.SPEC_REF}\",\"{row.Depth}\",\"{row.SPEC_DESC}\",\"{row.SPEC_PREP}\",\"{row.GRAG_UC}\",\"{row.GRAG_VCRE}\",\"{row.GRAG_GRAV}\",\"{row.GRAG_SAND}\",\"{row.GRAG_SILT}\",\"{row.GRAG_CLAY}\",\"{row.GRAG_FINE}\",\"{row.GRAG_REM}\",\"{row.GRAG_METH}\",\"{row.GRAG_LAB}\",\"{row.GRAG_CRED}\",\"{row.TEST_STAT}\",\"{row.FILE_FSET}\"";
        sb.Append(line);
        sb.AppendLine();
    }
    
    return sb.ToString();
}
public async Task<IActionResult>  getAGSTable(List<GRAG_WC> rows, string[] tables,
                        string version, Boolean min = false) {
        
        List<POINT> pt = new List<POINT>();
        List<SAMP> smp = new List<SAMP>();
        List<GRAG> gg = new List<GRAG>();
        List<GRAT> gt = new List<GRAT>();
 
        foreach (GRAG_WC row in rows) {
            GRAG g =  (GRAG) row;
                gg.Add (g);
            foreach(GRAT row2 in row.GRAT) {
                gt.Add (row2);    
            }
        }
        
        string grag =  getAGSTable(gg, version, min);
        string grat =  getAGSTable(gt, version, min);

        StringBuilder sb = new StringBuilder();
        
        sb.Append (grag);
        sb.AppendLine();
        sb.Append (grat);

        return Ok(sb.ToString());

}

private string getAGSTable(List<ABBR> rows, 
                        string version, bool min = false) {
    
    if (version!=AGS404 && version!=AGS403) {
        return "";
    } 

    string table_name = "\"GROUP\",\"ABBR\"";
    string table_headings = "\"HEADING\",\"ABBR_HDNG\",\"ABBR_CODE\",\"ABBR_DESC\",\"ABBR_LIST\",\"ABBR_REM\",\"FILE_FSET\"";
    string table_units = "\"UNIT\",\"\",\"\",\"\",\"\",\"\",\"\"";
    string table_types = "\"TYPE\",\"X\",\"X\",\"X\",\"X\",\"X\",\"X\"";
    
    StringBuilder sb = new StringBuilder();
    sb.Append (table_name);
    sb.AppendLine();
    sb.Append (table_headings);
    sb.AppendLine();
    sb.Append (table_units);
    sb.AppendLine();
    sb.Append (table_types);
    sb.AppendLine();

    foreach (ABBR row in rows) {
        string line = $"\"DATA\",\"{row.ABBR_HDNG}\",\"{row.ABBR_CODE}\",\"{row.ABBR_DESC}\",\"{row.ABBR_LIST}\",\"{row.ABBR_REM}\",\"{row.FILE_FSET}\"";
        sb.Append(line);
        sb.AppendLine();
    }
    
    return sb.ToString();
}
private string getAGSTable(List<TYPE> rows, 
                        string version, bool min = false) {
    
    if (version!=AGS404 && version!=AGS403) {
        return "";
    } 

    string table_name = "\"GROUP\",\"TYPE\"";
    string table_headings = "\"HEADING\",\"TYPE_TYPE\",\"TYPE_DESC\",\"FILE_FSET\"";
    string table_units = "\"UNIT\",\"\",\"\",\"\"";
    string table_types = "\"TYPE\",\"X\",\"X\",\"X\"";
    
    StringBuilder sb = new StringBuilder();
    sb.Append (table_name);
    sb.AppendLine();
    sb.Append (table_headings);
    sb.AppendLine();
    sb.Append (table_units);
    sb.AppendLine();
    sb.Append (table_types);
    sb.AppendLine();

    foreach (TYPE row in rows) {
        string line = $"\"DATA\",\"{row}\",\"{row.TYPE_TYPE}\",\"{row.TYPE_DESC}\",\"{row.FILE_FSET}\"";
        sb.Append(line);
        sb.AppendLine();
    }
    
    return sb.ToString();
}

private string REM_Clean(String remark) {
    if (remark==null) {
        return "";
    }

    remark = remark.Replace("\n", " ");
    remark = remark.Replace("\r", " ");

    return remark;

}
private string getAGSTable(List<MOND> rows, 
                        string version, bool min = false) {
    
    if (version!=AGS404 && version!=AGS403) {
        return "";
    } 
    
    string table_name = "\"GROUP\",\"MOND\"";
    string table_headings = "\"HEADING\",\"LOCA_ID\",\"MONG_ID\",\"MONG_DIS\",\"MOND_DTIM\",\"MOND_TYPE\",\"MOND_REF\",\"MOND_INST\",\"MOND_RDNG\",\"MOND_UNIT\",\"MOND_METH\",\"MOND_LIM\",\"MOND_ULIM\",\"MOND_NAME\",\"MOND_CRED\",\"MOND_CONT\",\"MOND_REM\",\"FILE_FSET\"";
    string table_units = "\"UNIT\",\"\",\"\",\"m\",\"yyyy-mm-ddThh:mm:ss\",\"m\",\"\",\"\",\"m\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"";
    string table_types = "\"TYPE\",\"ID\",\"X\",\"2DP\",\"DT\",\"PA\",\"X\",\"X\",\"XN\",\"PU\",\"X\",\"U\",\"U\",\"X\",\"X\",\"X\",\"X\",\"X\"";
    
    StringBuilder sb = new StringBuilder();
    sb.Append (table_name);
    sb.AppendLine();
    sb.Append (table_headings);
    sb.AppendLine();
    sb.Append (table_units);
    sb.AppendLine();
    sb.Append (table_types);
    sb.AppendLine();

    foreach (MOND row in rows) {
        string line = $"\"DATA\",\"{row.PointID}\",\"{row.ItemKey}\",\"{String.Format(DP2_FORMAT_AGS,row.MONG_DIS)}\",\"{String.Format(DATETIME_FORMAT_AGS,row.DateTime)}\",\"{row.MOND_TYPE}\",\"{row.MOND_REF}\",\"{row.MOND_INST}\",\"{row.MOND_RDNG}\",\"{row.MOND_UNIT}\",\"{row.MOND_METH}\",\"{row.MOND_LIM}\",\"{row.MOND_ULIM}\",\"{row.MOND_NAME}\",\"{row.MOND_CRED}\",\"{row.MOND_CONT}\",\"{REM_Clean(row.MOND_REM)}\",\"{row.FILE_FSET}\"";
        if (row.MOND_TYPE =="EC") {
            Console.Write (line);
        }
        sb.Append(line);
        sb.AppendLine();
    }
    
    return sb.ToString();
}
private string getAGSTable(List<TRAN> rows, 
                        string version, Boolean min = false) {
    
    if (version!=AGS404  && version!=AGS403) {
        return "";
    } 

    string table_name =     "\"GROUP\",\"MONG\"";
    string table_headings = "\"HEADING\",\"TRAN_ISNO\",\"TRAN_DATE\",\"TRAN_PROD\",\"TRAN_STAT\",\"TRAN_DESC\",\"TRAN_AGS\",\"TRAN_RECV\",\"TRAN_DLIM\",\"TRAN_RCON\",\"TRAN_REM\",\"FILE_FSET\"";
    string table_units =    "\"UNIT\",\"\",\"yyyy-mm-dd\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"";
    string table_types =    "\"TYPE\",\"X\",\"DT\",\"X\",\"X\",\"X\",\"X\",\"X\",\"X\",\"X\",\"X\",\"X\"";
    
    StringBuilder sb = new StringBuilder();
    sb.Append (table_name);
    sb.AppendLine();
    sb.Append (table_headings);
    sb.AppendLine();
    sb.Append (table_units);
    sb.AppendLine();
    sb.Append (table_types);
    sb.AppendLine();

    foreach (TRAN row in rows) {
        string line = $"\"DATA\",\"{row.ItemKey}\",\"{row.TRAN_DATE}\",\"{row.Process}\",\"{row.TRAN_PROD}\",\"{row.TRAN_STAT}\",\"{row.TRAN_DESC}\",\"{row.TRAN_AGS}\",\"{row.TRAN_RECV}\",\"{row.TRAN_DLIM}\",\"{row.TRAN_RCON}\",\"{row.TRAN_REM}\",\"{row.FILE_FSET}\"";
        sb.Append(line);
        sb.AppendLine();
    }
    
    return sb.ToString();
}
private string getAGSTable(List<PROJ> rows, 
                        string version, Boolean min = false) {
    
    if (version!=AGS404 && version!=AGS403) {
        return "";
    } 

    string table_name =     "\"GROUP\",\"PROJ\"";
    string table_headings = "";
    string table_units = "";
    string table_types = "";

    if (version==AGS403 &&  min==false) {
    table_headings = "\"HEADING\",\"PROJ_ID\",\"PROJ_NAME\",\"PROJ_LOC\",\"PROJ_CLNT\",\"PROJ_CONT\",\"PROJ_ENG\",\"PROJ_MEMO\",\"FILE_FSET\"";
    table_units =    "\"UNIT\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"";
    table_types =    "\"TYPE\",\"ID\",\"X\",\"X\",\"X\",\"X\",\"X\",\"X\",\"X\"";
    }
    
    if (version==AGS404) {
     
    }

    if (min==true && (version== AGS404 || version==AGS403)) {
     table_headings = "\"HEADING\",\"PROJ_ID\",\"PROJ_NAME\"";
     table_units =    "\"UNIT\",\"\",\"\"";
     table_types =    "\"TYPE\",\"ID\",\"X\"";
    }

    StringBuilder sb = new StringBuilder();
    sb.Append (table_name);
    sb.AppendLine();
    sb.Append (table_headings);
    sb.AppendLine();
    sb.Append (table_units);
    sb.AppendLine();
    sb.Append (table_types);
    sb.AppendLine();
    
    if (version==AGS403 && min==false) {
        foreach (PROJ row in rows) {
            string line = $"\"DATA\",\"{row.PROJ_ID}\",\"{row.PROJ_NAME}\",\"{row.PROJ_LOC}\",\"{row.PROJ_CLNT}\",\"{row.PROJ_CONT}\",\"{row.PROJ_ENG}\",\"{row.PROJ_MEMO}\",\"{row.FILE_FSET}\"";
            sb.Append(line);
            sb.AppendLine();
        }
    }
    
   if (min==true && (version== AGS404 || version==AGS403)) {
        foreach (PROJ row in rows) {
            string line = $"\"DATA\",\"{row.PROJ_ID}\",\"{row.PROJ_NAME}\"";
            sb.Append(line);
            sb.AppendLine();
        }
    }

    return sb.ToString();
}
private string getAGSTable(List<POINT> rows, 
                        string version, Boolean min = false) {
    
    if (version!=AGS404 && version!=AGS403) {
        return "";
    } 

    string table_name =     "\"GROUP\",\"LOCA\"";
    string table_headings = "";
    string table_units = "";
    string table_types = "";

    if (version==AGS404 && min==false) {
    table_headings = "\"HEADING\",\"LOCA_ID\",\"LOCA_TYPE\",\"LOCA_STAT\",\"LOCA_NATE\",\"LOCA_NATN\",\"LOCA_GREF\",\"LOCA_GL\",\"LOCA_REM\",\"LOCA_FDEP\",\"LOCA_STAR\",\"LOCA_PURP\",\"LOCA_TERM\",\"LOCA_ENDD\",\"LOCA_LETT\",\"LOCA_LOCX\",\"LOCA_LOCY\",\"LOCA_LOCZ\",\"LOCA_LREF\",\"LOCA_DATM\",\"LOCA_ETRV\",\"LOCA_NTRV\",\"LOCA_LTRV\",\"LOCA_XTRL\",\"LOCA_YTRL\",\"LOCA_ZTRL\",\"LOCA_LAT\",\"LOCA_LON\",\"LOCA_ELAT\",\"LOCA_ELON\",\"LOCA_LLZ\",\"LOCA_LOCM\",\"LOCA_LOCA\",\"LOCA_CLST\",\"LOCA_ALID\",\"LOCA_OFFS\",\"LOCA_CNGE\",\"LOCA_TRAN\",\"FILE_FSET\",\"LOCA_NATD\",\"LOCA_ORID\",\"LOCA_ORJO\",\"LOCA_CHKG\",\"LOCA_CKDT\",\"LOCA_APPG\"";
    table_units =    "\"UNIT\",\"\",\"\",\"\",\"m\",\"m\",\"m\",\"m\",\"\",\"m\",\"yyyy-mm-dd\",\"\",\"\",\"yyyy-mm-dd\",\"\",\"m\",\"m\",\"m\",\"\",\"\",\"m\",\"m\",\"m\",\"m\",\"m\",\"m\",\"deg\",\"deg\",\"m\",\"\",\"\",\"\",\"\",\"\",\"\",\"m\",\"m\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"";
    table_types =    "\"TYPE\",\"ID\",\"X\",\"X\",\"2DP\",\"2DP\",\"X\",\"2DP\",\"X\",\"2DP\",\"DT\",\"X\",\"X\",\"DT\",\"X\",\"2DP\",\"2DP\",\"2DP\",\"X\",\"X\",\"2DP\",\"2DP\",\"2DP\",\"2DP\",\"2DP\",\"2DP\",\"X\",\"X\",\"2DP\",\"X\",\"X\",\"X\",\"X\",\"X\",\"X\",\"2DP\",\"2DP\",\"X\",\"X\"";
    }
    
    if (version==AGS403 && min==false) {
    table_headings = "\"HEADING\",\"LOCA_ID\",\"LOCA_TYPE\",\"LOCA_STAT\",\"LOCA_NATE\",\"LOCA_NATN\",\"LOCA_GREF\",\"LOCA_GL\",\"LOCA_REM\",\"LOCA_FDEP\",\"LOCA_STAR\",\"LOCA_PURP\",\"LOCA_TERM\",\"LOCA_ENDD\",\"LOCA_LETT\",\"LOCA_LOCX\",\"LOCA_LOCY\",\"LOCA_LOCZ\",\"LOCA_LREF\",\"LOCA_DATM\",\"LOCA_ETRV\",\"LOCA_NTRV\",\"LOCA_LTRV\",\"LOCA_XTRL\",\"LOCA_YTRL\",\"LOCA_ZTRL\",\"LOCA_LAT\",\"LOCA_LON\",\"LOCA_ELAT\",\"LOCA_ELON\",\"LOCA_LLZ\",\"LOCA_LOCM\",\"LOCA_LOCA\",\"LOCA_CLST\",\"LOCA_ALID\",\"LOCA_OFFS\",\"LOCA_CNGE\",\"LOCA_TRAN\",\"FILE_FSET\"";
    table_units =    "\"UNIT\",\"\",\"\",\"\",\"m\",\"m\",\"m\",\"m\",\"\",\"m\",\"yyyy-mm-dd\",\"\",\"\",\"yyyy-mm-dd\",\"\",\"m\",\"m\",\"m\",\"\",\"\",\"m\",\"m\",\"m\",\"m\",\"m\",\"m\",\"deg\",\"deg\",\"m\",\"\",\"\",\"\",\"\",\"\",\"\",\"m\",\"m\",\"\",\"\"";
    table_types =    "\"TYPE\",\"ID\",\"X\",\"X\",\"2DP\",\"2DP\",\"X\",\"2DP\",\"X\",\"2DP\",\"DT\",\"X\",\"X\",\"DT\",\"X\",\"2DP\",\"2DP\",\"2DP\",\"X\",\"X\",\"2DP\",\"2DP\",\"2DP\",\"2DP\",\"2DP\",\"2DP\",\"X\",\"X\",\"2DP\",\"X\",\"X\",\"X\",\"X\",\"X\",\"X\",\"2DP\",\"2DP\",\"X\",\"X\"";
    }

    if (min==true && (version== AGS404 || version==AGS403)) {
    table_headings = "\"HEADING\",\"LOCA_ID\"";
    table_units =    "\"UNIT\",\"\"";
    table_types =  "\"TYPE\",\"ID\"";
    }

    StringBuilder sb = new StringBuilder();
    sb.Append (table_name);
    sb.AppendLine();
    sb.Append (table_headings);
    sb.AppendLine();
    sb.Append (table_units);
    sb.AppendLine();
    sb.Append (table_types);
    sb.AppendLine();
    
    if (version==AGS403 && min==false) {
        foreach (POINT row in rows) {
            string line = $"\"DATA\",\"{row.PointID}\",\"{row.LOCA_TYPE}\",\"{row.LOCA_STAT}\",\"{String.Format(DP2_FORMAT_AGS,row.LOCA_NATE)}\",\"{String.Format(DP2_FORMAT_AGS,row.LOCA_NATN)}\",\"{row.LOCA_GREF}\",\"{String.Format(DP2_FORMAT_AGS,row.LOCA_GL)}\",\"{row.LOCA_REM}\",\"{String.Format(DP2_FORMAT_AGS,row.HoleDepth)}\",\"{String.Format(DATE_FORMAT_AGS,row.LOCA_STAR)}\",\"{row.LOCA_PURP}\",\"{row.LOCA_TERM}\",\"{String.Format(DATE_FORMAT_AGS,row.LOCA_ENDD)}\",\"{row.LOCA_LETT}\",\"{String.Format(DP2_FORMAT_AGS,row.East)}\",\"{String.Format(DP2_FORMAT_AGS,row.North)}\",\"{String.Format(DP2_FORMAT_AGS,row.Elevation)}\",\"{row.LOCA_LREF}\",\"{row.LOCA_DATM}\",\"{row.LOCA_ETRV}\",\"{row.LOCA_NTRV}\",\"{row.LOCA_LTRV}\",\"{row.LOCA_XTRL}\",\"{row.LOCA_YTRL}\",\"{row.LOCA_ZTRL}\",\"{row.LOCA_LAT}\",\"{row.LOCA_LON}\",\"{row.LOCA_ELAT}\",\"{row.LOCA_ELON}\",\"{row.LOCA_LLZ}\",\"{row.LOCA_LOCM}\",\"{row.LOCA_LOCA}\",\"{row.LOCA_CLST}\",\"{row.LOCA_ALID}\",\"{row.LOCA_OFFS}\",\"{row.LOCA_CNGE}\",\"{row.LOCA_TRAN}\",\"{row.FILE_FSET}\"";
            sb.Append(line);
            sb.AppendLine();
        }
    }
  if (version==AGS404 && min==false) {
        foreach (POINT row in rows) {
            string line = $"\"DATA\",\"{row.PointID}\",\"{row.LOCA_TYPE}\",\"{row.LOCA_STAT}\",\"{String.Format(DP2_FORMAT_AGS,row.LOCA_NATE)}\",\"{String.Format(DP2_FORMAT_AGS,row.LOCA_NATN)}\",\"{row.LOCA_GREF}\",\"{String.Format(DP2_FORMAT_AGS,row.LOCA_GL)}\",\"{row.LOCA_REM}\",\"{row.HoleDepth}\",\"{String.Format(DATE_FORMAT_AGS,row.LOCA_STAR)}\",\"{row.LOCA_PURP}\",\"{row.LOCA_TERM}\",\"{String.Format(DATE_FORMAT_AGS,row.LOCA_ENDD)}\",\"{row.LOCA_LETT}\",\"{String.Format(DP2_FORMAT_AGS,row.East)}\",\"{String.Format(DP2_FORMAT_AGS,row.North)}\",\"{String.Format(DP2_FORMAT_AGS,row.Elevation)}\",\"{row.LOCA_DATM}\",\"{row.LOCA_LREF}\",\"{row.LOCA_ETRV}\",\"{row.LOCA_NTRV}\",\"{row.LOCA_LTRV}\",\"{row.LOCA_XTRL}\",\"{row.LOCA_YTRL}\",\"{row.LOCA_ZTRL}\",\"{row.LOCA_LAT}\",\"{row.LOCA_LON}\",\"{row.LOCA_ELAT}\",\"{row.LOCA_ELON}\",\"{row.LOCA_LLZ}\",\"{row.LOCA_LOCM}\",\"{row.LOCA_LOCA}\",\"{row.LOCA_CLST}\",\"{row.LOCA_ALID}\",\"{row.LOCA_OFFS}\",\"{row.LOCA_CNGE}\",\"{row.LOCA_TRAN}\",\"{row.FILE_FSET}\"";
            sb.Append(line);
            sb.AppendLine();
        }
    }

    if (min==true && (version== AGS404 || version==AGS403)) {
        foreach (POINT row in rows) {
            string line = $"\"DATA\",\"{row.PointID}\"";
            sb.Append(line);
            sb.AppendLine();
        }
    }


    return sb.ToString();
}

private string getAGS404Table(dsTable ds) {

    string table_name = "\"GROUP\",\"MOND\"";
    string table_headings = "\"HEADING\",\"LOCA_ID\",\"MONG_ID\",\"MONG_DIS\",\"MOND_DTIM\",\"MOND_TYPE\",\"MOND_REF\",\"MOND_INST\",\"MOND_RDNG\",\"MOND_UNIT\",\"MOND_METH\",\"MOND_LIM\",\"MOND_ULIM\",\"MOND_NAME\",\"MOND_CRED\",\"MOND_CONT\",\"MOND_REM\",\"FILE_FSET\"";
    string table_units = "\"UNIT\",\"\",\"\",\"m\",\"yyyy-mm-ddThh:mm:ss\",\"m\",\"\",\"\",\"m\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"";
    string table_types = "\"TYPE\",\"ID\",\"X\",\"2DP\",\"DT\",\"PA\",\"X\",\"X\",\"XN\",\"PU\",\"X\",\"U\",\"U\",\"X\",\"X\",\"X\",\"X\",\"X\"";
    
    StringBuilder sb = new StringBuilder();
    sb.Append (table_name);
    sb.AppendLine();
    sb.Append (table_headings);
    sb.AppendLine();
    sb.Append (table_units);
    sb.AppendLine();
    sb.Append (table_types);
    sb.AppendLine();

    if (ds.tableName=="MOND") { 
    foreach (DataRow data in ds.dataTable.Rows) {
        
        string line = "\"DATA\",\"" + data["PointId"] + "\",\"" + data["ItemKey"] + "\",\"" + data["MONG_DIS"] + "\",\"" + Convert.ToDateTime(data["DateTime"]).ToString(DATE_FORMAT_AGS) + "\",\"" + data["MOND_TYPE"] + "\",\"" + data["MOND_REF"] + "\",\"" + data["MOND_INST"] + "\",\"" + data["MOND_RDNG"] + "\",\"" + data["MOND_UNIT"] +  "\",\"" + data["MOND_METH"] + "\",\"" + data["MOND_LIM"] + "\",\"" + data["MOND_ULIM"] + "\",\"" + data["MOND_NAME"] + "\",\"" + data["MOND_CRED"] + "\",\"" + data["MOND_CONT"]+ "\",\"" +data["MOND_REM"]+ "\",\"" +data["FILE_FSET"] + "\"";
        sb.Append(line);
        sb.AppendLine();
    }
    }

    return sb.ToString();
}
public async Task<int> Upload(Guid projectId, 
                                 List<ERES> save_items, 
                                 string where = null
                                )
                                 {   


   // if (where == null) {
   // return await uploadSingle(projectId, save_items);
   // }

    return await uploadBulk(projectId,save_items, where);
    
    }
public async Task<int> Upload(Guid projectId, 
                                 List<SPEC> save_items, 
                                 string where = null
                                )
                                 {   


   // if (where == null) {
   // return await uploadSingle(projectId, save_items);
   // }

    return await uploadBulk(projectId,save_items, where);
    
    }
public async Task<int> Upload(Guid projectId, 
                                 List<SAMP> save_items, 
                                 string where = null
                                )
                                 {   


   // if (where == null) {
   // return await uploadSingle(projectId, save_items);
   // }

    return await uploadBulk(projectId,save_items, where);
    
    }

public async Task<int> Upload(Guid projectId, 
                                 List<MOND> save_items, 
                                 string where = null
                                )
                                 {   


    if (where == null) {
    return await uploadSingle(projectId, save_items);
    }

    return await uploadBulk(projectId,save_items, where);
    
    }
    public async Task<int> Upload(Guid projectId, 
                                 List<MONV> save_items, 
                                 string where = null
                                )
                                 {   


 //   if (where == null) {
    return await uploadSingle(projectId, save_items);
 //   }

  //  return await uploadBulk(projectId, save_items, where);
    
    }
private async Task<int> uploadSingle(Guid projectId, 
                                 List<MOND> save_items) 
                                 {   

    int NOT_OK = -1;
    int ret = 0;
    
    dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

    if (cd==null) {
        return NOT_OK;
    }

    string dbConnectStr = cd.AsConnectionString();
    int gINTProjectID = cd.ProjectId;

        return await Task.Run(() =>
        
        {
            using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
            {
                cnn.Open();
                dsTable dsMOND = new gINTTables().MOND;
                dsMOND.setConnection (cnn);        
                DataTable dtMOND = null;
                string sqlWhere = "";

                foreach (MOND item in save_items) {

                        DataRow row = null;
                        
                        if (item.gINTProjectID==0) item.gINTProjectID=gINTProjectID;

                        //check for unique records
                        if (item.MOND_REF !=null) {
                         sqlWhere = $"gINTProjectID={item.gINTProjectID} and PointId='{item.PointID}' and ItemKey='{item.ItemKey}' and MONG_DIS={item.MONG_DIS} and MOND_TYPE='{item.MOND_TYPE}' and DateTime='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",item.DateTime)}' and MOND_REF='{item.MOND_REF}'";
                        } 
                        
                        if (item.MOND_REF == null) {
                        sqlWhere = $"gINTProjectID={item.gINTProjectID} and PointId='{item.PointID}' and ItemKey='{item.ItemKey}' and MONG_DIS={item.MONG_DIS} and MOND_TYPE='{item.MOND_TYPE}' and DateTime='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",item.DateTime)}' and MOND_REF is null";
                        }
                        dsMOND.Reset();
                        dsMOND.sqlWhere(sqlWhere);
                        dsMOND.getDataSet();
                        dtMOND = dsMOND.getDataTable(); 

                        if (dtMOND.Rows.Count==0) {
                            row = dsMOND.NewRow();
                            dsMOND.addRow (row);
                        } else {
                            row = dtMOND.Rows[0];
                        } 
                        
                        setValues(item,row);
    
                       ret =+ dsMOND.Update();                           
                } 
            } 
           return ret;
        });
 
}
private int getBITValue(Boolean value) {
    if (value==true) { 
    return 1;
    }
    return 0;
}
private void setValues(ERES item, DataRow row) {
                        row["gINTProjectID"] = item.gINTProjectID;
                        row["PointID"] = item.PointID;
                        row["SAMP_Depth"] = item.SAMP_Depth;
                        row["SAMP_REF"] = item.SAMP_REF;
                        row["SAMP_TYPE"] = item.SAMP_TYPE;    
                        row["SAMP_ID"] = item.SAMP_ID;
                        row["Depth"] = item.Depth;
                        row["SPEC_REF"] = item.SPEC_REF;
                        row["ItemKey"] = item.ItemKey;
                        row["ERES_METH"] = item.ERES_METH; 
                        row["Matrix-Run Type"] = item.Matrix_Run_Type;
                        row["ERES_MATX"] = item.ERES_MATX;
                        row["ERES_RTYP"] = item.ERES_RTYP;
                        row["ERES_TESN"] = item.ERES_TESN;
                        row["ERES_NAME"] = item.ERES_NAME;
                        row["ERES_TNAM"] = item.ERES_TNAM;
                        if (item.ERES_RVAL == null) {row["ERES_RVAL"] = DBNull.Value;} else {row["ERES_RVAL"] = item.ERES_RVAL;}     
                        row["ERES_RUNI"] = item.ERES_RUNI;
                        row["ERES_RTXT"] = item.ERES_RTXT;
                        row["ERES_RTCD"] = item.ERES_RTCD; 
                        if (item.ERES_RRES == null) {row["ERES_RRES"] = DBNull.Value;} else {row["ERES_RRES"] = getBITValue (item.ERES_RRES.Value);}    
                        if (item.ERES_DETF == null) {row["ERES_DETF"] = DBNull.Value;} else {row["ERES_DETF"] = getBITValue (item.ERES_DETF.Value);}    
                        if (item.ERES_ORG == null) {row["ERES_ORG"] = DBNull.Value;} else {row["ERES_ORG"] = getBITValue (item.ERES_ORG.Value);}    
                        row["ERES_IQLF"] = item.ERES_IQLF;     
                        row["ERES_LQLF"] = item.ERES_LQLF;  
                        if (item.ERES_RDLM == null) {row["ERES_RDLM"] = DBNull.Value;} else {row["ERES_RDLM"] = item.ERES_RDLM;}      
                        if (item.ERES_MDLM == null) {row["ERES_MDLM"] = DBNull.Value;} else {row["ERES_MDLM"] = item.ERES_MDLM;}   
                        if (item.ERES_QLM == null) {row["ERES_QLM"] = DBNull.Value;} else {row["ERES_QLM"] = item.ERES_QLM;}   
                        row["ERES_DUNI"] = item.ERES_DUNI;
                        if (item.ERES_TPICP == null) {row["ERES_TPICP"] = DBNull.Value;} else {row["ERES_TPICP"] = item.ERES_TPICP;} 
                        if (item.ERES_TICT == null) {row["ERES_TICT"] = DBNull.Value;} else {row["ERES_TICT"] = item.ERES_TICT;} 
                        if (item.ERES_RDAT == null) {row["ERES_RDAT"] = DBNull.Value;} else {row["ERES_RDAT"] = item.ERES_RDAT;} 
                        row["ERES_SGRP"] = item.ERES_SGRP; 
                        row["SPEC_DESC"] = item.SPEC_DESC;
                        row["SPEC_PREP"] = item.SPEC_PREP;       
                        if (item.ERES_DTIM == null) {row["ERES_DTIM"] = DBNull.Value;} else {row["ERES_DTIM"] = item.ERES_DTIM;} 
                        row["ERES_TEST"] = item.ERES_TEST;
                        row["ERES_TORD"] = item.ERES_TORD;
                        row["ERES_LOCN"] = item.ERES_LOCN; 
                        row["ERES_BAS"] = item.ERES_BAS;
                        if (item.ERES_DIL == null) {row["ERES_DIL"] = DBNull.Value;} else {row["ERES_DIL"] = item.ERES_DIL;} 
                        row["ERES_LMTH"] = item.ERES_LMTH;
                        if (item.ERES_LDTM == null) {row["ERES_LDTM"] = DBNull.Value;} else {row["ERES_LDTM"] = item.ERES_LDTM;} 
                        row["ERES_IREF"] = item.ERES_IREF;
                        if (item.ERES_SIZE == null) {row["ERES_SIZE"] = DBNull.Value;} else {row["ERES_SIZE"] = item.ERES_SIZE;} 
                        if (item.ERES_PERP == null) {row["ERES_PERP"] = DBNull.Value;} else {row["ERES_PERP"] = item.ERES_PERP;} 
                        row["ERES_REM"] = item.ERES_REM;
                        row["ERES_LAB"] = item.ERES_LAB; 
                        row["ERES_CRED"] = item.ERES_CRED;
                        row["TEST_STAT"] = item.TEST_STAT;                        
                        row["FILE_FSET"] = item.FILE_FSET;

                        //Non standard LTC  fields
                        // row["ge_source"] = item.ge_source;
                        // row["ge_otherId"] = item.ge_otherId;
                        // row["RND_REF"] = item.RND_REF;
}
private void setValues(SAMP item, DataRow row) {
                        row["gINTProjectID"] = item.gINTProjectID;
                        row["PointID"] = item.PointID;
                        row["SAMP_TOP"] = item.Depth;
                        row["SAMP_REF"] = item.SAMP_REF;
                        row["SAMP_TYPE"] = item.SAMP_TYPE;    
                        row["SAMP_ID"] = item.SAMP_ID;
                        if (item.SAMP_BASE == null) {row["SAMP_BASE"] = DBNull.Value;} else {row["SAMP_BASE"] = item.SAMP_BASE;}
                        row["SAMP_LINK"] = item.SAMP_LINK;
                        if (item.SAMP_DTIM == null) {row["SAMP_DTIM"] = DBNull.Value;} else {row["SAMP_DTIM"] = item.SAMP_DTIM;}
                        if (item.SAMP_UBLO == null) {row["SAMP_UBLO"] = DBNull.Value;} else {row["SAMP_UBLO"] = item.SAMP_UBLO;}
                        row["SAMP_CONT"] = item.SAMP_CONT;
                        row["SAMP_PREP"] = item.SAMP_PREP;
                        if (item.SAMP_DIA == null) {row["SAMP_DIA"] = DBNull.Value;} else {row["SAMP_DIA"] = item.SAMP_DIA;}
                        if (item.SAMP_WDEP == null) {row["SAMP_WDEP"] = DBNull.Value;} else {row["SAMP_WDEP"] = item.SAMP_WDEP;}
                        if (item.SAMP_RECV == null) {row["SAMP_RECV"] = DBNull.Value;} else {row["SAMP_RECV"] = item.SAMP_RECV;}
                        row["SAMP_TECH"] = item.SAMP_TECH; 
                        row["SAMP_MATX"] = item.SAMP_MATX;
                        row["SAMP_TYPC"] = item.SAMP_TYPC;
                        row["SAMP_WHO"] = item.SAMP_WHO;
                        row["SAMP_WHY"] = item.SAMP_WHY;
                        row["SAMP_REM"] = item.SAMP_REM;
                        row["SAMP_DESC"] = item.SAMP_DESC;
                        if (item.SAMP_DESD == null) {row["SAMP_DESD"] = DBNull.Value;} else {row["SAMP_DESD"] = item.SAMP_DESD;}
                        row["SAMP_LOG"] = item.SAMP_LOG;
                        row["SAMP_COND"] = item.SAMP_COND;
                        row["SAMP_CLSS"] = item.SAMP_CLSS;
                        if (item.SAMP_BAR == null) {row["SAMP_BAR"] = DBNull.Value;} else {row["SAMP_BAR"] = item.SAMP_BAR;}
                        if (item.SAMP_TEMP == null) {row["SAMP_TEMP"] = DBNull.Value;} else {row["SAMP_TEMP"] = item.SAMP_TEMP;}
                        if (item.SAMP_PRES == null) {row["SAMP_PRES"] = DBNull.Value;} else {row["SAMP_PRES"] = item.SAMP_PRES;}
                        if (item.SAMP_FLOW == null) {row["SAMP_FLOW"] = DBNull.Value;} else {row["SAMP_FLOW"] = item.SAMP_FLOW;}
                        if (item.SAMP_ETIM == null) {row["SAMP_ETIM"] = DBNull.Value;} else {row["SAMP_ETIM"] = item.SAMP_ETIM;}
                        if (item.SAMP_DURN == null) {row["SAMP_DURN"] = DBNull.Value;} else {row["SAMP_DURN"] = item.SAMP_DURN;}
                        row["SAMP_CAPT"] = item.SAMP_CAPT;
                        row["GEOL_STAT"] = item.GEOL_STAT;
                        if (item.SAMP_RECL == null) {row["SAMP_RECL"] = DBNull.Value;} else {row["SAMP_RECL"] = item.SAMP_RECL;}
                        row["FILE_FSET"] = item.FILE_FSET;

                       // Non standard LTC  fields
                       // row["ge_source"] = item.ge_source;
                       // row["ge_otherId"] = item.ge_otherId;
                       // row["RND_REF"] = item.RND_REF;
}
private void setValues(SPEC item, DataRow row) {
                        row["gINTProjectID"] = item.gINTProjectID;
                        row["PointID"] = item.PointID;
                        row["SAMP_Depth"] = item.SAMP_Depth;
                        row["SAMP_REF"] = item.SAMP_REF;
                        row["SAMP_TYPE"] = item.SAMP_TYPE;    
                        row["SAMP_ID"] = item.SAMP_ID;
                        row["Depth"] = item.Depth;
                        row["SPEC_REF"] = item.SPEC_REF;
                        row["SPEC_DESC"] = item.SPEC_DESC;
                        row["SPEC_REM"] = item.SPEC_REM;
                       // row["SPEC_PREP"] = item.SPEC_PREP;        
                    
                    //Non standard LTC  fields
                    //    row["ge_source"] = item.ge_source;
                    //    row["ge_otherId"] = item.ge_otherId;
                    //    row["RND_REF"] = item.RND_REF;
}

private void setValues(MOND item, DataRow row) {
                        
                        row["gINTProjectID"] = item.gINTProjectID;
                        row["PointID"] = item.PointID;
                        row["MOND_REF"] = item.MOND_REF;
                        row["ItemKey"] = item.ItemKey;
                        if (item.MONG_DIS == null) {row["MONG_DIS"] = DBNull.Value;} else {row["MONG_DIS"] = item.MONG_DIS;}
                        row["DateTime"] = item.DateTime;    
                        row["MOND_TYPE"] = item.MOND_TYPE;
                        row["MOND_RDNG"] = item.MOND_RDNG;
                        row["MOND_NAME"] = item.MOND_NAME;
                        row["MOND_UNIT"] = item.MOND_UNIT;
                        row["MOND_REM"] = item.MOND_REM; 
                        row["MOND_CONT"] = item.MOND_CONT;
                        row["MOND_INST"] = item.MOND_INST;
                        row["MOND_CRED"] = item.MOND_CRED;
                        if (item.MOND_LIM == null) {row["MOND_LIM"] = DBNull.Value;} else {row["MOND_LIM"] = item.MOND_LIM;}
                        row["MOND_METH"] = item.MOND_METH;
                        if (item.MOND_ULIM == null) {row["MOND_ULIM"] = DBNull.Value;} else {row["MOND_ULIM"] = item.MOND_ULIM;}
                        row["FILE_FSET"] = item.FILE_FSET;

                        //Non standard LTC  fields
                        row["ge_source"] = item.ge_source;
                        row["ge_otherId"] = item.ge_otherId;
                        row["RND_REF"] = item.RND_REF;
}
private async Task<int> uploadBulk(Guid projectId, 
                                 List<SPEC> save_items, 
                                 string where = null )
                                 {   

    int NOT_OK = -1;
    int ret = 0;
    
    dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

    if (cd==null) {
        return NOT_OK;
    }

    var holes = save_items.Select(e => new {e.PointID})
                      .Distinct().ToList();
        
    string dbConnectStr = cd.AsConnectionString();
    int gINTProjectID = cd.ProjectId;

        
        return await Task.Run(() =>
        
        {
            using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
            {
                cnn.Open();
                dsTable dsMOND = new gINTTables().MOND;
                dsMOND.setConnection (cnn);


                DataTable dtMOND = null;

                // reduce the dataset, all logger records could be massive
                for (int i=0; i < holes.Count; i++) {
                
                    string wherePointID = holes[i].PointID;

                    if (where != null && wherePointID == "") {
                            dsMOND.sqlWhere($"gINTProjectID={gINTProjectID} and {where}'");
                    }
                    
                    if (where == null && wherePointID == "") {
                            dsMOND.sqlWhere($"gINTProjectID={gINTProjectID}");    
                    }

                    if (where != null && wherePointID !="") {
                            dsMOND.sqlWhere($"gINTProjectID={gINTProjectID} and {where} and PointID='{wherePointID}'");
                    }
                    
                    dsMOND.getDataSet();
                    dtMOND = dsMOND.getDataTable();
                    Boolean checkExisting = false;

                    if (dtMOND.Rows.Count>0) {
                        checkExisting=true;
                    }

                    foreach (SPEC item in save_items) {

                            DataRow row = null;
                            
                            if (item.gINTProjectID==0) item.gINTProjectID=gINTProjectID;
                            
                            if (checkExisting==true) {
                                //check for existing records
                                if (item.GintRecID>0) {
                                row = dtMOND.Select ($"GintRecID={item.GintRecID}").SingleOrDefault();
                                }

                                //check for unique records
                                // primary unique key gINTProjectID, PointID, SAMP_Depth, SAMP_REF, SAMP_TYPE, SAMP_ID, Depth, SPEC_REF
                                if (row == null) {
                                    if (item.SPEC_REF !=null) row = dtMOND.Select ($"gINTProjectID={item.gINTProjectID} and PointId='{item.PointID}' and SAMP_Depth={item.SAMP_Depth} and SAMP_REF={item.SAMP_REF} and SAMP_TYPE='{item.SAMP_TYPE}' and SAMP_ID='{item.SAMP_ID}' and Depth='{item.Depth}' and SPEC_REF='{item.SPEC_REF}'").SingleOrDefault();
                                    if (item.SPEC_REF == null) row = dtMOND.Select ($"gINTProjectID={item.gINTProjectID} and PointId='{item.PointID}' and SAMP_Depth={item.SAMP_Depth} and SAMP_REF={item.SAMP_REF} and SAMP_TYPE='{item.SAMP_TYPE}' and SAMP_ID='{item.SAMP_ID}' and Depth='{item.Depth}' and SPEC_REF is null").SingleOrDefault();
                                }
                            }

                            if (row == null) {
                                row = dsMOND.NewRow();
                                dsMOND.addRow (row);
                            }

                            setValues(item, row);                        
                    } 
                
                    ret = dsMOND.BulkUpdate();

                }
            } 
           return ret;
        });
 }
private async Task<int> uploadBulk(Guid projectId, 
                                 List<SAMP> save_items, 
                                 string where = null )
                                 {   

    int NOT_OK = -1;
    int ret = 0;
    
    dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

    if (cd==null) {
        return NOT_OK;
    }

    var holes = save_items.Select(e => new {e.PointID})
                      .Distinct().ToList();
        
    string dbConnectStr = cd.AsConnectionString();
    int gINTProjectID = cd.ProjectId;

        
        return await Task.Run(() =>
        
        {
            using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
            {
                cnn.Open();
                dsTable dsMOND = new gINTTables().MOND;
                dsMOND.setConnection (cnn);


                DataTable dtMOND = null;

                // reduce the dataset, all logger records could be massive
                for (int i=0; i < holes.Count; i++) {
                
                    string wherePointID = holes[i].PointID;

                    if (where != null && wherePointID == "") {
                            dsMOND.sqlWhere($"gINTProjectID={gINTProjectID} and {where}'");
                    }
                    
                    if (where == null && wherePointID == "") {
                            dsMOND.sqlWhere($"gINTProjectID={gINTProjectID}");    
                    }

                    if (where != null && wherePointID !="") {
                            dsMOND.sqlWhere($"gINTProjectID={gINTProjectID} and {where} and PointID='{wherePointID}'");
                    }
                    
                    dsMOND.getDataSet();
                    dtMOND = dsMOND.getDataTable();
                    Boolean checkExisting = false;

                    if (dtMOND.Rows.Count>0) {
                        checkExisting=true;
                    }

                    foreach (SAMP item in save_items) {

                            DataRow row = null;
                            
                            if (item.gINTProjectID==0) item.gINTProjectID=gINTProjectID;
                            
                            if (checkExisting==true) {
                                //check for existing records
                                if (item.GintRecID>0) {
                                row = dtMOND.Select ($"GintRecID={item.GintRecID}").SingleOrDefault();
                                }

                                //check for unique records
                                // primary unique key gINTProjectID, PointID, SAMP_Depth, SAMP_REF, SAMP_TYPE, SAMP_ID, Depth, SPEC_REF
                                if (row == null) {
                                //    if (item.SPEC_REF !=null) row = dtMOND.Select ($"gINTProjectID={item.gINTProjectID} and PointId='{item.PointID}' and SAMP_Depth={item.SAMP_Depth} and SAMP_REF={item.SAMP_REF} and SAMP_TYPE='{item.SAMP_TYPE}' and SAMP_ID='{item.SAMP_ID}' and Depth='{item.Depth}' and SPEC_REF='{item.SPEC_REF}'").SingleOrDefault();
                                //    if (item.SPEC_REF == null) row = dtMOND.Select ($"gINTProjectID={item.gINTProjectID} and PointId='{item.PointID}' and SAMP_Depth={item.SAMP_Depth} and SAMP_REF={item.SAMP_REF} and SAMP_TYPE='{item.SAMP_TYPE}' and SAMP_ID='{item.SAMP_ID}' and Depth='{item.Depth}' and SPEC_REF is null").SingleOrDefault();
                                }
                            }

                            if (row == null) {
                                row = dsMOND.NewRow();
                                dsMOND.addRow (row);
                            }

                            setValues(item, row);                        
                    } 
                
                    ret = dsMOND.BulkUpdate();

                }
            } 
           return ret;
        });
 }

private async Task<int> uploadBulk(Guid projectId, 
                                 List<ERES> save_items, 
                                 string where = null )
                                 {   

    int NOT_OK = -1;
    int ret = 0;
    
    dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

    if (cd==null) {
        return NOT_OK;
    }

    var holes = save_items.Select(e => new {e.PointID})
                      .Distinct().ToList();
        
    string dbConnectStr = cd.AsConnectionString();
    int gINTProjectID = cd.ProjectId;

        
        return await Task.Run(() =>
        
        {
            using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
            {
                cnn.Open();
                dsTable dsMOND = new gINTTables().MOND;
                dsMOND.setConnection (cnn);


                DataTable dtMOND = null;

                // reduce the dataset, all logger records could be massive
                for (int i=0; i < holes.Count; i++) {
                
                    string wherePointID = holes[i].PointID;

                    if (where != null && wherePointID == "") {
                            dsMOND.sqlWhere($"gINTProjectID={gINTProjectID} and {where}'");
                    }
                    
                    if (where == null && wherePointID == "") {
                            dsMOND.sqlWhere($"gINTProjectID={gINTProjectID}");    
                    }

                    if (where != null && wherePointID !="") {
                            dsMOND.sqlWhere($"gINTProjectID={gINTProjectID} and {where} and PointID='{wherePointID}'");
                    }
                    
                    dsMOND.getDataSet();
                    dtMOND = dsMOND.getDataTable();
                    Boolean checkExisting = false;

                    if (dtMOND.Rows.Count>0) {
                        checkExisting=true;
                    }

                    foreach (ERES item in save_items) {

                            DataRow row = null;
                            
                            if (item.gINTProjectID==0) item.gINTProjectID=gINTProjectID;
                            
                            if (checkExisting==true) {
                                //check for existing records
                                if (item.GintRecID>0) {
                                row = dtMOND.Select ($"GintRecID={item.GintRecID}").SingleOrDefault();
                                }

                                //check for unique records
                                // primary unique key gINTProjectID, PointID, SAMP_Depth, SAMP_REF, SAMP_TYPE, SAMP_ID, Depth, SPEC_REF
                                if (row == null) {
                                    if (item.SPEC_REF !=null) row = dtMOND.Select ($"gINTProjectID={item.gINTProjectID} and PointId='{item.PointID}' and SAMP_Depth={item.SAMP_Depth} and SAMP_REF={item.SAMP_REF} and SAMP_TYPE='{item.SAMP_TYPE}' and SAMP_ID='{item.SAMP_ID}' and Depth='{item.Depth}' and SPEC_REF='{item.SPEC_REF}'").SingleOrDefault();
                                    if (item.SPEC_REF == null) row = dtMOND.Select ($"gINTProjectID={item.gINTProjectID} and PointId='{item.PointID}' and SAMP_Depth={item.SAMP_Depth} and SAMP_REF={item.SAMP_REF} and SAMP_TYPE='{item.SAMP_TYPE}' and SAMP_ID='{item.SAMP_ID}' and Depth='{item.Depth}' and SPEC_REF is null").SingleOrDefault();
                                }
                            }

                            if (row == null) {
                                row = dsMOND.NewRow();
                                dsMOND.addRow (row);
                            }

                            setValues(item, row);                        
                    } 
                
                    ret = dsMOND.BulkUpdate();

                }
            } 
           return ret;
        });
 }

private async Task<int> uploadBulk(Guid projectId, 
                                 List<MOND> save_items, 
                                 string where = null )
                                 {   

    int NOT_OK = -1;
    int ret = 0;
    
    string wherePointID = "";
    double? whereMONG_DIS = null;

    dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

    if (cd==null) {
        return NOT_OK;
    }

    var holes = save_items.Select(mond => new { mond.PointID})
                      .Distinct().ToList();
    if (holes.Count==1) {
        wherePointID= holes[0].PointID;
    }
    var mong_dis = save_items.Select(mond => new { mond.MONG_DIS})
                      .Distinct().ToList();

    if (mong_dis.Count==1) {
        whereMONG_DIS= mong_dis[0].MONG_DIS;
    }
    
    DateTime minDateTime =  save_items.Min(e=>e.DateTime).GetValueOrDefault();

    string dbConnectStr = cd.AsConnectionString();
    int gINTProjectID = cd.ProjectId;

        
        return await Task.Run(() =>
        
        {
            using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
            {
                cnn.Open();
                dsTable dsMOND = new gINTTables().MOND;
                dsMOND.setConnection (cnn);        
                DataTable dtMOND = null;

                // reduce the dataset, all logger records could be massive

                if (where != null && wherePointID == "") {
                        dsMOND.sqlWhere($"gINTProjectID={gINTProjectID} and {where} and DateTime>='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",minDateTime)}'");
                }
                
                if (where == null && wherePointID == "") {
                        dsMOND.sqlWhere($"gINTProjectID={gINTProjectID} and DateTime>='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",minDateTime)}'");    
                }

                if (where != null && wherePointID !="" && whereMONG_DIS ==null) {
                        dsMOND.sqlWhere($"gINTProjectID={gINTProjectID} and {where} and PointID='{wherePointID}' and DateTime>='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",minDateTime)}'");
                }
                
                if (where != null && wherePointID !="" && whereMONG_DIS !=null) {
                        dsMOND.sqlWhere($"gINTProjectID={gINTProjectID} and {where} and PointID='{wherePointID}' and DateTime>='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",minDateTime)}' and MONG_DIS={whereMONG_DIS.Value}");
                }
                dsMOND.getDataSet();
                dtMOND = dsMOND.getDataTable();
                Boolean checkExisting = false;

                if (dtMOND.Rows.Count>0) {
                    checkExisting=true;
                }

                foreach (MOND item in save_items) {

                        DataRow row = null;
                        
                        if (item.gINTProjectID==0) item.gINTProjectID=gINTProjectID;
                        
                        if (checkExisting==true) {
                            //check for existing records
                            if (item.GintRecID>0) {
                            row = dtMOND.Select ($"GintRecID={item.GintRecID}").SingleOrDefault();
                            }

                           // if (row == null && item.ge_otherid!=null) {
                                // try and check for existing ge_generated records, but some ge_source and ge_otherid 
                                // combination may results in mutiple matches in the MOND table 
                                // so this is not a reliable way of identifying singular records
                          //      try {
                           //         row = dtMOND.Select ($"ge_source='{item.ge_source}' and ge_otherId='{item.ge_otherid}'").SingleOrDefault();
                          //      } catch {
                          //      row = null;
                          //      }
                          //  }

                            //check for unique records
                            if (row == null) {
                                if (item.MOND_REF !=null) row = dtMOND.Select ($"gINTProjectID={item.gINTProjectID} and PointId='{item.PointID}' and ItemKey='{item.ItemKey}' and MONG_DIS={item.MONG_DIS} and MOND_TYPE='{item.MOND_TYPE}' and DateTime='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",item.DateTime)}' and MOND_REF='{item.MOND_REF}'").SingleOrDefault();
                                if (item.MOND_REF == null) row = dtMOND.Select ($"gINTProjectID={item.gINTProjectID} and PointId='{item.PointID}' and ItemKey='{item.ItemKey}' and MONG_DIS={item.MONG_DIS} and MOND_TYPE='{item.MOND_TYPE}' and DateTime='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",item.DateTime)}' and MOND_REF is null").SingleOrDefault();
                            }
                        }

                        if (row == null) {
                            row = dsMOND.NewRow();
                            dsMOND.addRow (row);
                        }

                        setValues(item, row);                        
                } 
            
                ret = dsMOND.BulkUpdate();

               

            } 
           return ret;
        });
 }


private MOND getUnique(MOND newM) {

foreach(MOND existM in MOND) {
        if  ((newM.gINTProjectID==existM.gINTProjectID) &&
                (newM.PointID==existM.PointID) &&
                    (newM.MONG_DIS==newM.MONG_DIS) &&
                        (newM.MOND_TYPE == existM.MOND_TYPE) && 
                            (newM.DateTime==existM.DateTime)) {
                                newM.DateTime.Value.AddSeconds(1);
                                    return getUnique(newM);
        }
}

return newM;

}
[HttpPost]
public async Task<int> uploadSingle(Guid projectId, List<MONV> save_items){   

    int NOT_OK = -1;
    int ret = 0;
    
    dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

    if (cd==null) {
        return NOT_OK;
    }

    string dbConnectStr = cd.AsConnectionString();
    int gINTProjectID = cd.ProjectId;

        return await Task.Run(() =>
        
        {
            using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
            {
                cnn.Open();
                dsTable ds_MONV = new gINTTables().MONV;
                ds_MONV.setConnection (cnn);        
                //ds_MOND.getDataTable ();  
                // reduce the dataset only to the esri feature attribute records, all logger records could be massive
                ds_MONV.sqlWhere($"gINTProjectID={gINTProjectID} and ge_source like '%esri%'");
                ds_MONV.getDataSet();
                
                DataTable dtMONV = ds_MONV.getDataTable();
                DataRow row = null;
                
                foreach (MONV item in save_items) {

                        row = dtMONV.Select ($"ge_source='{item.ge_source}' and ge_otherId='{item.ge_otherId}'").SingleOrDefault();
                       
                        if (row == null) {
                            row = ds_MONV.NewRow();
                            row["ge_source"] = item.ge_source;
                            row["ge_otherId"] = item.ge_otherId;
                            ds_MONV.addRow (row);
                           // row.SetAdded();     
                        } else{
                           // row.SetModified();
                        }
                        
                        setValues(item, row);   
                }
                ret = ds_MONV.Update();
            } 
           return ret;
        });
 
}
// private async Task<int> Upload<T>(Guid projectId, List<T> save_items) {   

//     int NOT_OK = -1;
//     int ret = 0;
    
//     dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

//     if (cd==null) {
//         return NOT_OK;
//     }

//     string dbConnectStr = cd.AsConnectionString();
//     int gINTProjectID = cd.ProjectId;

//         return await Task.Run(() =>
        
//         {
//             using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
//             {
//                 cnn.Open();
//                 dsTable dst = new gINTTables().MONV;
//                 dst.setConnection (cnn);        
//                 //ds_MOND.getDataTable ();  
//                 // reduce the dataset only to the esri feature attribute records, all logger records could be massive
//                 dst.sqlWhere($"gINTProjectID={gINTProjectID} and ge_source like '%esri%'");
//                 dst.getDataSet();
                
//                 DataTable dt = dst.getDataTable();
//                 DataRow row = null;
                
//                 foreach (T item in save_items) {

//                         row = dt.Select ($"ge_source='{item.ge_source}' and ge_otherId='{item.ge_otherid}'").SingleOrDefault();
                       
//                         if (row == null) {
//                             row = ds_MONV.NewRow();
//                             row["ge_source"] = item.ge_source;
//                             row["ge_otherId"] = item.ge_otherid;
//                             ds_MONV.addRow (row);
//                            // row.SetAdded();     
//                         } else{
//                            // row.SetModified();
//                         }
                        
//                         setValues(item, row);   
//                 }
//                 ret = ds.Update();
//             } 
//            return ret;
//         });
 
// }


private void setValues( MONV item, DataRow row) {

                            row["gINTProjectID"] = item.gINTProjectID;
                            row["DateTime"] = item.DateTime;
                            row["PointID"] = item.PointID;
                            row["MONV_REF"] = item.MONV_REF; 
                            row["MONV_DIS"] = item.MONV_DIS;
                            if (item.MONV_STAR == null) {row["MONV_STAR"] = DBNull.Value;} else {row["MONV_STAR"] = item.MONV_STAR;}
                            if (item.MONV_ENDD == null) {row["MONV_ENDD"] = DBNull.Value;} else {row["MONV_ENDD"] = item.MONV_ENDD;}
                            if (item.RND_REF == null) {row["RND_REF"] = DBNull.Value;} else {row["RND_REF"] = item.RND_REF;}
                            if (item.MONV_WEAT == null) {row["MONV_WEAT"] = DBNull.Value;} else {row["MONV_WEAT"] = item.MONV_WEAT;}
                            if (item.MONV_TEMP == null) {row["MONV_TEMP"] = DBNull.Value;} else {row["MONV_TEMP"] = item.MONV_TEMP;}
                            if (item.MONV_WIND == null) {row["MONV_WIND"] = DBNull.Value;} else {row["MONV_WIND"] = item.MONV_WIND;}
                            if (item.MONV_DIPR == null) {row["MONV_DIPR"] = DBNull.Value;} else {row["MONV_DIPR"] = item.MONV_DIPR;}
                            if (item.MONV_LOGR == null) {row["MONV_LOGR"] = DBNull.Value;} else {row["MONV_LOGR"] = item.MONV_LOGR;}                           
                            if (item.MONV_GASR == null) {row["MONV_GASR"] = DBNull.Value;} else {row["MONV_GASR"] = item.MONV_GASR;}
                            if (item.DIP_SRLN == null) {row["DIP_SRLN"] = DBNull.Value;} else {row["DIP_SRLN"] = item.DIP_SRLN;}
                            if (item.DIP_CLBD == null) {row["DIP_CLBD"] = DBNull.Value;} else {row["DIP_CLBD"] = item.DIP_CLBD;}
                            if (item.GAS_SRLN == null) {row["GAS_SRLN"] = DBNull.Value;} else {row["GAS_SRLN"] = item.GAS_SRLN;}
                            if (item.GAS_CLBD == null) {row["GAS_CLBD"] = DBNull.Value;} else {row["GAS_CLBD"] = item.GAS_CLBD;}
                            if (item.FLO_SRLN == null) {row["FLO_SRLN"] = DBNull.Value;} else {row["FLO_SRLN"] = item.FLO_SRLN;}
                            if (item.FLO_CLBD == null) {row["FLO_CLBD"] = DBNull.Value;} else {row["FLO_CLBD"] = item.FLO_CLBD;}
                            if (item.PID_SRLN == null) {row["PID_SRLN"] = DBNull.Value;} else {row["PID_SRLN"] = item.PID_SRLN;}
                            if (item.PID_CLBD== null) {row["PID_CLBD"] = DBNull.Value;} else {row["PID_CLBD"] = item.PID_CLBD;}
                            if (item.MONV_DATM == null) {row["MONV_DATM"] = DBNull.Value;} else {row["MONV_DATM"] = item.MONV_DATM;}
                            if (item.MONV_REMD == null) {row["MONV_REMD"] = DBNull.Value;} else {row["MONV_REMD"] = item.MONV_REMD;}
                            if (item.MONV_REML == null) {row["MONV_REML"] = DBNull.Value;} else {row["MONV_REML"] = item.MONV_REML;} 
                            if (item.MONV_REMG == null) {row["MONV_REMG"] = DBNull.Value;} else {row["MONV_REMG"] = item.MONV_REMG;}
                            if (item.MONV_REMS == null) {row["MONV_REMS"] = DBNull.Value;} else {row["MONV_REMS"] = item.MONV_REMS;}
                            if (item.PUMP_TYPE == null) {row["PUMP_TYPE"] = DBNull.Value;} else {row["PUMP_TYPE"] = item.PUMP_TYPE;}
                            if (item.MONV_MENG == null) {row["MONV_MENG"] = DBNull.Value;} else {row["MONV_MENG"] = item.MONV_MENG;}
                            if (item.PIPE_DIA== null) {row["PIPE_DIA"] = DBNull.Value;} else {row["PIPE_DIA"] = item.PIPE_DIA;}
                            if (item.AIR_PRESS== null) {row["AIR_PRESS"] = DBNull.Value;} else {row["AIR_PRESS"] = item.AIR_PRESS;}
                            if (item.AIR_TEMP== null) {row["AIR_TEMP"] = DBNull.Value;} else {row["AIR_TEMP"] = item.AIR_TEMP;} 
 
 }

    private string getString (DataTable table) {
         
        StringBuilder sb = new StringBuilder();
       
        foreach (DataRow row in table.Rows) {
            sb.Append ("DATA");
            foreach (DataColumn column in table.Columns) {
             sb.Append( ",\"" + row[column].ToString() + "\"");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
  
    }

 }