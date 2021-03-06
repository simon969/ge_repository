using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ge_repository.interfaces;
using ge_repository.OtherDatabase;
using ge_repository.Extensions;

namespace ge_repository.repositories
{
    public class LoggerFileRepositoryADO : RepositoryADOParentChild<ge_log_file, ge_log_reading>, ILoggerFileRepository
    {
        
        public LoggerFileRepositoryADO(SqlConnection conn) 
            : base("ge_log_file", "Id", "ge_log_reading","fileId", conn)
        { }
        public LoggerFileRepositoryADO(dsTable<ge_log_file> log_file, dsTable<ge_log_reading> log_reading) 
            : base(log_file, log_reading)
        { }
        public async Task<ge_log_file> GetByIdNoReadingsAsync(Guid Id) {
             return await GetWhereParentAsync ($"Id='{Id}'", false);
        }
       
        public async Task<IEnumerable<ge_log_file>> GetAllLoggerFilesAsync() {
            
            return await Task.Run (() =>
                                    {
                    return _parent.TableAsList();
                                    }
                             );
            
        }
        public async Task<IEnumerable<ge_log_file>> GetAllLoggerFilesNoReadingsAsync()
        {
           return await Task.Run (() =>
                                    {
                    return _parent.TableAsList();
                                    }


                             );
        }

        public async Task<ge_log_file> GetByDataIdNoReadingsAsync(Guid Id, string table) {
            return await GetWhereParentAsync ($"DataId='{Id}' and channel='{table}'", false);
        }
        public async Task<IEnumerable<ge_log_file>> GetAllByDataIdAsync(Guid Id) {
            return await GetAllWhereParentAsync ($"Guid='{Id}'", true);
        }
        public async Task<IEnumerable<ge_log_file>> GetAllByDataIdNoReadingsAsync(Guid Id) {
            return await GetAllWhereParentAsync ($"Guid='{Id}'", false);
        }
        public override async Task<ge_log_file> FindByIdAsync(string Id) { 
            Guid Guid = new Guid(Id);
            return await FindByIdAsync(Guid);
        }
        public override async Task<ge_log_file> FindByIdAsync(int Id) {
           return null;
        } 
        public async Task<IEnumerable<ge_log_file>> GetAllWhereParentAsync(string _where, Boolean include_child) {
        
        return await Task.Run (() =>
                                    {       
                                        _parent.sqlWhere(_where);
                                        _parent.getDataTable();
                                        
                                        if (_parent.dataTable == null) {
                                            return null;
                                        }       
                                        
                                        if (_parent.dataTable.Rows.Count==0) {
                                            return null;
                                        }

                                        if (include_child == false) {
                                            return _parent.TableAsList();
                                        }
                                        
                                        List<ge_log_file> files = new List<ge_log_file>();

                                        foreach (DataRow row in _parent.dataTable.Rows) {
                                            
                                            ge_log_file file = new ge_log_file();
                                            get_values(row, file);
                                            
                                            _child.sqlWhere($"fileId='{file.Id}'");
                                            _child.getDataTable();
                                            file.readings = _child.TableAsList();
                                                                                        
                                            file.OrderReadings();
                                            file.unpack_exist_file();
                                            files.Add (file);

                                        }

                                        return files;
                                    }
                                        
                                );
        }
        private async Task<ge_log_file> GetWhereParentAsync(string _where, Boolean include_child) {

        return await Task.Run (() =>
                                    {       
                                        _parent.sqlWhere(_where);
                                        _parent.getDataTable();
                                        
                                        if (_parent.dataTable==null) {
                                            return null;
                                        }       
                                        
                                        if (_parent.dataTable.Rows.Count==0) {
                                            return null;
                                        }
                                        DataRow row = _parent.dataTable.Rows[0];
                                                                                           
                                        ge_log_file file = new ge_log_file();
                                        get_values(row, file);
                                        
                                        if (include_child == false) {
                                        return file;
                                        }
                                        
                                        _child.sqlWhere($"fileId='{file.Id}'");
                                        _child.getDataTable();

                                        file.readings = _child.TableAsList();
                                        
                                        file.OrderReadings();
                                        file.unpack_exist_file();
                                        return file;
                                    }
                                );
        }
        private async Task<IEnumerable<ge_log_reading>> GetAllWhereChildFileIdAsync(Guid fileId) { 
                return await GetAllWhereChildAsync($"fileId='{fileId}'");
        }

