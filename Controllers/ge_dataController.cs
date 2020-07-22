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
using ge_repository.Extensions;
using System.Xml.Serialization;

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
  public async Task<ge_data> Get (Guid id) {
    
        try {

            if (id == null)
            {
                return null;
            }

            var ge_data =  await _context.ge_data.SingleOrDefaultAsync(m => m.Id == id);
            
            return  ge_data;

       } catch (Exception e) {
            Console.WriteLine (e.Message );
              return null;
       }

    } 

    public async Task<List<ge_project>> GetProjects (Guid? groupId) {
                   
            if (groupId == null) {
                return null;
            }

            return await _context.ge_project
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
                       .Where (d=>d.project.groupId == groupId.Value).ToListAsync();
            }
            
            if (projectId != null) {
            return await _context.ge_data
                        .Where (m=>m.projectId == projectId.Value).ToListAsync();
            }

            if (Id != null) {
            return await _context.ge_data
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
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == id);
            if (_data == null)
            {
                return null;
            }

            var _data_big = await _context.ge_data_big.SingleOrDefaultAsync(m => m.Id == id);
            
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
    public void Post(ge_data value) {}
    [HttpPut]
    public void Put(int id, ge_data value) {}
    public async Task<IActionResult> Get(Guid id, string format="view")
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var _data = await _context.ge_data
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
            
            var _data_big = await _context.ge_data_big.SingleOrDefaultAsync(m => m.Id == id);
            
            if (_data_big == null)
            {
                return NotFound();
            }
           
         
            var ContentType = _data.filetype;
            var filename = _data.filename;
            var encode = _data.GetEncoding();
            Stream memory = _data_big.getMemoryStream(encode);
            
            /* if (ContentType == "text/xml") {
            
            } */
        //    HttpContext.Response.Headers.Add("Content-Disposition", "attachment; filename=FileName.pdf");
        //    HttpContext.Response.Headers.Add ("title", filename);
        //    HttpContext.Response.Headers.Add("Content-Disposition", $"inline; filename={filename}");
    
            if (format =="download") {
            return File (memory, ContentType, filename);
            }

            if (format == null || format=="view") {
            return File ( memory, ContentType);
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
public  async Task<string> getDataAsString (Guid Id) {
            
            var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == Id);

            var encode = _data.GetEncoding();

            var _data_big = await _context.ge_data_big.SingleOrDefaultAsync(m => m.Id == Id);
            
            if (_data_big == null)
            {
                return null;
            }
           
            MemoryStream memory = _data_big.getMemoryStream(encode);
      
            string s1 = Encoding.ASCII.GetString(memory.ToArray());
            
            return s1;

    }
 public  async Task<string[]> getDataByLines (Guid Id) {
            
            var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == Id);

            var encode = _data.GetEncoding();

            var _data_big = await _context.ge_data_big.SingleOrDefaultAsync(m => m.Id == Id);
            
            if (_data_big == null)
            {
                return null;
            }
           
            MemoryStream memory = _data_big.getMemoryStream(encode);
      
            string[] lines = Encoding.ASCII.GetString(memory.ToArray()).
                                Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            
            return lines;

    }
    public async Task<T> getDataAsClass<T> (Guid Id) {
  
        var _data = await _context.ge_data
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            if (_data == null)
                {
                return default(T);
            }

            Encoding encoding = _data.GetEncoding();

            var _data_big = await _context.ge_data_big.SingleOrDefaultAsync(m => m.Id == Id);
            
            if (_data_big == null)
            {
                return default(T);
            }

            try {

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T cs = (T) serializer.Deserialize(_data_big.getMemoryStream(encoding));
            
            return cs; 

            } catch (Exception e) {
                return default(T);
            }
    }
    }

}
