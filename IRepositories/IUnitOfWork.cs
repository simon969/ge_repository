using System;
using System.Threading.Tasks;

namespace ge_repository.interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDataRepository Data {get;}
        IProjectRepository Project { get; }
        IGroupRepository Group { get; }
        ITransformRepository Transform {get;}
        IUserRepository User {get;}
        IUserOpsRepository UserOps {get;}


        Task<int> CommitAsync();
    }
}