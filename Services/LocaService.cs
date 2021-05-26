 using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.interfaces;
using ge_repository.DAL;
using static ge_repository.Authorization.Constants;

 namespace ge_repository.services

{
    public class LocaService : ILocaService
    {
 
 
    public bool UpdateProjectionLoc(_ge_location loc, out string msg, string sourceCoordSystem = "") {
            
            bool retvar= false;

            if (loc.datumProjection == datumProjection.NONE) {
                retvar=true;
                msg = "No datumProjection";
            } else {  
                
                ProjectionSystem ps = new ProjectionSystem();
                ige_projectionDAL pd = ps.getProjectionDAL(loc);
                
                if (pd == null) {
                    msg = "datumProjection not understood";
                    retvar = false;
                } else {
                 retvar = ps.updateAll(sourceCoordSystem); 
                 msg = pd.getMessage();  
                }
            } 

            return retvar; 
    }

    }

}

