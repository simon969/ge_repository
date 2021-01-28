using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
using ge_repository.AGS;

namespace ge_repository.OtherDatabase  {

    public class POINT : AGSGroup {

// CREATE TABLE [dbo].[POINT](
	[Key] [Display(Name = "GintRecID")] public int GintRecId {get;set;} 
//	[GintRecID] [int] IDENTITY(1,1) NOT NULL,
  	[Display(Name = "gINTProjectID")] public int gINTProjectID {get;set;} 
//	[gINTProjectID] [int] NOT NULL,
    [Display(Name = "Location identifier")] public string PointID {get;set;} 
	// [PointID] [nvarchar](255) NOT NULL,
    [Display(Name = "Final depth")] public double HoleDepth {get;set;} 
    // [HoleDepth] [float] NOT NULL,
    [Display(Name = "Elevation")] public double? Elevation {get;set;} 
	// [Elevation] [float] NULL,
    [Display(Name = "North")] public double? North {get;set;} 
	// [North] [float] NULL,
    [Display(Name = "East")] public double? East {get;set;} 
	// [East] [float] NULL,
    [Display(Name = "Plunge")] public double? Plunge {get;set;} 
	// [Plunge] [float] NULL,
    [Display(Name = "Hole Type")] public string LOCA_TYPE {get;set;} 
	// [LOCA_TYPE] [nvarchar](255) NULL,
    [Display(Name = "Status of information relating to this position")] public string LOCA_STAT {get;set;} 
	// [LOCA_STAT] [nvarchar](255) NULL,
    [Display(Name = "National grid referencing system used")] public string LOCA_GREF {get;set;} 
	// [LOCA_GREF] [nvarchar](255) NULL,
     [Display(Name = "Started Date")] public DateTime? LOCA_STAR {get;set;} 
	// [LOCA_STAR] [datetime] NULL, 
	[Display(Name = "Ended Date")] public DateTime? LOCA_ENDD {get;set;} 
	// [LOCA_ENDD] [datetime] NULL,
	 [Display(Name = "Purpose of activity at this location")] public string LOCA_PURP {get;set;} 
	// [LOCA_PURP] [nvarchar](255) NULL,
 	[Display(Name = "Reason for activity termination")] public string LOCA_TERM {get;set;} 
	// [LOCA_TERM] [nvarchar](255) NULL,
	 [Display(Name = "OSGB letter grid reference")] public string LOCA_LETT {get;set;} 
	// [LOCA_LETT] [nvarchar](255) NULL,
	 [Display(Name = "National Grid Location Northing")] public double? LOCA_NATN {get;set;} 
	// [LOCA_NATN] [float] NULL,
	 [Display(Name = "National Grid Location Easting")] public double? LOCA_NATE {get;set;} 
	// [LOCA_NATE] [float] NULL,
	 [Display(Name = "Ground Level")] public double? LOCA_GL {get;set;} 
	// [LOCA_GL] [float] NULL,
	 [Display(Name = "Local grid referencing system used")] public string LOCA_LREF {get;set;} 
	// [LOCA_LREF] [nvarchar](255) NULL,
	 [Display(Name = "Local datum referencing system used")] public string LOCA_DATM{get;set;} 
	// [LOCA_DATM] [nvarchar](255) NULL,
	 [Display(Name = "National Grid Easting of end of traverse")] public double? LOCA_ETRV {get;set;} 
	// [LOCA_ETRV] [float] NULL,
	 [Display(Name = "National Grid Northing of end of traverse")] public string LOCA_NTRV {get;set;} 
	// [LOCA_NTRV] [float] NULL,
	 [Display(Name = "Ground level relative to datum of end of traverse")] public string LOCA_LTRV {get;set;} 
	// [LOCA_LTRV] [float] NULL,
	 [Display(Name = "Local grid easting of end of traverse")] public string LOCA_XTRL {get;set;} 
	// [LOCA_XTRL] [float] NULL,
	 [Display(Name = "Local grid northing of end of traverse")] public string LOCA_YTRL {get;set;} 
	// [LOCA_YTRL] [float] NULL,
	 [Display(Name = "Local elevation of end of traverse")] public string LOCA_ZTRL {get;set;} 
	// [LOCA_ZTRL] [float] NULL,
	 [Display(Name = "Latitude")] public double? LOCA_LAT {get;set;} 
	// [LOCA_LAT] [float] NULL,
	 [Display(Name = "Longitude")] public double? LOCA_LON {get;set;} 
	// [LOCA_LON] [float] NULL,
	 [Display(Name = "Latitude of end of traverse")] public double? LOCA_ELAT {get;set;} 
	// [LOCA_ELAT] [float] NULL,
	 [Display(Name = "Longitude of end of traverse")] public double? LOCA_ELON {get;set;} 
	// [LOCA_ELON] [float] NULL,
	 [Display(Name = "Projection Format")] public string LOCA_LLZ {get;set;} 
	// [LOCA_LLZ] [nvarchar](255) NULL,
	 [Display(Name = "Method of Location")] public string LOCA_LOCM {get;set;} 
	// [LOCA_LOCM] [nvarchar](255) NULL,
	 [Display(Name = "Location sub division within project")] public string LOCA_LOCA {get;set;} 
	// [LOCA_LOCA] [nvarchar](255) NULL,
	 [Display(Name = "Hole cluster reference number")] public string LOCA_CLST {get;set;} 
	// [LOCA_CLST] [nvarchar](255) NULL,
	 [Display(Name = "Alignmnet Identifier")] public string LOCA_ALID {get;set;} 
	// [LOCA_ALID] [nvarchar](255) NULL,
	 [Display(Name = "Offset of hole from assigned alignment")] public double? LOCA_OFFS {get;set;} 
	// [LOCA_OFFS] [float] NULL,
	 [Display(Name = "Chainage of hole on assigned alignment")] public string LOCA_CNGE {get;set;} 
	// [LOCA_CNGE] [nvarchar] (255) NULL,
	 [Display(Name = "Reference to or details of algorithm used to calculate local grid reference, local ground levels or chainage")] public string LOCA_TRAN {get;set;} 
	// [LOCA_TRAN] [nvarchar](255) NULL,
	 [Display(Name = "Remarks")] public string LOCA_REM {get;set;} 
	// [LOCA_REM] [ntext] NULL,
	 [Display(Name = "Checked By")] public string LOCA_CKBY {get;set;} 
	// [LOCA_CKBY] [nvarchar](255) NULL,
	 [Display(Name = "Checked Date")] public DateTime? LOCA_CKDT {get;set;} 
	// [LOCA_CKDT] [datetime] NULL,
	
