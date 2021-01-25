using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;

namespace ge_repository.interfaces
{
    public interface IGintService<T>
    {
        Task<IEnumerable<T>> GetAllRecords();
        Task<T> GetRecordById(int Id);
        Task<IEnumerable<T>> GetAllWhere(string where);
        Task CreateRecord(T newRecord);
        Task UpdateRecord(T recordToBeUpdated, T record);
        Task DeleteRecord(T record);
    }
}