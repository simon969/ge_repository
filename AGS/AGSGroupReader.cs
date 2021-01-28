using System;
using System.Linq;
using System.Collections.Generic;
using ge_repository.OtherDatabase;
namespace ge_repository.AGS {

public class AGSGroupReader <T> where T: IAGSGroup {

        
        private string[] _lines;
        private static int NOT_FOUND = -1;
        
        public AGSGroupReader (string s1) {
                _lines = s1.Split (",");
        }
        public AGSGroupReader (string[] s1) {
                _lines = s1;
        }
       
        private int find_line(string[] lines, string value) {
            for (int i=0; i<lines.Count();i++) {
                if (lines[i]==value){
                return i;
                };
            }
            return NOT_FOUND;
        } 
        public List<T> GroupList() { 
            List<T> list = new List<T>();
            readGroup(_lines, list,null,null,null);
            return list;
        } 
        
        public AGSTable<T> GroupTable() {
            AGSTable<T> table =  new AGSTable<T>();
            table.values = new List<T>();
            table.headers = new List<string>();
            table.units = new List<string>();
            table.types = new List<string>();
            readGroup(_lines, table.values, table.units, table.headers,table.types);
            return table;
        } 
        
        public int readGroup(string[] lines, List<T> values, List<string> units, List<string> header, List<string> types) {
        
        T group =  Activator.CreateInstance<T>();
            
        int group_start = find_line (lines, $"\"GROUP\",\"{group.GroupName()}\"");

        if (group_start==NOT_FOUND) {
            return -1;
        }
        
        string[] _header = QuoteSplit (lines[group_start+1]);
        string[] _units =  QuoteSplit (lines[group_start+2]);
        string[] _types =  QuoteSplit (lines[group_start+3]);
        
        readArray(_header, header);
        readArray(_units, units);
        readArray(_types, types);
                
        for (int i=group_start + 4; i<lines.Count();i++) {
            string line = lines[i];
            if (line.Length==0) {
                return i;
            }
            string[] _values = QuoteSplit(line);
            T item = Activator.CreateInstance<T>();
            item.setValues(_header, _values);
            values.Add (item);
        }

        return values.Count;
    }
    private int readArray(string[] array, List<string> list) {
        
        if (list == null) {
            return -1;
        }
        foreach (string s in array)
        {
            list.Add(s);
        }
        return list.Count;
    }

    private string[] QuoteSplit (string s1) {
        string s2 = s1.Substring(1, s1.Length-2);
        return s2.Split("\",\"");
    }
    
    }
}
