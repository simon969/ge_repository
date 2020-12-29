using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.AspNetCore.Diagnostics.Tools.Applications.Runtime
using System.Linq;
using System.Linq.Expressions; 
using Newtonsoft.Json.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Html;

using System.Web.Http;

using System.Xml.Xsl;
using System.Xml;
using System.Xml.Serialization;

using ge_repository.Models;
using ge_repository.Authorization;

namespace  ge_repository.Extensions {

    public static class Extensions {

      
       public static Uri GetUri(this HttpRequest request) {

            var uriBuilder = new UriBuilder {
                        Scheme = request.Scheme,
                        Host = request.Host.Host,
                        Port = request.Host.Port.GetValueOrDefault(80),
                        Path = request.Path.ToString(),
                        Query = request.QueryString.ToString()
            };
            
    return uriBuilder.Uri;
    }
        public static Boolean IsContentTypePlain(this IFormFile formFile) {

           switch (formFile.ContentType.ToLower()) {
                        case "text/plain": return true;
                        default: return false;
        }

       }   
       
       public static Boolean IsContentTypeXML(this IFormFile formFile) {
           
           switch (formFile.ContentType.ToLower()) {
                        case "text/xml": return true;
                        case "text/htm": return true;
                        case "text/xsl": return true;
                        case "text/html": return true;
                        default: return false;
        }

       }
        public static Boolean IsContentTypeJavaScript(this IFormFile formFile) {
        return formFile.ContentType.ToLower().Contains("javascript");
       }
       public static Boolean IsContentTypeText(this IFormFile formFile, Boolean CheckJavaScriptExtention) {
            
            if (CheckJavaScriptExtention = true && formFile.IsFileExtensionJavascript()) {
                return true;
            }
            
            switch (formFile.FileExtension()) {
                case ".csv": return true;
                case ".txt": return true;
                case ".log": return true;
                case ".vbs": return true;
            }

            switch (formFile.ContentType.ToLower()) {
                         case "text/plain": return true;
                         case "text/xml": return true;
                         case "text/htm": return true;
                         case "text/xsl": return true;
                         case "text/html": return true;
                         case "text/css": return true;
                         case "text/javascript": return true;
                         case "application/javascript": return true;
                         default: return false;
            }

       }
       
        public static Boolean IsFileExtensionJavascript(this IFormFile formFile) {
           string ext = System.IO.Path.GetExtension(formFile.FileName);
           switch (ext.ToLower()) {
                        case ".js": return true;
                        default: return false;
            }
       }
        public static Boolean IsFileExtensionXML(this IFormFile formFile) {
        
        string ext = System.IO.Path.GetExtension(formFile.FileName);
           switch (ext.ToLower()) {
                        case ".xsl": return true;
                        case ".xml": return true;
                        case ".htm": return true;
                        case ".html": return true;
                        default: return false;
            }
       }
       public static int findFirstIndex(this string[] array, string where) {
           for (int i =0; i < array.Count(); i++) {
                if (array[i]==where) {
                   return i;
                }
           }
           return -1;
       }
       public static int findFirstIndexContains(this string[] array, string where) {
           for (int i =0; i < array.Count(); i++) {
                if (array[i].Contains(where)) {
                   return i;
                }
           }
           return -1;
       }
       public static bool IsFloat(this string s)
        {
        float output;
        return float.TryParse(s, out output);
    }
        public static float? ToFloatOrNull(this string s)
        {
            float output;
            if (float.TryParse(s, out output)){
                return output;
            }
             
            return null;
        }
       public static Boolean IsFileExtensionBinary(this IFormFile formFile) {
        
        string ext = System.IO.Path.GetExtension(formFile.FileName);
           switch (ext.ToLower()) {
                        case ".ags": return false;
                        case ".txt": return false;
                        case ".xsl": return false;
                        case ".xml": return false;
                        case ".htm": return false;
                        case ".html": return false;
                        case ".css":return false;
                        case ".js":return false;
                        default: return true;
            }
       }

