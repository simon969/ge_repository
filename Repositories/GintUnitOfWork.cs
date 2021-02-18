 
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
        
        private PROJRepository _proj;
        private POINTRepository _point; 
        private MONGRepository _mong; 
        private MONDRepository _mond; 

       // public MONVRepository MONV {get; }
       // public ERESRepository ERES {get; }

        public GintUnitOfWork(dbConnectDetails connect)
        {
            _connect = connect;
            _connection = new SqlConnection(connect.AsConnectionString());
        }
        public IGintRepository<MOND> MOND => _mond = _mond ?? new MONDRepository(_connection, _connect.ProjectId);
        public IGintRepository<MONG> MONG => _mong = _mong ?? new MONGRepository(_connection, _connect.ProjectId);
        public IGintRepository<POINT> POINT => _point = _point ?? new POINTRepository(_connection, _connect.ProjectId);
        public IGintRepository<PROJ> PROJ => _proj = _proj ?? new PROJRepository(_connection, _connect.ProjectId);
       public async Task<int> CommitAsync(){

            var t = await _mond.CommitAsync();

            return t;
       }
       public async Task<int> CommitBulkAsync(){

            var t = await _mond.CommitBulkAsync();

            return t;
       }
        public void Dispose()
        {
            _connection.Close();
        }
    }
}