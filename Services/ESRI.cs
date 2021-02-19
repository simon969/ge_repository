using System;
using System.Linq;
using System.Net.Http;
using System.Globalization;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml.Serialization;
using ge_repository.Extensions;
using ge_repository.OtherDatabase;
namespace ge_repository.ESRI
{
// Courtesy of 
//https://gist.github.com/glenhallworthreadify/c9c377720de165103a73b06afa0a151b
    public static class Esri {
        private static Boolean AdjustForBST = false;
        
        // EPSG:4326: WGS 84
        public static int esriWGS84 = 4326;
        
        public static int esriOSGB36 = 7405;
      
        public static DateTime getDate(long epoch) {
                //https://community.esri.com/thread/215861-how-do-you-convert-epoch-dates-in-excel-power-bi-query-access-from-geodatabase
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch/1000);
        }
        public static class OrderBy {
                public const int None = 0; 
                public const int Ascending = 1;
                public const int Descending = 2;
               
        }
        public static long getEpoch(DateTime datetime) {
                return (datetime - new DateTime(1970,1,1,0,0,0)).Seconds * 1000;
        }
        public static DateTime getDateTimeWithTime(DateTime date, String time) {
        
            try {
            DateTime t = DateTime.ParseExact(time, "HH:mm",
                                                CultureInfo.InvariantCulture);
            

            DateTime dt = new DateTime( date.Year, 
                                        date.Month,
                                        date.Day,
                                        t.Hour,
                                        t.Minute,
                                        t.Second);

            if (AdjustForBST == false) {
                return dt;
            }
            
            TimeZoneInfo utcZone =  TimeZoneInfo.FindSystemTimeZoneById("UTC");
            TimeZoneInfo bstZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        
            if (bstZone.IsDaylightSavingTime (dt)) { 
            Console.Write (dt);
            }

            DateTime dt2  = TimeZoneInfo.ConvertTime(dt,  bstZone, utcZone);
            
            return dt2;

            } 
            
            catch (FormatException e) {
                return date;
            }
 
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
        public int objectid { get; set; }
    }

    public class EsriGeometry {
   
        [JsonProperty(PropertyName="x")]
        public double x {get;set;}
        [JsonProperty(PropertyName="y")]
        public double y {get;set;}
        
    }
    public class EsriGeometryWithAttributes: EsriAttributes {
    
        [JsonProperty(PropertyName="x")]
        public double x {get;set;}
        
        [JsonProperty(PropertyName="y")]
        public double y {get;set;}
        
        public double East {get;set;}
        public double North {get;set;}
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
           private string globalid="globalid";
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
        public async Task<string> getFeatureIdsOnlyByContent(string Where) {
            
            if (!String.IsNullOrEmpty(Where)) {
                where = Where;
            }

        // https://developers.arcgis.com/rest/services-reference/query-feature-service-layer-.htm
             var values = new List<KeyValuePair<string, string>>();
             values.Add(new KeyValuePair<string, string>("where", where));
             values.Add(new KeyValuePair<string, string>("token", token));
             values.Add(new KeyValuePair<string, string>("f", f));
             values.Add(new KeyValuePair<string, string>("returnIdsOnly", "true")); 
             var content = new FormUrlEncodedContent(values);
            
            // var url = $"{queryFeatureUrl}?f={f}&where={where}&token={token}&returnIdsOnly=true";
            var response = await _httpClient.PostAsync(queryFeatureUrl, content);
            var result =
                await response.Content.ReadAsStringAsync();
           return result;

        }
         public async Task<string> getFeatureIdsOnly(string Where) {
            
            if (!String.IsNullOrEmpty(Where)) {
                where = Where;
            }

        // https://developers.arcgis.com/rest/services-reference/query-feature-service-layer-.htm

            var url =
                $"{queryFeatureUrl}?f={f}&where={where}&token={token}&returnIdsOnly=true";
            var response = await _httpClient.PostAsync(url, null);
            var result =
                await response.Content.ReadAsStringAsync();
           return result;

        }
        
