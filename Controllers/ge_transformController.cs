using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;

using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;

using ge_repository.Models;
using ge_repository.Extensions;
using ge_repository.Authorization;
using ge_repository.DAL;
using ge_repository.AGS;
using Saxon.Api;

namespace ge_repository.Controllers 
{

     public class ge_transformController : ge_Controller
    {
       
		protected string xsl_data = "";
		protected string xml_data = "";
		protected string qry_data = "";

     	
		public ge_transform transform;

	  	public static string agsmlOpen ="<agsml>";
	    public static string agsmlClose ="</agsml>";
	    public static string constHref_transform = "/ge_transform/View?transformId=";
	   	public static string constHref_dataVIEW = "/ge_data/View?Id=";
		public static string constHref_dataDOWNLOAD = "/ge_data/Download?Id="; 
		public static string constHref_esriFEATURE ="/ge_esri"; 
		public static string constSELECTALLFROM = "select * from ";
		public static string constEXECUTE = "execute ";
		public static int MIN_COMMAND_TIMOUT = 120;
		public static string constHref_Logger = "/ge_log";
		public static string constHref_gINT = "/ge_gINT";
		public static string constHref_AGS = "/ge_ags";
		public static string constHref_LTC = "/ge_LTC";
		public static string constHref_TableFile = "/ge_tablefile";
		public static string ge_gis_xmlGet_Endpoint = "ge_gis/xmlGet";
		public static string ge_data_xmlGetData_Endpoint ="ge_data/xmlGetData";
		public static string ge_data_xmlGetProjects_Endpoint ="ge_data/xmlGetProjects";
		

    public ge_transformController(

            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
			IHostingEnvironment env,
		 	IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
		}
		public async Task<ActionResult> Run(
									Guid transformId, 
									string[] projects,
									string[] holes,
									string[] tables, 
									string[] geols,
									string[] options,
									string flwor,
									string xpath,
									string version) 
									{

			if (transformId==Guid.Empty) {
				return new EmptyResult();
			}

			transform = _context.ge_transform.
										Where (t=>t.Id == transformId).FirstOrDefault();
			
			if (transform == null ) {
				new EmptyResult();
			}

			var data = _context.ge_data.
							Where(d=>d.Id == transform.dataId).FirstOrDefault();
		
			if (data == null ) {
				new EmptyResult();
			}
			
			if (String.IsNullOrEmpty(transform.storedprocedure)) {
				new EmptyResult();
			}

			string dataId = data.Id.ToString();
			
			if (String.IsNullOrEmpty(transform.add_data)) {
				dataId = dataId + ";" + transform.add_data;
			}

			string[] data_all = dataId.Split(";");

			if (String.IsNullOrEmpty(version)) {
				version = data.AGSVersion();
			} 

			string rawSQL = getSQLCommand(constEXECUTE, data_all, holes, tables, version);

			int returnRows = await _context.Database.ExecuteSqlCommandAsync(rawSQL);
			
			if (returnRows==1) {
				return RedirectToPageMessage (msgCODE.TRANSFORM_RUN_STOREDPROCEDURE_SUCCESSFULL);
			} else {
				return RedirectToPageMessage (msgCODE.TRANSFORM_RUN_STOREDPROCEDURE_NOTSUCCESSFULL);
			}
		  //  return new OkResult();
        }
[HttpPost]	
 public async Task<ActionResult> ViewPost(Guid transformId, 
		 							Guid? Id,
									Guid? projectId,
									Guid? groupId,
									string[] projects,
									string[] holes,
									string[] tables, 
									string[] geols,
									string[] options,
									string[] args,
									string[] vals,
									string flwor,
									string xpath,
									string version) {
	return await View(transformId, 
		 				Id,
						projectId,
						groupId,
						projects,
						holes,
						tables, 
						geols,
						options,
						args,
						vals,
						flwor,
						xpath,
						version);
									

	}

