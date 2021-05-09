using System;
using ge_repository.OtherDatabase ;
using System.Collections.Generic;
using System.Data;


namespace ge_repository.OtherDatabase  {

public class ge_data_table {

public DataTable dt {get; set; } = new DataTable(); 
public ge_search search_template {get;set;}
public search_table search_table {get;set;}
public ge_data_table (){}
public ge_data_table (string name, string[] columns) {

        dt.TableName = name;

        foreach(string column_name in columns) {
            dt.Columns.Add(column_name);
        }

}
public ge_data_table (string[] columns) {

        foreach(string column_name in columns) {
            dt.Columns.Add(column_name);
        }

}

}

}

