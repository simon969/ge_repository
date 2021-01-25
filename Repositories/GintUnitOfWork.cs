 
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.interfaces;
using ge_repository.OtherDatabase;
using System.Data.SqlClient;
namespace ge_repository.repositories
{
    public class GintUnitOfWork : IGintUnitOfWork
    {   
        private readonly dbConnectDetails _connect;
        
        private readonly SqlConnection _connection;
        
       // public PROJRepository PROJ {get; }
       // public POINTRepository POINT {get; }
      //  public MONGRepository MONG {get;} 
        private MONDRepository _MOND;

       // public MONVRepository MONV {get; }
       // public ERESRepository ERES {get; }

        public GintUnitOfWork(dbConnectDetails connect)
        {
            _connect = connect;
            _connection = new SqlConnection(connect.AsConnectionString());
        }
        public IGintRepository<MOND> MOND => _MOND = _MOND ?? new MONDRepository(_connection);

       public async Task<int> CommitAsync(){

            var t = await _MOND.CommitAsync();

            return t;
       }
        public void Dispose()
        {
            _connection.Close();
        }
    }
}