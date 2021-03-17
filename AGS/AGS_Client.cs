using System;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace ge_repository.AGS
{
    public abstract class AGS_Client_Base  {
        public  String ags_data = "";
        public String xml_data = "";
        public String dictionaryfile;
        public String datastructure;
        protected static int NOT_OK = -1;
        protected static int OK = 1;
        public enumStatus status = enumStatus.AGSEmpty;
        TcpClient socket = null;

        // const String AGS_START= "[ags_start]";
        // const String AGS_END = "[ags_end]";
        const int MAX_BUFFER_SIZE = 4096;
        const int MAX_WAIT_MINS = 30;

        // const String XML_START= "[xml_start]";
        // const String XML_END = "[xml_end]";
 
        public enum enumStatus
        {  
            AGSStarted,
            AGSStartFailed,
            Connected,
            NotConnected,
            AGSEmpty,
            AGSReceived,
            AGSReceiveFailed,
            AGSSaved,
            AGSSaveFailed,
            AGSSent,
            AGSSentFailed,
            XMLReceived,
            XMLReceiveFailed,
            XMLSaved,
            XMLSaveFailed
          
        }

        public AGS_Client_Base(String ags_server, int port)
        {
          try {

            socket = new TcpClient();
            socket.Connect(ags_server, port);
            Console.WriteLine ("Connected to " + ags_server + ":" + port);

        } catch (Exception e){
         Console.WriteLine(e.Message);
        }

        }
        
        public virtual void CloseConnections() {
          socket.Close();
          socket = null;
        }
                 
       public bool IsConnected(){
        
        if (!socket.Connected) {
        status = AGS_Client_Base.enumStatus.NotConnected;
        }

        return (status!=AGS_Client_Base.enumStatus.NotConnected);
        
      }
        /// <summary>
        /// 
        /// </summary>
        protected async Task<int> sendAGS() {
            return sendAGSByStream();
        }
        private int sendAGSByLine() {
           try {

                 AGS_Package ap = new AGS_Package(ags_data,
                                                dictionaryfile, 
                                                datastructure);
                String s1 = ap.getContentsAGS();

                TextReader tr = new StringReader(s1);

                String line = "";
                
                Stream networkStream = socket.GetStream();
                StreamWriter s_out = new StreamWriter(networkStream) { NewLine = "\r\n", AutoFlush = true };

                while ((line = tr.ReadLine()) != null) {
                    s_out.Write(line);
                    s_out.WriteLine();
                }

                s_out.Flush();

                // status = enumStatus.AGSSent;
                return 1;

            } catch (Exception e)  {
                Console.WriteLine (e.Message);
                // status = enumStatus.AGSSendFailed;
                return -1;
            }
        }
        private int sendAGSByStream() {
            try {
                
                AGS_Package ap = new AGS_Package(ags_data,
                                                dictionaryfile, 
                                                datastructure);
                String s1 = ap.getContentsAGS();
 
                byte[] buffer = new byte[MAX_BUFFER_SIZE];
                int length = 0;
                int total =0 ;
                
                Stream networkStream = socket.GetStream();
                BufferedStream s_out = new BufferedStream(networkStream);

                MemoryStream ms = new MemoryStream( Encoding.ASCII.GetBytes(s1));
                // reset to beginning of string'
                ms.Position = 0;
         
                while((length = ms.Read(buffer)) != 0) {
                    s_out.Write(buffer, 0, length);
                    total =+ length;
                }
                
                s_out.Flush();
                // String msg = "AGS data sent (" + total + ")";
                // System.out.println(msg);
                // status = enumStatus.AGSSent;
                return 1;
            
            } catch (Exception e) {
                Console.WriteLine (e.Message);
                // status = enumStatus.AGSSendFailed;
                return -1;
            }

        }

        protected  async virtual Task<int> saveAGS() {
            // status = enumStatus.AGSSaved;
            return -1;
        }

        protected async virtual Task<int> readAGS() {
            
            if (ags_data.Length>=0) {
                status = AGS_Client_Base.enumStatus.AGSReceived;
                return 1;
            }

            return -1;
        }
        protected async virtual Task<int> readXML() {
            // recieve xml data from ags_server 
            // https://stackoverflow.com/questions/5867227/convert-streamreader-to-byte
            
            DateTime dtStart  = DateTime.Now;
            
            while (IsConnected()) {

                    try {
                        
                        Stream networkStream = socket.GetStream();
                        StreamReader s_in = new StreamReader(networkStream);
                        Boolean IsXMLData = false;
                        int read;
                        byte[] buffer = new byte[MAX_BUFFER_SIZE];

                        // MemoryStream ms = new MemoryStream();
                        StringBuilder sb =  null; 
                        while ((read = s_in.BaseStream.Read(buffer, 0, MAX_BUFFER_SIZE)) > 0) {
                                // ms.Write(buffer, 0, read);
                                string s = System.Text.Encoding.UTF8.GetString(buffer, 0, read);
                                if (IsXMLData==true) { 
                                    sb.Append(s);  
                                    if (s.IndexOf(AGS_Package.FILE_END)>0) {
                                        break; 
                                    } 
                                }
                                if (IsXMLData==false) {
                                    if (s.IndexOf(AGS_Package.FILE_START)==0) {
                                        IsXMLData=true;
                                        sb =  new StringBuilder();
                                        sb.Append(s);
                                        
                                        if (s.IndexOf(AGS_Package.FILE_END)>0) {
                                            break; 
                                        } 

                                    } else {
                                    // status = enumStatus.XMLReceiveFailed;
                                        return -1;
                                    // throw new Exception ("Unexpected response from AGS Server [" + s.Substring(0,32) + "]");
                                    }
                                }
                                
                                                    
                            }

                        //    xml_data = Encoding.UTF8.GetString(ms.ToArray());
                        AGS_Package ap = new AGS_Package (AGS_Package.ContentType.XML, sb.ToString());
                        
                        if (ap.HasXMLData()) {
                            xml_data = ap.data_xml;
                            status = enumStatus.XMLReceived;
                            return 1;
                        } else {
                            return -1;       
                            // throw new Exception("No XML data found in AGS_Package");
                        }
                        
                        DateTime dtNow = DateTime.Now;
                        TimeSpan tsDuration = dtNow-dtStart;

                        if (tsDuration.Minutes >= MAX_WAIT_MINS) {
                            return -1;
                        }

                    } catch (Exception e) {
                        Console.WriteLine (e.Message);
                    //   status = enumStatus.XMLReceiveFailed;
                        return -1;
                    }
            }

            return -1;
        
        }
        protected async virtual Task<int> saveXML() {
            // Write xmldata to database
             return -1;
        }
        protected async virtual Task init_actions(Boolean SaveByAction) {}
        protected async virtual Task actionStarted(string s1, DateTime when) {}
        protected async virtual Task actionEnded(string s1, int status) { }
        protected async virtual Task final_actions(string s1) {}

      public async Task<enumStatus> start() {

            
            status = enumStatus.AGSEmpty;
            
            await init_actions( true);

            while (IsConnected()) {
                
                if (status == enumStatus.AGSEmpty) {
                    
                    await actionStarted("readAGS", DateTime.Now);
                    int resp = await readAGS();
                    await actionEnded("readAGS",resp);
                    if (resp== NOT_OK) status = enumStatus.AGSStartFailed;
                    if (resp >= OK) status = enumStatus.AGSReceived;
                }
                
                if (status == AGS_Client_Base.enumStatus.AGSReceived) {
                    await actionStarted("saveAGS", DateTime.Now); 
                    int resp = await saveAGS();
                    await actionEnded("readAGS",resp);
                    if (resp== NOT_OK) status = enumStatus.AGSSaveFailed;
                    if (resp >= OK) status = enumStatus.AGSSaved;
                }

                if (status == AGS_Client_Base.enumStatus.AGSSaved) {
                    await actionStarted("sendAGS", DateTime.Now); 
                    int resp = await sendAGS();
                    await actionEnded("readAGS",resp);
                    if (resp== NOT_OK) status = enumStatus.AGSSentFailed;
                    if (resp >= OK) status = enumStatus.AGSSent;
                }

                if (status == AGS_Client_Base.enumStatus.AGSSent) {
                    await actionStarted("readXML", DateTime.Now); 
                    int resp = await readXML();
                    await actionEnded("readXML",resp);
                    if (resp == NOT_OK) status = enumStatus.XMLReceiveFailed;
                    if (resp >= OK) status = enumStatus.XMLReceived;
                }

                if (status == AGS_Client_Base.enumStatus.XMLReceived) {
                    await actionStarted("saveXML", DateTime.Now); 
                    int resp = await saveXML();
                    await actionEnded("saveXML",resp);
                    if (resp == NOT_OK) status = enumStatus.XMLSaveFailed;
                    if (resp >= OK) status = enumStatus.XMLSaved;
                }
                
                if (status == AGS_Client_Base.enumStatus.AGSSaveFailed) {
                    CloseConnections();
                    break;
                }

                if (status == AGS_Client_Base.enumStatus.XMLSaved) {
                    CloseConnections();
                    break;
                }

                if (status == AGS_Client_Base.enumStatus.AGSSentFailed) {
                    CloseConnections();
                    break;
                }

                if (status == AGS_Client_Base.enumStatus.XMLReceiveFailed) {
                    CloseConnections();
                    break;
                }
                
                if (status == AGS_Client_Base.enumStatus.XMLSaveFailed) {
                    CloseConnections();
                    break;
                }
            }

        return status;

        }
}
public class AGS_ClientFile : AGS_Client_Base {
        public String ags_fileNameIN  {get;set;}
        public String ags_fileNameOUT {get;set;}
        public String xml_fileNameOUT {get;set;}
   
        public AGS_ClientFile (string host, int port):base(host, port) {}
        protected async override Task<int> readAGS() {
        
                if (ags_fileNameIN.Length == 0) {
                    return -1;
                }

                using (System.IO.StreamReader file = 
                new System.IO.StreamReader(ags_fileNameIN))
                    {
                        ags_data = file.ReadToEnd();
                    }
                
                if (ags_data.Length == 0) {
                    return -1;
                }  

                return 1;
        }  

        protected async override Task<int> saveAGS() {
        
            if (ags_fileNameOUT.Length == 0) {
                return -1;
            }
        
            using (System.IO.StreamWriter file = 
                new System.IO.StreamWriter(ags_fileNameOUT))
                    {
                        file.Write(ags_data);
                        file.Flush();
                    }
            
            if (ags_data.Length == 0) {
                return -1;
            }

            return 1;

        }
        
        protected async override Task<int> saveXML() {
        
            if (xml_fileNameOUT.Length == 0) {
                return -1;
            }
       
            using (System.IO.StreamWriter file = 
                new System.IO.StreamWriter(xml_fileNameOUT))
                    {
                        file.Write(xml_data);
                        file.Flush();
                    }
            if (xml_data.Length == 0) {
                return -1;
            }

            return 1;
        }
}
public class AGS_ClientDb : AGS_ClientFile {
        String db_connect = "";
        String db_uniqueguidAGS  = "";
        String db_uniqueguidXML = "";
    public AGS_ClientDb (string host, int port):base(host, port) {}
     
   
   protected async override Task<int> readAGS() { 

        if (db_connect.Length == 0) {
            Console.WriteLine ("db_connect:" + db_connect);
            Console.WriteLine ("Insufficient parameters for readDatabaseAGS");
            return -1;
        }    
        
            using (AGS_Database ad =  new AGS_Database (db_connect)) {
                ad.setUniqueGUID_AGS(db_uniqueguidAGS);
                ags_data = ad.readAGS();
            }

            if (ags_data.Length == 0) {
                    return -1;
            }  

            return 1;
        
        }   
   
      protected async override Task<int> saveAGS() {

        base.saveAGS();  

        if (db_connect.Length == 0) {
                Console.WriteLine ("Insufficient parameters for saveDatabaseAGS");
                return -1;
            } else { 
                using (AGS_Database ad =  new AGS_Database (db_connect)) {
                ad.setUniqueGUID_AGS(db_uniqueguidAGS);
                ad.saveAGS();
                }
            }

        return 1;
    }
     protected async override Task<int> saveXML() {
        base.saveXML();

        if (db_connect.Length == 0) {
            Console.WriteLine ("Insufficient parameters for saveDatabaseXML");
            return -1;
        }   else { 
                using (AGS_Database ad =  new AGS_Database (db_connect)) {
                ad.setUniqueGUID_XML(db_uniqueguidXML);
                ad.saveXML();
                }
            }

       return 1;
    }
        
    public void setDbConnect(String connect) {
          db_connect = connect;
    }   
}
    interface IAGS_Client { 

        void readAGS();
        void saveAGS() ;
        void saveXML() ;

    }





}
