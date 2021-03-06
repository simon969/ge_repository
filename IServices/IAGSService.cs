using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.OtherDatabase;
using ge_repository.AGS;

namespace ge_repository.interfaces {

    public interface IDataAGSService: IDataService {
        Task<IAGSGroupTables> SetLibraryAGSData(Guid Id, string[] groups);
        Task<int> AddLibraryAGSData(IAGSGroupTables tables, Guid? LibraryAGSDataId);
        Task<IAGSGroupTables> GetAGSData(Guid Id, string[] groups);
        Task<ge_data> CreateData (Guid projectId,string UserId,IAGSGroupTables tables, string filename, string description, string format);
    }

   
}
