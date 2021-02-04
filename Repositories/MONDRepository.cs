using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ge_repository.interfaces;
using ge_repository.OtherDatabase;

namespace ge_repository.repositories
{
public class MONDRepository : GintRepositoryADO<MOND>, IGintRepository<MOND>
    {

     public MONDRepository(SqlConnection conn, int gINTProjectId) 
            : base("MOND", "GintRecID", conn, gINTProjectId)
        { }
    public async Task<IEnumerable<MOND>> GetWhereAsync(string where) {

        return await Task.Run (() => {
                                          DataRow[] row = _table.dataTable.Select (where);
                                          if (row==null) {
                                              _table.sqlWhere(where);
                                              _table.getDataTable();
                                          }
                                          return _table.TableAsList();
                                    });

    }
    public void set_values(MOND item, DataRow row) {
                        
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

    public override async Task<int> UpdateRangeAsync(IEnumerable<MOND> save_items, 
                                 string where = null )
                                 {   

    string wherePointID = "";
    double? whereMONG_DIS = null;
    int? wheregINTProjectID = null;
  
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
    
    var gintprojectid = save_items.Select(mond => new { mond.gINTProjectID})
                      .Distinct().ToList();

    if (gintprojectid.Count==1) {
        wheregINTProjectID= gintprojectid[0].gINTProjectID;
        if (wheregINTProjectID == null) {
            wheregINTProjectID = _gINTProjectID;
        }
    }

    DateTime minDateTime =  save_items.Min(e=>e.DateTime).GetValueOrDefault();
    DateTime maxDateTime =  save_items.Max(e=>e.DateTime).GetValueOrDefault();

        
        return await Task.Run(() =>
        
        {
                // reduce the dataset, all logger records could be massive

                if (where != null && wherePointID == "") {
                        _table.sqlWhere($"gINTProjectID={wheregINTProjectID} and {where} and DateTime>='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",minDateTime)}' and DateTime<='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",maxDateTime)}'");
                }
                
                if (where == null && wherePointID == "") {
                        _table.sqlWhere($"gINTProjectID={wheregINTProjectID} and DateTime>='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",minDateTime)}' and DateTime<='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",maxDateTime)}'");    
                }

                if (where != null && wherePointID !="" && whereMONG_DIS ==null) {
                        _table.sqlWhere($"gINTProjectID={wheregINTProjectID} and {where} and PointID='{wherePointID}' and DateTime>='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",minDateTime)}' and DateTime<='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",maxDateTime)}'");
                }
                
                if (where != null && wherePointID !="" && whereMONG_DIS !=null) {
                        _table.sqlWhere($"gINTProjectID={wheregINTProjectID} and {where} and PointID='{wherePointID}' and DateTime>='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",minDateTime)}' and DateTime<='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",maxDateTime)}' and MONG_DIS={whereMONG_DIS.Value}");
                }
   
                _table.getDataSet();
                DataTable dt = _table.getDataTable();

                Boolean checkExisting = false;

                if (dt.Rows.Count>0) {
                    checkExisting=true;
                }

                foreach (MOND item in save_items) {

                        DataRow row = null;
                        
                        if (item.gINTProjectID==0) item.gINTProjectID = wheregINTProjectID.Value;
                        
                        if (checkExisting==true) {
                            //check for existing records
                            if (item.GintRecID>0) {
                            row = dt.Select ($"GintRecID={item.GintRecID}").SingleOrDefault();
                            }

                           // if (row == null && item.ge_otherid!=null) {
                                // try and check for existing ge_generated records, but some ge_source and ge_otherid 
                                // combination may results in mutiple matches in the MOND table 
                                // so this is not a reliable way of identifying singular records
                          //      try {
                           //         row = dtMOND.Select ($"ge_source='{item.ge_source}' and ge_otherId='{item.ge_otherid}' and MOND_TYPE='{item.MOND_TYPE}'").SingleOrDefault();
                          //      } catch {
                          //      row = null;
                          //      }
                          //  }
                            
                            if (item.DateTime == DateTime.Parse("21 Oct 2019 12:45PM")) {
                               Console.Write (item); 
                            }

                            //check for unique records
                            if (row == null) {
                                string s1 = $"gINTProjectID={item.gINTProjectID} and PointId='{item.PointID}' and ItemKey='{item.ItemKey}' and MONG_DIS={item.MONG_DIS} and MOND_TYPE='{item.MOND_TYPE}' and DateTime='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",item.DateTime)}' and MOND_REF='{item.MOND_REF}'";
                                if (item.MOND_REF !=null) row = dt.Select ($"gINTProjectID={item.gINTProjectID} and PointId='{item.PointID}' and ItemKey='{item.ItemKey}' and MONG_DIS={item.MONG_DIS} and MOND_TYPE='{item.MOND_TYPE}' and DateTime='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",item.DateTime)}' and MOND_REF='{item.MOND_REF}'").SingleOrDefault();
                                if (item.MOND_REF == null) row = dt.Select ($"gINTProjectID={item.gINTProjectID} and PointId='{item.PointID}' and ItemKey='{item.ItemKey}' and MONG_DIS={item.MONG_DIS} and MOND_TYPE='{item.MOND_TYPE}' and DateTime='{String.Format("{0:yyyy-MM-dd HH:mm:ss}",item.DateTime)}' and MOND_REF is null").SingleOrDefault();
                            }
                        }

                        if (row == null) {
                            row = _table.NewRow();
                            _table.addRow (row);
                        }

                        set_values(item, row);                        
                } 
                    row_states ret = _table.get_row_states();
                    return ret.updated;

           
        });
 }
    public async Task<int> CommitBulkAsync(){ 
    
        return await Task.Run (() => {
                                    row_states ret = _table.get_row_states();
                                    ret.updated = _table.BulkUpdate();
                                    return ret.updated;
                                    });

    }
    public async Task<int> CommitAsync(){ 
    
        return await Task.Run (() => {
                                    int rec_updated = _table.Update();
                                    return rec_updated;
                                    });

    }
    
    }

}

