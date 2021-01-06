
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
namespace ge_repository.OtherDatabase  {

    public class MOND {

   [Key] 
   [Display(Name = "GintRecID")] public int GintRecID {get;set;} 
    //	[GintRecID] [int] IDENTITY(1,1) NOT NULL,
  	[Display(Name = "gINTProjectID")] public int gINTProjectID {get;set;} 
    //	[gINTProjectID] [int] NOT NULL,
    [Display(Name = "PointID")] public string PointID {get;set;} 
    // 	[PointID] [nvarchar](255) NOT NULL,
    [Display(Name = "Monitoring Point Identifier")] public string ItemKey {get;set;} 
    // 	[ItemKey] [nvarchar](255) NULL,
    [Display(Name = "Distance of monitoring point from HOLE_ID")] public double? MONG_DIS {get;set;} 
    // 	[MONG_DIS] [float] NULL,
    [Display(Name = "Date and time of reading")] public DateTime? DateTime {get;set;}
    // 	[DateTime] [datetime] NULL,
    [Display(Name = "Reading Type")] public string MOND_TYPE {get;set;} 
    // 	[MOND_TYPE] [nvarchar](255) NULL,
    [Display(Name = "Reading Ref")] public string MOND_REF {get;set;} 
    // 	[MOND_REF] [nvarchar](255) NULL,
    [Display(Name = "Instrument reference / serial number")] public string MOND_INST {get;set;} 
    // 	[MOND_INST] [nvarchar](255) NULL,
    [Display(Name = "Reading")] public string MOND_RDNG {get;set;} 
    // 	[MOND_RDNG] [nvarchar](255) NULL,
    [Display(Name = "Unit of reading")] public string MOND_UNIT {get;set;} 
    // 	[MOND_UNIT] [nvarchar](255) NOT NULL,
    [Display(Name = "Test method")] public string MOND_METH {get;set;} 
    // 	[MOND_METH] [nvarchar](255) NULL,
     [Display(Name = "Instrument/method reading/detection limit")] public double? MOND_LIM {get;set;}  
    // 	[MOND_LIM] [float] NULL,
      [Display(Name = "Instrument/method upper reading/detection (when appropriate)")] public double? MOND_ULIM {get;set;} 
    // 	[MOND_ULIM] [float] NULL,
      [Display(Name = "Client preferred name of measurement")] public string MOND_NAME {get;set;} 
    // 	[MOND_NAME] [nvarchar](255) NULL,
      [Display(Name = "Remarks")] public string MOND_REM {get;set;} 
    // 	[MOND_REM] [ntext] NULL,
      [Display(Name = "Accrediting body and reference number (when appropriate)")] public string MOND_CRED {get;set;} 
    // 	[MOND_CRED] [nvarchar](255) NULL,
      [Display(Name = "Name of testing organisation")] public string MOND_CONT {get;set;} 
    // 	[MOND_CONT] [nvarchar](255) NULL,
    [Display(Name = "Associated file reference")] public string FILE_FSET {get;set;} 
    // 	[FILE_FSET] [nvarchar](255) NULL,

    [Display(Name = "Addition field for ESRI feature update (NON AGS)")] 
    public string ge_source {get;set;}
    
    [Display(Name = "Addition field for ESRI feature update (NON AGS)")] 
    public string ge_otherId {get;set;}
 
    [Display (Name = "Survey Round Ref")] 
    	public string RND_REF {get;set;}
	  // RND_REF [nvarchar](255) NULL,
    public string DateTimeFormated(string format, string enclosed) {
      try {
          if (enclosed==null) {
          return DateTime.Value.ToString(format);
          } else {
          return enclosed + DateTime.Value.ToString(format) + enclosed;
          }
          
      } catch (Exception e) {
        Console.Write (e.Message);
        return null;
      }
    }
    
    }
}
