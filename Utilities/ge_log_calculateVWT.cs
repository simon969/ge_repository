using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
using ge_repository.Models;
using Newtonsoft.Json;
using System.Xml.Serialization;
using ge_repository.Extensions;

namespace ge_repository.OtherDatabase  {

  public class ge_log_calculateVWT : _log_calculate {
        
        /* Calculation class to calculate WHEAD_NET (m) accounting for reference pressure barometric pressure thermal pressure
            WHEAD represents the height of the column of water above the probe depth
        */

        [Display(Name = "Zero Reading")] public float ZeroReading {get;set;} 
        [Display (Name = "Zero Temperature")] public float ZeroTemperature {get;set;}
        [Display (Name = "Channel Ref")] public string ChannelRef {get;set;}
        [Display (Name = "Linear Factor")] public float LinearFactor {get;set;}
        [Display (Name = "Barometric Pressure")] public float BaroPressure {get;set;}
        [Display (Name = "Temperature At Cal")] public float TempatCal {get;set;}
        [Display (Name = "Constant Factor A")] public float ConstA {get;set;}
        [Display (Name = "Constant Factor B")] public float ConstB {get;set;}
        [Display (Name = "Constant Factor C")] public float ConstC {get;set;}
        [Display (Name = "Constant Factor T")] public float ConstT {get;set;}
        [Display (Name = "Probe Depth")] public float ProbeDepth {get;set;}
      
        public override int Calculate(int? BARO_BUFFER_MINS,
                             float? ATMOS_HEAD_M,
                             float? OFFSET_OVERRIDE_M,
                             float? PROBE_DEPTH_OVERRIDE_M, 
                             string BHOLE_REF_OVERRIDE,
                             float? DRY_DEPTH_M
                             ) {
                
                int over_ride = AddOverrides( BARO_BUFFER_MINS,
                                            ATMOS_HEAD_M,
                                            OFFSET_OVERRIDE_M,
                                            PROBE_DEPTH_OVERRIDE_M,
                                            BHOLE_REF_OVERRIDE,
                                            DRY_DEPTH_M,
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            null);


                int conversionFactors = findConversionFactors();
                
                if (conversionFactors == NOT_FOUND ) {
                    return NOT_FOUND;
                }
            
                string method = log_file.getCompensationMethod(COMPENSATE_HEAD);
             
                if (method == COMPENSATE_HEAD || String.IsNullOrEmpty(method)) {
                    CompensateHead();
                }

                if (method==COMPENSATE_HEAD_DIFF) {
                    CompensateHeadDifferential();
                }
            
                log_file.packFileHeader();
                log_file.packFieldHeaders();
                
                return 0;
        }
                public int Calculate(int? BARO_BUFFER_MINS,
                             float? ATMOS_HEAD_M,
                             float? OFFSET_OVERRIDE_M,
                             float? PROBE_DEPTH_OVERRIDE_M, 
                             string BHOLE_REF_OVERRIDE,
                             float? DRY_DEPTH_M,
                             float? ZERO_READING,
                             float? ZERO_TEMP,
                             float? LINEAR_FACTOR,
                             float? BARO_PRESSURE_M,
                             float? TEMP_AT_CAL,
                             float? CONST_A,
                             float? CONST_B,
                             float? CONST_C,
                             float? CONST_T
                             ) {
                
                int over_ride = AddOverrides( BARO_BUFFER_MINS,
                                            ATMOS_HEAD_M,
                                            OFFSET_OVERRIDE_M,
                                            PROBE_DEPTH_OVERRIDE_M,
                                            BHOLE_REF_OVERRIDE,
                                            DRY_DEPTH_M,
                                            ZERO_READING,
                                            ZERO_TEMP,
                                            LINEAR_FACTOR,
                                            BARO_PRESSURE_M,
                                            TEMP_AT_CAL,
                                            CONST_A,
                                            CONST_B,
                                            CONST_C,
                                            CONST_T
                                            );


                int conversionFactors = findConversionFactors();
                
                int conversionFactorOverrides = applyConversionFactorsOverrides();

                if (conversionFactors == NOT_FOUND ) {
                    return NOT_FOUND;
                }
            
                string method = log_file.getCompensationMethod(COMPENSATE_HEAD);
             
                if (method == COMPENSATE_HEAD || String.IsNullOrEmpty(method)) {
                    CompensateHead();
                }

                if (method==COMPENSATE_HEAD_DIFF) {
                    CompensateHeadDifferential();
                }
            
                log_file.packFileHeader();
                log_file.packFieldHeaders();
                
                return 0;
        }
        private void CompensateHeadDifferential() {

        int whead_added = addWHeadMfromVWRAWChannel();

        int thead_added = addHeadTfromTemperature();
        
        int baro_head_added = AddBaroHead();

        int w_head_net_added = AddHeadNetM_SubtractDifferential();

        int w_depth_added = addWDepthM();
        }
        
