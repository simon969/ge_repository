using System;
using System.Threading.Tasks;
using ge_repository.interfaces;

namespace ge_repository.ESRI {

public class ge_esri_Client {


    protected Guid _projectId {get;}
    protected Guid[] _projectIds {get;}
    protected IProjectService _projectService {get;}

    public ge_esri_Client (Guid ProjectId, IProjectService ProjectService) {
        _projectId = ProjectId;
        _projectService = ProjectService;
    }
    public ge_esri_Client (Guid[] ProjectIds, IProjectService ProjectService) {
        _projectIds= ProjectIds;
        _projectService = ProjectService;
    }

    public Task<int> start() {

        return null;
        
    }

    
}

}

namespace ge_repository.interfaces {
     
public interface IEsriClient {

    Task<int> start();
    Task<int> readFeature(); 
    Task<int> readFeature(int page_size, int page); 
    Task<int> writeFeature ();

        
}

}