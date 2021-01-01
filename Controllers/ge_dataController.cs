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
using Microsoft.Net.Http.Headers;
using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Extensions;
using System.Xml.Serialization;
using System.Data.Common;
using Newtonsoft.Json;

namespace ge_repository.Controllers
{

    public class ge_dataController: ge_ControllerBase  {     
    
     public ge_dataController(

            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager, 
            IHostingEnvironment env,
		 	IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
            
        }
        
 [HttpGet("{id}")]
  public async Task<IActionResult> Get (Guid id) {
    
        try {

            if (id == null)
            {
                return UnprocessableEntity();
            }

            var ge_data =  await _context.ge_data
                                                .AsNoTracking()
                                                .SingleOrDefaultAsync(m => m.Id == id);
            
            return  Ok(ge_data);

       } catch (Exception e) {
            Console.WriteLine (e.Message );
              return NotFound();
       }

    } 

    public async Task<List<ge_project>> GetProjects (Guid? groupId) {
                   
            if (groupId == null) {
                return null;
            }

            return await _context.ge_project
                                        .AsNoTracking()
                                        .Where (p=>p.groupId == groupId).ToListAsync();
            
            
    }

[HttpGet]  
[Produces("application/xml")]  public async Task<List<ge_project>> xmlGetProjects(Guid? groupId) {
return await GetProjects (groupId);
}
[HttpGet]  
[Produces("application/xml")]  public async Task<List<ge_data>> xmlGetData (Guid? Id, Guid? projectId, Guid? groupId) {
return await Get (Id, projectId, groupId);
}
[HttpGet]  
[Produces("application/xml")]  public async Task<List<ge_data>> xmlGet (Guid? Id, Guid? projectId, Guid? groupId) {
return await Get (Id, projectId, groupId);
}

[HttpGet]
    public async Task<List<ge_data>> Get (Guid? Id, Guid? projectId, Guid? groupId) {
    
        try {
            
            // var url = this.HttpContext.Request.Query;
            
            if (groupId != null) {
            return await _context.ge_data
                                .AsNoTracking()
                                .Where (d=>d.project.groupId == groupId.Value).ToListAsync();
            }
            
            if (projectId != null) {
            return await _context.ge_data
                                .AsNoTracking()
                                .Where (m=>m.projectId == projectId.Value).ToListAsync();
            }

            if (Id != null) {
            return await _context.ge_data
                                .AsNoTracking()
                                .Where (m=>m.Id == Id.Value).ToListAsync();
            }

            return null;

       } catch (Exception e) {
           Console.WriteLine (e.Message );
              return null;
       }

    }
 public async Task<MemoryStream> GetMemoryStream(Guid id)
        {
            if (id == null)
            {
                return null;
            }
            
            var _data = await _context.ge_data
                                    .AsNoTracking()
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == id);
            if (_data == null)
            {
                return null;
            }

            var _data_big = await _context.ge_data_big
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync(m => m.Id == id);
            
            if (_data_big == null)
            {
                return null;
            }
           
         
            var ContentType = _data.filetype;
            var filename = _data.filename;
            var encode = _data.GetEncoding();
            MemoryStream memory = _data_big.getMemoryStream(encode);
            
            return memory;
    }

    [HttpPost]
    public async Task<IActionResult> Post(string data, string data_big, string format) {

            ge_data data1 =  null;
            ge_data_big data_big1 = null;

            if (format == "json") {   
                data1 = JsonConvert.DeserializeObject<ge_data>(data);
                data_big1 = JsonConvert.DeserializeObject<ge_data_big>(data_big);
            }
            
            if (format=="xml") {
                data1 = data.DeserializeFromXmlString<ge_data>();
                data_big1 = data_big.DeserializeFromXmlString<ge_data_big>();
            }
            
            data1.data = data_big1;
            _context.ge_data.Add(data1);
                        
            int DbCommandTimeout = Int32.Parse( _ge_config.Value.defaultEFDBTimeOut);
            
            if (DbCommandTimeout> 0 ) {
            _context.Database.SetCommandTimeout(DbCommandTimeout);     
            }
           
            int resp = await _context.SaveChangesAsync();
            
            if (resp == 1) {
                return Ok(data1);
            }
            
            return BadRequest(data1);

    }
    [HttpPut]
   public async Task<IActionResult> Put(Guid Id, string data, string data_big, string format) {

            ge_data data1 =  null;
            ge_data_big data_big1 = null;

            if (format == "json") {   
                data1 = JsonConvert.DeserializeObject<ge_data>(data);
                data_big1 = JsonConvert.DeserializeObject<ge_data_big>(data_big);
            }
            
            if (format=="xml") {
                data1 = data.DeserializeFromXmlString<ge_data>();
                data_big1 = data_big.DeserializeFromXmlString<ge_data_big>();
            }

            data1.Id=Id;
            data1.data = data_big1;
            
            _context.ge_data.Update(data1);
                        
            int DbCommandTimeout = Int32.Parse( _ge_config.Value.defaultEFDBTimeOut);
            
            if (DbCommandTimeout> 0 ) {
            _context.Database.SetCommandTimeout(DbCommandTimeout);     
            }
           
            int resp = await _context.SaveChangesAsync();
            
            if (resp == 1) {
                return Ok(data1);
            }
            
            return BadRequest(data1);

    }
    
