
using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Text;
namespace ge_repository.OtherDatabase {


public class gINTTables {

        public dsTable MOND {get;} = new dsTable ("MOND", "DateTime ASC");
        public dsTable MONG {get;} = new dsTable ("MONG");
        public dsTable MONV {get;} = new dsTable ("MONV");
        public dsTable POINT {get;} = new dsTable ("POINT");
        public dsTable PROJ {get;} = new dsTable ("PROJECT");
        public dsTable ERES {get;} = new dsTable ("ERES");
        public dsTable SAMP {get;} = new dsTable ("SAMP");
        public static string DB_DATA_TYPE = "gINT";
        public dsTable TRAN {get;} = new dsTable ("TRAN");
}

public class logTables {

        public dsTable reading {get;} =  new dsTable("ge_log_reading","ReadingDatetime ASC" );
        public dsTable file {get;} = new dsTable("ge_log_file");
        public static string DB_DATA_TYPE = "logger";
    
}
 

public class dsTable {
    public string tableName {get;}
    public string sqlQuery {get;private set;}
    public DataSet dataSet {get;private set;}
    public DataTable dataTable {get;private set;}
    private SqlConnection connection;
    private SqlDataAdapter dataAdapter;
    private SqlCommandBuilder builder;
    private StringBuilder sb;
    private int current = 0;
    public string sortOrder {get;set;} = "";
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
    public DataSet getDataSet() {
        try {
        dataAdapter = new SqlDataAdapter(sqlQuery, connection);
        builder = new SqlCommandBuilder(dataAdapter);
        dataSet = new DataSet();
        dataAdapter.Fill(dataSet,tableName);
        dataSet.Tables[0].DefaultView.Sort = sortOrder;
        return dataSet;
        } catch (Exception e) {
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
            return null;
        }
    }
    public DataRow NewRow() {
        try {
        return dataSet.Tables[tableName].NewRow();
        } catch (Exception e) {
            return null;
        }
    }
    public void addRow(DataRow newRow) {
        dataSet.Tables[tableName].Rows.Add(newRow);
    } 
    public DataRow LastRow() {
    try {
       return dataTable.Rows[dataTable.Rows.Count-1];
        } catch (Exception e) {
            return null;
        }
    }
    public DataRow FirstRow() {
        try {
            return dataTable.Rows[0];
        } catch(Exception e) {
            return null;
        }
   }
    public bool EOF() {
        try {
        return (current>dataTable.Rows.Count);
        } catch (Exception e) {
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
        builder.GetUpdateCommand();
        return dataAdapter.Update(dataSet,tableName);
    }
    public int BulkUpdate() {
        // initialise string builder to collect insert update strings
        sb = new StringBuilder();
        // add event handller to get upadte records to compile batch sql string
        dataAdapter.RowUpdating += new SqlRowUpdatingEventHandler(da_RowUpdating);

        int rows = dataAdapter.Update(dataSet,tableName);
        
        string s1 = sb.ToString();
        
        if (s1.Length>0) {
            SqlCommand cmd = new SqlCommand(sb.ToString(), connection);
            // Execute the update command.
            var ret = cmd.ExecuteScalar();
        }

        return 0;

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

