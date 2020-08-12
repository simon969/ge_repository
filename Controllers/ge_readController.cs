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
using ge_repository.OtherDatabase;
using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Extensions;
using System.Xml.Serialization;

namespace ge_repository.Controllers
{

    public class ge_readController: ge_Controller  {     
    
     public ge_readController(

            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager, 
            IHostingEnvironment env,
		 	IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
            
        }
  public async Task<IActionResult> PSDWorkbook(Guid Id,
                                             Guid templateId,
                                             string table,
                                             string sheet,
                                             string format = "view", 
                                             Boolean save = false) {
            var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            if (_data == null)
            {
                return NotFound();
            } 
            
            if (_data.fileext != ".xlsx") {
                return UnprocessableEntity();
            }

            var assert =  await new ge_accessController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).AssertOperations(_data, new string[] {"Download","Create"});

            var template = await new ge_dataController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getDataAsClass<ge_search>(templateId);

          
                    
            List<GRAG_WC> GRAG_WC;

            using (MemoryStream ms = await new ge_dataController(  _context,
                                        _authorizationService,
                                        _userManager,
                                        _env ,
                                        _ge_config).GetMemoryStream(Id)) {
                ge_log_workbook wb = new ge_log_workbook(ms);  
                SearchTerms st = new SearchTerms();
                ge_search template_loaded  =  st.findSearchTerms (template, table, wb, sheet);
                    if (template_loaded.search_tables.Count==0) {
                        return BadRequest(template_loaded);
                    }
                var psd_resp =  await ReadPSDData (template_loaded, wb, sheet);
                var psd_okResult = psd_resp as OkObjectResult;  
                    if (psd_okResult.StatusCode!=200) {
                        return Json(psd_resp);
                    }

                GRAG_WC = psd_okResult.Value as List<GRAG_WC>; 
            }
            
            string[] tables = new string[] {"GRAG","GRAT"};
            var ags_resp = await new ge_gINTController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getAGSTable(GRAG_WC, tables ,"4.04",false);
            var ags_okResult = ags_resp as OkObjectResult;  
                    if (ags_okResult.StatusCode!=200) {
                        return Json(ags_resp);
                    }
            
            string s1 = ags_okResult.Value as string;

            byte[] ags = Encoding.ASCII.GetBytes(s1);

            if (format =="download") {
            return File ( ags, "text/plain", _data.filename );
            }

            if (format == null || format=="view") {
            return File ( ags, "text/plain");
            }

         //   if (format=="view") {
         //   return View("GRAG_WC",GRAG_WC);
         //   }

            return Ok(GRAG_WC);
    }

    private GRAT newGRAT(GRAG gg) {
        GRAT gt = new GRAT {
                        PointID=gg.PointID,

                        };
        return gt;
    }
    private Double? getDouble(object obj, int? retError = null) {

        try {
        return Convert.ToDouble (obj);
        } catch {
            return retError;
        }
    }
    private async Task<IActionResult> ReadPSDData(ge_search template, ge_log_workbook wb, string sheet) {
               
                search_table st = template.search_tables[0];

                int psize_col = st.headers.Find(e=>e.id=="PARTICLESIZE").found;
                int descr_row = template.search_items.Find(e=>e.name=="soil_description").row;
                int cobble_row =  template.search_items.Find(e=>e.name=="cobbles").row;
                int gravel_row =  template.search_items.Find(e=>e.name=="gravel").row;
                int sand_row =  template.search_items.Find(e=>e.name=="sand").row;
                int silt_row =  template.search_items.Find(e=>e.name=="silt").row;
                int clay_row =  template.search_items.Find(e=>e.name=="clay").row;
                int line_start = template.data_start_row(-1);
                int line_end = template.data_end_row(40);

                List<GRAG_WC> GRAG_WC =  new List<GRAG_WC>();
                
                foreach (value_header vh in st.headers) {
                    
                    if (vh.id=="PARTICLESIZE") continue; 
                    
                    GRAG_WC gg = new GRAG_WC();
                    gg.PointID = vh.id;
                    gg.SPEC_DESC = Convert.ToString(wb.getValue(descr_row,vh.found));
                    gg.GRAG_VCRE = getDouble(wb.getValue(cobble_row, vh.found));
                    gg.GRAG_GRAV = getDouble(wb.getValue(gravel_row,vh.found));
                    gg.GRAG_SAND =  getDouble(wb.getValue(sand_row,vh.found));
                    gg.GRAG_SILT =  getDouble(wb.getValue(silt_row,vh.found));
                    gg.GRAG_CLAY =  getDouble(wb.getValue(clay_row,vh.found));
                    GRAG_WC.Add (gg);

                    for (int i=line_start; i<=line_end; i++) {   
                        string psize =  Convert.ToString(wb.getValue(i, psize_col));
                        if (psize=="") break;
                        string ppassing = Convert.ToString(wb.getValue(i,vh.found));
                        if (ppassing=="") break;
                        GRAT gt = newGRAT(gg);
                        gt.Reading = Convert.ToDouble(psize);
                        gt.GRAT_PERP = Convert.ToInt16(ppassing);
                        gg.GRAT.Add(gt);
                    }
                }

                return Ok(GRAG_WC);

    } 

     public async Task<IActionResult> ReadCSVFile(Guid Id,
                                             Guid dicId, 
                                             string table,
                                             string format = "view", 
                                             Boolean save = false ) {

            if (Id == null)
            {
                return NotFound();
            }
            
            var _data = await _context.ge_data
                                    .Include(d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            if (_data == null)
            {
                return NotFound();
            }
            ge_data empty_data = new ge_data();
            
            var user = GetUserAsync().Result;
            
            if (user != null) {
                    int IsDownloadAllowed = _context.IsOperationAllowed(Constants.DownloadOperationName, _data.project, _data);
                    Boolean CanUserDownload = _context.DoesUserHaveOperation(Constants.DownloadOperationName,_data.project,user.Id);
                    
                    int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, _data.project, empty_data);
                    Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName,_data.project,user.Id);

                    if (IsDownloadAllowed!=geOPSResp.Allowed) {
                        return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_PROHIBITED);
                    }
                    
                    if (!CanUserDownload) {
                    return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_USER_PROHIBITED);
                    }

                    if (IsCreateAllowed!=geOPSResp.Allowed) {
                        return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
                    }
                    if (!CanUserCreate) {
                    return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
                    }
            }
            
            var dic = await new ge_dataController(  _context,
                                                    _authorizationService,
                                                    _userManager,
                                                    _env ,
                                                    _ge_config).getDataAsClass<ge_search>(dicId);

                      
            var lines = await new ge_dataController( _context,
                                                _authorizationService,
                                                _userManager,
                                                _env ,
                                                _ge_config).getDataByLines(Id);

          
            
      return Ok();
 }

} 

}


        