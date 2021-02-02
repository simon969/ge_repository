using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ge_repository.interfaces;
using ge_repository.OtherDatabase;

namespace ge_repository.repositories
{
public class POINTRepository : GintRepositoryADO<POINT>, IGintRepository<POINT>
    {

     public POINTRepository(SqlConnection conn,int gINTProjectID) 
            : base("MOND","GintRecId",conn, gINTProjectID)
        { }
    public async Task<IEnumerable<POINT>> GetWhereAsync(string where) {

        return await Task.Run (() => {
                                          DataRow[] row = _table.dataTable.Select (where);
                                          if (row==null) {
                                              _table.sqlWhere(where);
                                              _table.getDataTable();
                                          }
                                          return _table.TableAsList();
                                    });

    }
    public void set_values(POINT item, DataRow row) {
                        
                        // row["gINTProjectID"] = item.gINTProjectID;
                        // row["PointID"] = item.PointID;
                        // row["MOND_REF"] = item.MOND_REF;
                        // row["ItemKey"] = item.ItemKey;
                        // if (item.MONG_DIS == null) {row["MONG_DIS"] = DBNull.Value;} else {row["MONG_DIS"] = item.MONG_DIS;}
                        // row["DateTime"] = item.DateTime;    
                        // row["MOND_TYPE"] = item.MOND_TYPE;
                        // row["MOND_RDNG"] = item.MOND_RDNG;
                        // row["MOND_NAME"] = item.MOND_NAME;
                        // row["MOND_UNIT"] = item.MOND_UNIT;
                        // row["MOND_REM"] = item.MOND_REM; 
                        // row["MOND_CONT"] = item.MOND_CONT;
                        // row["MOND_INST"] = item.MOND_INST;
                        // row["MOND_CRED"] = item.MOND_CRED;
                        // if (item.MOND_LIM == null) {row["MOND_LIM"] = DBNull.Value;} else {row["MOND_LIM"] = item.MOND_LIM;}
                        // row["MOND_METH"] = item.MOND_METH;
                        // if (item.MOND_ULIM == null) {row["MOND_ULIM"] = DBNull.Value;} else {row["MOND_ULIM"] = item.MOND_ULIM;}
                        // row["FILE_FSET"] = item.FILE_FSET;

                        // //Non standard LTC  fields
                        // row["ge_source"] = item.ge_source;
                        // row["ge_otherId"] = item.ge_otherId;
                        // row["RND_REF"] = item.RND_REF;
    }


    public async Task<int> CommitAsync(){ 
    
        return await Task.Run (() => {
                                    int rec_updated = _table.Update();
                                    return rec_updated;
                                    });

    }
    
    }

}