        public static string FileExtension(this IFormFile formFile) {
               string ext = System.IO.Path.GetExtension(formFile.FileName);
               return ext.ToLower();
       }
       public static Encoding PeekEncoding(this IFormFile formFile, Encoding defaultEncodingIfNoBom) {
         
       using (var reader = new StreamReader(formFile.OpenReadStream(), defaultEncodingIfNoBom, true))
            {
                reader.Peek(); // you need this!
                return reader.CurrentEncoding;
            }
        }
       public static Encoding ReadEncoding(this IFormFile formFile, Encoding defaultEncoding)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var stream = formFile.OpenReadStream())
            {
                stream.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return defaultEncoding;
        }
/*         public static async Task<string> ProcessFormFileString(this IFormFile formFile, ModelStateDictionary modelState, long MaxFileSize, Encoding encode) { 
            return await ProcessFormFileString(formFile,modelState,MaxFileSize, encode, false);
        } */
        //  public static async Task<string> ProcessFormFileStringXML_ReplaceUTF8(this IFormFile formFile, ModelStateDictionary modelState, long MaxFileSize, Encoding encode) { 
        //     // https://stackoverflow.com/questions/41494856/sql-server-defining-an-xml-type-column-with-utf-8-encoding
        //      return await ProcessFormFileString(formFile,modelState,MaxFileSize, encode, true);
        // }
        public static async Task<string> ProcessFormFileString(this IFormFile formFile, ModelStateDictionary modelState, long MaxFileSize, Encoding encoding, Boolean removeXMLDeclaration)
        {
            var fieldDisplayName = string.Empty;
            
            // Use reflection to obtain the display name for the model 
            // property associated with this IFormFile. If a display
            // name isn't found, error messages simply won't show
            // a display name.
            
            MemberInfo property = 
                typeof(ge_fileUpload).GetProperty(formFile.Name.Substring(formFile.Name.IndexOf(".") + 1));

            if (property != null)
            {
                var displayAttribute = 
                    property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

                if (displayAttribute != null)

                if (displayAttribute != null)
                {
                    fieldDisplayName = $"{displayAttribute.Name} ";
                }
            }

            // Use Path.GetFileName to obtain the file name, which will
            // strip any path information passed as part of the
            // FileName property. HtmlEncode the result in case it must 
            // be returned in an error message.
            var fileName = WebUtility.HtmlEncode(Path.GetFileName(formFile.FileName));

            if (!formFile.IsContentTypeText(true))
            {
                modelState.AddModelError(formFile.Name, 
                                         $"The {fieldDisplayName}file ({fileName}) must be a text file.");
            }

            // Check the file length and don't bother attempting to
            // read it if the file contains no content. This check
            // doesn't catch files that only have a BOM as their
            // content, so a content length check is made later after 
            // reading the file's content to catch a file that only
            // contains a BOM.
            if (formFile.Length == 0)
            {
                modelState.AddModelError(formFile.Name, $"The {fieldDisplayName}file ({fileName}) is empty.");
            }
            else if (formFile.Length > MaxFileSize)
            {
                modelState.AddModelError(formFile.Name, $"The {fieldDisplayName}file ({fileName}) exceeds {MaxFileSize/1e6} MB.");
            }
            else
            {
                try
                {
                    string fileContents;
                    // The StreamReader is created to read files that are UTF-8 encoded. 
                    // If uploads require some other encoding, provide the encoding in the 
                    // using statement. To change to 32-bit encoding, change 
                    // new UTF8Encoding(...) to new UTF32Encoding().
                   
                    using (
                        var reader = 
                            new StreamReader(
                                formFile.OpenReadStream(), 
                                encoding, 
                                detectEncodingFromByteOrderMarks: true))
                    {
                        fileContents = await reader.ReadToEndAsync();
                        
                        // Check the content length in case the file's only
                        // content was a BOM and the content is actually
                        // empty after removing the BOM.
                        if (fileContents.Length > 0)
                        {
                            if (removeXMLDeclaration==true) {
                                // Encoding newEncoding = new UnicodeEncoding();
                                // string fileContents2 = fileContents.SetEncoding(newEncoding);
                                string fileContents2 =  fileContents.RemoveXmlDeclaration();
                                return fileContents2;
                            }
                                                       
                            return fileContents;
                        }
                        else
                        {
                            modelState.AddModelError(formFile.Name, 
                                                     $"The {fieldDisplayName}file ({fileName}) is empty.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    modelState.AddModelError(formFile.Name, 
                                             $"The {fieldDisplayName}file ({fileName}) upload failed. " +
                                             $"Please contact the Help Desk for support. Error: {ex.Message}");
                    // Log the exception
                }
            }

            return string.Empty;
        }
        public static async Task<byte[]> ProcessFormFileBinary(this IFormFile formFile, ModelStateDictionary modelState, long MaxFileSize)
            {
            var fieldDisplayName = string.Empty;

            // Use reflection to obtain the display name for the model 
            // property associated with this IFormFile. If a display
            // name isn't found, error messages simply won't show
            // a display name.

            MemberInfo property = 
                typeof(ge_fileUpload).GetProperty(formFile.Name.Substring(formFile.Name.IndexOf(".") + 1));

            if (property != null)
            {
                var displayAttribute = 
                    property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

                if (displayAttribute != null)

                if (displayAttribute != null)
                {
                    fieldDisplayName = $"{displayAttribute.Name} ";
                }
            }

            // Use Path.GetFileName to obtain the file name, which will
            // strip any path information passed as part of the
            // FileName property. HtmlEncode the result in case it must 
            // be returned in an error message.
            var fileName = WebUtility.HtmlEncode(Path.GetFileName(formFile.FileName));

            // Check the file length and don't bother attempting to
            // read it if the file contains no content. This check
            // doesn't catch files that only have a BOM as their
            // content, so a content length check is made later after 
            // reading the file's content to catch a file that only
            // contains a BOM.
            
            if (formFile.Length == 0)
            {
                modelState.AddModelError(formFile.Name, $"The {fieldDisplayName}file ({fileName}) is empty.");
            }
            else if (formFile.Length > MaxFileSize)
            {
                modelState.AddModelError(formFile.Name, $"The {fieldDisplayName}file ({fileName}) exceeds max file size ({MaxFileSize/1e6} Mb)");
            }
            else
            {
                try
                {
                        byte[] buffer = new byte[16*1024]; 
                        MemoryStream ms = new MemoryStream();
                        BinaryReader reader = new BinaryReader(formFile.OpenReadStream());
                           int read;
                        while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                            {
                            ms.Write(buffer, 0, read);
                        }
                        
                        Byte[] fileContents = ms.ToArray();

                        // Check the content length in case the file's only
                        // content was a BOM and the content is actually
                        // empty after removing the BOM.

                        if (fileContents.Length > 0)
                        {
                            return fileContents;
                        }
                        else
                        {
                            modelState.AddModelError(formFile.Name, 
                                                     $"The {fieldDisplayName}file ({fileName}) is empty.");
                        }
                    
                }
                catch (Exception ex)
                {
                    modelState.AddModelError(formFile.Name, 
                                             $"The {fieldDisplayName}file ({fileName}) upload failed. " +
                                             $"Please contact the Help Desk for support. Error: {ex.Message}");
                    // Log the exception
                }
            }
        return null;
        }
       
/// <summary>
/// Pages the specified query.
// https://www.codeproject.com/Tips/758999/Dynamic-paging-in-Entity-Framework
/// </summary>
/// <typeparam name="T">Generic Type Object</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
/// <param name="query">The Object query where paging needs to be applied.</param>
/// <param name="pageNum">The page number.</param>
/// <param name="pageSize">Size of the page.</param>
/// <param name="orderByProperty">The order by property.</param>
/// <param name="isAscendingOrder">if set to <c>true</c> [is ascending order].</param>
/// <param name="rowsCount">The total rows count.</param>
/// <returns></returns>

public static IQueryable<T> PagedResult<T, TResult>(this IQueryable<T> query, int? pageNum, int? pageSize,
                Expression<Func<T, TResult>> orderByProperty, bool isAscendingOrder, out int pageTotal)
{
    int pNum = 1;
    int pSize = 20;
    int rowsCount = 0;

    if (pageNum != null){
        pNum = pageNum.Value;
    } 

    if (pageSize != null) {
        pSize = pageSize.Value;
    }
    
    if (pSize <= 0) {
        pSize = 20;
    }

    //Total result count
    rowsCount = query.Count();
    pageTotal = (int) ((rowsCount / pSize) + 1);

    //If page number should be > 0 else set to first page
    if (rowsCount <= pSize || pNum <= 0) pNum = 1;
    

    //Calculate nunber of rows to skip on pagesize
    int excludedRows = (pNum - 1) * pSize;

    query = isAscendingOrder ? query.OrderBy(orderByProperty) : query.OrderByDescending(orderByProperty);
    
    //Skip the required rows for the current page and take the next records of pagesize count
    return query.Skip(excludedRows).Take(pSize);
}
public static IQueryable<ge_data> WhereFileExtentionIn(this IQueryable<ge_data> query, List<String> fileextensions)
{
    IQueryable<ge_data> datas = from data in query
      where fileextensions.Contains(data.fileext)
      select data;

    return datas;

}

    public static string nextVersion(this ge_DbContext context, ge_data data) {
        
        if (data.project==null) {
            return null;
        }

        string proj_verex = data.project.verex;

        return null;
    }

    public static IQueryable<ge_project> projectFull(this ge_DbContext context) {

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.ge_project
                .Include(p => p.users)
                .Include(p =>p.data)
                .Include(p => p.group);

        }

    public static IQueryable<ge_project> projectFull(this ge_DbContext context, Guid Id) {

           if (context == null || Id == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            return context.ge_project
             .Include(p => p.users)
             .Include(p => p.group)
             .Include("group.users")
             .Include(p =>p.data)
             .Where(p => p.Id == Id);
        }
    // public static IQueryable<ge_project> getuserprojects (this ge_DbContext _context, string operation, string userId, string search) {

    // var user_projects = _context.ge_project
    //                                 .Where (p=>p.name.Contains(search) || p.description.Contains(search) || p.keywords.Contains(search))
    //                                 .Where (p=>p.users.Any(u=>u.userId == userId && u.operations.Contains(operation)));

    // var group_project = _context.ge_project
    //                                 .Where (p=>p.name.Contains(search) || p.description.Contains(search) || p.keywords.Contains(search))
    //                                 .Where (p=>p.group.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation)));
    
    // return user_projects.Union(group_project);
    
    // }
    public static IQueryable<ge_project> getuserprojects (this ge_DbContext _context, string operation, string userId, string search) {

    var user_projects = _context.ge_project
                                    .Include (p=>p.group)
                                    .Where (p=>p.name.Contains(search) || p.description.Contains(search) || p.keywords.Contains(search))
                                    .Where (p=>p.users.Any(u=>u.userId == userId && u.operations.Contains(operation)) || p.group.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation)));
    return user_projects;
    
    }
    public static IQueryable<ge_project> getuserprojects (this ge_DbContext _context, string operation, string userId) {

    var user_projects = _context.ge_project
                                    .Include (p=>p.group)
                                    .Where (p=>p.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation)) || p.group.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation)));
    return user_projects;
    }
    // public static IQueryable<ge_project> getuserprojects (this ge_DbContext _context, string operation, string userId) {

    // var user_projects = _context.ge_project
    //                                 .Include (p=>p.group)
    //                                 .Where (p=>p.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation)));

    // var group_project = _context.ge_project
    //                                 .Include (p=>p.group)
    //                                 .Where (p=>p.group.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation)));
    
    // return user_projects.Union(group_project);
    // }
    
    public static IQueryable<ge_data> getuserdata (this ge_DbContext _context, string operation, string userId) {

    var project_data = _context.ge_data
                                    .Include(d=>d.project)
                                    .Where (d=>d.project.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation)));

    var group_data = _context.ge_data
                                    .Include(d=>d.project)
                                    .Include(d=>d.project.group)
                                    .Where (d=>d.project.group.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation)));
    
       return project_data.Union(group_data).Include(d=>d.project)
                                    .Include(d=>d.project.group);;
    }
    //public static IQueryable<ge_data> getuserdata (this ge_DbContext _context, string operation, string userId, string search) {
    //    return getuserdata (_context, operation, userId)
    //                        .Where (d=>d.filename.Contains(search) || d.description.Contains(search) || d.keywords.Contains(search));
    
    //}
    public static IQueryable<ge_data> getuserdata (this ge_DbContext _context, string operation, string userId, string search) {

    var project_data = _context.ge_data
                                    .Where (d=>d.filename.Contains(search) || d.description.Contains(search) || d.keywords.Contains(search))
                                    .Include(d=>d.project)
                                    .Where (d=>d.project.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation)));

    var group_data = _context.ge_data
                                    .Where (d=>d.filename.Contains(search) || d.description.Contains(search) || d.keywords.Contains(search))
                                    .Include(d=>d.project)
                                    .Include(d=>d.project.group)
                                    .Where (d=>d.project.group.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation)));
    
    return project_data.Union(group_data).Include(d=>d.project)
                                    .Include(d=>d.project.group);
    }
    
 //   public static IQueryable<ge_transform> getusertransform(this ge_DbContext _context, string operation, string userId, string search) {

  //  var project_transform = _context.ge_transform
  //                                  .Where (d=>d.name.Contains(search) || d.description.Contains(search)) 
  //                                  .Include(d=>d.project)
  //                                  .Where (d=>d.project.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation)));

