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
       
        protected EsriClient _client;
        protected EsriAppClient _appClient;
        protected EsriFeatureTable _featureTable;
        protected DataTable _feature;
        public List<TEntity> list {get;set;}
        public RepositoryEsri(EsriClient Client, EsriAppClient AppClient, EsriFeatureTable FeatureTable) {}
        public RepositoryEsri() {}
        public void SetConnections(EsriClient Client, EsriAppClient AppClient, EsriFeatureTable FeatureTable) {
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

    public class RepositoryParentEsri<TEntity> : RepositoryEsri<TEntity>, IEsriParentRepository<TEntity> where TEntity: class, IEsriParent, new() {
       
        public RepositoryParentEsri(EsriClient Client, EsriAppClient AppClient, EsriFeatureTable FeatureTable) : base (Client,AppClient, FeatureTable) {
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
      public class RepositoryChildEsri<TEntity> : RepositoryEsri<TEntity>, IEsriChildRepository<TEntity> where TEntity: class, IEsriChild, new() {
       
        public RepositoryChildEsri(EsriClient Client, EsriAppClient AppClient, EsriFeatureTable FeatureTable):base (Client,AppClient,FeatureTable) {
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
}

