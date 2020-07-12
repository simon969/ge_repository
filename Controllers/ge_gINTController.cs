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
      
         public ge_gINTController(

            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,  
            IHostingEnvironment env ,
            IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
           
        }
    

     public async Task<IActionResult> ReadCSVFile(Guid Id,
                                             Guid dicId, 
                                             string table,
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
            ge_data empty_data = new ge_data();
            
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
            
            var dic = await new ge_dataController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getDataAsClass<ge_search>(dicId);

                      
            var lines = await new ge_dataController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).getDataByLines(Id);

          
            dbConnectDetails cd = await GetDbConnectDetails(_data.projectId, gINTTables.DB_DATA_TYPE);

            if (cd==null) {
                return null;
            }

            string dbConnectStr = cd.AsConnectionString();
            int? gINTProjectId = cd.ProjectId;
            
            SqlConnection cnn = new SqlConnection(dbConnectStr);
            cnn.Open();

            switch (table) {
                case "MOND":
                    // ReadMOND (lines, dic, gINTProjectId);
                    break;




           }

            cnn.Close();

      return Ok();
 }


 
 public async Task<List<MONG>> getMONG(Guid projectId, string[] points) {
            
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
            
            // if (mongs.Count>0) {
            //     sql_where +=" and (";
            //         foreach (MONG m in mongs) {
            //             sql_where += $"(PointID='{m.PointID} and ItemKey='{m.ItemKey}' and MONG_DIS={m.MONG_DIS}) or "; 
            //         }
            //     //remove last ' or '
            //     sql_where = sql_where.Substring(0,sql_where.Length-4);
            //     sql_where += ")";
            // }

            return await Task.Run(() =>
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
                        return  dsMOND.Update();
                    }
            }
            );

 }



 public async Task<List<MOND>> getMOND (Guid projectId, 
                                        string where) {
            if (projectId == null) {
                return null;
            }
            
            var project = await _context.ge_project
                                        .Include(p=>p.group)
                                    .SingleOrDefaultAsync(m => m.Id == projectId);
            
            if (project == null) {
                return null;
            }
            

            dbConnectDetails cd = await GetDbConnectDetails(projectId, gINTTables.DB_DATA_TYPE);

            if (cd==null) {
                throw new Exception ($"There is a problem with {project.name} gINT connection file");
            }

            string dbConnectStr = cd.AsConnectionString();
            int? gINTProjectId = cd.ProjectId;
            
            string sql_where = "gINTProjectId=" + gINTProjectId.Value;
            
            if (!String.IsNullOrEmpty(where)) {
                sql_where += " and "  + where;
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
                            return null;
                        } 
                        
                        if (dt_MOND.Rows.Count==0) {
                            return null;
                        }
                        
                        return  ConvertDataTable<MOND>( dt_MOND);
                    }
            }
            );

 }

public async Task<List<MOND>> getMOND( Guid projectId, 
                                        DateTime? fromDT, 
                                        DateTime? toDT,
                                        string[] points) {
            
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
            
            if (fromDT!=null) {
                sql_where += String.Format(" and DateTime>='{0:yyyy-MM-dd HH:mm:ss}'",fromDT.Value);
            }
            
            if (toDT!=null) {
                sql_where += String.Format(" and DateTime<='{0:yyyy-MM-dd HH:mm:ss}'",toDT.Value);
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
                            return null;
                        } 
                        
                        if (dt_MOND.Rows.Count==0) {
                            return null;
                        }
                        
                        return  ConvertDataTable<MOND>( dt_MOND);
                    }
            }
            );

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
public async Task<List<TRAN>> getTRAN(Guid projectId) {
            
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


  public async Task<List<POINT>> getPOINT(Guid projectId, string[] points) {
            
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
           
            if (!String.IsNullOrEmpty(points [0])) {
                sql_where += " and PointID in (" + points.ToDelimString(",","'") + ")";
            }

            return await Task.Run(() =>
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

 }

[HttpPost]  
  
public async Task<IActionResult> createAGS(Guid Id, 
                                           string[] holes,
                                           string filename,
                                           string[] tables,     
                                           DateTime? fromDT, 
                                           DateTime? toDT,  
                                           Guid? appendId,
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
            MOND = await getMOND (Id, fromDT, toDT, holes);
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
            POINT = await getPOINT (Id, SelectPoints);  
                if (POINT != null) {
                    sb.Append (getAGSTable(POINT, version, true));
                    sb.AppendLine();
                }
            }
            
            if (tables.Contains("POINT")) {
            POINT = await getPOINT (Id, SelectPoints); 
                if (POINT!=null) { 
                    sb.Append (getAGSTable(POINT, version, false));
                    sb.AppendLine();
                }
            }
            
            if (tables.Contains("TRAN")) {
            TRAN = await getTRAN (Id);
                if (TRAN!=null) {
                    sb.Append (getAGSTable(TRAN, version, false));
                    sb.AppendLine();
                }
            }
            
            if (tables.Contains("MONG")) {
            MONG = await getMONG(Id, SelectPoints);
                if (MONG!=null) {
                    sb.Append (getAGSTable(MONG, version, true));
                    sb.AppendLine();
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
                            encoding ="ascii",
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
           
            byte[] ags = Encoding.ASCII.GetBytes(s1);

            if (format =="download") {
            return File ( ags, "text/plain", _data.filename );
            }

            if (format == null || format=="view") {
            return File ( ags, "text/plain");
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
public async Task<int> UploadMOND(Guid projectId, 
                                 List<MOND> save_items, 
                                 string where = null
                                )
                                 {   


    if (where == null) {
    return await uploadMOND_Single(projectId, save_items);
    }

    return await uploadMOND_Bulk(projectId,save_items, where);
    
    }
private async Task<int> uploadMOND_Single(Guid projectId, 
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
                        row["ge_otherId"] = item.ge_otherid;
                        row["RND_REF"] = item.RND_REF;
}


private async Task<int> uploadMOND_Bulk(Guid projectId, 
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
public async Task<int> UploadMONV(Guid projectId, List<MONV> save_items){   

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

                        row = dtMONV.Select ($"ge_source='{item.ge_source}' and ge_otherId='{item.ge_otherid}'").SingleOrDefault();
                       
                        if (row == null) {
                            row = ds_MONV.NewRow();
                            row["ge_source"] = item.ge_source;
                            row["ge_otherId"] = item.ge_otherid;
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