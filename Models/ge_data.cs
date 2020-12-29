using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;
using ge_repository.Authorization;

using System.Net;

namespace ge_repository.Models {

    [Table("ge_data")]
    public partial class ge_data: _ge_location {
        public Guid Id {get;set;}
        [Display(Name = "File Name")] public string filename {get;set;}
        [Display(Name = "File Size (bytes)")]public long filesize {get;set;}
        [Display(Name = "File Extension")][StringLength(8)] public string fileext {get;set;}
        [Display(Name = "File Content Type")][StringLength(128)] public string filetype {get;set;}
        [Display(Name = "Encoding")] [StringLength(6)] public string encoding {get;set;}
        [Display(Name = "Last Modified Date")] public DateTime filedate {get;set;}
        [Display(Name = "Description")] public string description {get;set;}
        [Display(Name = "Keywords")] public string keywords{get;set;}
        [Display(Name = "Project Id")]public Guid projectId {get;set;}
        virtual public ge_project project {set;get;}
        [Display(Name = "Confidentiality")] public Constants.ConfidentialityStatus cstatus {get;set;}
        [Display(Name = "Publish Status")] public Constants.PublishStatus pstatus {get;set;}
        [Display(Name = "Qualitative Status")] public Constants.QualitativeStatus qstatus {get;set;}
        [Display(Name = "Version")] [StringLength(64)] public string version{get;set;} 
        [Display(Name = "Version Status")] public Constants.VersionStatus vstatus {get;set;}

        
       [ForeignKey("Id")]  
        public virtual ge_data_big data { get; set; }   
        
        public string GetContentType()
        {
            var types = new ge_MimeTypes();
            var ext = Path.GetExtension(filename).ToLowerInvariant();
            String type = types[ext];
            
            if (filetype!=null) {
                return filetype;
            }
            
            return type;
          
        }
        public string GetContentFieldName() {
            
            string m_type = GetContentType();

            if (m_type == "text/plain") return "data_string";
            if (m_type == "text/xml") return "data_xml";
               
            return "data_string";
       } 
       
        public string GetExtention()
        {
            return Path.GetExtension(filename).ToLowerInvariant();
        }
        public Encoding GetEncoding() {
            if (encoding =="utf-7") return Encoding.UTF7;
            if (encoding =="utf-8") return Encoding.UTF8;
            if (encoding =="utf-16") return Encoding.Unicode;
            if (encoding =="ascii") return Encoding.ASCII;
            return null;   
        }
        public Boolean SetEncoding(Encoding encode) {
            
            if (encode == null) {
                encoding ="raw";
                return true;
            }
            if (encode == Encoding.UTF7) {
                encoding = "utf-7";
                return true;
            }
            if (encode == Encoding.UTF8) {
                encoding = "utf-8";
                return true;
            }
            if (encode == Encoding.Unicode) {
                encoding = "utf-16";
                return true;
            }

            if (encode == Encoding.ASCII) {
                encoding ="ascii";
                return true;
            }


            return false;
        }
        }
    
        
    [Table("ge_data")]
    public partial class ge_data_big {
        public Guid Id {get;set;}
        public Byte[] data_binary {get;set;}
        public string data_string {get;set;}
        public string data_xml {get;set;}
        public ge_data data {get;set;}
        
       public MemoryStream getMemoryStream(Encoding encode) {

           if (data_binary != null) {
               return new MemoryStream(data_binary); 
           }
           
           if (data_string !=null && encode !=null ) {
               return new MemoryStream(encode.GetBytes(data_string)); 
           }

           if (data_xml !=null && encode !=null) {
              return new MemoryStream(encode.GetBytes(data_xml));
           }

           return null;
       }

       public void toStream (Stream response, Encoding encode) {
            using (Stream responseStream = getMemoryStream(encode))
                {
                // response.AllowWriteStreamBuffering= false;   // to prevent buffering 
                byte[] buffer = new byte[1024]; 
                int bytesRead = 0; 
                while ((bytesRead = responseStream.Read(buffer, 0, buffer.Length)) > 0)  
                { 
                        response.Write(buffer, 0, bytesRead); 
                }
                }
       }

       public String getString(Encoding encode = null, Boolean removeBOM = false) {
            
            //check for BOM at begining of xslt string from file
            string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            
            if (data_binary != null && encode==Encoding.Unicode) {
               return System.Text.Encoding.Unicode.GetString(data_binary);
            }
            
            if (data_binary != null && encode==Encoding.UTF8) {
               return System.Text.Encoding.UTF8.GetString(data_binary);
            }
            
            if (data_binary != null && encode == null) {
               return System.Text.Encoding.Default.GetString(data_binary);
            }
            
            if (data_string !=null ) {
               return data_string; 
            }

           if (data_xml !=null ) {
                
                if (data_xml.StartsWith(_byteOrderMarkUtf8) && removeBOM == true)   {
                return data_xml.Remove(0, _byteOrderMarkUtf8.Length);
                }

                return data_xml;
            }

           return null;
       }
              
    }
       
   
    
    }
    
 


 

