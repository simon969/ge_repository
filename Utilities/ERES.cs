using System;
using System.ComponentModel.DataAnnotations;
using ge_repository.AGS;

namespace ge_repository.OtherDatabase  {

    public class ERES : AGSGroup {
        
// CREATE TABLE [dbo].[ERES](
    [Display(Name = "GintRecID")] public int GintRecID {get;set;} 
//	[GintRecID] [int] IDENTITY(1,1) NOT NULL,
    [Display(Name = "gINTProjectID")] public int gINTProjectID {get;set;} 
//	[gINTProjectID] [int] NOT NULL,
    [Display(Name = "PointID")] public string PointID {get;set;}     
// 	[PointID] [nvarchar](255) NOT NULL,
    [Display(Name = "SAMP_TOP")] public double SAMP_Depth {get;set;}     
// 	[SAMP_Depth] [float] NOT NULL,
    [Display(Name = "SAMP_REF")] public string SAMP_REF {get;set;}  
// 	[SAMP_REF] [nvarchar](255) NULL,
    [Display(Name = "SAMP_TYPE")] public string SAMP_TYPE {get;set;}   
// 	[SAMP_TYPE] [nvarchar](255) NULL,
    [Display(Name = "SAMP_ID")] public string SAMP_ID {get;set;}  
// 	[SAMP_ID] [nvarchar](255) NULL,
    [Display(Name = "SPEC_DPTH")] public double Depth {get;set;}  
// 	[Depth] [float] NOT NULL,
    [Display(Name = "SPEC_REF")] public string SPEC_REF {get;set;}  
// 	[SPEC_REF] [nvarchar](255) NULL,
   [Display(Name = "ERES_CODE")] public string ItemKey {get;set;}  
// 	[ItemKey] [nvarchar](255) NOT NULL,
   [Display(Name = "ERES_METH")] public string ERES_METH {get;set;}  
// 	[ERES_METH] [nvarchar](255) NULL,
   [Display(Name = "Matrix-Run Type")] public string Matrix_Run_Type {get;set;}  
// 	[Matrix-Run Type] [nvarchar](255) NULL,
   [Display(Name = "ERES_MATX")] public string ERES_MATX {get;set;}  
// 	[ERES_MATX] [nvarchar](255) NOT NULL,
   [Display(Name = "ERES_RTYP")] public string ERES_RTYP {get;set;}  
// 	[ERES_RTYP] [nvarchar](255) NULL,
   [Display(Name = "ERES_TESN")] public string ERES_TESN {get;set;}  
// 	[ERES_TESN] [nvarchar](255) NULL,
   [Display(Name = "ERES_NAME")] public string ERES_NAME {get;set;}  
// 	[ERES_NAME] [nvarchar](255) NULL,
   [Display(Name = "ERES_TNAM")] public string ERES_TNAM {get;set;}  
// 	[ERES_TNAM] [nvarchar](255) NULL,
   [Display(Name = "ERES_RVAL")] public double? ERES_RVAL {get;set;}  
// 	[ERES_RVAL] [float] NULL,
   [Display(Name = "ERES_RUNI")] public string ERES_RUNI {get;set;}  
// 	[ERES_RUNI] [nvarchar](255) NOT NULL,
   [Display(Name = "ERES_RTXT")] public string ERES_RTXT {get;set;}  
// 	[ERES_RTXT] [nvarchar](255) NULL,
   [Display(Name = "ERES_RTCD")] public string ERES_RTCD {get;set;}  
// 	[ERES_RTCD] [nvarchar](255) NULL,
   [Display(Name = "ERES_RRES")] public Boolean? ERES_RRES {get;set;}  
// 	[ERES_RRES] [bit] NULL,
   [Display(Name = "ERES_DETF")] public Boolean? ERES_DETF {get;set;}  
// 	[ERES_DETF] [bit] NULL,
   [Display(Name = "ERES_ORG")] public Boolean? ERES_ORG {get;set;}  
// 	[ERES_ORG] [bit] NULL,
   [Display(Name = "ERES_IQLF")] public string ERES_IQLF {get;set;}  
// 	[ERES_IQLF] [nvarchar](255) NULL,
   [Display(Name = "ERES_LQLF")] public string ERES_LQLF {get;set;}  
// 	[ERES_LQLF] [nvarchar](255) NULL,
   [Display(Name = "ERES_RDLM")] public double? ERES_RDLM {get;set;}  
// 	[ERES_RDLM] [float] NULL,
   [Display(Name = "ERES_MDLM")] public double? ERES_MDLM {get;set;}  
// 	[ERES_MDLM] [float] NULL,
   [Display(Name = "ERES_QLM")] public double? ERES_QLM {get;set;}  
// 	[ERES_QLM] [float] NULL,
   [Display(Name = "ERES_DUNI")] public string ERES_DUNI {get;set;}  
// 	[ERES_DUNI] [nvarchar](255) NULL,
   [Display(Name = "ERES_TPICP")] public int? ERES_TPICP {get;set;}  
// 	[ERES_TICP] [smallint] NULL,
   [Display(Name = "ERES_TICT")] public int? ERES_TICT {get;set;}  
// 	[ERES_TICT] [smallint] NULL,
   [Display(Name = "ERES_RDAT")] public DateTime? ERES_RDAT {get;set;}  
// 	[ERES_RDAT] [datetime] NULL,
   [Display(Name = "ERES_SGRP")] public string ERES_SGRP {get;set;}  
// 	[ERES_SGRP] [nvarchar](255) NULL,
   [Display(Name = "SPEC_DESC")] public string SPEC_DESC {get;set;}  
// 	[SPEC_DESC] [ntext] NULL,
   [Display(Name = "SPEC_PREP")] public string SPEC_PREP {get;set;}  
// 	[SPEC_PREP] [ntext] NULL,
   [Display(Name = "ERES_DTIM")] public DateTime? ERES_DTIM {get;set;}  
// 	[ERES_DTIM] [datetime] NULL,
   [Display(Name = "ERES_TEST")] public string ERES_TEST {get;set;}  
// 	[ERES_TEST] [nvarchar](255) NULL,
   [Display(Name = "ERES_TORD")] public string ERES_TORD {get;set;}  
// 	[ERES_TORD] [nvarchar](255) NULL,
   [Display(Name = "ERES_LOCN")] public string ERES_LOCN {get;set;}  
// 	[ERES_LOCN] [nvarchar](255) NULL,
   [Display(Name = "ERES_BAS")] public string ERES_BAS {get;set;}  
// 	[ERES_BAS] [nvarchar](255) NULL,
   [Display(Name = "ERES_DIL")] public int? ERES_DIL {get;set;}  
// 	[ERES_DIL] [int] NULL,
   [Display(Name = "ERES_LMTH")] public string ERES_LMTH {get;set;}  
// 	[ERES_LMTH] [nvarchar](255) NULL,
   [Display(Name = "ERES_LDTM")] public DateTime? ERES_LDTM {get;set;}  
// 	[ERES_LDTM] [datetime] NULL,
   [Display(Name = "ERES_IREF")] public string ERES_IREF {get;set;}  
// 	[ERES_IREF] [nvarchar](255) NULL,
   [Display(Name = "ERES_SIZE")] public string ERES_SIZE {get;set;}  
// 	[ERES_SIZE] [smallint] NULL,
   [Display(Name = "ERES_PERP")] public double? ERES_PERP {get;set;}  
// 	[ERES_PERP] [float] NULL,
   [Display(Name = "ERES_REM")] public string ERES_REM {get;set;}  
// 	[ERES_REM] [ntext] NULL,
   [Display(Name = "ERES_LAB")] public string ERES_LAB {get;set;}  
// 	[ERES_LAB] [nvarchar](255) NULL,
   [Display(Name = "ERES_CRED")] public string ERES_CRED {get;set;}  
// 	[ERES_CRED] [nvarchar](255) NULL,
   [Display(Name = "TEST_STAT")] public string TEST_STAT {get;set;}  
// 	[TEST_STAT] [nvarchar](255) NULL,
   [Display(Name = "FILE_FSET")] public string FILE_FSET {get;set;}  
// 	[FILE_FSET] [nvarchar](255) NULL,
    [Display(Name = "ge_source Addition field (NON AGS)")] 
    public string ge_source {get;set;}
    
