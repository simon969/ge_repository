
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ge_repository.Models;
using ge_repository.Authorization;
using static ge_repository.Authorization.Constants;
using ge_repository.Extensions;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using ge_repository.OtherDatabase;
using Newtonsoft.Json;
using ge_repository.ESRI;
using ge_repository.LowerThamesCrossing;
using System.Xml;
using System.Xml.Serialization;

namespace ge_repository.Controllers
{
    public class ge_esriController: ge_Controller  {     

   

     public ge_esriController(

            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,  
            IHostingEnvironment env ,
            IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
           
        }

    private async Task<EsriConnectionSettings> getEsriConnection (Guid projectId) {
            
            if (projectId==null) {
            return  null;
            }
        
            var project = await _context.ge_project
                        .Include(p =>p.group)
                        .FirstOrDefaultAsync(m => m.Id == projectId);

            if (project == null) {
                return  null;
            }

            if (project.esriConnectId==null) {
            return  null;
            }

            var cs = await new ge_dataController(  _context,
                                                        _authorizationService,
                                                        _userManager,
                                                        _env ,
                                                        _ge_config).getDataAsClass<EsriConnectionSettings>(project.esriConnectId.Value); 

            return cs; 

    }

    private async Task<EsriConnectionSettings> _getEsriConnection (Guid projectId) {
            
            if (projectId==null) {
            return  null;
            }
        
            var project = await _context.ge_project
                        .Include(p =>p.group)
                        .FirstOrDefaultAsync(m => m.Id == projectId);

            if (project == null) {
                return  null;
            }

            if (project.esriConnectId==null) {
            return  null;
            }

             

            var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == project.esriConnectId);
            if (_data == null)
                {
                return null;
            }

            Encoding encoding = _data.GetEncoding();

            var _data_big = await _context.ge_data_file.SingleOrDefaultAsync(m => m.Id == project.esriConnectId);
            
            if (_data_big == null)
            {
                return null;
            }
            try {

            XmlSerializer serializer = new XmlSerializer(typeof(EsriConnectionSettings));
            EsriConnectionSettings cs = (EsriConnectionSettings) serializer.Deserialize(_data_big.getMemoryStream(encoding));
            
            return cs; 

            } catch (Exception) {
                return null;
            }



    }

    [AllowAnonymous]
     public async  Task<JsonResult> getFeatures(Guid projectId, string name, string where = "", int page_size = 250, int[] pages = null, int orderby=Esri.OrderBy.None) {
       
              
        
        var cs = await getEsriConnection(projectId);
        
        if (cs==null) {
        return Json ("No EsriConnectionSettings found");
        }

        EsriAppClient eaClient = cs.EsriAppClient;
        
        if (eaClient == null) {
        return Json ("No EsriAppClient found");
        }

        EsriFeatureTable eft = cs.features.FirstOrDefault(f=>f.Name==name);
        
        if (eft==null) {
        return Json ($"Esri feature table {name} not found in connection file");
        }

        EsriActionService es = eft.services.FirstOrDefault(s=>s.geServiceAction=="getFeatures");
        
        if (es==null) {
        return Json ($"Esri feature table {name} does not have query service in connection file");
        }

        HttpClient client = new HttpClient();
        
        var token2 = eaClient.GetToken(client);

        EsriFeatureQueryRequest  eFeature = new EsriFeatureQueryRequest(client, token2.Result.AccessToken,es.Url);

        var result = await eFeature.getFeaturesArray(where, page_size, pages, orderby);
        
        Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
        settings.Formatting = Newtonsoft.Json.Formatting.Indented;

        return Json (result,settings);

    //    return View(result);

    }


