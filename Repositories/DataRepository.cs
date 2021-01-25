using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ge_repository.Models;
using ge_repository.interfaces;
using ge_repository.Extensions;
using System.Xml.Serialization;
using System.Text;
using System.IO;
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
       
        public async Task<ge_data> GetWithProjectAsync(Guid id)
        {
            return await ge_DbContext.ge_data
                .Include(a => a.project)
                .SingleOrDefaultAsync(a => a.Id == id);
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
		
		public async Task<T> getDataAsClass<T> (Guid Id, string format="xml") {
  
			var _data = await ge_DbContext.ge_data
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            if (_data == null)
                {
                return default(T);
            }

            Encoding encoding = _data.GetEncoding();

            var _data_big = await ge_DbContext.ge_data_big
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync(m => m.Id == Id);
            
            if (_data_big == null)
            {
                return default(T);
            }
            bool removeBOM = true;

            string s =_data_big.getString(encoding,removeBOM);
            
            try {
                
                if (format == "xml") {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    using (TextReader reader = new StringReader(s))
                    {
                    T cs = (T) serializer.Deserialize(reader);
                    return cs; 
                    }
                }
                
                if (format == "json") {
                    T cs = JsonConvert.DeserializeObject<T>(s);
                    return cs;
                }
                
                return default(T);
            
            } catch (Exception e) {
                Console.Write (e.Message);
                return default(T);
            }
    }
        private ge_DbContext ge_DbContext
        {
            get { return Context as ge_DbContext; }
        }
    }
}