    [Display(Name = "ge_otherId Addition field for ESRI feature update (NON AGS)")] 
    public string ge_otherId {get;set;}
 
    [Display (Name = "Survey Round Ref")] 
    public string RND_REF {get;set;}
	  // RND_REF [nvarchar](255) NULL,
    public ERES () : base ("ERES") {}

     public override int set_values(string[] header, string[] values) {
         try {
            for (int i=0;i<header.Length;i++) {
                if (header[i] == "LOCA_ID" && values[i] != "") PointID = values[i];
                if (header[i] == "SAMP_TOP" && values[i] != "") SAMP_Depth = Convert.ToDouble(values[i]);
                if (header[i] == "SAMP_REF" && values[i]!= "") SAMP_REF = values[i];
                if (header[i] == "SAMP_TYPE" && values[i] != "") SAMP_TYPE = values[i];
                if (header[i] == "SAMP_ID" && values[i] != "") SAMP_ID = values[i];
                if (header[i] == "SPEC_DPTH" && values[i] != "") Depth = Convert.ToDouble(values[i]);
                if (header[i] == "SPEC_REF" && values[i] != "") SPEC_REF = values[i];
                if (header[i] == "ERES_CODE" && values[i] != "") ItemKey = values[i];
                if (header[i] == "ERES_METH" && values[i] != "") ERES_METH = values[i];
                if (header[i] == "Matrix-Run Type" && values[i] != "") Matrix_Run_Type= values[i];
                if (header[i] == "ERES_MATX" && values[i] != "") ERES_MATX = values[i];
                if (header[i] == "ERES_RTYP" && values[i] != "") ERES_RTYP = values[i];
                if (header[i] == "ERES_TESN" && values[i] != "") ERES_TESN = values[i]; 
                if (header[i] == "ERES_NAME" && values[i] != "") ERES_NAME = values[i]; 
                if (header[i] == "ERES_TNAM" && values[i] != "") ERES_TNAM = values[i]; 
                if (header[i] == "ERES_RVAL" && values[i] != "") ERES_RVAL = Convert.ToDouble(values[i]); 
                if (header[i] == "ERES_RUNI" && values[i] != "") ERES_RUNI = values[i]; 
                if (header[i] == "ERES_RTXT" && values[i] != "") ERES_RTXT = values[i]; 
                if (header[i] == "ERES_RTCD" && values[i] != "") ERES_RTCD = values[i]; 
                if (header[i] == "ERES_RRES" && values[i] != "") ERES_RRES = Convert.ToBoolean(values[i]);  
                if (header[i] == "ERES_DETF" && values[i] != "") ERES_DETF = Convert.ToBoolean(values[i]); 
                if (header[i] == "ERES_ORG" && values[i] != "") ERES_ORG = Convert.ToBoolean(values[i]); 
                if (header[i] == "ERES_IQLF" && values[i] != "") ERES_IQLF = values[i]; 
                if (header[i] == "ERES_LQLF" && values[i] != "") ERES_LQLF = values[i]; 
                if (header[i] == "ERES_RDLM" && values[i] != "") ERES_RDLM =  Convert.ToDouble(values[i]); 
                if (header[i] == "ERES_MDLM" && values[i] != "") ERES_MDLM = Convert.ToDouble(values[i]); 
                if (header[i] == "ERES_QLM" && values[i] != "") ERES_QLM =  Convert.ToDouble(values[i]); 
                if (header[i] == "ERES_DUNI" && values[i] != "") ERES_DUNI = values[i]; 
                if (header[i] == "ERES_TPICP" && values[i] != "") ERES_TPICP = Convert.ToInt16(values[i]); 
                if (header[i] == "ERES_TICT" && values[i] != "") ERES_TICT = Convert.ToInt16(values[i]); 
                if (header[i] == "ERES_RDAT" && values[i] != "") ERES_RDAT = Convert.ToDateTime(values[i]); 
                if (header[i] == "ERES_SGRP" && values[i] != "") ERES_SGRP = values[i]; 
                if (header[i] == "SPEC_DESC" && values[i] != "") SPEC_DESC = values[i]; 
                if (header[i] == "SPEC_PREP" && values[i] != "") SPEC_PREP = values[i]; 
                if (header[i] == "ERES_DTIM" && values[i] != "") ERES_DTIM = Convert.ToDateTime(values[i]); 
                if (header[i] == "ERES_TEST" && values[i] != "") ERES_TEST = values[i]; 
                if (header[i] == "ERES_TORD" && values[i] != "") ERES_TORD = values[i]; 
                if (header[i] == "ERES_LOCN" && values[i] != "") ERES_LOCN = values[i]; 
                if (header[i] == "ERES_BAS" && values[i] != "") ERES_BAS = values[i]; 
                if (header[i] == "ERES_DIL" && values[i] != "") ERES_DIL = Convert.ToInt16(values[i]); 
                if (header[i] == "ERES_LMTH" && values[i] != "") ERES_LMTH = values[i]; 
                if (header[i] == "ERES_LDTM" && values[i] != "") ERES_LDTM = Convert.ToDateTime(values[i]); 
                if (header[i] == "ERES_IREF" && values[i] != "") ERES_IREF = values[i]; 
                if (header[i] == "ERES_SIZE" && values[i] != "") ERES_SIZE = values[i]; 
                if (header[i] == "ERES_PERP" && values[i] != "") ERES_PERP = Convert.ToDouble(values[i]); 
                if (header[i] == "ERES_REM" && values[i] != "") ERES_REM = values[i]; 
                if (header[i] == "ERES_LAB" && values[i] != "") ERES_LAB = values[i]; 
                if (header[i] == "ERES_CRED" && values[i] != "") ERES_CRED = values[i]; 
                if (header[i] == "TEST_STAT" && values[i] != "") TEST_STAT = values[i]; 
                if (header[i] == "FILE_FSET") FILE_FSET= values[i];
            }

         } catch {
             return -1;
         }
         
         return 0;
        }
    }
    
}