    // https://dogschasingsquirrels.com/2020/06/02/streaming-a-response-in-net-core-webapi/
    //     [HttpGet]
    // [Route( "streaming" )]
    // public async Task GetStreaming() {
    //     const string filePath = @"C:\Users\mike\Downloads\dotnet-sdk-3.1.201-win-x64.exe";
    //     this.Response.StatusCode = 200;
    //     this.Response.Headers.Add( HeaderNames.ContentDisposition, $"attachment; filename=\"{Path.GetFileName( filePath )}\"" );
    //     this.Response.Headers.Add( HeaderNames.ContentType, "application/octet-stream"  );
    //     var inputStream = new FileStream( filePath, FileMode.Open, FileAccess.Read );
    //     var outputStream = this.Response.Body;
    //     const int bufferSize = 1 << 10;
    //     var buffer = new byte[bufferSize];
    //     while ( true ) {
    //         var bytesRead = await inputStream.ReadAsync( buffer, 0, bufferSize );
    //         if ( bytesRead == 0 ) break;
    //         await outputStream.WriteAsync( buffer, 0, bytesRead );
    //     }
    //     await outputStream.FlushAsync();
    // }
    // using (var dataReader = command.ExecuteReader())
    // {
    //     dataReader.Read();
    //     using (var stream = dataReader.GetStream("Text"))
    //     using (var streamReader = new StreamReader(stream))
    //     {
    //         // read the text using the StreamReader...
    //     }
    // }

    [HttpGet]
     public async Task GetStream(Guid id)
        {
        
            var _data = await _context.ge_data
                                        .Include(d =>d.project)
                                        .SingleOrDefaultAsync(m => m.Id == id);

            this.Response.StatusCode = 200;
            this.Response.Headers.Add( HeaderNames.ContentDisposition, $"attachment; filename=\"{Path.GetFileName( _data.filename )}\"" );
            this.Response.Headers.Add( HeaderNames.ContentType, _data.filetype  );    
            
            string data_field = _data.GetContentFieldName();
            Encoding encode  = _data.GetEncoding();
            using (var connection = _context.Database.GetDbConnection()) {
                DbCommand command = connection.CreateCommand();
                command.CommandText = $"SELECT {data_field} FROM ge_data where id='{id}'";
                command.CommandTimeout = 0;
                connection.Open();
                using (var dataReader = command.ExecuteReader()) {
                    dataReader.Read();
                    using (var inputStream = dataReader.GetCharStream(data_field,encode)) {
                        using (var outputStream = this.Response.Body) {;
                            const int bufferSize = 8192;
                            var buffer = new byte[bufferSize];
                            while ( true ) {
                                var bytesRead = await inputStream.ReadAsync( buffer, 0, bufferSize );
                                if ( bytesRead == 0 ) break;
                                await outputStream.WriteAsync( buffer, 0, bytesRead );
                            }
                            await outputStream.FlushAsync();
                        }
                    }
                }
            }

           
        }

