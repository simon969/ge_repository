using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;

using ge_repository.AGS;
namespace ge_repository.OtherDatabase  {

    public class ABBR : AGSGroup {
    [Display(Name = "GintRecID")] public int GintRecID {get;set;} 
    //	[GintRecID] [int] IDENTITY(1,1) NOT NULL,
  	[Display(Name = "gINTProjectID")] public int gINTProjectID {get;set;} 
    //	[gINTProjectID] [int] NOT NULL,
    [Display(Name = "Heading")] public string ABBR_HDNG {get;set;} 
    //	[ABBR_HDNG] [nvarchar] (255) NOT NULL,
    [Display(Name = "Code")] public string ABBR_CODE {get;set;} 
    // 	[Code] [nvarchar](255) NOT NULL,
    [Display(Name = "Description")] public string ABBR_DESC {get;set;} 
    // 	[Description] [nvarchar](255) NOT NULL,
    [Display(Name = "List")] public string ABBR_LIST {get;set;} 
    // 	[ABBR_List] [nvarchar](255) NOT NULL,
    [Display(Name = "Remark")] public string ABBR_REM {get;set;} 
    // 	[ABBR_REM] [nvarchar](255) NOT NULL,
    [Display(Name = "File Set")] public string FILE_FSET {get;set;} 
    // 	[FILE_FSET] [nvarchar](255) NOT NULL,
    public ABBR() : base ("ABBR") {}
    
    public override int set_values(string[] header, string[] values) {
         try {
            for (int i=0;i<header.Length;i++) {
                if (header[i] == "ABBR_CODE") ABBR_CODE = values[i];
                if (header[i] == "ABBR_DESC") ABBR_DESC = values[i];
                if (header[i] == "ABBR_HDNG") ABBR_HDNG = values[i];
                if (header[i] == "ABBR_LIST") ABBR_LIST = values[i];
                if (header[i] == "ABBR_REM" ) ABBR_REM = values[i];
                if (header[i] == "FILE_FSET") FILE_FSET = values[i];
            }
            return 0;
         } catch {
             return -1;
         }

        
      }
      public override string[] get_values(string[] header, string[] unit, string[] type) {
         try {
            
            string[] ret = new string[header.Length];  
            
            for (int i=0;i<header.Length;i++) {
                if (header[i] == "HEADING") ret[i] = "DATA";
                if (header[i] == "ABBR_CODE" && ABBR_CODE != null) ret[i] = ABBR_CODE;
                if (header[i] == "ABBR_DESC" && ABBR_DESC != null) ret[i] = ABBR_DESC;
                if (header[i] == "ABBR_HDNG" && ABBR_HDNG != null) ret[i] = ABBR_HDNG;
                if (header[i] == "ABBR_LIST" && ABBR_LIST != null) ret[i] = ABBR_LIST;
                if (header[i] == "ABBR_REM" && ABBR_REM != null) ret[i] = ABBR_REM;
                if (header[i] == "FILE_FSET" && FILE_FSET!=null) ret[i] = FILE_FSET;
            }
            
            return ret;

         } catch {
             return null;
         }
      }
    }


}