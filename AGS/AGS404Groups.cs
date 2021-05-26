using System.Collections.Generic;
using ge_repository.OtherDatabase;

namespace ge_repository.AGS  {

public class AGS404GroupLists : IAGSGroupLists {
            
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

public interface IAGSGroupTables {
   
    AGSTable<PROJ> AddTable(List<PROJ> list);
    AGSTable<POINT> AddTable(List<POINT> list);
    AGSTable<ERES> AddTable(List<ERES> list);
    AGSTable<MOND> AddTable(List<MOND> list);
    AGSTable<MONG> AddTable(List<MONG> list);
    AGSTable<SAMP> AddTable(List<SAMP> list);
    AGSTable<TRAN> AddTable(List<TRAN> list);
    AGSTable<UNIT> AddTable(List<UNIT> list);
    AGSTable<ABBR> AddTable(List<ABBR> list);
    AGSTable<TYPE> AddTable(List<TYPE> list);
    AGSTable<DICT> AddTable(List<DICT> list);
    
    // int AppendTable<T> (List<T> list);
    
    string ToString();
}
public interface IAGSGroupLists {


}
public class AGS404GroupTables : IAGSGroupTables {
            
            // 
            
            public AGSTable<PROJ> PROJ {get;set;}
            public AGSTable<PROJ> AddTable(List<PROJ> list) {
                PROJ = new AGSTable<PROJ>();
                PROJ.headers = new List<string>() {"HEADING","PROJ_ID","PROJ_NAME","PROJ_LOC","PROJ_CLNT","PROJ_CONT","PROJ_ENG","PROJ_MEMO","FILE_FSET"};
                PROJ.units = new List<string>(){"UNIT","","","","","","","",""};
                PROJ.types = new List<string>(){"TYPE","ID","X","X","X","X","X","X","X"};
                PROJ.values = list;
                return PROJ;
            }

           // public int AppendTable<T>(List<AGS> list) {

             //    if (typeof(T) == typeof(PROJ)) {PROJ.values.AddRange(list); return P;}

            // }

            public AGSTable<POINT> LOCA {get;set;}
            public AGSTable<POINT> AddTable(List<POINT> list) {
                // "GROUP","LOCA"
                // "HEADING","LOCA_ID","LOCA_TYPE","LOCA_STAT","LOCA_NATE","LOCA_NATN","LOCA_GREF","LOCA_GL","LOCA_REM","LOCA_FDEP","LOCA_STAR","LOCA_PURP","LOCA_TERM","LOCA_ENDD","LOCA_LETT","LOCA_LOCX","LOCA_LOCY","LOCA_LOCZ","LOCA_LREF","LOCA_DATM","LOCA_ETRV","LOCA_NTRV","LOCA_LTRV","LOCA_XTRL","LOCA_YTRL","LOCA_ZTRL","LOCA_LAT","LOCA_LON","LOCA_ELAT","LOCA_ELON","LOCA_LLZ","LOCA_LOCM","LOCA_LOCA","LOCA_CLST","LOCA_ALID","LOCA_OFFS","LOCA_CNGE","LOCA_TRAN","FILE_FSET","LOCA_NATD","LOCA_ORID","LOCA_ORJO","LOCA_ORCO","LOCA_CHKG","LOCA_APPG","LOCA_PNCT"
                // "UNIT","","","","m","m","","m","","m","yyyy-mm-dd","","","yyyy-mm-dd","","m","m","m","","","m","m","m","m","m","m","","","","","","","","","","","","","","","","","","","",""
                // "TYPE","ID","PA","PA","2DP","2DP","PA","2DP","X","2DP","DT","X","X","DT","X","2DP","2DP","2DP","X","X","2DP","2DP","2DP","2DP","2DP","2DP","DMS","DMS","DMS","DMS","X","X","X","X","X","2DP","X","X","X","PA","U","X","X","X","X","X"

                LOCA = new AGSTable<POINT>();
                LOCA.headers = new List<string>() {"HEADING","LOCA_ID","LOCA_TYPE","LOCA_STAT","LOCA_NATE","LOCA_NATN","LOCA_GREF","LOCA_GL","LOCA_REM","LOCA_FDEP","LOCA_STAR","LOCA_PURP","LOCA_TERM","LOCA_ENDD","LOCA_LETT","LOCA_LOCX","LOCA_LOCY","LOCA_LOCZ","LOCA_LREF","LOCA_DATM","LOCA_ETRV","LOCA_NTRV","LOCA_LTRV","LOCA_XTRL","LOCA_YTRL","LOCA_ZTRL","LOCA_LAT","LOCA_LON","LOCA_ELAT","LOCA_ELON","LOCA_LLZ","LOCA_LOCM","LOCA_LOCA","LOCA_CLST","LOCA_ALID","LOCA_OFFS","LOCA_CNGE","LOCA_TRAN","FILE_FSET","LOCA_NATD","LOCA_ORID","LOCA_ORJO","LOCA_ORCO","LOCA_CHKG","LOCA_APPG","LOCA_PNCT"};
                LOCA.units = new List<string>() {"UNIT","","","","m","m","","m","","m","yyyy-mm-dd","","","yyyy-mm-dd","","m","m","m","","","m","m","m","m","m","m","","","","","","","","","","","","","","","","","","","",""};
                LOCA.types = new List<string>() {"TYPE","ID","PA","PA","2DP","2DP","PA","2DP","X","2DP","DT","X","X","DT","X","2DP","2DP","2DP","X","X","2DP","2DP","2DP","2DP","2DP","2DP","DMS","DMS","DMS","DMS","X","X","X","X","X","2DP","X","X","X","PA","U","X","X","X","X","X"};
                LOCA.values = list;
                return LOCA;
            }
            public AGSTable<MOND> MOND {get; set;}
            public AGSTable<MOND> AddTable(List<MOND> list) {
                MOND = new AGSTable<MOND>();
                MOND.headers = new List<string>() {"HEADING","LOCA_ID","MONG_ID","MONG_DIS","MOND_DTIM","MOND_TYPE","MOND_REF","MOND_INST","MOND_RDNG","MOND_UNIT","MOND_METH","MOND_LIM","MOND_ULIM","MOND_NAME","MOND_CRED","MOND_CONT","MOND_REM","FILE_FSET"};
                MOND.units = new List<string>() {"UNIT","","","m","yyyy-mm-ddThh:mm:ss","m","","","m","","","","","","","","",""};
                MOND.types = new List<string>() {"TYPE","ID","X","2DP","DT","PA","X","X","XN","PU","X","U","U","X","X","X","X","X"};                
                MOND.values = list;
                return MOND;
            }
            public AGSTable<MONG> MONG {get;set;}    
              public AGSTable<MONG> AddTable(List<MONG> list) {
              
                MONG.values = list;
                return MONG;
            }

