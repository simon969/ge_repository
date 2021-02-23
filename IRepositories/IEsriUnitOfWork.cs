
using System;
using System.Threading.Tasks;
using ge_repository.interfaces;
using ge_repository.ESRI;

public interface IEsriUnitOfWork<TParent, TChild, TGeom>    where TParent:class, IEsriParent 
                                                            where TChild:class, IEsriChild 
                                                            where TGeom:class, IEsriGeometryWithAttributes {

    IEsriParentRepository<TParent> ParentFeatureData {get;}
    IEsriChildRepository<TChild> ChildFeatureData {get;} 
    IEsriGeomRepository<TGeom> GeomFeatureData {get;}

    int SetConnections(EsriConnectionSettings esriConnectionSettings);
    Task<int> CommitAsync();
    Task<int> CommitBulkAsync();


    
}