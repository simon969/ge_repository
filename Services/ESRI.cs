using System;
using System.Net.Http;
using System.Globalization;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ge_repository.Services
{
// Courtesy of 
//https://gist.github.com/glenhallworthreadify/c9c377720de165103a73b06afa0a151b
    public static class Esri {
        
        public static DateTime getDate(long epoch) {
                //https://community.esri.com/thread/215861-how-do-you-convert-epoch-dates-in-excel-power-bi-query-access-from-geodatabase
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch/1000);
        }
        
        public static long getEpoch(DateTime datetime) {
                return (datetime - new DateTime(1970,1,1,0,0,0)).Seconds;
        }
 
        public static DateTime setDateWithTime(DateTime date, String time) {
       
        DateTime t = DateTime.ParseExact(time, "HH:mm",
                                            CultureInfo.InvariantCulture);
        DateTime dt = new DateTime( date.Year, 
                                    date.Month,
                                    date.Day,
                                    t.Hour,
                                    t.Minute,
                                    t.Second);
        return dt;
     
        }
    }

    public class EsriTokenResponse {
    
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresInMinutes { get; set; }
    }
    
    public class EsriAppTokenResponse {
            
        [JsonProperty(PropertyName = "token")]
        public string AccessToken { get; set; }
        
        [JsonProperty(PropertyName = "expires")]
        public long ExpiresInMinutes { get; set; }

        [JsonProperty (PropertyName="ssl")]
        public Boolean SSL {get;set;}
    }


    public class EsriFeature {

        [JsonProperty(PropertyName="attributes")]
        public EsriAttributes attributes{get;set;}
        [JsonProperty(PropertyName="geometry")] 
        public EsriGeometry geometry {get;set;}
    }

    public class EsriAttributes {
        [JsonProperty(PropertyName = "objectid")]
        public string objectid { get; set; }
    }

    public class EsriGeometry {
    
        [JsonProperty(PropertyName="x")]
        public double x {get;set;}
        [JsonProperty(PropertyName="y")]
        public double y {get;set;}
        
    }


    public class EsriClient
    {
        private HttpClient _httpClient {get; set;}
        public string Id {get;set;} = "AECOM";
        public string tokenUrl {get;set;}  = "https://www.arcgis.com/sharing/rest/oauth2/token";
        public string clientId {get;set;}  = "W8RLr2EtX5ZijoGN"; // update
        public string clientSecret {get;set;}  = "3fc47f44369c4f96ae9fc7cf71075cfb"; // update
        public string grantType {get;set;}    = "client_credentials";
        public int expirationInMinutes {get;set;}   = 120;
        public EsriClient() {}
        public EsriClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<EsriTokenResponse> GetToken(HttpClient httpClient = null)
        {
            if (httpClient != null) {
                _httpClient = httpClient;
            }

            var url =
                $"{tokenUrl}?client_id={clientId}&client_secret={clientSecret}&grant_type={grantType}&expiration={expirationInMinutes}";
            var response = await _httpClient.PostAsync(url, null);
            var result =
                await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<EsriTokenResponse>(result);
            if (string.IsNullOrWhiteSpace(token.AccessToken)) // Esri does not respect HTTP status codes and will always return 200. It puts errors in the body. 
            {
                throw new Exception("Could not retrieve Esri Token");
            }
            return token;
        }
    }

    public class EsriAppClient {
        private HttpClient _httpClient;
        public string Id {get;set;} = "LTC";
        public string tokenUrl {get;set;}  = "https://pc-ltc.maps.arcgis.com/sharing/generateToken"; //update
        public string request {get;set;}  = "getToken";
        public string f {get;set;}  = "json";
        public string username {get;set;}  = "ltc.system.access"; // update
        public string password {get;set;}  = "0system.access0"; // update
        public string referer {get;set;}  = "pc-ltc.maps.arcgis.com"; //update
        private string token1 = ""; 
        public EsriAppClient() {

        }
        public EsriAppClient(HttpClient httpClient, string Token)
        {
            _httpClient = httpClient;
            token1 = Token;
        }
        public EsriAppClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            get_token1(); 
           
        
        }
        private Boolean get_token1() {
            EsriClient eClient = new EsriClient(_httpClient);
            var token = eClient.GetToken();
            token1 = token.Result.AccessToken;
            return true;
        }
        public async Task<EsriAppTokenResponse> GetToken(HttpClient httpClient = null)
        {
            if (httpClient != null) {
                _httpClient = httpClient;
                get_token1();
            }

            var url =
                $"{tokenUrl}?request={request}&token={token1}&f={f}&username={username}&password={password}&referer={referer}";
            var response = await _httpClient.PostAsync(url, null);
            var result =
                await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<EsriAppTokenResponse>(result);
            if (string.IsNullOrWhiteSpace(token.AccessToken)) // Esri does not respect HTTP status codes and will always return 200. It puts errors in the body. 
            {
                throw new Exception("Could not retrieve Esri Token");
            }
            return token;
        }
    }
    public class dicFeatureUrls : Dictionary<string,string> {
        public dicFeatureUrls() {
            Add ("HOLESPHASE2","https://services9.arcgis.com/4MYxhHBDmXiGXKqw/arcgis/rest/services/Exploratory_Holes_Phase2/FeatureServer/1/query");
            Add ("LTMSURVEY1","https://services9.arcgis.com/4MYxhHBDmXiGXKqw/arcgis/rest/services/service_ba523c33499f4b05ba5e0536836d572f/FeatureServer/0");
            Add ("HOLEPHASE2_UPDATE","https://services9.arcgis.com/4MYxhHBDmXiGXKqw/arcgis/rest/services/Exploratory_Holes_Phase2/UpdateFeature/1/query");
            Add ("TEST_LTM","https://services9.arcgis.com/4MYxhHBDmXiGXKqw/arcgis/rest/services/TEST_LTM/FeatureServer/1/query");
            Add ("LTM_MON_DATA","https://services9.arcgis.com/4MYxhHBDmXiGXKqw/arcgis/rest/services/TEST_LTM/FeatureServer/10/query");

        }
    }
    class  EsriFeatureRequest {
           private readonly HttpClient _httpClient;
           private readonly string featureUrl;
           private readonly string f="json";
           private string where ="1=1"; 
           private string outFields="*";
           private readonly string token;
           
        public EsriFeatureRequest(HttpClient httpClient, string Token, string FeatureUrl)
        {
            _httpClient = httpClient;
            token = Token;

            featureUrl = FeatureUrl;    
            }
        public async Task<string> getFeature(string Where) {
            
            if (!String.IsNullOrEmpty(Where)) {
                where = Where;
            }

        // https://developers.arcgis.com/rest/services-reference/query-feature-service-layer-.htm

            var url =
                $"{featureUrl}?f={f}&where={where}&outFields={outFields}&token={token}";
            var response = await _httpClient.PostAsync(url, null);
            var result =
                await response.Content.ReadAsStringAsync();
           return result;
        }
    }
    class  EsriFeatureQueryRequest {
           private readonly HttpClient _httpClient;
           private readonly string queryFeatureUrl;
           private readonly string f="json";
           private string where ="1=1"; 
           private string outFields="*";
           private readonly string token;
           
        public EsriFeatureQueryRequest(HttpClient httpClient, string Token, string QueryFeatureUrl)
        {
            _httpClient = httpClient;
            token = Token;

            dicFeatureUrls dic = new dicFeatureUrls();

            if (dic.ContainsKey(QueryFeatureUrl)) {
            queryFeatureUrl=dic.GetValueOrDefault(QueryFeatureUrl);
            } else {
            queryFeatureUrl = QueryFeatureUrl;    
            }
        }
        public async Task<string> getFeature(string Where) {
            
            if (!String.IsNullOrEmpty(Where)) {
                where = Where;
            }

        // https://developers.arcgis.com/rest/services-reference/query-feature-service-layer-.htm

            var url =
                $"{queryFeatureUrl}?f={f}&where={where}&outFields={outFields}&token={token}";
            var response = await _httpClient.PostAsync(url, null);
            var result =
                await response.Content.ReadAsStringAsync();
           return result;
        }
    }

        class  EsriFeatureUpdateRequest {
           private readonly HttpClient _httpClient;
           private readonly string updateFeatureUrl;
           private readonly string f="json";
           private string features {get;set;} = ""; 
           private readonly string token;
        public EsriFeatureUpdateRequest(HttpClient httpClient, string Token, string UpdateFeatureUrl)
        {
            _httpClient = httpClient;
            token = Token;
            dicFeatureUrls dic = new dicFeatureUrls();
            if (dic.ContainsKey(UpdateFeatureUrl)) {
            updateFeatureUrl = dic.GetValueOrDefault(UpdateFeatureUrl);
            } else {
            updateFeatureUrl = UpdateFeatureUrl;    
            }
        }
        public async Task<string> updateFeatures(string Features) {
            if (!String.IsNullOrWhiteSpace(Features)) {
                features = Features;
            }
        // https://developers.arcgis.com/rest/services-reference/update-features.htm
            var url =
                $"{updateFeatureUrl}?f={f}&features={features}&token={token}";
            var response = await _httpClient.PostAsync(url, null);
            var result =
                await response.Content.ReadAsStringAsync();
           return result;
        }
        
        }
        public class esriFeature<T> {

        public string objectIdFieldName {get;set;}
        public uniqueField uniqueIdField {get;set;}
        public string globalIdFieldName {get;set;}
        public serverGen serverGens {get;set;}  
        public List<field> fields {get;set;}
        public List<items<T>> features {get;set;}
        public esriFeature() {
            
        }
}


