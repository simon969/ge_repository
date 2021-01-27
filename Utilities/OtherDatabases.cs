
using System;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Text;
namespace ge_repository.OtherDatabase {


public class gINTTables {

        public dsTable<MOND> MOND {get;} = new dsTable<MOND> ("MOND", "DateTime ASC");
        public dsTable<MONG> MONG {get;} = new dsTable<MONG> ("MONG");
        public dsTable<MONV> MONV {get;} = new dsTable<MONV> ("MONV");
        public dsTable<POINT> POINT {get;} = new dsTable<POINT> ("POINT");
        public dsTable<PROJ> PROJ {get;} = new dsTable<PROJ> ("PROJECT");
        public dsTable<ERES> ERES {get;} = new dsTable<ERES> ("ERES");
        public dsTable<SAMP> SAMP {get;} = new dsTable<SAMP> ("SAMP");
        public static string DB_DATA_TYPE = "gINT";
        public dsTable<TRAN> TRAN {get;} = new dsTable<TRAN> ("TRAN");
       
}

public class logTables {

        public dsTable<ge_log_reading> reading {get;} =  new dsTable<ge_log_reading>("ge_log_reading","ReadingDatetime ASC" );
        public dsTable<ge_log_file> file {get;} = new dsTable<ge_log_file>("ge_log_file");
        public static string DB_DATA_TYPE = "logger";
    
}
 

public class dsTable<T> {
    public string tableName {get;}
    public string sqlQuery {get;private set;}
    public DataSet dataSet {get;private set;}
    public DataTable dataTable {get;private set;}
    private SqlConnection connection;
    private SqlDataAdapter dataAdapter;
    private SqlCommandBuilder builder;
    private StringBuilder sb;
    private int current = 0;
    public int COMMAND_TIMEOUT {get;set;} = 1200;
    public string sortOrder {get;set;} = "";
    public string UniqueKey {get;set;}= "Id";

    public dsTable(string TableName, SqlConnection conn) {
        tableName = TableName;
        sqlQuery = "select * from " + tableName + " where 0 = 1";
        sortOrder = "";

    }
    
    public dsTable(string TableName, string SortOrder="") {
        tableName = TableName;
        sqlQuery = "select * from " + tableName + " where 0 = 1";
        sortOrder = SortOrder;
    }
    public void sqlWhere(string where) {
    if (string.IsNullOrEmpty(where)) {
        sqlQuery = "select * from " + tableName;
    } else {
        sqlQuery = "select * from " + tableName + " where (" + where + ")";
    }
    }
    public void setConnection (SqlConnection Connection) {
    connection = Connection;
    }
    public void Reset() {
        sqlQuery = "select * from " + tableName + " where 0 = 1";;
        getDataSet();

    }
   public List<T> TableAsList()  
        {  
            List<T> data = new List<T>();  
            foreach (DataRow row in dataTable.Rows)  
            {  
                T item = GetItem(row);  
                data.Add(item);  
            }  
            return data;  
    }  
    public T GetItem (DataRow dr)  
        {  
            Type temp = typeof(T);  
            T obj = Activator.CreateInstance<T>();  
        
            foreach (DataColumn column in dr.Table.Columns)  
            {  
                foreach (PropertyInfo pro in temp.GetProperties())  
                {  
                    if (pro.Name == column.ColumnName)  
                        pro.SetValue(obj, dr[column.ColumnName], null);  
                    else  
                        continue;  
                }  
            }  
            return obj;  
        } 
    
