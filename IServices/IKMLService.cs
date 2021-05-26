using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.OtherDatabase;
using ge_repository.spatial;

namespace ge_repository.interfaces {

    public interface IDataKMLService: IDataService {
      
        Task<ge_data> CreateData (Guid projectId,string UserId,KMLDoc doc, string filename, string description, string format);
    }

   
}