	// [Display(Name = "Depth of log on page")] public float Depth_Log_Page {get;set;} 
	// [Depth_Log_Page] [real] NULL,
	// [Display(Name = "Site Map on Log Radius")] public string [Sit] {get;set;} 
	// [Site Map on Log Radius] [float] NULL,

	 [Display(Name = "Associated File")] public string FILE_FSET {get;set;} 
	// [FILE_FSET] [nvarchar](255) NULL,
	public POINT() : base ("LOCA") {}

	public override int setValues(string[] header, string[] values) {
         try {
            for (int i=0;i<header.Length;i++) {
                if (header[i] == "LOCA_NATE" && values[i] != "") East = Convert.ToDouble(values[i]);
                if (header[i] == "LOCA_GL" && values[i] != "") Elevation = Convert.ToDouble(values[i]);
                if (header[i] == "FILE_FSET") FILE_FSET= values[i];
                if (header[i] == "LOCA_FDEP" && values[i]!= "") HoleDepth = Convert.ToDouble(values[i]);
                if (header[i] == "LOCA_ALID") LOCA_ALID = values[i];
                if (header[i] == "LOCA_CKBY") LOCA_CKBY = values[i];
                if (header[i] == "LOCA_CKDT" && values[i] != "") LOCA_CKDT = Convert.ToDateTime(values[i]);
                if (header[i] == "LOCA_CLST") LOCA_CLST = values[i];
                if (header[i] == "LOCA_CNGE") LOCA_CNGE = values[i];
                if (header[i] == "LOCA_DATM") LOCA_DATM = values[i];
                if (header[i] == "LOCA_ELAT" && values[i] != "") LOCA_ELAT = Convert.ToDouble(values[i]);
                if (header[i] == "LOCA_ELON" && values[i] != "") LOCA_ELON = Convert.ToDouble(values[i]);
                if (header[i] == "LOCA_ENDD" && values[i] != "") LOCA_ENDD = Convert.ToDateTime(values[i]);
                if (header[i] == "LOCA_ETRV" && values[i] != "") LOCA_ETRV = Convert.ToDouble(values[i]);
            }

         } catch {
             return -1;
         }
         
         return 0;
        }
	
    }
}

