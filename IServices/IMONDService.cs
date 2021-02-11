using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.OtherDatabase;

namespace ge_repository.interfaces
{
    public interface IMONDService : IGintTableService2<MONG,MOND>
    {
      
      Task <MONG> getMONG(string PointId, string MONG_DIS);
      Task <List<MOND>> CreateMOND (ge_log_file log_file, 
                                        string table,
                                        string round_ref,
                                        DateTime? fromDT,
                                        DateTime? toDT,
                                        Boolean save_MOND);
     
    }


}