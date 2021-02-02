using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ge_repository.AGS;

namespace ge_repository.OtherDatabase  {

    public class SAMP : AGSGroup  {

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
   [Display(Name = "ge_source Addition field (NON AGS)")] 
    public string ge_source {get;set;}
    
    [Display(Name = "ge_otherId Addition field for ESRI feature update (NON AGS)")] 
    public string ge_otherId {get;set;}
 
    [Display (Name = "Survey Round Ref")] 
    	public string RND_REF {get;set;}
	  // RND_REF [nvarchar](255) NULL,
    
    public  SAMP() : base ("SAMP") {
        
    }

    public override int set_values(string[] header, string[] values) {
         try {
            for (int i=0;i<header.Count();i++) {
                if (header[i] == "LOCA_ID" && values[i] != "") PointID = values[i];
                if (header[i] == "SAMP_TOP" && values[i] != "") Depth = Convert.ToDouble(values[i]);
                if (header[i] == "SAMP_REF" && values[i]!= "") SAMP_REF = values[i];
                if (header[i] == "SAMP_TYPE" && values[i] != "") SAMP_TYPE = values[i];
                if (header[i] == "SAMP_ID" && values[i] != "") SAMP_ID = values[i];
                if (header[i] == "SAMP_BASE" && values[i] != "") SAMP_BASE = Convert.ToDouble(values[i]);
                if (header[i] == "SAMP_LINK" && values[i] != "") SAMP_LINK = values[i];
                if (header[i] == "SAMP_DTIM" && values[i] != "") SAMP_DTIM = Convert.ToDateTime(values[i]);
                if (header[i] == "SAMP_UBLO" && values[i] != "") SAMP_UBLO =Convert.ToInt16(values[i]);
                if (header[i] == "SAMP_CONT" && values[i] != "") SAMP_CONT = values[i];
                if (header[i] == "SAMP_PREP" && values[i] != "") SAMP_PREP = values[i];
                if (header[i] == "SAMP_DIA" && values[i] != "") SAMP_DIA = values[i];
                if (header[i] == "SAMP_WDEP" && values[i] != "") SAMP_WDEP = Convert.ToDouble(values[i]); 
                if (header[i] == "SAMP_RECV" && values[i] != "") SAMP_RECV = Convert.ToInt16(values[i]); 
                if (header[i] == "SAMP_TECH" && values[i] != "") SAMP_TECH = values[i]; 
                if (header[i] == "SAMP_MATX" && values[i] != "") SAMP_MATX = values[i]; 
                if (header[i] == "SAMP_TYPC" && values[i] != "") SAMP_TYPC = values[i]; 
                if (header[i] == "SAMP_WHO" && values[i] != "") SAMP_WHO = values[i]; 
                if (header[i] == "SAMP_WHY" && values[i] != "") SAMP_WHY = values[i]; 
                if (header[i] == "SAMP_DESC" && values[i] != "") SAMP_DESC = values[i];  
                if (header[i] == "SAMP_DESD" && values[i] != "") SAMP_DESD = Convert.ToDateTime(values[i]); 
                if (header[i] == "SAMP_LOG" && values[i] != "") SAMP_LOG = values[i]; 
                if (header[i] == "SAMP_COND" && values[i] != "") SAMP_COND = values[i]; 
                if (header[i] == "SAMP_CLSS" && values[i] != "") SAMP_CLSS = values[i]; 
                if (header[i] == "SAMP_BAR" && values[i] != "") SAMP_BAR =  Convert.ToDouble(values[i]); 
                if (header[i] == "SAMP_TEMP" && values[i] != "") SAMP_TEMP = Convert.ToInt16(values[i]); 
                if (header[i] == "SAMP_PRES" && values[i] != "") SAMP_PRES =  Convert.ToDouble(values[i]); 
                if (header[i] == "SAMP_FLOW" && values[i] != "") SAMP_FLOW = Convert.ToDouble(values[i]); 
                if (header[i] == "SAMP_ETIM" && values[i] != "") SAMP_ETIM = Convert.ToDateTime(values[i]); 
                if (header[i] == "SAMP_DURN" && values[i] != "") SAMP_DURN = Convert.ToInt16(values[i]); 
                if (header[i] == "SAMP_CAPT" && values[i] != "") SAMP_CAPT = values[i]; 
                if (header[i] == "GEOL_STAT" && values[i] != "") GEOL_STAT = values[i]; 
                if (header[i] == "SAMP_RECL" && values[i] != "") SAMP_RECL = values[i]; 
                if (header[i] == "FILE_FSET" && values[i] != "") FILE_FSET= values[i];
            }

         } catch {
             return -1;
         }
         
         return 0;
        }
}
}