         public async Task<ActionResult> View(Guid transformId, 
		 							Guid? Id,
									Guid? projectId,
									Guid? groupId,
									string[] projects,
									string[] holes,
									string[] tables, 
									string[] geols,
									string[] options,
									string[] args,
									string[] vals,
									string flwor,
									string xpath,
									string version)
        {
            
			if (transformId==Guid.Empty) {
				return new EmptyResult();
			}

			transform = _context.ge_transform.
									Include(t=>t.project).
									Where (t=>t.Id == transformId).FirstOrDefault();
			
			if (transform == null) {
				return new EmptyResult();
			}
			
			string arg_val = "";

			for (int i=0; i<args.Length; i++) {
				if (arg_val.Length>0) arg_val += "&";
				arg_val += args[i] + "=" + vals[i];
			}

			if (projectId==null) {
				projectId=transform.projectId;
			}

			if (groupId==null) {
				groupId=transform.project.groupId;
			}

			ge_transform_parameters transform_params = getTransformParameters(transformId, Id,projectId,groupId,projects, holes, tables, geols, options, arg_val, flwor, xpath, version);
			
			/* Convert JSON back to string to pass via ViewBag */
			string parameters =  JsonConvert.SerializeObject(transform_params);

			if (transform.service_endpoint !=null) {
				string endpoint = getEndPointURL(transform.service_endpoint);
				var resp  = await getServiceEndPointData(endpoint,transform_params);
				var xmlResult = resp as XmlActionResult;
				if (xmlResult!=null) {
					xml_data = xmlResult.Value();
				}
				var okResult = resp as OkObjectResult;
				if (okResult !=null) {   
					if (okResult.StatusCode==200) {
						xml_data = okResult.Value as string;
					}
				} 
            }
			
			if (transform.dataId !=null && transform.storedprocedure==null) {
			xml_data  = await new ge_dataController(  _context,
                                                        _authorizationService,
                                                        _userManager,
                                                        _env ,
                                                        _ge_config).getDataAsParsedXmlString(transform.dataId.Value); 
			}
			
			if (transform.styleId !=null) {
			xsl_data  = await new ge_dataController(  _context,
                                                        _authorizationService,
                                                        _userManager,
                                                        _env ,
                                                        _ge_config).getDataAsParsedXmlString(transform.styleId.Value); 
			}

			if (transform.queryId !=null) {
			qry_data  = await new ge_dataController(  _context,
                                                        _authorizationService,
                                                        _userManager,
                                                        _env ,
                                                        _ge_config).getDataAsString(transform.queryId.Value,false); 
			}

			if (transform.dataId !=null && transform.storedprocedure != null) {
				
				string dataId = transform.dataId.ToString();

				if (!String.IsNullOrEmpty(transform.add_data)) {
					dataId = dataId + ";" + transform.add_data;
				}

				string[] data_all = dataId.Split(";");

				string rawSQL = getSQLCommand(constSELECTALLFROM, data_all,holes,tables,version);

				ge_data_file task_xml_data_big = await _context.ge_data_file.FromSql (rawSQL).SingleOrDefaultAsync();
				xml_data = task_xml_data_big.getParsedXMLstring(null);
			}

			if (!String.IsNullOrEmpty(qry_data)) {
				string res = XQuery(xml_data, qry_data);

				if (!String.IsNullOrEmpty(res)) {
					xml_data = res;
				}
			}	


			if (xml_data == null || xsl_data == null) {
				new EmptyResult();
			}
							
			ViewBag.xml_data = xml_data;
			ViewBag.xsl_stylesheet = xsl_data;
			ViewBag.xlt_arguments = parameters;

            return View("View");
        } 
		private string getEndPointURL(string url) {

			string ret = "";
			string host_ref = getHostHref();

			ret = url.Replace ("$host", host_ref);			
			ret = ret.Replace("$host_view", host_ref + constHref_transform);
			ret = ret.Replace("$host_file", host_ref + constHref_dataVIEW);
			ret = ret.Replace("$host_download", host_ref + constHref_dataDOWNLOAD);
			ret = ret.Replace("$host_transform", host_ref + constHref_transform);
			ret = ret.Replace ("$host_esri",host_ref + constHref_esriFEATURE);
			ret = ret.Replace ("$host_logger", host_ref + constHref_Logger);
			ret = ret.Replace("$host_gint",host_ref + constHref_gINT);
			ret = ret.Replace ("$host_tablefile", host_ref + constHref_TableFile);

			return ret;
		}
		public async Task<IActionResult> getServiceEndPointData(string url, ge_transform_parameters transform_params)
        {
            Guid? Id = null;
			Guid? projectId = null;
			Guid? groupId = null;

			if (transform_params.Id!=null) {Id = new Guid(transform_params.Id);}
			if (transform_params.projectId!=null) {projectId = new Guid(transform_params.projectId);}
			if (transform_params.groupId!=null) {groupId = new Guid(transform_params.groupId);}
			
			
			if (url== ge_gis_xmlGet_Endpoint) {
				var res  = await new ge_gisController(  _context,
                                                        _authorizationService,
                                                        _userManager,
                                                        _env ,
                                                        _ge_config).xmlGet(projectId); 

				if (res==null) {
					return NotFound();
				}
				
				return res;
			}
			
			if (url== ge_data_xmlGetData_Endpoint) {
				var res  = await new ge_dataController(  _context,
                                                        _authorizationService,
                                                        _userManager,
                                                        _env ,
                                                        _ge_config).xmlGetData(Id,projectId,groupId); 

				if (res==null) {
					return UnprocessableEntity("data is null for projectid {} groupid {}");
				}
				// var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
				// var settings = new XmlWriterSettings();
        		// settings.Indent = true;
        		// settings.OmitXmlDeclaration = true;
				
				// var serializer = new XmlSerializer(typeof(List<ge_data>),
                //                    new XmlRootAttribute("ge_root"));
				
				// using (var stream = new StringWriter()) 
				// using (var writer = XmlWriter.Create(stream, settings))
				// {
    			// 	serializer.Serialize(writer, res, emptyNamespaces);
            	// 	return Ok(stream.ToString());
				// }

				string s1 = XmlSerialiseList<ge_data>(res,"ge_root");

				return Ok(s1);
			
			}
			if (url== ge_data_xmlGetProjects_Endpoint) {
				var res  = await new ge_dataController(  _context,
                                                        _authorizationService,
                                                        _userManager,
                                                        _env ,
                                                        _ge_config).xmlGetProjects(groupId); 

				if (res==null) {
					return UnprocessableEntity("ge_project is null for groupid {}");
				}
	
				//to prevent circular reference in xml serialisation;
				foreach (ge_project p in res) {
					p.transform = null;
				}
				string s1 = XmlSerialiseList<ge_project>(res,"ge_root");
				return Ok(s1);

				// var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
				// var settings = new XmlWriterSettings();
        		// settings.Indent = true;
        		// settings.OmitXmlDeclaration = true;
				// var serializer = new XmlSerializer(typeof(List<ge_project>),
                //                    new XmlRootAttribute("ge_root"));
				
				

				// using (var stream = new StringWriter()) 
				// using (var writer = XmlWriter.Create(stream, settings))
				// {
    			// 	serializer.Serialize(writer, res, emptyNamespaces);
            	// 	return Ok(stream.ToString());
				// }
			
			}

			if (url.Contains("ge_LTC2/ViewSurvey123")) {

				string hole_id = null;
				if (!String.IsNullOrEmpty(transform_params.arg_vals)) {
					string[] args = transform_params.arg_vals.Split("&");
					foreach(string arg in args) {
						if (arg.Contains("hole_id")) {
							string[] arg_val = arg.Split("=");
							hole_id = arg_val[1]; 
						}
					}
				}
				var res  = await new ge_LTC2Controller(  _context,
                                                        _authorizationService,
                                                        _userManager,
                                                        _env ,
                                                        _ge_config).ViewSurvey123(projectId.Value,hole_id); 
				
				return res;
				
			}

			string url2 = transform_params.host + "/" + url;
			
			if (!String.IsNullOrEmpty(transform_params.arg_vals)) {
				url2 += "\\?" + transform_params.arg_vals;
			} 

			var resp =  Redirect(url2);
			return resp;
			
						
			// HttpClient _httpClient = new HttpClient();
            // var response = await _httpClient.PostAsync(url2, null);
            // var result =  await response.Content.ReadAsStringAsync();
           
		    // return StatusCode((int)response.StatusCode, response);;

        }
		private string XmlSerialiseList<T>(List<T> list, string root) {

				var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
				var settings = new XmlWriterSettings();
        		// settings.Indent = true;
        		settings.OmitXmlDeclaration = true;
				
				var serializer = new XmlSerializer(typeof(List<T>),
                                   new XmlRootAttribute(root));
				
				using (var stream = new StringWriter()) 
				using (var writer = XmlWriter.Create(stream, settings))
				{
    				serializer.Serialize(writer, list, emptyNamespaces);
            		return stream.ToString();
				}

		}

