using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.OtherDatabase;
using ge_repository.interfaces;

namespace ge_repository.services
{
  public class MONDService : gINTBaseService
    {
       
        public MONDService(IGintUnitOfWork unitOfWork) : base (unitOfWork)
        { }

        public async Task<MONG> GetParentById(int Id) {
           return await _unitOfWork.MONG.GetByIdAsync(Id);
        }
        public Task<List<MONG>> GetParentsByHoleId(string Id) {
            return _unitOfWork.MONG.FindAsync ($"PointID='{Id}'");
        }
        public Task <MONG> getMONG(string PointId, string MONG_DIS) {
            return null;
        }
     
        public Task<List<MONG>> GetParentsWhere(string where) {
            return _unitOfWork.MONG.FindAsync (where);
        }
        public async Task<List<MOND>> GetAllRecords() {
            return await _unitOfWork.MOND.GetAllAsync();
        }

        public async Task<MOND> GetRecordById(int Id) {
            return await _unitOfWork.MOND.GetByIdAsync(Id);
        }
        public async Task<List<MOND>> GetAllWhere(string where) {
            return await _unitOfWork.MOND.FindAsync (where);
        }

        public async Task CreateRecord(MOND newRecord) {
            await _unitOfWork.MOND.AddAsync(newRecord);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateRecord(MOND recordToBeUpdated, MOND record) {
            var rec = await _unitOfWork.MOND.GetByIdAsync(recordToBeUpdated.GintRecID);
            
            await _unitOfWork.CommitAsync();
        }
        public async Task CreateRange(List<MOND> records) {

            
        }
        public async Task UpdateRange(List<MOND> records, string where) {
            await _unitOfWork.MOND.UpdateRangeAsync(records, where);
            await _unitOfWork.CommitBulkAsync();
        }
        public async Task DeleteRecord(MOND record) {
                _unitOfWork.MOND.Remove(record);
                await _unitOfWork.CommitAsync();
           
        }
    }

    public class MONDLogService : MONDService, IMONDLogService
    {
       
        public MONDLogService(IGintUnitOfWork unitOfWork) : base (unitOfWork)
        { }

