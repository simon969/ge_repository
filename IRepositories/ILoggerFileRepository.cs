using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.OtherDatabase;

namespace ge_repository.interfaces
{
    public interface ILoggerFileRepository : IRepository<ge_log_file>
    {
        Task<IEnumerable<ge_log_file>> GetAllLoggerFilesWithoutReadingsAsync();
        Task<IEnumerable<ge_log_file>> GetAllLoggerFilesAsync();
        Task<ge_log_file> GetByIdWithoutReadingsAsync(Guid id);
        Task<ge_log_file> GetByDataIdAsync(Guid id, string table);
        Task <int> UpdateAsync (ge_log_file file, Boolean includereadings);
        Task<int> CommitAsync();
    }
}