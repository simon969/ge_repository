using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using ge_repository.Models;
using ge_repository.interfaces;
using ge_repository.OtherDatabase;
using ge_repository.ESRI;
using ge_repository.Extensions;

namespace ge_repository.services
{
    public class DataService : IDataService
    {
        protected readonly IUnitOfWork _unitOfWork;
        public DataService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<ge_data> CreateData(ge_data newData)
        {
            await _unitOfWork.Data.AddAsync(newData);
            await _unitOfWork.CommitAsync();
            return newData;
        }

        public async Task DeleteData(ge_data data)
        {
            _unitOfWork.Data.Remove(data);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ge_data>> GetAllWithProject()
        {
            return await _unitOfWork.Data
                .GetAllWithProjectAsync();
        }
        public async Task<ge_data> GetDataByIdWithAll(Guid id)
        {
            return await _unitOfWork.Data
                .GetWithAllAsync(id);
        }
        public async Task<ge_data> GetDataById(Guid id)
        {
            return await _unitOfWork.Data
                .FindByIdAsync(id);
        }  
        public async Task<ge_data> GetDataByIdWithFile(Guid Id) {
            ge_data d =await _unitOfWork.Data
                            .FindByIdAsync(Id);
                    d.file = await _unitOfWork.Data.GetFileAsync(Id);
            return d;
        }

        public async Task<MemoryStream> GetFileAsMemoryStream(Guid Id) {
          
            var _data = await _unitOfWork.Data.FindByIdAsync(Id);

            if (_data == null)
            {
                return null;
            }

            var _data_big = await _unitOfWork.Data.GetFileAsync(Id);
            
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
        public  async Task<string> GetFileAsString (Guid Id, bool removeBOM = false) {
            
           var _data = await _unitOfWork.Data.FindByIdAsync(Id);

            if (_data == null)
            {
                return null;
            }

            var encode = _data.GetEncoding();

            var _data_big = await _unitOfWork.Data.GetFileAsync(Id);
            
            if (_data_big == null)
            {
                return null;
            }
            
            string s1 = _data_big.getString(encode,removeBOM);
            
            return s1;

    }
        
        public async Task<string[]> GetFileAsLines(Guid Id) {

            var _data = await _unitOfWork.Data.FindByIdAsync(Id);

            var encode = _data.GetEncoding();

            var _data_big = await _unitOfWork.Data.GetFileAsync(Id);
            
            if (_data_big == null)
            {
                return null;
            }
           
            MemoryStream memory = _data_big.getMemoryStream(encode);
      
            string[] lines = Encoding.ASCII.GetString(memory.ToArray()).
                                Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            
            return lines;
        }


        public async Task<T> GetFileAsClass<T> (Guid Id) {
  
            var _data = await _unitOfWork.Data.FindNoTrackingAsync(m => m.Id == Id);
            
            if (_data == null)
                {
                return default(T);
            }

            Encoding encoding = _data.GetEncoding();
            string format = _data.GetContentType();

            var _data_big = await _unitOfWork.Data.GetFileAsync(Id);
            
            if (_data_big == null)
            {
                return default(T);
            }
            bool removeBOM = true;
            
            try {
                
                if (format == "text/xml") {
                    string s =_data_big.getParsedXMLstring(encoding);
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    using (TextReader reader = new StringReader(s))
                    {
                    T cs = (T) serializer.Deserialize(reader);
                    return cs; 
                    }
                }
                
                if (format == "json") {
                     string s =_data_big.getString(encoding,removeBOM);
                    T cs = JsonConvert.DeserializeObject<T>(s);
                    return cs;
                }
                
                return default(T);
            
            } catch (Exception e) {
                Console.Write (e.Message);
                return default(T);
            }
    }
        public async Task<OtherDbConnections> GetOtherDbConnectionsByDataId(Guid Id) {

            ge_data data = await _unitOfWork.Data.GetWithProjectAsync(Id);
            
            if (data.project.otherDbConnectId == null) {
                return null;
            }

            OtherDbConnections odb = await GetFileAsClass<OtherDbConnections>(data.project.otherDbConnectId.Value);

            return odb;

        }
        public async Task<EsriConnectionSettings> GetEsriConnectionSettingsByProjectId(Guid Id) {
            
            ge_project project = await _unitOfWork.Project.FindByIdAsync(Id);
            
            if (project.esriConnectId == null) {
                return null;
            }

            EsriConnectionSettings ecs = await GetFileAsClass<EsriConnectionSettings>(project.esriConnectId.Value);

            return ecs;

        }
       
    public async Task<IEnumerable<ge_data>> GetDataByProjectId(Guid projectId)
        {
            return await _unitOfWork.Data
                .GetAllByProjectIdAsync(projectId);
        }

        public async Task UpdateData(ge_data Exist, ge_data data)
        {
           ge_data exist =  await _unitOfWork.Data.FindByIdAsync(Exist.Id);
            
         //  Extensions.Extensions.Copy<ge_data>(exist,data);
           
           await _unitOfWork.CommitAsync();
        }
        public Boolean DataExists (Guid Id) {
            return _unitOfWork.Data.Exists(Id);
        }
        public async Task UpdateData(ge_data data)
        {
            ge_data exist = null;

            exist =  await _unitOfWork.Data.FindByIdAsync(data.Id);
                        
            Extensions.Extensions.Copy<ge_data>(data, exist);

            await _unitOfWork.CommitAsync();
        }
        public async Task SetProcessFlag(Guid Id, int value) {
           
            ge_data data = await _unitOfWork.Data.FindByIdAsync (Id);
            data.pflag = value;

        }
        public async Task SetProcessFlagAddEvents(Guid Id, int value, string add) {
            
            ge_data data = await _unitOfWork.Data.FindByIdAsync (Id);
            data.pflag = value;
            data.phistory += add;

        }
        
        public async Task<int> GetProcessFlag(Guid Id) {

            ge_data data = await _unitOfWork.Data.FindByIdAsync (Id);
            
            if (data==null) {
                return -1;
            }

            return data.pflag;
        
        }

        public ge_data NewData (Guid projectId, string UserId) {
            ge_data _data = new ge_data();
            
            _data.projectId = projectId;
            _data.createdId = UserId;
            _data.createdDT =  DateTime.Now;
            
            _unitOfWork.Data.AddAsync(_data);
            return _data;        
        }
    //    public ge_data NewData() {

    //                 d.projectId = project.Id;

    //                 d.locEast = data.locEast;
    //                 d.locNorth = data.locNorth;
    //                 d.locLevel = data.locLevel;

    //                 d.locLatitude = data.locLatitude;
    //                 d.locLongitude =data.locLongitude;
    //                 d.locHeight = data.locHeight;
                    
    //                 d.locMapReference = data.locMapReference;
    //                 d.locName = data.locName;
    //                 d.locAddress = data.locAddress;
    //                 d.locPostcode = data.locPostcode;
                    
    //                 d.datumProjection = data.datumProjection ;

    //                 d.description = data.description;
    //                 d.keywords = data.keywords;
    //                 d.operations = data.operations;



    //    }
       public async Task<ge_data> CreateData(ge_data newData, IFormFile formFile, ModelStateDictionary modelState, long MaxFileSize) {

             
        //     newData.file = _unitOfWork.Data.GetFormFileString(formFile, modelState, MaxFileSize);

        //     newData.file = _unitOfWork.Data.GetFormFileBinary(formFile, modelState, MaxFileSize);
                 
                   
        return await CreateData(newData);

       }

       public async Task<int> CreateData (List<IFormFile> uploadFiles, string[] lastmodified, ge_data template, ModelStateDictionary ModelState, int MaxFileSize) {

            ge_MimeTypes mtypes = new ge_MimeTypes();
            
            int last_modified_offset = 0;

            foreach (var formFile in uploadFiles)
                {
                    ge_data d = new ge_data();
                    ge_data_file f;
                   
                    d.createdId = template.createdId;   
                    d.createdDT = DateTime.UtcNow; 
                    
                    d.editedId = template.editedId;
                    d.editedDT = DateTime.UtcNow;
                   
                    Boolean IsContentText = formFile.IsContentTypeText(true);

                    if (IsContentText) { 
                        Boolean IsContentXML = formFile.IsContentTypeXML();
                        if (IsContentXML) { 
                                f = await _unitOfWork.Data.GetDataFileString(formFile, ModelState, MaxFileSize,Encoding.UTF8,true);
                                d.SetEncoding(Encoding.UTF8);
                        } else {
                                Encoding encoding = formFile.ReadEncoding (Encoding.UTF8);
                                f = await _unitOfWork.Data.GetDataFileString(formFile, ModelState, MaxFileSize, encoding, false);
                                d.SetEncoding(encoding); 
                        }

                    }  else {
                        f = await _unitOfWork.Data.GetDataFileBinary(formFile, ModelState, MaxFileSize);
                        d.SetEncoding(null);
                    }

                    // Perform a second check to catch ProcessFormFile method
                    // violations.
                    if (!ModelState.IsValid) {
                    return -1;
                    }
                    
                    d.projectId = template.Id;

                    d.locEast = template.locEast;
                    d.locNorth = template.locNorth;
                    d.locLevel = template.locLevel;

                    d.locLatitude = template.locLatitude;
                    d.locLongitude = template.locLongitude;
                    d.locHeight = template.locHeight;
                    
                    d.locMapReference = template.locMapReference;
                    d.locName = template.locName;
                    d.locAddress = template.locAddress;
                    d.locPostcode = template.locPostcode;
                    
                    d.datumProjection = template.datumProjection ;

                    d.description = template.description;
                    d.keywords = template.keywords;
                    d.operations = template.operations;
                     
                    // Add deatils of uploaded file to new _ge_data record
                    d.file = f ;
                    d.filesize = formFile.Length; 
                    d.filename = formFile.FilenameNoPath();
                    d.fileext = formFile.FileExtension();
                   
                   if (mtypes.ContainsKey(d.fileext)) {
                    d.filetype = mtypes[d.fileext];
                    } else {
                    d.filetype = formFile.ContentType;    
                    }

                    if (lastmodified[last_modified_offset].IsDateTimeFormat())    {
                    d.filedate = Convert.ToDateTime(lastmodified[last_modified_offset]);
                    }
                    
                    await _unitOfWork.Data.AddAsync(d);
                    
                    last_modified_offset++;
               }

            return last_modified_offset;
       }

    }
}