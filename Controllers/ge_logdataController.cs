using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
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
using ge_repository.OtherDatabase;
using Newtonsoft.Json;
using ge_repository.spatial;
using System.Xml.Serialization;


namespace ge_repository.Controllers
{

    public class ge_logdataController: ge_Controller  {     
   
        public ge_log_file ge_file {get;set;}

        public ge_logdataController(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager, 
            IHostingEnvironment env,
		 	IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
            
        }
    //Create
    [HttpPost]
    public async Task<IActionResult>  Post (string log_file, string origin_data, string format ) {
        
        ge_log_file f =  null;
        ge_data d =  null;
        ge_data_file b = new ge_data_file();

       if (format == "json") {     
            d = JsonConvert.DeserializeObject<ge_data>(origin_data);
            f = JsonConvert.DeserializeObject<ge_log_file>(log_file);
            d.filetype = "text/json";
            d.fileext = ".json";
            d.encoding = "utf-8";
            b.data_string  = log_file;
       }
       
       if (format=="xml") {
            d = (ge_data) origin_data.DeserializeFromXmlString<ge_data>();
            f = (ge_log_file) log_file.DeserializeFromXmlString<ge_log_file>();
            d.filetype = "text/xml";
            d.fileext = ".xml";
            d.encoding ="utf-8";
            b.data_xml  = log_file;
       }
        
        var user = await GetUserAsync();

        d.Id = f.Id;
        string filename = d.filename.Substring (0,d.filename.IndexOf(".")) + ' ' + f.channel + ".xml";
        d.filename = filename;
        d.filesize = log_file.Length;
        d.createdDT = DateTime.Now;
        d.editedDT = DateTime.Now;
        d.editedId = user.Id;
        d.createdId =  user.Id;

        string s1 = d.SerializeToXmlString<ge_data>();
        string s2 = b.SerializeToXmlString<ge_data_file>();

        var resp_post = await  new ge_dataController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env,
                                                _ge_config).Post(s1, s2, "xml"); 

        return resp_post;    
    }
    
    
    //Read
    [HttpGet]
    public async Task<IActionResult>  Get (Guid Id) {

        
        
        
        
        return Ok(ge_file);    
    }
    
    //Update
    [HttpPut]
    public async Task<IActionResult>  Put (Guid Id, string log_file, string origin_data, string format) {

        ge_log_file f =  null;
        ge_data d =  null;
        ge_data_file b = new ge_data_file();

       if (format == "json") {     
            d = JsonConvert.DeserializeObject<ge_data>(origin_data);
            f = JsonConvert.DeserializeObject<ge_log_file>(log_file);
            d.filetype = "text/json";
            d.fileext = ".json";
            d.encoding = "utf-8";
            b.data_string  = log_file;
       }
       
       if (format=="xml") {
            d = (ge_data) origin_data.DeserializeFromXmlString<ge_data>();
            f = (ge_log_file) log_file.DeserializeFromXmlString<ge_log_file>();
            d.filetype = "text/xml";
            d.fileext = ".xml";
            d.encoding ="utf-8";
            b.data_xml  = log_file;
       }
        
        var user = await GetUserAsync();

        d.Id = Id;
        string filename = d.filename.Substring (0,d.filename.IndexOf(".")) + ' ' + f.channel + ".xml";
        d.filename = filename;
        d.filesize = log_file.Length;
        d.createdDT = DateTime.Now;
        d.editedDT = DateTime.Now;
        d.editedId = user.Id;
        d.createdId =  user.Id;

        string s1 = d.SerializeToXmlString<ge_data>();
        string s2 = b.SerializeToXmlString<ge_data_file>();
        
        var resp_put = await  new ge_dataController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env,
                                                _ge_config).Put(Id, s1, s2, "xml"); 

        return resp_put;    

    }
    
    //Update
    [HttpPatch]
    public async Task<IActionResult>  Patch (string ge_file) {
        return new EmptyResult();    
    } 

    //Delete
    [HttpDelete]
    public async Task<IActionResult>  Delete (string ge_file) {
        return new EmptyResult();    
    }

    }


}
