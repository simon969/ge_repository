using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
using ge_repository.Models;
using Newtonsoft.Json;
using System.Xml.Serialization;
using Microsoft.Extensions.Options;

namespace ge_repository.OtherDatabase  {

    public static class ge_log_constants {

        public static string BAROPRESSURE = "BAROPRESSURE";
        public static string PRESSURE ="PRESSURE";
        public static string NETPRESSURE ="NETPRESSURE";
        public static string VWTRAWREADING="VW_RAW";
        public static string WDEPTH = "WDEPTH";
        public static string TEMP ="TEMP";
        public static string THEAD ="THEAD";
        public static string WHEAD = "WHEAD";
        public static string BAROHEAD ="BAROHEAD";
        public static string NETHEAD ="WHEAD_NET";
        public static string NETDEPTH ="WDEPTH_NET";
        public static string MMHG ="mmHg";

       // mercury conversion factor fixed 2020-06-15 previous value was
       // public static float FACTOR_mmHg_to_mH20 = 0.013951F;
        public static float FACTOR_mmHg_to_mH20 = 0.013595476F;
        //https://www.sensorsone.com/pressure-converter/?frval=1&frunit=mmHg+0%C2%B0C&frfctr=mmHg0C&tounit=mH%E2%82%82O+4%C2%B0C&tofctr=mH2O4C

        public static float FACTOR_mmHg_to_kPa = 0.133322387F;
        //https://www.sensorsone.com/pressure-converter/?frval=1&frunit=mmHg+0%C2%B0C&frfctr=mmHg0C&tounit=kPa&tofctr=kPa

        public static float FACTOR_mH20_to_kPa = 9.80665F;
        //https://www.sensorsone.com/pressure-converter/?frval=1&frunit=mH%E2%82%82O+4%C2%B0C&frfctr=mH2O4C&tounit=kPa&tofctr=kPa

        public static float FACTOR_kPa_to_mH20 = 0.101971621F;
        //https://www.sensorsone.com/pressure-converter/?frval=1&frunit=kPa&frfctr=kPa&tounit=mH%E2%82%82O+4%C2%B0C&tofctr=mH2O4C

        public static float FACTOR_PSI_to_mH20 = 0.70306958F;
        //https://www.sensorsone.com/pressure-converter/?frval=1&frunit=psi&frfctr=psi&tounit=mH%E2%82%82O+4%C2%B0C&tofctr=mH2O4C

        public static float FACTOR_cmH20_to_mH20 = 0.01F;
        //https://www.sensorsone.com/pressure-converter/?frval=1&frunit=cmH%E2%82%82O+4%C2%B0C&frfctr=cmH2O4C&tounit=mH%E2%82%82O+4%C2%B0C&tofctr=mH2O4C

        public static float FACTOR_mbar_to_mH20 = 0.0101971621F;
        //https://www.sensorsone.com/pressure-converter/?frval=1&frunit=cmH%E2%82%82O+4%C2%B0C&frfctr=cmH2O4C&tounit=mH%E2%82%82O+4%C2%B0C&tofctr=mH2O4C

        public static string SOURCE_CALCULATED = "calculated";
        public static string SOURCE_ACTUAL = "actual";
        public static string SOURCE_ASSIGNED = "assigned";
        public static int ISDRY = -1;
        public static int ISNOTDRY = 0;
        public static string LOG_DIVER ="diver";
        public static string LOG_VWIRE ="vwt";
        public static string LOG_WQ ="wq";
        public static string READINGDATETIME = "ReadingDateTime";
        public static string DURATION = "Duration";
        public static string VALUE1 = "Value1";
        public static string VALUE2 = "Value2";
        public static string VALUE3 = "Value3";
        public static string VALUE4 = "Value4";
        public static string VALUE5 = "Value5";
        public static string VALUE6 = "Value6";
        public static string VALUE7 = "Value7";
        public static string VALUE8 = "Value8";
        public static string VALUE9 = "Value9";
        public static string VALUE10 = "Value10";
        public static string VALUE11 = "Value11";
        public static string VALUE12 = "Value12";
        public static string VALUE13 = "Value13";
        public static string VALUE14 = "Value14";
        public static string VALUE15 = "Value15";
        public static string VALUE16 = "Value16";
        public static string VALUE17 = "Value17";
        public static string VALUE18 = "Value18";
        public static int NOT_FOUND = -1;
        public static int OK = 0;

        public static int MAX_FIELD_INDEX = 7;
    }
    public class ge_log_reading {
    
        public Guid Id {get;set;}

        [Display(Name = "Log File Id")] public Guid fileId {get;set;}
        virtual public ge_log_file file {set;get;}
        [Display(Name = "Reading DateTime")] public DateTime ReadingDatetime {get;set;} 
        [Display(Name = "Reading Duration")] public long Duration {get;set;} 
        [Display(Name = "Value1")] public float Value1 {get;set;} 
        [Display(Name = "Value2")] public float? Value2 {get;set;} 
        [Display(Name = "Value3")] public float? Value3 {get;set;} 
        [Display(Name = "Value4")] public float? Value4 {get;set;}   
        [Display (Name = "Value5")] public float? Value5 {get;set;}
        [Display (Name = "Value6")] public float? Value6 {get;set;}
        [Display (Name = "Value7")] public float? Value7 {get;set;}
        [Display(Name = "Value8")] public float Value8 {get;set;} 
        [Display(Name = "Value9")] public float? Value9 {get;set;} 
        [Display(Name = "Value10")] public float? Value10 {get;set;} 
        [Display(Name = "Value11")] public float? Value11 {get;set;}   
        [Display (Name = "Value12")] public float? Value12 {get;set;}
        [Display (Name = "Value13")] public float? Value13 {get;set;}
        [Display(Name = "Value14")] public float? Value14 {get;set;} 
        [Display(Name = "Value15")] public float? Value15 {get;set;} 
        [Display(Name = "Value16")] public float? Value16 {get;set;}   
        [Display (Name = "Value17")] public float? Value17 {get;set;}
        [Display (Name = "Value18")] public float? Value18 {get;set;}
        [Display (Name = "data_xml")] public string data_xml {get;set;}
        [Display (Name = "data_string")] public string data_string {get;set;}
        [Display (Name = "Remark")] public string Remark {get;set;}      
        [Display (Name = "Valid")] public int Valid {get;set;}
        [Display (Name = "Include")] public int Include {get;set;} 
        [Display (Name = "Processing Flag")] public int pflag {get;set;}
        [Display (Name = "NotDry")] public int NotDry {get;set;} 
        public float? getValue(string name) {
            switch (name) {
                        case "Value1": return Value1;
                        case "Value2": return Value2;
                        case "Value3": return Value3;
                        case "Value4": return Value4;
                        case "Value5": return Value5;
                        case "Value6": return Value6;
                        case "Value7": return Value7;
                        case "Value8": return Value8;
                        case "Value9": return Value9;
                        case "Value10": return Value10;
                        case "Value11": return Value11;
                        case "Value12": return Value12;
                        case "Value13": return Value15;
                        case "Value14": return Value14;
                        case "Value15": return Value15;
                        case "Value16": return Value16;
                        case "Value17": return Value17;
                        case "Value18": return Value18;
                        default: return null;
            }
        }
         public void setValue(string name, float value) {
            switch (name) {
                        case "Value1": 
                            Value1=value;
                        break;
                        case "Value2": 
                            Value2=value;
                        break;
                        case "Value3": 
                            Value3=value;
                        break;
                        case "Value4": 
                            Value4=value;
                        break;
                        case "Value5": 
                            Value5=value;
                        break;
                        case "Value6": 
                            Value6=value;    
                        break;
                         case "Value7": 
                            Value7=value;    
                        break;
                        case "Value8": 
                            Value8=value;
                        break;
                        case "Value9": 
                            Value9=value;
                        break;
                        case "Value10": 
                            Value10=value;
                        break;
                        case "Value11": 
                            Value11=value;
                        break;
                        case "Value12": 
                            Value12=value;
                        break;
                        case "Value13": 
                            Value13=value;    
                        break;
                        case "Value14": 
                            Value14=value;    
                        break;
                        case "Value15": 
                            Value15=value;
                        break;
                        case "Value16": 
                            Value16=value;
                        break;
                        case "Value17": 
                            Value17=value;    
                        break;
                        case "Value18": 
                            Value18=value;    
                        break;
            }
        }
    }
	
