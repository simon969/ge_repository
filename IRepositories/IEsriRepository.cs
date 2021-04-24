
using System;
using System.Linq;

using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.ESRI;

namespace ge_repository.interfaces {
public interface IEsriRepository <TEntity> where TEntity: class{
List<TEntity> list {get;set;}
Task<string[]> getFeatures(string where, int page_size, int[] pages, int orderby);
void SetConnections (IEsriOrgClient Client, IEsriAppClient AppClient, EsriFeatureTable FeatureTable);

}

public interface IEsriParentRepository <TParent> : IEsriRepository<TParent> where TParent : class, IEsriParent  {
    List<TParent> list {get;set;}

    Task<string[]> getFeatures(string where, int page_size, int[] pages, int orderby);
   
}


public interface IEsriChildRepository <TChild> : IEsriRepository<TChild>  where TChild: class, IEsriChild {
    List<TChild> list {get;set;}
    Task<string[]> getFeatures(string where, int page_size, int[] pages, int orderby);

}

public interface IEsriGeomRepository <TGeom>  : IEsriRepository<TGeom> where TGeom: class, IEsriGeometryWithAttributes {
    List<TGeom> list {get;set;}
    Task<string[]> getFeatures(string where, int page_size, int[] pages, int orderby);

}

}
