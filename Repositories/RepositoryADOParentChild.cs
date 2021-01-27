using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ge_repository.interfaces;
using ge_repository.OtherDatabase;

namespace ge_repository.repositories
{
    public abstract class  RepositoryADOParentChild<TParent, TChild> : IRepository<TParent> where TParent : class where TChild : class
    {
        protected readonly dsTable<TParent> _parent;
        protected readonly dsTable<TChild> _child;
        protected readonly string _parentKeyField;
        protected readonly string _childForeignKeyField;
        public RepositoryADOParentChild(string parentTable, 
                                        string parentKeyField,
                                        string childTable,
                                        string childForeignKeyField,
                                        SqlConnection conn)
        {
            _parent = new dsTable<TParent>(parentTable,conn);
            _parentKeyField = parentKeyField;
            _child = new dsTable<TChild>(childTable,conn);
            _childForeignKeyField = childForeignKeyField;
        }        
        public RepositoryADOParentChild(dsTable<TParent> p, dsTable<TChild> c)
        {
            _parent = p;
            _child = c;
        }
        public async Task AddAsync(TParent entity)
        {
            await Task.Run (()=>{
                                _parent.addRow(entity);
                                });

        }

        public async Task AddRangeAsync(IEnumerable<TParent> entities)
        {
            //To be done, but not implemented yet
        }

        public IEnumerable<TParent> Find(Expression<Func<TParent, bool>> predicate)
        {
          //To be done, but not implemented yet
            return null;
        }
        public Task<TParent> FindNoTrackingAsync(Expression<Func<TParent, bool>> predicate){
            return null;
        }

        public async Task<IEnumerable<TParent>> GetAllAsync()
        {
            //To be done, but not implemented yet
            return null;
        }

        public abstract Task<TParent> GetByIdAsync(Guid id);
        public abstract Task<TParent> GetByIdAsync(string id);
        public abstract Task<TParent> GetByIdAsync(int id);
        
        // public async Task<TParent> GetByIdAsync(string id)
        // {
        //     return await Task.Run (() =>
        //                             {
        //                                   string where = $"{_parentKeyField}='{id}'";
        //                                   DataRow prow = _parent.dataTable.Select (where).SingleOrDefault();
        //                                   if (prow==null) {
        //                                       _parent.sqlWhere(where);
        //                                       _parent.getDataTable();
        //                                       prow = _parent.dataTable.Rows[0];
        //                                   }
                                                        
        //                                   TParent parent = default(TParent);
        //                                   get_values(prow, parent);
        //                                   string parentId = prow[_parentKeyField].ToString();
        //                                   string rwhere = $"{_childForeignKeyField}='{parentId}'";
        //                                   DataRow[] rows = _child.dataTable.Select (rwhere);
        //                                   if (rows==null) {
        //                                       _child.sqlWhere(rwhere);
        //                                       _child.getDataTable();
        //                                       rows = _child.dataTable.Select();
        //                                   }

        //                                   parent.readings = new List<ge_log_reading>();

        //                                   foreach(DataRow rrow in rows)
        //                                     {    
        //                                         ge_log_reading r =  new ge_log_reading();
        //                                         get_values(rrow, r);
        //                                         file.readings.Add(r);
        //                                     }  
        //                                 file.OrderReadings();
        //                                 file.unpack_exist_file();
        //                                 return file;
        //                             }

        //                      );
           
        // }
        // public abstract void  get_values(DataRow row, TParent entity) ;
        // public abstract void get_values(DataRow row, TChild entity); 
        public void Remove(TParent entity)
        {
         //To be done, but not implemented yet
        }

        public void RemoveRange(IEnumerable<TParent> entities)
        {
           //To be done, but not implemented yet
        }

        public Task<TParent> SingleOrDefaultAsync(Expression<Func<TParent, bool>> predicate)
        {
            //Not implemented yet
            return null;
        }
    }
}