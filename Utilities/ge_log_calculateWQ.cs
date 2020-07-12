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
  public class ge_log_processWQ {
    public Guid Id {get;set;}
    public string bh_ref {get;set;}
    public float probe_depth {get;set;}
  }




  public class ge_log_calculateWQ : _log {

  
    public int Calculate(float? PROBE_DEPTH_OVERRIDE_M, 
                         string BHOLE_REF_OVERRIDE) {
                           
       AddOverrides (PROBE_DEPTH_OVERRIDE_M,
                      BHOLE_REF_OVERRIDE);
       AddWaterDepthM();
       AddDuration();
       log_file.packFileHeader();
       log_file.packFieldHeaders();
       
       return 0;
    }


  private int AddDuration() {

    value_header log_Duration= log_file.getDuration();

   if (log_Duration==null) {
      log_Duration = new value_header {
                                id = ge_log_constants.DURATION,
                                units ="s",
                                comments =$"Calculated test durations (s) from ordered ReadingDateTime)",
                                source = ge_log_constants.SOURCE_CALCULATED};
      log_file.addDuration();
    }
 
    return 0;
  }


  private int AddWaterDepthM() {
        
        int wdepth_success = NOT_OK;

        value_header log_WDepthM = log_file.getHeaderByIdUnits(ge_log_constants.WDEPTH,"m");

        if (log_WDepthM !=null) {
            log_file.removeHeader (log_WDepthM);
            log_WDepthM = null;

        }

        value_header log_WDepth  = log_file.getHeaderById("WDEPTH1");
       
        if  (log_WDepth !=null) {
            switch (log_WDepth.units) {
                    case "cm": {
                        log_WDepthM = new value_header {
                                id = ge_log_constants.WHEAD,
                                units ="m",
                                comments =$"Calculated conversion of {log_WDepth.db_name} from cm to m ({ge_log_constants.FACTOR_cmH20_to_mH20})",
                                source = ge_log_constants.SOURCE_CALCULATED};
                        log_file.addHeader(log_WDepthM);
                        log_file.addValues(log_WDepth.db_name,ge_log_constants.FACTOR_cmH20_to_mH20, log_WDepthM.db_name);
                        wdepth_success = 0;
                        break;       
                    }
                    default: {
                       wdepth_success = NOT_OK; 
                       break;
                    }
            }    
                       
            } else {
            wdepth_success = NOT_OK;
        }
          
        return wdepth_success;

    }

      
  }

}
