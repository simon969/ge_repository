using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using ge_repository.interfaces;
using ge_repository.OtherDatabase;

namespace ge_repository.services
{
    public class LoggerFileService : ILoggerFileService
    {
        private static int NOT_FOUND = -1;
        private readonly ILoggerFileUnitOfWork _unitOfWork;
        public LoggerFileService(ILoggerFileUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ge_log_file> GetById(Guid Id) {
             return await _unitOfWork.LoggerFile.GetByIdAsync(Id);
        }
        public async Task<ge_log_file> GetByDataId(Guid Id, string table) {
             return await _unitOfWork.LoggerFile.GetByDataIdAsync(Id, table);
        }
        public async Task<ge_log_file> GetByIdNoReadings(Guid Id) {
            return await _unitOfWork.LoggerFile.GetByIdWithoutReadingsAsync(Id);
        }

        public async Task<IEnumerable<ge_log_file>> GetAllByProjectIdNoReadings(Guid Id) {
            return await _unitOfWork.LoggerFile.GetAllLoggerFilesWithoutReadingsAsync();
        }

        public async Task  CreateLogFile(ge_log_file newData) {
             await _unitOfWork.LoggerFile.AddAsync (newData);
             await _unitOfWork.CommitAsync();
             
        }

        public async Task UpdateLogFile(ge_log_file dataToBeUpdated, ge_log_file data) {
            var rec = await _unitOfWork.LoggerFile.GetByIdAsync(dataToBeUpdated.Id);
            //int resp = 
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteLogFile(ge_log_file dataToBeDeleted) {
            _unitOfWork.LoggerFile.Remove(dataToBeDeleted);
            await _unitOfWork.CommitAsync();
        }

        public ge_log_file CreateLogFile(ge_search dic, 
                                  string[] lines,
                                  Guid dataId,
                                  Guid templateId) {
    
            ge_log_file file = new ge_log_file();
            
            file.dataId = dataId;
            file.templateId = templateId;
            
            file.search_template = dic;
            
            file.file_headers = dic.search_items;
            file.file_array = dic.array_items;
            
            search_table st = dic.search_tables.FirstOrDefault();
            file.search_table = st; 
            file.field_headers = st.headers;
            file.channel = st.name;
            
            value_header DateTimeReading = dic.getHeader(ge_log_constants.READINGDATETIME);
            
            int intReadTime = NOT_FOUND;
            if (DateTimeReading != null) {
                intReadTime = DateTimeReading.found;
            }

            value_header Duration = dic.getHeader(ge_log_constants.DURATION);
            int intDuration = NOT_FOUND;
            if (Duration!=null) {
                intDuration = Duration.found;    
            }

            value_header Header1 = dic.getHeader(ge_log_constants.VALUE1);
            int intValue1 = NOT_FOUND;
            if (Header1!=null) {
                intValue1 = Header1.found;
            }
            
            value_header Header2 = dic.getHeader(ge_log_constants.VALUE2); 
            int intValue2 = NOT_FOUND;
            if (Header2!=null) {
                intValue2 = Header2.found;
            }

            value_header Header3 = dic.getHeader(ge_log_constants.VALUE3);
            int intValue3 = NOT_FOUND;
            if (Header3!=null) {
                intValue3 = Header3.found;
            }

            value_header Header4 = dic.getHeader(ge_log_constants.VALUE4);
            int intValue4 = NOT_FOUND;
            if (Header4!=null) {
                intValue4 = Header4.found;
            }

            value_header Header5 = dic.getHeader(ge_log_constants.VALUE5);
            int intValue5 = NOT_FOUND;
            if (Header5!=null) {
                intValue5 = Header5.found;
            }

            value_header Header6 = dic.getHeader(ge_log_constants.VALUE6);
            int intValue6 = NOT_FOUND;
            if (Header6!=null) {
                intValue6 = Header6.found;
            }

            value_header Header7 = dic.getHeader(ge_log_constants.VALUE7);
            int intValue7 = NOT_FOUND;
            if (Header7!=null) {
                intValue7 = Header7.found;
            }

            value_header Header8 = dic.getHeader(ge_log_constants.VALUE8);
            int intValue8 = NOT_FOUND;
            if (Header8!=null) {
                intValue8 = Header8.found;
            }

            value_header Header9 = dic.getHeader(ge_log_constants.VALUE9);
            int intValue9 = NOT_FOUND;
            if (Header9!=null) {
                intValue9 = Header9.found;
            }
            
            value_header Header10 = dic.getHeader(ge_log_constants.VALUE10);       
            int intValue10 = NOT_FOUND;
            if (Header10!=null) {
                intValue10 = Header10.found;
            }

            value_header Header11 = dic.getHeader(ge_log_constants.VALUE11);
            int intValue11= NOT_FOUND;
            if (Header11!=null) {
                intValue11 = Header11.found;
            }

            value_header Header12 = dic.getHeader(ge_log_constants.VALUE12);
            int intValue12 = NOT_FOUND;
            if (Header12!=null) {
                intValue12 = Header12.found;
            }
            
            value_header Header13 = dic.getHeader(ge_log_constants.VALUE13);
            int intValue13 = NOT_FOUND;
            if (Header13!=null) {
                intValue13 = Header13.found;
            }

            value_header Header14 = dic.getHeader(ge_log_constants.VALUE14); 
            int intValue14 = NOT_FOUND;
            if (Header14!=null) {
                intValue14 = Header14.found;
            }
            
            value_header Header15 =dic.getHeader(ge_log_constants.VALUE15);
            int intValue15 = NOT_FOUND;
            if (Header15!=null) {
                intValue15 = Header15.found;
            }

            value_header Header16 =dic.getHeader(ge_log_constants.VALUE16);
            int intValue16= NOT_FOUND;
            if (Header16!=null) {
                intValue16 = Header16.found;
            }

            value_header Header17 = dic.getHeader(ge_log_constants.VALUE17);
            int intValue17 = NOT_FOUND;
            if (Header17!=null) {
                intValue17 = Header17.found;
            }

            value_header Header18 = dic.getHeader(ge_log_constants.VALUE18);
            int intValue18 = NOT_FOUND;
            if (Header18!=null) {
                intValue18 = Header18.found;
            }
            value_header HeaderRemark = dic.getHeader(ge_log_constants.REMARK);
            int intRemark = NOT_FOUND;
            if (HeaderRemark!=null) {
                intRemark = HeaderRemark.found;
            }

            value_header log_wdepthM = file.getHeaderByIdUnits(ge_log_constants.WDEPTH,"m");
            int intCheckValueForDry = NOT_FOUND;
            if (log_wdepthM!=null) {
                intCheckValueForDry = log_wdepthM.found;   
            }

            file.readings =  new List<ge_log_reading>();
            
            int line_start = dic.data_start_row(NOT_FOUND);
            
            int line_end = dic.data_end_row(lines.Count());

            
            int readlines = addReadingsAny(file.readings, 
                                        lines, 
                                        line_start, 
                                        line_end,
                                        intReadTime,
                                        intDuration, 
                                        intValue1, 
                                        intValue2, 
                                        intValue3,
                                        intValue4,
                                        intValue5,
                                        intValue6,
                                        intValue7, 
                                        intValue8, 
                                        intValue9,
                                        intValue10,
                                        intValue11,
                                        intValue12,
                                        intValue13, 
                                        intValue14, 
                                        intValue15,
                                        intValue16,
                                        intValue17,
                                        intValue18, 
                                        intRemark,
                                        intCheckValueForDry,
                                        DateTimeReading.format                         
                                        );
        if (readlines <= 0) {
            return null;
        }

        file.init_new_file();
        file.calcReadingAggregates();
        
       // _unitOfWork.LoggerFile.AddAsync(file);

        return file;
    
    }
    private int addReadingsAny(List<ge_log_reading> list, 
                    string[] lines, 
                    int line_start,
                    int line_end, 
                    int intReadTime, 
                    int intDuration, 
                    int intValue1, 
                    int intValue2,
                    int intValue3,
                    int intValue4,
                    int intValue5,
                    int intValue6, 
                    int intValue7, 
                    int intValue8,
                    int intValue9,
                    int intValue10,
                    int intValue11,
                    int intValue12,
                    int intValue13, 
                    int intValue14,
                    int intValue15,
                    int intValue16,
                    int intValue17,
                    int intValue18,
                    int intRemark,
                    int intValueCheckForDry,
                    string dateformat = "") {

    for (int i = line_start; i<line_end; i++) {
                string line = lines[i];
                if (line.Length>0) {
                    string[] values = line.Split(",");
                    
                    if (values[0].Contains("\"")) {
                            values = QuoteSplit(line);
                    }
                    if (values[0] == "") {
                        break;
                    }
                   
                    ge_log_reading r= new ge_log_reading();
                    
                    if (intReadTime != NOT_FOUND) {
                        if (ContainsError(values[intReadTime])) {continue;}
                        r.ReadingDatetime = getDateTime(values[intReadTime],dateformat);
                    }
                    if (intDuration!= NOT_FOUND) {r.Duration = getDuration(values[intDuration], null);}
                    if (intValue1 != NOT_FOUND) {r.Value1 = getFloat(values[intValue1],null);}
                    if (intValue2 != NOT_FOUND) {r.Value2 = getFloat(values[intValue2],null);}
                    if (intValue3 != NOT_FOUND) {r.Value3 = getFloat(values[intValue3],null);}
                    if (intValue4 != NOT_FOUND) {r.Value4 = getFloat(values[intValue4],null);}
                    if (intValue5!= NOT_FOUND) { r.Value5 = getFloat(values[intValue5],null);}
                    if (intValue6 != NOT_FOUND) {r.Value6 = getFloat(values[intValue6],null);}
                    if (intValue7 != NOT_FOUND) {r.Value7 = getFloat(values[intValue7],null);}
                    if (intValue8 != NOT_FOUND) {r.Value8 = getFloat(values[intValue8],null);}
                    if (intValue9 != NOT_FOUND) {r.Value9 = getFloat(values[intValue9],null);}
                    if (intValue10 != NOT_FOUND) {r.Value10 = getFloat(values[intValue10],null);}
                    if (intValue11 != NOT_FOUND) {r.Value11 = getFloat(values[intValue11],null);}
                    if (intValue12 != NOT_FOUND) {r.Value12 = getFloat(values[intValue12],null);}
                    if (intValue13 != NOT_FOUND) {r.Value13 = getFloat(values[intValue13],null);}
                    if (intValue14 != NOT_FOUND) {r.Value14 = getFloat(values[intValue14],null);}
                    if (intValue15 != NOT_FOUND) {r.Value15 = getFloat(values[intValue15],null);}
                    if (intValue16 != NOT_FOUND) {r.Value16 = getFloat(values[intValue16],null);}
                    if (intValue17 != NOT_FOUND) {r.Value17 = getFloat(values[intValue17],null);}
                    if (intValue18 != NOT_FOUND) {r.Value18 = getFloat(values[intValue18],null);}
                    if (intRemark != NOT_FOUND) {r.Remark = values[intRemark];}
                    if (intValueCheckForDry!=NOT_FOUND) {
                        if (values[intValueCheckForDry] == "Dry" || values[intValueCheckForDry] == "DRY") {
                            r.NotDry = -1;
                        }   
                    }

                    list.Add (r);
                }
    }

    return list.Count();

 }
 private DateTime getDateTime(string s1, string dateformat = "") {

                    DateTime dt;
                    
                        if (dateformat=="") { 
                            dt = DateTime.Parse(s1);
                        } else {
                            dt = DateTime.ParseExact(s1, dateformat, CultureInfo.CurrentCulture,DateTimeStyles.AllowInnerWhite);
                        }
                    
                    return dt;


 }
 private static float? getFloat(string s1, float? retOnError) {
     float? fl;
     try {
         fl = Convert.ToSingle(s1);
         return fl;
     } catch {
         return retOnError;
     }


 }
 private static Boolean ContainsError( string s1) {

     if (s1.Contains("#VALUE")) return true;
     if (s1.Contains("#ERROR")) return true;
     if (s1.Contains("#REF")) return true;
     if (s1.Contains("#N/A")) return true;

     return false;
 
 }
 private static long? getDuration(string duration, long? retIfError) {
    Int64 dur  = 0 ;
    try {
        TimeSpan ts = TimeSpan.Parse(duration);
        dur = Convert.ToInt64(ts.TotalSeconds);
        return dur;
    } catch {
        try {
            dur = (Int64) Convert.ToSingle(duration);
            return dur;
        } catch {
            return retIfError;
        }
    }
  }
   private static string[] QuoteSplit (string s1) {
            string s2 = s1.Substring(1, s1.Length-2);
            return s2.Split("\",\"");
    }

    }
    
}
