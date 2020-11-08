using System;
using System.Collections.Generic;
using ge_repository.Services;
using System.Linq;

namespace ge_repository.LowerThamesCrossing {

public class LTM_Survey_Data {
        public int OBJECTID {get;set;} // (type: esriFieldTypeOID, alias: OBJECTID, SQL Type: sqlTypeOther, length: 0, nullable: false, editable: false)
        public Guid globalid {get;set;} //(type: esriFieldTypeGlobalID, alias: GlobalID, SQL Type: sqlTypeOther, length: 38, nullable: false, editable: false)
        public string proj_num {get;set;} //(type: esriFieldTypeString, alias: Project number, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string package {get;set;} //(type: esriFieldTypeString, alias: Package ID, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string mon_rd_nb {get;set;} //(type: esriFieldTypeString, alias: Monitoring round number, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string hole_id {get;set;} //(type: esriFieldTypeString, alias: Exploratory Hole ID, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string holeguid {get;set;} //(type: esriFieldTypeGUID, alias: holeguid, SQL Type: sqlTypeOther, length: 38, nullable: true, editable: true)
        public string mong_ID {get;set;} //(type: esriFieldTypeString, alias: Install ID, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public int? pipe_diam {get;set;} //(type: esriFieldTypeInteger, alias: Pipe diameter (mm), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public string engineer {get;set;} //(type: esriFieldTypeString, alias: Engineer's name, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string eng_other {get;set;} //(type: esriFieldTypeString, alias: Engineer's name(s) - not listed, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public long? date1 {get;set;} //(type: esriFieldTypeDate, alias: Date, SQL Type: sqlTypeOther, length: 8, nullable: true, editable: true)
        // public DateTime? date1 {get;set;} //(type: esriFieldTypeDate, alias: Date, SQL Type: sqlTypeOther, length: 8, nullable: true, editable: true)
        public DateTime? date1_getDT() {if (date1==null) {return null;} return Esri.getDate(date1.Value);}
        public void date1_setDT(DateTime? value) { if (value==null){date1=null; return;} date1 = Esri.getEpoch(value.Value);}        
        public string time1 {get;set;} //(type: esriFieldTypeString, alias: Start time, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public DateTime? time1_getDT() {if (date1==null) {return null;} return Esri.getDateTimeWithTime(date1_getDT().Value, time1);}
        public string weath {get;set;} //(type: esriFieldTypeString, alias: Weather, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string temp {get;set;} //(type: esriFieldTypeString, alias: Temperature, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string wind {get;set;} //(type: esriFieldTypeString, alias: Wind, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string gas_mon {get;set;} //(type: esriFieldTypeString, alias: Ground gas monitoring required?, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string gas_check {get;set;} //(type: esriFieldTypeString, alias: Ground gas monitored?, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string gas_fail {get;set;} //(type: esriFieldTypeString, alias: Reason gas not monitored, SQL Type: sqlTypeOther, length: 1000, nullable: true, editable: true)
        public string gas_instr {get;set;} //(type: esriFieldTypeString, alias: Gas instrument ID, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string gas_cali {get;set;} //(type: esriFieldTypeString, alias: Instrument is calibrated?, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public long? gas_cali_d {get;set;} //(type: esriFieldTypeDate, alias: Date calibrated, SQL Type: sqlTypeOther, length: 8, nullable: true, editable: true)
        public DateTime? gas_cali_d_getDT() {if (gas_cali_d==null) {return null;} return Esri.getDate(gas_cali_d.Value);}
        public void gas_cali_d_setDT(DateTime? value) { if (value==null){gas_cali_d=null; return;} gas_cali_d = Esri.getEpoch(value.Value);}          
        public string PID_instr {get;set;} //(type: esriFieldTypeString, alias: PID instrument ID, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string PID_cali {get;set;} //(type: esriFieldTypeString, alias: Instrument is calibrated?, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public long? PID_cali_d {get;set;} //(type: esriFieldTypeDate, alias: Date calibrated, SQL Type: sqlTypeOther, length: 8, nullable: true, editable: true)
        public DateTime? PID_cali_d_getDT() {if (PID_cali_d == null) {return null;} return Esri.getDate(PID_cali_d.Value);}
        public void PID_cali_d_setDT(DateTime? value) { if (value==null) {PID_cali_d = null; return;} PID_cali_d = Esri.getEpoch(value.Value);}     
        public double? atmo_pressure {get;set;} //(type: esriFieldTypeDouble, alias: Atmospheric pressure (mbar), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? atmo_temp {get;set;} //(type: esriFieldTypeDouble, alias: Atmospheric temperature (°C), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? Diff_BH_pressure {get;set;} //(type: esriFieldTypeDouble, alias: Differential borehole pressure (mbar), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? gas_flow_peak {get;set;} //(type: esriFieldTypeDouble, alias: Peak gas flow rate (l/h), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? gas_flow_steady {get;set;} //(type: esriFieldTypeDouble, alias: Steady gas flow rate (l/h), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public string gas_com {get;set;} //(type: esriFieldTypeString, alias: Comments / notes, SQL Type: sqlTypeOther, length: 1000, nullable: true, editable: true)
        public string dip_req {get;set;} //(type: esriFieldTypeString, alias: Manual dip water level required?, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string dip_check {get;set;} //(type: esriFieldTypeString, alias: Water level measured?, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string dip_fail {get;set;} //(type: esriFieldTypeString, alias: Reason water level not measured, SQL Type: sqlTypeOther, length: 1000, nullable: true, editable: true)
        public string dip_instr {get;set;} //(type: esriFieldTypeString, alias: Dip meter ID, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string dip_cali {get;set;} //(type: esriFieldTypeString, alias: Instrument is calibrated?, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public long? dip_cali_d {get;set;} //(type: esriFieldTypeDate, alias: Date calibrated, SQL Type: sqlTypeOther, length: 8, nullable: true, editable: true)
        public DateTime? dip_cali_d_getDT() {if (dip_cali_d==null) {return null;} return Esri.getDate(dip_cali_d.Value);}
        public void dip_cali_d_setDT(DateTime? value) { if (value==null) {dip_cali_d=null; return;} dip_cali_d = Esri.getEpoch(value.Value);}     
        public string interface_instr {get;set;} //(type: esriFieldTypeString, alias: Interface meter ID, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string interface_cali {get;set;} //(type: esriFieldTypeString, alias: Instrument is calibrated?, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public long? interface_cali_d {get;set;} //(type: esriFieldTypeDate, alias: Date calibrated, SQL Type: sqlTypeOther, length: 8, nullable: true, editable: true)
        public DateTime? interface_cali_d_getDT() {if (interface_cali_d==null) {return null;} return Esri.getDate(interface_cali_d.Value);}
        public void interface_cali_d_setDT(DateTime? value) { if (value==null){interface_cali_d=null; return;} interface_cali_d = Esri.getEpoch(value.Value);}     
        public double? ground_elev {get;set;} //(type: esriFieldTypeDouble, alias: Ground elevation (mOD), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public string dip_datum {get;set;} //(type: esriFieldTypeString, alias: Measurement datum, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string dip_datum_oth {get;set;} //(type: esriFieldTypeString, alias: Measurement datum - Other, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public int? dip_datum_offset {get;set;} //(type: esriFieldTypeInteger, alias: Datum offset (cm), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? depth_gwl {get;set;} //(type: esriFieldTypeDouble, alias: Water depth (m), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? depth_install {get;set;} //(type: esriFieldTypeDouble, alias: Install depth (m), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? depth_gwl_bgl {get;set;} //(type: esriFieldTypeDouble, alias: Groundwater Depth (m bgl), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? elev_gwl {get;set;} //(type: esriFieldTypeDouble, alias: Groundwater Elevation (mOD), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? depth_install_bgl {get;set;} //(type: esriFieldTypeDouble, alias: Install base Depth (m bgl), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? elev_install_base {get;set;} //(type: esriFieldTypeDouble, alias: Install base Elevation (mOD), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? dnapl_top {get;set;} //(type: esriFieldTypeDouble, alias: Top of DNAPL (m), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? dnapl_base {get;set;} //(type: esriFieldTypeDouble, alias: Base of DNAPL (m), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? dnapl_thick {get;set;} //(type: esriFieldTypeDouble, alias: DNAPL thickness (mm), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? lnapl_top {get;set;} //(type: esriFieldTypeDouble, alias: Top of LNAPL (m), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? lnapl_base {get;set;} //(type: esriFieldTypeDouble, alias: Base of LNAPL (m), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? lnapl_thick {get;set;} //(type: esriFieldTypeDouble, alias: LNAPL thickness (mm), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public string dip_com {get;set;} //(type: esriFieldTypeString, alias: Comments / notes, SQL Type: sqlTypeOther, length: 1000, nullable: true, editable: true)
        public string logger_downld_req {get;set;} //(type: esriFieldTypeString, alias: Download of VWP / level logger required?, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string logger_type {get;set;} //(type: esriFieldTypeString, alias: What type of logger is present?, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string logger_check {get;set;} //(type: esriFieldTypeString, alias: Data downloaded and checked?, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string logger_fail {get;set;} //(type: esriFieldTypeString, alias: Reason logger(s) not downloaded, SQL Type: sqlTypeOther, length: 1000, nullable: true, editable: true)
        public string logger_ID {get;set;} //(type: esriFieldTypeString, alias: Logger ID(s) downloaded, SQL Type: sqlTypeOther, length: 1000, nullable: true, editable: true)
        public double? logger_ground_elev {get;set;} //(type: esriFieldTypeDouble, alias: Ground elevation (mOD), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public string logger_dip_datum {get;set;} //(type: esriFieldTypeString, alias: Measurement datum, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public int? logger_dip_datum_offset {get;set;} //(type: esriFieldTypeInteger, alias: Datum offset (cm), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public string logger_dip_datum_oth {get;set;} //(type: esriFieldTypeString, alias: Measurement datum - Other, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public long? logger_removed {get;set;} //(type: esriFieldTypeDate, alias: Time logger removed, SQL Type: sqlTypeOther, length: 8, nullable: true, editable: true)
        public DateTime? logger_removed_getDT() {if (logger_removed==null) {return null;} return Esri.getDate(logger_removed.Value);}
        public void logger_removed_setDT(DateTime? value) { if (value==null){logger_removed=null; return;} logger_removed = Esri.getEpoch(value.Value);}           
        public double? logger_wdepth_1 {get;set;} //(type: esriFieldTypeDouble, alias: Depth to water before logger removed, SQL Type: sqlTypeOther, nullable: true, editable: true)
        public string logger_wdepth_bgl_1 {get;set;} //(type: esriFieldTypeString, alias: Groundwater Depth (m bgl), SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string logger_wdepth_elev_1 {get;set;} //(type: esriFieldTypeString, alias: Groundwater Elevation (mOD), SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string logger_fname {get;set;} //(type: esriFieldTypeString, alias: Logger file name, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public long? logger_t {get;set;} //(type: esriFieldTypeDate, alias: Download time, SQL Type: sqlTypeOther, length: 8, nullable: true, editable: true)
        public DateTime? logger_t_getDT() {if (logger_t==null) {return null;} return Esri.getDate(logger_t.Value);}
        public void logger_t_setDT(DateTime? value) { if (value==null){logger_t=null; return;} logger_t = Esri.getEpoch(value.Value);}             
        public long? logger_replaced {get;set;} //(type: esriFieldTypeDate, alias: Time logger replaced, SQL Type: sqlTypeOther, length: 8, nullable: true, editable: true)
        public DateTime? logger_replaced_getDT() {if (logger_replaced==null) {return null;} return Esri.getDate(logger_replaced.Value);}
        public void logger_replaced_setDT(DateTime? value) { if (value==null){logger_replaced=null; return;} logger_replaced = Esri.getEpoch(value.Value);}     
        public double? logger_wdepth_2 {get;set;} //(type: esriFieldTypeDouble, alias: Depth to water after logger replaced, SQL Type: sqlTypeOther, nullable: true, editable: true)
        public string logger_wdepth_bgl_2 {get;set;} //(type: esriFieldTypeString, alias: Groundwater Depth (m bgl), SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string logger_wdepth_elev_2 {get;set;} //(type: esriFieldTypeString, alias: Groundwater Elevation (mOD), SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string logger_com {get;set;} //(type: esriFieldTypeString, alias: Comments / notes, SQL Type: sqlTypeOther, length: 1000, nullable: true, editable: true)
        public string samp_req {get;set;} //(type: esriFieldTypeString, alias: Ground water sampling required ?, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string samp_check {get;set;} //(type: esriFieldTypeString, alias: Samples taken?, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string samp_fail {get;set;} //(type: esriFieldTypeString, alias: Reason sampling not carried out, SQL Type: sqlTypeOther, length: 1000, nullable: true, editable: true)
        public string BDA {get;set;} //(type: esriFieldTypeString, alias: BDA classification, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string purg_method {get;set;} //(type: esriFieldTypeString, alias: Purging method
        public string purg_pump {get;set;} //(type: esriFieldTypeString, alias: Pump type, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string purg_pump_oth {get;set;} //(type: esriFieldTypeString, alias: Pump type - other, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string purg_vol_matrix {get;set;} //(type: esriFieldTypeString, alias: Purge volume marix, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string vol_calc_rad {get;set;} //(type: esriFieldTypeString, alias: vol_calc_rad, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string vol_calc_area {get;set;} //(type: esriFieldTypeString, alias: vol_calc_area, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string vol_calc_vol {get;set;} //(type: esriFieldTypeString, alias: vol_calc_vol, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public int? target_volume {get;set;} //(type: esriFieldTypeInteger, alias: Target volume of purged water (3 well volumes) (l), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public string purg_disposal {get;set;} //(type: esriFieldTypeString, alias: Purge water disposal, SQL Type: sqlTypeOther, length: 1000, nullable: true, editable: true)
        public string purg_meter {get;set;} //(type: esriFieldTypeString, alias: Field meter ID, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string purg_meter_cali {get;set;} //(type: esriFieldTypeString, alias: Instrument is calibrated?, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public long? purg_meter_cali_d {get;set;} //(type: esriFieldTypeDate, alias: Date calibrated, SQL Type: sqlTypeOther, length: 8, nullable: true, editable: true)
        public DateTime? purg_meter_cali_d_getDT() {if (purg_meter_cali_d==null) {return null;} return Esri.getDate(purg_meter_cali_d.Value);}
        public void purg_meter_cali_d_setDT(DateTime? value) { if (value==null){purg_meter_cali_d=null; return;} purg_meter_cali_d = Esri.getEpoch(value.Value);}     
        
