using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
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
using Newtonsoft.Json;
using ge_repository.spatial;
using System.Xml.Serialization;


namespace ge_repository.Controllers
{

    public class ge_logdbController: ge_Controller  {     
   
        public List<ge_log_file> log_files {get;set;}
        
        public ge_logdbController(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager, 
            IHostingEnvironment env,
		 	IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
            
        }
    //Create
    [HttpPost]
    public async Task<IActionResult>  Post (string s1, string format ) {
        
        ge_log_file log_file =  null;

       if (format == "json") {     
            log_file = JsonConvert.DeserializeObject<ge_log_file>(s1);
       }
       
       if (format=="xml") {
            log_file = s1.DeserializeFromXmlString<ge_log_file>();
       }

        var resp = await AddNewFile(log_file);

        if (resp == -1) {
            BadRequest("Unable to add new log file");
        }

        return Ok($"log file id:{log_file.Id} created");
            
    }
   public async Task<IActionResult>  GetAll (Guid dataId, 
                                         Boolean IncludeReadings=true) {
    

        var resp = await GetFiles (dataId, IncludeReadings);

        if (resp==null) {
            return BadRequest ($"Not Found dataId:{dataId} ");
        }   
        
        return Ok(resp);    
    }
    //Read
    [HttpGet]
    public async Task<IActionResult>  Get ( Guid dataId, 
                                            string table = "data_pressure", 
                                            Boolean IncludeReadings=true) {
    

        var resp = await GetFile (dataId, table, IncludeReadings);

        if (resp==null) {
            return BadRequest ($"Not Found dataId:{dataId} table:{table}");
        }   
        
        return Ok(resp);    
    }
    
    //Update
    [HttpPut]
    public async Task<IActionResult>  Put (string s1, Boolean IncludeReadings, string format) {

        ge_log_file log_file = null;;

        if (format == "json") {     
            log_file = JsonConvert.DeserializeObject<ge_log_file>(s1);
        }
       
        if (format=="xml") {
            log_file = s1.DeserializeFromXmlString<ge_log_file>();
        }

        var resp = await UpdateFile (log_file, IncludeReadings);
        
        if (resp == -1) {
            BadRequest ($"Unable to update log_file Id:{log_file.Id} dataId:{log_file.dataId} table:{log_file.channel}");
        }   
        
        return Ok(resp);  
    
    
    }
    
    //Update
    [HttpPatch]
    public async Task<IActionResult>  Patch (string ge_file) {
        return new EmptyResult();    
    } 

    //Delete
    [HttpDelete]
    public async Task<IActionResult>  Delete (string ge_file) {
        return new EmptyResult();    
    }

   
    private async Task<int> AddNewFile(ge_log_file file) {
    
        int NOT_OK = -1;
        int ret = 0;
        
        if (file == null) {
                return NOT_OK;
        }
        
        file.packFieldHeaders();
        file.packFileHeader();
        
        var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == file.dataId);

        dbConnectDetails cd = await getConnectDetails(_data.projectId,logTables.DB_DATA_TYPE);
      
        if (cd==null) {
            return NOT_OK;
        }

        string dbConnectStr = cd.AsConnectionString();

