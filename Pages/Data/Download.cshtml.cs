using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.Encodings;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using ge_repository.Authorization;
using ge_repository.Pages.Shared;

using ge_repository.Models;

namespace ge_repository.Pages.Data
{
    public class DownloadModel : _geBasePageModel
    {
        public DownloadModel(

            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager)
            : base(context, authorizationService, userManager)
        {
            
        }

        [BindProperty]
        public ge_data_big _data_big { get; set; }
        public ge_data _data { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
             _data = await _context.ge_data
                                    .Include (d =>d.project)
                                    .SingleOrDefaultAsync(m => m.Id == id);
            if (_data == null) {
                return NotFound();
            }

            var userId = GetUserIdAsync().Result;
            
            int IsDownloadAllowed = _context.IsOperationAllowed(Constants.DownloadOperationName, _data.project, _data);
            Boolean CanUserDownload = _context.DoesUserHaveOperation(Constants.DownloadOperationName,_data.project,userId);
            
             if (IsDownloadAllowed!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_PROHIBITED);
            }
            if (!CanUserDownload) {
               return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_USER_PROHIBITED);
            }

        
            _data_big = await _context.ge_data_big.SingleOrDefaultAsync(m => m.Id == id);
            
            if (_data_big == null) {
                return NotFound();
            }

            var filetype = _data.filetype;
            var filename = _data.filename;
            var encode = _data.GetEncoding();
            MemoryStream memory = _data_big.getMemoryStream(encode);

            return File(memory, filetype, filename);

            }
                       

/*         public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Data = await _context.ge_data.FindAsync(id);

            if (Data != null)
            {
                _context.ge_data.Remove(Data);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }


         public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path)); */
        }
}