        // public DateTime? purg_meter_cali_d {get;set;} //(type: esriFieldTypeDate, alias: Date calibrated, SQL Type: sqlTypeOther, length: 8, nullable: true, editable: true)
        public string purg_log {get;set;} //(type: esriFieldTypeString, alias: Purge readings logged by instrument?, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string purg_log_fname {get;set;} //(type: esriFieldTypeString, alias: Purge log file name, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string purg_time_strt {get;set;} //(type: esriFieldTypeString, alias: Time purging started, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public double? ph {get;set;} //(type: esriFieldTypeDouble, alias: pH, SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? redox_potential {get;set;} //(type: esriFieldTypeDouble, alias: Redox potential (mV), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? eh {get;set;} //(type: esriFieldTypeDouble, alias: Eh (mV), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? conductivity {get;set;} //(type: esriFieldTypeDouble, alias: Specific conductivity (μS/cm), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? temperature {get;set;} //(type: esriFieldTypeDouble, alias: Temperature (°C), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? dissolved_oxy {get;set;} //(type: esriFieldTypeDouble, alias: Dissolved oxygen (mg/l), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public string turbitity {get;set;} //(type: esriFieldTypeString, alias: Turbidity, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string water_quality {get;set;} //(type: esriFieldTypeString, alias: Water quality parameters stabilised?, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public double? actual_volume {get;set;} //(type: esriFieldTypeDouble, alias: Volume purged (l), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public string purg_time_fin {get;set;} //(type: esriFieldTypeString, alias: Time purging finished, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public string type_number_samples {get;set;} //(type: esriFieldTypeString, alias: Type and number of samples taken, SQL Type: sqlTypeOther, length: 1000, nullable: true, editable: true)
        public int? number_field_samples {get;set;} //(type: esriFieldTypeInteger, alias: Number of field blank samples taken, SQL Type: sqlTypeOther, nullable: true, editable: true)
        public int? number_trip_samples {get;set;} //(type: esriFieldTypeInteger, alias: Number of trip blank samples taken, SQL Type: sqlTypeOther, nullable: true, editable: true)
        public string samp_com {get;set;} //(type: esriFieldTypeString, alias: Comments / notes, SQL Type: sqlTypeOther, length: 1000, nullable: true, editable: true)
        public string time2 {get;set;} //(type: esriFieldTypeString, alias: Finish time, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
        public DateTime? time2_getDT() {if (date1==null) {return null;} return Esri.getDateTimeWithTime(date1_getDT().Value, time2);}
        public long? CreationDate {get;set;} //(type: esriFieldTypeDate, alias: CreationDate, SQL Type: sqlTypeOther, length: 8, nullable: true, editable: false)
        public DateTime? CreationDate_getDT() {if (CreationDate==null) {return null;} return new DateTime(CreationDate.Value);}
        public void CreationDate_setDT(DateTime? value) { if (value==null){CreationDate=null; return;} CreationDate = value.Value.Ticks;}        
        public string Creator {get;set;} //(type: esriFieldTypeString, alias: Creator, SQL Type: sqlTypeOther, length: 128, nullable: true, editable: false)
        public long? EditDate {get;set;} //(type: esriFieldTypeDate, alias: EditDate, SQL Type: sqlTypeOther, length: 8, nullable: true, editable: false)
        public DateTime? EditDate_getDT() {if (EditDate==null) {return null;} return new DateTime(EditDate.Value);}
        public void EditDate_setDT(DateTime? value) { if (value==null){EditDate=null; return;} EditDate = value.Value.Ticks;}        
        public string Editor {get;set;} //(type: esriFieldTypeString, alias: Editor, SQL Type: sqlTypeOther, length: 128, nullable: true, editable: false)
      
}

public class LTM_Survey_Data_Add:LTM_Survey_Data {