        private void CompensateHead() {


                int whead_added = addWHeadMfromVWRAWChannel();
                
                int thead_added = addHeadTfromTemperature();
                
                int baro_head_added = AddBaroHead();
            
                int w_head_net_added = AddHeadNetM();
        
                int w_depth_added = addWDepthM();

        }

        private int addWHeadMfromVWRAWChannel() {
                
                value_header channel_raw = log_file.getHeaderById(ge_log_constants.VWTRAWREADING);
                value_header temp = log_file.getHeaderById(ge_log_constants.TEMP);
                string method = log_file.getConversionMethod(LINEAR_CONVERSION);
                
                if (channel_raw==null) {
                    return -1;
                }

                // Convert VWT readings to water head (m)'
                value_header head_m = log_file.getHeaderById(ge_log_constants.WHEAD);
                
                if (head_m==null) {
                head_m = new value_header {
                                            id = ge_log_constants.WHEAD,
                                            units ="m",
                                            source = ge_log_constants.SOURCE_CALCULATED};
                log_file.addHeader(head_m);
                }
                
                if (method == LINEAR_CONVERSION) {
                log_file.addAddMultiply(channel_raw.db_name,-ZeroReading, LinearFactor, head_m.db_name);
                head_m.comments = $"(ZeroReading={ZeroReading}) - (Raw Channel Reading={channel_raw.db_name}) * (LinearFactor={LinearFactor}";
                head_m.units ="m";
                return 0;
                }
                
                if (method == POLY_CONVERSION) {
                float polyFactor = log_file.getPolyFactor(1);   
                
                ConstA = ConstA * polyFactor;
                ConstB = ConstB * polyFactor;
                
                float ZeroReading2 = Convert.ToSingle(Math.Pow(Convert.ToDouble(ZeroReading),2.00));
                ConstC = -ConstA * ZeroReading2 - ConstB * ZeroReading;

                log_file.addPoly(channel_raw.db_name, ConstA, ConstB, ConstC, head_m.db_name);
                
                head_m.comments = $"(ConstA={ConstA}) * {channel_raw.db_name} ^ 2 + (ConstB={ConstB}) * {channel_raw.db_name} + (ConstC={ConstC}) (polyFactor={polyFactor})";
                return 0;
                }

                return -1;

        }

        private int addHeadTfromTemperature(){
                
                value_header channel_temp = log_file.getHeaderById(ge_log_constants.TEMP);
                
                if (channel_temp==null) {
                    return -1;
                }

                // Calculate head due to thermal variations (m)'
                value_header temp_m = log_file.getHeaderById(ge_log_constants.THEAD);
                               
                if (temp_m==null) {
                temp_m = new value_header {
                                            id = ge_log_constants.THEAD,
                                            units ="m",
                                            source = ge_log_constants.SOURCE_CALCULATED};
                log_file.addHeader(temp_m);
                }
                
                log_file.addAddMultiply(channel_temp.db_name, -ZeroTemperature, Math.Abs(ConstT), temp_m.db_name);

                temp_m.comments = $"({channel_temp.db_name} - ZeroT={ZeroTemperature}) * Math.Abs(ConstT {ConstT})";
                temp_m.units = "m";

                return 0;


        }

              
        private int findConversionFactors() {
            
            int colChannel = log_file.getChannelColId("CalibrationFactors");

            if (colChannel != NOT_FOUND) {
           
                ZeroReading = log_file.getArrayValue("ZeroReading", colChannel);
                ZeroTemperature = log_file.getArrayValue("ZeroTemperature", colChannel);
                
                LinearFactor = log_file.getArrayValue("LinearFactor", colChannel);
                BaroPressure = log_file.getArrayValue("BaroPressure", colChannel);
                TempatCal = log_file.getArrayValue("TempatCal", colChannel);
                ProbeDepth = log_file.getArrayValue("SensorName",colChannel);
                
                ConstA = log_file.getArrayValue("ConstA", colChannel);
                ConstB = log_file.getArrayValue("ConstB", colChannel);
                ConstC = log_file.getArrayValue("ConstC", colChannel);
                ConstT = log_file.getArrayValue("ConstT", colChannel);
            
                return 0;
           
            }
            
            return NOT_FOUND;
        }

        private int applyConversionFactorsOverrides() {
            
           try {
                ZeroReading = log_file.getOverride("zero_reading", ZeroReading);
                ZeroTemperature = log_file.getOverride("zero_temp", ZeroTemperature);
                
                LinearFactor = log_file.getOverride("linear_factor", LinearFactor);
                BaroPressure = log_file.getOverride("baro_pressure", BaroPressure);
                TempatCal = log_file.getOverride("temp_at_cal", TempatCal);
                              
                ConstA = log_file.getOverride("const_a", ConstA);
                ConstB = log_file.getOverride("const_b", ConstB);
                ConstC = log_file.getOverride("const_c", ConstC);
                ConstT = log_file.getOverride("const_t", ConstT);

                return 0;       
            
            } catch (Exception e) {
                Console.Write (e.Message);
                return -1;
            }
            
           
        }
        
        
        
  }
}
