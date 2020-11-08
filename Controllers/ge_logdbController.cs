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

    public class ge_logdbController: ge_Controller  {     
   
        public ge_log_file ge_file {get;set;}

        public ge_logdbController(
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
    public async Task<IActionResult>  Post (string s1, string format ) {
        
        ge_log_file log_file =  null;

       if (format == "json") {     
            log_file = JsonConvert.DeserializeObject<ge_log_file>(s1);
       }
       
       if (format=="xml") {
            log_file = (ge_log_file) DeserializeFromXmlString<ge_log_file>(s1);
       }



        return new EmptyResult();    
    }
    public static T DeserializeFromXmlString<T>( string xmlString)
        {
            var serializer = new XmlSerializer(typeof(T));
                using (TextReader reader = new StringReader(xmlString))
                {
                return (T) serializer.Deserialize(reader);
            }   
        }
    //Read
    [HttpGet]
    public async Task<IActionResult>  Get (Guid Id) {

        
        
        
        
        return Ok(ge_file);    
    }
    
    //Update
    [HttpPut]
    public async Task<IActionResult>  Put (string ge_file) {
        return new EmptyResult();    
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