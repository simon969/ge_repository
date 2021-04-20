using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using ge_repository.AGS;
using ge_repository.interfaces;

namespace ge_repository.OtherDatabase  {

    public class POINT : AGSGroup , IGintTable{

// CREATE TABLE [dbo].[POINT](
	[Key] [Display(Name = "GintRecID")] public int GintRecID {get;set;} 
//	[GintRecID] [int] IDENTITY(1,1) NOT NULL,
  	[Display(Name = "gINTProjectID")] public int gINTProjectID {get;set;} 
//	[gINTProjectID] [int] NOT NULL,

    [Display(Name = "Location identifier", Description=""), 
		AGSAttribute("ags_unit","NA"), 
		AGSAttribute("ags_heading","LOCA"),
		AGSAttribute("ags_type","ID")] 
	public string PointID {get;set;} 
	// [PointID] [nvarchar](255) NOT NULL,

    [Display(Name = "Final depth"),
	 	AGSAttribute("ags_unit","m"), 
		AGSAttribute("ags_heading","LOCA_FDEP"),
		AGSAttribute("ags_type","2DP")]
	public double HoleDepth {get;set;} 
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
	 [Display(Name = "National Grid Northing of end of traverse")] public double? LOCA_NTRV {get;set;} 
	// [LOCA_NTRV] [float] NULL,
	 [Display(Name = "Ground level relative to datum of end of traverse")] public double? LOCA_LTRV {get;set;} 
	// [LOCA_LTRV] [float] NULL,
	 [Display(Name = "Local grid easting of end of traverse")] public double? LOCA_XTRL {get;set;} 
	// [LOCA_XTRL] [float] NULL,
	 [Display(Name = "Local grid northing of end of traverse")] public double? LOCA_YTRL {get;set;} 
	// [LOCA_YTRL] [float] NULL,
	 [Display(Name = "Local elevation of end of traverse")] public double? LOCA_ZTRL {get;set;} 
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

