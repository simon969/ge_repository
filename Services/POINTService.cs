using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.OtherDatabase;
using ge_repository.interfaces;

namespace ge_repository.services
{
    public class POINTService : gINTBaseService, IGintTableService<POINT>
    {
      
        public POINTService(IGintUnitOfWork unitOfWork) : base (unitOfWork) {}

        public async Task<List<POINT>> GetAllRecords() {
            return await _unitOfWork.POINT.GetAllAsync();
        }

        public async Task<POINT> GetRecordById(int Id) {
            return await _unitOfWork.POINT.GetByIdAsync(Id);
        }
        public async Task<List<POINT>> GetAllWhere(string where) {
            return await _unitOfWork.POINT.FindAsync (where);
        }

        public async Task CreateRecord(POINT newRecord) {
            await _unitOfWork.POINT.AddAsync(newRecord);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateRecord(POINT recordToBeUpdated, POINT record) {
           // var rec = await _unitOfWork.POINT.GetByIdAsync(recordToBeUpdated.GintRecID);
            
            await _unitOfWork.CommitAsync();
        }
        public async Task CreateRange(List<POINT> records) {

            
        }
        public async Task UpdateRange(List<POINT> records, string where) {

            
        }
        public async Task DeleteRecord(POINT record) {
                _unitOfWork.POINT.Remove(record);
                await _unitOfWork.CommitAsync();
           
        }

    }
}
