using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
namespace ge_repository.OtherDatabase  { 
            
    public class ge_ags_proj {
        public List<PROJ> PROJ {get;set;}
        public List<MOND> MOND {get; set;}
        public List<MONV> MONV {get; set;}
        public List<MONG> MONG {get;set;}          
        public List<POINT> POINT {get;set;}
        public List<ABBR> ABBR {get;set;}
        public List<TRAN> TRAN {get;set;}
        public List<TYPE> TYPE {get;set;}
        public List<UNIT> UNIT {get;set;}
        public List<DICT> DICT {get;set;}
    }
    
}