        return await Task.Run(() =>
            {
                using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
                {
                    dsTable ds_readings = new logTables().reading;
                    dsTable ds_file = new logTables().file;
                    cnn.Open();
                    ds_file.setConnection (cnn);  
                    ds_file.Reset(); 
                           
                    ds_readings.setConnection (cnn);
                    ds_readings.Reset();

                    DataTable dt_file = ds_file.getDataTable();
                    DataRow file_row = dt_file.NewRow();
                    
                    file.Id = Guid.NewGuid();
                    set_log_file_values (file, file_row);
                    ds_file.addRow (file_row);

                    ret = ds_file.Update();
                    
                    DataTable dt_readings = ds_readings.getDataTable();
                    
                    foreach (ge_log_reading reading in file.readings) {
                        DataRow row = dt_readings.NewRow();
                        reading.Id = Guid.NewGuid();
                        reading.fileId = file.Id;
                        set_log_reading_values (reading, row);
                        ds_readings.addRow (row);
                    }

                    ret = ret + ds_readings.Update();
                    return ret;

                }
            });
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
    private void get_log_reading_values(DataRow row, ge_log_reading reading) {

                reading.Id = (Guid) row ["Id"];
                reading.fileId = (Guid) row["fileId"];     
                reading.ReadingDatetime = (DateTime) row["ReadingDateTime"];
                if (row["Duration"] != DBNull.Value) reading.Duration= Convert.ToInt64(row["Duration"].ToString());
                if (row["Value1"] != DBNull.Value) reading.Value1 =Convert.ToSingle( row["Value1"].ToString());
                if (row["Value2"] != DBNull.Value) reading.Value2= Convert.ToSingle(row["Value2"].ToString());
                if (row["Value3"] != DBNull.Value) reading.Value3 =Convert.ToSingle( row["Value3"].ToString());
                if (row["Value4"] != DBNull.Value) reading.Value4 = Convert.ToSingle(row["Value4"].ToString());
                if (row["Value5"] != DBNull.Value)  reading.Value5 =Convert.ToSingle(row["Value5"].ToString());
                if (row["Value6"] != DBNull.Value)  reading.Value6 =Convert.ToSingle(row["Value6"].ToString());
                if (row["Value7"] != DBNull.Value)  reading.Value7 =Convert.ToSingle(row["Value7"].ToString());
                if (row["Value8"] != DBNull.Value) reading.Value8= Convert.ToSingle(row["Value8"].ToString());
                if (row["Value9"] != DBNull.Value) reading.Value9 =Convert.ToSingle( row["Value9"].ToString());
                if (row["Value10"] != DBNull.Value) reading.Value10 = Convert.ToSingle(row["Value10"].ToString());
                if (row["Value11"] != DBNull.Value)  reading.Value11 =Convert.ToSingle(row["Value11"].ToString());
                if (row["Value12"] != DBNull.Value)  reading.Value12 =Convert.ToSingle(row["Value12"].ToString());
                if (row["Value13"] != DBNull.Value)  reading.Value13 =Convert.ToSingle(row["Value13"].ToString());
                if (row["Value14"] != DBNull.Value) reading.Value14 =Convert.ToSingle( row["Value14"].ToString());
                if (row["Value15"] != DBNull.Value) reading.Value15 = Convert.ToSingle(row["Value15"].ToString());
                if (row["Value16"] != DBNull.Value)  reading.Value16 =Convert.ToSingle(row["Value16"].ToString());
                if (row["Value17"] != DBNull.Value)  reading.Value17 =Convert.ToSingle(row["Value17"].ToString());
                if (row["Value18"] != DBNull.Value)  reading.Value18 =Convert.ToSingle(row["Value18"].ToString());
                if (row["Remark"] != DBNull.Value) reading.Remark = (String) row["Remark"]; 
                reading.Valid = (int) row["Valid"];
                reading.Include = (int) row["Include"];
                reading.pflag = (int) row["pflag"];
                reading.NotDry = (int) row["NotDry"];
    }
    private void set_log_reading_values(ge_log_reading reading, DataRow row) {

                row["Id"] = reading.Id;
                row["fileId"] = reading.fileId;
                row["ReadingDateTime"] = reading.ReadingDatetime;
                if (reading.Duration==null) { row["Duration"] = DBNull.Value;} else {row["Duration"] = reading.Duration;} 
                if (reading.Value1==null) { row["Value1"] = DBNull.Value;} else {row["Value1"] = reading.Value1;} 
                if (reading.Value2==null) { row["Value2"] = DBNull.Value;} else {row["Value2"] = reading.Value2;} 
                if (reading.Value3==null) { row["Value3"] = DBNull.Value;}  else {row["Value3"] = reading.Value3;}
                if (reading.Value4==null) { row["Value4"] = DBNull.Value;} else {row["Value4"] = reading.Value4;} 
                if (reading.Value5==null) { row["Value5"] = DBNull.Value;} else {row["Value5"] = reading.Value5;} 
                if (reading.Value6==null) { row["Value6"] =  DBNull.Value;} else {row["Value6"] =reading.Value6;}
                if (reading.Value7==null) { row["Value7"] = DBNull.Value;} else {row["Value7"] = reading.Value7;} 
                if (reading.Value8==null) { row["Value8"] = DBNull.Value;} else {row["Value8"] = reading.Value8;} 
                if (reading.Value9==null) { row["Value9"] = DBNull.Value;}  else {row["Value9"] = reading.Value9;}
                if (reading.Value10==null) { row["Value10"] = DBNull.Value;} else {row["Value10"] = reading.Value10;} 
                if (reading.Value11==null) { row["Value11"] = DBNull.Value;} else {row["Value11"] = reading.Value11;} 
                if (reading.Value12==null) { row["Value12"] =  DBNull.Value;} else {row["Value12"] =reading.Value12;}
                if (reading.Value13==null) { row["Value13"] = DBNull.Value;} else {row["Value13"] = reading.Value13;} 
                if (reading.Value14==null) { row["Value14"] = DBNull.Value;} else {row["Value14"] = reading.Value14;} 
                if (reading.Value15==null) { row["Value15"] = DBNull.Value;} else {row["Value15"] = reading.Value15;} 
                if (reading.Value16==null) { row["Value16"] =  DBNull.Value;} else {row["Value16"] =reading.Value16;}
                if (reading.Value17==null) { row["Value17"] = DBNull.Value;} else {row["Value17"] = reading.Value17;} 
                if (reading.Value18==null) { row["Value18"] = DBNull.Value;} else {row["Value18"] = reading.Value18;} 
                row["Remark"] = reading.Remark;
                row["Valid"] = reading.Valid;
                row["Include"] = reading.Include;
                row["pflag"] = reading.pflag;
                row["NotDry"] = reading.NotDry;

    }
    private void get_log_file_values(DataRow row, ge_log_file file) {

                file.Id = (Guid) row["Id"];
                file.dataId = (Guid) row["dataId"]; 
                if (row["ReadingAggregates"] != DBNull.Value) file.readingAggregates = (String) row["ReadingAggregates"]; 
                if (row["FieldHeader"] != DBNull.Value) file.fieldHeader= (String) row["FieldHeader"]; 
                if (row["FileHeader"] != DBNull.Value) file.fileHeader= (String) row["FileHeader"]; 
                if (row["Comments"] != DBNull.Value) file.Comments= (String) row["Comments"]; 
                if (row["channel"] != DBNull.Value) file.channel = (String) row["channel"]; 
                if (row["SearchTemplate"] != DBNull.Value) file.SearchTemplate = (String) row["SearchTemplate"];
                if (row["templateId"] !=DBNull.Value) file.templateId = (Guid) row["templateId"]; 

    }

