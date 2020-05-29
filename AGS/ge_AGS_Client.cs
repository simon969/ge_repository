using System;
using System.IO;
using ge_repository.Models;
using Microsoft.EntityFrameworkCore;
using ge_repository.Authorization;
using System.Xml;
using System.Xml.Linq;
namespace ge_repository.AGS 

{

public class ags_config {
    public string host { get; set; }
    public int port { get; set; }
    public string dictionary_file {get;set;}
    public string data_structure {get;set;}
}
   
public class item {
    public string name {get;set;}
    public string description {get;set;}

}
public static class FileExtension {
    		public const string AGS = ".ags";
    		public const string XML = ".xml";
    		public const string XSL = ".xsl";
}
public static class ge_AGS {
    	public const string AGS_ATTRIBUTE_VERSION = "agsversion";
        private const string EmptyString = "";
        
        public const string AGS31 ="3.1";
        public const string AGS404 ="4.04";
        public const string AGS403 = "4.03";
        public const string AGS30 = "3.0";
        public const string AGS40 = "4.0";
        
        public const string AGS3 ="3";
        public const string AGS4 = "4";



        public static string getVersion(string xml_data) {
        var doc = XDocument.Parse(xml_data);
        return getVersion(doc);
        }

        public static bool IsAGS4Any(string s) {
            
            if (s.Equals(AGS4)) return true;
            if (s.Equals(AGS40)) return true;
            if (s.Equals(AGS404)) return true;
            if (s.Equals(AGS403)) return true;
 
            return false;
        }
        public static bool IsAGS3Any(string s) {
            
            if (s.Equals(AGS3)) return true;
            if (s.Equals(AGS30)) return true;
            if (s.Equals(AGS31)) return true;
            return false;
        }
public static string getVersion(XDocument xml_data) {
    
    var retvar = EmptyString;

    var AGSVersion= xml_data.Root.Attribute(ge_AGS.AGS_ATTRIBUTE_VERSION);
    
    if (AGSVersion !=null) {
       retvar = AGSVersion.Value;  
    }

    if (String.IsNullOrEmpty(retvar)) {
        var ags4holefield = xml_data.Descendants("Loca"); 
    }

    if (String.IsNullOrEmpty(retvar)) {
        var ags3holefield = xml_data.Element("Hole"); 
    }
    return retvar;
}
}

public class ge_AGS_Client : AGS_Client_Base {

    public ge_data data_ags {get;set;}
    public ge_data data_xml {get;set;}
    public ge_DbContext _context {get;set;}
    public string userId {get;set;}
    public ge_AGS_Client (string host, int port, 
                            ge_DbContext Context, string UserId):base(host, port) {
        _context = Context;
        userId = UserId;

    }
    public override void readAGS(){
        if (data_ags!=null) {
        ags_data = data_ags.data.getString();
            if (ags_data.Length >0 ) {
                status=enumStatus.AGSReceived;
                //set process flag to prevent process running again
                setProcessFlag(pflagCODE.PROCESSING);
            }
        }
        
    }

    public override void saveXML(){
            
            ge_data_big b = new ge_data_big();
            data_xml = new ge_data();
            
            b.data_xml = xml_data;
            
            ge_MimeTypes types = new ge_MimeTypes(); 
            string stype;
            types.TryGetValue(FileExtension.XML,out stype);
            string filename = Path.GetFileNameWithoutExtension(data_ags.filename);
            
            data_xml.projectId = data_ags.projectId;
            data_xml.createdDT = DateTime.Now;
            data_xml.createdId = userId;
            data_xml.filename = filename + FileExtension.XML;
            data_xml.filesize = xml_data.Length;
            data_xml.filetype = stype;
            data_xml.filedate =  DateTime.UtcNow;
            data_xml.fileext = FileExtension.XML;
            data_xml.encoding = "ascii";
            data_xml.operations = "Read;Download;Update;Delete";
            data_xml.data = b;
            _context.ge_data.Add(data_xml);
            _context.SaveChanges();
            status=enumStatus.XMLSaved;
                      
    }
    public override void CloseConnections(){
        base.CloseConnections();
        setProcessFlag (pflagCODE.NORMAL);
    }
   public  void setProcessFlag(int value) {
            // set process flag = 1 so that the user cannot rerun another ags conversion before this one has completed
                data_ags.pflag = value;
                _context.Attach(data_ags).State = EntityState.Modified;
                _context.SaveChanges();
        }
}

}

