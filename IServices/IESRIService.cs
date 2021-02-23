using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.ESRI;
using ge_repository.OtherDatabase;
namespace ge_repository.interfaces {


public interface IEsriService<TParent, TChild, TGeom> {


    // Task<string> ReadFeature (string where);
   
    Task<List<MOND>> ReadFeaturesToMOND (string where, int page_size = 250, int[] pages = null, int orderby = Esri.OrderBy.Descending, Boolean save=false); 
    Task<List<TParent>> ReadParentFeature (string where, int page_size = 250, int[] pages = null, int orderby= Esri.OrderBy.Descending); 
    Task<List<TChild>> ReadChildFeature (string where, int page_size = 250, int[] pages = null, int orderby = Esri.OrderBy.Descending); 
}


}