    private void set_log_file_values(ge_log_file file, DataRow row) {

                row["Id"] = file.Id;
                row["dataId"] = file.dataId;
                row["fieldHeader"] = file.fieldHeader;
                row["ReadingAggregates"] = file.readingAggregates;
                row["FileHeader"] = file.fileHeader;
                row["Comments"] = file.Comments;
                row["channel"] = file.channel;
                row["templateId"] = file.templateId;
                row["SearchTemplate"] = file.SearchTemplate;

    }

        private async Task<List<ge_log_file>> GetFiles (Guid dataId, 
                                        Boolean IncludeReadings=true) {

        if (dataId == null) {
                return null;
        }
        
        
        var _logger = await _context.ge_data
                                    .Include(d =>d.project)
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync(m => m.Id == dataId);

        dbConnectDetails cd = await getConnectDetails(_logger.projectId,logTables.DB_DATA_TYPE);
      
        if (cd==null) {
            return null;
        }

        string dbConnectStr = cd.AsConnectionString();
        
        var dt_file2 =  await Task.Run(() =>    
                            {
                                using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
                                {
                                    cnn.Open();
                                    dsTable ds_files = new logTables().file;
                                    ds_files.setConnection (cnn);        
                                    ds_files.sqlWhere("dataId='" + dataId.ToString() + "'" );    
                                                        
                                    ds_files.getDataSet();
                                    DataTable dt_file = ds_files.getDataTable();
                                    return dt_file;

                                    
                                }   
                            });
        
        List<ge_log_file> local_log_files = new List<ge_log_file>();

        foreach (DataRow row in dt_file2.Rows) {
                ge_log_file file = new ge_log_file();
                get_log_file_values(row, file);
                
                if (IncludeReadings==true) {
                file = await GetFile(file.dataId, file.channel, true);
                }

                local_log_files.Add (file);
        } 
        
        
        return local_log_files;




}
    private async Task<ge_log_file> GetFile (Guid dataId, 
                                        string table = "data_pressure", 
                                        Boolean IncludeReadings=true) {

        if (dataId == null) {
                return null;
        }
        
        
        var _logger = await _context.ge_data
                                    .Include(d =>d.project)
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync(m => m.Id == dataId);

        dbConnectDetails cd = await getConnectDetails(_logger.projectId,logTables.DB_DATA_TYPE);
      
        if (cd==null) {
            return null;
        }

        string dbConnectStr = cd.AsConnectionString();
        
        return await Task.Run(() =>
        
        {
                    using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
                    {
                        cnn.Open();
                        dsTable ds_readings = new logTables().reading;
                        dsTable ds_file = new logTables().file;
                        ds_file.setConnection (cnn);        
                        ds_readings.setConnection (cnn);
                        
                        //Multichannel transducer have upto 8 tables which will all have the same dataId

                        if (string.IsNullOrEmpty(table)) {
                        ds_file.sqlWhere("dataId='" + dataId.ToString() + "' and (channel is null or channel='')");
                        } else {
                        ds_file.sqlWhere("dataId='" + dataId.ToString() + "' and channel='" + table + "'" );    
                        }
                        
                        ds_file.getDataSet();
                        DataTable dt_file = ds_file.getDataTable();
    
                        if (dt_file==null) {
                            return null;
                        } 
                        
                        if (dt_file.Rows.Count==0) {
                            return null;
                        }

                        ge_log_file file = new ge_log_file();
                        
                        DataRow row = dt_file.Rows[0];
                        get_log_file_values(row, file);

                       
                        if (IncludeReadings) {
                            ds_readings.sqlWhere("FileId='" + file.Id.ToString() + "'");
                            ds_readings.getDataSet();
                            DataTable dt_readings = ds_readings.getDataTable();
                            file.readings = new List<ge_log_reading>();

                            foreach(DataRow rrow in dt_readings.Rows)
                            {    
                                ge_log_reading r =  new ge_log_reading();
                                get_log_reading_values(rrow, r);
                                file.readings.Add(r);
                            }  
                        file.OrderReadings();
                        }

                        file.unpack_exist_file();
                                            
                        return file;
                    }   
            });

}

private async Task<int> UpdateChannel (Guid[] Id, Guid dataId, string header, float [] values) {

//value_header vh = Json.Convert<value_header>(header);

        int NOT_OK = -1;
        int ret = 0;
        
        if (dataId == null) {
            return NOT_OK;
        }

        var _logger = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == dataId);
        if (_logger == null) {
            return NOT_OK;
        }

