using System;
using System.Text;
using System.Linq;
using ge_repository.OtherDatabase;

namespace ge_repository.AGS {


public class AGSWriter { 
    private IAGSGroupTables _tables; 
    public AGSWriter(IAGSGroupTables tables) {
        _tables = tables;
    }
    public string CreateAGS404String(string [] groups) {
        
        if (groups==null) {
            groups =  new string[] {"PROJ","UNIT","DICT","ABBR","TYPE","LOCA","SAMP","ERES","MONG","MOND"};
        }

        AGS404GroupTables _tables404 = (AGS404GroupTables) _tables;
        StringBuilder sb = new StringBuilder();

        if (groups.Contains("PROJ") && _tables404.PROJ != null) {
            AGSGroupWriter<PROJ> gw = new AGSGroupWriter<PROJ>(_tables404.PROJ); 
            gw.writeGroup(sb);
        }
        
        if (groups.Contains("LOCA") && _tables404.LOCA != null) {
            AGSGroupWriter<POINT> gw = new AGSGroupWriter<POINT>(_tables404.LOCA); 
             sb.AppendLine();
             gw.writeGroup(sb);
           
        }
           
        if (groups.Contains("SAMP") && _tables404.SAMP != null) {
            AGSGroupWriter<SAMP> gw = new AGSGroupWriter<SAMP>(_tables404.SAMP); 
            sb.AppendLine();  
            gw.writeGroup(sb);
        }
        
        if (groups.Contains("ERES") && _tables404.ERES != null) {
            AGSGroupWriter<ERES> gw = new AGSGroupWriter<ERES>(_tables404.ERES); 
            sb.AppendLine(); 
            gw.writeGroup(sb);
        }

        if (groups.Contains("TRAN") && _tables404.TRAN != null) {
            AGSGroupWriter<TRAN> gw = new AGSGroupWriter<TRAN>(_tables404.TRAN); 
            sb.AppendLine(); 
            gw.writeGroup(sb);
        }

        if (groups.Contains("ABBR") && _tables404.ABBR != null) {
            AGSGroupWriter<ABBR> gw = new AGSGroupWriter<ABBR>(_tables404.ABBR); 
            sb.AppendLine(); 
            gw.writeGroup(sb);
        }

        if (groups.Contains("UNIT") && _tables404.UNIT != null) {
            AGSGroupWriter<UNIT> gw = new AGSGroupWriter<UNIT>(_tables404.UNIT); 
            sb.AppendLine(); 
            gw.writeGroup(sb);
        }


        return sb.ToString();
    }
}

}
