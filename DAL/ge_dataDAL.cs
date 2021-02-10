using System;
using System.Collections.Generic;
using ge_repository.Models;
using ge_repository.Authorization;

using ge_repository.DAL;

namespace ge_repository.DAL {
    
    public class ge_dataDAL: ige_dataDAL {

    public ge_DbContext context {get;set;}
    public 
    ge_dataDAL(ge_DbContext context) { 
        this.context = context; 
        
    }
    public IEnumerable<ge_data> getOfficeData(Guid officeId){return null;}
    public IEnumerable<ge_data> getProjectData(Guid projectId){return null;}
    public ge_data getDataById(Guid Id){return null;}
    public ge_data_file getBigDataById(Guid Id){return null;}
    public void insertData(ge_data data){}
    public void insertBigData(ge_data_file data){}
    public void deleteData(int dataId){}
    public void updateData(ge_data data){}
    public void Save(){}
    public void Dispose(){}
    }
}






