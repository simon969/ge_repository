using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.interfaces;
using ge_repository.OtherDatabase;
using System.Data.SqlClient;

namespace ge_repository.repositories
{
    public class LogUnitOfWork : ILoggerFileUnitOfWork
    {   
        private readonly dbConnectDetails _connect;
        
        private readonly SqlConnection _connection;
        
        private LoggerFileRepositoryADO _loggerFileRepository;
       
        public LogUnitOfWork(dbConnectDetails connect)
        {
            _connect = connect;
            _connection = new SqlConnection(connect.AsConnectionString());
          
        }
        public ILoggerFileRepository LoggerFile => _loggerFileRepository = _loggerFileRepository ?? new LoggerFileRepositoryADO(_connection);

       public async Task<int> CommitAsync(){

            var t = await LoggerFile.CommitAsync();

            return t;
       }
        public async Task<int> CommitBulkAsync(){

            var t = await LoggerFile.CommitBulkAsync();

            return t;
       }
        public void Dispose()
        {
            _connection.Close();
        }
    }
}