//    var group_transform = _context.ge_transform
//                                    .Where (d=>d.name.Contains(search) || d.description.Contains(search))
//                                    .Include(d=>d.project)
//                                    .Include(d=>d.project.group)
//                                    .Where (d=>d.project.group.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation)));
    
//       return project_transform.Union(group_transform);
//    }
 public static IQueryable<ge_transform> getusertransform (this ge_DbContext _context, string operation, string userId) {

    var t = _context.ge_transform
                                    .Include(d=>d.project)
                                    .Include(d=>d.project.group)
                                    .Include(d=>d.data)
                                    .Include(d=>d.style)
                                    .Where (d=>d.project.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation)) ||
                                            d.project.group.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation))
                                    );
     
       return t;
    }

    // public static IQueryable<ge_transform> getusertransform (this ge_DbContext _context, string operation, string userId) {

    // var project_transform = _context.ge_transform
    //                                 .Include(d=>d.project)
    //                                 .Where (d=>d.project.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation)));

    // var group_transform = _context.ge_transform
    //                                 .Include(d=>d.project)
    //                                 .Include(d=>d.project.group)
    //                                 .Where (d=>d.project.group.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation)));
    
    //    return project_transform.Union(group_transform);
    //}
    public static IQueryable<ge_transform> getusertransform (this ge_DbContext _context, string operation, string userId, string search) {
    return getusertransform ( _context, operation, userId)
                            .Where(d=>d.name.Contains(search) || d.description.Contains(search));
    }

    public static IQueryable<ge_project> projectSearchByUserOperation(this ge_DbContext _context, string userId, string operation, string search) { 
      
      // https://arnhem.luminis.eu/linq-and-entity-framework-some-dos-and-donts/
      
        IQueryable<ge_project> project;
        
        var user_proj = _context.ge_user_ops
                        .Where(p => p.userId == userId && p.projectId != null && p.user_operations.Contains(operation));
        
        if (!string.IsNullOrEmpty(search)) {
            project = from item in _context.ge_project join t in user_proj on item.Id equals t.projectId 
                     where item.name.Contains(search) || item.description.Contains(search) || item.keywords.Contains(search) 
                      select item;
        } else {
            project = from item in _context.ge_project join t in user_proj on item.Id equals t.projectId 
                       select item;
        }

       return project
                    .Include (p=>p.group);
            
                                       
    }
  
    public static string getQueryParam(this string s1, string paramName,string returnvalIfAbsent) {
         
         string ret = returnvalIfAbsent;

         if (s1.Contains(paramName)) {
            string[] paramList = s1.Split(";");
                for (int i=0;  i<paramList.Count(); i++) {
                    if (paramList[i].Contains(paramName)) {
                        string[] paramValue = paramList[i].Split("=");
                        ret = paramValue[1];
                        break;
                    }
                }

         } 

        return ret;
    }
    public static IQueryable<ge_data> dataSearch(this ge_DbContext _context, string search) { 
      
      // https://arnhem.luminis.eu/linq-and-entity-framework-some-dos-and-donts/
      
        if (!string.IsNullOrEmpty(search)) {
        var data = from item in _context.ge_data  
                     where item.description.Contains(search) || item.keywords.Contains(search) || item.filename.Contains(search) || item.filetype.Contains(search)
                      select item;
        return data;
       } else {
        var data2 = from item in _context.ge_data  
                       select item;
        return data2;
       }
    }
     public static IQueryable<ge_data> dataSearchByProjectUserOperation(this ge_DbContext _context, string userId, string operation, string search) { 
      
      // https://arnhem.luminis.eu/linq-and-entity-framework-some-dos-and-donts/
      
        var user_proj = _context.ge_user_ops
                        .Where(p => p.userId == userId && p.projectId != null && p.user_operations.Contains(operation));

        if (!string.IsNullOrEmpty(search)) {
        var data = from item in _context.ge_data join t in user_proj on item.projectId equals t.projectId 
                     where item.description.Contains(search) || item.keywords.Contains(search) || item.filename.Contains(search) || item.filetype.Contains(search)
                      select item;
        return data;
       } else {
        var data2 = from item in _context.ge_data.Include(d=>d.project) join t in user_proj on item.projectId equals t.projectId 
                       select item;
        return data2;
       }
    }

                       
    public static IQueryable<ge_data> dataSearchBygroupUserOperation(this ge_DbContext _context, string userId, string operation, string search) { 
      
      // https://arnhem.luminis.eu/linq-and-entity-framework-some-dos-and-donts/
      
        var user_group = _context.ge_user_ops
                            .Where(p => p.userId == userId && p.groupId != null && p.user_operations.Contains(operation));
        
        var project = from item in _context.ge_project join t in user_group on item.groupId equals t.groupId  
                         select item;
        
        if (!string.IsNullOrEmpty(search)) {
        var data = from item in _context.ge_data join t in project on item.projectId equals t.Id 
                     where item.description.Contains(search) || item.keywords.Contains(search)
                        || t.name.Contains(search) || t.description.Contains(search) || t.keywords.Contains(search)
                      select item;
        return data;
       } else {
        var data2 = from item in _context.ge_data join t in project on item.projectId equals t.Id 
                       select item;
        return data2;
       }
    }

    public static IQueryable<ge_project> projectSearch(this ge_DbContext _context, string search) { 
      
      // https://arnhem.luminis.eu/linq-and-entity-framework-some-dos-and-donts/
      
        if (!string.IsNullOrEmpty(search)) {
        var project = from item in _context.ge_project  
                      where item.name.Contains(search) || item.description.Contains(search) || item.keywords.Contains(search) 
                      select item;
        return project;
       } else {
        var project2 = from item in _context.ge_project 
                       select item;
        return project2;
       }
            
                                       
    }
    public static IQueryable<ge_project> projectSearchBygrouptUserOperation(this ge_DbContext _context, string userId, string operation, string search) { 
      
      // https://arnhem.luminis.eu/linq-and-entity-framework-some-dos-and-donts/
      
        var user_group = _context.ge_user_ops
                        .Where(p => p.userId == userId && p.projectId != null && p.user_operations.Contains(operation));

        if (!string.IsNullOrEmpty(search)) {
        var project = from item in _context.ge_project join t in user_group on item.groupId equals t.groupId 
                      where item.name.Contains(search) || item.description.Contains(search) || item.keywords.Contains(search) 
                      select item;
        return project;
       } else {
        var project2 = from item in _context.ge_project join t in user_group on item.groupId equals t.groupId 
                       select item;
        return project2;
       }
            
                                       
    }
    public static IQueryable<IGrouping<System.Guid, ge_project>> projectSearchByUserOperationGrouped(this ge_DbContext _context, string userId, string operation, string search) { 
      
      // https://arnhem.luminis.eu/linq-and-entity-framework-some-dos-and-donts/
      
        var user_group = _context.ge_user_ops
                        .Where(g => g.userId == userId && g.groupId != null && g.user_operations.Contains(operation));

        var user_project = _context.ge_user_ops
                        .Where(p => p.userId == userId && p.projectId != null && p.user_operations.Contains(operation));

        if (!string.IsNullOrEmpty(search)) {
        var project = (from item in _context.ge_project 
                        join o in user_group on  item.groupId equals o.groupId 
                        join p in user_project on item.Id equals p.projectId   
                        where item.name.Contains(search) || item.description.Contains(search) || item.keywords.Contains(search) 
                        select item).GroupBy (g=>g.Id);
        return project;
       } else {
        var project2 = (from item in _context.ge_project 
                        join o in user_group on item.groupId equals o.groupId 
                        join p in user_project on item.Id equals p.projectId  
                        select item).GroupBy (g=>g.Id);
        return project2;
       }
            
                                       
    }

     public static IQueryable<ge_project> SearchByUserOperation(this ge_DbContext _context, string userId, string operation, string search) { 
      
      // https://arnhem.luminis.eu/linq-and-entity-framework-some-dos-and-donts/
      
        var user_group = _context.ge_user_ops
                        .Where(g => g.userId == userId && g.groupId != null && g.user_operations.Contains(operation));

        var user_project = _context.ge_user_ops
                        .Where(p => p.userId == userId && p.projectId != null && p.user_operations.Contains(operation));

        if (!string.IsNullOrEmpty(search)) {
        var project = from item in _context.ge_project 
                        join p in user_project on item.Id equals p.projectId 
                        join o in user_group on item.groupId equals o.groupId
                        where item.name.Contains(search) || item.description.Contains(search) || item.keywords.Contains(search) 
                      
                        select item;
        return project;
       } else {
        var project2 = from item in _context.ge_project 
                        join p in user_project on item.Id equals p.projectId  
                        join o in user_group on item.groupId equals o.groupId 
                        select item;
        return project2;
       }
            
                                       
    }
    public static IQueryable<ge_group> GroupSearchByUserOperation(this ge_DbContext _context, string userId, string operation, string search) { 
      
      // https://arnhem.luminis.eu/linq-and-entity-framework-some-dos-and-donts/
        
        var user_group = _context.ge_user_ops
                            .Where(g => g.userId == userId && g.groupId != null && g.user_operations.Contains(operation));
        
        if (!string.IsNullOrEmpty(search)) {
        var group = from item in _context.ge_group join t in user_group on item.Id equals t.groupId 
                     where item.locAddress.Contains(search) || item.locName.Contains(search) 
                      select item;
        return group;
       } else {
        var group2 = from item in _context.ge_group join t in user_group on item.Id equals t.groupId 
                       select item;
        return group2;
       }
    }
     public static IQueryable<ge_group> GroupSearchByOperation(this ge_DbContext _context, string userId, string operation, string search) { 
      
        var group = _context.ge_group
                            .Include (g=>g.users)
                            .Where (g=>g.project_operations.Contains(operation))
                            .Where (g=>g.users.Any(u=>u.userId == userId && u.user_operations.Contains(operation)));

        if (search!="") {
            group.Where(g=>g.locAddress.Contains(search) || g.locName.Contains(search));
        }

        return group;
    }
   public static IQueryable<ge_user_ops> GroupUserSearch(this ge_DbContext _context, string userId, string search, string operation, Guid? groupId) { 
      
      // https://arnhem.luminis.eu/linq-and-entity-framework-some-dos-and-donts/
      
            if (_context == null) {
                return null;
            }
            
            IQueryable<ge_user_ops> user_group;
            
            user_group = _context.ge_user_ops
                            .Include(u=>u.user)
                            .Include(u=>u.group)
                            .Where (u=>u.groupId != null);

            if (!string.IsNullOrEmpty(userId)) {
                user_group = user_group
                            .Where (u=>u.userId == userId);

            }
            
            if (!string.IsNullOrEmpty(search)) {
                user_group = user_group
                            .Where(u => u.user.FirstName.Contains(search) || u.user.LastName.Contains(search));
            }
            
            if (!string.IsNullOrEmpty(operation)) {
                user_group = user_group
                            .Where (u=>u.user_operations.Contains(operation));
            }

            if (groupId!=null) {
                user_group = user_group
                            .Where (u=>u.groupId==groupId.Value);
            }

            return user_group;
    }

 public static IQueryable<ge_user_ops> UserOperationsSearch(this ge_DbContext _context, string userId, string search, string operation, Guid? groupId, Guid? projectId) { 
      
      // https://arnhem.luminis.eu/linq-and-entity-framework-some-dos-and-donts/
            
            if (_context == null) {
                return null;
            }
            
            IQueryable<ge_user_ops> user_ops;
            
            user_ops = _context.ge_user_ops
                            .Include(u=>u.user)
                            .Include(u=>u.project)
                            .Include(u=>u.group)
                            .Include(u=>u.project.group);
                              
            if (!string.IsNullOrEmpty(userId)) {
                user_ops = user_ops
                            .Where (u=>u.userId == userId);

            }

            if (!string.IsNullOrEmpty(search)) {
                user_ops = user_ops
                            .Where(u => u.user.FirstName.Contains(search) || u.user.LastName.Contains(search) 
                            || u.user_operations.Contains(search) || u.project.name.Contains(search) || u.group.name.Contains(search));
            }
            
            if (!string.IsNullOrEmpty(operation)) {
                user_ops = user_ops
                            .Where (u=>u.user_operations.Contains(operation));
            }


            if (groupId!=null) {
                user_ops = user_ops
                            .Where (u=>u.project.groupId==groupId.Value || u.groupId==groupId.Value);
            }

            if (projectId!=null) {
                user_ops = user_ops
                            .Where (u=>u.projectId==projectId.Value);
            }
                       
            return user_ops;
        
    }

       public static IQueryable<ge_user_ops> ProjectUserSearch(this ge_DbContext _context, string userId, string search, string operation, Guid? groupId, Guid? projectId) { 
      
      // https://arnhem.luminis.eu/linq-and-entity-framework-some-dos-and-donts/
            
            if (_context == null) {
                return null;
            }
            
            IQueryable<ge_user_ops> user_project;
            
            user_project = _context.ge_user_ops
                            .Include(u=>u.user)
                            .Include(u=>u.project)
                            .Include(u=>u.project.group)
                            .Where (u=>u.projectId != null);
      
            if (!string.IsNullOrEmpty(userId)) {
                user_project = user_project
                            .Where (u=>u.userId == userId);

            }

            if (!string.IsNullOrEmpty(search)) {
                user_project = user_project
                            .Where(u => u.user.FirstName.Contains(search) || u.user.LastName.Contains(search));
            }
            
            if (!string.IsNullOrEmpty(operation)) {
                user_project = user_project
                            .Where (u=>u.user_operations.Contains(operation));
            }


            if (groupId!=null) {
                user_project = user_project
                            .Where (u=>u.project.groupId==groupId.Value);
            }

            if (projectId!=null) {
                user_project = user_project
                            .Where (u=>u.projectId==projectId.Value);
            }
                       
            return user_project;
        
    }
   
  
