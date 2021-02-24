
using ge_repository.ESRI;
using ge_repository.repositories;

namespace ge_repository.LowerThamesCrossing

{
public class LTCUnitOfWork: EsriUnitOfWork<LTM_Survey_Data2, LTM_Survey_Data_Repeat2, LTM_Geometry>
    {   
      
        public LTCUnitOfWork (EsriConnectionSettings connectionSettings,
                                string parent, 
                                string child) :base (connectionSettings, parent, child) {
          
            _parentData =  new RepositoryParentEsri<LTM_Survey_Data2>(_client, _appClient, _featureParentTable);
            _childData =  new RepositoryChildEsri<LTM_Survey_Data_Repeat2>(_client, _appClient, _featureChildTable);
        }
    }

}

