using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;

namespace ge_repository.OtherDatabase  {

    public class SAMP {

    //  [Key] CREATE TABLE [dbo].[SAMP](
    [Display(Name = "GintRecID")] public int GintRecID {get;set;} 
    //	[GintRecID] [int] IDENTITY(1,1) NOT NULL,
    [Display(Name = "gINTProjectID")] public int gINTProjectID {get;set;} 
    //	[gINTProjectID] [int] NOT NULL,
    [Display(Name = "PointID")] public string PointID {get;set;}     
    // 	[PointID] [nvarchar](255) NOT NULL,
     [Display(Name = "SAMP_TOP")] public double Depth {get;set;}   
     // [Depth] [float] NOT NULL,
     [Display(Name = "SAMP_REF")] public string SAMP_REF {get;set;}   
     //	[SAMP_REF] [nvarchar](255) NULL,
     [Display(Name = "SAMP_TYPE")] public string SAMP_TYPE {get;set;}   
     //	[SAMP_TYPE] [nvarchar](255) NULL,
     [Display(Name = "SAMP_ID")] public string SAMP_ID {get;set;}   
     //	[SAMP_ID] [nvarchar](255) NULL,
     [Display(Name = "SAMP_BASE")] public double? SAMP_BASE {get;set;}   
     //	[SAMP_BASE] [float] NULL,
     [Display(Name = "SAMP_LINK")] public string SAMP_LINK {get;set;}   
     // [SAMP_LINK] [ntext] NULL,
     [Display(Name = "SAMP_DTIM")] public DateTime? SAMP_DTIM {get;set;}   
     //	[SAMP_DTIM] [datetime] NULL,
     [Display(Name = "SAMP_UBLO")] public int? SAMP_UBLO {get;set;}   
     //	[SAMP_UBLO] [smallint] NULL,
     [Display(Name = "SAMP_CONT")] public string SAMP_CONT {get;set;}
     //	[SAMP_CONT] [nvarchar](255) NULL,
     [Display(Name = "SAMP_PREP")] public string SAMP_PREP {get;set;}    
     //	[SAMP_PREP] [ntext] NULL,
     [Display(Name = "SAMP_DIA")] public string SAMP_DIA {get;set;}    
     //	[SAMP_SDIA] [smallint] NULL,
     [Display(Name = "SAMP_WDEP")] public double? SAMP_WDEP {get;set;}   
     //	[SAMP_WDEP] [float] NULL,
     [Display(Name = "SAMP_RECV")] public int? SAMP_RECV {get;set;}    
     // [SAMP_RECV] [smallint] NULL,
     [Display(Name = "SAMP_TECH")] public string SAMP_TECH {get;set;}    
     // [SAMP_TECH] [nvarchar](255) NULL,
     [Display(Name = "SAMP_MATX")] public string SAMP_MATX {get;set;}
     // [SAMP_MATX] [nvarchar](255) NULL,
    [Display(Name = "SAMP_TYPC")] public string SAMP_TYPC {get;set;}
     // [SAMP_TYPC] [nvarchar](255) NULL,
    [Display(Name = "SAMP_WHO")] public string SAMP_WHO {get;set;}    
    // 	[SAMP_WHO] [nvarchar](255) NULL,
    [Display(Name = "SAMP_WHY")] public string SAMP_WHY {get;set;}    
    // 	[SAMP_WHY] [nvarchar](255) NULL,
    [Display(Name = "SAMP_REM")] public string SAMP_REM {get;set;}   
    // 	[SAMP_REM] [ntext] NULL,
    [Display(Name = "SAMP_DESC")] public string SAMP_DESC {get;set;}
    // 	[SAMP_DESC] [ntext] NULL,
    [Display(Name = "SAMP_DESD")] public DateTime SAMP_DESD {get;set;}    
    // 	[SAMP_DESD] [datetime] NULL,
    [Display(Name = "SAMP_LOG")] public string SAMP_LOG {get;set;}
    // 	[SAMP_LOG] [nvarchar](255) NULL,
    [Display(Name = "SAMP_COND")] public string SAMP_COND {get;set;}
    // 	[SAMP_COND] [nvarchar](255) NULL,
    [Display(Name = "SAMP_CLSS")] public string SAMP_CLSS {get;set;}
    // 	[SAMP_CLSS] [nvarchar](255) NULL,
   [Display(Name = "SAMP_BAR")] public double? SAMP_BAR {get;set;}
    // 	[SAMP_BAR] [float] NULL,
   [Display(Name = "SAMP_TEMP")] public int? SAMP_TEMP {get;set;}
    // 	[SAMP_TEMP] [smallint] NULL,
   [Display(Name = "SAMP_PRES")] public double? SAMP_PRES {get;set;}    
    // 	[SAMP_PRES] [float] NULL,
   [Display(Name = "SAMP_FLOW")] public double? SAMP_FLOW {get;set;}     
   // 	[SAMP_FLOW] [float] NULL,
   [Display(Name = "SAMP_ETIM")] public DateTime? SAMP_ETIM {get;set;}     
   // 	[SAMP_ETIM] [datetime] NULL,
   [Display(Name = "SAMP_DURN")] public double? SAMP_DURN {get;set;}
    // 	[SAMP_DURN] [float] NULL,
    [Display(Name = "SAMP_CAPT")] public string SAMP_CAPT {get;set;}
    // 	[SAMP_CAPT] [nvarchar](255) NULL,
     [Display(Name = "GEOL_STAT")] public string GEOL_STAT {get;set;}   
     // [GEOL_STAT] [nvarchar](255) NULL,
    [Display(Name = "SAMP_RECL")] public string SAMP_RECL {get;set;}    
    // [SAMP_RECL] [smallint] NULL,
   [Display(Name = "FILE_FSET")] public string FILE_FSET {get;set;}     
    // 	[FILE_FSET] [nvarchar](255) NULL


    }
}