		// private string getSeriveEndPointData(string service_end_point) {

		// Uri baseAddress = new Uri("http://localhost:8000/HelloService");
		// string address = "http://localhost:8000/HelloService/MyService";

		// 	using (ServiceHost serviceHost = new ServiceHost(typeof(HelloService), baseAddress))
		// 	{
		// 		serviceHost.AddServiceEndpoint(typeof(IHello), new BasicHttpBinding(), address);
		// 		serviceHost.Open();
		// 		Console.WriteLine("Press <enter> to terminate service");
		// 		Console.ReadLine();
		// 		serviceHost.Close();
		// 	}

		// }

		private string getSQLCommand(	string SQLCommand,
										string[] datas, 
										string[] holes,
										string[] tables, 
										string version ) {

				int? _timeout = _context.Database.GetCommandTimeout();
				int _timeoutEF = int.Parse(ge_config().defaultEFDBTimeOut);

				// Increase commandtimeout for stored procedures  ]'
				 if (_timeout==null) {
				_timeout = MIN_COMMAND_TIMOUT;
				 } 

				if (_timeout < _timeoutEF) {
				_timeout =  _timeoutEF;
				}

				_context.Database.SetCommandTimeout(_timeout);

				string data = datas.ToDelimString(";","'");
				string hole =  holes.ToDelimString(";","'");
				string table = tables.ToDelimString(";","'");
				string version2 = "'" + version + "'"; 
				string user = "'" + GetUserAsync().Result.Id + "'";
				String rawSQL =  SQLCommand + transform.storedprocedure.Replace(data,hole,table,user,version2);
				return rawSQL;

		}
		private ge_transform_parameters getTransformParameters( Guid transformId,
									Guid? Id,
									Guid? projectId,
									Guid? groupId,
									string[] projects,
									string[] holes,
									string[] tables, 
									string[] geols,
									string[] options,
									string arg_vals,
									string flwor,
									string xpath,
									string version){

			ge_transform_parameters transform_params = new ge_transform_parameters();

			if (!String.IsNullOrEmpty(transform.parameters)) {
				try { 
					transform_params  = Newtonsoft.Json.JsonConvert.DeserializeObject<ge_transform_parameters>(transform.parameters);
				} catch (Newtonsoft.Json.JsonReaderException e) {
 					Console.WriteLine (e.Message );
				}
			}

			/* Incase all parameters are passed in first element of parameters array purgeArray for delimiters*/
			string[] delims = new string[] {",",";"};

			if (projects.Any()) {
			projects= projects.purgeArray(delims);
			transform_params.projects = projects;	
			}
			
			if (holes.Any()) {
			holes = holes.purgeArray(delims);
			xml_data = getHoles(xml_data, holes);
			transform_params.holes = holes;
			}

			if (tables.Any()) {
			tables = tables.purgeArray(delims);
			transform_params.tables = tables;	
			} 

			if (geols.Any()) {
			geols = geols.purgeArray(delims);
			xml_data = getGeols(xml_data, geols);
			transform_params.geols = geols;
			}

			if (options.Any()) {
			options = options.purgeArray(delims);
			transform_params.options = options;	
			}
			
			if (!String.IsNullOrEmpty(arg_vals)) {
			/* To be added process FLO string */
			transform_params.arg_vals = arg_vals;
			}
			if (!String.IsNullOrEmpty(flwor)) {
			/* To be added process FLO string */
			transform_params.flwor = flwor;
			}

			if (!String.IsNullOrEmpty(xpath)) {
			xml_data =  getXPath(xml_data, xpath);
			transform_params.xpath = xpath;
			}
			
			if (transformId!=null & transformId!=Guid.Empty) {
			transform_params.transformId = transformId.ToString();
			}

			if (Id!=null & Id!=Guid.Empty) {
			transform_params.Id = Id.ToString();
			}
			
			if (projectId!=null & projectId!=Guid.Empty) {
			transform_params.projectId = projectId.ToString();
			}
			
			if (groupId!=null & groupId!=Guid.Empty) {
			transform_params.groupId = groupId.ToString();
			}

			/* Add current local parameters for passing to stylesheet */
			transform_params.host = getHostHref();
			transform_params.host_view = getHostHref() + constHref_transform;
			transform_params.host_file = getHostHref() + constHref_dataVIEW;
			transform_params.host_download = getHostHref() + constHref_dataDOWNLOAD;
			transform_params.host_esri = getHostHref() + constHref_esriFEATURE;
			transform_params.host_logger = getHostHref() + constHref_Logger;
			transform_params.host_gint = getHostHref() + constHref_gINT;
			transform_params.host_tablefile = getHostHref() + constHref_TableFile;
			transform_params.host_ags = getHostHref() + constHref_AGS;
			transform_params.version = version;
			// transform_params.sessionid = this.Session.Id; 
			var user = GetUserAsync();

			if (user!=null) {
				ge_user u = user.Result as ge_user;
				transform_params.user = u.FirstName + ' ' + u.LastName + '(' + u.Email + ')';
			}

			return transform_params;
		}

