using System;
using System.IO;
using System.Data;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
// https://stackoverflow.com/questions/29672383/streaming-data-from-a-nvarcharmax-column-using-c-sharp

namespace ge_repository.OtherDatabase  {

public static class logExtensions {

public static string calcReadingAggregates(this ge_log_file file) {

    aggregate_reading ar = new aggregate_reading();
    ar.count = file.readings.Count();
    ar.maxReadingDate = file.readings.Max(r=>r.ReadingDateTime);
    ar.minReadingDate = file.readings.Min(r=>r.ReadingDateTime);
    
    if (file.getHeader1()!=null) {
        try {
            ar.Value1= new ValueRange {
            max = file.readings.Max(r=>r.Value1.Value),
            min = file.readings.Min(r=>r.Value1.Value),
            } ;
        } catch {};
    }
    if (file.getHeader2()!=null) {
        try {
            ar.Value2 = new ValueRange {
            max = file.readings.Max(r=>r.Value2.Value),
            min = file.readings.Min(r=>r.Value2.Value),
            } ;
        } catch {};
    }
    
    if (file.getHeader3()!=null) {
        try {
            ar.Value3 = new ValueRange {
            max = file.readings.Max(r=>r.Value3.Value),
            min = file.readings.Min(r=>r.Value3.Value),
            };
        } catch {};
    }
    
    if (file.getHeader4()!=null) {
        try {
            ar.Value4 = new ValueRange {
            max = file.readings.Max(r=>r.Value4.Value),
            min = file.readings.Min(r=>r.Value4.Value),
            };
        } catch {};
    }
    
    if (file.getHeader5()!=null) {
        try {
            ar.Value5 = new ValueRange {
            max = file.readings.Max(r=>r.Value5.Value),
            min = file.readings.Min(r=>r.Value5.Value),
            };
        } catch {};
    }
    if (file.getHeader6()!=null) {
        try {
            ar.Value6 = new ValueRange {
            max = file.readings.Max(r=>r.Value6.Value),
            min = file.readings.Min(r=>r.Value6.Value),
            };
        } catch{};
    }
    
    file.readingAggregates = JsonConvert.SerializeObject(ar);
    
    return file.readingAggregates;
}


public static void AddRoundRef(this ge_log_file log_file, string ROUND_REF) {
    
    if (!String.IsNullOrEmpty(ROUND_REF)) {
            search_item round_ref = new search_item {
                                name = "round_ref",
                                value = ROUND_REF,
                                units ="",
                                comments = "Monitoring Round Reference (MOND_REF)",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                log_file.AddReplaceSearchItem (round_ref);
            }
}


}
}

