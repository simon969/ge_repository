
using System;
using System.Threading.Tasks;
using ge_repository.interfaces;
using ge_repository.ESRI;

public interface IEsriUnitOfWork<TParent, TChild, TGeom> where TParent:IEsriParent where TChild:IEsriChild where TGeom:IEsriGeometryWithAttributes {

    IEsriParentRepository<TParent> ParentFeatureData {get;}
    IEsriChildRepository<TChild> ChildFeatureData {get;} 
    IEsriGeomRepository<TGeom> GeomFeatureData {get;}
    Task<int> CommitAsync();
    Task<int> CommitBulkAsync();


    
}