        public async Task<List<MOND>> CreateMOND(ge_log_file log_file, 
                                        string table,
                                        string round_ref,
                                        DateTime? fromDT,
                                        DateTime? toDT,
                                        Boolean save_MOND
                                        ) 
        {
            
            string ge_source = "";

            if (table.Contains("waterquality") || 
                table.Contains("wq") ) {
                ge_source = "ge_flow";
            }

            if (table.Contains("depth") || 
                table.Contains("head") || 
                table.Contains("pressure") || 
                table.Contains("channel") || 
                table.Contains("r0") ||
                table.Contains("r1")
                ) {
                ge_source = "ge_logger";
            }
            
            int page_size = 1000;
            int row_count = log_file.getIncludeReadings(fromDT, toDT).Count() ;
            int total_pages = Convert.ToInt32(row_count / page_size) + 1;
            
            List<MOND> ordered =  new List<MOND>();
            
            for (int page = 1; page <= total_pages; page++) {

                    
                List<MOND> batch = await createMOND( log_file,
                                                    page_size,
                                                    page,
                                                    round_ref,
                                                    fromDT,
                                                    toDT,
                                                    ge_source,
                                                    true);
                
               if (save_MOND == true) { 

                    string where2 = $"ge_source='{ge_source}'"; 
                    await UpdateRange(batch, where2);
                            
                }
                
                ordered.AddRange(batch.OrderBy(e=>e.DateTime).ToList());

            }

        return ordered;

    }
    private int getInt32(string s1) {
        
        char[] allowed_chars = new char[] {'0','1','2','3','4','5','6','7','8','9'}; 
        
        string s2 ="";

        foreach(char c in s1) {
            if (allowed_chars.Contains(c)) {
                s2 += c;
            }
        }

        int ret_val = 0;
        Int32.TryParse(s2, out ret_val);
        
        return ret_val;
    }
    private async Task<List<MOND>> createMOND ( ge_log_file log_file,
                                                int page_size,
                                                int page,
                                                string round_ref,
                                                DateTime? fromDT,
                                                DateTime? toDT,
                                                string ge_source="ge_flow",
                                                Boolean addWLEV = true) {


        // Find borehole in point table of gint database
        
        string holeId = log_file.getBoreHoleId();

        if (holeId=="") {
            return null; // BadRequest ($"Borehole ref not provided");
        }

        POINT pt = await GetPointByHoleId(holeId);

        if (pt == null) {
            return null;//         return BadRequest ($"Borehole ref {holeId} not found in {project.name}");
        }                
    
        List<MONG> mgs = await GetParentsByHoleId(holeId);

        
         // Find monitoring point in mong table of gint database
        float probe_depth = log_file.getProbeDepth();
        
        if (probe_depth==0) {
             return null; // return BadRequest ($"No probe depth provided for borehole ref {holeId} not found in {project.name}"); 
        }


        MONG mg = null;

        string formatMATCH ="{0:00.0}";

       if (mgs.Count==1) {
           mg = mgs.FirstOrDefault();
       } else {
            foreach (MONG m in mgs) {
                if (m.MONG_DIS!=null) {
                    if (String.Format(formatMATCH, m.MONG_DIS.Value) == String.Format(formatMATCH,probe_depth)) {
                        mg = m;
                        break;
                    }
                }
            }
       }

        if (mg==null) {
            return null; // return BadRequest ($"No installations in borehole ref {holeId} have a probe depth of {probe_depth} in {project.name}"); 
        }
        
        // Add all readings to new items in List<MOND> 
        List<MOND> MOND = new List<MOND>();
               
        string device_name = log_file.getDeviceName();
        
        float? gl = null;
        
        if (pt.Elevation!=null) {
            gl = Convert.ToSingle(pt.Elevation.Value);
        }

        if (gl==null && pt.LOCA_GL!=null) {
            gl = Convert.ToSingle(pt.LOCA_GL.Value);
        }

        // int round_no = getInt32(round_ref);
        
        string mond_rem_suffix = "";
        string mond_ref = "";

        if (ge_source =="ge_flow") {
            mond_rem_suffix = " flow meter reading";
        }

        if (ge_source =="ge_logger") {
            mond_rem_suffix = " datalogger reading";
        }

        List<ge_log_reading> readings2 = log_file.getIncludeReadingsPage(fromDT, toDT, page_size, page);
                                          
            foreach (ge_log_reading reading in readings2) {
                
                foreach (value_header vh in log_file.field_headers) {
                    
                    if (ge_source =="ge_flow")  {
                        mond_ref = String.Format("Round {0} Seconds {1:00}",round_ref,reading.Duration);
                    }

                    if (vh.id == "WDEPTH" && vh.units=="m") {
                        // Add MOND WDEP record
                       
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "WDEP", mg.MONG_TYPE + mond_rem_suffix, mond_ref, vh.db_name, "Water Depth", vh.units,vh.format, null, ge_source);
                        if (md!=null) MOND.Add (md);
                
                        if (gl!=null && addWLEV==true) {           
                        // Add MOND WLEV record
                        MOND md2 = NewMOND (mg, reading, device_name, round_ref, "WLEV", mg.MONG_TYPE + mond_rem_suffix, mond_ref, vh.db_name,"Water Level", vh.units, vh.format, gl, ge_source);
                        if (md2!=null) MOND.Add (md2);
                        }
                    }
                    
                    if (vh.id == "PH" ) {
                        // Add MOND Potential Hydrogen
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "PH", mg.MONG_TYPE + mond_rem_suffix, mond_ref, vh.db_name, "", vh.units, vh.format, null, ge_source);
                        if (md!=null) MOND.Add (md);
                    }
                    
                    if (vh.id == "DO" && vh.units == "mg/l") {
                        // Add MOND Disolved Oxygen
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "DO", mg.MONG_TYPE + mond_rem_suffix, mond_ref, vh.db_name, "Dissolved Oxygen", vh.units, vh.format, null, ge_source);
                        if (md!=null)  MOND.Add (md);
                    }
                    
