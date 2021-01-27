using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ge_repository.Models;
using ge_repository.interfaces;

namespace ge_repository.services
{
    public class DataService : IDataService
    {
        private readonly IUnitOfWork _unitOfWork;
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

        public async Task<ge_data> GetDataById(Guid id)
        {
            return await _unitOfWork.Data
                .GetByIdAsync(id);
        }
        public async Task<MemoryStream> GetFileAsMemoryStream(Guid Id) {
          
            var _data = await _unitOfWork.Data.GetByIdAsync(Id);

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
            
           var _data = await _unitOfWork.Data.GetByIdAsync(Id);

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

            var _data = await _unitOfWork.Data.GetByIdAsync(Id);

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
                
                if (format == "xml") {
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
    public async Task<IEnumerable<ge_data>> GetDataByProjectId(Guid projectId)
        {
            return await _unitOfWork.Data
                .GetAllByProjectIdAsync(projectId);
        }

        public async Task UpdateData(ge_data dataToBeUpdated, ge_data data)
        {
           ge_data to =  await _unitOfWork.Data.GetByIdAsync(dataToBeUpdated.Id);
            

           await _unitOfWork.CommitAsync();
        }
        public async Task UpdateData(ge_data data)
        {
            

            await _unitOfWork.CommitAsync();
        }
    }
}