using System;
using System.Threading.Tasks;
using ge_repository.interfaces;
using ge_repository.Models;
using ge_repository.AGS;
using ge_repository.spatial;
using static ge_repository.Extensions.Extensions;
using static ge_repository.Authorization.Constants;
namespace ge_repository.services
{
    public class DataKMLService : DataService, IDataKMLService {

    public DataKMLService (IUnitOfWork unitOfWork): base (unitOfWork) {}

    
    public async Task<ge_data> CreateData (Guid projectId,string UserId,KMLDoc doc, string filename, string description, string format) {
        
        ge_MimeTypes mtypes = new ge_MimeTypes();
        
        string fileext = ".kml";
        
        string s1 = doc.SerializeToXmlString<KMLDoc>();
        
        if (format=="kml") fileext = ".kml";
        if (format=="json") fileext = ".json";
 
        string filetype = mtypes[fileext];

        var _data =  new ge_data {
                            Id = Guid.NewGuid(),
                            projectId = projectId,
                            createdId = UserId,
                            createdDT = DateTime.Now,
                            editedDT = DateTime.Now,
                            editedId = UserId,
                            filename = filename,
                            filesize = s1.Length,
                            fileext = fileext,
                            filetype = filetype,
                            filedate = DateTime.Now,
                            encoding = "utf-8",
                            datumProjection = datumProjection.NONE,
                            pstatus = PublishStatus.Uncontrolled,
                            cstatus = ConfidentialityStatus.RequiresClientApproval,
                            version= "P01.1",
                            vstatus= VersionStatus.Intermediate,
                            qstatus = QualitativeStatus.AECOMFactual,
                            description= description,
                            operations ="Read;Download;Update;Delete",
                            file = new ge_data_file {
                                 data_xml = s1
                                }
                            };
            
            return await CreateData (_data);


      }

    }
}