                    if ((vh.id == "AEC" && vh.units == "μS/cm") || 
                        (vh.id == "AEC" && vh.units == "mS/cm")) {
                        // Add MOND Electrical Conductivity 
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "AEC", mg.MONG_TYPE + mond_rem_suffix, mond_ref, vh.db_name, "Actual Electrical Conductivity", vh.units, vh.format, null, ge_source);
                        if (md!=null) MOND.Add (md);
                    }
                    
                    if ((vh.id == "EC" && vh.units == "μS/cm") || 
                        (vh.id == "EC" && vh.units == "mS/cm")) {
                        // Add MOND Electrical Conductivity 
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "EC", mg.MONG_TYPE + mond_rem_suffix, mond_ref, vh.db_name, "Electrical Conductivity", vh.units, vh.format, null, ge_source);
                        if (md!=null) MOND.Add (md);
                    }
                    
                    if (vh.id == "SAL" && vh.units == "g/cm3") {
                        // Add MOND Salinity record 
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "SAL", mg.MONG_TYPE + mond_rem_suffix,mond_ref,  vh.db_name, "Salinity", vh.units, vh.format, null, ge_source);
                        if (md!=null) MOND.Add (md);
                    }
                    
                    if (vh.id == "TEMP" && vh.units == "Deg C") {
                        // Add MOND Temp record 
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "DOWNTEMP", mg.MONG_TYPE + mond_rem_suffix, mond_ref, vh.db_name, "Downhole Temperature", vh.units, vh.format, null, ge_source);
                        MOND.Add (md);
                    }
                    
                    if (vh.id == "RDX" && vh.units == "mV") {
                        // Add MOND Redox Salinity record 
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "RDX", mg.MONG_TYPE + mond_rem_suffix, mond_ref, vh.db_name, "Redox Potential", vh.units, vh.format, null, ge_source);
                        if (md!=null) MOND.Add (md);
                    }

                    if (vh.id == "TURB" && vh.units == "NTU") {
                        // Add MOND Salinity record 
                        MOND md = NewMOND (mg, reading, device_name, round_ref, "TURB", mg.MONG_TYPE + mond_rem_suffix,mond_ref,  vh.db_name, "Turbity", vh.units, vh.format, null, ge_source);
                        if (md!=null) MOND.Add (md);
                    }

                }
            }

        return MOND;
    }
    private MOND NewMOND (MONG mg, ge_log_reading read,
                        string instrument_name, 
                        string round_ref, 
                        string mond_type, 
                        string mond_rem,
                        string mond_ref, 
                        string value_name, 
                        string mond_name, 
                        string units,
                        string format, 
                        float? GL, 
                        string ge_source) {
        
        string value = null; 
        string format2 = "{0:" + format + "}";

        if (read.NotDry==ge_log_constants.ISNOTDRY) {
            float? reading = read.getValue(value_name);
            if (reading!=null) {
                value = String.Format(format2,reading);
                if (mond_type=="WLEV") value = String.Format(format2,GL.Value - reading);
            }
        }

        if (read.NotDry==ge_log_constants.ISDRY) {
            value = "Dry";
        }

        if (!String.IsNullOrEmpty(read.Remark)) {
            mond_rem += " " + read.Remark;
        }
        
        if (String.IsNullOrEmpty(value)) {
            return null;
        }

        MOND md =  new MOND {
                    gINTProjectID = mg.gINTProjectID,
                    PointID = mg.PointID,
                    ItemKey = mg.ItemKey,
                    MONG_DIS = mg.MONG_DIS,
                    MOND_TYPE = mond_type,
                    MOND_REF = mond_ref,
                    DateTime = read.ReadingDateTime,
                    MOND_UNIT = units,
                    MOND_RDNG = value,
                    MOND_INST = instrument_name,
                    MOND_NAME = mond_name,
                    MOND_REM = mond_rem,
                    RND_REF = round_ref,
                    ge_source = ge_source,
                    ge_otherId = read.Id.ToString()                    
        };

        return md;

}

    }
}