            public AGSTable<ERES> ERES {get;set;}
            public AGSTable<ERES> AddTable(List<ERES> list) {
                ERES = new AGSTable<ERES>();
                ERES.headers = new List<string>() {"HEADING","LOCA_ID","SAMP_TOP","SAMP_REF","SAMP_TYPE","SAMP_ID","SPEC_REF","SPEC_DPTH","ERES_CODE","ERES_METH","ERES_MATX","ERES_RTYP","ERES_TESN","ERES_NAME","ERES_TNAM","ERES_RVAL","ERES_RUNI","ERES_RTXT","ERES_RTCD","ERES_RRES","ERES_DETF","ERES_ORG","ERES_IQLF","ERES_LQLF","ERES_RDLM","ERES_MDLM","ERES_QLM","ERES_DUNI","ERES_TICP","ERES_TICT","ERES_RDAT","ERES_SGRP","SPEC_PREP","SPEC_DESC","ERES_DTIM","ERES_TEST","ERES_TORD","ERES_LOCN","ERES_BAS","ERES_DIL","ERES_LMTH","ERES_LDTM","ERES_IREF","ERES_SIZE","ERES_PERP","ERES_REM","ERES_LAB","ERES_CRED","TEST_STAT","FILE_FSET"};
                ERES.units = new List<string>() {"UNIT","","m","","","","","m","","","","","","","","","","","","","","","","","","","","","%","s","yyyy-mm-dd","","","","yyyy-mm-ddThh:mm:ss","","","","","","","yyyy-mm-ddThh:mm:ss","","mm","%","","","","",""};
                ERES.types = new List<string>() {"TYPE","ID","2DP","X","PA","ID","X","2DP","PA","X","PA","PA","X","X","X","2DP","PA","X","PA","YN","YN","YN","X","X","2DP","2DP","2DP","PA","0DP","0DP","DT","X","X","X","DT","X","X","PA","PA","0DP","X","DT","X","0DP","1DP","X","X","X","X","X"};
                ERES.values = list;
                return ERES;
            }
            public AGSTable<SPEC> SPEC {get;set;}
            public AGSTable<SAMP> SAMP {get;set;}
            public AGSTable<SAMP> AddTable(List<SAMP> list) {
                SAMP = new AGSTable<SAMP>();
                SAMP.headers = new List<string>() {"HEADING","LOCA_ID","SAMP_TOP","SAMP_REF","SAMP_TYPE","SAMP_ID","SAMP_BASE","SAMP_DTIM","SAMP_UBLO","SAMP_CONT","SAMP_PREP","SAMP_SDIA","SAMP_WDEP","SAMP_RECV","SAMP_TECH","SAMP_MATX","SAMP_TYPC","SAMP_WHO","SAMP_WHY","SAMP_REM","SAMP_DESC","SAMP_DESD","SAMP_LOG","SAMP_COND","SAMP_CLSS","SAMP_BAR","SAMP_TEMP","SAMP_PRES","SAMP_FLOW","SAMP_ETIM","SAMP_DURN","SAMP_CAPT","SAMP_LINK","GEOL_STAT","FILE_FSET","SAMP_RECL"};
                SAMP.units = new List<string>() {"UNIT","","m","","","","m","yyyy-mm-ddThh:mm:ss","","","","mm","m","%","","","","","","","","yyyy-mm-dd","","","","bar","DegC","bar","l/min","yyyy-mm-ddThh:mm:ss","hh:mm:ss","","","","","mm"};
                SAMP.types = new List<string>() {"TYPE","ID","2DP","X","PA","ID","2DP","DT","0DP","X","X","0DP","X","0DP","X","X","X","X","X","X","X","DT","X","X","X","1DP","0DP","1DP","1DP","DT","T","X","X","X","X","0DP"};
                SAMP.values = list;
                return SAMP;
            }
            public AGSTable<TRAN> TRAN {get;set;}
            public AGSTable<TRAN> AddTable(List<TRAN> list) {
                TRAN = new AGSTable<TRAN>();
                TRAN.headers = new List<string>() {"HEADING","TRAN_ISNO","TRAN_DATE","TRAN_PROD","TRAN_STAT","TRAN_DESC","TRAN_AGS","TRAN_RECV","TRAN_DLIM","TRAN_RCON","TRAN_REM","FILE_FSET"};
                TRAN.units = new List<string>() {"UNIT","","yyyy-mm-dd","","","","","","","","",""};
                TRAN.types = new List<string>() {"TYPE","X","DT","X","X","X","X","X","X","X","X","X"};
                TRAN.values = list;
                return TRAN;
            }
            public AGSTable<TYPE> TYPE {get;set;}

