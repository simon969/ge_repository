using System;
using System.Collections.Generic;
using System.Threading.Tasks; 
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ge_repository.Models;

namespace ge_repository.interfaces
{
    public interface IDataRepository : IRepository<ge_data>
    {
        Task<IEnumerable<ge_data>> GetAllDataAsync();  
        Task<IEnumerable<ge_data>> GetAllWithProjectAsync();
        Task<ge_data> GetWithProjectAsync(Guid id);
        Task<ge_data> GetWithAllAsync(Guid Id);
        Task<IEnumerable<ge_data>> GetAllByProjectIdAsync(Guid Id);
        Task<IEnumerable<ge_data>> GetAllByGroupIdAsync(Guid Id);
        Task<ge_data_file> GetFileAsync(Guid id);
        Task WriteFileByteStreamToBody(Microsoft.AspNetCore.Http.HttpResponse Response, Guid id);
        Task<ge_data_file> GetDataFileBinary(IFormFile formFile, ModelStateDictionary modelState, long MaxFileSize);
        Task<ge_data_file> GetDataFileString(IFormFile formFile, ModelStateDictionary modelState, long MaxFileSize, Encoding encoding, Boolean removeXMLDeclaration);
    }
}