	public class ge_log_file {   
        public Guid Id {get;set;}
        [Display(Name = "Table Channel Name")] public string channel {get;set;}
        [Display(Name = "Field Headers")] public string fieldHeader {get;set;} 
        [Display(Name = "Reading Aggregates")] public string readingAggregates {get;set;} 
        [Display(Name = "File Header")] public string fileHeader {get;set;} 
        [Display(Name = "Comments")] public string Comments {get;set;} 
        [Display(Name = "Data source file Id")] public Guid dataId {get;set;}
        [Display(Name = "Data template Id")] public Guid templateId {get;set;}
        [Display(Name = "Related MONG GintRecId")] public int MONG_GintRecID {get;set;}
        [Display(Name = "Data template")] public string SearchTemplate {get;set;}
        public virtual List<search_item>  file_headers {get;set;}
        public virtual List<array_item>  file_array {get;set;}
        public virtual List<value_header> field_headers {get;set;}
        public virtual ge_search search_template {get;set;}
        public virtual search_table search_table {get;set;}
        public virtual ge_data data {get; set; }
        public virtual List<ge_log_reading> readings { get; set; }
        public value_header getReadingDateTime() {
            return field_headers.Find(v=>v.db_name==ge_log_constants.READINGDATETIME);
        }
        public value_header getDuration() {
            return field_headers.Find(v=>v.db_name==ge_log_constants.DURATION);
        }
        public value_header getHeader1() {
            return field_headers.Find(v=>v.db_name==ge_log_constants.VALUE1);
        }
        public value_header getHeader2() {
            return field_headers.Find(v=>v.db_name==ge_log_constants.VALUE2);
        }
        public value_header getHeader3() {
           return field_headers.Find(v=>v.db_name==ge_log_constants.VALUE3);
        }
        public value_header getHeader4() {
            return field_headers.Find(v=>v.db_name==ge_log_constants.VALUE4);
        }
        public value_header getHeader5() {
            return field_headers.Find(v=>v.db_name==ge_log_constants.VALUE5);
        }
        public value_header getHeader6() {
            return field_headers.Find(v=>v.db_name==ge_log_constants.VALUE6);
        }
        public value_header getHeader7() {
            return field_headers.Find(v=>v.db_name==ge_log_constants.VALUE7);
        }
        public List<ge_log_reading> getValidReadings(DateTime? FromDT, DateTime? ToDT) {
            return getReadings(FromDT, ToDT).Where(e=>e.Valid == 0).ToList();
        }
        public List<ge_log_reading> getIncludeReadings(DateTime? FromDT, DateTime? ToDT) {
            return getReadings(FromDT, ToDT).Where(e=>e.Include == 0).ToList();
        }
        public List<ge_log_reading> getReadings(DateTime? FromDT, DateTime? ToDT) {

        if (FromDT != null && ToDT != null) {
            return readings.Where(e=>e.ReadingDatetime >= FromDT &&
                                     e.ReadingDatetime <= ToDT 
                                     ).ToList();
        }

        if (FromDT != null) {
            return readings.Where(e=>e.ReadingDatetime >= FromDT 
                                 ).ToList();
        }
    
        if (ToDT != null) {
            return readings.Where(e=>e.ReadingDatetime <= ToDT
                                    ).ToList();
        }
 
        return readings;
        
        }
        public void init_new_file() {

            packSearchTemplate();
            packFileHeader();
            packFieldHeaders();
           
            TimeSpan time_offset = getDateTimeOffset("0:0:0");
            
            addTimeSpan(time_offset);
        
        }

        public int OrderReadings() {
            List<ge_log_reading> ordered = readings.OrderBy (e=>e.ReadingDatetime).ToList();
            readings = ordered;
            return 0;
        }

        public List<search_item> unpackFileHeader() {

            if (fileHeader==null) {
                file_headers = null;
                file_array = null;
                return null;
            }

            try {
                file_headersJSON f = JsonConvert.DeserializeObject<file_headersJSON>(fileHeader);
                file_headers = f.items;
                file_array = unpackFileArray();
                return file_headers;

            } catch (Exception e) {
                return null;
            }
            
        }
        public ge_search unpackSearchTemplate() {

            if (SearchTemplate==null) {
                search_template = null;
                return null;
            }

            try {
                search_template = JsonConvert.DeserializeObject<ge_search>(SearchTemplate);
                return search_template;

            } catch (Exception e) {
                return null;
            }
            
        }
        public string packSearchTemplate() {
            if (search_template == null) {
                SearchTemplate="";
                return "";
            }
            
           SearchTemplate = JsonConvert.SerializeObject(search_template);
            return SearchTemplate;
        }
        private List<array_item> unpackFileArray() {

            List<array_item> array_items = new List<array_item>();
            
            foreach (search_item si in file_headers) {
                if (si.split > 0 ) {
                    if (si.row_text != null) {
                        string s1 = si.row_text;
                        string[] array = s1.Split (",");
                        array_item ai = new array_item {
                            name = si.name,
                            values = array,
                        };
                        array_items.Add (ai);
                    }
                }
            }
            
            if (array_items.Count==0) {
                return null;
            }

            return array_items;
        }
        