        //Additional field added for QA_purposes
        public string QA_status {get;set;} = "Dip_Approved,Purge_Approved,Gas_Approved" ; // QA_status, esriFieldTypeString, QA_status, sqlTypeOther, 1000

        public LTM_Survey_Data_Add() {}

        public LTM_Survey_Data_Add(LTM_Survey_Data s) {

             var props = typeof(LTM_Survey_Data).GetProperties().Where(p => !p.GetIndexParameters().Any());
                foreach (var prop in props)
                {
                        if (prop.CanWrite)
                        prop.SetValue(this, prop.GetValue(s));
                }   
        }
}


public class Additional_LTM_Survey_Data {
        public List<LTM_Survey_Data_Add> Feature_Adds {get;set;} = new List<LTM_Survey_Data_Add>();
}
              
public class LTM_Survey_Data_Repeat {
        public int OBJECTID {get;set;} //(type: esriFieldTypeOID, alias: OBJECTID, SQL Type: sqlTypeOther, length: 0, nullable: false, editable: false)
        public Guid globalid {get;set;} //(type: esriFieldTypeGlobalID, alias: GlobalID, SQL Type: sqlTypeOther, length: 38, nullable: false, editable: false)
        public int? elapse_t {get;set;} //(type: esriFieldTypeInteger, alias: Elapsed time (seconds), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? gas_flow_t {get;set;} //(type: esriFieldTypeDouble, alias: Gas flow (l/h), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? CH4_lel_t {get;set;} //(type: esriFieldTypeDouble, alias: CH4 LEL (%), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? CH4_t {get;set;} //(type: esriFieldTypeDouble, alias: CH4 (% v/v), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? CO2_t {get;set;} //(type: esriFieldTypeDouble, alias: CO2 (% v/v), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? O2_t {get;set;} //(type: esriFieldTypeDouble, alias: O2 (% v/v), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? H2S_t {get;set;} //(type: esriFieldTypeDouble, alias: H2S (ppm), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? CO_t {get;set;} //(type: esriFieldTypeDouble, alias: CO (ppm), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? PID_t {get;set;} //(type: esriFieldTypeDouble, alias: PID (ppm), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public double? voc_t {get;set;} //(type: esriFieldTypeDouble, alias: VOC (ppm), SQL Type: sqlTypeOther, nullable: true, editable: true)
        public Guid parentglobalid {get;set;} //(type: esriFieldTypeGUID, alias: ParentGlobalID, SQL Type: sqlTypeOther, length: 38, nullable: true, editable: true)
        public long? CreationDate {get;set;} //(type: esriFieldTypeDate, alias: CreationDate, SQL Type: sqlTypeOther, length: 8, nullable: true, editable: false)
        public DateTime? CreationDate_getDT() {if (CreationDate==null) {return null;} return Esri.getDate(CreationDate.Value);}
        public void CreationDate_setDT(DateTime? value) { if (value==null){CreationDate=null; return;} CreationDate = Esri.getEpoch(value.Value);} 
        public string Creator {get;set;} //(type: esriFieldTypeString, alias: Creator, SQL Type: sqlTypeOther, length: 128, nullable: true, editable: false)
        public long? EditDate {get;set;} //(type: esriFieldTypeDate, alias: EditDate, SQL Type: sqlTypeOther, length: 8, nullable: true, editable: false)
        public DateTime? EditDate_getDT() {if (EditDate==null) {return null;} return Esri.getDate(EditDate.Value);}
        public void EditDate_setDT(DateTime? value) { if (value==null){EditDate=null; return;} EditDate = Esri.getEpoch(value.Value);}  
        public string Editor {get;set;} //(type: esriFieldTypeString, alias: Editor, SQL Type: sqlTypeOther, length: 128, nullable: true, editable: false)

       

        }
}
