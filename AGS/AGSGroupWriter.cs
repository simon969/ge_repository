using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using ge_repository.Extensions;

namespace ge_repository.AGS {

public class AGSGroupWriter <T> where T: IAGSGroup { 

        private AGSTable<T> _table;

        public AGSGroupWriter (AGSTable<T> table) {
                _table = table;
        }

        public int writeGroup(StringBuilder sb) {
            try {
                T group =  Activator.CreateInstance<T>();
                string[] header = _table.headers.ToArray<string>();
                string[] units = _table.units.ToArray<string>();
                string[] types = _table.types.ToArray<string>();
                
                sb.Append($"\"GROUP\",\"{group.GroupName()}\"");
                sb.AppendLine();
                sb.Append(header.ToDelimString(",","\""));
                sb.AppendLine();
                sb.Append(units.ToDelimString(",","\""));
                sb.AppendLine();
                sb.Append(types.ToDelimString(",","\""));
                sb.AppendLine();

                foreach (T item in _table.values) {
                    string[] row = item.get_values(header,units,types);
                    sb.Append(row.ToDelimString(",","\""));
                    sb.AppendLine();
                }
            
            return 1;

            } catch (Exception e) {
                return -1;
            }
            
        }

        public int appendGroup(StringBuilder sb) {
            try {
            
            string[] header = _table.headers.ToArray<string>();
            string[] units = _table.units.ToArray<string>();
            string[] types = _table.types.ToArray<string>();
            
            foreach (T item in _table.values) {
                    string[] row = item.get_values(header,units,types);
                    sb.Append(row.ToDelimString(",","\""));
                    sb.AppendLine();
                }
            
            return 1;

            } catch (Exception e) {
                return -1;
            }
        }


        }
    
}