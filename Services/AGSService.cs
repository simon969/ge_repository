using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using ge_repository.interfaces;
using ge_repository.OtherDatabase;
using ge_repository.Models;
using ge_repository.AGS;

namespace ge_repository.services
{
    public class DataAGSService : DataService, IDataAGSService {

    public DataAGSService (IUnitOfWork unitOfWork): base (unitOfWork) {}
    
    public async Task<AGS404GroupTables> GetAGS404GroupTables(Guid Id, string[] groups) {

            string[] _lines = await GetFileAsLines(Id);

            if (_lines == null) {
                return null;
            }
            
            AGSReader reader = new AGSReader(_lines);
            AGS404GroupTables ags_tables = reader.CreateAGS404GroupTables(groups);
            
            return ags_tables;
    }

    }
}
    