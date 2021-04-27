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
public override int set_values(string[] header, string[] values) {
         try {
            for (int i=0;i<header.Length;i++) {
                if (header[i] == "LOCA_ID" && values[i] != "") PointID = values[i];
				if (header[i] == "MONG_ID" && values[i] != "") ItemKey = values[i];
                if (header[i] == "MONG_BRGA" && values[i] != "") MONG_BRGA = Convert.ToInt16(values[i]);
                if (header[i] == "MONG_BRGB" && values[i]!= "") MONG_BRGB = Convert.ToInt16(values[i]);
                if (header[i] == "MONG_BRGC" && values[i] != "") MONG_BRGC = Convert.ToInt16(values[i]);
                if (header[i] == "MONG_BRZ" && values[i] != "") MONG_BRZ = Convert.ToDouble(values[i]);
                if (header[i] == "MONG_CONT" && values[i] != "") MONG_CONT = values[i];
                if (header[i] == "MONG_DATE" && values[i] != "") MONG_DATE = Convert.ToDateTime(values[i]);
                if (header[i] == "MONG_DETL" && values[i] != "") MONG_DETL = values[i]; 
                if (header[i] == "MONG_DIS" && values[i] != "") MONG_DIS = Convert.ToDouble(values[i]);
				if (header[i] == "MONG_INCA" && values[i] != "") MONG_INCA = Convert.ToInt16(values[i]);
                if (header[i] == "MONG_INCB" && values[i]!= "") MONG_INCB = Convert.ToInt16(values[i]);
                if (header[i] == "MONG_INCC" && values[i] != "") MONG_INCC = Convert.ToInt16(values[i]);
                if (header[i] == "MONG_REM" && values[i] != "") MONG_REM = values[i];
				if (header[i] == "MONG_RSCA" && values[i] != "") MONG_RSCA = values[i];
                if (header[i] == "MONG_RSCB" && values[i]!= "") MONG_RSCB = values[i];
                if (header[i] == "MONG_RSCC" && values[i] != "") MONG_RSCC = values[i];
                if (header[i] == "MONG_TRZ" && values[i] != "") MONG_TRZ = Convert.ToDouble(values[i]);
                if (header[i] == "MONG_TYPE" && values[i] != "") MONG_TYPE = values[i];
                if (header[i] == "PIPE_REF" && values[i] != "") PIPE_REF = values[i];
                if (header[i] == "FILE_FSET" && values[i] !="") FILE_FSET = values[i];
            }

         } catch {
             return -1;
         }
         
         return 0;
        }
    public override string[] get_values(string[] header, string[] unit, string[] type) {
    
         try {
            
            string[] ret = new string[header.Length];  
            
            for (int i=0;i<header.Length;i++) {
                if (header[i] == "HEADING") ret[i] = "DATA";
                if (header[i] == "LOCA_ID" && PointID != null) ret[i] = PointID;
				if (header[i] == "MONG_ID" && ItemKey != null) ret[i] = ItemKey;
                if (header[i] == "MONG_BRGA" && MONG_BRGA != null) ret[i] = String.Format(get_format(unit[i],type[i]), MONG_BRGA.Value);
                if (header[i] == "MONG_BRGB" && MONG_BRGB != null) ret[i] = String.Format(get_format(unit[i],type[i]),  MONG_BRGB.Value);
                if (header[i] == "MONG_BRGC" && MONG_BRGC != null) ret[i] = String.Format(get_format(unit[i],type[i]), MONG_BRGC.Value);
                if (header[i] == "MONG_BRZ" && MONG_BRZ != null) ret[i] = String.Format(get_format(unit[i],type[i]), MONG_BRZ.Value); 
                if (header[i] == "MONG_CONT" && MONG_CONT != null) ret[i] = MONG_CONT;
                if (header[i] == "MONG_DATE" && MONG_DATE != null) ret[i] = MONG_DATE.Value.ToString(get_format(unit[i],type[i])); 
                if (header[i] == "MONG_DETL" && MONG_DETL != null) ret[i] = MONG_DETL; 
                if (header[i] == "MONG_DIS" &&  MONG_DIS != null) ret[i] = String.Format(get_format(unit[i],type[i]), MONG_DIS.Value);
                if (header[i] == "MONG_INCA" && MONG_INCA != null) ret[i] = String.Format(get_format(unit[i],type[i]), MONG_INCA.Value);
                if (header[i] == "MONG_INCB" && MONG_INCB != null) ret[i] = String.Format(get_format(unit[i],type[i]),  MONG_INCB.Value);
                if (header[i] == "MONG_INCC" && MONG_INCC != null) ret[i] = String.Format(get_format(unit[i],type[i]), MONG_INCC.Value);
				if (header[i] == "MONG_REM" && MONG_REM != null) ret[i] = MONG_REM;
                if (header[i] == "MONG_RSCA" && MONG_RSCA != null) ret[i] = MONG_RSCA;
                if (header[i] == "MONG_RSCB" && MONG_RSCB != null) ret[i] = MONG_RSCB;
                if (header[i] == "MONG_RSCC" && MONG_RSCC != null) ret[i] = MONG_RSCC;
				if (header[i] == "MONG_TRZ" && MONG_TRZ != null) ret[i] = String.Format(get_format(unit[i],type[i]), MONG_TRZ.Value); 
               	if (header[i] == "MONG_TYPE" && MONG_TYPE != null) ret[i] = MONG_TYPE;
                if (header[i] == "MONG_DIS" && MONG_DIS != null) ret[i] = String.Format(get_format(unit[i],type[i]),MONG_DIS); 
				if (header[i] == "PIPE_REF" && PIPE_REF != null) ret[i] = PIPE_REF;
                if (header[i] == "FILE_FSET" && FILE_FSET != null) ret[i] = FILE_FSET;
            }
            return ret;
         } catch {
             return null;
         }
    }
    }

}