using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.interfaces;


namespace ge_repository.services
{
    public class TransformService : ITransformService
    { 
        private readonly IUnitOfWork _unitOfWork;
        public TransformService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ge_transform>> GetAllTransforms() {
            return await _unitOfWork.Transform
                .GetAllTransformAsync();
        }
         public async Task<IEnumerable<ge_transform>> GetAllWithProject() {
            return await _unitOfWork.Transform
                .GetAllWithProjectAsync();
        }
        public async Task<IEnumerable<ge_transform>> GetAllByProjectId(Guid Id) {
            return await _unitOfWork.Transform
                .GetAllByProjectIdAsync(Id);
        }
        public async Task<IEnumerable<ge_transform>> GetAllByGroupId(Guid Id) {
            return await _unitOfWork.Transform
                .GetAllByGroupIdAsync(Id);
        }
        public async Task<ge_transform> GetTransformById(Guid id) {
            return await _unitOfWork.Transform
                .GetByIdAsync(id);
        }
        public async Task<ge_transform> CreateTransform(ge_transform newTransform) {
            await _unitOfWork.Transform.AddAsync(newTransform);
            await _unitOfWork.CommitAsync();
            return newTransform;
        }

         public async Task UpdateTransform( ge_transform dest, ge_transform srce) {
            dest.name = srce.name;
            dest.styleId = srce.styleId;
            await _unitOfWork.CommitAsync();
        }
         public async Task DeleteTransform(ge_transform transform) {
            _unitOfWork.Transform.Remove(transform);
            await _unitOfWork.CommitAsync();

        }
   
    }
}
