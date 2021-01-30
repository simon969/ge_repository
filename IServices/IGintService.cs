using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.OtherDatabase;

namespace ge_repository.interfaces
{
    public interface IGintTableService<T> where T : class 
    {
        Task<PROJ> GetProjectById(int Id);
        Task<POINT> GetPointById(int Id);
        Task<IEnumerable<POINT>> GetAllPointWhere(string where);
        Task<IEnumerable<T>> GetAllRecords();
        Task<T> GetRecordById(int Id);
        Task<IEnumerable<T>> GetAllWhere(string where);
        Task CreateRecord(T newRecord);
        Task UpdateRecord(T recordToBeUpdated, T record);
        Task DeleteRecord(T record);
    }
    public interface IGintBaseService 
    {
        Task<PROJ> GetProjectById(int Id);
        Task<POINT> GetPointById(int Id);
        Task<IEnumerable<POINT>> GetAllPointWhere(string where);
    }

    public interface IGintTableService2<TParent, TChild> : IGintTableService<TChild> where TParent : class where TChild : class
    {
        Task<TParent> GetParentById(int Id);
        Task<IEnumerable<TParent>> GetParentWhere(string where);
    }
    

}