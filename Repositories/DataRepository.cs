using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

using ge_repository.Models;
using ge_repository.interfaces;
using ge_repository.Extensions;
using System.Xml.Serialization;


using Newtonsoft.Json;



namespace ge_repository.repositories
{
    public class DataRepository : Repository<ge_data>, IDataRepository
    {
        public DataRepository(ge_DbContext context) 
            : base(context)
        { }
        public async Task<IEnumerable<ge_data>> GetAllDataAsync()
        {
            return await ge_DbContext.ge_data
                   .ToListAsync();
        }
        public async Task<IEnumerable<ge_data>> GetAllWithProjectAsync()
        {
            return await ge_DbContext.ge_data
                .Include(a => a.project)
                .ToListAsync();
        }
        public async Task<ge_data> GetWithAllAsync(Guid Id)
        {
            return await ge_DbContext.ge_data
                .Include(a => a.project)
                .Include(a => a.project.group)
                .SingleOrDefaultAsync(a => a.Id == Id);
        }
        public async Task<ge_data> GetWithProjectAsync(Guid Id)
        {
            return await ge_DbContext.ge_data
                .Include(a => a.project)
                .SingleOrDefaultAsync(a => a.Id == Id);
        }
        public async Task<ge_data_file> GetFileAsync(Guid Id) {

            return await ge_DbContext.ge_data_file
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync(m => m.Id == Id);

        }
        public async Task<IEnumerable<ge_data>>  GetAllByProjectIdAsync(Guid Id) 
        {
            return await ge_DbContext.ge_data
                .Where(a => a.projectId == Id)
                .ToListAsync();

        }
        public async Task<IEnumerable<ge_data>>  GetAllByGroupIdAsync(Guid Id) 
        {
            return await ge_DbContext.ge_data
                .Where(a => a.project.groupId == Id)
                .ToListAsync();

        }
				
        private ge_DbContext ge_DbContext
        {
            get { return _context as ge_DbContext; }
        }

        public async Task<ge_data_file> GetDataFileBinary(IFormFile formFile, ModelStateDictionary modelState, long MaxFileSize) {

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
                            await ms.WriteAsync(buffer, 0, read);
                        }
                        ge_data_file _file = new ge_data_file();
                        _file.data_binary = ms.ToArray();

                        // Check the content length in case the file's only
                        // content was a BOM and the content is actually
                        // empty after removing the BOM.

                        if (_file.data_binary.Length > 0)
                        {
                            return _file;
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
        public async Task<ge_data_file> GetDataFileString(IFormFile formFile, ModelStateDictionary modelState, long MaxFileSize, Encoding encoding, Boolean removeXMLDeclaration) {
            
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
                        ge_data_file _file = new ge_data_file();
                        
                        _file.data_string = await reader.ReadToEndAsync();
                        
                        // Check the content length in case the file's only
                        // content was a BOM and the content is actually
                        // empty after removing the BOM.
                        if (_file.data_string.Length > 0)
                        {
                            if (removeXMLDeclaration==true) {
                                // Encoding newEncoding = new UnicodeEncoding();
                                // string fileContents2 = fileContents.SetEncoding(newEncoding);
                                _file.data_string =  _file.data_string.RemoveXmlDeclaration();
                                return  _file;
                            }
                                                       
                            return  _file;
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

            return null;
        }
        public async Task WriteFileByteStreamToBody(Microsoft.AspNetCore.Http.HttpResponse Response, Guid id)
        {
        
            var _data = await ge_DbContext.ge_data
                                        .Include(d =>d.project)
                                        .SingleOrDefaultAsync(m => m.Id == id);

            Response.StatusCode = 200;
            Response.Headers.Add( HeaderNames.ContentDisposition, $"attachment; filename=\"{Path.GetFileName( _data.filename )}\"" );
            Response.Headers.Add( HeaderNames.ContentType, _data.filetype  );    
            
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
                        using (var outputStream = Response.Body) {;
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


    }
}