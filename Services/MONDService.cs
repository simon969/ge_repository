using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.OtherDatabase;
using ge_repository.interfaces;

namespace ge_repository.services
{
    public class MONDService : gINTBaseService, IGintTableService2<MONG, MOND>
    {
        private readonly IGintUnitOfWork _unitOfWork;
        public MONDService(IGintUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public Task<MONG> GetParentById(int Id) {
            return null;
        }
        public Task<IEnumerable<MONG>> GetParentWhere(string where) {
            return null;
        }
        public async Task<IEnumerable<MOND>> GetAllRecords() {
            return await _unitOfWork.MOND.GetAllAsync();
        }

        public async Task<MOND> GetRecordById(int Id) {
            return await _unitOfWork.MOND.GetByIdAsync(Id);
        }
        public async Task<IEnumerable<MOND>> GetAllWhere(string where) {
            return await _unitOfWork.MOND.GetWhereAsync (where);
        }

        public async Task CreateRecord(MOND newRecord) {
            await _unitOfWork.MOND.AddAsync(newRecord);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateRecord(MOND recordToBeUpdated, MOND record) {
            var rec = await _unitOfWork.MOND.GetByIdAsync(recordToBeUpdated.GintRecID);
            
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteRecord(MOND record) {
                _unitOfWork.MOND.Remove(record);
                await _unitOfWork.CommitAsync();
           
        }

    }
}
