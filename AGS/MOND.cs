
using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using ge_repository.interfaces;
using ge_repository.AGS;

namespace ge_repository.OtherDatabase  {

    public class MOND : AGSGroup, IGintTable {

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
 
    [Display (Name = "Additional field Survey Round Ref (NON AGS)")] 
    	public string RND_REF {get;set;}
	  // RND_REF [nvarchar](255) NULL,
    [Display (Name = "Additional field for update tracking (NON AGS)")] 
    	public string ge_updates {get;set;}
	  // RND_REF [nvarchar](255) NULL,
    public MOND() : base ("MOND") {}

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
   
     public override int set_values(string[] header, string[] values) {
         try {
            for (int i=0;i<header.Length;i++) {
                if (header[i] == "LOCA_ID" && values[i] != "") PointID = values[i];
                if (header[i] == "MONG_ID" && values[i] != "") ItemKey = values[i];
                if (header[i] == "MOND_CONT" && values[i] != "") MOND_CONT = values[i];
                if (header[i] == "MOND_CRED" && values[i] != "") MOND_CRED = values[i];
                if (header[i] == "MOND_DTIM" && values[i] != "") DateTime = Convert.ToDateTime(values[i]); 
                if (header[i] == "MOND_INST" && values[i] != "") MOND_INST = values[i];
                if (header[i] == "MOND_LIM" && values[i] != "") MOND_LIM = Convert.ToDouble(values[i]);
                if (header[i] == "MOND_METH" && values[i] != "") MOND_METH = values[i];
                if (header[i] == "MOND_NAME" && values[i] != "") MOND_NAME = values[i];
                if (header[i] == "MOND_RDNG" && values[i] != "") MOND_RDNG = values[i]; 
                if (header[i] == "MOND_REF" && values[i] != "") MOND_REF = values[i];
                if (header[i] == "MOND_REM" && values[i] != "") MOND_REM = values[i];
                if (header[i] == "MOND_TYPE" && values[i] != "") MOND_TYPE = values[i];
                if (header[i] == "MOND_LIM" && values[i] != "") MOND_ULIM = Convert.ToDouble(values[i]);
                if (header[i] == "MOND_UNIT" && values[i] != "") MOND_UNIT = values[i];
                if (header[i] == "MONG_DIS" && values[i] != "") MONG_DIS = Convert.ToDouble(values[i]); 
                if (header[i] == "FILE_FSET" && values[i] !="") FILE_FSET = values[i];
                
                //Non standard LTC  fields
                if (header[i] == "ge_source" && values[i] !="") ge_source = values[i];
                if (header[i] == "ge_otherId" && values[i] !="") ge_otherId = values[i];
                if (header[i] == "RND_REF" && values[i] !="") RND_REF = values[i];
               
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
                if (header[i] == "MONG_ID" && ItemKey !=null) ret[i] = ItemKey;
                if (header[i] == "MOND_CONT" && MOND_CONT != null) ret[i] = MOND_CONT;
                if (header[i] == "MOND_CRED" && MOND_CRED!= null) ret[i] = MOND_CRED;
                if (header[i] == "MOND_DTIM" && DateTime!= null) ret[i] = DateTime.Value.ToString(get_format(unit[i],type[i])); ;
                if (header[i] == "MOND_INST" && MOND_INST != null) ret[i] = MOND_INST;
                if (header[i] == "MOND_LIM" && MOND_LIM != null) ret[i] = String.Format(get_format(unit[i],type[i]), MOND_LIM.Value); 
                if (header[i] == "MOND_METH" && MOND_METH != null) ret[i] = MOND_METH;
                if (header[i] == "MOND_NAME" && MOND_NAME != null) ret[i] = MOND_NAME;
                if (header[i] == "MOND_RDNG" && MOND_RDNG != null) ret[i] = MOND_RDNG; 
                if (header[i] == "MOND_REF" &&  MOND_REF != null) ret[i] = MOND_REF;
                if (header[i] == "MOND_REM" && MOND_REM != null) ret[i] = MOND_REM;
                if (header[i] == "MOND_TYPE" && MOND_TYPE != null) ret[i] = MOND_TYPE;
                if (header[i] == "MOND_ULIM" && MOND_ULIM != null) ret[i] = String.Format(get_format(unit[i],type[i]), MOND_ULIM.Value); 
                if (header[i] == "MOND_UNIT" && MOND_UNIT!= null) ret[i] = MOND_UNIT;
                if (header[i] == "MONG_DIS" && MONG_DIS != null) ret[i] = String.Format(get_format(unit[i],type[i]),MONG_DIS); 
                if (header[i] == "FILE_FSET" && FILE_FSET != null) ret[i] = FILE_FSET;
                
                //Non standard LTC  fields
                if (header[i] == "ge_source" && ge_source !="") ret[i] = ge_source;
                if (header[i] == "ge_otherId" && ge_otherId  !="") ret[i] = ge_otherId;
                if (header[i] == "RND_REF" && RND_REF !="") ret[i] = RND_REF;

                
            }
            return ret;
         } catch {
             return null;
         }
    }
    public void set_values (DataRow row) {
            row["gINTProjectID"] = gINTProjectID;
            row["PointID"] = PointID;
            row["MOND_REF"] = MOND_REF;
            row["ItemKey"] = ItemKey;
            if (MONG_DIS == null) {row["MONG_DIS"] = DBNull.Value;} else {row["MONG_DIS"] = MONG_DIS;}
            row["DateTime"] = DateTime;    
            row["MOND_TYPE"] = MOND_TYPE;
            row["MOND_RDNG"] = MOND_RDNG;
            row["MOND_NAME"] = MOND_NAME;
            row["MOND_UNIT"] = MOND_UNIT;
            row["MOND_REM"] = MOND_REM; 
            row["MOND_CONT"] = MOND_CONT;
            row["MOND_INST"] = MOND_INST;
            row["MOND_CRED"] = MOND_CRED;
            if (MOND_LIM == null) {row["MOND_LIM"] = DBNull.Value;} else {row["MOND_LIM"] = MOND_LIM;}
            row["MOND_METH"] = MOND_METH;
            if (MOND_ULIM == null) {row["MOND_ULIM"] = DBNull.Value;} else {row["MOND_ULIM"] = MOND_ULIM;}
            row["FILE_FSET"] = FILE_FSET;

            //Non standard LTC  fields
            row["ge_source"] = ge_source;
            row["ge_otherId"] = ge_otherId;
            row["RND_REF"] = RND_REF;
    }

