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

namespace ge_repository.Controllers
{
//    [Route("api/[controller]")]
//    [ApiController]
    public class MONDController: ge_Controller  {     
     private IDictionary<string, measurement> dict_measurement = new Dictionary<string, measurement>();
     private static string  DATA_LAYOUT_NAME = "DataLayoutName";
     private static string DATE_FORMAT = "yyyy-MM-dd hh:mm:ss";
     private static string FILE_NAME_DATE_FORMAT = "yyyy_MM_dd";
     private static string DATE_FORMAT_AGS = "yyyy-MM-ddThh:mm:ss";
     
     private SqlConnection cnn;
     private dsTable MOND = new gINTTables().MOND;
     private dsTable MONG = new gINTTables().MONG;
     
         public MONDController(

            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,  
            IHostingEnvironment env ,
            IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
           
        }
    

     private void addDipperKeys_v101(IDictionary<string, measurement> dic) {

      dic.Add(new KeyValuePair<string, measurement>("CO2_Peak", new measurement {Type="GCD", Units="%vol",Name="Carbon Dioxide"}));
      dic.Add(new KeyValuePair<string, measurement>("O2_Peak", new measurement {Type="GOX", Units="%vol",Name="Oxygen"}));	
      dic.Add(new KeyValuePair<string, measurement>("CO_Peak", new measurement {Type="GCM", Units="%vol",Name="Carbon Monoxide"}));
      dic.Add(new KeyValuePair<string, measurement>("CH4_Peak", new measurement {Type="TGM", Units="%vol",Name="Methane"}));
      dic.Add(new KeyValuePair<string, measurement>("H2S_Peak", new measurement {Type="HYS", Units="%vol",Name="Hydrogen Sulphide"}));
      dic.Add(new KeyValuePair<string, measurement>("LEL_Peak", new measurement {Type="LEL", Units="%vol",Name="LEL (Lower Explosive Limit)"}));	
      dic.Add(new KeyValuePair<string, measurement>("PID_Peak", new measurement {Type="VC", Units="ppm",Name="PID"}));
      dic.Add(new KeyValuePair<string, measurement>("Press_Atmos", new measurement {Type="BAR", Units="mb",Name="Atmospheric pressure"}));
      dic.Add(new KeyValuePair<string, measurement>("Water_Depth", new measurement {Type="WDEP", Units="m",Name="Depth to water from datum"}));

 }
private void addLoggerKeys_v101(IDictionary<string, measurement> dic) {
    dic.Add(new KeyValuePair<string, measurement>("Water Depth", new measurement {Type="WDEP", Units="m",Name="Depth below ground level"}));
 }

 public async Task<IActionResult> AddData(Guid id) {

            if (id == null)
            {
                return NotFound();
            }
            
            var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == id);
            if (_data == null)
            {
                return NotFound();
            }

            var user = GetUserAsync().Result;
            
            if (user!= null) {
                    int IsDownloadAllowed = _context.IsOperationAllowed(Constants.DownloadOperationName, _data.project, _data);
                    Boolean CanUserDownload = _context.DoesUserHaveOperation(Constants.DownloadOperationName,_data.project,user.Id);
                    
                    int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, _data.project, _data);
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
            
            var _data_big = await _context.ge_data_big.SingleOrDefaultAsync(m => m.Id == id);
            
            if (_data_big == null)
            {
                return NotFound();
            }
           

            var ContentType = _data.filetype;
            var filename = _data.filename;
            var encode = _data.GetEncoding();

            MemoryStream memory = _data_big.getMemoryStream(encode);
      
            string[] lines = Encoding.ASCII.GetString(memory.ToArray()).
                                Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            dataLayout dl = getDataLayoutName(lines[0]);

            if (dl == null ) {
                 return NotFound();
            }

            dbConnectDetails cd = await getConnectDetails(_data.projectId,gINTTables.DB_DATA_TYPE);
      
            if (cd==null) {
                return null;
            }

            string dbConnectStr = cd.AsConnectionString();
            
            cnn = new SqlConnection(dbConnectStr);
            cnn.Open();

            MOND.setConnection (cnn);
            MONG.setConnection (cnn);

            if (dl == dataLayout.DataDipper101) {
                  addDipperKeys_v101(dict_measurement);
                  addDipperLayout101(_data, lines);

            }
            
            if (dl == dataLayout.DataLogger101) {
                 addLoggerKeys_v101(dict_measurement);
                 addLoggerLayout101(_data, lines);
            }

            cnn.Close();