public class uniqueField {
    string name {get;set;}
    Boolean isSystemMaintained {get;set;}
}

public class serverGen {
public int minServerGen {get;set;}
public int ServerGen {get;set;}

}
public class field {
    public string name {get;set;}
    public string type {get;set;}
    public string alias {get;set;}
    public string sqlType {get;set;}
    public int length {get;set;}
    public domain domain {get;set;}
    public string defaultValue {get;set;}
}

public class domain {
    public string type {get;set;}
    public string name {get;set;}
    public List<codedValues> codedValues {get;set;}
}

public class codedValues {
    public string name;
    public string code;
}

public class items<T> {
     public T attributes {get;set;}
}

public class EsriFeatureTable {
    public string Name {get;set;}
    public string BaseEndPoint {get;set;}
    public List<EsriService> services {get;set;}
}

public class EsriService {
    public string Url {get;set;}
    public string geServiceAction {get;set;}
    public string EsriServiceAction {get;set;}
}
public class EsriDataSet {
    public string Name {get;set;}
    
    [XmlArray("tables"), XmlArrayItem(typeof(string), ElementName = "EsriFeatureTableName")]
    public List<string> tables {get;set;}
}


public class EsriConnectionSettings {

    public EsriClient EsriClient {get;set;}
    public EsriAppClient EsriAppClient {get;set;}
    public List<EsriFeatureTable> features {get;set;}
    public List<EsriDataSet> datasets {get;set;}
}
 
}

