using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ge_repository.interfaces;
using ge_repository.OtherDatabase;
//using System.Data.DataSetExtensions;

namespace ge_repository.repositories
{
    public class RepositoryADO<TEntity> : IADORepository<TEntity> where TEntity : class
    {
        protected readonly dsTable<TEntity> _table;
        protected readonly string _primarykey;
        
        public RepositoryADO (dsTable<TEntity> t) {
            _table = t;
        }
        public RepositoryADO(string name,string primarykey, SqlConnection conn)
        {
            _table = new dsTable<TEntity>(name,conn);
            _primarykey = primarykey;

        }
         public RepositoryADO(string name,string primarykey, SqlConnection conn, int gINTProjectId)
        {
            _table = new dsTable<TEntity>(name,conn, gINTProjectId);
            _primarykey = primarykey;

        }
        public async Task AddAsync(TEntity entity)
        {
          
          await Task.Run (() => { 
                                 _table.addRow(entity);
                                 });

        }
       
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            
        }
        public virtual async Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities, string exist_where)
        {
            return -1;
        }

        public async Task<List<TEntity>> FindAsync(string where)
        {
            return await Task.Run (() => {
                                            _table.sqlWhere(where);
                                            _table.getDataTable();
                                            
                                            if (_table.dataTable == null) {
                                            return null;
                                            }
                                            
                                            return _table.TableAsList();
                                            
                                            
                                    });
        }
        public async Task<TEntity> FindSingleAsync(string where){
             return await Task.Run (() => {
                                            DataRow row = null;
                                            
                                            if (_table.dataTable!=null)  { 
                                            // Is the record already in the dataTable
                                                row = _table.dataTable.Select (where).SingleOrDefault();
                                            }
                                            
                                            if (row == null) {
                                            // record not in current datatable, requery data source,
                                                _table.sqlWhere(where);
                                                _table.getDataTable();

                                                if (_table.dataTable == null){
                                                    return null;
                                                }
                                                row = _table.dataTable.Rows[0];
                                            }

                                            if (row!=null) {
                                               return _table.GetItem(row);
                                            }

                                            return null;  
                                            
                                    });
        }
        public async Task<List<TEntity>> GetAllAsync()
        {
            return await Task.Run (() => {
                                            _table.getDataTable();
                                            return _table.TableAsList();
                                    });
        }
        public async Task<TEntity> GetByIdAsync(int Id)
        { 
            return await FindSingleAsync ($"{_primarykey}={Id}");
        }
        public async Task<TEntity> GetByIdAsync(Guid Id)
        {
            return await FindSingleAsync ($"{_primarykey}={Id}");
        }

        
        public async Task<TEntity> GetByIdAsync(string Id)
        {
            return await FindSingleAsync ($"{_primarykey}={Id}");
        }
        public void Remove(TEntity entity)
        {
         
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
           
        }
        public async Task<TEntity> SingleOrDefaultAsync(string where) 
        {
            return await FindSingleAsync (where);
        }
        
    }
}