		public XDocument Transform(string xml, string xsl, XsltArgumentList argList)
			{
    		
			var originalXml = XDocument.Load(new StringReader(xml));
		    var transformedXml = new XDocument();
    		using (var xmlWriter = transformedXml.CreateWriter())
    		{
        		var xslt = new XslCompiledTransform();
        			xslt.Load(XmlReader.Create(new StringReader(xsl)));

        		xslt.Transform(originalXml.CreateReader(), argList, xmlWriter);
    		}

    		return transformedXml;
		}

		private string getXPath( string xml_data, string xpath ) 
		{

				MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml_data));

				XPathDocument docNav = new XPathDocument(ms);
				
				XPathNavigator nav = docNav.CreateNavigator();
				XPathNodeIterator NodeIter = nav.Select(xpath);

				StringBuilder sb = new StringBuilder();
				sb.Append (agsmlOpen); 

				while (NodeIter.MoveNext()) {
    			sb.Append(NodeIter.Current.OuterXml);  
  				};

				sb.Append (agsmlClose);		

				return sb.ToString();

		}
		private string getHoles( string xml_data, string[] holes) {
				StringBuilder sb = new StringBuilder();
				sb.Append (agsmlOpen);
				// Load the document and set the root element.  
				XmlDocument doc = new XmlDocument();  
				doc.LoadXml(xml_data);  
				XmlNode root = doc.DocumentElement;  
  
				// Add the namespace.  
				// XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);  
				// nsmgr.AddNamespace("agsml", "urn:agsml-schema");  
  
				// Select the hole nodes  
				XmlNodeList nodeList = null;

				foreach (string xpath in _ge_config.Value.xpath_hole) {
					nodeList = root.SelectNodes(xpath);
					if (nodeList.Count > 0) break;
				}
				
				if (nodeList.Count == 0) {
					//nodelist is empty return original xml data;
					return xml_data;
				}
				
				foreach (XmlNode nhole in nodeList)  
				{  
					XmlNode nHoleId = nhole.SelectSingleNode("HoleId");
					if (holes.Contains(nHoleId.InnerText)) {
						sb.Append(nhole.OuterXml);
					}

				}
				sb.Append (agsmlClose);		

				return sb.ToString();
		}

		private string getGeols(string xml_data, string[] geols) {
			try {	
				
				StringBuilder sb = new StringBuilder();
				sb.Append (agsmlOpen);
				
				// Load the document and set the root element.  
				XmlDocument doc = new XmlDocument();  
				doc.LoadXml(xml_data);  
				XmlNode root = doc.DocumentElement;  
  
				// Add the namespace.  
				// XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);  
				// nsmgr.AddNamespace("agsml", "urn:agsml-schema");  
  
				// Select all the Geol Nodes  
				// Select the hole nodes  

				XmlNodeList nodeList = null;

				foreach (string xpath in ge_config().xpath_geol) {
					nodeList = root.SelectNodes(xpath);
					if (nodeList != null) break;
				}
				
				if (nodeList==null) {
					return "";
				}
				
				foreach (XmlNode nGeol in nodeList)  
				{  
					XmlNode nGeolGeo = nGeol.SelectSingleNode("GeolGeo");
					if (geols.Contains(nGeolGeo.InnerText)) {
						sb.Append (nGeol.OuterXml);
					}

				}
				
				sb.Append (agsmlClose);
				return sb.ToString();

			} catch (Exception e) {
			 Console.WriteLine (e.Message );
			 return "";
			}
		}
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
///<Summary>
///https://forums.asp.net/t/2098192.aspx?How+to+call+a+stored+procedure+from+ASP+net+core
///<Summary>

		
   private bool get_borehole( Guid id, string holeid) {
		try {	
			
			SqlConnection con =  (SqlConnection) _context.Database.GetDbConnection();
			SqlCommand cmd = new SqlCommand("sp_getBorehole", con);
			
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@data", SqlDbType.Int).Value = id;
			cmd.Parameters.Add("@hole", SqlDbType.VarChar).Value = holeid;
			
			SqlParameter output_param = cmd.Parameters.Add("@outputXML", SqlDbType.Xml);
			output_param.Direction = ParameterDirection.Output;
			
			con.Open();
			
			cmd.ExecuteNonQuery();
			
			xml_data = Convert.ToString(output_param.Value);
			
			return true;	
		}
		catch (Exception e) {
			 Console.WriteLine (e.Message );
		//	addMessage (ex.ToString() );
			return false;
		}
    }
