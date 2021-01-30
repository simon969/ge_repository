using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.OtherDatabase;
using ge_repository.interfaces;

namespace ge_repository.services
{
    public class gINTBaseService : gINTProjectService,  IGintBaseService
    {

        public Task<POINT> GetPointById(int Id) {
            return null;
        }
        public Task<IEnumerable<POINT>> GetAllPointWhere(string where) {
            return null;
        }


    }


    public class gINTProjectService 
    {
        public Task<PROJ> GetProjectById(int Id) {
            return null;
        }
      

    }

}

    