         public string packFileHeader() {
            if (file_headers == null) {
                fileHeader="";
                return "";
            }
            
            file_headersJSON f = new file_headersJSON {
                items=file_headers};
            
            fileHeader = JsonConvert.SerializeObject(f);
            return fileHeader;
        }
        
        public List<value_header> unpackFieldHeaders() {
           
            if (fieldHeader==null) {
                field_headers = null;
                return null;
            }

            if (search_template!=null) {
                search_table = search_template.search_tables.FirstOrDefault();
                field_headers = search_table.headers;
                channel = search_table.name;
            }

            try {
                field_headersJSON f = JsonConvert.DeserializeObject<field_headersJSON>(fieldHeader);
                field_headers = f.items;
                return field_headers;
            
            } catch (Exception e) {
                return null;
            }
            
        }
        public string packFieldHeaders() {
            if (field_headers == null) {
                fieldHeader="";
                return "";
            }
            
            field_headersJSON f = new field_headersJSON {
                items=field_headers};

            fieldHeader= JsonConvert.SerializeObject(f);
            return fieldHeader;
        }
       
        public void setReadingDateTime (value_header vh) {
            setHeaderByDbName(ge_log_constants.READINGDATETIME,vh);
        }
        public void setDuration (value_header vh) {
             setHeaderByDbName(ge_log_constants.DURATION,vh);
        }
        public void setHeader1 (value_header vh) {
            setHeaderByDbName(ge_log_constants.VALUE1,vh);
        }
        public void setHeader2 (value_header vh) {
            setHeaderByDbName(ge_log_constants.VALUE2,vh);
        }
        public void setHeader3 (value_header vh) {
             setHeaderByDbName(ge_log_constants.VALUE3,vh);
        }
        public void setHeader4 (value_header vh) {
            setHeaderByDbName(ge_log_constants.VALUE4,vh);
        }
        public void setHeader5 (value_header vh) {
            setHeaderByDbName(ge_log_constants.VALUE5,vh);
        }
        public void setHeader6 (value_header vh) {
            setHeaderByDbName(ge_log_constants.VALUE6,vh);
        }
        public value_header getHeaderByDbName(string db_name) {
            return field_headers.Find(v=>v.db_name==db_name);

        }
        public value_header getHeaderById(string id) {
            return field_headers.Find(v=>v.id==id);
        }
        
        public value_header getHeaderByIdUnits(string id, string units) {
            return field_headers.Find(v=>v.id==id && v.units==units);
        }
       

       public void setHeaderByDbName(string db_name, value_header new_header) {

           foreach (value_header vh in field_headers) {
               if (vh.db_name==db_name) {
                   field_headers.Remove(vh);
                   break;
               }
            }

            field_headers.Add (new_header);
        }
        public void setHeaderById(string id, value_header new_header) {

           foreach (value_header vh in field_headers) {
               if (vh.id==id) {
                   field_headers.Remove(vh);
                   break;
               }
            }

            field_headers.Add (new_header);
        }


        public int firstAvailableIndex() {
         for (int i=1; i< ge_log_constants.MAX_FIELD_INDEX;i++)    {
             string value_test = "Value" + i.ToString();
             value_header vh = field_headers.FirstOrDefault(m=>m.db_name==value_test);
                if (vh==null) {
                    return i;
                }
         }

         return -1;
        }
        // public int currentMaxValueIndex() {

        //     int index_max = 0;
            
        //     foreach (value_header vh in field_headers) {
        //         if (vh.db_name==ge_log_constants.VALUE7 && index_max < 7) {
        //           index_max = 7;
        //         }
        //         if (vh.db_name==ge_log_constants.VALUE6 && index_max < 6) {
        //           index_max = 6;
        //         }
        //         if (vh.db_name==ge_log_constants.VALUE5 && index_max < 5) {
        //           index_max = 5;
        //         }
        //         if (vh.db_name==ge_log_constants.VALUE4 && index_max < 4) {
        //           index_max = 4;
        //         }
        //         if (vh.db_name==ge_log_constants.VALUE3 && index_max < 3) {
        //           index_max = 3;
        //         }
        //         if (vh.db_name==ge_log_constants.VALUE2 && index_max < 2) {
        //           index_max = 2;
        //         }
        //         if (vh.db_name==ge_log_constants.VALUE1 && index_max < 1) {
        //           index_max = 1;
        //         }
        //     }
        //     return index_max;
        // }
        // public int addHeader(value_header vh, Boolean replace_id=true, Boolean retain_db_name=true) {
            
        //     if (replace_id==true) {
        //         int index = field_headers.FindIndex(h=>h.id==vh.id);
        //         if (index !=-1 && String.IsNullOrEmpty(field_headers[index].db_name) == false) {
        //           if (retain_db_name==true) {
        //                 vh.db_name = field_headers[index].db_name;
        //                }
        //             field_headers[index] = vh;
        //             return index; 
        //         }
        //     }

        //     int next_index = currentMaxValueIndex() + 1;
            