    public DataSet getDataSet() {
        try {
        dataAdapter = new SqlDataAdapter(sqlQuery, connection);
        builder = new SqlCommandBuilder(dataAdapter);
        dataSet = new DataSet();
        dataAdapter.Fill(dataSet,tableName);
        dataSet.Tables[0].DefaultView.Sort = sortOrder;
        return dataSet;
        } catch (Exception e) {
            Console.Write (e.Message);
            return null;
        }
    }
    public DataTable getDataTable () {
        if (dataSet==null) {
            getDataSet();
        }
        try {
        dataTable  = dataSet.Tables[tableName];
        dataTable.DefaultView.Sort = sortOrder;
        return dataTable;
        } catch (Exception e) {
            Console.Write (e.Message);
            return null;
        }
    }
    public DataRow NewRow() {
        try {
        return dataSet.Tables[tableName].NewRow();
        } catch (Exception e) {
            Console.Write (e.Message);
            return null;
        }
    }
    public void addRow (T newObj) {

            Type temp = typeof(T);  

            DataRow newRow = dataTable.NewRow();

            foreach (DataColumn column in dataTable.Columns)  
            {  
                foreach (PropertyInfo pro in temp.GetProperties())  
                {  
                    if (pro.Name == column.ColumnName)  
                        pro.SetValue(newRow[column.ColumnName],newObj, null);  
                    else  
                        continue;  
                }  
            }  
      
        dataTable.Rows.Add (newRow);
    }
    public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();      

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
    }
    
    public void addRow(DataRow newRow) {
        dataSet.Tables[tableName].Rows.Add(newRow);
    } 
    public DataRow LastRow() {
    try {
       return dataTable.Rows[dataTable.Rows.Count-1];
        } catch (Exception e) {
             Console.Write (e.Message);
            return null;
        }
    }
    public DataRow FirstRow() {
        try {
            return dataTable.Rows[0];
        } catch(Exception e) {
            Console.Write (e.Message);
            return null;
        }
   }
    public bool EOF() {
        try {
        return (current>dataTable.Rows.Count);
        } catch (Exception e) {
        Console.Write (e.Message);
        return true;
    }
    }
    public DataRow NextRow() {
        if (current+1 < dataTable.Rows.Count) {
            current++;
            return dataTable.Rows[current];
        } 
        return null;
    }

    public int Update() {
       return dataAdapter.Update(dataSet,tableName);
    }
    public int BulkUpdate() {
        int ret = 0;
        // initialise string builder to collect insert update strings
        sb = new StringBuilder();
        // add event handller to get upadte records to compile batch sql string
        dataAdapter.UpdateCommand = builder.GetUpdateCommand();
        dataAdapter.InsertCommand = builder.GetInsertCommand();
        dataAdapter.RowUpdating += new SqlRowUpdatingEventHandler(da_RowUpdating);

      //  int rows = dataAdapter.Update(dataSet,tableName);
        int rows_updated = Update();
        
        string s1 = sb.ToString();
        
        if (s1.Length>0) {
         
            SqlCommand cmd = new SqlCommand(sb.ToString(), connection);
            cmd.CommandTimeout = COMMAND_TIMEOUT; 
            // Execute the update command.
            var int32Execute = cmd.ExecuteScalar();
        }

        return ret;

    }
    public row_states get_row_states () {
        row_states us = new row_states();

        us.deleted = dataTable.Select(null, null, DataViewRowState.Deleted);  
        us.added = dataTable.Select(null, null, DataViewRowState.Added);  
        us.modified  = dataTable.Select(null, null, DataViewRowState.ModifiedCurrent);  
        us.unchanged = dataTable.Select(null, null, DataViewRowState.Unchanged);  
        return us;
    }
    public int Delete() {
        SqlCommand s1 = builder.GetDeleteCommand();
        int count = dataTable.Rows.Count;
        for (int i = dataTable.Rows.Count - 1; i >= 0; i--) {
        dataTable.Rows[i].Delete();
        }
        return dataAdapter.Update(dataTable);
    }
    private void da_RowUpdating(object sender, SqlRowUpdatingEventArgs e)
    {  
    //   https://flylib.com/books/en/1.105.1/performing_batch_updates_with_a_dataadapter.html
    // Get the command for the current row update.
   
        if (e.Command==null) {
                return;
        }

    StringBuilder sqlText = new StringBuilder(e.Command.CommandText.ToString());
    // Replace the parameters with values.
    for (int i = e.Command.Parameters.Count - 1; i >= 0; i--)   {
        
        SqlParameter parm = e.Command.Parameters[i];
        
        if (parm.Value==DBNull.Value) {
            sqlText.Replace(parm.ParameterName, "null");
            continue;
        }
        
        if (parm.SqlDbType == SqlDbType.NVarChar ||parm.SqlDbType == SqlDbType.NText) {
            // Quotes around the nvarchar and ntype fields
            sqlText.Replace(parm.ParameterName,"'" + parm.Value.ToString( ) + "'");
            continue;
        }
        
        if (parm.SqlDbType==SqlDbType.DateTime) {
           // format datetime 
            sqlText.Replace(parm.ParameterName, String.Format("'{0:yyyy-MM-ddTHH:mm:ss}'",parm.Value));
            continue;
        }

        sqlText.Replace(parm.ParameterName, parm.Value.ToString( ));
    }
    // Add the row command to the aggregate update command.
    sb.Append(sqlText.ToString( ) + ";");

    // Skip the DataAdapter update of the row.
    e.Status = UpdateStatus.SkipCurrentRow;
    
    }

   
    }    

