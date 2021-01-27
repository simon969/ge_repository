using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ge_repository.interfaces;
using ge_repository.OtherDatabase;

namespace ge_repository.repositories
{
    public class LoggerFileRepository : RepositoryADOParentChild<ge_log_file, ge_log_reading>, ILoggerFileRepository
    {
        
        public LoggerFileRepository(SqlConnection conn) 
            : base("ge_log_file", "Id", "ge_log_reading","fileId", conn)
        { }
        public LoggerFileRepository(dsTable<ge_log_file> log_file, dsTable<ge_log_reading> log_reading) 
            : base(log_file, log_reading)
        { }
        public async Task<IEnumerable<ge_log_file>> GetAllLoggerFilesAsync() {
            
            return await Task.Run (() =>
                                    {
                    return _parent.TableAsList();
                                    }
                             );
            
        }
        public async Task<IEnumerable<ge_log_file>> GetAllLoggerFilesWithoutReadingsAsync()
        {
           return await Task.Run (() =>
                                    {
                    return _parent.TableAsList();
                                    }


                             );
        }
        public async Task<ge_log_file> GetByIdWithoutReadingsAsync(Guid Id) {
        return await Task.Run (() =>
                                    {
                                          string where = $"Guid={Id}";
                                          DataRow row = _parent.dataTable.Select (where).SingleOrDefault();
                                          if (row==null) {
                                              _parent.sqlWhere(where);
                                              _parent.getDataTable();
                                              row = _parent.dataTable.Rows[0];
                                          }
                                                        
                                          ge_log_file file = new ge_log_file();
                                          get_values(row, file);
                                          return file;
                                    });
        }
        public override async Task<ge_log_file> GetByIdAsync(string Id) { 
            Guid Guid = new Guid(Id);
            return await GetByIdAsync(Guid);
        }
        public override async Task<ge_log_file> GetByIdAsync(int Id) {
           return null;
        } 
        private async Task<ge_log_file> GetWhereParentAsync(string _where) {
        return await Task.Run (() =>
                                    {
                                          DataRow row = _parent.dataTable.Select (_where).SingleOrDefault();
                                          if (row==null) {
                                              _parent.sqlWhere(_where);
                                              _parent.getDataTable();
                                              row = _parent.dataTable.Rows[0];
                                          }
                                                        
                                          ge_log_file file = new ge_log_file();
                                          get_values(row, file);
                                          
                                          string rwhere = $"fileId={file.Id}";
                                          DataRow[] rows = _child.dataTable.Select (rwhere);
                                          if (rows==null) {
                                              _child.sqlWhere(rwhere);
                                              _child.getDataTable();
                                              rows = _child.dataTable.Select();
                                          }

                                          file.readings = new List<ge_log_reading>();

                                          foreach(DataRow rrow in rows)
                                            {    
                                                ge_log_reading r =  new ge_log_reading();
                                                get_values(rrow, r);
                                                file.readings.Add(r);
                                            }  
                                        file.OrderReadings();
                                        file.unpack_exist_file();
                                        return file;
                                    }

                             );
           

        }
        public override async Task<ge_log_file> GetByIdAsync(Guid Id) {
            return await GetWhereParentAsync ($"Id='{Id}'");

        }
        public async Task<ge_log_file> GetByDataIdAsync(Guid Id, string table) {
            return await GetWhereParentAsync ($"DataId='{Id}' and channel='{table}'");

        }
       public async Task<int> AddAsync (ge_log_file file) {

       return await Task.Run(() =>
                                    {
                                            DataRow new_file = _parent.NewRow();
                                            file.Id = Guid.NewGuid();
                                            set_values (file, new_file);
                                            _parent.addRow (new_file);
                                            
                                            foreach (ge_log_reading reading in file.readings) {
                                                DataRow new_reading = _child.NewRow();
                                                reading.Id = Guid.NewGuid();
                                                reading.fileId = file.Id;
                                                set_values (reading, new_reading);
                                                _child.addRow (new_reading);
                                            }
                                            
                                            return 1;

                                    });
    }
     private void get_values(DataRow row, ge_log_reading reading) {

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
    private void set_values(ge_log_reading reading, DataRow row) {

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
    private void get_values(DataRow row, ge_log_file file) {

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

    private void set_values(ge_log_file file, DataRow row) {

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

    public async Task<int> CommitAsync(){ 
    
        return await Task.Run (() => {
                                    int p = _parent.Update();
                                    int c = _child.Update();
                                    return p+c;
                                    });

    }

    }

}