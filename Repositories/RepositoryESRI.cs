using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ge_repository.interfaces;
using ge_repository.ESRI;

namespace ge_repository.repositories
{
    
    public class RepositoryEsri<TEntity> : IEsriRepository<TEntity> where TEntity: class {
       
        private readonly EsriClient _client;
        private readonly EsriAppClient _appClient;
        private readonly EsriFeatureTable _featureTable;
        private DataTable _feature;
        public List<TEntity> list {get;set;}

        public RepositoryEsri(EsriClient Client, EsriAppClient AppClient, EsriFeatureTable FeatureTable) {
            _client = Client;
            _appClient = AppClient;
            _featureTable = FeatureTable;
        }
        
        public async  Task<string[]> getFeatures(string where = "", int page_size = 250, int[] pages = null, int orderby=Esri.OrderBy.None) {
        
            HttpClient client = new HttpClient();
            
            var token2 = _appClient.GetToken(client);

            EsriService es = _featureTable.services.FirstOrDefault(s=>s.geServiceAction=="getFeatures");
            
            EsriFeatureQueryRequest  eFeature = new EsriFeatureQueryRequest(client, token2.Result.AccessToken,es.Url);

            var result = await eFeature.getFeaturesArray(where, page_size, pages, orderby);
            
            // Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
            // settings.Formatting = Newtonsoft.Json.Formatting.Indented;

            return result;

        //    return View(result);

        }

    }
       
        
    public class RepositoryEsri<TParent, TChild> : IEsriRepository<TParent, TChild> where TParent : class where TChild : class
    {
        
        public Task<string[]> getFeatures(string where, int page_size, int[] pages, int orderby) {
            return null;
        }
        public Task<string> GetParentGlobalIdsOnly() {
            return null;
        }

        public Task<List<TParent>> GetParentWhereGlobalIdsIn(string globalids) {
            return null;
        } 
        Task<List<TChild>> GetChildWhereGlobalIdsIn(string globalids) {
            return null;
        } 

    }
}
