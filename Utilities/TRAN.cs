using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
using ge_repository.AGS;

namespace ge_repository.OtherDatabase  {

    public class TRAN : AGSGroup {
    // CREATE TABLE [dbo].[TRAN](
    [Display(Name = "GintRecID")] public int GintRecID {get;set;} 
    //	[GintRecID] [int] IDENTITY(1,1) NOT NULL,
  	[Display(Name = "gINTProjectID")] public int gINTProjectID {get;set;} 
    //	[gINTProjectID] [int] NOT NULL,
    [Display(Name = "Tran Identifier")] public string ItemKey {get;set;} 
// 	[ItemKey] [nvarchar](255) NOT NULL,
    [Display(Name = "Tran date time")] public DateTime TRAN_DATE {get;set;} 
// 	[TRAN_DATE] [datetime] NOT NULL,
    [Display(Name = "Process")] public string Process {get;set;} 
// 	[Process] [nvarchar](255) NOT NULL,
    [Display(Name = "Tran Producer")] public string TRAN_PROD {get;set;} 
// 	[TRAN_PROD] [nvarchar](255) NOT NULL,
    [Display(Name = "Tran Stat")] public string TRAN_STAT {get;set;} 
// 	[TRAN_STAT] [nvarchar](255) NOT NULL,
    [Display(Name = "Tran Description")] public string TRAN_DESC {get;set;} 
// 	[TRAN_DESC] [ntext] NULL,
    [Display(Name = "Tran AGS")] public string TRAN_AGS {get;set;} 
// 	[TRAN_AGS] [nvarchar](255) NOT NULL,
    [Display(Name = "Tran Reciever")] public string TRAN_RECV {get;set;} 
// 	[TRAN_RECV] [nvarchar](255) NOT NULL,
    [Display(Name = "Tran Delimeter")] public string TRAN_DLIM {get;set;} 
// 	[TRAN_DLIM] [nvarchar](255) NULL,
    [Display(Name = "Tran RCON")] public string TRAN_RCON {get;set;}    
// 	[TRAN_RCON] [nvarchar](255) NULL,
    [Display(Name = "Tran Remarks")] public string TRAN_REM {get;set;} 
// 	[TRAN_REM] [ntext] NULL,
    [Display(Name = "File Set")] public string FILE_FSET {get;set;} 
// 	[FILE_FSET] [nvarchar](255) NULL,
    public TRAN() : base ("TRAN") {}
    
    }

}