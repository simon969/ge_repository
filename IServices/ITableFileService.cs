using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.ESRI;
using ge_repository.spatial;
using ge_repository.Models;
using ge_repository.OtherDatabase;
using ge_repository.AGS;
namespace ge_repository.interfaces {


public interface ITableFileService {
    ge_data_table NewFile( ge_search dic, 
                                string[] lines,
                                Guid dataId,
                                Guid templateId);
    Task<ge_data_table> NewFile(Guid Id, 
                                     Guid templateId, 
                                     string table, 
                                     string sheet, 
                                     IDataService _dataService);    
}

public interface IDataTableFileService: IDataService {
 Task<ge_data> CreateData(Guid projectId, string UserId, ge_data_table dt_file, string filename, string description, string format); 
}

public interface ITableFileAGSService {
  Task<IAGSGroupTables> CreateAGS (Guid Id,Guid tablemapId,string[] agstables, string options, IDataService _dataService);
  IAGSGroupTables CreateAGS (ge_data_table dt_file, ge_table_map mapping, string[] agstables,string options);
}
public interface ITableFileKMLService {
  Task<KMLDoc> CreateKML (Guid Id,Guid tablemapId,string options, IDataService _dataService);
  KMLDoc CreateKML (ge_data_table dt_file, ge_table_map mapping, string options);
}
}