public static SelectList getGroupsWhereUserAdmin(this ge_DbContext context, string userId) {
    
    IQueryable<ge_group> groups = null;
        
    if (userId != null) {
        groups = context.ge_group
                            .Where(o=>o.users.Any(ou=>ou.userId == userId && ou.user_operations.Contains(Constants.AdminOperationName)));
    } 
     
    List<SelectListItem> list= new List<SelectListItem>();

    foreach (var o in groups) {
            SelectListItem item =  new SelectListItem(o.name + " " + o.locName + " "  + o.locAddress + " ", o.Id.ToString());
            list.Add (item);
    }
   
    return new SelectList(list,"Value","Text", null);

}
public static SelectList getProjectsWhereUserAdmin(this ge_DbContext context, string userId) {
    
    IQueryable<ge_project> projects = null;
        
    if (userId != null) {
        projects = context.ge_project
                            .Where(o=>o.users.Any(pu=>pu.userId == userId && pu.user_operations.Contains(Constants.AdminOperationName)) ||
                                o.group.users.Any(gu=>gu.userId == userId && gu.user_operations.Contains(Constants.AdminOperationName)));
    } 
     
    List<SelectListItem> list= new List<SelectListItem>();

    foreach (var o in projects) {
            SelectListItem item =  new SelectListItem(o.name + " " + o.locName + " "  + o.locAddress + " ", o.Id.ToString());
            list.Add (item);
    }
   
    return new SelectList(list,"Value","Text", null);

}


