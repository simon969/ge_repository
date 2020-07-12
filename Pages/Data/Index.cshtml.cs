using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ge_repository.Models;
using ge_repository.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ge_repository.Pages.Shared;
using ge_repository.Authorization;
using System.Linq;

namespace ge_repository.Pages.Data
{
    public class IndexModel : _geFullPagedTypedModel<ge_data>
    {
         public IndexModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager)
            : base(context, authorizationService, userManager, 10)
    {
    }
       

        [BindProperty]
        public String fileext {get;set;}
        public List<SelectListItem> filetypes {get;} = new List<SelectListItem> {
            new SelectListItem{Value = "All", Text = "All"},
            new SelectListItem{Value =".xls,.xlsx", Text = "Workbooks(xls,xlsx)"},
            new SelectListItem{Value =".gpj,.glb", Text = "Databases(gbj,glb,mdb,acdb)"},
            new SelectListItem{Value = ".js", Text = "Scripts(js,sql)"},
            new SelectListItem{Value =".jpg,.png", Text = "Images(jpg,png)"},
            new SelectListItem{Value =".ags,.txt", Text = "Text(ags,txt)"},
            new SelectListItem{Value =".pdf,.doc,.docx", Text = "Documents(pdf,doc,docx)"},
            new SelectListItem{Value =".xsl,.css", Text = "Stylesheets(xsl,css)"},
            new SelectListItem{Value =".xml,.csv", Text = "Data(xml,csv)"}
        };

        
        public async Task<IActionResult>  OnGetAsync(string pageFilter, string pageSort, int? pageIndex, int? pageSize, 
                                                    Guid? groupId, Guid? projectId, Constants.PublishStatus? pStatus, String fileext) 
            {
            base.setPaging(pageFilter, pageSort, pageIndex, pageSize, groupId, projectId,  pStatus);

            this.fileext = fileext;
            var UserId = GetUserIdAsync().Result;
            
            if (this.pageSort == null) {
                this.pageSort = "filename";
            }

            int pTotal;
            
            IQueryable<ge_data> ldata = null;

            if (pageFilter != null) {
            ldata = _context.getuserdata("Read", UserId, pageFilter);
            } else {
            ldata = _context.getuserdata("Read", UserId);
            } 
            
            if (fileext != null && fileext != "All"){
            List<String> fileextensions = new List<String> (fileext.Split(","));
            ldata  = ldata.WhereFileExtentionIn(fileextensions);
            }

            if (projectId != null) {
            ldata = ldata.Where(p=>p.projectId == projectId);
            } 

            if (groupId != null) {
            ldata = ldata.Where(p=>p.project.groupId == groupId);
            } 
            
            if (pStatus != null) {
            ldata = ldata.Where(d=>d.pstatus == pStatus);
            }
            
            items = await ldata
                            .PagedResult(   pageIndex,
                                            pageSize,
                                            d=>d.editedDT,
                                            false, 
                                            out pTotal)
                            .Include(d=>d.project)
                            .Include(d=>d.project.group)
                            .ToListAsync() ;
            
            pageTotal = pTotal;

            if (items == null) {
                return NotFound();
            }

            setViewData();
            return Page();
            
            }
            protected void setViewData(){
                    if (project != null) {
                    ViewData["ProjectName"] = project.name;
                    }
            }
                    
    }

}
