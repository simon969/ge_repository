using System;
using System.ComponentModel.DataAnnotations;

namespace ge_repository.OtherDatabase  {

    public class ERES {
        
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

    }
    
}
