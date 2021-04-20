using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using ge_repository.interfaces;
using ge_repository.AGS;
namespace ge_repository.OtherDatabase  {

    public class MONG : AGSGroup, IGintTable {


// CREATE TABLE [dbo].[MONG](
	[Key] [Display(Name = "GintRecID")] public int GintRecID {get;set;} 
//	[GintRecID] [int] IDENTITY(1,1) NOT NULL,
  	[Display(Name = "gINTProjectID")] public int gINTProjectID {get;set;} 
//	[gINTProjectID] [int] NOT NULL,
    [Display(Name = "PointID")] public string PointID {get;set;} 
	// [PointID] [nvarchar](255) NOT NULL,
	[Display(Name = "ItemKey")] public string ItemKey {get;set;} 
//  [ItemKey] [nvarchar](255) NULL,
	[Display(Name = "Distance")] public double? MONG_DIS {get;set;} 
//	[MONG_DIS] [float] NULL,
	[Display(Name = "Pipe Reference")] public string PIPE_REF {get;set;} 
//	[PIPE_REF] [nvarchar](255) NULL,
	[Display(Name = "Installed DateTime")] public DateTime? MONG_DATE {get;set;} 
//	[MONG_DATE] [datetime] NULL,
	[Display(Name = "Installation Type")] public string MONG_TYPE {get;set;} 
//	[MONG_TYPE] [nvarchar](255) NULL,
	[Display(Name = "Installation Details")] public string MONG_DETL {get;set;} 
//	[MONG_DETL] [nvarchar](255) NULL,
	[Display(Name = "Top of Response Zone")] public double? MONG_TRZ {get;set;} 
//	[MONG_TRZ] [float] NULL,
	[Display(Name = "Base of Response Zone")] public double? MONG_BRZ {get;set;} 
//	[MONG_BRZ] [float] NULL,
	[Display(Name = "Bearing of monitoring axis A (compass bearing)")] public int? MONG_BRGA {get;set;} 
//	[MONG_BRGA] [smallint] NULL,
	[Display(Name = "Bearing of monitoring axis B (compass bearing)")] public int? MONG_BRGB {get;set;} 
//	[MONG_BRGB] [smallint] NULL,
	[Display(Name = "Bearing of monitoring axis C (compass bearing)")] public int? MONG_BRGC {get;set;} 
//	[MONG_BRGC] [smallint] NULL,
	[Display(Name = "Inclination of instrument axis A (measured positively down from horizontal)")] public int? MONG_INCA {get;set;} 
//	[MONG_INCA] [smallint] NULL,
	[Display(Name = "Inclination of instrument axis B (measured positively down from horizontal)")] public int? MONG_INCB {get;set;} 
//	[MONG_INCB] [smallint] NULL,
	[Display(Name = "Inclination of instrument axis C (measured positively down from horizontal)")] public int? MONG_INCC {get;set;} 
//	[MONG_INCC] [smallint] NULL,
	[Display(Name = "Reading sign convention in direction A")] public string MONG_RSCA {get;set;} 
//	[MONG_RSCA] [nvarchar](255) NULL,
	[Display(Name = "Reading sign convention in direction B")] public string MONG_RSCB {get;set;} 
//	[MONG_RSCB] [nvarchar](255) NULL,
	[Display(Name = "Reading sign convention in direction C")] public string MONG_RSCC {get;set;} 
//	[MONG_RSCC] [nvarchar](255) NULL,
	[Display(Name = "Name of testing organisation")] public string MONG_CONT {get;set;} 
//	[MONG_CONT] [nvarchar](255) NULL,
	[Display(Name = "Remarks")] public string MONG_REM {get;set;} 
//	[MONG_REM] [ntext] NULL,
	[Display(Name = "Associated file reference")] public string FILE_FSET {get;set;} 
//	[FILE_FSET] [nvarchar](255) NULL,

	public MONG() : base ("MONG") {}

	public void set_values (DataRow row) {




    }

    }

}