        dbConnectDetails cd = await getConnectDetails(_logger.projectId,logTables.DB_DATA_TYPE);
      
        if (cd==null) {
            return NOT_OK;
        }

        string dbConnectStr = cd.AsConnectionString();
        return await Task.Run(() =>
        
        {
            using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
            {
                dsTable ds_readings = new logTables().reading;
                cnn.Open();
                ds_readings.setConnection (cnn);
                ds_readings.getDataTable();
                ds_readings.sqlWhere("Id='" + Id.ToString() + "'");
                ds_readings.getDataSet();
                DataTable dt_readings = ds_readings.getDataTable();

                DataRow row = dt_readings.Rows[0];
                // if (Valid!=null) row["Valid"] = Valid;
                // if (Include!=null) row["Include"] = Include;
                // if (pflag!=null)  row["pflag"] =  pflag;
                // if (NotDry!=null) row["NotDry"] = NotDry;
                // if (Remark!=null)  row["Remark"] = Remark;
                ret = ds_readings.Update();
                return ret;  
            }

        });

}


private async Task<int> UpdateReading (Guid Id,
                                    Guid dataId,
                                    int? Valid,
                                    int? Include,
                                    int? pflag,
                                    int? NotDry,
                                    string Remark) {

        
        int NOT_OK = -1;
        int ret = 0;
        
        if (dataId == null) {
            return NOT_OK;
        }

        var _logger = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == dataId);
        if (_logger == null) {
            return NOT_OK;
        }

