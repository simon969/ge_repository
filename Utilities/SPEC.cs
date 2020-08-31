using System;
using System.ComponentModel.DataAnnotations;

namespace ge_repository.OtherDatabase  {

    public class SPEC {


    // CREATE TABLE [dbo].[SPEC](
	[Display(Name = "GintRecID")] public int GintRecID {get;set;} 
    //	[GintRecID] [int] IDENTITY(1,1) NOT NULL,
    [Display(Name = "gINTProjectID")] public int gINTProjectID {get;set;} 
    //	[gINTProjectID] [int] NOT NULL,
    [Display(Name = "PointID")] public string PointID {get;set;}     
    // 	[PointID] [nvarchar](255) NOT NULL,
    [Display(Name = "SAMP_Depth")] public double SAMP_Depth {get;set;}    
   	// [SAMP_Depth] [float] NOT NULL,
    [Display(Name = "SAMP_REF")] public string SAMP_REF {get;set;}    	
    // [SAMP_REF] [nvarchar](255) NULL,
	[Display(Name = "SAMP_TYPE")] public string SAMP_TYPE {get;set;}    
    // [SAMP_TYPE] [nvarchar](255) NULL,
	[Display(Name = "SAMP_ID")] public string SAMP_ID {get;set;}    
    // [SAMP_ID] [nvarchar](255) NULL,
	[Display(Name = "SPEC_DPTH")] public string Depth {get;set;}    
    // [Depth] [float] NOT NULL,
	[Display(Name = "SPEC_REF")] public string SPEC_REF {get;set;}    
    // [SPEC_REF] [nvarchar](255) NULL,
	[Display(Name = "SPEC_DESC")] public string SPEC_DESC {get;set;}    
    // [SPEC_DESC] [ntext] NULL,
	[Display(Name = "SPEC_REM")] public string SPEC_REM {get;set;}    
    // [SPEC_REM] [ntext] NULL

    [Display(Name = "ge_source Addition field (NON AGS)")] 
    public string ge_source {get;set;}
    
    [Display(Name = "ge_otherId Addition field for ESRI feature update (NON AGS)")] 
    public string ge_otherId {get;set;}
 
    [Display (Name = "Survey Round Ref")] 
    public string RND_REF {get;set;}
	  // RND_REF [nvarchar](255) NULL,
    }
}
