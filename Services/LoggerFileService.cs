using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.interfaces;
using ge_repository.OtherDatabase;

namespace ge_repository.services
{
    public class LoggerFileService : ILoggerFileService
    {
        private readonly ILoggerFileUnitOfWork _unitOfWork;
        public LoggerFileService(ILoggerFileUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ge_log_file> GetById(Guid Id) {
             return await _unitOfWork.LoggerFile.GetByIdAsync(Id);
        }

        public async Task<ge_log_file> GetByIdNoReadings(Guid Id) {
            return await _unitOfWork.LoggerFile.GetByIdWithoutReadingsAsync(Id);
        }

        public async Task<IEnumerable<ge_log_file>> GetAllByProjectIdNoReadings(Guid Id) {
            return await _unitOfWork.LoggerFile.GetAllLoggerFilesWithoutReadingsAsync();
        }

        public async Task  CreateLogFile(ge_log_file newData) {
             await _unitOfWork.LoggerFile.AddAsync (newData);
             await _unitOfWork.CommitAsync();
             
        }

        public async Task UpdateLogFile(ge_log_file dataToBeUpdated, ge_log_file data) {
            var rec = await _unitOfWork.LoggerFile.GetByIdAsync(dataToBeUpdated.Id);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteLogFile(ge_log_file dataToBeDeleted) {
            _unitOfWork.LoggerFile.Remove(dataToBeDeleted);
            await _unitOfWork.CommitAsync();
        }
    }
}