        //     if (next_index > ge_log_constants.MAX_FIELD_INDEX) {
        //         return -1;
        //     }
        //     string s1 = "Value" + next_index;
        //     vh.db_name = s1;
        //     field_headers.Add (vh);
        //     return next_index;    
        // }
        public int addHeader(value_header vh) {
            
            int next_index = firstAvailableIndex();
            
            if (next_index < 0) {
                return -1;
            }

            string s1 = "Value" + next_index;
            vh.db_name = s1;
            field_headers.Add (vh);
            return next_index;    
        }
       public void removeHeader(value_header remove_vh) {
            
            foreach (value_header vh in field_headers) {
               if (vh == remove_vh) {
                   field_headers.Remove(vh);
                    return;
               }
            }
            return; 
        }
        // }
        // public value_header getBaroPressureHeader() {
        //      return getHeaderWithId(ge_log_constants.BAROPRESSURE);
        // }
        //  public value_header getBaroHeadHeader() {
        //      return getHeaderWithId(ge_log_constants.BAROHEAD);
        // }
        // public value_header getPressureHeader() {
        //     return getHeaderWithId(ge_log_constants.PRESSURE);
        // }
        // public value_header getTemperatureHeader() {
        //     return getHeaderWithId(ge_log_constants.TEMP);
        // }
        // public value_header getWaterDepthHeader() {
        //    return getHeaderWithId(ge_log_constants.WDEPTH);
        // }
        // public value_header getWaterHeadHeader() {
        //    return getHeaderWithId(ge_log_constants.WHEAD);
        // }
        public value_header getSourcePressureHeader() {
            
            List<value_header> list = field_headers.Where(v=>v.source==ge_log_constants.SOURCE_ACTUAL).ToList();

            foreach (value_header vh in list) {
                if (vh.id=="WHEAD") return vh;
                if (vh.id=="PRESSURE") return vh;
                if (vh.id=="BHEAD") return vh;
                if (vh.id=="BAROHEAD") return vh;
                if (vh.id=="BAROPRESSURE") return vh;
            }
            
            return null;

        }
        public value_header getSourceWaterPressureHeader() {
            
            List<value_header> list = field_headers.Where(v=>v.source==ge_log_constants.SOURCE_ACTUAL).ToList();

            foreach (value_header vh in list) {
                if (vh.id=="WHEAD") return vh;
                if (vh.id=="PRESSURE") return vh;
            }
            
            return null;

        }
         public value_header getSourceBarometricPressureHeader() {
            
            List<value_header> list = field_headers.Where(v=>v.source==ge_log_constants.SOURCE_ACTUAL).ToList();

            foreach (value_header vh in list) {
                if (vh.id=="BHEAD") return vh;
                if (vh.id=="BAROHEAD") return vh;
                if (vh.id=="BAROPRESSURE") return vh;
            }
            
            return null;

        }
        public string getCompensationMethod(string IsNullReturn) {

            search_item si = null;

            if (search_table !=null) {
                si = search_table.options.Find(e=>e.name.Contains("compensation_method"));
            }

            if (si !=null) {
                return si.value;
            }

            return IsNullReturn;
        }
    public TimeSpan getDateTimeOffset (string IsNullReturn) {
            
            TimeSpan? timeSpan = null;
        
            search_item si =  file_headers.Find(h=>h.name == "datetime_offset");
            
            if (si != null) {
              timeSpan = TimeSpan.Parse (si.value);
              timeSpan = -1 * timeSpan;
            }

            if (search_table !=null) {
                search_item si2 = search_table.options.Find(e=>e.name.Contains("datetime_offset"));
                    if (si2 != null) {
                        timeSpan = TimeSpan.Parse (si2.value);
                    }
            }
            
            if (timeSpan!= null) {
                return timeSpan.Value;
            }

             return TimeSpan.Parse( IsNullReturn);

        }

        public float getPolyFactor(float IsNullReturn) {

            search_item si = null;

            if (search_table !=null) {
                si = search_table.options.Find(e=>e.name.Contains("poly_factor"));
            }

            if (si !=null) {
                 return Convert.ToSingle(si.value);
            }

            return IsNullReturn;
        }

        public string getConversionMethod(string IsNullReturn) {

            search_item si = null;

            if (search_table !=null) {
                si = search_table.options.Find(e=>e.name.Contains("conversion_method"));
            }

            if (si !=null) {
                return si.value;
            }

            return IsNullReturn;
        }
        public void setReadingAggregates(aggregate_reading Aggregate_Reading) {
            readingAggregates = JsonConvert.SerializeObject(Aggregate_Reading);
        }
        public aggregate_reading getReadingAggregates() {
           return JsonConvert.DeserializeObject<aggregate_reading>(readingAggregates);
        }
         public void removeCalculatedHeaders() {
            
            List<value_header> NotCalculatedHeaders= new List<value_header>();

            foreach (value_header vh in field_headers) {
                if (vh.source!=ge_log_constants.SOURCE_CALCULATED) {
                    NotCalculatedHeaders.Add (vh);
                }
            }

            field_headers = NotCalculatedHeaders;
        } 
         public void addTimeSpan (TimeSpan sp) {
            
            value_header fh = getReadingDateTime();
            
            fh.comments = $"DateTime offset {sp.ToString()} has been applied";

            foreach (ge_log_reading r in readings) {
                DateTime? dt = r.ReadingDatetime;
                if (dt!=null) {
                    DateTime new_dt = dt.Value  + sp;
                    r.ReadingDatetime = new_dt;
                }
            }
        }
        public void addConstantSubtract (string value1, float constant, string value2 , string value3) {
            foreach (ge_log_reading r in readings) {
                float? v1 = r.getValue(value1);
                float? v2 = r.getValue(value2);
                if (v1!=null && v2!=null) {
                    float v3 = (v1.Value + constant) - v2.Value;
                    r.setValue(value3, v3);
                }
            }
        }
        public void addConstant (string value1, float constant, string value2) {
            foreach (ge_log_reading r in readings) {
                float? v1 = r.getValue(value1);
                if (v1!=null) {
                    float v2 = v1.Value + constant;
                    r.setValue(value2, v2);
                }
            }
        }
        public void addAddMultiply (string value1, float add, float factor, string value2) {
            foreach (ge_log_reading r in readings) {
                float? v1 = r.getValue(value1);
                if (v1!=null) {
                    float v2 = (v1.Value + add) * factor;
                    r.setValue(value2, v2);
                } 
            }
        }
         public void addPoly (string value1, float constA, float constB, float constC, string value2) {
            foreach (ge_log_reading r in readings) {
                float? v1 = r.getValue(value1);
                if (v1!=null) {
                    float v12 = Convert.ToSingle(Math.Pow(Convert.ToDouble(v1),2.00));
                    float v2 = v12 * constA + v1.Value * constB + constC;
                    r.setValue(value2, v2);
                } 
            }
        }
         public void subtractConstant (float constant, string value1, string value2) {
            foreach (ge_log_reading r in readings) {
                float? v1 = r.getValue(value1);
                if (v1!=null) {
                    float v2 = constant - v1.Value;
                    r.setValue(value2, v2);
                }
            }
        }
        public void setNotDry(string value1, float below_depth) {
              foreach (ge_log_reading r in readings) {
                float? v1 = r.getValue(value1);
                if (v1!=null) {
                   if (v1.Value >= below_depth){ 
                    r.NotDry = ge_log_constants.ISNOTDRY; 
                   } else {
                     r.NotDry = ge_log_constants.ISDRY;
                   }
                }
            }

        } 
        public void addValues (string value1, float factor, string value2) {
            foreach (ge_log_reading r in readings) {
                float? v1 = r.getValue(value1);
                if (v1!=null) {
                    float v2 = v1.Value*factor;
                    r.setValue(value2, v2);
                }
            }
        }
         public void addToExisting (string value1, float factor, string value2) {
            foreach (ge_log_reading r in readings) {
                float? v1 = r.getValue(value1);
                float? v2 = r.getValue(value2);
                if (v1!=null && v2!=null) {
                    float v3 = v2.Value + v1.Value * factor;
                    r.setValue(value2, v3);
                }
            }
        }
        public void addInverted (float constant, string value1, string value2) {
            foreach (ge_log_reading r in readings) {
                float? v1 = r.getValue(value1);
                if (v1!=null) {
                    float v2 = -v1.Value + constant;
                    r.setValue(value2, v2);
                }
            }
        }
        public void addValues (string value1, float value) {
            foreach (ge_log_reading r in readings) {
                r.setValue(value1,value);
            }
        }
        public void addValues (string value1, string value2,  float factor2, string value3) {
            foreach (ge_log_reading r in readings) {
                float? v1 = r.getValue(value1);
                float? v2 = r.getValue(value2);
                if (v1!=null && v2!=null) {
                    float v3 = v1.Value + v2.Value * factor2;
                    r.setValue(value3, v3);
                }
            }
        }
        public void subtractDifferential (string value1, string value2,  string value3) {
            
            ge_log_reading row0 = readings[0];

            float? v01 = row0.getValue(value1);
            
            if(v01!=null) {
                row0.setValue(value3, v01.Value);    
            }

            for (int i=1;i<readings.Count;i++) {
                ge_log_reading rowi = readings[i-1];
                ge_log_reading rowj = readings[i];
                float? v1 = rowj.getValue(value1);
                float? v2 = rowi.getValue(value2);
                float? v22 = rowj.getValue(value2);
                if (v1!=null && v2!=null & v22!=null) {
                    float v3 = v1.Value - (v22.Value - v2.Value);
                    rowj.setValue(value3, v3);
                }
            }
        }
        public void addDifferential (string value1, string value2,  string value3) {
            
            ge_log_reading row0 = readings[0];

            float? v01 = row0.getValue(value1);
            
            if(v01!=null) {
                row0.setValue(value3, v01.Value);    
            }

            for (int i=1;i<readings.Count;i++) {
                ge_log_reading rowi = readings[i-1];
                ge_log_reading rowj = readings[i];
                float? v1 = rowj.getValue(value1);
                float? v2 = rowi.getValue(value2);
                float? v22 = rowj.getValue(value2);
                if (v1!=null && v2!=null & v22!=null) {
                    float v3 = v1.Value + (v22.Value - v2.Value);
                    rowj.setValue(value3, v3);
                }
            }
        }
        public float getDryDepth(float NullReturn = -1) {
            search_item si = file_headers.Find(e=>e.name.Contains("dry_depth"));
            
            if (si != null) {
                return Convert.ToSingle(si.value);
            }

            return NullReturn;
        }
        public float getOffset(float NullReturn = 0){
        
        search_item si = file_headers.Find(e=>e.name.Contains("level_reference_value"));

        search_item si2 = file_headers.Find(e=>e.name=="offset_override");
 
        if (si2 != null) {
            return Convert.ToSingle(si2.value);
        }
        
        if (si != null) {
            double? val = si.value_dbl();
            if (val!=null) {
                return Convert.ToSingle(val.Value);
            }
        }

        return NullReturn;

    }
        public float getProbeDepth(float NullReturn = 0){
        
        search_item si1 = file_headers.Find(e=>e.name.Contains("depth_probe"));
        search_item si2 =  file_headers.Find(h=>h.name == "probe_depth_override");
        
        if (si2 != null) {
           return Convert.ToSingle(si2.value);
        }
        
        if (si1 != null) {
            return (float) si1.value_dbl();
        }
        
        return NullReturn;
        
        }

