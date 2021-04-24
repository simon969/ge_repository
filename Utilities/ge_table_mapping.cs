 using System;
 using System.Collections.Generic;
 using ge_repository.Extensions;
 using System.Linq;
 namespace ge_repository.OtherDatabase  {

public class ge_table_map {
    public List<table_map> table_maps {get;set;}  = new List<table_map>();

}

    public class table_map {
            public string name {get;set;}
            public string destination {get;set;}
            public string source {get;set;}
            public string where {get;set;}
            public string order {get;set;}
            
            public List<field_map> field_maps {get;set;} = new List<field_map>();
    }
        
    public class field_map {
            public string source {get;set;}
            public string value {get;set;}
            public string destination {get;set;}
            public string format {get;set;}
            public string type {get;set;}
            public string comment {get;set;}
            public string get_value(string[] headers, string[] values) {
                string value = "";

                string[] s1 = source.Split("+");
                
                foreach (string s2 in s1) {
                    for (int i = 0; i<headers.Count(); i++) {
                        if (headers[i]==s2) {
                            value += values[i];
                        }
                    }
                }
                
                if (String.IsNullOrEmpty(format)) {
                    return value;
                }
                
                return String.Format(format,value); 
            }
            public Type get_typeOf() {
                if (type=="string") return typeof(string);
                if (type=="long") return typeof(long);
                return null;
            }
    }
 
 }

