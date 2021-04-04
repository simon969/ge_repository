using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.OtherDatabase;
using ge_repository.AGS;

namespace ge_repository.interfaces {
 public interface IAGSConvertXMLService  {

    Task<ge_AGS_Client.enumStatus> NewAGSClientAsync(ags_config Config, Guid Id, String UserId) ;
    ge_AGS_Client.enumStatus NewAGSClient(ags_config Config, Guid Id, String UserId) ; 
    
    }
}