    public async Task<IActionResult> Get(Guid id, string format="view")
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var _data = await _context.ge_data
                                    .AsNoTracking()
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == id);
            if (_data == null)
            {
                return NotFound();
            }

            var user = GetUserAsync().Result;
            
            int IsDownloadAllowed = _context.IsOperationAllowed(Constants.DownloadOperationName, _data.project, _data);
            Boolean CanUserDownload = _context.DoesUserHaveOperation(Constants.DownloadOperationName,_data.project, user.Id);
            
             if (IsDownloadAllowed!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_PROHIBITED);
            }
            if (!CanUserDownload) {
               return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_USER_PROHIBITED);
            }
            string content_type = _data.GetContentType();

            if (format =="download" && content_type.Contains ("text")) {
                    await GetStream(id);
                    return new EmptyResult();
            }

            var _data_big = await _context.ge_data_big
                                        .AsNoTracking()
                                        .SingleOrDefaultAsync(m => m.Id == id);
            
                if (_data_big == null)
                {
                return NotFound();
                }

                var encode = _data.GetEncoding();
                //have to convert utf-16 to utf8 for display in browser
                
               // if (encode==Encoding.Unicode) {
               //     encode = Encoding.UTF8;
               // }

                Stream memory = _data_big.getMemoryStream(encode);

            if (format == "download") {
                
                return File (memory, content_type, _data.filename);
            
            }
            
            if (format == null || format=="view") {
                
                return File ( memory, content_type);

            }

            // If we get down here soemthing is wrong 
             return NotFound();
            }
     public async Task<IActionResult> View(Guid id)
            {
            return await this.Get(id,"view");
            }
     public async Task<IActionResult> Download(Guid id)
            {
            return await this.Get(id,"download");
            }

    
   [HttpPost]

   public async Task<IActionResult> Post([FromForm] ge_fileUpload data) {
          
            if (data.files == null) {
                throw new ArgumentNullException(nameof(data.files));
            }
            if (data.projectId == null) {
                throw new ArgumentNullException(nameof(data.files));
            }
            
            if (_ge_config == null) {
                throw new ArgumentNullException(nameof(data.files));
            } 

            
            var ge_project = await _context.ge_project.FindAsync(data.projectId);

            if (ge_project == null) {
                return NotFound();
            } 
            
            long size = data.files.Sum(f => f.Length);
            long max_size = _ge_config.Value.defaultMaxFileSize;

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var uploadFile in data.files) {
                if (uploadFile.Length > 0) {

                    Boolean IsContentText = uploadFile.IsContentTypeText(true);
                    var b = new ge_data_big();
                    var d = new ge_data ();
    
                    if (IsContentText) { 
                        Boolean IsContentXML = uploadFile.IsContentTypeXML();
                        Encoding encoding = uploadFile.ReadEncoding (Encoding.Unicode);
                        d.SetEncoding(encoding);
                        if (IsContentXML) {   
                           Encoding encoding16 = new UnicodeEncoding();
                           d.SetEncoding(encoding16);  
                           b.data_xml = await uploadFile.ProcessFormFileString( ModelState, max_size,encoding,true);
                        } else {
                           b.data_string = await uploadFile.ProcessFormFileString( ModelState, max_size,encoding, false);
                        }
                    }  else {
                        b.data_binary = await uploadFile.ProcessFormFileBinary( ModelState, max_size);
                        d.SetEncoding(null);
                    }
                    // Perform a second check to catch ProcessFormFile method violations.
                    if (!ModelState.IsValid) {
                        return NotFound();
                    }
                    d.projectId = ge_project.Id;
                    d.locEast = ge_project.locEast;
                    d.locNorth = ge_project.locNorth;
                    // Add deatils of uploaded file to new _ge_data record
                    d.data = b;
                    d.filesize = uploadFile.Length; 
                    d.filename = uploadFile.FileName; 
                    d.createdDT = DateTime.UtcNow;
                    // ge_data.ownerId = User.
                    _context.ge_data.Add(d);
                    await _context.SaveChangesAsync();
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok();
        } 
public  async Task<string> getDataAsString (Guid Id, bool removeBOM = false) {
            
            var _data = await _context.ge_data
                                     .AsNoTracking()
                                     .Include(d =>d.project)
                                     .SingleOrDefaultAsync(m => m.Id == Id);

            var encode = _data.GetEncoding();

            var _data_big = await _context.ge_data_big
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            
            if (_data_big == null)
            {
                return null;
            }
            
            // MemoryStream memory = _data_big.getMemoryStream(encode);
            
            // string s1 = Encoding.ASCII.GetString(memory.ToArray());

            string s1 = _data_big.getString(encode,removeBOM);
            
            return s1;

    }
public  async Task<string> getDataAsParsedXmlString (Guid Id) {
            
            var _data = await _context.ge_data
                                     .AsNoTracking()
                                     .Include(d =>d.project)
                                     .SingleOrDefaultAsync(m => m.Id == Id);

            var encode = _data.GetEncoding();

            var _data_big = await _context.ge_data_big
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            
            if (_data_big == null)
            {
                return null;
            }
            
            string s1 = _data_big.getParsedXMLstring(encode);
            
            return s1;

    }

 public  async Task<string[]> getDataByLines (Guid Id) {
            
            var _data = await _context.ge_data
                                    .AsNoTracking()
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == Id);

            var encode = _data.GetEncoding();

            var _data_big = await _context.ge_data_big
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            
            if (_data_big == null)
            {
                return null;
            }
           
            MemoryStream memory = _data_big.getMemoryStream(encode);
      
            string[] lines = Encoding.ASCII.GetString(memory.ToArray()).
                                Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            
            return lines;

    }
    public async Task<T> getDataAsClass<T> (Guid Id, string format="xml") {
  
        var _data = await _context.ge_data
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            if (_data == null)
                {
                return default(T);
            }

            Encoding encoding = _data.GetEncoding();

            var _data_big = await _context.ge_data_big
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            
            if (_data_big == null)
            {
                return default(T);
            }
            bool removeBOM = true;

            string s =_data_big.getString(encoding,removeBOM);
            
            try {
                
                if (format == "xml") {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    using (TextReader reader = new StringReader(s))
                    {
                    T cs = (T) serializer.Deserialize(reader);
                    return cs; 
                    }
                }
                
                if (format == "json") {
                    T cs = JsonConvert.DeserializeObject<T>(s);
                    return cs;
                }
                
                return default(T);
            
            } catch (Exception e) {
                return default(T);
            }
    }
    }

}
