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
public class GintRepositoryADO<T> : RepositoryADO<T> where T : class {
    
    protected int _gINTProjectID {get;}

    public GintRepositoryADO(string name,string primarykey, SqlConnection conn, int gINTProjectID) 
            : base(name, primarykey, conn, gINTProjectID)
        {
            _gINTProjectID = gINTProjectID;
        }
    public int gINTProjectID() {
        return _gINTProjectID;
    }
}
}


    
    
