using System;
using System.Threading.Tasks;
using ge_repository.interfaces;
using ge_repository.Models;
using ge_repository.AGS;
using ge_repository.OtherDatabase;
using static ge_repository.Authorization.Constants;
namespace ge_repository.services
{
    public class DataAGSService : DataService, IDataAGSService {
    public IAGSGroupTables  _library {get;set;}

    public DataAGSService (IUnitOfWork unitOfWork): base (unitOfWork) {}

    public async Task<IAGSGroupTables> SetLibraryAGSData(Guid Id, string[] groups=null) {

        if (groups==null) {
        groups =  new string[] {"ABBR","UNIT","TYPE","DICT"};
        }

        _library = await GetAGSData(Id,groups);
        return _library;
    }
    public async Task<int> AddLibraryAGSData(IAGSGroupTables tables, Guid? LibraryAGSDataId) {

        if (LibraryAGSDataId != null) {
            await SetLibraryAGSData(LibraryAGSDataId.Value);
        }

        if (_library == null) {
            return -1;
        }

        AGS404GroupTables ags404_lib = (AGS404GroupTables) _library;
        AGS404GroupTables _tables = (AGS404GroupTables) tables;
        // ABBR
        
        if (_tables.ABBR != null) {
            foreach (ABBR abbr in _tables.ABBR.values) {
                ABBR abbr_lib = ags404_lib.ABBR.values.Find(e=>e.ABBR_CODE == abbr.ABBR_CODE && 
                                                            e.ABBR_HDNG == abbr.ABBR_HDNG);
                if (abbr_lib != null) {
                    abbr.ABBR_DESC = abbr_lib.ABBR_DESC;
                    abbr.ABBR_REM = abbr_lib.ABBR_REM;
                }
            }
        }

        if (_tables.UNIT != null) {
            foreach (UNIT unit in _tables.UNIT.values) {
                UNIT unit_lib = ags404_lib.UNIT.values.Find(e=>e.UNIT_UNIT == unit.UNIT_UNIT);
                if (unit_lib != null) {
                    unit.UNIT_DESC = unit_lib.UNIT_DESC;
                    unit.UNIT_REM = unit_lib.UNIT_REM;
                }
            }
        }

        if (_tables.TYPE != null) {
            foreach (TYPE type in _tables.TYPE.values) {
                TYPE type_lib = ags404_lib.TYPE.values.Find(e=>e.TYPE_TYPE == type.TYPE_TYPE);
                if (type_lib != null) {
                    type.TYPE_DESC = type_lib.TYPE_DESC;
                //    type.TYPE_REM = type_lib.TYPE_REM;
                }
            }
        }

        if (_tables.DICT != null) {
            foreach (DICT dict in _tables.DICT.values) {
                DICT dict_lib = ags404_lib.DICT.values.Find(e=>e.DICT_HDNG == dict.DICT_HDNG && 
                                                            e.DICT_PGRP == dict.DICT_PGRP);
                if (dict_lib != null) {
                    dict.DICT_DESC = dict_lib.DICT_DESC;
                    dict.DICT_DTYP = dict_lib.DICT_DTYP;
                    dict.DICT_EXMP = dict_lib.DICT_EXMP;
                    dict.DICT_REM = dict_lib.DICT_REM;
                }
            }
        }

        return 1;

    }
    public async Task<IAGSGroupTables> GetAGSData(Guid Id, string[] groups) {

            string[] _lines = await GetFileAsLines(Id);

            if (_lines == null) {
                return null;
            }
            
            AGSReader reader = new AGSReader(_lines);
            IAGSGroupTables ags_tables = reader.CreateAGS404GroupTables(groups);
            
            return ags_tables;
    }

    public async Task<string> NewAGSData (Guid projectId,string UserId, IAGSGroupTables tables, string filename, string format = "ags") {

           AGSWriter writer = new AGSWriter(tables);

           string s1 = writer.CreateAGS404String(null);
           
           var _data =  new ge_data {
                            Id = Guid.NewGuid(),
                            projectId = projectId,
                            createdId = UserId,
                            createdDT = DateTime.Now,
                            editedDT = DateTime.Now,
                            editedId = UserId,
                            filename = filename,
                            filesize = s1.Length,
                            fileext = ".ags",
                            filetype = "text/plain",
                            filedate = DateTime.Now,
                            encoding = "utf-8",
                            datumProjection = datumProjection.NONE,
                            pstatus = PublishStatus.Uncontrolled,
                            cstatus = ConfidentialityStatus.RequiresClientApproval,
                            version= "P01.1",
                            vstatus= VersionStatus.Intermediate,
                            qstatus = QualitativeStatus.AECOMFactual,
                            description= "AGS conversion",
                            operations ="Read;Download;Update;Delete",
                            file = new ge_data_file {
                                 data_string = s1
                                }
                            };
            await CreateData (_data);

            return s1;
        }
    }
}
    