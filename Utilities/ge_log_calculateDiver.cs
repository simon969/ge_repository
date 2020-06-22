using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
using ge_repository.Models;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace ge_repository.OtherDatabase  {

       public class ge_log_calculateDiver : _log_calculate {
       
       /* Calculation class to calculate WHEAD (m) accounting for 
            reference pressure
            barometric pressure
            thermal pressure

            WHEAD represents the height of the column of water above the probe depth
        */

       
    public override int Calculate (int? BARO_BUFFER_MINS,
                          float? ATMOS_HEAD_M,
                          float? OFFSET_OVERRIDE_M,
                          float? PROBE_DEPTH_OVERRIDE_M,
                          string BHOLE_REF_OVERRIDE,
                          float? DRY_DEPTH_M
                                  ) {
            if (log_file==null) {
                return -1;
            }
            
            int over_ride = AddOverrides(   BARO_BUFFER_MINS,
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

            
            string method = log_file.getCompensationMethod(COMPENSATE_HEAD);
            
            if (method == COMPENSATE_HEAD || String.IsNullOrEmpty(method)) {
                CompensateHead();
            }

            if (method==COMPENSATE_HEAD_DIFF) {
                CompensateHeadDifferential();
            }
            
            if (method==COMPENSATE_DEPTH_DIFF) {
                CompensateDepthDifferential();
            }

          //  if (baro_file!=null) {
          //  baro_file.packFileHeader();    
          //  baro_file.packFieldHeaders();
          //  }
            
           
            
            log_file.packFileHeader();
            log_file.packFieldHeaders();

           return 0;
          
    }

    private void CompensateHead() {

            int whead_added = AddWHeadM();
            if (whead_added == NOT_OK) return;

            int baro_head_added = AddBaroHead();
            
            int w_head_net_added = AddHeadNetM();
        
            int w_depth_added = addWDepthM();

    }
    private void CompensateDepthDifferential() {

        int baro_head_added = AddBaroHead();

        int w_depth_net_added = AddDepthNetM_AddDifferential();

        int w_depth_added = addWDepthMFromNetDepthM();
    }

     private void CompensateHeadDifferential() {

        int whead_added = AddWHeadM();
        if (whead_added == NOT_OK) return;

        int baro_head_added = AddBaroHead();

        int w_head_net_added = AddHeadNetM_SubtractDifferential();

        int w_depth_added = addWDepthM();
    }
     
    
    private int AddDepthNetM_AddDifferential() {
        
        int whead_success = NOT_OK;

        value_header log_wdepth1M = log_file.getHeaderByIdUnits("WDEPTH1","m");
        value_header log_baroM  = log_file.getHeaderByIdUnits("BAROHEAD","m");    

        if (log_wdepth1M==null || log_baroM==null) {
            return 0;
        }
        
        value_header log_netdepthM = log_file.getHeaderByIdUnits(ge_log_constants.NETDEPTH,"m");     
        
        if (log_netdepthM==null) {
            log_netdepthM = new value_header {
                            id = ge_log_constants.NETDEPTH,
                            units ="m",
                            source = ge_log_constants.SOURCE_CALCULATED};
            log_file.addHeader (log_netdepthM);
        }
        
        // Order list so that differential will work 
        List<ge_log_reading> ordered  = log_file.readings.OrderBy(e=>e.ReadingDatetime).ToList();

        log_file.readings = ordered;
        log_file.addDifferential(log_wdepth1M.db_name, log_baroM.db_name, log_netdepthM.db_name);
        log_netdepthM.comments = $"Water depth with barometric differential removed {log_wdepth1M.db_name}+({log_baroM.db_name}j-{log_baroM.db_name}i)";
        
        return whead_success;

    }
    private int AddWHeadM() {
        
        int whead_success = NOT_OK;

        value_header log_headM = log_file.getHeaderByIdUnits(ge_log_constants.WHEAD,"m");

        if (log_headM !=null) {
            log_file.removeHeader (log_headM);
            log_headM = null;

        }

        value_header log_press  = log_file.getSourceWaterPressureHeader();
       
        if  (log_press !=null) {
            switch (log_press.units) {
                    case "kPa": {
                        log_headM = new value_header {
                                id = ge_log_constants.WHEAD,
                                units ="m",
                                comments =$"Calculated conversion of {log_press.db_name} from kPa to m ({ge_log_constants.FACTOR_kPa_to_mH20})",
                                source = ge_log_constants.SOURCE_CALCULATED};
                        log_file.addHeader(log_headM);
                        log_file.addValues(log_press.db_name,ge_log_constants.FACTOR_kPa_to_mH20, log_headM.db_name);
                        whead_success = 0;
                        break;   
                    }
                    case "PSI": {
                        log_headM = new value_header {
                                id = ge_log_constants.WHEAD,
                                units ="m",
                                comments =$"Calculated conversion of {log_press.db_name} from PSI to m ({ge_log_constants.FACTOR_PSI_to_mH20})",
                                source = ge_log_constants.SOURCE_CALCULATED};
                        log_file.addHeader(log_headM);
                        log_file.addValues(log_press.db_name,ge_log_constants.FACTOR_PSI_to_mH20, log_headM.db_name);
                        whead_success = 0;
                        break;       
                    }
                    case "cm": {
                        log_headM = new value_header {
                                id = ge_log_constants.WHEAD,
                                units ="m",
                                comments =$"Calculated conversion of {log_press.db_name} from cm to m ({ge_log_constants.FACTOR_cmH20_to_mH20})",
                                source = ge_log_constants.SOURCE_CALCULATED};
                        log_file.addHeader(log_headM);
                        log_file.addValues(log_press.db_name,ge_log_constants.FACTOR_cmH20_to_mH20, log_headM.db_name);
                        whead_success = 0;
                        break;       
                    }
                    case "mbar": {
                        log_headM = new value_header {
                                id = ge_log_constants.WHEAD,
                                units ="m",
                                comments =$"Calculated conversion of {log_press.db_name} from mbar to m ({ge_log_constants.FACTOR_mbar_to_mH20})",
                                source = ge_log_constants.SOURCE_CALCULATED};
                        log_file.addHeader(log_headM);
                        log_file.addValues(log_press.db_name,ge_log_constants.FACTOR_mbar_to_mH20, log_headM.db_name);
                        whead_success = 0;
                        break;       
                    }
                    default: {
                       whead_success = NOT_OK; 
                       break;
                    }
            }    
                       
            } else {
            whead_success = NOT_OK;
        }
          
        return whead_success;

    }

   
    private int addWDepthMFromNetDepthM() {
            
            float offset = log_file.getOffset(0);
                     
            value_header log_netdepthM = log_file.getHeaderByIdUnits(ge_log_constants.NETDEPTH,"m");     

            if (log_netdepthM==null) {
                return -1;
            }
                      
            value_header log_wdepthM = log_file.getHeaderById(ge_log_constants.WDEPTH);
              
            if (log_wdepthM == null) {
                log_wdepthM = new value_header {
                            id = ge_log_constants.WDEPTH,
                            units ="m",
                            comments = $"Water depth below datum ({log_netdepthM.db_name}-{offset})",
                            source = ge_log_constants.SOURCE_CALCULATED};
                        log_file.addHeader (log_wdepthM);
            }            
           
            log_file.addConstant(log_netdepthM.db_name, -offset, log_wdepthM.db_name);
            log_wdepthM.comments = $"Water depth below datum ({log_netdepthM.db_name}-{offset})";          
            return 0;

        }

         
    }

    
}