    public async Task<JsonResult> updateFeatures(Guid dataId, string table) {
            
        if (dataId==null) {
        return  null;
        }
    
        var data = await _context.ge_data
                    .Include(p =>p.project)
                    .FirstOrDefaultAsync(m => m.Id == dataId);

        if (data == null) {
            return null;
        }        

        var feature = await getFeatures(data.projectId, table);

        string s1 = JsonConvert.SerializeObject(feature.Value);
        s1 = feature.Value.ToString();
        var result1 = JsonConvert.DeserializeObject<esriFeature<Exploratory_Holes_Phase2>>(s1);
        
        Exploratory_Holes_Phase2 a1 = result1.features.FirstOrDefault().attributes;
        
        
        if (string.IsNullOrEmpty(a1.spare_txt1)) {
             a1.spare_txt1 = "testing http restful api update";
             a1.spare_num1 = 12345; 
        }
        
        string updated =  JsonConvert.SerializeObject(result1);

        HttpClient client = new HttpClient();
        
        EsriAppClient eaClient =  new EsriAppClient (client);
        var token2 = eaClient.GetToken();

        EsriFeatureUpdateRequest  eFeature = new EsriFeatureUpdateRequest(client, token2.Result.AccessToken,"HOLEPHASE2_UPDATE");
        
        var result2 = await eFeature.updateFeatures(updated);
        
        Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
        settings.Formatting = Newtonsoft.Json.Formatting.Indented;

        return Json (result2,settings);

    }

      public async  Task<IActionResult> updateFeatures(Guid projectId, string name, string features) {
            
        if (projectId==null) {
        return  BadRequest("projectId is null");
        }
    
        var project = await _context.ge_project
                    .FirstOrDefaultAsync(m => m.Id == projectId);

        if (project == null) {
            return BadRequest("project is null");
        }        

        var cs = await getEsriConnection(projectId);
        
        if (cs==null) {
        return Json ("No EsriConnectionSettings found");
        }

        EsriAppClient eaClient = cs.EsriAppClient;
        
        if (eaClient == null) {
        return Json ("No EsriAppClient found");
        }

        EsriFeatureTable eft = cs.features.FirstOrDefault(f=>f.Name==name);
        
        if (eft==null) {
        return Json ($"Esri feature table {name} not found in connection file");
        }

        EsriActionService es = eft.services.FirstOrDefault(s=>s.geServiceAction=="updateFeatures");
        
        if (es==null) {
        return Json ($"Esri feature table {name} does not have update service in connection file");
        }

        HttpClient client = new HttpClient();
        
        var token2 = eaClient.GetToken(client);

        EsriFeatureUpdateRequest  eFeature = new EsriFeatureUpdateRequest(client, token2.Result.AccessToken,es.Url);

        var result = await eFeature.updateFeatures(features);
        
        Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
        settings.Formatting = Newtonsoft.Json.Formatting.Indented;

        return Json (result,settings);

        // return View(result);

        // return result;

    }
    
    // public IActionResult Start()
    // {

    // // https://stackoverflow.com/questions/42473922/send-json-to-another-server-using-asp-net-core-mvc-c-sharp
    //  var jsonRequest = Json(new { ServerId = "1", ServerPort = "27015" }).Value.ToString();
    //                 HttpClient client = new HttpClient();
    //                 client.BaseAddress = new Uri("https://www.arcgis.com/sharing/rest/oauth2/token");
    //                 client.DefaultRequestHeaders
    //                       .Accept
    //                       .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

    //                 HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "relativeAddress");

    //                 request.Content = new StringContent(jsonRequest,
    //                                                     Encoding.UTF8,
    //                                                     "application/json");//CONTENT-TYPE header

    //                 client.SendAsync(request)
    //                       .ContinueWith(responseTask =>
    //                       {
    //                            //here your response 
    //                           Debug.WriteLine("Response: {0}", responseTask.Result);
    //                       });
    //     return View();
    // }
 private string GetToken()
    {
        var request = (HttpWebRequest) WebRequest.Create("https://www.arcgis.com/sharing/rest/oauth2/token/");

        var postData = "client_id=yourclientid"; //required
        postData += "&client_secret=yourclientsecret"; //required
        postData += "&grant_type=client_credentials"; //required
        postData += "&expiration=120"; //optional, default
        var data = Encoding.ASCII.GetBytes(postData);

        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        var response = (HttpWebResponse)request.GetResponse();

        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        EsriTokenResponse eToken = Newtonsoft.Json.JsonConvert.DeserializeObject<EsriTokenResponse>(responseString);

        return eToken.AccessToken;
    } 


}
}