public static SelectList getGroupUsers(this ge_DbContext context, Guid? groupId) {
        
    IQueryable<ge_user_ops> ou = null;

    ou = context.ge_user_ops
                            .Include(u=>u.user)
                            .Where(u=>u.groupId != null);

    if (groupId != null) {
        ou.Where(u=>u.groupId == groupId);
    } 
    
    List<SelectListItem> list= new List<SelectListItem>();

    foreach (var u in ou) {
            SelectListItem item =  new SelectListItem(u.user.LastName + ","  + u.user.FirstName, u.user.Id);
            list.Add (item);
    }
   
    return new SelectList(list,"Value","Text", null);

}
public static SelectList getUserGroups(this ge_DbContext context, string userId) {
    
    IQueryable<ge_group> groups = null;
        
    if (userId != null) {
        groups = context.ge_group
                            .Where(o=>o.users.Any(ou=>ou.userId == userId));
    } 
     
    List<SelectListItem> list= new List<SelectListItem>();

    foreach (var o in groups) {
            SelectListItem item =  new SelectListItem(o.name + " " + o.locName + " "  + o.locAddress + " ", o.Id.ToString());
            list.Add (item);
    }
   
    return new SelectList(list,"Value","Text", null);

}
public static SelectList getProjectUsers(this ge_DbContext context, Guid? projectId) {
    
    IQueryable<ge_user_ops> pu = null;
    
    if (projectId == null ) {
          pu = context.ge_user_ops
                                .Include(u=>u.user)
                                .Where(u=>u.projectId != null);
    } else {
           pu = context.ge_user_ops
                                .Include(u=>u.user)
                                .Where(u=>u.projectId == projectId);

    }

    List<SelectListItem> list= new List<SelectListItem>();

    foreach (var u in pu) {
            SelectListItem item =  new SelectListItem(u.user.LastName + ","  + u.user.FirstName, u.user.Id);
            list.Add (item);
    }
   
   return new Microsoft.AspNetCore.Mvc.Rendering.SelectList(list,"Value","Text", null);

}
public static SelectList getProjectData(this ge_DbContext context, Guid[] projectId, string FileType) {

  var data = context.ge_data
                                        .Where(d=>projectId.Contains(d.projectId))
                                        .Where(d=>d.fileext == FileType);
   
   List<SelectListItem> list= new List<SelectListItem>();

    foreach (var d in data) {
            SelectListItem item =  new SelectListItem(d.filename + "," + d.description ,d.Id.ToString());
            list.Add (item);
    }
   
   return new Microsoft.AspNetCore.Mvc.Rendering.SelectList(list,"Value","Text", null);                                
}


public static SelectList getProjectTransforms(this ge_DbContext context, Guid? projectId) {
    
    IQueryable<ge_transform> pt = null;
    
    if (projectId == null ) {
        pt = context.ge_transform
                                .Include(t=>t.project)
                                .Include(t=>t.style)
                                .Include(t=>t.data)
                                .Where(t=>t.projectId != null);
    } else {
        pt = context.ge_transform
                                .Include(t=>t.project)
                                .Include(t=>t.style)
                                .Include(t=>t.data)
                                .Where(t=>t.projectId == projectId);

    }

    List<SelectListItem> list= new List<SelectListItem>();

    foreach (var t in pt) {
            SelectListItem item =  new SelectListItem(t.name + "," + t.description + "(" + t.style.filename + ")" ,t.Id.ToString());
            list.Add (item);
    }
   
   return new Microsoft.AspNetCore.Mvc.Rendering.SelectList(list,"Value","Text", null);

}