        public float getOverride(string s1, float NullReturn = 0) {

            search_item si = file_headers.Find(e=>e.name.Contains(s1));
           
            if (si != null) {
              return Convert.ToSingle(si.value);
            }

            return NullReturn;


        }
        public string getBoreHoleId() {

        search_item si =  file_headers.Find(h=>h.name == "log_name" || h.name =="BoreholeRef");
        search_item si2 =  file_headers.Find(h=>h.name == "bhole_ref_override");
        
        if (si2 != null) {
            return si2.value;
        }
        
        if (si != null) {
            return si.value_string();
        }

        return "";

        }
         public float getAtmosphericOverride(float NullReturn = 0) {

        search_item si =  file_headers.Find(h=>h.name == "atmos_override");
        
        if (si != null) {
           return Convert.ToSingle(si.value);
        }
        
        return NullReturn;

        }
         public int getBaroBuffer(int NullReturn = 0) {

        search_item si =  file_headers.Find(h=>h.name == "baro_buffer");
        
        if (si != null) {
          return Convert.ToInt16(si.value);
        }
        
        return NullReturn;

        }
        public string getRoundRef() {
        string ret = "";

        search_item round_ref =  file_headers.Find(h=>h.name == "round_ref");
        
        if ( round_ref != null) {
            ret = round_ref.value;
        }
        
        return ret;
        }
        public int getChannelColId(string ChannelRefs) {
            
            if (file_array==null) {
                return -1;
            }

            var ai2= file_array.Find(a=>a.name==ChannelRefs);

            if (ai2==null) {
                return -1;
            }

           int colChannel = -1;
        
            foreach (value_header vh in field_headers)  {
                for (int i = 0; i < ai2.values.Length; i++) {
                    if (ai2.values[i]!=null) {
                        if (vh.name == ai2.values[i]) {
                            colChannel = i;
                            break;
                        }
                    }        
                }
            }
            
            return colChannel;

        }
        public float getArrayValue(string name, int offset, float retNullValue = -1) {
            array_item ai1 = file_array.Find(a=>a.name == name);
            if (ai1!=null) {
                try {
                return Convert.ToSingle(ai1.values[offset]);
                } catch {
                    return retNullValue;
                }
            }
            return 0;
        }
        public ge_log_reading get_reading(DateTime from, DateTime to) {
            ge_log_reading reading = readings.Find(r=>r.ReadingDatetime > from && r.ReadingDatetime< to);
            
            return reading;
        }
        public ge_log_reading get_closest_reading(DateTime from, DateTime now, DateTime to) {
            
            ge_log_reading early = get_closest_reading(from, now).LastOrDefault();
            ge_log_reading later = get_closest_reading(now,to).FirstOrDefault();
            
            if (early==null && later==null) {
                return null;
            }

            if (early==null && later!=null) {
                return later;
            }

            if (early!=null && later==null) {
                return early;
            }

            TimeSpan earlyTS = now-early.ReadingDatetime; 
            TimeSpan laterTS = later.ReadingDatetime-now; 
            
            if (earlyTS<=laterTS) {
                return early;
            }

            return later;
        }
        public IEnumerable<ge_log_reading> get_closest_reading(DateTime from, DateTime to) {
            return readings.Where(r=>r.ReadingDatetime >= from && r.ReadingDatetime <= to)
                                                        .OrderBy(r=>r.ReadingDatetime);
        }
        public string getArrayString(string name, int offset) {
            array_item ai1 = file_array.Find(a=>a.name == name);
            if (ai1!=null) {
                return ai1.values[offset];
            }
            return "";
        }

