using System.Collections.Generic;
using ge_repository.OtherDatabase;

namespace ge_repository.AGS  {

public class AGS404GroupLists {
            
            public List<PROJ> PROJ {get;set;}

            public List<POINT> LOCA {get;set;}

            public List<MOND> MOND {get; set;}
            public List<MONG> MONG {get;set;}          
            public List<ERES> ERES {get;set;}
            public List<SPEC> SPEC {get;set;}
            public List<SAMP> SAMP {get;set;}
           
            public List<TRAN> TRAN {get;set;}

            public List<TYPE> TYPE {get;set;}
            public List<UNIT> UNIT {get;set;}
            public List<DICT> DICT {get;set;}
            public List<ABBR> ABBR {get;set;}

}
public class AGS404GroupTables {
            
            public AGSTable<PROJ> PROJ {get;set;}

            public AGSTable<POINT> LOCA {get;set;}

            public AGSTable<MOND> MOND {get; set;}
            public AGSTable<MONG> MONG {get;set;}          
            public AGSTable<ERES> ERES {get;set;}
            public AGSTable<SPEC> SPEC {get;set;}
            public AGSTable<SAMP> SAMP {get;set;}
           
            public AGSTable<TRAN> TRAN {get;set;}

            public AGSTable<TYPE> TYPE {get;set;}
            public AGSTable<UNIT> UNIT {get;set;}
            public AGSTable<DICT> DICT {get;set;}
            public AGSTable<ABBR> ABBR {get;set;}
            
 
}
}