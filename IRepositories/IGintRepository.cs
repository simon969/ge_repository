using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.OtherDatabase;

namespace ge_repository.interfaces
{
    public interface IGintRepository<T> : IADORepository<T> where T : class, IGintTable
    {
        int gINTProjectID();
        Task<int> CommitAsync();
        void set_values(T item, DataRow row);
    }

    public interface IGintTable {
        void set_values(DataRow row);
    }

}