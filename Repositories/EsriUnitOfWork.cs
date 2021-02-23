
using System.Threading.Tasks;
using ge_repository.ESRI;
using ge_repository.interfaces;


namespace ge_repository.repositories

{
public class EsriUnitOfWork<TParent, TChild, TGeom> : IEsriUnitOfWork<TParent, TChild, TGeom> 
                                                      where TParent:class, IEsriParent 
                                                      where TChild:class, IEsriChild 
                                                      where TGeom:class, IEsriGeometryWithAttributes 
    {   
        protected EsriConnectionSettings _conn;
        protected EsriClient _client {get;}
        protected EsriAppClient _appClient {get;}
        protected readonly EsriFeatureTable _featureParentTable;
        protected readonly EsriFeatureTable _featureChildTable;
        protected IEsriParentRepository<TParent> _parentData; 
        protected IEsriChildRepository<TChild> _childData; 
        protected IEsriGeomRepository<TGeom> _geomData;
        public IEsriParentRepository<TParent> ParentFeatureData {get {return _parentData;}}
        public IEsriChildRepository<TChild> ChildFeatureData {get {return _childData;}}
        public IEsriGeomRepository<TGeom> GeomFeatureData {get {return _geomData;}}
        public EsriUnitOfWork(EsriConnectionSettings esriConnectionSettings, string parent, string child) {
         _conn = esriConnectionSettings;
         _client = esriConnectionSettings.EsriClient;
         _appClient = esriConnectionSettings.EsriAppClient;

         _featureParentTable = _conn.features.Find(m => m.Name == parent);
         _featureChildTable = _conn.features.Find(m => m.Name == child);
        }
        public int SetConnections(EsriConnectionSettings esriConnectionSettings) {
         _conn = esriConnectionSettings;
         return 1;
        }
        public EsriUnitOfWork (EsriClient Client,
                               EsriAppClient AppClient,
                               EsriFeatureTable FeatureParentTable,
                               EsriFeatureTable FeatureChildTable,
                               IEsriParentRepository<TParent> ParentRepository, 
                               IEsriChildRepository<TChild> ChildRepository, 
                               IEsriGeomRepository<TGeom> GeomRepository) {
            _client = Client;
            _appClient = AppClient;
            _featureChildTable = FeatureParentTable;
            _featureChildTable = FeatureChildTable;
            
            _parentData = ParentRepository;
            _parentData.SetConnections(_client,_appClient,_featureParentTable);

            _childData = ChildRepository;
            _childData.SetConnections(_client,_appClient,_featureChildTable);

            _geomData = GeomRepository;
            _geomData.SetConnections(_client,_appClient,_featureParentTable);
        }

        public Task<int> CommitAsync() {
            return null;
        }
        public Task<int> CommitBulkAsync() {
            return null;
        }
    }
}
