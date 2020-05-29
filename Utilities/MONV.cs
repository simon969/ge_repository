using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
namespace ge_repository.OtherDatabase  {

// Additional table to store monitoring visit details NO-AGS
    public class MONV {

// CREATE TABLE [dbo].[MONV](
	[Key] [Display(Name = "GintRecID")] public int GintRecID {get;set;} 
//	[GintRecID] [int] IDENTITY(1,1) NOT NULL,	
	[Display(Name = "gINTProjectID")] public int gINTProjectID {get;set;} 
//	[gINTProjectID] [int] NOT NULL,
 	[Display(Name = "PointID")] public string PointID {get;set;} 
//	[PointID] [nvarchar](255) NOT NULL,
	[Display(Name = "Date and time of monitoring visit")] public DateTime? DateTime {get;set;} 
//	[DateTime] [datetime] NOT NULL,
	[Display(Name = "Weather Conditions during visit")] public string MONV_WEAT {get;set;} 
//	[MONV_WEAT] [nvarchar](255) NULL,
	[Display(Name = "Air temperature during visit")] public string MONV_TEMP {get;set;} 
//	[MONV_TEMP] [float] NULL,
	[Display(Name = "Monitoring visit reference")] public string MONV_REF {get;set;} 
//	[MONV_REF] [nvarchar](255) NULL,
	[Display(Name = "Wind conditions during visit")] public string MONV_WIND {get;set;} 
//	[MONV_WIND] [nvarchar](255) NULL,
	[Display(Name = "Are samples required from this visit")] public string MONV_SAMR {get;set;} 
//	[MONV_SAMR] [nvarchar](255) NULL,
	[Display(Name = "Is logger download required in this visit")] public string MONV_LOGR {get;set;} 
//	[MONV_LOGR] [nvarchar](255) NULL,
	[Display(Name = "Are gas readings required from this visit")] public string MONV_GASR {get;set;} 
//	[MONV_GASR] [nvarchar](255) NULL,
	[Display(Name = "Is a dip reading required from this visit")] public string MONV_DIPR {get;set;} 
//	[MONV_REF] [nvarchar](255) NULL,
	[Display(Name = "Datum height from Ground Level")] public float MONV_DIS {get;set;} 
//	[MONV_DIS] [float] NULL,
	[Display(Name = "Monitoring Geotechnical Engineer")] public string MONV_MENG {get;set;} 
//	[MONV_MENG] [nvarchar](255) NULL,
	[Display(Name = "Gas Remarks")] public string MONV_REMG {get;set;} 
//	[MONG_REM] [ntext] NULL,
	[Display(Name = "Logger Remarks")] public string MONV_REML {get;set;} 
//	[MONG_REM] [ntext] NULL,
	[Display(Name = "Dip Remarks")] public string MONV_REMD {get;set;} 
//	[MONG_REM] [ntext] NULL,
	[Display(Name = "Start Date and time of survey")] public DateTime? MONV_STAR {get;set;} 
//	[MONV_STAR] [datetime] NOT NULL,
	[Display(Name = "End Date and time of survey")] public DateTime? MONV_ENDD {get;set;} 
//	[MONV_ENDD] [datetime] NOT NULL,
	[Display(Name = "Associated file reference")] public string FILE_FSET {get;set;} 
//	[FILE_FSET] [nvarchar](255) NULL,
  	
	[Display(Name = "Addition field for ESRI feature update")] 
    public string ge_source {get;set;}
//	[ge_source] [nvarchar](255) NULL,    
    
	[Display(Name = "Addition field for ESRI feature update")] 
    public string ge_otherid {get;set;}
  //	[ge_otherId] [nvarchar](255) NULL,  
   
    [Display(Name = "Gas Meter Serial Number")] 
    
	public string GAS_SRLN {get;set;}
   //	[GAS_SRLN] [nvarchar](255) NULL,  
       	[Display (Name = "PID Calibration Date")] 
    public DateTime? GAS_CLBD {get;set;}
   //	[GAS_CLBD] [nvarchar](255) NULL, 
	
	[Display(Name = "Dip Meter Serial Number")] 
    public string DIP_SRLN {get;set;}
   //	[DIP_SRLN] [nvarchar](255) NULL,  
    public DateTime? DIP_CLBD {get;set;}
   //	[GAS_CLBD] [nvarchar](255) NULL, 
	
	[Display (Name = "Flow Meter Serial Number")] 
    public string FLO_SRLN {get;set;}
   //	[FLO_SRLN] [nvarchar](255) NULL,  
	[Display (Name = "PID Calibration Date")] 
    public DateTime? FLO_CLBD {get;set;}
   //	[FLO_CLBD] [nvarchar](255) NULL, 
	
	[Display (Name = "PID Meter Serial Number")] 
    public string PID_SRLN {get;set;}
   //	[PID_SRLN] [nvarchar](255) NULL,  
   
   	[Display (Name = "PID Calibration Date")] 
    public DateTime? PID_CLBD {get;set;}
   //	[PID_CLBD] [nvarchar](255) NULL, 
	
	[Display (Name = "Survey Round Ref")] 
	public string RND_REF {get;set;}
	// RND_REF [nvarchar](255) NULL,
	
	[Display (Name = "Pipe Diameter")] 
 	public int? PIPE_DIA {get;set;}
	 // PIPE_DIAM [int] NULL,
	[Display (Name = "Dip Datum Description")] 
 	public string MONV_DATM {get;set;}
	 // MONV_DATM [nvarchar] (255) NULL,
	
	[Display (Name = "Atmospheric Pressure (mbar)")] 
	public double? AIR_PRESS {get;set;} 
	// AIR_PRESS [float] NULL, 
	//(type: esriFieldTypeDouble, alias: Atmospheric pressure (mbar), SQL Type: sqlTypeOther, nullable: true, editable: true)
   	
	[Display (Name = "Atmospheric Temperature degC")] 
    public double? AIR_TEMP {get;set;} 
	// AIR_TEMP [float] NULL,
	//(type: esriFieldTypeDouble, alias: Atmospheric temperature (°C), SQL Type: sqlTypeOther, nullable: true, editable: true)
	
	}

}