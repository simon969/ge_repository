 
 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
namespace ge_repository.OtherDatabase  {

 
 // Data structure for ABBR table in gINT Library file (*.glb) records
    public class gINTLib_ABBR {
    [Display(Name = "GintRecID")] public int GintRecID {get;set;} 
    //	[GintRecID] [int] IDENTITY(1,1) NOT NULL,
    [Display(Name = "Code")] public string Code {get;set;} 
    // 	[Code] [nvarchar](255) NOT NULL,
    [Display(Name = "Description")] public string Description {get;set;} 
    // 	[Code] [nvarchar](255) NOT NULL,
    [Display(Name = "ABBR List")] public string ABBR_List {get;set;} 
    // 	[ABBR_List] [nvarchar](255) NOT NULL,
    [Display(Name = "ABBR REM")] public string ABBR_REM {get;set;} 
    // 	[ABBR_REM] [nvarchar](255) NOT NULL,
    [Display(Name = "File Set")] public string FILE_FSET {get;set;} 
    // 	[FILE_FSET] [nvarchar](255) NOT NULL,
    }
}