        dbConnectDetails cd = await getConnectDetails(_logger.projectId,logTables.DB_DATA_TYPE);
      
        if (cd==null) {
            return NOT_OK;
        }

        string dbConnectStr = cd.AsConnectionString();
        return await Task.Run(() =>
        
        {
            using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
            {
                dsTable ds_readings = new logTables().reading;
                cnn.Open();
                ds_readings.setConnection (cnn);
                ds_readings.getDataTable();
                ds_readings.sqlWhere("Id='" + Id.ToString() + "'");
                ds_readings.getDataSet();
                DataTable dt_readings = ds_readings.getDataTable();

                DataRow row = dt_readings.Rows[0];
                if (Valid!=null) row["Valid"] = Valid;
                if (Include!=null) row["Include"] = Include;
                if (pflag!=null)  row["pflag"] =  pflag;
                if (NotDry!=null) row["NotDry"] = NotDry;
                if (Remark!=null)  row["Remark"] = Remark;
                ret = ds_readings.Update();
                return ret;  
            }

        });

}



private async Task<int>  UpdateFile (ge_log_file file, Boolean IncludeReadings) {

        int NOT_OK = -1;
        int ret = 0;
        
        if (file.dataId == null) {
                return NOT_OK;
        }
        
        file.packFieldHeaders();
        file.packFileHeader();

        var _logger = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == file.dataId);

        dbConnectDetails cd = await getConnectDetails(_logger.projectId,logTables.DB_DATA_TYPE);
      
        if (cd==null) {
            return NOT_OK;
        }

        string dbConnectStr = cd.AsConnectionString();
        
        return await Task.Run(() =>
        
        {
            using ( SqlConnection cnn = new SqlConnection(dbConnectStr)) 
            {
                dsTable ds_readings = new logTables().reading;
                dsTable ds_file = new logTables().file;
                cnn.Open();
                ds_file.setConnection (cnn);        
                ds_file.getDataTable ();  
                ds_readings.setConnection (cnn);
                ds_readings.getDataTable();
                ds_file.sqlWhere("Id='" + file.Id + "'");
                ds_file.getDataSet();
                DataTable dt_file = ds_file.getDataTable();

                if (dt_file==null) {
                    return NOT_OK;
                } 
                
                if (dt_file.Rows.Count==0) {
                    return NOT_OK;
                }

                DataRow file_row = dt_file.Rows[0];
                set_log_file_values (file, file_row);
                ret = ds_file.Update();
                
                if (IncludeReadings) {  
                    ds_readings.sqlWhere("FileId='" + file.Id.ToString() + "'");
                    ds_readings.getDataSet();
                    DataTable dt_readings = ds_readings.getDataTable();
                    Boolean checkExisting = false;
                    
                    if (dt_readings.Rows.Count>0) {
                        checkExisting=true;
                    }

                    foreach (ge_log_reading reading in file.readings) {
                        
                        DataRow row = null;
                        if (checkExisting==true) {
                            if (reading.Id != Guid.Empty) {
                            row = dt_readings.Select ($"Id='{reading.Id}'").SingleOrDefault();
                            }

                            if (row==null) {
                            row =  dt_readings.Select ($"ReadingDateTime='{String.Format("{0:yyyy-MM-dd HH:mm:ss.ffff}",reading.ReadingDatetime)}'").SingleOrDefault();
                            }
                        }

                        if (row==null) {
                            row = ds_readings.NewRow();
                            reading.Id = Guid.NewGuid();
                            reading.fileId = file.Id;
                            ds_readings.addRow (row); 
                        } else {
                            reading.Id = (Guid) row["Id"];
                            reading.fileId = file.Id;
                        }
                       
                        set_log_reading_values (reading,row);
                    }

                    //what if there are other records (more) in dt_readings from a previous version of the ge_log_file? 
                    // mark for deletion all records not 'new' or 'updated'
                    if (file.readings.Count() < dt_readings.Rows.Count) {
                        foreach (DataRow row in dt_readings.Rows) {
                        if (row.RowState == DataRowState.Added | 
                            row.RowState != DataRowState.Modified) {
                                row.Delete();
                            }

                        } 
                    }

                    
                    ret = ret + ds_readings.Update();
                    return ret;
                } 
            return ret;  
            }
        });

} 


    }


}