using System;
using System.Threading.Tasks;
using ge_repository.OtherDatabase;

namespace ge_repository.interfaces
{
    public interface IGintUnitOfWork : IDisposable
    {
        IGintRepository<PROJ> PROJ {get; }
        IGintRepository<POINT> POINT {get; }
        IGintRepository<MONG> MONG {get;} 
        IGintRepository<MOND> MOND {get;}
        
        // IGintRepository<MONV> MONV {get; }
        // IGintRepository<ERES> ERES {get; }

       Task<int> CommitAsync();
       Task<int> CommitBulkAsync();
    }
}