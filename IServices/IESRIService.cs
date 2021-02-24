using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.ESRI;
using ge_repository.OtherDatabase;
namespace ge_repository.interfaces {


public interface IEsriService<TParent, TChild, TGeom> {


    Task<List<MOND>> ReadFeaturesToMONDList (string where, int page_size = 250, int[] pages = null, int orderby = Esri.OrderBy.Descending, Boolean save=false); 
    Task<List<TParent>> ReadParentFeatureToList (string where, int page_size = 250, int[] pages = null, int orderby= Esri.OrderBy.Descending); 
    Task<string> ReadParentFeatureToJson (string where, int page_size = 250, int[] pages = null, int orderby= Esri.OrderBy.Descending); 
    Task<string> ReadParentFeatureToXml (string where, int page_size = 250, int[] pages = null, int orderby= Esri.OrderBy.Descending); 
    Task<List<TChild>> ReadChildFeatureToList (string where, int page_size = 250, int[] pages = null, int orderby = Esri.OrderBy.Descending); 
    Task<string> ReadChildFeatureToJson (string where, int page_size = 250, int[] pages = null, int orderby= Esri.OrderBy.Descending); 
    Task<string> ReadChildFeatureToXml (string where, int page_size = 250, int[] pages = null, int orderby= Esri.OrderBy.Descending); 
    Task<List<TGeom>> ReadGeomFeatureToList (string where, int page_size = 250, int[] pages = null, int orderby = Esri.OrderBy.Descending); 
    Task<string> ReadGeomFeatureToJson (string where, int page_size = 250, int[] pages = null, int orderby= Esri.OrderBy.Descending); 
    Task<string> ReadGeomFeatureToXml (string where, int page_size = 250, int[] pages = null, int orderby= Esri.OrderBy.Descending); 
}


}