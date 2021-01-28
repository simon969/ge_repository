using System;
using System.Linq;
using ge_repository.OtherDatabase;

namespace ge_repository.AGS {


public class AGSReader {

    private string[] _lines;


    public AGSReader(string s1) {
    
    _lines = s1.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

    }
    public AGSReader (string[] lines) {
        _lines = lines;
    }
    public AGS404GroupLists CreateAGS404GroupLists() {

        AGS404GroupLists g =  new AGS404GroupLists();
        
        g.PROJ = new AGSGroupReader<PROJ>(_lines).GroupList();
        g.UNIT = new AGSGroupReader<UNIT>(_lines).GroupList();
        g.DICT = new AGSGroupReader<DICT>(_lines).GroupList();
        g.ABBR = new AGSGroupReader<ABBR>(_lines).GroupList();

        g.LOCA = new AGSGroupReader<POINT>(_lines).GroupList();
        g.SAMP = new AGSGroupReader<SAMP>(_lines).GroupList();
        g.ERES = new AGSGroupReader<ERES>(_lines).GroupList();
        
        g.MONG = new AGSGroupReader<MONG>(_lines).GroupList();
        g.MOND = new AGSGroupReader<MOND>(_lines).GroupList();

        return g;
    }
    public AGS404GroupTables CreateAGS404GroupTables(string[] groups) {

        if (groups==null) {
            groups =  new string[] {"PROJ","UNIT","DICT","ABBR","TYPE","LOCA","SAMP","ERES","MONG","MOND"};
        }

        AGS404GroupTables g =  new AGS404GroupTables();
        
        if (groups.Contains("PROJ")) { g.PROJ = new AGSGroupReader<PROJ>(_lines).GroupTable();}

        if (groups.Contains("UNIT")) { g.UNIT = new AGSGroupReader<UNIT>(_lines).GroupTable();}
        if (groups.Contains("DICT")) {g.DICT = new AGSGroupReader<DICT>(_lines).GroupTable();}
        if (groups.Contains("ABBR")) {g.ABBR = new AGSGroupReader<ABBR>(_lines).GroupTable();}
        if (groups.Contains("TYPE")) {g.TYPE = new AGSGroupReader<TYPE>(_lines).GroupTable();}
        
        if (groups.Contains("LOCA")) {g.LOCA = new AGSGroupReader<POINT>(_lines).GroupTable();}
        
        if (groups.Contains("SAMP")) { g.SAMP = new AGSGroupReader<SAMP>(_lines).GroupTable();}
        if (groups.Contains("ERES")) { g.ERES = new AGSGroupReader<ERES>(_lines).GroupTable();}
        if (groups.Contains("MONG")) { g.MONG = new AGSGroupReader<MONG>(_lines).GroupTable();}
        if (groups.Contains("MOND")) { g.MOND = new AGSGroupReader<MOND>(_lines).GroupTable();}

        return g;
    }
}

}    