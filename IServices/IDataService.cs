using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.OtherDatabase;
using ge_repository.ESRI;

namespace ge_repository.interfaces
{
    public interface IDataService
    {
        Task<IEnumerable<ge_data>> GetAllWithProject();
        Task<ge_data> GetDataById(Guid Id);
        Task<ge_data> GetDataByIdWithFile(Guid Id);
        Task<ge_data> GetDataByIdWithAll(Guid id);
        Task<OtherDbConnections> GetOtherDbConnectionsByDataId(Guid Id);
        Task<EsriConnectionSettings> GetEsriConnectionSettingsByProjectId(Guid Id);
        Task SetProcessFlag(Guid Id, int value);
        Task SetProcessFlagAddEvents(Guid Id, int value, string add);
        Task<int> GetProcessFlag(Guid Id);
        Task<T> GetFileAsClass<T>(Guid Id);
        Task<string[]> GetFileAsLines(Guid Id);
        Task<MemoryStream> GetFileAsMemoryStream(Guid Id); 
        Task<string> GetFileAsString (Guid Id, bool removeBOM);
        Task<IEnumerable<ge_data>> GetDataByProjectId(Guid Id);
        Task<ge_data> CreateData(ge_data newData);
        Task UpdateData(ge_data dataToBeUpdated, ge_data data);
        Task UpdateData(ge_data data);
        Task DeleteData(ge_data data);
        Boolean DataExists (Guid Id);
        ge_data NewData (Guid projectId, string UserId);
      

        
    }

}