        private async Task<IEnumerable<ge_log_reading>> GetAllWhereChildAsync(string _where) {

        return await Task.Run (() =>
                                    {       
                                        _child.sqlWhere(_where);
                                        _child.getDataTable();
                                    
                                        if (_child.dataTable==null) {
                                            return null;
                                        }

                                        if (_child.dataTable.Rows.Count==0) {
                                            return null;
                                        } 

                                        return _child.TableAsList();

                                        // DataRow[] rows = _child.dataTable.Select();
                                                                                    
                                        // List<ge_log_reading> readings = new List<ge_log_reading>();

                                        // foreach(DataRow rrow in rows)
                                        // {    
                                        //     ge_log_reading r =  new ge_log_reading();
                                        //     get_values(rrow, r);
                                        //     readings.Add(r);
                                        // }

                                        // return readings;
                                    }
                                );
        }
        public override async Task<ge_log_file> FindByIdAsync(Guid Id) {
            return await GetWhereParentAsync ($"Id='{Id}'", true);

        }
        public async Task<ge_log_file> GetByDataIdAsync(Guid Id, string table) {
            return await GetWhereParentAsync ($"DataId='{Id}' and channel='{table}'", true);

        }
         
       public override async Task AddAsync (ge_log_file file) {

            await Task.Run(() =>
                                    {
                                            if (_parent.dataTable==null) {
                                                _parent.getDataTable();
                                            }

                                            if (_child.dataTable==null) {
                                                _child.getDataTable();
                                            }

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
                                                                                       

                                    });
    }
    private ge_log_file get_file(int row_id, Boolean includereadings) {
            
            if (_parent.dataTable !=null) {
            DataRow row = _parent.dataTable.Rows[row_id]; 
            ge_log_file file = new ge_log_file();
            get_values (row, file);
                if (includereadings) {
                DataRow[] rows = _child.dataTable.Select($"FileId='{file.Id}'");
                file.readings = new List<ge_log_reading>();
                    foreach(DataRow rrow in rows)
                    {    
                        ge_log_reading r =  new ge_log_reading();
                        get_values(rrow, r);
                        file.readings.Add(r);
                    }
                file.OrderReadings();
                file.unpack_exist_file();
                }
            return file;
            }
            return null;
    }
     
     
    public async Task<int> UpdateAsync (ge_log_file file, Boolean IncludeReadings) {

            int NOT_OK = -1;
            int ret = 0;
            
            file.packFieldHeaders();
            file.packFileHeader();

            return await Task.Run(() =>
            
            {
                    if (_parent.dataTable==null) { 
                    _parent.sqlWhere($"Id='{file.Id}' ");
                    _parent.getDataSet();
                    _parent.getDataTable();
                    }
                    DataTable dt_file = _parent.dataTable;

                    if (dt_file == null) {
                        return NOT_OK;
                    } 
                    
                    if (dt_file.Rows.Count==0) {
                        return NOT_OK;
                    }

                    DataRow file_row = dt_file.Rows[0];
                    set_values (file, file_row);
                // ret = _parent.Update();
                    
                    if (IncludeReadings) { 
                        
                        if (_child.dataTable==null) {
                        _child.sqlWhere($"FileId='{file.Id}'");
                        _child.getDataSet();
                        _child.getDataTable();
                        }
                        DataTable dt_readings = _child.dataTable;

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
                                row =  dt_readings.Select ($"ReadingDateTime='{String.Format("{0:yyyy-MM-dd HH:mm:ss.ffff}",reading.ReadingDateTime)}'").SingleOrDefault();
                                }
                            }

                            if (row==null) {
                                row = _child.NewRow();
                                reading.Id = Guid.NewGuid();
                                reading.fileId = file.Id;
                                _child.addRow (row); 
                            } else {
                                reading.Id = (Guid) row["Id"];
                                reading.fileId = file.Id;
                            }
                        
                            set_values (reading, row);
                        }

                        //what if there are other records (more) in dt_readings from a previous version of the ge_log_file? 
                        // mark for deletion all records not 'new' or 'updated'
                        if (file.readings.Count() < dt_readings.Rows.Count) {
                            try {
                            foreach (DataRow row in dt_readings.Rows) {
                            if (row.RowState != DataRowState.Added || 
                                row.RowState != DataRowState.Modified) {
                                    row.Delete();
                                }

                            } 
                            } catch(Exception e) {
                                
                            }
                        }

                        return ret;
                    } 

                return ret;  
                
            });

    } 
     private void get_values(DataRow row, ge_log_reading reading) {

                reading.Id = (Guid) row ["Id"];
                reading.fileId = (Guid) row["fileId"];     
                reading.ReadingDateTime = (DateTime) row["ReadingDateTime"];
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
                row["ReadingDateTime"] = reading.ReadingDateTime;
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
    public void get_values(DataRow row, ge_log_file file) {

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
    public async Task<int> CommitBulkAsync(){ 
        
        return await Task.Run (() => {
                                    int p = _parent.BulkUpdate();
                                    int c = _child.BulkUpdate();
                                    return p+c;
                                    });

    }

    public override bool ExistsLocal(ge_log_file log_file)
    {
        return _parent.dataTable.Select($"Id='{log_file.Id}'") != null;
    }
    
    public bool Exists(ge_log_file log_file)
        {
        return GetByIdNoReadingsAsync(log_file.Id) != null;
        }
    }

}