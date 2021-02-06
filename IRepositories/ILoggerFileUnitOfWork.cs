using System;
using System.Threading.Tasks;
using ge_repository.OtherDatabase;

namespace ge_repository.interfaces
{
    public interface ILoggerFileUnitOfWork : IDisposable
    {
       ILoggerFileRepository LoggerFile {get;}
       Task<int> CommitAsync();
       Task<int> CommitBulkAsync();
    }
}