            public AGSTable<TYPE> AddTable(List<TYPE> list) {
                //  "GROUP","TYPE"
                // "HEADING","TYPE_TYPE","TYPE_DESC","FILE_FSET"
                // "UNIT","","",""
                // "TYPE","X","X","X"
                if (TYPE == null) {
                    TYPE = new AGSTable<TYPE>();
                    TYPE.headers = new List<string>() {"HEADING","TYPE_TYPE","TYPE_DESC","FILE_FSET"};
                    TYPE.units = new List<string>() {"UNIT","","",""};
                    TYPE.types = new List<string>() {"TYPE","X","X","X"};
                    TYPE.values = list;
                } else {
                    TYPE.values.AddRange (list);
                }
                                
                return TYPE;
            }
            public AGSTable<UNIT> UNIT {get;set;}
            public AGSTable<UNIT> AddTable(List<UNIT> list) {
                
                if (UNIT == null) {
                    UNIT = new AGSTable<UNIT>();
                    UNIT.headers = new List<string>() {"HEADING","UNIT_UNIT","UNIT_DESC","UNIT_REM","FILE_FSET"};
                    UNIT.units = new List<string>() {"UNIT","","","",""};
                    UNIT.types = new List<string>() {"TYPE","X","X","X","X"};
                    UNIT.values = list;
                } else {
                    UNIT.values.AddRange (list);
                }
                                
                return UNIT;
            }
            public AGSTable<DICT> DICT {get;set;}
            public AGSTable<DICT> AddTable(List<DICT> list) {
            // "HEADING","DICT_TYPE","DICT_GRP","DICT_HDNG","DICT_STAT","DICT_DTYP","DICT_DESC","DICT_UNIT","DICT_EXMP","DICT_PGRP","DICT_REM","FILE_FSET"
            // "UNIT","","","","","","","","","","",""
            // "TYPE","PA","X","X","PA","PT","X","PU","X","X","X","X"

             if (DICT == null) {
                    DICT = new AGSTable<DICT>();
                    DICT.headers = new List<string>() {"HEADING","DICT_TYPE","DICT_GRP","DICT_HDNG","DICT_STAT","DICT_DTYP","DICT_DESC","DICT_UNIT","DICT_EXMP","DICT_PGRP","DICT_REM","FILE_FSET"};
                    DICT.units = new List<string>() {"UNIT","","","",""};
                    DICT.types = new List<string>() {"TYPE","PA","X","X","PA","PT","X","PU","X","X","X","X"};
                    DICT.values = list;
                } else {
                    DICT.values.AddRange (list);
                }
                                
                return DICT;
            }

            public AGSTable<ABBR> ABBR {get;set;}
            public AGSTable<ABBR> AddTable(List<ABBR> list) {
                
                if (ABBR==null) {
                    ABBR = new AGSTable<ABBR>();
                    ABBR.headers = new List<string>() {"HEADING","ABBR_HDNG","ABBR_CODE","ABBR_DESC","ABBR_LIST","ABBR_REM","FILE_FSET"};
                    ABBR.units = new List<string>() {"UNIT","","","","","",""};
                    ABBR.types = new List<string>() {"TYPE","X","X","X","X","X","X"};
                    ABBR.values = list;
                } else {
                    ABBR.values.AddRange(list);
                }

                return ABBR;
            }
            
 
}
}