// https://www.nuget.org/packages/Saxon-HE/

public string XQuery (string xml, string query) {
			Processor processor = new Processor();

            XmlDocument input = new XmlDocument();
            input.LoadXml(xml);
            
			XdmNode indoc = processor.NewDocumentBuilder().Build(new XmlNodeReader(input));
			DomDestination qout = new DomDestination();
            
			XQueryCompiler compiler = processor.NewXQueryCompiler();
            XQueryExecutable exp = compiler.Compile(query);
            XQueryEvaluator eval = exp.Load();
            eval.ContextItem = indoc;
            eval.Run(qout);
            XmlDocument outdoc = qout.XmlDocument;

			using (var stringWriter = new StringWriter())
			using (var xmlTextWriter = XmlWriter.Create(stringWriter))
			{
    		outdoc.WriteTo(xmlTextWriter);
    		xmlTextWriter.Flush();
    		return stringWriter.GetStringBuilder().ToString();
			}

}

    

	// using (StreamWriter streamWriter = 
    //    new StreamWriter("books_xq.html", false, Encoding.UTF8))
	// 	{
    // // Load the input doc
    // XmlDocument xmlDoc = new XmlDocument();
    // xmlDoc.Load("books.xml");

    // // Create the output XmlWriter
    // XmlTextWriter xmlWriter = new XmlTextWriter(streamWriter);

    // // Do the transformation
    // XQueryCompiler xp = new Processor().NewXQueryCompiler();
    // xp.LoadFromFile("books-to-html.xq");
    // xp.RunQuery(xmlDoc, xmlWriter);   
	// return   

