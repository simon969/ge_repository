using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
using ge_repository.AGS;
namespace ge_repository.OtherDatabase  {

    public class UNIT : AGSGroup {

    // Create table AGS_FIELD_TYPES
    [Key] 
    [Display(Name = "GintRecID")] public int GintRecID {get;set;} 
    //	[GintRecID] [int] IDENTITY(1,1) NOT NULL,
  	[Display(Name = "gINTProjectID")] public int gINTProjectID {get;set;} 
    //	[gINTProjectID] [int] NOT NULL,
    [Display(Name = "Unit")] public string UNIT_UNIT {get;set;} 
    // 	[Code] [nvarchar](255) NOT NULL,
    [Display(Name = "Description")] public string UNIT_DESC {get;set;} 
    // 	[Code] [nvarchar](255) NOT NULL,
  
    [Display(Name = "Remark")] public string UNIT_REM {get;set;} 
    // 	[Code] [nvarchar](255) NOT NULL,

    [Display(Name = "File Set")] public string FILE_FSET {get;set;} 
    // 	[FILE_FSET] [nvarchar](255) NOT NULL,
   
    public UNIT():base ("UNIT") {}
    public override int set_values(string[] header, string[] values) {
         try {
            for (int i=0;i<header.Length;i++) {
               if (header[i] == "UNIT_DESC") UNIT_DESC = values[i];
               if (header[i] == "UNIT_REM") UNIT_REM = values[i];
               if (header[i] == "UNIT_UNIT") UNIT_UNIT = values[i]; 
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
               if (header[i] == "UNIT_DESC" && UNIT_DESC != null) ret[i] = UNIT_DESC;
               if (header[i] == "UNIT_REM" && UNIT_REM != null) ret[i] = UNIT_REM;
               if (header[i] == "UNIT_UNIT" && UNIT_UNIT != null) ret[i] = UNIT_UNIT;
               if (header[i] == "FILE_FSET" && FILE_FSET != null) ret[i] = FILE_FSET;
            }
            return ret;
         } catch {
             return null;
         }
      }  
    } 
}