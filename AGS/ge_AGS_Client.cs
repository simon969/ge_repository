using System;
using System.IO;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.Authorization;
using System.Xml.Linq;
using ge_repository.interfaces;
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
            public const string XQ = ".xq";
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
    
    protected Guid _Id {get;}
    protected ge_data data_ags {get;set;}
    protected ge_data data_xml {get;set;}
    protected IDataService _dataService {get;}
    public Boolean saveByAction {get;set;} = false;

    protected string userId {get;}
    private DateTime _started; 
    private DateTime _started_action; 
    private string _result {get;set;}
  
    public ge_AGS_Client (string host, int port, string dictionary_file, string data_structure,
                            Guid Id, IDataService DataService, string UserId):base(host, port) {
        _Id = Id;
        _dataService = DataService;
        dictionaryfile = dictionary_file;
        datastructure = data_structure;
        userId = UserId;

    }
    public ge_AGS_Client (ags_config config,
                           Guid Id, 
                           IDataService DataService, 
                           string UserId):base(config.host, config.port) {
        _dataService = DataService;
        userId = UserId;
        _Id = Id;
        dictionaryfile = config.dictionary_file;
        datastructure = config.data_structure;
    }
    protected async override Task<int> readAGS(){

        ags_data = await _dataService.GetFileAsString(_Id,false);

        if (ags_data.Length >0 ) {
                // status=enumStatus.AGSReceived;
                // set process flag to prevent process running again
                // setProcessFlag(pflagCODE.PROCESSING);
            return 1;
        }
            return -1;
    }

    protected override async Task<int> saveXML(){
            
            ge_data_file b = new ge_data_file();
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
            data_xml.file = b;
            
            await _dataService.CreateData(data_xml);
          
            return 1;
                      
    }
    public override void CloseConnections(){
        base.CloseConnections();
        setProcessFlag (pflagCODE.NORMAL);
    }
   public  void setProcessFlag(int value) {
            // set process flag = 1 so that the user cannot rerun another ags conversion before this one has completed
                _dataService.SetProcessFlag(_Id, value);
    }
    private async Task<int> ensureDataFile() {
            

            if (_Id == null) {
            return -1;
            }
            
            if (data_ags != null) {
                if (data_ags.Id == _Id) {;
                    return 1;
                }
            }


           data_ags = await _dataService.GetDataById(_Id);
           
            if (data_ags == null) {
               return -1;
            }
           
            return 1; 
    
    
    }
    protected override async Task init_actions(string s1) {
        // base.init_actions(s1); 
        
        saveByAction = true;
        
        var resp = await ensureDataFile();

        _started = DateTime.Now;
        _result =  $"{_started} Background ge_ags_client started with";
        _result += $" dictionary='{dictionaryfile}' datastructure='{datastructure}'" + System.Environment.NewLine;
     
        data_ags.pflag = pflagCODE.PROCESSING;
        
        if (saveByAction==true) {
            data_ags.pflag = pflagCODE.PROCESSING;
            data_ags.phistory += _result;
            await _dataService.UpdateData (data_ags);
        }


    }
     protected override async Task final_actions(string s1) {
        
       // base.final_actions (s1);

        DateTime ended = DateTime.Now;
        TimeSpan dur = ended - _started;

         string s2 =  $"{ended} Background ge_ags_client exited {String.Format("{0:0.000}",dur.TotalMinutes)} mins" + System.Environment.NewLine;
         _result += s2;
         
        data_ags.pflag = pflagCODE.NORMAL;
         
        if (saveByAction==true) {
            data_ags.phistory += s2;
        } else {
            data_ags.phistory += _result;
        }   
        await _dataService.UpdateData (data_ags);
    }

    protected override async Task actionStarted (string s1, DateTime when) {
        base.actionStarted (s1, when);
        _started_action = when;
        string s2 = $"{_started_action} {s1} started" + System.Environment.NewLine;
        _result += s2;

        if (saveByAction==true) {
            data_ags.pflag = pflagCODE.PROCESSING;
            data_ags.phistory += s2;
            await _dataService.UpdateData(data_ags);
        }   
    }
    
    protected override async Task actionEnded (string s1, int i) {
        base.actionEnded (s1, i);
        string outcome = "";
        DateTime ended = DateTime.Now;
        TimeSpan dur = ended - _started_action;
        if (i == NOT_OK) outcome="Not Ok";
        if (i >= OK) outcome="Ok";

        string s2 = $"{DateTime.Now} {s1} ended {String.Format("{0:0.000}",dur.TotalMinutes)} mins result ({i}) {outcome}" + System.Environment.NewLine;
        _result += s2;
        
        if (saveByAction==true) {
          data_ags.phistory += s2;
          await _dataService.UpdateData(data_ags);
        }  
    }
}

}

