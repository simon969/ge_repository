using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
namespace ge_repository.OtherDatabase  {

    public class GRAT {

// CREATE TABLE [dbo].[GRAT](

	[Key] [Display(Name = "GintRecID")] public int GintRecId {get;set;} 
//	[GintRecID] [int] IDENTITY(1,1) NOT NULL,
  	[Display(Name = "gINTProjectID")] public int gINTProjectID {get;set;} 
//	[gINTProjectID] [int] NOT NULL,
    [Display(Name = "Location identifier")] public string PointID {get;set;} 
	// [PointID] [nvarchar](255) NOT NULL,
    [Display(Name = "Depth to the TOP of sample")] public double SAMP_Depth {get;set;} 
	// [SAMP_Depth] [float] NOT NULL,
	 [Display(Name = "Sample Reference number")] public string SAMP_REF {get;set;} 
	 // [SAMP_REF] [nvarchar](255) NULL,
	 [Display(Name = "Sample type")] public string SAMP_TYPE {get;set;} 
	 // [SAMP_TYPE] [nvarchar](255) NULL,
	 [Display(Name = "Sample unique global identifier")] public string SAMP_ID {get;set;} 
	 // [SAMP_ID] [nvarchar](255) NULL,
	 [Display(Name = "Specimen depth")] public double Depth {get;set;} 
	 // [Depth] [float] NOT NULL,
	 [Display(Name = "Specimen reference number")] public string SPEC_REF {get;set;} 
	 // [SPEC_REF] [nvarchar](255) NULL,
	 [Display(Name = "Particle Size (mm)")] public double Reading {get;set;} 
	 // [Reading] [float] NOT NULL,
	 [Display(Name = "Percentage passing")] public int? GRAT_PERP {get;set;} 
	 // [GRAT_PERP] [smallint] NULL,
	 [Display(Name = "Distribution Test Type")] public string GRAT_TYPE {get;set;} 
	 // [GRAT_TYPE] [nvarchar](255) NULL,
	[Display(Name = "Remarks on test")] public string GRAT_REM {get;set;}
	// [GRAT_REM] [ntext] NULL,
	[Display(Name = "Associated file reference")] public string FILE_FSET {get;set;} 
	// [FILE_FSET] [nvarchar](255) NULL,
    
    }
}

