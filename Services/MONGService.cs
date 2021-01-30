using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.OtherDatabase;
using ge_repository.interfaces;

namespace ge_repository.services
{
    public class MONGService : gINTBaseService, IGintTableService<MONG> 
    {
        private readonly IGintUnitOfWork _unitOfWork;
        public MONGService(IGintUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<MONG>> GetAllRecords() {
            return await _unitOfWork.MONG.GetAllAsync();
         }

        public async Task<MONG> GetRecordById(int Id) {
            return await _unitOfWork.MONG.GetByIdAsync(Id);
        }
        public async Task<IEnumerable<MONG>> GetAllWhere(string where) {
            return await _unitOfWork.MONG.GetWhereAsync (where);
        }
        public async Task CreateRecord(MONG newRecord) {
            await _unitOfWork.MONG.AddAsync(newRecord);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateRecord(MONG recordToBeUpdated, MONG record) {
            var rec = await _unitOfWork.MONG.GetByIdAsync(recordToBeUpdated.GintRecID);
            
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteRecord(MONG record) {
                _unitOfWork.MONG.Remove(record);
                await _unitOfWork.CommitAsync();
           
        }

    }
}