// public XDocument Query(string xml, string qry) {



// XQueryNavigatorCollection col = new XQueryNavigatorCollection();
//             col.AddNavigator("C:\\Users\\matea\\workspace\\test\\my-file.xml", "doc");

//             string query = "for $x in document(\"doc\")/dsKurs/KursZbir return (<p>{$x/Oznaka/text(), \" \", $x/Nomin/text(), \" \", $x/Sreden/text()}</p>)";

//             XQueryExpression xepr = new XQueryExpression(query);
//             string result = xepr.Execute(col).ToXml();
//             String html = "<html>" + result + "</html>";

//             using (FileStream fs = new FileStream("D:\\test.htm", FileMode.Create))
//             {
//                 using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
//                 {
//                     w.WriteLine(html);
//                 }
//             }

//             Console.Write(result);
//             Console.ReadKey();
// string query = "for $x in document('foo')//bar " +
//         "where $x/something = 4 " +
//         "return $x/somethingElse";

// XQuery expr = new XQueryExpression(query);

// string rawXML = (expr.Execute(col)).ToXml();


// }	

/*
public void test_stored_procedure () {
var connection = (SqlConnection) context.Database.AsSqlServer().Connection.DbConnection;

var command = connection.CreateCommand();
command.CommandType = CommandType.StoredProcedure;
command.CommandText = "MySproc";
command.Parameters.AddWithValue("@MyParameter", 42);

command.ExecuteNonQuery();

var userType = dbContext.Set().FromSql("dbo.SomeSproc @Id = {0}, @Name = {1}", 45, "Ada");
	

}


   	public async Task<IActionResult> OnGetAsync(Guid? id, string holeId, string view)
        {
		try {
            if (id == null)
            {
                return NotFound();
            }

            var ge_data = await _context.ge_data.
                .Include(g => g.owner)
                .Include(g => g.project).FirstOrDefaultAsync(m => m.Id == id);

            if (ge_data == null)
            {
                return NotFound();
            }
           ViewData["ownerId"] = new SelectList(_context.Set<ge_user>(), "Id", "Id");
           ViewData["projectId"] = new SelectList(_context.ge_project, "Id", "Id");


			addMessage("Parameters passsed (id=[" + id + "];holeid=[" + holeid + "];view=[" + view + "])");
		
			if (get_xslfilename(id, view)) {
				addMessage ("Using xslt data transform stylsheet: " + xslfile);
					if (get_borehole(id, holeId)) {
						addMessage ("xml_data returned: " + xml_data.Length + " char(s)");
							Xml1.DocumentContent = xml_data;
							Xml1.TransformSource = Server.MapPath(xslfolder + "/" + xslfile);
					} else {
						addMessage ("No xml data returned");
					}
				} else {
				addMessage ("No valid stylesheet was found for the selected view type and xml data set; check the version of the ags data and the type of xml data structure");
				}
		}
		catch (Exception ex) {
			addMessage (ex.ToString());
			return Page();
		}
	}

	private bool  get_xslfilename( int id, string stylename) {
			try {	
				
				SqlConnection con = ConnectionManager.GetGroundEngineeringDataConnection();
				SqlCommand cmd = new SqlCommand("sp_getStylesheetFileName", con);
				
				cmd.CommandType = CommandType.StoredProcedure;
				
				cmd.Parameters.Add("@data_id", SqlDbType.Int).Value = id;
				
				if (String.IsNullOrEmpty(stylename) != true) {
					cmd.Parameters.Add("@stylename", SqlDbType.VarChar).Value = stylename;
				}
				
				SqlParameter output_param = cmd.Parameters.Add("@filename", SqlDbType.VarChar, 64);
				output_param.Direction = ParameterDirection.Output;
				
				con.Open();
				
				cmd.ExecuteNonQuery();
				
				xslfile = Convert.ToString(output_param.Value);
				
				return true;
				
			}
			catch (Exception ex) {
				addMessage(ex.ToString());
				return false ;
			}
	}

	private bool get_borehole( Guid id, string holeid) {
		try {	
			
			SqlConnection con = ConnectionManager.GetGroundEngineeringDataConnection();
			SqlCommand cmd = new SqlCommand("sp_getBorehole", con);
			
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@data_id", SqlDbType.Int).Value = id;
			cmd.Parameters.Add("@hole_id", SqlDbType.VarChar).Value = holeid;
			
			SqlParameter output_param = cmd.Parameters.Add("@outputXML", SqlDbType.Xml);
			output_param.Direction = ParameterDirection.Output;
			
			con.Open();
			
			cmd.ExecuteNonQuery();
			
			xml_data = Convert.ToString(output_param.Value);
			
			return true;	
		}
		catch (Exception ex) {
			addMessage (ex.ToString() );
			return false;
		}
    }
	

		
	private void addMessage(string s1) {
			TextBox1.Text += s1 + "\r\n";
	}
	} */

