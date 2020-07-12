    using System;
    using System.Collections.Generic;
    namespace ge_repository.OtherDatabase  {
    public class ge_search {

        // private string DATE_FORMAT = "yyyy-MM-dd hh:mm:ss";
        // private string FILE_NAME_DATE_FORMAT = "yyyy_MM_dd";
        // private string DATE_FORMAT_AGS = "yyyy-MM-ddThh:mm:ss";

        private int NOT_FOUND = -1;
        public string status {get;set;}
        public string name {get;set;}
        public List<search_item> search_items {get;set;}
        public List<search_table> search_tables {get;set;}
        public List<array_item> array_items {get;set;}
        public ge_search() {
            search_items = new List<search_item>();
            search_tables = new List<search_table>();
        }
        
        public value_header getHeader(string db_name) {
            return getFoundHeader(db_name);
        }
        public value_header getFoundHeader(string db_name, int table = 0) {
           search_table st = search_tables[table]; 
           value_header db_col = st.headers.Find(c=>c.found!=NOT_FOUND && c.db_name==db_name); 
           return db_col;
        }
        public List<array_item> getSplitItems() {
            
            List<array_item> list = new List<array_item>();
            
            foreach (search_item si in search_items) {
                if (si.split > 0 ) {
                    if (si.row_text != null) {
                        string[] array = (si.value).Split (",");
                        array_item ai = new array_item {
                        name = si.name,
                        values = array,
                        };
                        list.Add (ai);
                    }
                }
            }
            if (list.Count==0) {
                return null;
            }

            return list;
            
        }

        private int findChannelCol(search_table st) {

            if (array_items==null) {
                return NOT_FOUND;
            }

            var ai2= array_items.Find(a=>a.name=="CalibrationFactors");
            
            if (ai2 == null) {
                return NOT_FOUND;
            }

            int colChannel = NOT_FOUND;
            
            foreach (value_header vh in st.headers)  {
                for (int i = 0; i < ai2.values.Length; i++) {
                    if (ai2.values[i]!=null) {
                        if (vh.name == ai2.values[i]) {
                           colChannel = i;
                            break;
                        }
                    }        
                }
            }

            return colChannel;

        }

        public void setFoundTableValues(search_table st) {
            
            int colChannel = findChannelCol(st);
            
            if (colChannel == NOT_FOUND) {
                return;
            }

            foreach (array_item ai in array_items) {
                foreach (search_item si in search_items) {
                    if (si.name == ai.name && ai.values.Length >= colChannel) {
                        si.value2 = ai.values[colChannel];
                   }
                }
            }
        }
    }

            

        public class field_headersJSON {
            public List<value_header> items {get;set;}
        }
        public class file_headersJSON {
            public List<search_item> items {get;set;} 
        }
        public class file_arrayJSON {
            public List<array_item> items {get;set;} 
        }
        public class search_item {
                public string name {get;set;}
                public string search_text {get;set;}
                public string row_text {get;set;}
                public int start_offset {get;set;}
                public int row_offset {get;set;}
                public int length {get;set;}   
                public string value {get;set;}
                public string value2 {get;set;}
                public int split {get;set;} = 0;
                public int row {get;set;} 
                public int found {get;set;} = -1;   
                public string units {get;set;}
                public string comments {get;set;} 
                public string source {get;set;} 
            public search_item(){}
            public search_item(string Search_text, int StartOffset, int Length) {
            search_text =   Search_text;
            start_offset = StartOffset;
            length = Length;
         }
      
         public string any_value_string() {
            string ret;
             
            ret = value_string();
             
            if (ret!="") {
                return ret;
            }

            ret = value;
            
            if (ret!="") {
                return ret;
            }
            
            ret = value2;
            
            if (ret!="") {
                return ret;
            }

            return "";

         }

         public string value_string() {
            try {

            int max_len = row_text.Length - (row_text.IndexOf(search_text) + search_text.Length + start_offset);
            
            if (max_len <= 0) return null;
            if (max_len < length) length = max_len;

            value = row_text.Substring(row_text.IndexOf(search_text) + search_text.Length + start_offset,length);

            return value;
            } catch (Exception e) {
              return ""; 
            }
         }
         
         
         public int? value_int() {
             try {
             return Convert.ToInt32(value_string());
             } catch {
                 return null;
             }

         }
         public double? value_dbl() {
             try {
             return Convert.ToDouble(value_string());
             } catch {
                return null;
             }
         }
         public DateTime? value_DateTime() {
             try {
             return Convert.ToDateTime(value_string());
             } catch {
                 return null;
             }
         }
         
        
        }

        public class value_header {
            
            public string name {get;set;}
            public string db_name {get;set;}
            public string id {get;set;}
            public string source {get;set;} 
            public string search_text {get;set;}
            public int col_offset {get;set;} = 0;
            public string units {get;set;}
            public string comments {get;set;} 
            public int found {get;set;} = -1;
            public string format {get;set;} 
        }

        public class search_table {
            public string name {get;set;}
            public string id {get;set;}
            public search_range header {get;set;}
            public search_range data_start {get;set;}
            public search_range data_end {get;set;}
            public List<value_header> headers {get;set;}
            public List<search_item> options {get;set;}
            public string action {get;set;}

        }

        public class search_range {
            public string search_item {get;set;}
            public int row {get;set;}
        }
        public class array_item {
            public string name {get;set;}
            public string[] values {get;set;}
        }
    
    }


    