public static IQueryable<ge_user> UserSearch(this ge_DbContext context, string search) {
    
    if (String.IsNullOrEmpty(search)) {
    return null;
    }
    
    IQueryable<ge_user> users = context.ge_user
                                .Where(u=>u.FirstName.Contains(search) || u.LastName.Contains(search) || u.Email.Contains(search));
    
    return users;
}

public static SelectList getUsers(this ge_DbContext context, string search) {

    if (String.IsNullOrEmpty(search)) {
    return null;
    }
    
    IQueryable<ge_user> users = context.ge_user
                                .Where(u=>u.FirstName.Contains(search) || u.LastName.Contains(search) || u.Email.Contains(search));

    List<SelectListItem> list= new List<SelectListItem>();

    foreach (var u in users) {
            SelectListItem item =  new SelectListItem(u.FullName() + "(" + u.Email + ")" ,u.Id);
            list.Add (item);
    }
   
   return new Microsoft.AspNetCore.Mvc.Rendering.SelectList(list,"Value","Text", null);

}
public static SelectList getGroupTransforms(this ge_DbContext context, Guid? groupId) {
    
    IQueryable<ge_transform> gt = null;
    
    if (groupId == null ) {
        gt = context.ge_transform
                                .Include(t=>t.project)
                                .Include(t=>t.style)
                                .Include(t=>t.data)
                                .Where(t=>t.projectId != null);
    } else {
        gt = context.ge_transform
                                .Include(t=>t.project)
                                .Include(t=>t.style)
                                .Include(t=>t.data)
                                .Where(t=>t.project.groupId == groupId);

    }

    List<SelectListItem> list= new List<SelectListItem>();

    foreach (var t in gt) {
            SelectListItem item =  new SelectListItem(t.style.filename  + "," +  t.name + "," + t.description + "(" + t.project.name + ")" ,t.Id.ToString());
            list.Add (item);
    }
   
   return new Microsoft.AspNetCore.Mvc.Rendering.SelectList(list,"Value","Text", null);

}
public static SelectList Append(this SelectList list, SelectList appendList) {
    SelectList new_list = list;
    if (appendList.Any()) {
        foreach (SelectListItem sli in appendList) {
            new_list.Append(sli);
    }
    }
    return new_list;
}
/*   // https://www.codeproject.com/Articles/1056011/Bind-Enum-to-DropdownList-in-ASP-NET-MVC
  
public static string AttributeValue<TEnum,TAttribute>(this TEnum value,Func<TAttribute,string> func) 
    where T : Attribute
{
   FieldInfo field = value.GetType().GetField(value.ToString());

   T attribute = Attribute.GetCustomAttribute(field, typeof(T)) as T;

   return attribute == null ? value.ToString() : func(attribute);

}
 public static Microsoft.AspNetCore.Mvc.Rendering.SelectList ToSelectList<TEnum,TAttribute>
(this TEnum obj,Func<TAttribute,string> func,object selectedValue=null)
  where TEnum : struct, IComparable, IFormattable, IConvertible
  where TAttribute : Attribute
    {
        
        return new SelectList(Enum.GetValues(typeof(TEnum)).OfType<Enum>() 
             .Select(x => 
                 new SelectListItem 
                 { 
                    Text = x.AttributeValue<TEnum,TAttribute>(func), 
                    Value = (Convert.ToInt32(x)).ToString() 
                 }), 
             "Value", 
             "Text",
              selectedValue);
    } */                                      
    