      return Ok();
 }

private dataLayout getDataLayoutName(string s1) {

    if (s1.Contains(DATA_LAYOUT_NAME) && s1.Contains(dataLayout.DataDipper101.Name)) {
     return dataLayout.DataDipper101;
    }
    
    if (s1.Contains(DATA_LAYOUT_NAME) && s1.Contains(dataLayout.DataLogger101.Name)) {
     return dataLayout.DataLogger101;
    }
      
    if (s1.Contains("C02_Peak")) {
    return dataLayout.DataDipper101;
    }
    
    if (s1.Contains("Water Depth")) {
    return dataLayout.DataLogger101;
    }
    
    return dataLayout.NotIdentified;
}

 
public async Task<IActionResult> CreateAGS(Guid id, DateTime fromDate, DateTime toDate, string version = "4.04", string format = "view", Boolean save = false) {
            
            if (id == null)
            {
                return NotFound();
            }
            
            var _project = await _context.ge_project
                                    .Include(p=>p.group)
                                    .SingleOrDefaultAsync(m => m.Id == id);
            if (_project == null)
            {
                return NotFound();
            }
            
            dbConnectDetails cd = await getConnectDetails(id,gINTTables.DB_DATA_TYPE);
      
            if (cd==null) {
                return null;
            }
            
            int? _gINTProjectID = cd.ProjectId;

            if (_gINTProjectID == null) {
                return NotFound();
            }

            var _data = new ge_data();
            _data.project = _project;
            
            var user = GetUserAsync();

            if (user == null) {
            return RedirectToPageMessage (msgCODE.USER_NOTFOUND);
            }
            
            String userId = user.Result.Id;
            
            int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, _project, _data);
            Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName, _project, userId);

            if (IsCreateAllowed!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
            }
            if (!CanUserCreate) {
            return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
            }

   
            string dbConnectStr = cd.AsConnectionString();

            cnn = new SqlConnection(dbConnectStr);
            cnn.Open();

            MOND.setConnection (cnn);


            MOND.sqlWhere("gINTProjectID=" + _gINTProjectID + " and DateTime>='" + fromDate.ToString(DATE_FORMAT) + "' and DateTime<='" + toDate.ToString(DATE_FORMAT) + "'");
            MOND.getDataTable();

            String s1 = getAGS404Table (MOND);

            _data =  new ge_data {
                            Id = Guid.NewGuid(),
                            createdId = userId,
                            createdDT = DateTime.Now,
                            editedDT = DateTime.Now,
                            editedId = userId,
                            filename = "AGS MOND Table " + fromDate.ToString(FILE_NAME_DATE_FORMAT) + " to " + toDate.ToString(FILE_NAME_DATE_FORMAT) + ".ags",
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
                            description= _project.name + " AGS MOND table data (" + fromDate.ToString(FILE_NAME_DATE_FORMAT) + " to " + toDate.ToString(FILE_NAME_DATE_FORMAT) + ")",
                            operations ="Read;Download;Update;Delete",
                            data = new ge_data_big {
                                 data_string = s1
                                }
                            };
            if (save) {
                 _project.data = new List<ge_data>();
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
    foreach (DataRow item in ds.dataTable.Rows) {
        
        string line = "\"DATA\",\"" + item["PointId"] + "\",\"" + item["ItemKey"] + "\",\"" + item["MONG_DIS"] + "\",\"" + Convert.ToDateTime(item["DateTime"]).ToString(DATE_FORMAT_AGS) + "\",\"" + item["MOND_TYPE"] + "\",\"" + item["MOND_REF"] + "\",\"" + item["MOND_INST"] + "\",\"" + item["MOND_RDNG"] + "\",\"" + item["MOND_UNIT"] +  "\",\"" + item["MOND_METH"] + "\",\"" + item["MOND_LIM"] + "\",\"" + item["MOND_ULIM"] + "\",\"" + item["MOND_NAME"] + "\",\"" + item["MOND_CRED"] + "\",\"" + item["MOND_CONT"]+ "\",\"" +item["MOND_REM"]+ "\",\"" +item["FILE_FSET"] + "\"";
        sb.Append(line);
        sb.AppendLine();
    }
    }

    return sb.ToString();
}


