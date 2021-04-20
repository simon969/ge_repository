using System;
using System.Threading.Tasks;
using ge_repository.interfaces;
using ge_repository.Models;
using ge_repository.AGS;
using static ge_repository.Authorization.Constants;
namespace ge_repository.services
{
    public class DataAGSService : DataService, IDataAGSService {
           
    public DataAGSService (IUnitOfWork unitOfWork): base (unitOfWork) {}
    
    public async Task<IAGSGroupTables> GetAGSData(Guid Id, string[] groups) {

            string[] _lines = await GetFileAsLines(Id);

            if (_lines == null) {
                return null;
            }
            
            AGSReader reader = new AGSReader(_lines);
            AGS404GroupTables ags_tables = reader.CreateAGS404GroupTables(groups);
            
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
    