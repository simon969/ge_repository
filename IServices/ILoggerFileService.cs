using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.OtherDatabase;

namespace ge_repository.interfaces
{
    public interface ILoggerFileService
    {
        Task<ge_log_file> GetById(Guid Id);
        Task<ge_log_file> GetByDataId(Guid Id, string table);
        Task<ge_log_file> GetByIdNoReadings(Guid Id);
        Task<IEnumerable<ge_log_file>> GetAllByProjectIdNoReadings(Guid Id);
        Task<int> CreateLogFile(ge_log_file newData);
        Task<int> UpdateLogFile(ge_log_file data, Boolean includereadings);
        Task<int> DeleteLogFile(ge_log_file data);
        ge_log_file NewLogFile(ge_search dic, 
                                  string[] lines,
                                  Guid dataId,
                                  Guid templateId);
    }
}