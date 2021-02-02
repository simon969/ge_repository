using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.OtherDatabase;
using ge_repository.interfaces;

namespace ge_repository.services
{
    public class MONDService : gINTBaseService, IGintTableService2<MONG, MOND>
    {
       
        public MONDService(IGintUnitOfWork unitOfWork) : base (unitOfWork)
        { }

        public async Task<MONG> GetParentById(int Id) {
           return await _unitOfWork.MONG.GetByIdAsync(Id);
        }
        public Task<List<MONG>> GetParentsByHoleId(string Id) {
            return _unitOfWork.MONG.FindAsync ($"POINT='{Id}'");
        }

        public Task<List<MONG>> GetParentsWhere(string where) {
            return _unitOfWork.MONG.FindAsync (where);
        }
        public async Task<List<MOND>> GetAllRecords() {
            return await _unitOfWork.MOND.GetAllAsync();
        }

        public async Task<MOND> GetRecordById(int Id) {
            return await _unitOfWork.MOND.GetByIdAsync(Id);
        }
        public async Task<List<MOND>> GetAllWhere(string where) {
            return await _unitOfWork.MOND.FindAsync (where);
        }

        public async Task CreateRecord(MOND newRecord) {
            await _unitOfWork.MOND.AddAsync(newRecord);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateRecord(MOND recordToBeUpdated, MOND record) {
            var rec = await _unitOfWork.MOND.GetByIdAsync(recordToBeUpdated.GintRecID);
            
            await _unitOfWork.CommitAsync();
        }
        public async Task CreateRange(List<MOND> records) {

            
        }
        public async Task UpdateRange(List<MOND> records, string where) {

        }
        public async Task DeleteRecord(MOND record) {
                _unitOfWork.MOND.Remove(record);
                await _unitOfWork.CommitAsync();
           
        }

    }
}