	public override int set_values(string[] header, string[] values) {
         try {
            for (int i=0;i<header.Length;i++) {
                
				if (header[i] == "LOCA_ID" && values[i] != "") PointID = values[i];
				if (header[i] == "LOCA_TYPE" && values[i] != "") LOCA_TYPE = values[i];
				if (header[i] == "LOCA_STAT" && values[i] != "") LOCA_STAT = values[i];
				
				if (header[i] == "LOCA_NATE" && values[i] != "") LOCA_NATE = Convert.ToDouble(values[i]);
				if (header[i] == "LOCA_NATN" && values[i] != "") LOCA_NATN = Convert.ToDouble(values[i]);
				if (header[i] == "LOCA_GREF" && values[i] != "") LOCA_GREF = values[i];
				if (header[i] == "LOCA_GL" && values[i] != "") LOCA_GL = Convert.ToDouble(values[i]);
				
				if (header[i] == "LOCA_REM" && values[i] != "") LOCA_REM = values[i];
				if (header[i] == "LOCA_FDEP" && values[i]!= "") HoleDepth = Convert.ToDouble(values[i]);
				if (header[i] == "LOCA_STAR" && values[i]!= "") LOCA_STAR = Convert.ToDateTime(values[i]);
				
				if (header[i] == "LOCA_PURP" && values[i] != "") LOCA_PURP = values[i];
				if (header[i] == "LOCA_TERM" && values[i] != "") LOCA_TERM = values[i];
				if (header[i] == "LOCA_ENDD" && values[i]!= "") LOCA_ENDD = Convert.ToDateTime(values[i]);
				if (header[i] == "LOCA_LETT" && values[i]!= "") LOCA_LETT = values[i];

				if (header[i] == "LOCA_LOCX" && values[i] != "") East = Convert.ToDouble(values[i]);
				if (header[i] == "LOCA_LOCY" && values[i] != "") North = Convert.ToDouble(values[i]);
                if (header[i] == "LOCA_LOCZ" && values[i] != "") Elevation = Convert.ToDouble(values[i]);
				
				if (header[i] == "LOCA_LREF" && values[i]!= "") LOCA_LREF = values[i];
				if (header[i] == "LOCA_DATM" && values[i]!= "") LOCA_DATM = values[i];
				
				if (header[i] == "LOCA_ETRV" && values[i] != "") LOCA_ETRV = Convert.ToDouble(values[i]);
				if (header[i] == "LOCA_NTRV" && values[i] != "") LOCA_NTRV = Convert.ToDouble(values[i]);
                if (header[i] == "LOCA_LTRV" && values[i] != "") LOCA_LTRV = Convert.ToDouble(values[i]);
				
				if (header[i] == "LOCA_XTRL" && values[i] != "") LOCA_XTRL = Convert.ToDouble(values[i]);
				if (header[i] == "LOCA_YTRL" && values[i] != "") LOCA_YTRL =  Convert.ToDouble(values[i]);
				if (header[i] == "LOCA_ZTRL" && values[i] != "") LOCA_ZTRL = Convert.ToDouble(values[i]);
 				
				if (header[i] == "LOCA_LAT" && values[i] != "") LOCA_LAT = Convert.ToDouble(values[i]);
                if (header[i] == "LOCA_LON" && values[i] != "") LOCA_LON = Convert.ToDouble(values[i]);
                if (header[i] == "LOCA_ELAT" && values[i] != "") LOCA_ELAT = Convert.ToDouble(values[i]);
                if (header[i] == "LOCA_ELON" && values[i] != "") LOCA_ELON = Convert.ToDouble(values[i]);
				
				if (header[i] == "LOCA_LLZ" && values[i] != "") LOCA_LLZ = values[i];
                if (header[i] == "LOCA_LOCM" && values[i] != "") LOCA_LOCM = values[i];
                if (header[i] == "LOCA_LOCA" && values[i] != "") LOCA_LOCA = values[i];

				if (header[i] == "LOCA_CLST" && values[i] !="") LOCA_CLST = values[i];
				if (header[i] == "LOCA_ALID" && values[i] !="") LOCA_ALID = values[i];
				if (header[i] == "LOCA_OFFS" && values[i] != "") LOCA_OFFS = Convert.ToDouble(values[i]);
				if (header[i] == "LOCA_CNGE" && values[i] != "") LOCA_CNGE = values[i];
				if (header[i] == "LOCA_TRAN" && values[i] != "") LOCA_TRAN = values[i];

                if (header[i] == "FILE_FSET" && values[i] != "") FILE_FSET= values[i];
               
                if (header[i] == "LOCA_CKBY") LOCA_CKBY = values[i];
                if (header[i] == "LOCA_CKDT" && values[i] != "") LOCA_CKDT = Convert.ToDateTime(values[i]);
                if (header[i] == "LOCA_CNGE") LOCA_CNGE = values[i];
                
            }

         } catch {
             return -1;
         }
         
         return 0;
        }
	public void set_values (DataRow row) {




    }
	public override string[] get_values(string[] header, string[] unit, string[] type) {
         try {

			string[] ret = new string[header.Length];  
            
			for (int i=0;i<header.Length;i++) {
				if (header[i] == "HEADING") ret[i] = "DATA";
				if (header[i] == "LOCA_ID" && PointID != "") ret[i] = PointID;
				if (header[i] == "LOCA_TYPE" && LOCA_TYPE != null) ret[i]= LOCA_TYPE;
				if (header[i] == "LOCA_STAT" && LOCA_STAT != null) ret[i]= LOCA_STAT;
				
				if (header[i] == "LOCA_NATE" && LOCA_NATE != null) ret[i]= String.Format(get_format(unit[i],type[i]),LOCA_NATE.Value);
				if (header[i] == "LOCA_NATN" && LOCA_NATN != null) ret[i]= String.Format(get_format(unit[i],type[i]),LOCA_NATN.Value);
				if (header[i] == "LOCA_GREF" && LOCA_GREF != null) ret[i]= LOCA_GREF;
				if (header[i] == "LOCA_GL" && LOCA_GL != null) ret[i]= String.Format(get_format(unit[i],type[i]),LOCA_GL.Value);
				
				if (header[i] == "LOCA_REM" && LOCA_REM != null) ret[i]= LOCA_REM;
				if (header[i] == "LOCA_FDEP") ret[i] = String.Format(get_format(unit[i],type[i]),HoleDepth);
				if (header[i] == "LOCA_STAR" && LOCA_STAR != null) ret[i]= LOCA_STAR.Value.ToString(get_format(unit[i],type[i]));
				
				if (header[i] == "LOCA_PURP" && LOCA_PURP != null) ret[i]= LOCA_PURP;
				if (header[i] == "LOCA_TERM" && LOCA_TERM != null) ret[i]= LOCA_TERM;
				if (header[i] == "LOCA_ENDD" && LOCA_ENDD != null) ret[i]= LOCA_ENDD.Value.ToString(get_format(unit[i],type[i]));
				if (header[i] == "LOCA_LETT" && LOCA_LETT!= null) ret[i]= LOCA_LETT;

				if (header[i] == "LOCA_LOCX" && East != null) {
					ret[i]= String.Format(get_format(unit[i],type[i]),East.Value);
				}
				if (header[i] == "LOCA_LOCY" && North != null) ret[i]= String.Format(get_format(unit[i],type[i]),North.Value);
                if (header[i] == "LOCA_LOCZ" && Elevation != null) ret[i]= String.Format(get_format(unit[i],type[i]),Elevation.Value);
				
				if (header[i] == "LOCA_LREF" && LOCA_LREF != null) ret[i]= LOCA_LREF;
				if (header[i] == "LOCA_DATM" && LOCA_DATM != null) ret[i]= LOCA_DATM;
				
				if (header[i] == "LOCA_ETRV" && LOCA_ETRV != null) ret[i]= String.Format(get_format(unit[i],type[i]),LOCA_ETRV.Value);
				if (header[i] == "LOCA_NTRV" && LOCA_NTRV != null) ret[i]= String.Format(get_format(unit[i],type[i]),LOCA_NTRV.Value);
                if (header[i] == "LOCA_LTRV" && LOCA_LTRV != null) ret[i]= String.Format(get_format(unit[i],type[i]),LOCA_LTRV.Value);
				
				if (header[i] == "LOCA_XTRL" && LOCA_XTRL != null) ret[i]= String.Format(get_format(unit[i],type[i]),LOCA_XTRL.Value);
				if (header[i] == "LOCA_YTRL" && LOCA_YTRL != null) ret[i]= String.Format(get_format(unit[i],type[i]),LOCA_YTRL.Value);
				if (header[i] == "LOCA_ZTRL" && LOCA_ZTRL != null) ret[i]= String.Format(get_format(unit[i],type[i]),LOCA_ZTRL.Value);
 				
				if (header[i] == "LOCA_LAT" && LOCA_LAT != null) ret[i]= String.Format(get_format(unit[i],type[i]),LOCA_LAT.Value);
                if (header[i] == "LOCA_LON" && LOCA_LON != null) ret[i]= String.Format(get_format(unit[i],type[i]),LOCA_LON.Value);
                if (header[i] == "LOCA_ELAT" && LOCA_ELAT != null) ret[i]= String.Format(get_format(unit[i],type[i]),LOCA_ELAT.Value);
                if (header[i] == "LOCA_ELON" && LOCA_ELON != null) ret[i]= String.Format(get_format(unit[i],type[i]),LOCA_ELON.Value);
				
				if (header[i] == "LOCA_LLZ" && LOCA_LLZ != null) ret[i]= LOCA_LLZ;
                if (header[i] == "LOCA_LOCM" && LOCA_LOCM != null) ret[i]= LOCA_LOCM;
                if (header[i] == "LOCA_LOCA" && LOCA_LOCA != null) ret[i]= LOCA_LOCA;

				if (header[i] == "LOCA_CLST" && LOCA_CLST != null) ret[i] = LOCA_CLST;
				if (header[i] == "LOCA_ALID" && LOCA_ALID != null) ret[i] = LOCA_ALID;
				if (header[i] == "LOCA_OFFS" && LOCA_OFFS != null) ret[i]= String.Format(get_format(unit[i],type[i]),LOCA_OFFS.Value);
				if (header[i] == "LOCA_CNGE" && LOCA_CNGE != null) ret[i]= LOCA_CNGE;
				if (header[i] == "LOCA_TRAN" && LOCA_TRAN != null) ret[i]= LOCA_TRAN;

                if (header[i] == "FILE_FSET" && FILE_FSET != null) ret[i]= FILE_FSET;
               
                if (header[i] == "LOCA_CKBY" && LOCA_CKBY != null) ret[i] = LOCA_CKBY;
                if (header[i] == "LOCA_CKDT" && LOCA_CKDT != null) ret[i]= LOCA_CKDT.Value.ToString(get_format(unit[i],type[i])); 
                
                
            }
   			return ret;
         } catch (Exception e) {
             return null;
         }
         
      
        }
	
    }
}