     public static explicit operator MOND (DataRow row)
    {
        MOND output = new MOND() ;

        output.gINTProjectID  = (int) row["gINTProjectID"];
        output.PointID  = (string) row["PointID"] ;
        output.MOND_REF = (string) row["MOND_REF"] ;
        output.ItemKey = (string) row["ItemKey"];
        if (row["MONG_DIS"] == DBNull.Value) {output.MONG_DIS = null;} else {output.MONG_DIS = (float) row["MONG_DIS"];}
        output.DateTime = (DateTime) row["DateTime"];    
        output.MOND_TYPE = (string) row["MOND_TYPE"];
        output.MOND_RDNG = (string) row["MOND_RDNG"];
        output.MOND_NAME = (string) row["MOND_NAME"];
        output.MOND_UNIT= (string) row["MOND_UNIT"];
        output.MOND_REM = (string) row["MOND_REM"]; 
        output.MOND_CONT= (string) row["MOND_CONT"];
        output.MOND_INST= (string) row["MOND_INST"];
        output.MOND_CRED= (string) row["MOND_CRED"];
        if (row["MOND_LIM"] == DBNull.Value) {output.MOND_LIM = null;} else {output.MOND_LIM = (float) row["MOND_LIM"];}
        output.MOND_METH = (string) row["MOND_METH"];
        if (row["MOND_ULIM"] == DBNull.Value) {output.MOND_ULIM = null;} else {output.MOND_ULIM = (float) row["MOND_ULIM"];}
        output.FILE_FSET = (string) row["FILE_FSET"];

        //Non standard LTC  fields
        output.ge_source = (string) row["ge_source"];
        output.ge_otherId = (string) row["ge_otherId"];
        output.RND_REF = (string) row["RND_REF"];
        return output;
    }


    }


}