        public async Task<string[]> getFeaturesArray(string Where, int page_size=250, int[] pages = null, int OrderBy = Esri.OrderBy.None) {
                
                
                if (Where == null) {
                    Where = "";
                }
                
                var t1 = await getFeatureIdsOnlyByContent(Where);
                var globalids  = JsonConvert.DeserializeObject<EsriGlobalIdOnly>(t1);
                
                if (globalids.objectIds == null) {
                    return null;
                }
                
                if (globalids.objectIds.Count() == 0) {
                    return null;
                }

                int[] array = globalids.objectIds;
                if (pages != null) {
                    if (pages.Length == 0) {
                        pages=null;
                    }
                }

                if (OrderBy == Esri.OrderBy.Ascending) {
                array = array.OrderBy(c => c).ToArray(); 
                }
                
                if (OrderBy == Esri.OrderBy.Descending) {
                array = array.OrderByDescending(c => c).ToArray(); 
                }

                int total_pages = Convert.ToInt32(array.Length / page_size + 1);
                
                string[] resp = new string[total_pages];

                for (int id = 0; id < total_pages; id++) {
                    if (pages != null) {
                        if (!pages.Contains(id+1)) {
                        continue;
                        }
                    }
                    int offset = id * page_size;
                    int length = page_size;
                    if (offset + length > array.Length) {
                    length = array.Length-offset;
                    }
                    int[] list = array.sub_array(offset,length);
                    string where2 = "objectid in (" + list.ToDelimString(",") + ")"; 
                    var s1 = await getFeatures (where2);
                    resp[id] = (s1);
                }

                return resp;

        }
        public async Task<string> getFeatures(string Where) {
            
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
            
            // https://developers.arcgis.com/rest/services-reference/update-features.htm
            if (!String.IsNullOrWhiteSpace(Features)) {
                features = Features;
            }
             var values = new List<KeyValuePair<string, string>>();
             values.Add(new KeyValuePair<string, string>("features", features));
             values.Add(new KeyValuePair<string, string>("token", token));
             values.Add(new KeyValuePair<string, string>("f", f));
             var content = new FormUrlEncodedContent(values);

          //  var url = $"{updateFeatureUrl}?f={f}&features={features}&token={token}";
            var response = await _httpClient.PostAsync(updateFeatureUrl, content);
            var result =  await response.Content.ReadAsStringAsync();
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
        public esriFeature() {  }
        }

        public class esriFeatureLayer<T>: esriFeature<T> {
        // "geometryType": "<geometryType>", //for feature layers only
        public string geometryType {get;set;}
        // "spatialReference": <spatialReference>, //for feature layers only
        public spatialReference spatialReference {get;set;}

        public esriFeatureLayer() { }
        }

public class EsriGlobalIdOnly {
    public string objectIdFieldName {get;set;}
    public int[] objectIds {get;set;}
    public serverGen serverGens {get;set;}

}

public class uniqueField {
    string name {get;set;}
    Boolean isSystemMaintained {get;set;}
}
public class spatialReference {
    // "spatialReference":{"wkid":4326,"latestWkid":4326}
    public int wkid {get;set;}
    public int latestWkid {get;set;}
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
    public EsriGeometry geometry {get;set;}
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
 
 public interface IEsriGeometryWithAttributes {
    
       double x {get;set;}
        
       double y {get;set;}
        
       double East {get;set;}
       double North {get;set;}

 }

 public interface IEsriParent {
    int objectid {get;set;}	 // objectid, esriFieldTypeOID, ObjectID, sqlTypeOther, 
    Guid globalid {get;set;}
    double? surv_g_level {get;set;}
    string QA_status {get;set;}	// QA_status, esriFieldTypeString, QA_status, sqlTypeOther, 1000
    string QA_check_by {get;set;}	// QA_check_by, esriFieldTypeString, QA_check_by, sqlTypeOther, 1000
    // int AddDip(MONG mg, LTM_Survey_Data2 survey, List<MOND> MOND);
    // int AddGas(MONG mg, LTM_Survey_Data2 survey, List<MOND> MOND) ;
    // int AddPurge(MONG mg, LTM_Survey_Data2 survey, List<MOND> MOND);
    // int AddTopo(MONG mg, LTM_Survey_Data2 survey, List<MOND> MOND);
    // int AddVisit(POINT pt, LTM_Survey_Data2 survey, List<MONV> MONV);
 }
 public interface IEsriChild {

    int objectid {get;set;}	// objectid, esriFieldTypeOID, ObjectID, sqlTypeOther, 
    Guid globalid {get;set;}
    Guid parentglobalid {get;set;}	// parentglobalid, esriFieldTypeGUID, ParentGlobalID, sqlTypeGUID, 38
  //  int AddGas(MONG mg, LTM_Survey_Data2 survey, LTM_Survey_Data_Repeat2 survey2, List<MOND> MOND);
 }
 
}

