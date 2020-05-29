using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
namespace ge_repository.OtherDatabase  {

    public class ABBR {
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
    }


}