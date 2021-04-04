using System;
using Microsoft.Extensions.DependencyInjection;
using ge_repository.interfaces;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.AGS;
using ge_repository.repositories;

namespace ge_repository.services
{
    public class AGSConvertXMLService : IAGSConvertXMLService {
           
        protected private IServiceScopeFactory _serviceScopeFactory;

        public AGSConvertXMLService (IServiceScopeFactory ServiceScopeFactory) { 
            _serviceScopeFactory = ServiceScopeFactory;
        }
    
        public async Task<ge_AGS_Client.enumStatus> NewAGSClientAsync(  ags_config Config, 
                                                                        Guid Id,
                                                                        String UserId) {
            return  await Task.Run(()=> NewAGSClient (Config,
                                                      Id,
                                                      UserId));
        }
        
        public ge_AGS_Client.enumStatus NewAGSClient(   ags_config Config,
                                                        Guid Id, 
                                                        String UserId) {
            var scope = _serviceScopeFactory.CreateScope() ;
            ge_DbContext _context = scope.ServiceProvider.GetService<ge_DbContext>();

            IUnitOfWork _unit = new UnitOfWork(_context); 
            IDataService _dataService = new DataService(_unit);
          
            ge_AGS_Client ac = new ge_AGS_Client(Config, 
                                                Id,
                                                _dataService, 
                                                UserId);
            if (!ac.IsConnected ()) {
                return AGS_Client_Base.enumStatus.NotConnected;
            }

            ac.start();
            
            return AGS_Client_Base.enumStatus.Connected;
        }
   
    }
}
    