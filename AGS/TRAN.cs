using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
using ge_repository.AGS;

namespace ge_repository.OtherDatabase  {

    public class TRAN : AGSGroup {
    // CREATE TABLE [dbo].[TRAN](
    [Display(Name = "GintRecID")] public int GintRecID {get;set;} 
    //	[GintRecID] [int] IDENTITY(1,1) NOT NULL,
  	[Display(Name = "gINTProjectID")] public int gINTProjectID {get;set;} 
    //	[gINTProjectID] [int] NOT NULL,
    [Display(Name = "Tran Identifier")] public string ItemKey {get;set;} 
// 	[ItemKey] [nvarchar](255) NOT NULL,
    [Display(Name = "Tran date time")] public DateTime TRAN_DATE {get;set;} 
// 	[TRAN_DATE] [datetime] NOT NULL,
    [Display(Name = "Process")] public string Process {get;set;} 
// 	[Process] [nvarchar](255) NOT NULL,
    [Display(Name = "Tran Producer")] public string TRAN_PROD {get;set;} 
// 	[TRAN_PROD] [nvarchar](255) NOT NULL,
    [Display(Name = "Tran Stat")] public string TRAN_STAT {get;set;} 
// 	[TRAN_STAT] [nvarchar](255) NOT NULL,
    [Display(Name = "Tran Description")] public string TRAN_DESC {get;set;} 
// 	[TRAN_DESC] [ntext] NULL,
    [Display(Name = "Tran AGS")] public string TRAN_AGS {get;set;} 
// 	[TRAN_AGS] [nvarchar](255) NOT NULL,
    [Display(Name = "Tran Reciever")] public string TRAN_RECV {get;set;} 
// 	[TRAN_RECV] [nvarchar](255) NOT NULL,
    [Display(Name = "Tran Delimeter")] public string TRAN_DLIM {get;set;} 
// 	[TRAN_DLIM] [nvarchar](255) NULL,
    [Display(Name = "Tran RCON")] public string TRAN_RCON {get;set;}    
// 	[TRAN_RCON] [nvarchar](255) NULL,
    [Display(Name = "Tran Remarks")] public string TRAN_REM {get;set;} 
// 	[TRAN_REM] [ntext] NULL,
    [Display(Name = "File Set")] public string FILE_FSET {get;set;} 
// 	[FILE_FSET] [nvarchar](255) NULL,
    public TRAN() : base ("TRAN") {}
   
    public override int set_values(string[] header, string[] values) {
         try {
            for (int i=0;i<header.Length;i++) {
               if (header[i] == "TRAN_ISNO" && values[i] !="") ItemKey = values[i];
               if (header[i] == "TRAN_DATE" && values[i] !="") TRAN_DATE = Convert.ToDateTime(values[i]);
               if (header[i] == "TRAN_PROD" && values[i] !="") TRAN_PROD = values[i]; 
               if (header[i] == "TRAN_STAT" && values[i] !="") TRAN_STAT = values[i];
               if (header[i] == "TRAN_DESC" && values[i] !="") TRAN_DESC = values[i];
               if (header[i] == "TRAN_AGS" && values[i] !="") TRAN_AGS = values[i];
               if (header[i] == "TRAN_RECV" && values[i] !="") TRAN_RECV = values[i];
               if (header[i] == "TRAN_DLIM" && values[i] !="") TRAN_DLIM = values[i];
               if (header[i] == "TRAN_RCON" && values[i] !="") TRAN_RCON = values[i];
               if (header[i] == "TRAN_REM" && values[i] !="") TRAN_REM = values[i];
               if (header[i] == "FILE_FSET" && values[i] !="") FILE_FSET = values[i]; 
            }
            return 0;
         } catch {
             return -1;
         }
    }
    public override string[] get_values(string[] header, string[] unit, string[] type) {
         try {
            
            string[] ret = new string[header.Length];  
            
            for (int i=0;i<header.Length;i++) {
               if (header[i] == "HEADING") ret[i] = "DATA"; 
               if (header[i] == "TRAN_ISNO" && ItemKey != null) ret[i] = ItemKey;
               if (header[i] == "TRAN_DATE" && TRAN_DATE != null) ret[i] = TRAN_DATE.ToString(get_format(unit[i],type[i]));
               if (header[i] == "TRAN_PROD" && TRAN_PROD!= null) ret[i] = TRAN_PROD; 
               if (header[i] == "TRAN_STAT" && TRAN_STAT != null) ret[i] = TRAN_STAT;
               if (header[i] == "TRAN_DESC" && TRAN_DESC != null) ret[i] = TRAN_DESC;
               if (header[i] == "TRAN_AGS" && TRAN_AGS != null) ret[i] = TRAN_AGS;
               if (header[i] == "TRAN_RECV" && TRAN_RECV != null) ret[i] = TRAN_RECV;
               if (header[i] == "TRAN_DLIM" && TRAN_DLIM !=null) ret[i] = TRAN_DLIM;
               if (header[i] == "TRAN_RCON" && TRAN_RCON !=null) ret[i] = TRAN_RCON;
               if (header[i] == "TRAN_REM" && TRAN_REM !=null) ret[i] = TRAN_REM;
               if (header[i] == "FILE_FSET" && FILE_FSET != null) ret[i] = FILE_FSET; 
            }
            return ret;
         } catch {
             return null;
         }
        
      }  
    } 
}
