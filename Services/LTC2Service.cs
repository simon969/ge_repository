using Newtonsoft.Json;

using ge_repository.DAL;
using ge_repository.Extensions;
using ge_repository.interfaces;
using ge_repository.OtherDatabase;
using ge_repository.Models;
using ge_repository.ESRI;
using ge_repository.services;

namespace ge_repository.LowerThamesCrossing {

    public class LTCEsriService : ESRIService<LTM_Survey_Data2, LTM_Survey_Data_Repeat2, LTM_Geometry> , ILTCEsriService {

    public LTCEsriService(IEsriUnitOfWork<LTM_Survey_Data2, LTM_Survey_Data_Repeat2, LTM_Geometry>  unitOfWork):base (unitOfWork) {
    }
    }

    public interface ILTCEsriService: IEsriService<LTM_Survey_Data2, LTM_Survey_Data_Repeat2, LTM_Geometry> {
   
    }
}