/// <summary>  
    /// Applies an XSL transformation to an XML document  
     // https://www.c-sharpcorner.com/article/display-xml-data-as-html-using-xslt-in-asp-net-mvc/ 
    /// </summary>  
    /// <param name="helper"></param>  
    /// <param name="xml"></param>  
    /// <param name="xsltPath"></param> 
    /// <returns></returns>  
    public static HtmlString RenderXml(this IHtmlHelper helper, string xml, string xslt, string parameters = "")  
    {  
        if (String.IsNullOrEmpty(xml) || String.IsNullOrEmpty(xslt)) {
            HtmlString htmlStringNoData = new HtmlString ("Empty XML or XSLT");  
            return htmlStringNoData;  
        }

        XsltArgumentList args = new XsltArgumentList();  
        string namespaceUri = "";
        if (!String.IsNullOrEmpty(parameters)) {
            var resource = JObject.Parse(parameters);
            foreach (var res in resource) {
            string sKey = res.Key;
            JToken value = res.Value;
            string sValue = value.ToString(",");
            args.AddParam(sKey, namespaceUri, sValue);
            }
			/* Dictionary<string, string> arguments = resource.ToObject<Dictionary<string, string>>();
				foreach (var arg in arguments) {
                    string val = arg.Value;
                    if (String.IsNullOrEmpty(val)) {
                        val = "";   
                    }
 					args.AddParam(arg.Key, namespaceUri, val);
				} */
		  }
        // Create the XsltSettings object with script enabled.
        XsltSettings xsltsettings = new XsltSettings(false,true);

        //check for BOM at begining of xslt string from file
        // string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        // if (xslt.StartsWith(_byteOrderMarkUtf8))
        // {
        // xslt = xslt.Remove(0, _byteOrderMarkUtf8.Length);
        // }

        // Create XslCompiledTransform object to loads and compile XSLT string. 
        XslCompiledTransform tranformObj = new XslCompiledTransform();  
        tranformObj.Load(new XmlTextReader(new StringReader(xslt)),xsltsettings, new XmlUrlResolver());  
        
        // Create XMLReaderSetting object to assign DtdProcessing, Validation type  
        XmlReaderSettings xmlSettings = new XmlReaderSettings();  
        xmlSettings.DtdProcessing = DtdProcessing.Parse;  
        xmlSettings.ValidationType = ValidationType.DTD;  
        
        // Create XMLReader object to Transform xml value with XSLT setting   
        using (XmlReader reader = XmlReader.Create(new StringReader(xml), xmlSettings))  
        {  
            StringWriter writer = new StringWriter();  
            tranformObj.Transform(reader, args, writer); 
           
            // Generate HTML string from StringWriter  
            HtmlString htmlString = new HtmlString(writer.ToString());  
            return htmlString;  
        } 
        
    } 
    public static string EpochToDate(string s)
     {
         try {
		 long epoch = Convert.ToInt64(s);
         DateTime dt =  new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch/1000);
         return dt.ToString("MM/dd/yyyy hh:mm:ss.fff");
		 } catch {
		 return s;
		 }
     }
    public static string Replace (this string rawSQL, string data, string hole, string table, string user, string version) {

                string rdata = rawSQL.Replace( "@data", data);
				string rhole = rdata.Replace( "@hole", hole);
				string rtable = rhole.Replace( "@table", table);
                string ruser = rtable.Replace("@user", user);
				string rawSQL2 = ruser.Replace( "@version", version);
                
                return rawSQL2;
    }
    public static int findColumn(this string[] line, string s1) {
    for (int i =0; i<line.Length; i++) {
        if (line[i]==s1) return i;
    }
    return -1;
    }

    public static string[] QuoteSplit (this string s1) {
            string s2 = s1.Substring(1, s1.Length-2);
            return s2.Split("\",\"");
    }
    public static string ToCSV (this int [] array) {
    
        StringBuilder sb =  new StringBuilder();
        
        for (int i=0;i<array.Count();i++) {
            string s = Convert.ToString(array[i]);
            if (i>0) sb.Append(",");
            sb.Append (s);
        }
    
        return sb.ToString();
    }
    public static string ToDelimString(this string[] array, string delimeter,string encapsulate="") {
            
            StringBuilder sb = new StringBuilder();
 
            for (int i = 0; i < array.Length; i++) {
                String s1 = array[i];
                s1 = encapsulate + s1 + encapsulate;
                 if (!String.IsNullOrEmpty(s1)) { 
                    if (sb.Length>0) {
                    sb.Append (delimeter);
                    }
                    sb.Append (s1);
                }
            }
            if (sb.Length == 0 && encapsulate == null) {
                return "null";
            } 
                       
            if (sb.Length == 0 && encapsulate.Length > 0) {
                return encapsulate + encapsulate;
            }
            
            return sb.ToString();

         }
     public static T[] sub_array<T> (this T[] array, int offset, int length) {
            T[] result = new T[length];
            Array.Copy(array,offset,result, 0,length);
            return result;

        }    
    public static string ToDelimString(this Guid[] array, string delimeter,string encapsulate="") {
            
            StringBuilder sb = new StringBuilder();
 
            for (int i = 0; i < array.Length; i++) {
                Guid g1 = array[i];
                String s1 = encapsulate + g1.ToString() + encapsulate;
                 if (!String.IsNullOrEmpty(s1)) { 
                    if (sb.Length>0) {
                    sb.Append (delimeter);
                    }
                    sb.Append (s1);
                }
            }
            if (sb.Length == 0 && encapsulate == null) {
                return "null";
            } 
                       
            if (sb.Length == 0 && encapsulate.Length > 0) {
                return encapsulate + encapsulate;
            }
            
            return sb.ToString();

         }
            public static string ToDelimString<T>(this T[] array, string delimeter,string encapsulate="") {
            
            StringBuilder sb = new StringBuilder();
 
            for (int i = 0; i < array.Length; i++) {
                T g1 = array[i];
                String s1 = encapsulate + g1.ToString() + encapsulate;
                 if (!String.IsNullOrEmpty(s1)) { 
                    if (sb.Length>0) {
                    sb.Append (delimeter);
                    }
                    sb.Append (s1);
                }
            }
            if (sb.Length == 0 && encapsulate == null) {
                return "null";
            } 
                       
            if (sb.Length == 0 && encapsulate.Length > 0) {
                return encapsulate + encapsulate;
            }
            
            return sb.ToString();

         } 
    public static string[] purgeArray(this string[] array, string[] delimeters) {
        List<string> retvar = new List<string>();

        foreach (string s1 in array) {
            if (!String.IsNullOrEmpty(s1)) {
                bool split = false;
                foreach (string delim in delimeters) {
                    if (s1.Contains(delim)) {
                        split = true;
                        string[] array2 = s1.Split(delim);
                            foreach (string s2 in array2) {
                            retvar.Add(s2);
                            }
                    }
                }
                if (split == false) {
                retvar.Add(s1);  
                }
            }
        }
        
        if (retvar.Any()) {
            return retvar.ToArray();
        } else {
            return null;
        }
    }
        
        
    public static string ToString(this JToken value, string delimeter){
        string retvar = "";
 
        if (!value.HasValues) {
            return value.ToString();
        }
 
        foreach (var res in (JArray) value)
            {
            if (retvar.Length>0) {
                retvar=retvar + delimeter;
            }
            retvar = retvar + res.ToString();            
        }
        return retvar;
    }
    public static string AGSVersion (this ge_data data) {
        
        if (data.fileext==AGS.FileExtension.XML) {
            if (data.data != null) {
                string xml_data = data.data.data_xml;
                return AGS.ge_AGS.getVersion(xml_data);
            }
        }

        return "";
    }
    
    public static string ToString (this double Num,int SagnificantDigits)
    {
        var Format = $"{0}{new string('#', SagnificantDigits-1)}";
        var NumAbs = 0 <= Num ? Num : -Num;
        var Power  = 10d;

        for (int p=1; p<= SagnificantDigits; p++) 
            if(NumAbs < Power) return Num.ToString(Format.Insert(p, ".")); 
            else Power *= 10;

        return Num.ToString("0.").Substring(0, SagnificantDigits);
    }
    public static T DeserializeFromXmlString<T>(this string xmlString)
        {
            var serializer = new XmlSerializer(typeof(T));
                using (TextReader reader = new StringReader(xmlString))
                {
                return (T) serializer.Deserialize(reader);
            }   
        }
     public static string SerializeToXmlString<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using(StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
    public static string SetEncoding(this String xmlString, Encoding newEncoding) {
    try {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlString);

        // Make sure that the XML does not already have an encoding.
        if (xmlDocument.FirstChild.NodeType == XmlNodeType.XmlDeclaration) {
            // If it does, set the top node to the correct encoding
            ((XmlDeclaration)xmlDocument.FirstChild).Encoding = newEncoding.HeaderName;
        } else {
            // If not, we need to create an encoding node and set it to be the first part of the XML
            XmlDeclaration declarationNode = (XmlDeclaration)xmlDocument.CreateNode(XmlNodeType.XmlDeclaration, "xml", string.Empty);
            declarationNode.Encoding = newEncoding.HeaderName;
            xmlDocument.InsertBefore(declarationNode, xmlDocument.FirstChild);
        }
            return xmlDocument.InnerXml;
    } catch (Exception e) {
        // Problem encounterened parsing Xml string return original string
        return xmlString;
    }  
    }

    public static string ToSafeSubString(this string value, int count)
    {
         return value != null && value.Length > count ?
                                                   value.Substring(0, count) : value;
    }
    
    public static string RemoveXmlDeclaration(this String xmlString) {
    
        XmlDocument xmlDocument = new XmlDocument();

        xmlDocument.LoadXml(xmlString);

        if (xmlDocument.FirstChild.NodeType == XmlNodeType.XmlDeclaration) {
            xmlDocument.RemoveChild(xmlDocument.FirstChild);
            return xmlDocument.InnerXml;
        }
            // XmlDeclaration Node not present just return original string
         return xmlString;

       
    }

    public static ge_data FindFirstWhereKeywordsContains(this List<ge_data> items, string[] values) {
    
    List<ge_data> a = items;
    
    foreach (string v in values) {
         List<ge_data> b =  a.FindAll(m=>m.keywords.Contains(v));
         a = b ;
    }
    
    return a.First();
}
    public static string XmlSerializeToString(this object objectInstance)
{
    var serializer = new XmlSerializer(objectInstance.GetType());
    var sb = new StringBuilder();

    using (TextWriter writer = new StringWriter(sb))
    {
        serializer.Serialize(writer, objectInstance);
    }

    return sb.ToString();
}

public static T XmlDeserializeFromString<T>(this string objectData)
{
    return (T)XmlDeserializeFromString(objectData, typeof(T));
}

