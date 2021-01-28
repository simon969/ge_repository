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
    public override int setValues(string[] header, string[] values) {
         try {
            for (int i=0;i<header.Length;i++) {
               if (header[i] == "FILE_FSET") FILE_FSET = values[i];
               if (header[i] == "UNIT_DESC") UNIT_DESC = values[i];
               if (header[i] == "UNIT_REM") UNIT_REM = values[i];
               if (header[i] == "UNIT_UNIT") UNIT_UNIT = values[i];
            }
         } catch {
             return -1;
         }
        return 0;
      }
    } 
}