/*		
				 if (task_xml_data_big == null && ge_transform.storedprocedure.Contains("@data") && ge_transform.storedprocedure.Contains("@hole") && ge_transform.storedprocedure.Contains("@table")) {
						
						 SqlParameter paramData = new SqlParameter("@data",data_all.ToDelimString(";")); 
						 SqlParameter paramHole = new SqlParameter("@hole",holes.ToDelimString(";")); 
						 SqlParameter paramTable = new SqlParameter("@table",tables.ToDelimString(";")); 
						// "dbo.SomeSproc @data = {0}, @hole = {1}, @table = {2}"
						task_xml_data_big = await _context.ge_data_file.FromSql (ge_transform.storedprocedure, paramData, paramHole, paramTable).SingleOrDefaultAsync();
				}
				if (task_xml_data_big == null && ge_transform.storedprocedure.Contains("@data") && ge_transform.storedprocedure.Contains("@hole")) {
						SqlParameter paramData = new SqlParameter("@data",data_all.ToDelimString(";")); 
						SqlParameter paramHole = new SqlParameter("@hole",holes.ToDelimString(";")); 
						// "dbo.SomeSproc @data = {0}, @hole = {1}"
						task_xml_data_big = await _context.ge_data_file.FromSql (ge_transform.storedprocedure,paramData, paramHole).SingleOrDefaultAsync();
				}
				if (task_xml_data_big == null && ge_transform.storedprocedure.Contains("@data") && ge_transform.storedprocedure.Contains("@table")) {
					 	SqlParameter paramData = new SqlParameter("@data",data_all.ToDelimString(";")); 
						SqlParameter paramTable = new SqlParameter("@table",tables.ToDelimString(";")); 
						// "dbo.SomeSproc @data = {0}, @table = {1}"
						task_xml_data_big = await _context.ge_data_file.FromSql (ge_transform.storedprocedure,paramData, paramTable).SingleOrDefaultAsync();
				}
				if (task_xml_data_big == null && ge_transform.storedprocedure.Contains("@data") && ge_transform.storedprocedure.Contains("@version")) {
					 	SqlParameter paramData = new SqlParameter("@data",data_all.ToDelimString(";")); 
						SqlParameter paramVersion = new SqlParameter("@version",tables.ToDelimString(";")); 
						// "select * from dbo.SomeSproc @data, @version"
						task_xml_data_big = await _context.ge_data_file.FromSql (ge_transform.storedprocedure,paramData, paramVersion).SingleOrDefaultAsync();
				}
				if (task_xml_data_big == null && ge_transform.storedprocedure.Contains("@data")) {
					    SqlParameter paramData = new SqlParameter("@data",data_all.ToDelimString(";")); 
						// "dbo.SomeSproc @data = {0}"
						task_xml_data_big =  await _context.ge_data_file.FromSql (ge_transform.storedprocedure,paramData).SingleAsync();
				}
				if (task_xml_data_big == null) {
						task_xml_data_big = await _context.ge_data_file.FromSql (ge_transform.storedprocedure).SingleOrDefaultAsync();
				}
 */

}

}

