
using System;
using System.Collections.Generic;
using ge_repository.ESRI;
using ge_repository.OtherDatabase;
using ge_repository.services;
using static ge_repository.LowerThamesCrossing.LTC;

namespace ge_repository.LowerThamesCrossing {

public class LTM_Survey_Data_Repeat2 : IEsriChild {

    public int objectid {get;set;}	// objectid, esriFieldTypeOID, ObjectID, sqlTypeOther, 
    public Guid globalid {get;set;}	// globalid, esriFieldTypeGlobalID, GlobalID, sqlTypeGUID, 38
    public string gas_bh_ref {get;set;}	// gas_bh_ref, esriFieldTypeString, gas_bh_ref, sqlTypeOther, 255
    public string gas_date_ref {get;set;}	// gas_date_ref, esriFieldTypeDate, gas_date_ref, sqlTypeOther, 255
    public int? elapse_t {get;set;}	// elapse_t, esriFieldTypeInteger, Elapsed time (seconds), sqlTypeOther, 
    public double? CH4_lel_t {get;set;}	// CH4_lel_t, esriFieldTypeDouble, CH4 LEL (%), sqlTypeOther, 
    public double? CH4_t {get;set;}	// CH4_t, esriFieldTypeDouble, CH4 (% v/v), sqlTypeOther, 
    public double? CO2_t {get;set;}	// CO2_t, esriFieldTypeDouble, CO2 (% v/v), sqlTypeOther, 
    public double? O2_t {get;set;}	// O2_t, esriFieldTypeDouble, O2 (% v/v), sqlTypeOther, 
    public double? H2S_t {get;set;}	// H2S_t, esriFieldTypeDouble, H2S (ppm), sqlTypeOther, 
    public double? CO_t {get;set;}	// CO_t, esriFieldTypeDouble, CO (ppm), sqlTypeOther, 
    public double? voc_t {get;set;}	// voc_t, esriFieldTypeDouble, VOC (ppm), sqlTypeOther, 
    public Guid parentglobalid {get;set;}	// parentglobalid, esriFieldTypeGUID, ParentGlobalID, sqlTypeGUID, 38
    public long? CreationDate {get;set;}	// CreationDate, esriFieldTypeDate, CreationDate, sqlTypeOther, 8
    public DateTime? CreationDate_getDT() {if (CreationDate==null) {return null;} return Esri.getDate(CreationDate.Value);}
    public void CreationDate_setDT(DateTime? value) { if (value==null){CreationDate=null; return;} CreationDate = Esri.getEpoch(value.Value);} 
    public string Creator {get;set;}	// Creator, esriFieldTypeString, Creator, sqlTypeOther, 128
    public long? EditDate {get;set;}	// EditDate, esriFieldTypeDate, EditDate, sqlTypeOther, 8
    public DateTime? EditDate_getDT() {if (EditDate==null) {return null;} return Esri.getDate(EditDate.Value);}
    public void EditDate_setDT(DateTime? value) { if (value==null){EditDate=null; return;} EditDate = Esri.getEpoch(value.Value);}  
    public string Editor {get;set;}	// Editor, esriFieldTypeString, Editor, sqlTypeOther, 128

    public int AddGas(MONG mg, LTM_Survey_Data2 survey, LTM_Survey_Data_Repeat2 survey2, List<MOND> MOND) {
                
            DateTime survey_startDT = gINTDateTime(survey.date1_getDT()).Value;

                  
                if (survey2.elapse_t == null) {
                    return 0;
                }

                int elapsed = survey2.elapse_t.Value;
                
                //DateTime dt = survey_start.Value.AddSeconds(elapsed);
                DateTime dt = survey_startDT.AddSeconds(elapsed);
                
                if (survey.gas_repeat_tstart!=null) {
                dt  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value.AddSeconds(elapsed);
                }

                // Gas flow (l/h)
                // if (survey2.gas_flow_t != null) {
                //     MOND md = NewMOND(mg, survey, survey2);
                //     md.MOND_TYPE = "GFLO";
                //     md.MOND_RDNG = Convert.ToString(survey2.gas_flow_t);
                //     md.MOND_NAME = "Gas flow rate";
                //     md.MOND_UNIT = "l/h";
                //     md.MOND_INST = survey.gas_instr;
                //     md.DateTime = dt; 
                //     MOND.Add(md);  
                // }
                
                // Methane reading Limit CH4 LEL (%)
                if (survey2.CH4_lel_t != null) {
                    MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "GM";
                    md.MOND_RDNG = Convert.ToString(survey2.CH4_lel_t);
                    md.MOND_NAME = "Methane as percentage of LEL (Lower Explosive Limit)";
                    md.MOND_UNIT = "%vol";
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                    md.DateTime = dt; 
                    MOND.Add(md);
                }

                // Methane reading CH4 (% v/v)
                if (survey2.CH4_t != null) {
                      MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "TGM";
                    md.MOND_RDNG = Convert.ToString(survey2.CH4_t);
                    md.MOND_NAME = "Total Methane";
                    md.MOND_UNIT = "%vol";
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                    md.DateTime = dt; 
                    MOND.Add(md);  
                }

                // Carbon Dioxide reading CO2 (% v/v)
                if (survey2.CO2_t != null) {
                   MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "GCD";
                    md.MOND_RDNG = Convert.ToString(survey2.CO2_t);
                    md.MOND_NAME = "Carbon Dioxide";
                    md.MOND_UNIT = "%vol";
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                    md.DateTime = dt; 
                    MOND.Add(md);  
                }

                //Oxygen Reading O2 (% v/v)
                if (survey2.O2_t != null) {
                    MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "GOX";
                    md.MOND_RDNG = Convert.ToString(survey2.O2_t);
                    md.MOND_NAME = "Oxygen";
                    md.MOND_UNIT = "%vol";
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                    md.DateTime = dt;  
                    MOND.Add(md);  
                }

                //Hydrogen Sulphide H2S (ppm)
                if (survey2.H2S_t != null) {
                    MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "HYS";
                    md.MOND_RDNG = Convert.ToString(survey2.H2S_t);
                    md.MOND_NAME = "Hydrogen Sulphide";
                    md.MOND_UNIT = "ppm";
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                    md.DateTime = dt;  
                    MOND.Add(md);  
                }

                //Carbon Monoxide Readings CO (ppm)
                if (survey2.CO_t != null) {
                    MOND md = NewMOND(mg, survey, survey2);
                    md.MOND_TYPE = "GCO";
                    md.MOND_RDNG = Convert.ToString(survey2.CO_t);
                    md.MOND_NAME = "Carbon Monoxide";
                    md.MOND_UNIT = "ppm";
                    md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                    md.DateTime = dt;  
                    MOND.Add(md);  
                }

                return 0;
    }  
    public MOND NewMOND (MONG mg, LTM_Survey_Data2 survey, LTM_Survey_Data_Repeat2 repeat) {
        
        int round = convertToInt16(survey.mon_rd_nb,"R",-999);

        MOND md = new MOND {
                        ge_source ="esri_survey2_repeat",
                        ge_otherId = Convert.ToString(repeat.globalid),
                        gINTProjectID = mg.gINTProjectID,
                        PointID = mg.PointID,
                        ItemKey = mg.ItemKey,
                        MONG_DIS = mg.MONG_DIS,
                        RND_REF = survey.mon_rd_nb,
                        MOND_REF = String.Format("Round {0:00} Seconds {1:00}" ,round,repeat.elapse_t),
                        };
        return md;    
    }
    }
}
