using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.spatial;

namespace ge_repository.Controllers
{
        public class ge_gisController: ge_ControllerBase  {     
          public ge_project project {get;set;}
        //   public IList<ge_data> data { get; set;}
          public string constHref = "/api/ge_data/";

        public ge_gisController(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
           IHostingEnvironment env,
		 	IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
        }

        public async Task<IActionResult> xmlGet(Guid? projectId) {
            
            if (projectId==null) {
                return BadRequest(); 
            }
            
            project = await _context.ge_project
                                    .Include (p=>p.data)
                                    .FirstAsync(p => p.Id == projectId);
            
            if (project==null) {
                return UnprocessableEntity("project is null"); 
            }

            string data_string = "";
           
            data_string = createXML(project);

            return Ok(data_string);

        }
        
        public async Task<IActionResult> CreateGIS(Guid projectId, string format)
        {
            
            if (projectId==null) {
                return NotFound(); 
            }
            
            if (format==null) {
                return NotFound(); 
            }
            
            project = await _context.ge_project
                                    .Include (p=>p.data)
                                    .FirstAsync(p => p.Id == projectId);
            
            if (project==null) {
                return NotFound(); 
            }

            var user = await GetUserAsync();
            
            if (user!=null) {
              return RedirectToPageMessage (msgCODE.USER_NOTFOUND);
            }
            
            var new_data = new ge_data();
            
            
            int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, project, new_data);
            Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName, project,user.Id);
            
            if (IsCreateAllowed!=geOPSResp.Allowed) {
               return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
            }
            
            if (!CanUserCreate) {
               return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
            }

            string data_string = "";
           

            switch (format.ToLower()) {
                    case "kml":
                    data_string = createKML(project);
                    break;
                    
                    case "shp":
                    data_string = createSHP();
                    break;
                    
                    case "xml":
                    data_string = createXML(project);
                    break;

                    default:
                    return RedirectToPageMessage (msgCODE.GIS_UNEXPECTEDFORMAT);
                   
            }

            if (String.IsNullOrEmpty(data_string)) {
                 return RedirectToPageMessage (msgCODE.GIS_CREATE_UNSUCCESSFULL);
            }
            string filename = project.name; 
            saveGISFile (projectId,filename,user.Id,data_string, "." + format.ToLower());

            return RedirectToPage("/Data/Index",new {projectId=projectId});
        }

        private string createKML(ge_project project) {
             
            ge_KML g = new ge_KML();
            g._href = getHostHref() + constHref; 

            g.createContainer (ge_KML.kmlContainer.DOCUMENT, project.name, project.description, project.Id.ToString());
            foreach (var d in project.data) {
                g.process_loc (d, d.filename, d.Id.ToString());
            }

            return g.ToString();    

        } 

        private string createXML (ge_group group) {
            
            ge_XML g = new ge_XML();
            g._href = getHostHref() + constHref; 
            XmlNode xmlgroup = g.createChild ("group", group.Id.ToString(),null);
            foreach (var p in group.projects) {
                XmlNode xmlproject = g.createChild ("project", p.Id.ToString(),null);
                g.write (p, xmlproject);
                foreach (var d in project.data) {
                    XmlNode xmldata = g.createChild ("data", d.Id.ToString(), xmlproject);
                    g.write (d, xmldata);
                }
            }

            return g.ToString();    
        }
        

        private string createXML (ge_project project) {
            
            ge_XML g = new ge_XML();
          //  g._href = getHostHref() + constHref; 

                XmlNode xmlproject = g.createChild ("project", project.Id.ToString(),null);
                g.write (project, xmlproject);
                foreach (var d in project.data) {
                XmlNode xmldata = g.createChild ("data", d.Id.ToString(), xmlproject);
                g.write (d, xmldata);
                }
            

            return g.ToString();    
        }
        private string createSHP() {
            return "";
        }
        private void saveGISFile(Guid projectId, string filename, string userId, string data_string, string ext){
            
            ge_data_file b = new ge_data_file();
            ge_data d = new ge_data();
            
            b.data_string = data_string;
            
            ge_MimeTypes types = new ge_MimeTypes(); 
            string stype;
            types.TryGetValue(ext,out stype);
            
            d.projectId = projectId;
            d.createdDT = DateTime.Now;
            d.createdId = userId;
            d.filename = filename + ext;
            d.filesize = data_string.Length;
            d.filetype = stype;
            d.filedate =  DateTime.UtcNow;
            d.fileext = ext;
            d.encoding ="ascii";
            d.operations = "Read;Download;Update;Delete";
            d.file = b;
            _context.ge_data.Add(d);
            _context.SaveChanges();
        //    status = enumStatus.XMLSaved;
                      
    }
    
    
    }
}




