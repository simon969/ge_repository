
using System;
using System.Linq;

using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.ESRI;

namespace ge_repository.interfaces {
public interface IEsriRepository <TEntity> where TEntity: class{
 List<TEntity> list {get;set;}
Task<string[]> getFeatures(string where, int page_size, int[] pages, int orderby);
   
}

public interface IEsriParentRepository <TParent> where TParent : IEsriParent {
    List<TParent> list {get;set;}

    Task<string[]> getFeatures(string where, int page_size, int[] pages, int orderby);
   
}


public interface IEsriChildRepository <TChild> where TChild: IEsriChild {
    List<TChild> list {get;set;}
    Task<string[]> getFeatures(string where, int page_size, int[] pages, int orderby);

}

public interface IEsriGeomRepository <TGeom> where TGeom: IEsriGeometryWithAttributes {
    List<TGeom> list {get;set;}
    Task<string[]> getFeatures(string where, int page_size, int[] pages, int orderby);

}
public interface IEsriRepository<TParent, TChild> where TParent : class where TChild : class {
Task<string[]> getFeatures(string where, int page_size, int[] pages, int orderby);

}

}
