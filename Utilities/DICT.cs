using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
using ge_repository.AGS;

namespace ge_repository.OtherDatabase  {

    public class DICT : AGSGroup {

        // CREATE TABLE [dbo].[DICT](
        [Display(Name = "GintRecID")] public int GintRecID {get;set;} 
    //	[GintRecID] [int] IDENTITY(1,1) NOT NULL,
  	    [Display(Name = "gINTProjectID")] public int gINTProjectID {get;set;} 
    //	[gINTProjectID] [int] NOT NULL,
        [Display(Name = "Group Name")] public string ItemKey {get;set;} 
        // 	[ItemKey] [nvarchar](255) NOT NULL,
         [Display(Name = "Dictionary Type")] public string DICT_TYPE {get;set;} 
        // 	[DICT_TYPE] [nvarchar](255) NOT NULL,
         [Display(Name = "Heading Order")] public string Heading_Order {get;set;} 
        // 	[Heading Order] [nvarchar](255) NULL,
         [Display(Name = "Dictionary Heading")] public string DICT_HDNG {get;set;} 
        // 	[DICT_HDNG] [nvarchar](255) NULL,
         [Display(Name = "Dictionary Stats")] public string DICT_STAT {get;set;} 
        // 	[DICT_STAT] [nvarchar](255) NULL,
         [Display(Name = "DICT_DTYP")] public string DICT_DTYP {get;set;} 
        // 	[DICT_DTYP] [nvarchar](255) NULL,
         [Display(Name = "Dictionary Group")] public string DICT_PGRP {get;set;} 
        // 	[DICT_PGRP] [nvarchar](255) NULL,
         [Display(Name = "Dictionary Description")] public string DICT_DESC {get;set;} 
        // 	[DICT_DESC] [ntext] NOT NULL,
         [Display(Name = "Dictionary Unit")] public string DICT_UNIT {get;set;} 
        // 	[DICT_UNIT] [nvarchar](255) NULL,
         [Display(Name = "Dictionary Example")] public string DICT_EXMP {get;set;} 
         // 	[DICT_EXMP] [nvarchar](255) NULL,
         [Display(Name = "Dictionary Remark")] public string DICT_REM {get;set;} 
        // 	[DICT_REM] [nvarchar](255) NULL,
         [Display(Name = "File FSet")] public string FILE_FSET {get;set;} 
        // 	[FILE_FSET] [nvarchar](255) NULL,
        public DICT() : base ("DICT") {}
    
        public override int set_values(string[] header, string[] values) {
         try {
            for (int i=0;i<header.Length;i++) { 
                if (header[i] == "DICT_DESC") DICT_DESC = values[i];
                if (header[i] == "DICT_DTYP") DICT_DTYP = values[i];
                if (header[i] == "DICT_HDNG") DICT_HDNG = values[i];
                if (header[i] == "DICT_PGRP") DICT_PGRP = values[i];
                if (header[i] == "DICT_REM") DICT_REM = values[i];
                if (header[i] == "DICT_STAT") DICT_STAT = values[i];
                if (header[i] == "DICT_TYPE") DICT_TYPE = values[i];
                if (header[i] == "DICT_UNIT") DICT_UNIT = values[i];
                if (header[i] == "FILE_FSET") FILE_FSET = values[i];
            }
         } catch {
             return -1;
         }
         
         return 0;
        }
    }
}
