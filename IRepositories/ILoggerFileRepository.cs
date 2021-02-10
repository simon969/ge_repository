using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.OtherDatabase;

namespace ge_repository.interfaces
{
    public interface ILoggerFileRepository : IRepository<ge_log_file>
    {
        Task<IEnumerable<ge_log_file>> GetAllLoggerFilesNoReadingsAsync();
        Task<IEnumerable<ge_log_file>> GetAllLoggerFilesAsync();
        Task<ge_log_file> GetByIdNoReadingsAsync(Guid id);
        Task<ge_log_file> GetByDataIdAsync(Guid id, string table);
        Task<ge_log_file> GetByDataIdNoReadingsAsync(Guid id, string table);
         Task<IEnumerable<ge_log_file>> GetAllByDataIdAsync(Guid Id);
        Task<IEnumerable<ge_log_file>> GetAllByDataIdNoReadingsAsync(Guid Id);
        Task <int> UpdateAsync (ge_log_file file, Boolean includereadings);
        Task<int> CommitAsync();
        Task<int> CommitBulkAsync(); 
        
    }

   
}