private Boolean AddMOND (MOND newMOND, Boolean ReplaceExisting) {
    
    DataRow row = null;
    
    if (ReplaceExisting == true) {
        MOND.sqlWhere("gINTProjectID=" + newMOND.gINTProjectID + " and PointID='"  + newMOND.PointID + "' and MONG_DIS=" + newMOND.MONG_DIS + " and ItemKey='" + newMOND.ItemKey + "' and DateTime=" + newMOND.DateTimeFormated(DATE_FORMAT,"'") + " and MOND_TYPE='" + newMOND.MOND_TYPE + "'");
        MOND.getDataTable();
        if (!MOND.EOF()) {
        row = MOND.FirstRow();
        }
    }

    if (row == null) {
        MOND.Reset();
        row = MOND.NewRow();
    }

    row["gINTProjectID"] = newMOND.gINTProjectID;
    row["PointID"] = newMOND.PointID;
    row["MOND_REF"] = newMOND.MOND_REF;
    row["ItemKey"] = newMOND.ItemKey;
    row["MONG_DIS"] = newMOND.MONG_DIS;
    row["DateTime"] = newMOND.DateTime;    
    row["MOND_TYPE"] = newMOND.MOND_TYPE;
    row["MOND_RDNG"] = newMOND.MOND_RDNG;
    row["MOND_NAME"] = newMOND.MOND_NAME;
    row["MOND_UNIT"] = newMOND.MOND_UNIT;
    row["MOND_REM"] = newMOND.MOND_REM;                                     
    row["MOND_CONT"] = newMOND.MOND_CONT;
    row["FILE_FSET"] = newMOND.FILE_FSET; 
          
    MOND.Update();

    return true;

}


 private  async Task<IActionResult> addDipperLayout101(ge_data data, string [] lines) {

        dbConnectDetails cd = await getConnectDetails(data.projectId,gINTTables.DB_DATA_TYPE);
      
        if (cd==null) {
            return NotFound();
        }
        
        int gINTProjectID = cd.ProjectId;
        
        string dbConnectStr = cd.AsConnectionString();
            string POINT_ID = "HoleId";
            string MOND_ID = "Monitoring Id";
            string MOND_REF = "Monitoring Ref";
            string DATE_TIME = "DateTime";

            string[] header = lines[0].Split(",");
            int colPOINT_ID = header.findColumn(POINT_ID);
            int colMOND_ID = header.findColumn(MOND_ID);
            int colMOND_REF= header.findColumn(MOND_REF);
            int colDATE_TIME= header.findColumn(DATE_TIME);
            
            int row_start = 1;
            int col_start = colDATE_TIME+1 ;
            
            for (int i = row_start; i < lines.Length; i++)
            {
                string[] line =  lines[i].Split(",");
                for (int j = col_start; j < line.Length; j++)
                {
                    string val = line[j];
                    if (val.Length>0) { 
                        try {
                            measurement mm = dict_measurement[header[j]];
                            var newMOND = new MOND();
                            newMOND.gINTProjectID = gINTProjectID;
                            newMOND.PointID = line[colPOINT_ID];
                            newMOND.ItemKey = line[colMOND_ID];
                            
                            MONG.sqlWhere("gINTProjectID=" + gINTProjectID + " and PointID='" + newMOND.PointID + "' and ItemKey='" + newMOND.ItemKey +"'");
                            MONG.getDataTable();
                            if (!MONG.EOF()) {
                            DataRow row = MONG.FirstRow();
                            newMOND.MONG_DIS = Convert.ToDouble(row["MONG_DIS"]); 
                            }
                            
                            newMOND.MOND_REF = line[colMOND_REF];
                            newMOND.DateTime = Convert.ToDateTime(line[colDATE_TIME]);
                            newMOND.MOND_RDNG = val;
                            newMOND.MOND_TYPE = mm.Type;
                            newMOND.MOND_NAME = mm.Name;
                            newMOND.MOND_UNIT = mm.Units;
                        
                            AddMOND(newMOND, true);
                        
                        } catch (Exception e) {
                            Console.Write (e.Message);
                        }
                    }
                }
            }
        return Ok();

 } 
 private  async Task<IActionResult> addLoggerLayout101(ge_data data, string [] lines) {

        dbConnectDetails cd = await getConnectDetails(data.projectId,gINTTables.DB_DATA_TYPE);
      
        if (cd==null) {
            return NotFound();
        }
        
        int gINTProjectID = cd.ProjectId;
            
            string POINT_ID = "PointId";
            string MOND_ID = "Monitoring Id";
            string WATER_DEPTH = "Water Depth";
            string DATE_TIME = "DateTime";
            measurement mm = dict_measurement[WATER_DEPTH];

            string[] header = lines[0].Split(",");
            int colPOINT_ID = header.findColumn(POINT_ID);
            int colMOND_ID = header.findColumn(MOND_ID);
            int colDATE_TIME= header.findColumn(DATE_TIME);
            int colWATER_DEPTH = header.findColumn(WATER_DEPTH);

            int row_start = 1;
            int col_start = colDATE_TIME+1 ;
            
            for (int i = row_start; i < lines.Length; i++)
            {
                string[] line =  lines[i].Split(",");
                        try {
                            var newMOND = new MOND();
                            newMOND.gINTProjectID = gINTProjectID;
                            newMOND.PointID = line[colPOINT_ID];
                            newMOND.ItemKey = line[colMOND_ID];

                            MONG.sqlWhere ("gINTProjectID=" + gINTProjectID + " and PointID='" + newMOND.PointID + "' and ItemKey='" + newMOND.ItemKey +"'");
                            MONG.getDataTable();
                            if (!MONG.EOF()) {
                            DataRow row = MONG.FirstRow();
                            newMOND.MONG_DIS = Convert.ToDouble(row["MONG_DIS"]); 
                            }
                            
                            newMOND.DateTime = Convert.ToDateTime(line[colDATE_TIME]);
                            newMOND.MOND_RDNG = line[colWATER_DEPTH];
                            newMOND.MOND_TYPE = mm.Type;
                            newMOND.MOND_NAME = mm.Name;
                            newMOND.MOND_UNIT = mm.Units;
                        
                            AddMOND(newMOND, true);
                        
                        } catch (Exception e) {
                        Console.Write (e.Message);
                        }
            }

            return Ok();
 }

  private async Task<dbConnectDetails> getConnectDetails (Guid projectId, string dbType) {
            
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

            if (cs==null) {    
                return null;
            }
     
            dbConnectDetails cd = cs.getConnectType(dbType);

            return cd;
    }
            
  
     private class measurement {

         public string Type {get;set;}
         public string Units {get;set;}
         public string Name {get;set;}

     }


}

 }