        public string getDeviceName() {
        string ret = "";

        search_item device =  file_headers.Find(h=>h.name == "device" || h.name == "InstrumentType");
        search_item serial =  file_headers.Find(h=>h.name == "serial_number" || h.name == "SerialNumber");
        
        if (device != null) {
            ret = device.value_string().Trim() + " ";
        }
        
        if (serial != null) {
            ret = ret + serial.value_string().Trim() + " ";
        }
        
        if (ret=="") {
            int colChannel = getChannelColId ("CalibrationFactors");
            string ch_device =  getArrayString("Model",colChannel);
            string ch_serial =  getArrayString("Serial",colChannel);
            ret = ch_device + " " + ch_serial;
        }

        return ret;
        }
        public void tempFixHeaders() {

        foreach (value_header vh in field_headers) {
            if (vh.name == "Barometric Pressure" && vh.units==ge_log_constants.MMHG)  {
                vh.name ="Barometric Head";
                vh.id = ge_log_constants.BAROHEAD;
            }
            
            if (vh.id==ge_log_constants.WDEPTH) {
                vh.name = "Head";
                vh.id= ge_log_constants.WHEAD;
            }
        }
        
        }
        public void AddReplaceSearchItem(search_item newItem) {

            int loc = file_headers.FindIndex(i=>i.name==newItem.name) ;

            if (loc == -1) {
                file_headers.Add (newItem);
            } else {
                file_headers[loc] = newItem;
            }


        }
        
    }

	public class aggregate_reading {
        public DateTime maxReadingDate {get;set;}
        public DateTime minReadingDate {get;set;} 
        public int count {get;set;}

        public ValueRange Value1{get;set;}
        public ValueRange Value2{get;set;}
        public ValueRange Value3{get;set;}
        public ValueRange Value4{get;set;}
        public ValueRange Value5{get;set;}
        public ValueRange Value6{get;set;}

    }  
    
	public class ValueRange {
        public float max {get;set;}
        public float min {get;set;} 
    }
    public abstract class _log {
        public static int NOT_FOUND = -1;
        public static int NOT_OK = -1;
        public static int OK = 0;
        public ge_log_file log_file {get;set;}

