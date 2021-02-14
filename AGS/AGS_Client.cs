using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Sockets;
using System.IO;

using System.Net;
using System.Text;
using System.Data.SqlClient;

namespace ge_repository.AGS
{
    public abstract class AGS_Client_Base  {
        public  String ags_data = "";
        public String xml_data = "";
        public String dictionaryfile;
        public String datastructure;
    
        public enumStatus status = enumStatus.AGSEmpty;
        TcpClient socket = null;

        // const String AGS_START= "[ags_start]";
        // const String AGS_END = "[ags_end]";
        const int MAX_BUFFER_SIZE = 4096;

        // const String XML_START= "[xml_start]";
        // const String XML_END = "[xml_end]";
 
        public enum enumStatus
        {  
            AGSStartFailed,
            NotConnected,
            AGSEmpty,
            AGSReceived,
            AGSSaved,
            AGSSent,
            AGSSendFailed,
            XMLReceived,
            XMLReceiveFailed,
            XMLSaved,
          
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
        private void sendAGS() {
            sendAGSByStream();
        }
        private void sendAGSByLine() {
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

                status = enumStatus.AGSSent;

            } catch (Exception e)  {
                Console.WriteLine (e.Message);
                status = enumStatus.AGSSendFailed;

            }
        }
        private void sendAGSByStream() {
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
                String msg = "AGS data sent (" + total + ")";
                //System.out.println(msg);
                status = enumStatus.AGSSent;
            
            } catch (Exception e) {
                Console.WriteLine (e.Message);
                status = enumStatus.AGSSendFailed;
            }

        }

        public  virtual void saveAGS() {
             status = enumStatus.AGSSaved;
        }

        public virtual void readAGS() {
            if (ags_data.Length>=0) {
                status = AGS_Client_Base.enumStatus.AGSReceived;
            }
           
        }
        public virtual void readXML() {
            // recieve xml data from ags_server 
            // https://stackoverflow.com/questions/5867227/convert-streamreader-to-byte

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
                                status = enumStatus.XMLReceiveFailed;
                                throw new Exception ("Unexpected response from AGS Server [" + s.Substring(0,32) + "]");
                            }
                        }
                        
                                               
                    }

                //    xml_data = Encoding.UTF8.GetString(ms.ToArray());
                AGS_Package ap = new AGS_Package (AGS_Package.ContentType.XML, sb.ToString());
                
                if (ap.HasXMLData()) {
                    xml_data = ap.data_xml;
                    status = enumStatus.XMLReceived;
                } else {
                    throw new Exception("No XML data found in AGS_Package");       
                }
                
                // xml_data = sb.ToString();
               // status = enumStatus.XMLReceived;
 
            } catch (Exception e) {
                Console.WriteLine (e.Message);
                 status = enumStatus.XMLReceiveFailed;
            }
            
        }
        public virtual void saveXML() {
            // Write xmldata to database
             status = enumStatus.XMLSaved;
        }


        public ge_AGS_Client.enumStatus start() {
 
            while (IsConnected()) {
                
                if (status == AGS_Client_Base.enumStatus.AGSEmpty) {
                    readAGS();
                }
                
                if (status == AGS_Client_Base.enumStatus.AGSReceived) {
                    saveAGS();
                }

                if (status == AGS_Client_Base.enumStatus.AGSSaved) {
                    sendAGS();
                }

                if (status == AGS_Client_Base.enumStatus.AGSSent) {
                    readXML();
                }

                if (status == AGS_Client_Base.enumStatus.XMLReceived) {
                    saveXML();
                }
                
                if (status == AGS_Client_Base.enumStatus.XMLSaved) {
                    CloseConnections();
                    break;
                }

                if (status == AGS_Client_Base.enumStatus.AGSSendFailed) {
                    CloseConnections();
                    break;
                }

                if (status == AGS_Client_Base.enumStatus.XMLReceiveFailed) {
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
        public override void readAGS() {
        
        if (ags_fileNameIN.Length == 0) {
            return;
        }
                using (System.IO.StreamReader file = 
                new System.IO.StreamReader(ags_fileNameIN))
                    {
                        ags_data = file.ReadToEnd();
                    }  
        }  
   public override void saveAGS(){
        
        if (ags_fileNameOUT.Length == 0) {
            return;
        }
        
            using (System.IO.StreamWriter file = 
                new System.IO.StreamWriter(ags_fileNameOUT))
                    {
                        file.Write(ags_data);
                        file.Flush();
                    }
        }
        
        public override void saveXML() {
        
        if (xml_fileNameOUT.Length == 0) {
            return;
        }
       
            using (System.IO.StreamWriter file = 
                new System.IO.StreamWriter(xml_fileNameOUT))
                    {
                        file.Write(xml_data);
                        file.Flush();
                    }
        }
}
public class AGS_ClientDb : AGS_ClientFile {
        String db_connect = "";
        String db_uniqueguidAGS  = "";
        String db_uniqueguidXML = "";
    public AGS_ClientDb (string host, int port):base(host, port) {}
     
   
    public override void readAGS() {

        if (db_connect.Length == 0) {
            Console.WriteLine ("db_connect:" + db_connect);
            Console.WriteLine ("Insufficient parameters for readDatabaseAGS");
            return;
        }    
        
            using (AGS_Database ad =  new AGS_Database (db_connect)) {
                ad.setUniqueGUID_AGS(db_uniqueguidAGS);
                ags_data = ad.readAGS();
            }

           
        }   
   
    public override void saveAGS() {
        base.saveAGS();   
        if (db_connect.Length == 0) {
                Console.WriteLine ("Insufficient parameters for saveDatabaseAGS");
                return;
            } else { 
                using (AGS_Database ad =  new AGS_Database (db_connect)) {
                ad.setUniqueGUID_AGS(db_uniqueguidAGS);
                ad.saveAGS();
                }
            }
        status = enumStatus.XMLSaved;  
    }
    public override void saveXML() {
        base.saveXML();

        if (db_connect.Length == 0) {
            Console.WriteLine ("Insufficient parameters for saveDatabaseXML");
            return;
        }   else { 
                using (AGS_Database ad =  new AGS_Database (db_connect)) {
                ad.setUniqueGUID_XML(db_uniqueguidXML);
                ad.saveXML();
                }
            }
        status = enumStatus.XMLSaved; 
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
