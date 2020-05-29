using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
namespace ge_repository.OtherDatabase  {

    public class PROJ {


// CREATE TABLE [dbo].[PROJECT](
    [Key] [Display(Name = "GintRecID")] public int GintRecId {get;set;} 
// 	[GintRecID] [int] IDENTITY(1,1) NOT NULL,
 	[Display(Name = "gINTProjectID")] public int gINTProjectID {get;set;} 
// 	[gINTProjectID] [int] NOT NULL,
    [Display(Name = "Project identifier")] public string PROJ_ID {get;set;} 
// 	[PROJ_ID] [nvarchar](255) NOT NULL,
    [Display(Name = "Project name")] public string PROJ_NAME {get;set;} 
// 	[PROJ_NAME] [nvarchar](255) NULL,
    [Display(Name = "Project location")] public string PROJ_LOC {get;set;} 
// 	[PROJ_LOC] [nvarchar](255) NULL,
    [Display(Name = "Client name")] public string PROJ_CLNT {get;set;} 
// 	[PROJ_CLNT] [nvarchar](255) NULL,
    [Display(Name = "Contractor name")] public string PROJ_CONT {get;set;} 
// 	[PROJ_CONT] [nvarchar](255) NULL,
    [Display(Name = "Project Engineer")] public string PROJ_ENG {get;set;} 
// 	[PROJ_ENG] [nvarchar](255) NULL,
    [Display(Name = "General project comments")] public string PROJ_MEMO {get;set;} 
// 	[PROJ_MEMO] [ntext] NULL,
    [Display(Name = "Associated File")] public string FILE_FSET {get;set;} 
// 	[FILE_FSET] [nvarchar](255) NULL,
    [Display(Name = "Project datum")] public string PROJ_GRID {get;set;} 
// 	[PROJ_GRID] [nvarchar](255) NULL,
// 	[Depth_Log_Page] [real] NULL,
// 	[Site Map on Log Radius] [float] NULL,

    }
}
