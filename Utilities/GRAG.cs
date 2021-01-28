using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
using ge_repository.AGS;

namespace ge_repository.OtherDatabase  {

    public class GRAG : AGSGroup {

// CREATE TABLE [dbo].[GRAG](

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
	 [Display(Name = "Classification Override")] public string Classification_Override {get;set;} 
	 // [Classification_Override] [nvarchar](255) NULL,
	 [Display(Name = "BSI Grading Override")] public string BSI_Grading_Override {get;set;} 
	 // [BSI_Grading_Override] [nvarchar](255) NULL,
	 [Display(Name = "Specimen Description")] public string SPEC_DESC {get;set;} 
	 // [SPEC_DESC] [ntext] NULL,
	 [Display(Name = "Details of specimen preparation including time between preparation and testing")] public string SPEC_PREP {get;set;} 
	 // [SPEC_PREP] [ntext] NULL,
	
	 [Display(Name = "Uniformity coefficient D60/D10")] public double? GRAG_UC {get;set;} 
	 // [GRAG_UC] [float] NULL,
	
	[Display(Name = "Percentage of material tested greater than 63mm (cobbles)")] public double? GRAG_VCRE {get;set;}
	// [GRAG_VCRE] [float] NULL,
	[Display(Name = "Percentage of material tested in range 63mm to 2mm (gravel)")] public double? GRAG_GRAV {get;set;}
	// [GRAG_GRAV] [float] NULL,
	[Display(Name = "Percentage of material tested in range 2mm to 63um (sand)")] public double? GRAG_SAND {get;set;}
	// [GRAG_SAND] [float] NULL,
	[Display(Name = "Percentage of material tested in range 63um to 2um (silt)")] public double? GRAG_SILT {get;set;}
	// [GRAG_SILT] [float] NULL,
	[Display(Name = "Percentage of material than 2um (clay)")] public double? GRAG_CLAY {get;set;}
	// [GRAG_CLAY] [float] NULL,
	[Display(Name = "Percentage less than 63um")] public double? GRAG_FINE {get;set;}
	// [GRAG_FINE] [float] NULL,
	[Display(Name = "Percentage of material tested retained on 37.5mm sieve")] public double? GRAG_375 {get;set;}
	// [GRAG_375] [float] NULL,
	[Display(Name = "Percentage of material tested retained on 20mm sieve")] public double? GRAG_200 {get;set;}
	// [GRAG_200] [float] NULL,
	[Display(Name = "Percentage of material tested retained on 6.3mm sieve")] public double? GRAG_063 {get;set;}
	// [GRAG_063] [float] NULL,
	[Display(Name = "Percentage of material tested retained on 2mm sieve")] public double? GRAG_020 {get;set;}
	// [GRAG_020] [float] NULL,
	[Display(Name = "Percentage of material tested passing 425um sieve")] public double? GRAG_425 {get;set;}
	// [GRAG_425] [float] NULL,
	[Display(Name = "Remarks on test")] public string GRAG_REM {get;set;}
	// [GRAG_REM] [ntext] NULL,
	 [Display(Name = "Test Method")] public string GRAG_METH {get;set;} 
	 // [GRAG_METH] [nvarchar](255) NULL,
	 [Display(Name = "Laboratory")] public string GRAG_LAB {get;set;} 
	 // [GRAG_LAB] [nvarchar](255) NULL,
	 [Display(Name = "Accrediting body and reference number (when appropriate)")] public string GRAG_CRED {get;set;} 
	 // [GRAG_CRED] [nvarchar](255) NULL,
	 [Display(Name = "Test Status")] public string TEST_STAT {get;set;} 
	 // [TEST_STAT] [nvarchar](255) NULL,
	 [Display(Name = "Associated file reference")] public string FILE_FSET {get;set;} 
	// [FILE_FSET] [nvarchar](255) NULL,
    
    public GRAG(): base ("GRAG") {}
	}

	[Display(Name ="GRAG with GRAT children")] public class GRAG_WC : GRAG 
	{
		public List<GRAT> GRAT {get;set;} = new List<GRAT>();
	}
}

