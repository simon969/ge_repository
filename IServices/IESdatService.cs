using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.ESRI;
using ge_repository.ESdat;
using ge_repository.OtherDatabase;
using ge_repository.AGS;
namespace ge_repository.interfaces {


public interface IESdatFileService {
    ge_esdat_file NewFile( ge_search dic, 
                                string[] lines,
                                Guid dataId,
                                Guid templateId);
    Task<ge_esdat_file> NewFile(Guid Id, 
                                     Guid templateId, 
                                     string table, 
                                     string sheet, 
                                     IDataService _dataService);    
}

public interface IDataESdatFileService: IDataService {
 Task<ge_esdat_file> NewData(Guid projectId, string UserId, ge_esdat_file esdat_file, string format); 
}

public interface IESdatAGSService {
  Task<IAGSGroupTables> CreateAGS (Guid Id,Guid tablemapId,string[] agstables,string options,IDataService _dataService);
  IAGSGroupTables CreateAGS (ge_esdat_file es_file, ge_table_map mapping,string[] agstables,string options);
}

}