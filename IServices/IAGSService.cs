using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.OtherDatabase;
using ge_repository.AGS;

namespace ge_repository.interfaces {

    public interface IDataAGSService: IDataService {

        Task<AGS404GroupTables> GetAGS404GroupTables(Guid Id, string[] groups);
    }
}
