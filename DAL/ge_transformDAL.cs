using System;
using System.Collections.Generic;
using ge_repository.Models;
using ge_repository.Authorization;

using ge_repository.DAL;

namespace ge_repository.DAL {
    
    public class ge_transformDAL: ige_transformDAL {

    public ge_DbContext context {get;set;}
    public 
    ge_transformDAL(ge_DbContext context) { 
        this.context = context; 
        
    }
    public IEnumerable<ge_transform> getOfficeTransform(Guid officeId){return null;}
    public IEnumerable<ge_transform> getProjectTransform(Guid projectId){return null;}
    public ge_transform getTransform(Guid Id){return null;}
    public void insertTransform(ge_transform transform){}
    public void deleteTransform(Guid Id){}
    public void updateTransform(ge_transform transform){}
    public void Save(){}
    public void Dispose(){}
    }

    
}






