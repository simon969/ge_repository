using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
namespace ge_repository.OtherDatabase  {

    public class UNIT {

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
    }

}