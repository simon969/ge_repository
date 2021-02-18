
using System.Threading.Tasks;
using ge_repository.ESRI;
using ge_repository.interfaces;


namespace ge_repository.repositories

{
public class EsriUnitOfWork<TParent, TChild, TGeom> where TParent:IEsriParent where TChild:IEsriChild where TGeom:IEsriGeometryWithAttributes 
    {   
        private EsriClient _client {get;}
        private EsriAppClient _appClient {get;}
        private readonly EsriFeatureTable _featureParentTable;
        private readonly EsriFeatureTable _featureChildTable;
        private IEsriParentRepository<TParent> _parentData; 
        private IEsriChildRepository<TChild> _childData; 
        private IEsriGeomRepository<TGeom> _geomData;
        // public IEsriParentRepository<TParent> ParentFeatureData  => _parentData = _parentData ?? new RepositoryEsri<TParent>(_client, _appClient, _featureParentTable);
        // public IEsriChildRepository<TChild> ChildFeatureData  => _childData = _childData ?? new RepositoryEsri<TChild>(_client, _appClient, _featureChildTable);
        // public IEsriGeomRepository<TGeom> GeomFeatureData  => _geomData = _geomData ?? new RepositoryEsri<TGeom>(_client, _appClient, _featureParentTable);
        public Task<int> CommitAsync() {
            return null;
        }
        public Task<int> CommitBulkAsync() {
            return null;
        }
    }
}
