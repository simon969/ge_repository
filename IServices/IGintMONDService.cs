using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.OtherDatabase;

namespace ge_repository.interfaces
{
    public interface IGintMONDService : IGintTableService<MOND>
    {
      
      Task <MOND> getMONG(string PointId, string MONG_DIS);
      
    }


}