        public int AddOverrides(float? PROBE_DEPTH_OVERRIDE_M,
                          string BHOLE_REF_OVERRIDE) {

            if (PROBE_DEPTH_OVERRIDE_M!=null) {
            search_item depth_probe = new search_item {
                                name = "probe_depth_override",
                                value = PROBE_DEPTH_OVERRIDE_M.Value.ToString(),
                                units ="m",
                                comments = "Manual depth_probe override (m)",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                log_file.AddReplaceSearchItem (depth_probe);
            }
            
            if (BHOLE_REF_OVERRIDE !=null) {
            search_item bh_ref = new search_item {
                                name = "bhole_ref_override",
                                value = BHOLE_REF_OVERRIDE.ToString(),
                                units ="m",
                                comments = "Manual borehole reference assignment",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                log_file.AddReplaceSearchItem (bh_ref);
            }

            return 0;

         }
    }
    public abstract class _log_calculate: _log {

        
        public List<ge_log_file> baro_files {get;set;}

        public static int BARO_BUFFER_MINS = 15;

     
        public static string COMPENSATE_HEAD_DIFF = "compensate_head_diff";
        public static string COMPENSATE_DEPTH_DIFF = "compensate_depth_diff";
        public static string COMPENSATE_HEAD = "compensate_head";
        public static string COMPENSATE_DEPTH = "compensate_depth";
        public static string LINEAR_CONVERSION = "linear";
        public static string POLY_CONVERSION = "poly";

        protected _log_calculate () {
          baro_files =  new List<ge_log_file> {};  
        }
        public int AddOverrides(int? BARO_BUFFER_MINS,
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
                          ){
            
               int ret = base.AddOverrides (PROBE_DEPTH_OVERRIDE_M,
                                            BHOLE_REF_OVERRIDE);
                
                if (baro_files !=null) {
                string baroNames = "";
                string baroIds = "";

                foreach (ge_log_file barofile in baro_files) {
                    if (baroNames.Length>0) {
                        baroNames +=",";
                        baroIds +=",";
                    }
                    baroNames += barofile.data.filename;
                    baroIds +=barofile.Id;
               }

                search_item baro = new search_item {
                                name = "baroId",
                                value = baroIds,
                                units = "",
                                comments = "Baro fileId(s) used for compensating diver readings",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                log_file.AddReplaceSearchItem (baro);
                            
                search_item barof = new search_item {
                                    name = "baro_file",
                                    value = baroNames,
                                    units = "",
                                    comments = "Baro file(s) used for compensating diver readings",
                                    source = ge_log_constants.SOURCE_ASSIGNED
                                    };
                log_file.AddReplaceSearchItem (barof);
                
            }

            if (BARO_BUFFER_MINS!=null) {
                search_item baro_buff = new search_item {
                                name = "baro_buffer",
                                value = BARO_BUFFER_MINS.Value.ToString(),
                                units = "mins",
                                comments = "Manual barrow buffer time for finding matching records (mins)",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
               log_file.AddReplaceSearchItem (baro_buff);
            }
            
            if (OFFSET_OVERRIDE_M!=null) {
                search_item offset = new search_item {
                                name = "offset_override",
                                value = OFFSET_OVERRIDE_M.Value.ToString(),
                                units ="m",
                                comments = "Manual offset override (m)",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                log_file.AddReplaceSearchItem (offset);
            }

            if (ATMOS_HEAD_M!=null) {
            search_item atmos = new search_item {
                                name = "atmos_override",
                                value = ATMOS_HEAD_M.Value.ToString(),
                                units = "m",
                                comments = "Manual atmospheric pressure (m)",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                log_file.AddReplaceSearchItem (atmos);
            }

           
             
            if (DRY_DEPTH_M !=null) {
            search_item dry_depth = new search_item {
                                name = "dry_depth",
                                value = DRY_DEPTH_M.Value.ToString(),
                                units ="m",
                                comments = "Manual min dry depth assignment",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                log_file.AddReplaceSearchItem (dry_depth);
            }

            if (ZERO_READING !=null) {
            search_item zero_reading = new search_item {
                                name = "zero_reading",
                                value = ZERO_READING.Value.ToString(),
                                units ="",
                                comments = "Zero Reading",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                log_file.AddReplaceSearchItem (zero_reading);
            }

            if (ZERO_TEMP != null) {
            search_item zero_temp = new search_item {
                                name = "zero_temp",
                                value = ZERO_TEMP.Value.ToString(),
                                units ="",
                                comments = "Zero Temperature",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                log_file.AddReplaceSearchItem (zero_temp);
            }

            if (LINEAR_FACTOR != null) {
            search_item linear_factor = new search_item {
                                name = "linear_factor",
                                value = LINEAR_FACTOR.Value.ToString(),
                                units ="",
                                comments = "Linear Factor",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                log_file.AddReplaceSearchItem (linear_factor);
            }
            
            if (BARO_PRESSURE_M != null) {
            search_item baro_pressure = new search_item {
                                name = "baro_pressure",
                                value = BARO_PRESSURE_M.Value.ToString(),
                                units ="m",
                                comments = "Baro Pressure",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                log_file.AddReplaceSearchItem (baro_pressure);
            }

            if (TEMP_AT_CAL != null) {
            search_item temp_at_cal = new search_item {
                                name = "temp_at_cal",
                                value = TEMP_AT_CAL.Value.ToString(),
                                units ="m",
                                comments = "Temperature at Calibration",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                log_file.AddReplaceSearchItem (temp_at_cal);
            }
            
            if (CONST_A != null) {
            search_item const_a = new search_item {
                                name = "const_a",
                                value = CONST_A.Value.ToString(),
                                units ="",
                                comments = "Constant A",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                log_file.AddReplaceSearchItem (const_a);
            }

            if (CONST_B != null) {
            search_item const_b = new search_item {
                                name = "const_b",
                                value = CONST_B.Value.ToString(),
                                units ="",
                                comments = "Constant B",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                log_file.AddReplaceSearchItem (const_b);
            }
            
            if (CONST_C != null) {
            search_item const_c = new search_item {
                                name = "const_c",
                                value = CONST_C.Value.ToString(),
                                units ="",
                                comments = "Constant C",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                log_file.AddReplaceSearchItem (const_c);
            }
            
            if (CONST_T != null) {
            search_item const_t = new search_item {
                                name = "const_t",
                                value = CONST_T.Value.ToString(),
                                units ="",
                                comments = "Constant T",
                                source = ge_log_constants.SOURCE_ASSIGNED
                                };
                log_file.AddReplaceSearchItem (const_t);
            }
          
            return 0;

    }
protected int AddBaroHead() {

    // cycle through each barofile and set baro pressure records where they exist within buffer offset,
    // only set the values in the log_baro_headM column that have valid readingtime offsets 
    foreach (ge_log_file baro_file in baro_files) {
        
        int res = AddBaroHead(baro_file);
        
        if (res == NOT_OK) {
            return NOT_OK;
        }
    }
   
    // There may still be some readingtimes that don't have baro pressuer, this will be null, 
    // to avoid an issue when calculating the column fill this with zero.
    
  //  value_header log_baro_headM = log_file.getHeaderByIdUnits(ge_log_constants.BAROHEAD,"m");
  //  setEmptyBaroReadings (log_baro_headM.db_name,0);

    return OK;
 
 
}

protected int AddBaroHead(ge_log_file baro_file) {

            if (baro_file == null) {
            return -1;
            }
            
            int baro_file_success = NOT_OK;
            
            value_header baro_headM = null;

            value_header baro_head = baro_file.getSourceBarometricPressureHeader();
            
            if (baro_head != null) {
                if (baro_head.units=="m") {
                    baro_headM = baro_head;
                    baro_file_success = 0;
                }
                if (baro_head.units == "mmHg") {
                        baro_headM = new value_header {
                                id = ge_log_constants.BAROHEAD,
                                units ="m",
                                comments =$"Calculated conversion of {baro_head.db_name} from mmHg to m ({ge_log_constants.FACTOR_mmHg_to_mH20})",
                                source = ge_log_constants.SOURCE_CALCULATED};
                        baro_file.addHeader(baro_headM);
                        baro_file.addValues(baro_head.db_name,ge_log_constants.FACTOR_mmHg_to_mH20, baro_headM.db_name);
                        baro_file_success = 0;
                } else if (baro_head.units == "kPa") {
                        baro_headM = new value_header {
                                id = ge_log_constants.BAROHEAD,
                                units ="m",
                                comments =$"Calculated conversion of {baro_head.db_name} from kPa to m ({ge_log_constants.FACTOR_kPa_to_mH20})",
                                source = ge_log_constants.SOURCE_CALCULATED};
                        baro_file.addHeader(baro_headM);
                        baro_file.addValues(baro_head.db_name,ge_log_constants.FACTOR_kPa_to_mH20, baro_headM.db_name);
                        baro_file_success = 0;
                } else if (baro_head.units == "cm") {
                        baro_headM = new value_header {
                                id = ge_log_constants.BAROHEAD,
                                units ="m",
                                comments =$"Calculated conversion of {baro_head.db_name} from cm to m ({ge_log_constants.FACTOR_cmH20_to_mH20})",
                                source = ge_log_constants.SOURCE_CALCULATED};
                        baro_file.addHeader(baro_headM);
                        baro_file.addValues(baro_head.db_name,ge_log_constants.FACTOR_cmH20_to_mH20, baro_headM.db_name);
                        baro_file_success = 0;
                } else if (baro_head.units == "mbar") {
                        baro_headM = new value_header {
                                id = ge_log_constants.BAROHEAD,
                                units ="m",
                                comments =$"Calculated conversion of {baro_head.db_name} from mbar to m ({ge_log_constants.FACTOR_mbar_to_mH20})",
                                source = ge_log_constants.SOURCE_CALCULATED};
                        baro_file.addHeader(baro_headM);
                        baro_file.addValues(baro_head.db_name,ge_log_constants.FACTOR_mbar_to_mH20, baro_headM.db_name);
                        baro_file_success = 0;
                }else {
                // Uknown units...
                baro_file_success = NOT_OK;
                } 
            }
 
            if (baro_headM!=null) {
                    value_header log_baro_headM = log_file.getHeaderByIdUnits(ge_log_constants.BAROHEAD,"m");
                    int baro_buffer = log_file.getBaroBuffer(BARO_BUFFER_MINS);
                    if (log_baro_headM == null) {
                        //Add barometric head from baro file
                        log_baro_headM = new value_header {
                        id = ge_log_constants.BAROHEAD,
                        units ="m",
                        comments =$"Looked up barometric head (m) +/-{baro_buffer} mins)",
                        source = ge_log_constants.SOURCE_ACTUAL};
                        log_file.addHeader(log_baro_headM);
                    } else{
                       log_baro_headM.comments = $"Looked up barometric head (m) +/-{baro_buffer} mins)"; 
                    }
                    addBaroReadings(baro_file, baro_headM.db_name, baro_buffer, log_baro_headM.db_name);
                    baro_file_success = 0;
            }

            return baro_file_success; 
    }

        private void addBaroReadings(ge_log_file baro_file, string baro_value, int minsBuffer, string value) {
                
                TimeSpan time = new TimeSpan(0, 0, minsBuffer, 0);
                
                for(int i=0;i<log_file.readings.Count;i++) {
                    
                    ge_log_reading r = log_file.readings[i];
                    DateTime d1 = r.ReadingDatetime - time;
                    DateTime d2 = r.ReadingDatetime + time;

                    ge_log_reading r_baro = baro_file.get_closest_reading(d1,r.ReadingDatetime, d2);
                   

                    if (r_baro != null) {
                        float? baro_val = r_baro.getValue(baro_value);
                        if (baro_val !=null) {
                            r.setValue(value,baro_val.Value);
                        } 
                    }
                }
        
        }

        private void setEmptyBaroReadings(string baro_value, float empty_value) {
                for(int i=0;i<log_file.readings.Count;i++) {
                    ge_log_reading r = log_file.readings[i];
                    float? baro_val = r.getValue(baro_value);
                    if (baro_val == null) {
                        r.setValue(baro_value, empty_value); 
                    }
                }
        }
  
         protected int addWDepthM() {
            
            // Calculate Water Depth             
          
            float offset = log_file.getOffset(0);
            float probe_depth = log_file.getProbeDepth(0);
            
            if (offset == 0 && probe_depth == 0) {
                return -1;
            }
            
            value_header log_headM = log_file.getHeaderById(ge_log_constants.NETHEAD);
            
            if (log_headM==null) {
                log_headM = log_file.getHeaderById(ge_log_constants.WHEAD);
            }
            
            value_header log_wdepthM = log_file.getHeaderById(ge_log_constants.WDEPTH);
           
            if (log_wdepthM !=null) {
                log_file.removeHeader (log_wdepthM);
            }

            if (offset != 0) {
                log_wdepthM = new value_header {
                    id = ge_log_constants.WDEPTH,
                    units ="m",
                    comments = $"Water depth below datum (-{log_headM.db_name}+{offset})",
                    source = ge_log_constants.SOURCE_CALCULATED};
                log_file.addHeader (log_wdepthM);
                log_wdepthM = log_file.getHeaderById(ge_log_constants.WDEPTH);
                if (log_wdepthM!=null) {
                log_file.addInverted(offset,log_headM.db_name,log_wdepthM.db_name);
                }
            } else {
                log_wdepthM = new value_header {
                    id = ge_log_constants.WDEPTH,
                    units ="m",
                    comments =$"Water depth below datum ({probe_depth}-{log_headM.db_name})",
                    source = ge_log_constants.SOURCE_CALCULATED};
                log_file.addHeader (log_wdepthM);  
                log_wdepthM = log_file.getHeaderById(ge_log_constants.WDEPTH);
                if (log_wdepthM!=null) {
                log_file.subtractConstant(probe_depth, log_headM.db_name, log_wdepthM.db_name);
                }

            } 
                        
            float dry_depth = log_file.getDryDepth(NOT_FOUND);
            
            if (dry_depth != NOT_FOUND) {
                log_file.setNotDry(log_wdepthM.db_name, dry_depth);
            }

            return 0;
    
        }
        protected int AddHeadNetM_SubtractDifferential() {
        
        value_header log_baroheadM  = log_file.getHeaderByIdUnits(ge_log_constants.BAROHEAD,"m");  
        value_header log_TheadM = log_file.getHeaderByIdUnits(ge_log_constants.THEAD,"m");
        value_header log_wheadM = log_file. getHeaderByIdUnits(ge_log_constants.WHEAD,"m");
        
        if (log_wheadM==null || log_baroheadM==null) {
            return 0;
        }
        
        value_header log_netheadM = log_file.getHeaderByIdUnits(ge_log_constants.NETHEAD,"m");     
        
        if (log_netheadM==null) {
            log_netheadM = new value_header {
                            id = ge_log_constants.NETHEAD,
                            units ="m",
                            source = ge_log_constants.SOURCE_CALCULATED};
            log_file.addHeader (log_netheadM);
        }
        
        // Order list so that differential will work 
        List<ge_log_reading> ordered  = log_file.readings.OrderBy(e=>e.ReadingDatetime).ToList();

        log_file.readings = ordered;
        log_file.subtractDifferential(log_wheadM.db_name, log_baroheadM.db_name, log_netheadM.db_name);
        log_netheadM.comments = $"Water head with barometric differential removed {log_wheadM.db_name}-({log_baroheadM.db_name}j-{log_baroheadM.db_name}i)";
         
         if (log_TheadM != null) {
                log_netheadM.comments += $" and thermal head {log_TheadM.db_name} added ";
            log_file.addToExisting(log_TheadM.db_name,  1.0F, log_netheadM.db_name);
         }
        
        return 0;

        }
        
        protected int AddHeadNetM() {
                     
            value_header log_baroheadM = log_file.getHeaderByIdUnits(ge_log_constants.BAROHEAD,"m");
            value_header log_TheadM = log_file.getHeaderByIdUnits(ge_log_constants.THEAD,"m");
            value_header log_headM = log_file. getHeaderByIdUnits(ge_log_constants.WHEAD,"m");
            float atmos_head_m = log_file.getAtmosphericOverride(0);

            string comments = "";
            
            if (log_headM==null) {
                return -1;
            }

            if ((log_baroheadM==null && atmos_head_m==0 && log_TheadM==null) || log_headM==null) {
                return -1;
            }


            if (log_baroheadM!=null) {
                comments = $"Water head with barometric head removed (m)";
            } else {
                comments = $"Water head with const barometric head {atmos_head_m} removed (m)";
            }

            value_header log_netheadM = log_file.getHeaderByIdUnits(ge_log_constants.NETHEAD,"m");
         
            if (log_netheadM == null) {
            log_netheadM = new value_header {
                            id = ge_log_constants.NETHEAD,
                            units ="m",
                            comments = comments,
                            source = ge_log_constants.SOURCE_CALCULATED};
                    log_file.addHeader (log_netheadM);
            } else {
            log_netheadM.comments = comments;
            }

           if (log_baroheadM != null) {
              log_file.addValues(log_headM.db_name, log_baroheadM.db_name, -1, log_netheadM.db_name);
           } else {
              log_file.addConstant(log_headM.db_name,-atmos_head_m, log_netheadM.db_name);
           }

           if (log_TheadM != null) {
                log_netheadM.comments += $" and thermal head {log_TheadM.db_name} added (m)";
            log_file.addToExisting(log_TheadM.db_name,  1, log_netheadM.db_name);
           }
           
           return 0;
        
    }
        public virtual int Calculate (int? BARO_BUFFER_MINS,
                          float? ATMOS_HEAD_M,
                          float? OFFSET_OVERRIDE_M,
                          float? PROBE_DEPTH_OVERRIDE_M,
                          string BHOLE_REF_OVERRIDE,
                          float? DRY_DEPTH_M
                                  ) {
            return -1;
        }
}
}

