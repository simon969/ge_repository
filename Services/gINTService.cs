using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.OtherDatabase;
using ge_repository.interfaces;

namespace ge_repository.services
{

    public class gINTBaseService :IGintBaseService
    {

        protected  readonly IGintUnitOfWork _unitOfWork;
       
        public gINTBaseService(IGintUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }
        
        public Task<PROJ> GetProjectById(int Id) {
            return null;
        }
        public async Task<POINT> GetPointByHoleId(string Id) {

            return await _unitOfWork.POINT.FindSingleAsync($"PointID='{Id}'");

        }
       public async Task<POINT> GetPointById(int Id) {

          return await _unitOfWork.POINT.FindSingleAsync($"GintRecID={Id}");
        
        }
        public async Task<List<POINT>> GetAllPointWhere(string where) {
           
           return await _unitOfWork.POINT.FindAsync(where);
        
        }

    }

}

    