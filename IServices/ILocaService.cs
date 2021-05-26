 using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.OtherDatabase;
using ge_repository.ESRI;

namespace ge_repository.interfaces
{
    public interface ILocaService
    {
    Boolean UpdateProjectionLoc(_ge_location loc, out string msg, string sourceCoordSystem);
    
    }
}