public class OtherDbConnections {

        public List<dbConnectDetails> connections {get;set;} = new List<dbConnectDetails>();

    
    public dbConnectDetails getConnectType(string DataType) {
  
             foreach (dbConnectDetails value in connections) {
                if (value.Type==DataType) {
                     return value;
                }
             }
             return null;
    }
    
    public int? gINTProjectId (){
        
        dbConnectDetails cd = getConnectType(gINTTables.DB_DATA_TYPE);
        
        if (cd !=null) {
            return cd.ProjectId;
        }

        return null;

    }

    }
    public class row_states {
        public DataRow[] modified {get;set;}
        public DataRow[] deleted {get;set;}
        public DataRow[] added {get;set;}
        public DataRow[] unchanged {get;set;}
        public int updated {get;set;}
        public int Add (row_states rs) {
            modified = add_array(modified, rs.modified);
            deleted = add_array(deleted, rs.deleted);
            added = add_array(added, rs.added);
            unchanged = add_array(unchanged, rs.added);
            updated =+ rs.updated;
            return updated;
        }
        public string errorMsg {get;set;}

        public int Add (DataRow dr) { 
                if (dr.RowState==DataRowState.Modified) {
                    add_array(modified,dr);
                    return 1;
                }
                if (dr.RowState==DataRowState.Unchanged) {
                    add_array(unchanged,dr);
                    return 1;
                }
                if (dr.RowState==DataRowState.Added) {
                    add_array(added,dr);
                    return 1;
                }
                if (dr.RowState==DataRowState.Deleted) {
                    add_array(deleted,dr);
                    return 1;
                }
                return 0;
        }
        private DataRow[] add_array(DataRow[] orig, DataRow[] add) {
            DataRow[] combined = new DataRow[orig.Length + add.Length];
            Array.Copy(orig, combined, orig.Length);
            Array.Copy(add, 0, combined, orig.Length, add.Length);
            return combined;
        }
        private DataRow[] add_array(DataRow[] orig, DataRow add) {
            DataRow[] a = new DataRow[1];
            return add_array (orig, a);
        }
    }

    

    public class dbConnectDetails {
            public string Type {get;set;}
            public string Server {get;set;}
            public string Database {get;set;}
            public string DataSource {get;set;}
            public string UserId {get;set;}
            public String Password {get;set;}
            public int TimeOut {get;set;} = 30;
            public int ProjectId {get;set;}
          
     
    public string AsConnectionString() {
        
        StringBuilder sb = new StringBuilder();
        
        if (!String.IsNullOrEmpty(Server)) {
            if (sb.Length>0) sb.Append("; ");
            sb.Append("Server=" + Server);
        }
        if (!String.IsNullOrEmpty(Database)) {
            if (sb.Length>0) sb.Append("; ");
            sb.Append("Database=" + Database);
        }
        if (!String.IsNullOrEmpty(DataSource)) {
            if (sb.Length>0) sb.Append("; ");
            sb.Append("Data Source=" + DataSource);
        }
        if (!String.IsNullOrEmpty(UserId)) {
            if (sb.Length>0) sb.Append("; ");
            sb.Append("User Id=" + UserId);
        }
        if (TimeOut>0) {
            if (sb.Length>0) sb.Append("; ");
            sb.Append($"Connection Timeout={TimeOut}");
        }
        if (!String.IsNullOrEmpty(Password)) {
            if (sb.Length>0) sb.Append("; ");
            sb.Append("Password=" + Password);
        }
        
        return sb.ToString();
    }
  
    }

}

