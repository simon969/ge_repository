    using System;
    using System.Collections.Generic;
    using ge_repository.Extensions;
    using System.Linq;
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
        public int find_row(string name, int retIfNotFound, Boolean Exact=true) {
    
        search_item si = search_items.Find(e=>e.name==name);
        
        if (si==null && Exact==false) {
            si = search_items.Find(e=>e.name.Contains(name));
        }

        if (si==null) {
            return retIfNotFound;
        }
        
        return si.row;

        }

        public int data_start_row (int NotFoundVal = -1) {
        
        search_table st = st = search_tables[0];

        if (st==null) {
            return -1;
        }
        
        int line_start = find_row("data_start",NotFoundVal);
        
        if (line_start != NotFoundVal) {
            search_item si = search_items.Find(e=>e.name=="data_start");
            line_start = si.row + si.row_offset;
      
        }
       
        if (line_start==NotFoundVal) {
            line_start = find_row(st.header.search_item,NotFoundVal); 
            line_start = line_start + 1;   
        }
        
        if (line_start==NotFoundVal) {
            line_start = find_row("header",NotFoundVal); 
            line_start = line_start + 1;
        }

        return line_start;

        }

        public int data_end_row( int NotFoundVal = -1) {
        int line_end = find_row("data_end",NotFoundVal);
        return line_end;
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
                public string match_exact {get;set;} = "false";

                public Boolean MatchExact() {return match_exact=="true";}
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
            public string required {get;set;} = "true";

            public Boolean IsRequired() {return required=="true";}
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

        public class SearchTerms {
            private static int MAX_SEARCH_LINES = 100;
            private int NOT_FOUND = -1;
        public SearchTerms () {

        }
        public ge_search findSearchTerms (ge_search dic, string table, ge_log_workbook wb, string sheet) {
    
        ge_search new_dic = new ge_search();

        new_dic.name = "logger header created:" + DateTime.Now;
       
        try {
            
            if (!String.IsNullOrEmpty(sheet)) {
                wb.setWorksheet(sheet);
            } 

            if (String.IsNullOrEmpty(sheet) & wb.setOnlyWorksheet()==false) {
                new_dic.status ="Unable to determine which worksheet to get data from";
                return new_dic;
            } 

            foreach  (search_item si in dic.search_items) {
                    si.value = wb.matchReturnValue(si.search_text, si.start_offset, si.row_offset,si.MatchExact());
                    // ge_log_workbook is zero based array of worksheet so will match string[] 
                    si.row = wb.matchReturnRow(si.search_text, si.MatchExact());
                    new_dic.search_items.Add (si);
            }
                    
            foreach (search_table st in dic.search_tables.Where(e=>e.name.Contains(table))) {

                Boolean colNotFound = false;
                
                search_range sh = st.header;
                
                int header_row = 0;
                int header_offset = 0;
                search_item si2 = null;
                colNotFound = false;
                
                if (!String.IsNullOrEmpty(sh.search_item)) {
                    si2 =  new_dic.search_items.Find(e=>e.name==sh.search_item);
                    // ge_log_workbook is zero based array of worksheet so will match[]
                    header_row = si2.row ;
                    header_offset = si2.row_offset;

                }
                foreach (value_header vh in st.headers) { 
        
                            int i = wb.matchReturnColumn(vh.search_text,header_row,header_offset);
                            
                            if (i == NOT_FOUND) {
                                new_dic.status = $"column search text [{vh.search_text}] of table [{table}] not found";
                                colNotFound=true;
                                break;
                            } else {
                                // ge_log_workbook is zero based array of worksheet so 
                                // add 1 so column is correctly located in the csv file
                                vh.found = i + vh.col_offset;
                                vh.source = ge_log_constants.SOURCE_ACTUAL;
                            }
                }

                if (colNotFound==false) {
                        st.name = table;
                        st.id = table;
                        new_dic.search_tables.Add (st);
                        new_dic.setFoundTableValues (st);
                        new_dic.status =$"all columns of table {table} found";
                        break; 
                }
            }
        } catch (Exception e) {
        new_dic.status = e.Message;
        }

        return new_dic;
    }

    public ge_search findSearchTerms(ge_search dic, string name, string[] lines) {
        
        ge_search new_dic = new ge_search();

        new_dic.name = "logger header created:" + DateTime.Now;
        int max_line = lines.Count()-1;
        int max_search_lines = Math.Min(max_line, MAX_SEARCH_LINES);

        foreach  (search_item si in dic.search_items) {
            for (int i = 0; i<max_search_lines; i++) {
                if (lines[i].Contains(si.search_text)) {
                     si.row = i;
                     si.row_text = lines[i + si.row_offset];
                     si.value_string();
                     new_dic.search_items.Add (si);
                     break;
                }
            }

        }
        
        search_item end_line = dic.search_items.Find(i=>i.name=="data_end");
        
        if (end_line!=null) {
            if (lines[max_line].Contains(end_line.search_text))
            end_line.row_text = lines[max_line];
            end_line.row = max_line;
            new_dic.search_items.Add (end_line);
        }
        
        new_dic.array_items = new_dic.getSplitItems();
        
        Boolean colNotFound=false;
        
        foreach (search_table st in dic.search_tables) {
            
            search_range sh = st.header;
            string header = "";
            search_item si = null;
            colNotFound = false;
            if (sh.row>0) {header = lines[sh.row];}
            if (!String.IsNullOrEmpty(sh.search_item)) {
                si =  new_dic.search_items.Find(e=>e.name==sh.search_item);
                if (si==null) { continue;}
                header = new_dic.search_items.Find(e=>e.name==sh.search_item).row_text;
            }
            
            if (header=="") {
                new_dic.status=$"table {st.name} header {st.header.search_item} not found";
                continue;
            }    
            
            string[] columns =  header.Split(",");
                foreach (value_header vh in st.headers) { 
                            if (vh.found == NOT_FOUND) {
                                int i = columns.findFirstIndexContains(vh.search_text);
                                if (i == NOT_FOUND && vh.IsRequired()==true) {
                                    new_dic.status = $"required column [{vh.id}] search text [{vh.search_text}] of table [{name}] not found";
                                    colNotFound=true;
                                    break;
                                } 
                                if (i!=NOT_FOUND) {
                                vh.found = i + vh.col_offset;
                                vh.source = ge_log_constants.SOURCE_ACTUAL;
                                }
                            }
                }

                if (colNotFound==false) {
                    if (name == null | name == "" | name == st.name) {
                        new_dic.search_tables.Add (st);
                        new_dic.setFoundTableValues (st);
                        new_dic.status =$"all required columns of table {name} found";
                        break; 
                    }
                }

        }
        
        return new_dic;
    }




        }
    
    }


    