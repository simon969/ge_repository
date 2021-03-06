using System;
using System.Collections.Generic;
using ge_repository.ESRI;
using ge_repository.OtherDatabase;
using static ge_repository.LowerThamesCrossing.LTC;

namespace ge_repository.LowerThamesCrossing {

public class LTM_Survey_Data2 : IEsriParent {
public int objectid {get;set;}	// objectid, esriFieldTypeOID, ObjectID, sqlTypeOther, 
public Guid globalid {get;set;}	// globalid, esriFieldTypeGlobalID, GlobalID, sqlTypeGUID, 38
public string survey_version {get;set;}	// survey_version, esriFieldTypeString, survey_version, sqlTypeOther, 255
public string proj_num {get;set;}	// proj_num, esriFieldTypeString, Project number, sqlTypeOther, 255
public string pack {get;set;}	// pack, esriFieldTypeString, Package ID, sqlTypeOther, 255
public string mon_rd_nb {get;set;}	// mon_rd_nb, esriFieldTypeString, Monitoring round number, sqlTypeOther, 255
public string hole_id {get;set;}	// hole_id, esriFieldTypeString, Exploratory Hole ID, sqlTypeOther, 255
public string holeguid {get;set;}	// holeguid, esriFieldTypeGUID, holeguid, sqlTypeOther, 50
public string mong_ID {get;set;}	// mong_ID, esriFieldTypeString, Install ID, sqlTypeOther, 255
public int? pipe_diam {get;set;}	// pipe_diam, esriFieldTypeInteger, Pipe diameter (mm), sqlTypeOther, 
public double? ground_elev {get;set;}	// ground_elev, esriFieldTypeDouble, Ground elevation (mOD), sqlTypeOther, 
public string typical_datum_ref {get;set;}	// typical_datum_ref, esriFieldTypeString, typical_datum_ref, sqlTypeOther, 255
public double? typical_datum_offset {get;set;}	// typical_datum_offset, esriFieldTypeDouble, typical_datum_offset, sqlTypeOther, 
public string typical_purg_disposal {get;set;}	// typical_purg_disposal, esriFieldTypeString, typical_purg_disposal, sqlTypeOther, 255
public double? typical_logger_cable {get;set;}	// typical_logger_cable, esriFieldTypeDouble, typical_logger_cable, sqlTypeOther, 
public string BDA {get;set;}	// BDA, esriFieldTypeString, BDA classification, sqlTypeOther, 255
public string engineer {get;set;}	// engineer, esriFieldTypeString, Engineers & technicians name, sqlTypeOther, 255
public string eng_other {get;set;}	// eng_other, esriFieldTypeString, Name(s) - not listed, sqlTypeOther, 255
public long? date1 {get;set;}	// date1, esriFieldTypeDate, Start date and time, sqlTypeOther, 255
public DateTime? date1_getDT() {if (date1==null) {return null;} return Esri.getDate(date1.Value);}
public void date1_setDT(DateTime? value) { if (value==null){date1=null; return;} date1 = Esri.getEpoch(value.Value);}        
public string pumptest {get;set;}	// pumptest, esriFieldTypeString, Pump testing nearby?, sqlTypeOther, 255
public string weath {get;set;}	// weath, esriFieldTypeString, Weather, sqlTypeOther, 255
public string temp {get;set;}	// temp, esriFieldTypeString, Temperature, sqlTypeOther, 255
public string wind {get;set;}	// wind, esriFieldTypeString, Wind, sqlTypeOther, 255
public string bh_condi {get;set;}	// bh_condi, esriFieldTypeString, BH condition, sqlTypeOther, 255
public string bh_condi_comm {get;set;}	// bh_condi_comm, esriFieldTypeString, BH condition comments, sqlTypeOther, 1000
public string spare_text1 {get;set;}	// spare_text1, esriFieldTypeString, spare_text1, sqlTypeOther, 1000
public string spare_text2 {get;set;}	// spare_text2, esriFieldTypeString, spare_text2, sqlTypeOther, 1000
public double? spare_num1 {get;set;}	// spare_num1, esriFieldTypeDouble, spare_num1, sqlTypeOther, 
public double? spare_num2 {get;set;}	// spare_num2, esriFieldTypeDouble, spare_num2, sqlTypeOther, 
public string gas_mon {get;set;}	// gas_mon, esriFieldTypeString, Ground gas monitoring required?, sqlTypeOther, 255
public string gas_check {get;set;}	// gas_check, esriFieldTypeString, Ground gas monitored?, sqlTypeOther, 255
public string gas_fail {get;set;}	// gas_fail, esriFieldTypeString, Reason gas not monitored, sqlTypeOther, 1000
public string gas_instr {get;set;}	// gas_instr, esriFieldTypeString, Gas instrument ID, sqlTypeOther, 255
public string gas_instr_other {get;set;}	// gas_instr_other, esriFieldTypeString, Gas instrument ID - Other, sqlTypeOther, 255
public string gas_cali {get;set;}	// gas_cali, esriFieldTypeString, Instrument is calibrated?, sqlTypeOther, 255
public long? gas_cali_d {get;set;}	// gas_cali_d, esriFieldTypeDate, Date calibrated, sqlTypeOther, 255
public DateTime? gas_cali_d_getDT() {if (gas_cali_d==null) {return null;} return Esri.getDate(gas_cali_d.Value);}
public void gas_cali_d_setDT(DateTime? value) { if (value==null){gas_cali_d=null; return;} gas_cali_d = Esri.getEpoch(value.Value);}   
public string PID_instr {get;set;}	// PID_instr, esriFieldTypeString, PID instrument ID, sqlTypeOther, 255
public string PID_instr_other {get;set;}	// PID_instr_other, esriFieldTypeString, PID instrument ID - Other, sqlTypeOther, 255
public string PID_cali {get;set;}	// PID_cali, esriFieldTypeString, Instrument is calibrated?, sqlTypeOther, 255
public long? PID_cali_d {get;set;}	// PID_cali_d, esriFieldTypeDate, Date calibrated, sqlTypeOther, 255
public DateTime? PID_cali_d_getDT() {if (PID_cali_d == null) {return null;} return Esri.getDate(PID_cali_d.Value);}
public void PID_cali_d_setDT(DateTime? value) { if (value==null) {PID_cali_d = null; return;} PID_cali_d = Esri.getEpoch(value.Value);}     
public string ambi1_time {get;set;}	// ambi1_time, esriFieldTypeString, Time of ambient air readings, sqlTypeOther, 255
public DateTime? ambi1_time_getDT() {if (date1==null) {return null;} return Esri.getDateTimeWithTime(date1_getDT().Value, ambi1_time);}
public double? ambi1_CH4_lel {get;set;}	// ambi1_CH4_lel, esriFieldTypeDouble, CH4 LEL (%), sqlTypeOther, 
public double? ambi1_CH4 {get;set;}	// ambi1_CH4, esriFieldTypeDouble, CH4 (% v/v), sqlTypeOther, 
public double? ambi1_CO2 {get;set;}	// ambi1_CO2, esriFieldTypeDouble, CO2 (% v/v), sqlTypeOther, 
public double? ambi1_O2 {get;set;}	// ambi1_O2, esriFieldTypeDouble, O2 (% v/v), sqlTypeOther, 
public double? ambi1_H2S {get;set;}	// ambi1_H2S, esriFieldTypeDouble, H2S (ppm), sqlTypeOther, 
public double? ambi1_CO {get;set;}	// ambi1_CO, esriFieldTypeDouble, CO (ppm), sqlTypeOther, 
public string ambi1_check {get;set;}	// ambi1_check, esriFieldTypeString, Deviation check, sqlTypeOther, 255
public double? atmo_temp {get;set;}	// atmo_temp, esriFieldTypeDouble, Atmospheric temperature (°C), sqlTypeOther, 
public double? atmo_pressure {get;set;}	// atmo_pressure, esriFieldTypeDouble, Atmospheric pressure, sqlTypeOther, 
public string atmo_press_unit {get;set;}	// atmo_press_unit, esriFieldTypeString, Atmospheric pressure units, sqlTypeOther, 255
public double? BH_pressure_peak {get;set;}	// BH_pressure_peak, esriFieldTypeDouble, Differential borehole pressure - peak, sqlTypeOther, 
public double? BH_pressure_steady {get;set;}	// BH_pressure_steady, esriFieldTypeDouble, Differential borehole pressure - steady, sqlTypeOther, 
public string BH_pressure_units {get;set;}	// BH_pressure_units, esriFieldTypeString, Differential borehole pressure units, sqlTypeOther, 255
public double? gas_flow_peak {get;set;}	// gas_flow_peak, esriFieldTypeDouble, Peak gas flow rate (l/h), sqlTypeOther, 
public double? gas_flow_steady {get;set;}	// gas_flow_steady, esriFieldTypeDouble, Steady gas flow rate (l/h), sqlTypeOther, 
public string gas_repeat_tstart {get;set;}	// gas_repeat_tstart, esriFieldTypeString, Gas readings start time, sqlTypeOther, 255
public DateTime? gas_repeat_tstart_getDT() {if (date1==null) {return null;} return Esri.getDateTimeWithTime(date1_getDT().Value, gas_repeat_tstart);}
public double? PID_t {get;set;}	// PID_t, esriFieldTypeDouble, PID (ppm), sqlTypeOther, 
public string ambi2_time {get;set;}	// ambi2_time, esriFieldTypeString, Time of ambient air readings, sqlTypeOther, 255
public DateTime? ambi2_time_getDT() {if (date1==null) {return null;} return Esri.getDateTimeWithTime(date1_getDT().Value, ambi2_time);}
public double? ambi2_CH4_lel {get;set;}	// ambi2_CH4_lel, esriFieldTypeDouble, CH4 LEL (%), sqlTypeOther, 
public double? ambi2_CH4 {get;set;}	// ambi2_CH4, esriFieldTypeDouble, CH4 (% v/v), sqlTypeOther, 
public double? ambi2_CO2 {get;set;}	// ambi2_CO2, esriFieldTypeDouble, CO2 (% v/v), sqlTypeOther, 
public double? ambi2_O2 {get;set;}	// ambi2_O2, esriFieldTypeDouble, O2 (% v/v), sqlTypeOther, 
public double? ambi2_H2S {get;set;}	// ambi2_H2S, esriFieldTypeDouble, H2S (ppm), sqlTypeOther, 
public double? ambi2_CO {get;set;}	// ambi2_CO, esriFieldTypeDouble, CO (ppm), sqlTypeOther, 
public string ambi2_check {get;set;}	// ambi2_check, esriFieldTypeString, Deviation check, sqlTypeOther, 255
public string gas_com {get;set;}	// gas_com, esriFieldTypeString, Comments / notes, sqlTypeOther, 1000
public string dip_req {get;set;}	// dip_req, esriFieldTypeString, Manual dip water level required?, sqlTypeOther, 255
public string dip_check {get;set;}	// dip_check, esriFieldTypeString, Water level measured?, sqlTypeOther, 255
public string dip_fail {get;set;}	// dip_fail, esriFieldTypeString, Reason water level not measured, sqlTypeOther, 1000
public string dip_or_interface {get;set;}	// dip_or_interface, esriFieldTypeString, Dip or Interface meter, sqlTypeOther, 255
public string dip_instr {get;set;}	// dip_instr, esriFieldTypeString, Dip or Interface meter ID, sqlTypeOther, 255
public string dip_instr_other {get;set;}	// dip_instr_other, esriFieldTypeString, Dip or Interface meter ID - Other, sqlTypeOther, 255
public string dip_datum {get;set;}	// dip_datum, esriFieldTypeString, Measurement datum, sqlTypeOther, 255
public double? dip_datum_offset {get;set;}	// dip_datum_offset, esriFieldTypeDouble, Datum offset (m), sqlTypeOther, 
public string dip_datum_oth {get;set;}	// dip_datum_oth, esriFieldTypeString, Measurement datum - Other, sqlTypeOther, 255
public string dip_time {get;set;}	// dip_time, esriFieldTypeString, Time of dip measurement, sqlTypeOther, 255
public DateTime? dip_time_getDT() {if (date1==null) {return null;} return Esri.getDateTimeWithTime(date1_getDT().Value, dip_time);}
public string dip_water_or_dry {get;set;}	// dip_water_or_dry, esriFieldTypeString, Water level present or hole is dry, sqlTypeOther, 255
public double? depth_gwl {get;set;}	// depth_gwl, esriFieldTypeDouble, Water depth (m), sqlTypeOther, 
public double? depth_install {get;set;}	// depth_install, esriFieldTypeDouble, Install depth (m), sqlTypeOther, 
public double? depth_gwl_bgl {get;set;}	// depth_gwl_bgl, esriFieldTypeDouble, Groundwater Depth (m bgl), sqlTypeOther, 
public double? elev_gwl {get;set;}	// elev_gwl, esriFieldTypeDouble, Groundwater Elevation (mOD), sqlTypeOther, 
public double? depth_install_bgl {get;set;}	// depth_install_bgl, esriFieldTypeDouble, Install base Depth (m bgl), sqlTypeOther, 
public double? elev_install_base {get;set;}	// elev_install_base, esriFieldTypeDouble, Install base Elevation (mOD), sqlTypeOther, 
public double? dnapl_top {get;set;}	// dnapl_top, esriFieldTypeDouble, Top of DNAPL (m), sqlTypeOther, 
public double? dnapl_base {get;set;}	// dnapl_base, esriFieldTypeDouble, Base of DNAPL (m), sqlTypeOther, 
public double? dnapl_thick {get;set;}	// dnapl_thick, esriFieldTypeDouble, DNAPL thickness (mm), sqlTypeOther, 
public double? lnapl_top {get;set;}	// lnapl_top, esriFieldTypeDouble, Top of LNAPL (m), sqlTypeOther, 
public double? lnapl_base {get;set;}	// lnapl_base, esriFieldTypeDouble, Base of LNAPL (m), sqlTypeOther, 
public double? lnapl_thick {get;set;}	// lnapl_thick, esriFieldTypeDouble, LNAPL thickness (mm), sqlTypeOther, 
public string dip_com {get;set;}	// dip_com, esriFieldTypeString, Comments / notes, sqlTypeOther, 1000
public string logger_downld_req {get;set;}	// logger_downld_req, esriFieldTypeString, Download of VWP / level logger required?, sqlTypeOther, 255
public string logger_check {get;set;}	// logger_check, esriFieldTypeString, Data downloaded and checked?, sqlTypeOther, 255
public string logger_fail {get;set;}	// logger_fail, esriFieldTypeString, Reason logger(s) not downloaded, sqlTypeOther, 1000
public string logger_type {get;set;}	// logger_type, esriFieldTypeString, What type of logger is present?, sqlTypeOther, 255
public string logger_ID {get;set;}	// logger_ID, esriFieldTypeString, Logger ID(s) downloaded, sqlTypeOther, 1000
public string logger_dip_datum {get;set;}	// logger_dip_datum, esriFieldTypeString, Measurement datum, sqlTypeOther, 255
public double? logger_dip_datum_offset {get;set;}	// logger_dip_datum_offset, esriFieldTypeDouble, Datum offset (m), sqlTypeOther, 
public string logger_dip_datum_oth {get;set;}	// logger_dip_datum_oth, esriFieldTypeString, Measurement datum - Other, sqlTypeOther, 255
public string logger_removed {get;set;}	// logger_removed, esriFieldTypeString, Time logger removed, sqlTypeOther, 255
public double? logger_wdepth_1 {get;set;}	// logger_wdepth_1, esriFieldTypeDouble, Water depth before logger removed (m), sqlTypeOther, 
public string logger_wdepth_bgl_1 {get;set;}	// logger_wdepth_bgl_1, esriFieldTypeString, Groundwater Depth (m bgl), sqlTypeOther, 255
public string logger_wdepth_elev_1 {get;set;}	// logger_wdepth_elev_1, esriFieldTypeString, Groundwater Elevation (mOD), sqlTypeOther, 255
public double? logger_cable {get;set;}	// logger_cable, esriFieldTypeDouble, Logger cable length (m), sqlTypeOther, 
public string logger_fname {get;set;}	// logger_fname, esriFieldTypeString, Logger file name, sqlTypeOther, 255
public string logger_t {get;set;}	// logger_t, esriFieldTypeString, Download time, sqlTypeOther, 255
public DateTime? logger_t_getDT() {if (date1==null) {return null;} return Esri.getDateTimeWithTime(date1_getDT().Value, logger_t);}
public string logger_com {get;set;}	// logger_com, esriFieldTypeString, Comments / notes, sqlTypeOther, 1000
public string samp_req {get;set;}	// samp_req, esriFieldTypeString, Ground water sampling required ?, sqlTypeOther, 255
public string samp_check {get;set;}	// samp_check, esriFieldTypeString, Samples taken?, sqlTypeOther, 255
public string samp_fail {get;set;}	// samp_fail, esriFieldTypeString, Reason sampling not carried out, sqlTypeOther, 1000
public string purg_method {get;set;}	// purg_method, esriFieldTypeString, Purging method</b, sqlTypeOther, 255
public string purg_pump {get;set;}	// purg_pump, esriFieldTypeString, Pump type & ID, sqlTypeOther, 255
public string purg_pump_oth {get;set;}	// purg_pump_oth, esriFieldTypeString, Pump type & ID - other, sqlTypeOther, 255
public string purg_record_type {get;set;}	// purg_record_type, esriFieldTypeString, Purge recording type, sqlTypeOther, 255
public string purg_log_fname {get;set;}	// purg_log_fname, esriFieldTypeString, Purge log file name, sqlTypeOther, 255
public string vol_calc_rad {get;set;}	// vol_calc_rad, esriFieldTypeString, vol_calc_rad, sqlTypeOther, 255
public string vol_calc_area {get;set;}	// vol_calc_area, esriFieldTypeString, vol_calc_area, sqlTypeOther, 255
public string vol_calc_vol {get;set;}	// vol_calc_vol, esriFieldTypeString, vol_calc_vol, sqlTypeOther, 255
public int? target_volume {get;set;}	// target_volume, esriFieldTypeInteger, Target volume of purged water (3 well volumes) (l), sqlTypeOther, 
public string purg_disposal {get;set;}	// purg_disposal, esriFieldTypeString, Purge water disposal, sqlTypeOther, 1000
public string purg_disposal_oth {get;set;}	// purg_disposal_oth, esriFieldTypeString, Other - water disposal, sqlTypeOther, 255
public string purg_meter {get;set;}	// purg_meter, esriFieldTypeString, Field meter ID, sqlTypeOther, 255
public string purg_meter_other {get;set;}	// purg_meter_other, esriFieldTypeString, Field meter ID - Other, sqlTypeOther, 255
public string purg_meter_cali {get;set;}	// purg_meter_cali, esriFieldTypeString, Instrument is calibrated?, sqlTypeOther, 255
public long? purg_meter_cali_d {get;set;}	// purg_meter_cali_d, esriFieldTypeDate, Date calibrated, sqlTypeOther, 255
public DateTime? purg_meter_cali_d_getDT() {if (purg_meter_cali_d == null) {return null;} return Esri.getDate(purg_meter_cali_d.Value);}
public void purg_meter_cali_d_setDT(DateTime? value) { if (value==null) {purg_meter_cali_d = null; return;} purg_meter_cali_d = Esri.getEpoch(value.Value);}     

public string purg_time_strt {get;set;}	// purg_time_strt, esriFieldTypeString, Time purging started, sqlTypeOther, 255
public DateTime? purg_time_strtgetDT() {if (date1==null) {return null;} return Esri.getDateTimeWithTime(date1_getDT().Value, purg_time_strt);}
public double? ph {get;set;}	// ph, esriFieldTypeDouble, pH, sqlTypeOther, 
public double? redox_potential {get;set;}	// redox_potential, esriFieldTypeDouble, Redox potential (mV), sqlTypeOther, 
public double? conductivity {get;set;}	// conductivity, esriFieldTypeDouble, Specific conductivity (μS/cm @25 degC), sqlTypeOther, 
public double? temperature {get;set;}	// temperature, esriFieldTypeDouble, Temperature (°C), sqlTypeOther, 
public double? dissolved_oxy {get;set;}	// dissolved_oxy, esriFieldTypeDouble, Dissolved oxygen (mg/l), sqlTypeOther, 
public double? turbitity {get;set;}	// turbitity, esriFieldTypeDouble, Turbitity (NTU), sqlTypeOther, 
public string water_quality {get;set;}	// water_quality, esriFieldTypeString, Water quality parameters stabilised?, sqlTypeOther, 255
public double? actual_volume {get;set;}	// actual_volume, esriFieldTypeDouble, Volume purged (l), sqlTypeOther, 
public string purg_time_fin {get;set;}	// purg_time_fin, esriFieldTypeString, Time purging finished, sqlTypeOther, 255
public DateTime? purg_time_fingetDT() {if (date1==null) {return null;} return Esri.getDateTimeWithTime(date1_getDT().Value, purg_time_fin);}
public int? HNO3_prsrv_filtered {get;set;}	// HNO3_prsrv_filtered, esriFieldTypeInteger, Nitric Acid preserve filtered (ALE 204 - filtered), sqlTypeOther, 
public int? HNO3_prsrv_unfiltered {get;set;}	// HNO3_prsrv_unfiltered, esriFieldTypeInteger, Nitric Acid preserve unfiltered (ALE 204 - unfiltered), sqlTypeOther, 
public int? H2SO4_prsrv {get;set;}	// H2SO4_prsrv, esriFieldTypeInteger, Sulphuric Acid preserve (ALE 244), sqlTypeOther, 
public int? NaOH_prsrv {get;set;}	// NaOH_prsrv, esriFieldTypeInteger, Sodium Hydroxide preserve (ALE 245), sqlTypeOther, 
public int? glass_40mL {get;set;}	// glass_40mL, esriFieldTypeInteger, 40ml glass vials (ALE 297), sqlTypeOther, 
public int? glass_250mL {get;set;}	// glass_250mL, esriFieldTypeInteger, 250ml glass (ALE 219), sqlTypeOther, 
public int? glass_500mL {get;set;}	// glass_500mL, esriFieldTypeInteger, 500ml glass (ALE 227), sqlTypeOther, 
public int? plastic_BOD_250mL {get;set;}	// plastic_BOD_250mL, esriFieldTypeInteger, BOD 250ml plastic (ALE 212 ), sqlTypeOther, 
public int? plastic_500mL {get;set;}	// plastic_500mL, esriFieldTypeInteger, 500ml plastic (ALE 208), sqlTypeOther, 
public int? bacti_bottle_500mL {get;set;}	// bacti_bottle_500mL, esriFieldTypeInteger, 500ml Bacti Bottle (STL19), sqlTypeOther, 
public int? bacti_pot_500mL {get;set;}	// bacti_pot_500mL, esriFieldTypeInteger, 500ml Bacti Pot (STL20), sqlTypeOther, 
public string other_samples {get;set;}	// other_samples, esriFieldTypeString, Note any unlisted samples taken, sqlTypeOther, 1000
public string samp_duplicate {get;set;}	// samp_duplicate, esriFieldTypeString, Duplicate sample taken?, sqlTypeOther, 255
public string samp_duplicate_ID {get;set;}	// samp_duplicate_ID, esriFieldTypeString, Duplicate sample ID / name, sqlTypeOther, 255
public string samp_com {get;set;}	// samp_com, esriFieldTypeString, Comments / notes, sqlTypeOther, 1000
public string logger_replaced {get;set;}	// logger_replaced, esriFieldTypeString, Time logger replaced, sqlTypeOther, 255
public double? logger_wdepth_2 {get;set;}	// logger_wdepth_2, esriFieldTypeDouble, Water depth after logger replaced (m), sqlTypeOther, 
public string logger_wdepth_bgl_2 {get;set;}	// logger_wdepth_bgl_2, esriFieldTypeString, Groundwater Depth (m bgl), sqlTypeOther, 255
public string logger_wdepth_elev_2 {get;set;}	// logger_wdepth_elev_2, esriFieldTypeString, Groundwater Elevation (mOD), sqlTypeOther, 255
public string logger_com2 {get;set;}	// logger_com2, esriFieldTypeString, Comments / notes, sqlTypeOther, 1000
public string gen_comments {get;set;}	// gen_comments, esriFieldTypeString, General comments, sqlTypeOther, 1000
public string time2 {get;set;}	// time2, esriFieldTypeString, Finish time, sqlTypeOther, 255
public DateTime? time2_getDT() {if (date1==null) {return null;} return Esri.getDateTimeWithTime(date1_getDT().Value, time2);}

public string QA_status {get;set;}	// QA_status, esriFieldTypeString, QA_status, sqlTypeOther, 1000
public string QA_check_by {get;set;}	// QA_check_by, esriFieldTypeString, QA_check_by, sqlTypeOther, 1000
public string spare_text3 {get;set;}	// spare_text3, esriFieldTypeString, spare_text3, sqlTypeOther, 1000
public string spare_text4 {get;set;}	// spare_text4, esriFieldTypeString, spare_text4, sqlTypeOther, 1000
public string spare_text5 {get;set;}	// spare_text5, esriFieldTypeString, spare_text5, sqlTypeOther, 1000
public string spare_text6 {get;set;}	// spare_text6, esriFieldTypeString, spare_text6, sqlTypeOther, 1000
public double? spare_num3 {get;set;}	// spare_num3, esriFieldTypeDouble, spare_num3, sqlTypeOther, 
public double? spare_num4 {get;set;}	// spare_num4, esriFieldTypeDouble, spare_num4, sqlTypeOther, 
public double? spare_num5 {get;set;}	// spare_num5, esriFieldTypeDouble, spare_num5, sqlTypeOther, 
public double? spare_num6 {get;set;}	// spare_num6, esriFieldTypeDouble, spare_num6, sqlTypeOther, 
public long? spare_dateTime1 {get;set;}	// spare_dateTime1, esriFieldTypeDate, spare_dateTime1, sqlTypeOther, 255
public DateTime? spare_dateTime1_getDT() {if (spare_dateTime1==null) {return null;} return Esri.getDate(spare_dateTime1.Value);}
public void spare_dateTime1_setDT(DateTime? value) { if (value==null){spare_dateTime1 =null; return;} spare_dateTime1 = Esri.getEpoch(value.Value);}        

public long? spare_dateTime2 {get;set;}	// spare_dateTime2, esriFieldTypeDate, spare_dateTime2, sqlTypeOther, 255
public DateTime? spare_dateTime2_getDT() {if (spare_dateTime2==null) {return null;} return Esri.getDate(spare_dateTime2.Value);}
public void spare_dateTime2_setDT(DateTime? value) { if (value==null){spare_dateTime2 =null; return;} spare_dateTime2 = Esri.getEpoch(value.Value);}        

public long? spare_dateTime3 {get;set;}	// spare_dateTime3, esriFieldTypeDate, spare_dateTime3, sqlTypeOther, 255
public DateTime? spare_dateTime3_getDT() {if (spare_dateTime3==null) {return null;} return Esri.getDate(spare_dateTime3.Value);}
public void spare_dateTime3_setDT(DateTime? value) { if (value==null){spare_dateTime3 =null; return;} spare_dateTime3 = Esri.getEpoch(value.Value);}        

public long? CreationDate {get;set;}	// CreationDate, esriFieldTypeDate, CreationDate, sqlTypeOther, 8
public DateTime? CreationDate_getDT() {if (CreationDate==null) {return null;} return new DateTime(CreationDate.Value);}
public void CreationDate_setDT(DateTime? value) { if (value==null){CreationDate=null; return;} CreationDate = Esri.getEpoch(value.Value);}        

public string Creator {get;set;}	// Creator, esriFieldTypeString, Creator, sqlTypeOther, 128
public long? EditDate {get;set;}	// EditDate, esriFieldTypeDate, EditDate, sqlTypeOther, 8
public DateTime? EditDate_getDT() {if (EditDate==null) {return null;} return new DateTime(EditDate.Value);}
public void EditDate_setDT(DateTime? value) { if (value==null){EditDate=null; return;} EditDate = Esri.getEpoch(value.Value);}       
public string Editor {get;set;}	// Editor, esriFieldTypeString, Editor, sqlTypeOther, 128

public string dip_or_pressure {get;set;} // dip_or_pressure (type: esriFieldTypeString, alias: Type of water level reading taken, SQL Type: sqlTypeOther, length: 255, nullable: true, editable: true)
public double? water_pressure {get;set;} // water_pressure (type: esriFieldTypeDouble, alias: Water pressure (bar), SQL Type: sqlTypeOther, nullable: true, editable: true)

public string surv_req {get;set;} // string (255 characters)	surv_req	Survey measurements required?	Yes / No check box to say whether survey levels have been recorded

public string surv_check {get;set;}// string (255 characters)	surv_check	Survey meassurements carried out?	Yes / No check box to indicate where topo survey not carried out but was required
public string surv_fail {get;set;}// string (1000 characters)	surv_fail	Reason survey measurements not taken	If topo required, but not carried out, this comment section appears to explain why
public string surv_peg {get;set;}// string (255 characters)	surv_peg	Is the ground level peg in place and visible?	Check box for presence of ground level peg
public string surv_peg_c {get;set;}// string (1000 characters)	surv_peg_c	Comment - GL peg not visible:	Comments if ground level peg not present
public string surv_hwmark {get;set;}// string (255 characters)	surv_hwmark	Is the measurement point marked on the headworks?	Check box for presence of mark on head works
public string surv_hwmark_c {get;set;}// string (1000 characters)	surv_hwmark_c	Comment if measurement point not marked:	Comments if mark not present
public string surv_pipemark {get;set;}// string (255 characters)	surv_pipemark	Is the measurement point marked on the installed pipe?	Check box for presences of mark on installed pipe
public string surv_pipemarc_c {get;set;}// string (1000 characters)	surv_pipemark_c	Comment if measurement point not marked:	Comments if mark not present
public string surv_option {get;set;}// string (255 characters)	surv_option	Possible measurements	Filter option for the field engineer to choose if they measure
//                                                                      -	Ground level and Top of Cover only
//                                                                      -	Ground level, Top of Cover and Top of Installed pipe
public string surv_time {get;set;}// string (255 characters)	surv_time	Time survey carried out	Added a time field similar to other activity types (e.g. dip_time). Time only as string (no date)
public DateTime? surv_time_getDT() {if (date1==null) {return null;} return Esri.getDateTimeWithTime(date1_getDT().Value, surv_time);}
public double? surv_g_level {get;set;}// double	surv_g_level	Surveyed Ground Level (mAOD)	Surveyed level of ground
public double? surv_ToC {get;set;} // double	surv_ToC	Surveyed Top of Cover (mAOD)	surveyed level of Top of Cover
public double? surv_ToI {get;set;} // double	surv_ToI	Surveyed Top of Installed Pipe (mAOD)	Surveyed level of Top of Installation
public double? meas_ToC {get;set;} // double	meas_ToC	MeasuredTop of Cover - A(m)	Tape measured height difference (distance) between ground level and Top of Cover
public double? meas_ToI {get;set;}// double	meas_ToI	Measured Top of Installed pipe - B (m)	Tape measured height difference (distance) between ground level and Top of Installed pipe
public string surv_com {get;set;} // string (1000 characters)	surv_com	Notes and comments	General comments on survey work


public MOND NewMOND (MONG mg, LTM_Survey_Data2 survey ) {
        
        int round = convertToInt16(survey.mon_rd_nb,"R",-999);

        MOND md = new MOND {
                        ge_source ="esri_survey2",
                        ge_otherId = Convert.ToString(survey.globalid),
                        gINTProjectID = mg.gINTProjectID,
                        PointID = mg.PointID,
                        ItemKey = mg.ItemKey,
                        MONG_DIS = mg.MONG_DIS,
                        RND_REF = survey.mon_rd_nb,
                        MOND_REF = String.Format("Round {0:00}", round),
                        DateTime =  gINTDateTime(survey.date1_getDT())
                        };
        return md;    

 }
public MONV NewMONV (POINT pt, LTM_Survey_Data2 survey) {
      
        int round = convertToInt16(survey.mon_rd_nb,"R", -999);
        
        MONV mv = new MONV {
                        ge_source ="esri_survey2",
                        ge_otherId = Convert.ToString(survey.globalid),
                        gINTProjectID = pt.gINTProjectID,
                        PointID = pt.PointID,
                        DateTime = gINTDateTime(survey.date1_getDT()),
                        RND_REF = survey.mon_rd_nb,
                        MONV_REF = String.Format("Round {0:00}", round)
                        };
        return mv;    
 }

public int AddVisit(POINT pt, LTM_Survey_Data2 survey, List<MONV> MONV){
            
          
                MONV mv = NewMONV(pt,survey);
               
                mv.MONV_STAR = gINTDateTime(survey.date1_getDT()).Value;
                mv.MONV_ENDD = gINTDateTime(survey.time2_getDT()).Value;
                
                mv.MONV_DIPR = survey.dip_req;
                mv.MONV_GASR = survey.gas_mon;
                mv.MONV_LOGR = survey.logger_downld_req ;
                mv.MONV_TOPR = survey.surv_req;
                
                mv.MONV_REMG = survey.gas_fail + " " + survey.gas_com;
                mv.MONV_REMD = survey.dip_fail + " " + survey.dip_com;
                mv.MONV_REML = survey.logger_fail + " " + survey.logger_com;
                mv.MONV_REMS = survey.samp_fail + " " + survey.samp_com;
                mv.MONV_REMT = survey.surv_fail + " " + survey.surv_com;
                
                mv.PUMP_TYPE = survey.purg_pump + " " + survey.purg_pump_oth;

                mv.MONV_WEAT = survey.weath;
                mv.MONV_TEMP = survey.temp;
                mv.MONV_WIND = survey.wind;
                
                mv.DIP_SRLN = IfOther(survey.dip_instr,survey.dip_instr_other);
                mv.DIP_CLBD = null;
                
                mv.FLO_SRLN = IfOther(survey.purg_meter, survey.purg_meter_other);
                mv.FLO_CLBD = gINTDateTime(survey.purg_meter_cali_d_getDT());
                
                mv.GAS_SRLN = IfOther(survey.gas_instr, survey.gas_instr_other);
                mv.GAS_CLBD = gINTDateTime(survey.gas_cali_d_getDT());
                
                mv.PID_SRLN = IfOther(survey.PID_instr, survey.PID_instr_other);
                mv.PID_CLBD = gINTDateTime(survey.PID_cali_d_getDT());
                
                mv.RND_REF = survey.mon_rd_nb;
                mv.MONV_DATM = survey.dip_datum;

                mv.AIR_PRESS = survey.atmo_pressure;
                mv.AIR_TEMP = survey.atmo_temp;
                
                mv.PIPE_DIA = survey.pipe_diam;

                if(survey.dip_datum_offset != null) {
                   // mv.MONV_DIS = ((float) survey.dip_datum_offset.Value) / 100f;
                    mv.MONV_DIS = ((float) survey.dip_datum_offset.Value);
                }

                mv.MONV_MENG = survey.Creator;
                MONV.Add(mv);

    return 0;

}


public int AddPurge(MONG mg, LTM_Survey_Data2 survey, List<MOND> MOND) {
            
            // PH
            if (survey.ph != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "PH";
                md.MOND_RDNG = Convert.ToString(survey.ph);
                md.MOND_UNIT = "PH";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

            //Redox Potential
            if (survey.redox_potential != null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "RDX";
                md.MOND_RDNG = Convert.ToString(survey.redox_potential);
                md.MOND_UNIT = "mV";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

            // Electrical Conductivity
            if (survey.conductivity != null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "EC";
                md.MOND_RDNG = Convert.ToString(survey.conductivity);
                md.MOND_NAME = "Electrical Conductivity";
                md.MOND_UNIT = "uS/cm";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

            //Temperature (°C)
            if (survey.temperature != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DOWNTEMP";
                md.MOND_RDNG = Convert.ToString(survey.temperature);
                md.MOND_NAME = "Downhole temperature";
                md.MOND_UNIT = "Deg C";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

            //Dissolved oxygen (mg/l)
            if (survey.dissolved_oxy != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DO";
                md.MOND_RDNG = Convert.ToString(survey.dissolved_oxy);
                md.MOND_NAME = "Dissolved Oxygen";
                md.MOND_UNIT = "mg/l";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }

         
              //Turbidity
            if (survey.turbitity != null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "TURB";
                md.MOND_RDNG = Convert.ToString(survey.turbitity);
                md.MOND_NAME = "Turbidity";
                md.MOND_UNIT = "NTU";
                md.MOND_INST = IfOther(survey.purg_meter, survey.purg_meter_other);
                if (survey.purg_time_strt!=null) {
                md.DateTime  = gINTDateTime(survey.purg_time_strtgetDT()).Value;
                }
                MOND.Add(md);
            }
return 0;

}
public int AddTopo(MONG mg, LTM_Survey_Data2 survey, List<MOND> MOND) {
            
            // surv_g_level	Surveyed Ground Level (mAOD)	Surveyed level of ground
            // surv_ToC	Surveyed Top of Cover (mAOD)	surveyed level of Top of Cover


            if (survey.surv_g_level != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "LEV";
                md.MOND_RDNG = Convert.ToString(survey.surv_g_level);
                md.MOND_NAME = "Ground Level";
                md.MOND_UNIT = "m";
                md.MOND_INST = "GNSS Instrument";
                md.MOND_REM = survey.surv_com;
                if (survey.surv_time!=null) {
                md.DateTime  = gINTDateTime(survey.surv_time_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // meas_ToC	Measured Top of Cover - A(m)	Tape measured height difference (distance) between ground level and Top of Cover
            // meas_ToI	Measured Top of Installed pipe - B (m)	Tape measured height difference (distance) between ground level and Top of Installed pipe

            if (survey.meas_ToC != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DSTL";
                md.MOND_RDNG = Convert.ToString(survey.meas_ToC);
                md.MOND_NAME = "Distance from GL to Top of Cover";
                md.MOND_UNIT = "m";
                md.MOND_REM = survey.surv_com;
                if (survey.surv_time!=null) {
                md.DateTime  = gINTDateTime(survey.surv_time_getDT()).Value;
                }
                MOND.Add(md);
            }

        return 0;
}

public int AddDip(MONG mg, LTM_Survey_Data2 survey, List<MOND> MOND) {

            // Water depth below gl (m)
            
            if (survey.depth_gwl_bgl != null) {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "WDEP";
                md.MOND_RDNG = Convert.ToString(survey.depth_gwl_bgl);
                md.MOND_UNIT = "m";
                md.MOND_INST = "Dipmeter: " + IfOther(survey.dip_instr, survey.dip_instr_other);
                md.MOND_REM = survey.dip_com;
                if (survey.dip_time!=null) {
                md.DateTime  = gINTDateTime(survey.dip_time_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // Water depth below gl (m)
            if (survey.dip_check == "dry") {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "WDEP";
                md.MOND_RDNG = "Dry";
                md.MOND_UNIT = "m";
                md.MOND_INST = "Dipmeter: " + IfOther(survey.dip_instr, survey.dip_instr_other);
                md.MOND_REM = survey.dip_com;
                if (survey.dip_time!=null) {
                md.DateTime  = gINTDateTime(survey.dip_time_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // Water depth below gl (m)
            if (survey.depth_gwl_bgl == null && survey.dip_req == "yes" && survey.dip_datum_offset!=null && survey.dip_or_pressure != "PressureGauge") {
                MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "WDEP";
                md.MOND_RDNG = "Dry";
                md.MOND_UNIT = "m";
                md.MOND_INST = "Dipmeter: " + IfOther(survey.dip_instr, survey.dip_instr_other);
                md.MOND_REM = survey.dip_com;
                if (survey.dip_time!=null) {
                md.DateTime  = gINTDateTime(survey.dip_time_getDT()).Value;
                }
                MOND.Add(md);
            }    

            // Depth to base of instalation (m)
            if (survey.depth_install_bgl != null && survey.dip_datum_offset !=null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "DBSE";
                md.MOND_RDNG = Convert.ToString(survey.depth_install_bgl);
                md.MOND_NAME = "Depth to base of installation";
                md.MOND_UNIT = "m";
                md.MOND_INST = IfOther(survey.dip_instr, survey.dip_instr_other);
                if (survey.dip_time!=null) {
                md.DateTime  = gINTDateTime(survey.dip_time_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // Sub aquifer Water pressure (bar)
            if (survey.water_pressure != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "PRES";
                md.MOND_RDNG = Convert.ToString(survey.water_pressure);
                md.MOND_NAME = "Water pressure";
                md.MOND_UNIT = "bar";
                md.MOND_INST = "Pressure gauge on borehole";
                if (survey.dip_time!=null) {
                md.DateTime  = gINTDateTime(survey.dip_time_getDT()).Value;
                }
                MOND.Add(md);
            }
            return 0;
}
public int AddGas(MONG mg, LTM_Survey_Data2 survey, List<MOND> MOND) {

         //   DateTime survey_startDT = gINTDateTime(survey.date1_getDT()).Value;
            
         //   DateTime survey_endDT = gINTDateTime(survey.time2_getDT()).Value;
          
                        // Peak Gas flow
            if (survey.gas_flow_peak != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GFLOP";
                md.MOND_RDNG = Convert.ToString(survey.gas_flow_peak);
                md.MOND_NAME = "Peak gas flow rate";
                md.MOND_UNIT = "l/h";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
               MOND.Add(md);
            }

            // Steady Gas flow
            if (survey.gas_flow_steady != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GFLOS";
                md.MOND_RDNG = Convert.ToString(survey.gas_flow_steady);
                md.MOND_NAME = "Steady gas flow rate";
                md.MOND_UNIT = "l/h";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }

            // Peak diff barometric pressure (mbar?)
            if (survey.BH_pressure_peak != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GPRP";
                md.MOND_RDNG = Convert.ToString(survey.BH_pressure_peak);
                md.MOND_NAME = "Peak diff barometric pressure";
                md.MOND_UNIT = Convert.ToString(survey.BH_pressure_units);
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }

            // Steady diff barometric pressure (mbar?) 
            if (survey.BH_pressure_steady != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GPRS";
                md.MOND_RDNG = Convert.ToString(survey.BH_pressure_steady);
                md.MOND_NAME = "Steady diff barometric pressure";
                md.MOND_UNIT = Convert.ToString(survey.BH_pressure_units);;
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // Ambient Methane concentration 
            if (survey.ambi1_CH4 != null) {
             MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "TGMA";
                md.MOND_RDNG = Convert.ToString(survey.ambi1_CH4);
                md.MOND_NAME = "Ambient methane concentration";
                md.MOND_UNIT = "%vol";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
          
            // Ambient Methane concentration 
            if (survey.ambi1_CH4_lel != null) {
            MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GMA";
                md.MOND_RDNG = Convert.ToString(survey.ambi1_CH4_lel);
                md.MOND_NAME = "Ambient methane concentration";
                md.MOND_UNIT = "%LEL";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
               if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
           
            // Ambient Oxygen concentration 
            if (survey.ambi1_O2 != null) {
            MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GOXA";
                md.MOND_RDNG = Convert.ToString(survey.ambi1_O2);
                md.MOND_NAME = "Ambient oxygen concentration";
                md.MOND_UNIT = "%vol";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
               if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
           
            // Ambient Carbon Dioxide concentration 
            if (survey.ambi1_CO2 != null) {
            MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "GCDA";
                md.MOND_RDNG = Convert.ToString(survey.ambi1_CO2);
                md.MOND_NAME = "Ambient carbon dioxide concentration";
                md.MOND_UNIT = "%vol";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
               if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
            
            // Ambient Carbon Dioxide concentration 
            if (survey.PID_t != null) {
            MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "VOC";
                md.MOND_RDNG = Convert.ToString(survey.PID_t);
                md.MOND_NAME = "Volatile organic compounds";
                md.MOND_UNIT = "ppm";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }

            // Atmosperic Temperature (°C)
            if (survey.atmo_temp != null ) {
               MOND md = NewMOND(mg, survey);  
                md.MOND_TYPE = "TEMP";
                md.MOND_RDNG = Convert.ToString(survey.atmo_temp);
                md.MOND_NAME = "Atmospheric temperature";
                md.MOND_UNIT = "Deg C";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }

            // Atmospheric pressure (mbar)
            if (survey.atmo_pressure != null) {
               MOND md = NewMOND(mg, survey); 
                md.MOND_TYPE = "BAR";
                md.MOND_RDNG = Convert.ToString(survey.atmo_pressure);
                md.MOND_NAME = "Atmospheric pressure";
                md.MOND_UNIT = "mbar";
                md.MOND_INST = IfOther(survey.gas_instr, survey.gas_instr_other);
                if (survey.gas_repeat_tstart!=null) {
                md.DateTime  = gINTDateTime(survey.gas_repeat_tstart_getDT()).Value;
                }
                MOND.Add(md);
            }
            
           

            return 0;
}


}


}
