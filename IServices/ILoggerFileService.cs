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
        Task<int> CreateLogFile(ge_log_file newData);
        Task<int> UpdateLogFile(ge_log_file data, Boolean includereadings);
        Task<int> DeleteLogFile(ge_log_file data);
        ge_log_file NewLogFile( ge_search dic, 
                                string[] lines,
                                Guid dataId,
                                Guid templateId);
        Task<ge_log_file> NewLogFile(Guid Id, 
                                     Guid templateId, 
                                     string table, 
                                     string sheet, 
                                     IDataService _dataService);                          
    }
     public interface IDataLoggerFileService: IDataService {

        ge_data NewData (Guid projectId, string UserId, ge_log_file log_file, string format) ;
     }

}