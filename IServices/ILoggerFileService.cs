using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.OtherDatabase;
using ge_repository.Models;

namespace ge_repository.interfaces
{
    public interface ILoggerFileService
    {
        Task<ge_log_file> GetById(Guid Id); 
        Task<ge_log_file> GetByIdNoReadings(Guid Id);
        Task<ge_log_file> GetByDataId(Guid dataId, string table);
        Task<ge_log_file> GetByDataId(Guid dataId, string table, Boolean includereadings);
        Task<ge_log_file> GetByDataIdNoReadings(Guid dataId, string table);
        Task<IEnumerable<ge_log_file>> GetAllByDataIdNoReadings(Guid Id);
        Task<IEnumerable<ge_log_file>> GetAllByDataId(Guid Id);
        Task<int> CreateFile(ge_log_file newData);
        Task<int> UpdateFile(ge_log_file data, Boolean includereadings);
        Task<int> DeleteFile(ge_log_file data);
        ge_log_file NewFile( ge_search dic, 
                                string[] lines,
                                Guid dataId,
                                Guid templateId);
        Task<ge_log_file> NewFile(Guid Id, 
                                     Guid templateId, 
                                     string table, 
                                     string sheet, 
                                     IDataService _dataService);                          
    }
     public interface IDataLoggerFileService: IDataService {

        ge_data NewData (Guid projectId, string UserId, ge_log_file log_file, string format) ;
     }

}