public static object XmlDeserializeFromString(this string objectData, Type type)
{
    var serializer = new XmlSerializer(type);
    object result;

    using (TextReader reader = new StringReader(objectData))
    {
        result = serializer.Deserialize(reader);
    }

    return result;
}
}


 /* 

/// <summary>
/// Return Partial View.
/// The element naming convention is maintained in the partial view by setting the prefix name from the expression.
/// The name of the view (by default) is the class name of the Property or a UIHint("partial name").
/// @Html.PartialFor(m => m.Address)  - partial view name is the class name of the Address property.
/// </summary>
/// <param name="expression">Model expression for the prefix name (m => m.Address)</param>
/// <returns>Partial View as Mvc string</returns>
public static MvcHtmlString PartialFor<tmodel, tproperty>(this HtmlHelper<tmodel> html,
    Expression<func<TModel, TProperty>> expression)
{
    return html.PartialFor(expression, null);
}

/// <summary>
/// Return Partial View.
/// The element naming convention is maintained in the partial view by setting the prefix name from the expression.
/// </summary>
/// <param name="partialName">Partial View Name</param>
/// <param name="expression">Model expression for the prefix name (m => m.Group[2])</param>
/// <returns>Partial View as Mvc string</returns>
public static MvcHtmlString PartialFor<TModel, TProperty>(this HtmlHelper<TModel> html,
    Expression<Func<TModel, TProperty>> expression,
    string partialName
    )
{
    string name = ExpressionHelper.GetExpressionText(expression);
    string modelName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
    ModelMetadata metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
    object model = metaData.Model;


    if (partialName == null)
    {
        partialName = metaData.TemplateHint == null
            ? typeof(TProperty).Name    // Class name
            : metaData.TemplateHint;    // UIHint("template name")
    }

    // Use a ViewData copy with a new TemplateInfo with the prefix set
    ViewDataDictionary viewData = new ViewDataDictionary(html.ViewData)
    {
        TemplateInfo = new TemplateInfo { HtmlFieldPrefix = modelName }
    };

    // Call standard MVC Partial
    return html.Partial(partialName, model, viewData);
}

    }
    
 */

}
public interface IOrderer<TItem>
{
    IOrderedQueryable<TItem> Apply(IQueryable<TItem> source);
}

public class OrderBy<TItem, TType> : IOrderer<TItem>
{
    private Expression<Func<TItem, TType>> _orderExpr;
    public OrderBy(Expression<Func<TItem, TType>> orderExpr)
    {
        _orderExpr = orderExpr;
    }

    public IOrderedQueryable<TItem> Apply(IQueryable<TItem> source)
    {
        return source.OrderBy(_orderExpr);
    }
}   

public class ThenBy<TItem, TType> : IOrderer<TItem>
{
    private Expression<Func<TItem, TType>> _orderExpr;
    public ThenBy(Expression<Func<TItem, TType>> orderExpr)
    {
        _orderExpr = orderExpr;
    }

    public IOrderedQueryable<TItem> Apply(IQueryable<TItem> source)
    {
        return ((IOrderedQueryable<TItem>)source).ThenBy(_orderExpr);
    }
}   

public class OrderCoordinator<TItem>
{
    public List<IOrderer<TItem>> Orders { get; private set; } = new List<IOrderer<TItem>>();

    public IQueryable<TItem> ApplyOrder(IQueryable<TItem> source)
    {
        foreach (IOrderer<TItem> orderer in Orders)
        {
            source = orderer.Apply(source);
        }
        return source;
    }
    public int FindMaxValue<T>(List<T> list, Converter<T, int> projection)
    {
    if (list.Count == 0)
    {
        throw new InvalidOperationException("Empty list");
    }
    int maxValue = int.MinValue;
    foreach (T item in list)
    {
        int value = projection(item);
        if (value > maxValue)
        {
            maxValue = value;
        }
    }
    return maxValue;
    }
    public int FindMinValue<T>(List<T> list, Converter<T, int> projection)
    {
    if (list.Count == 0)
    {
        throw new InvalidOperationException("Empty list");
    }
    int minValue = int.MaxValue;
    foreach (T item in list)
    {
        int value = projection(item);
        if (value < minValue)
        {
            minValue = value;
        }
    }
    return minValue;
    }
    public OrderCoordinator<TItem> OrderBy<TValueType>(Expression<Func<TItem, TValueType>> orderByExpression)
    {
        Orders.Add(new OrderBy<TItem, TValueType>(orderByExpression));
        return this;
    }

    // Can add more sort calls over time
    public OrderCoordinator<TItem> ThenBy<TValueType>(Expression<Func<TItem, TValueType>> orderByExpression)
    {
        Orders.Add(new ThenBy<TItem, TValueType>(orderByExpression));
        return this;
    }
}   
    public class ge_MimeTypes : Dictionary<string, string> 
        {

            public  ge_MimeTypes()
             {
               // Loads more at
               // https://stackoverflow.com/questions/4212861/what-is-a-correct-mime-type-for-docx-pptx-etc
               
                // Documents'
                Add (".doc", "application/msword");
                Add (".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                Add (".pdf", "application/pdf");
                Add (".dwg","application/autocad");
                Add (".dgn","application/microstation");
                Add (".dxf","application/dxf");
                Add (".gpj","application/gINT");
                Add (".glb","application/gINT");
                
                //'Slideshows'
                Add (".ppt", "application/vnd.ms-powerpoint");
                Add (".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
                
                //'Data'
                Add (".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                Add (".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template");
                Add (".xls", "application/vnd.ms-excel");
                Add (".xlt", "application/vnd.ms-excel");
                Add (".xla", "application/vnd.ms-excel");
              

                Add (".csv", "text/plain");
                Add (".xml", "text/xml");
                Add (".xsl", "text/xsl");
                Add (".txt", "text/plain");
                Add (".ags", "text/plain");
                Add (".rtf","text/richtext");
                Add (".kml", "application/vnd.google-earth.kml+xml");
                Add (".json", "text/json");
                
                Add (".htm", "text/html");
                Add (".html", "text/html");
                Add (".css", "text/css");
                Add (".js", "application/javascript");
                Add (".xq", "application/xquery");
                Add (".xqy", "application/xquery");

                Add (".mdb", "application/vnd.ms-access");
                Add (".accdb", "application/vnd.ms-access");
                
                //'Compressed Folders'
                Add (".zip", "application/zip");

                //'Audio'
                Add (".ogg", "application/ogg");
                Add (".mp3", "audio/mpeg");
                Add (".wma", "audio/x-ms-wma");
                Add (".wav", "audio/x-wav");
                
                //'Video'
                Add (".wmv", "audio/x-ms-wmv");
                Add (".swf", "application/x-shockwave-flash");
                Add (".avi", "video/avi");
                Add (".mp4", "video/mp4");
                Add (".mpeg", "video/mpeg");
                Add (".mpg", "video/mpeg");
                Add (".qt", "video/quicktime");

                // 'Images'
                Add (".bmp", "image/bmp");
                Add (".gif", "image/gif");
                Add (".jpeg", "image/jpeg"); 
                Add (".jpg", "image/jpeg");
                Add (".png", "image/png");
                Add (".tif", "image/tiff");
                Add (".tiff", "image/tiff");
            }
        }
    





