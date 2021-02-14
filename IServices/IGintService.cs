using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.OtherDatabase;

namespace ge_repository.interfaces
{
    public interface IGintTableService<T> where T : class , IGintTable
    {
        Task<PROJ> GetProjectById(int Id);
        Task<POINT> GetPointById(int Id);
        Task<POINT> GetPointByHoleId(string Id);
        Task<List<POINT>> GetAllPointWhere(string where);
        Task<List<T>> GetAllRecords();
        Task<T> GetRecordById(int Id);
        Task<List<T>> GetAllWhere(string where);
        Task CreateRecord(T newRecord);
        Task CreateRange(List<T> records);
        Task UpdateRange(List<T> records, string exist_where);
        Task UpdateRecord(T recordToBeUpdated, T record);
        Task DeleteRecord(T record);
    }
    public interface IGintBaseService 
    {
        Task<PROJ> GetProjectById(int Id);
        Task<POINT> GetPointById(int Id);
        Task<List<POINT>> GetAllPointWhere(string where);
    }

    public interface IGintTableService2<TParent, TChild> : IGintTableService<TChild> where TParent : class where TChild : class, IGintTable
    {
        Task<TParent> GetParentById(int Id);
        Task<List<TParent>> GetParentsByHoleId(string Id);
        Task<List<TParent>> GetParentsWhere(string where);
    }
    

}