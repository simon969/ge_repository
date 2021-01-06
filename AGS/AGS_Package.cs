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


public class AGS_Package {
        
    public String data_series;
    public String data_dictionary;
    public String data_ags;
    public String data_xml;
    public ContentType data_type; 
    public  static String FILE_START = "[file_start]";
    public  static String FILE_END = "[file_end]";
    public  static String AGS_START = "[ags_start]";
    public  static String AGS_END = "[ags_end]";
    public  static String XML_START = "[xml_start]";
    public  static String XML_END = "[xml_end]";
    public  static String DICTIONARY_START = "[dictionary_start]";
    public  static String DICTIONARY_END = "[dictionary_end]";
    public  static String DATASTRUCTURE_START = "[datastructure_start]";
    public  static String DATASTRUCTURE_END = "[datastructure_end]";
    public static String EMPTY_STRING;
    public enum ContentType {
        AGS,
        XML
    }
   
    public AGS_Package(ContentType type, String contents) {
        
        data_type = type;
        
        if (data_type == AGS_Package.ContentType.AGS) {
            getAGSData(contents);
            getAGSDataStructureSeries(contents);
            getAGSDictionary(contents);
        }
        
        if (data_type == AGS_Package.ContentType.XML) {
             getXMLData(contents);
        }
    }
    public AGS_Package (String ags, String dictionary, String datastructure) {
        data_type = AGS_Package.ContentType.AGS;
        data_ags = ags;
        data_dictionary = dictionary;
        data_series = datastructure; 
                
    }   
     public AGS_Package (String xml) {
        data_type = AGS_Package.ContentType.XML;
        data_xml = xml;
    } 
    public String getContents () {
        try {
            if (data_type==AGS_Package.ContentType.AGS) {
                return getContentsAGS();
            }
        
            if (data_type==AGS_Package.ContentType.XML) {
                return getContentsXML();
            }
        
            throw new Exception("AGS Package content type not given");
        
        } catch (Exception e){
            Console.Write (e.Message);
            return EMPTY_STRING;
        } 
    }
    public String getContentsAGS(){
            
        StringBuilder sb = new StringBuilder();
            
            sb.Append(FILE_START);
            sb.Append(Environment.NewLine);
            sb.Append(AGS_Package.DICTIONARY_START);
                      sb.Append(data_dictionary);
            sb.Append(AGS_Package.DICTIONARY_END);
            sb.Append(Environment.NewLine);
            sb.Append(AGS_Package.DATASTRUCTURE_START);
                     sb.Append(data_series);
            sb.Append(AGS_Package.DATASTRUCTURE_END);
            sb.Append(Environment.NewLine);
            sb.Append(AGS_Package.AGS_START);
                     sb.Append(data_ags);
            sb.Append(AGS_Package.AGS_END);
            sb.Append(Environment.NewLine);
            sb.Append(AGS_Package.FILE_END);
            sb.Append(Environment.NewLine);
            
        return sb.ToString();
            
    }
     public String getContentsXML(){
            
        StringBuilder sb = new StringBuilder();
            
            sb.Append(FILE_START);
            sb.Append(Environment.NewLine);
            sb.Append(AGS_Package.XML_START);
                    sb.Append(data_xml);
            sb.Append(AGS_Package.XML_END);
            sb.Append(Environment.NewLine);
            sb.Append(AGS_Package.FILE_END);
            sb.Append(Environment.NewLine);
            
        return sb.ToString();
            
    }
    public int CheckContents(ContentType type) {
        int retvar = 0;
        
        if (type== ContentType.AGS) {
        if (string.IsNullOrEmpty(data_ags)) retvar =+ -1;
        if (string.IsNullOrEmpty(data_dictionary)) retvar =+ -1;
        if (string.IsNullOrEmpty(data_series)) retvar =+ -1;
        return retvar;
        }
        
        if (type== ContentType.XML) {
        if (string.IsNullOrEmpty(data_xml)) retvar =+ -1;
        return retvar;
        }
        
        return retvar;
    }
    public Boolean HasAGSData() {
            return !string.IsNullOrEmpty(data_ags);
    }
    public Boolean HasDataStructureSeries() {
            return !string.IsNullOrEmpty(data_series);
    }
    public Boolean HasDataDictionary() {
            return !string.IsNullOrEmpty(data_dictionary);
    }
    public Boolean HasXMLData() {
            return !string.IsNullOrEmpty(data_xml);
    }
    private Boolean getXMLData(String contents) {
        int from = contents.IndexOf(XML_START,0)+ XML_START.Length;
        int to = contents.IndexOf(XML_END,0);
        int len = to - from;
        data_xml = contents.Substring(from, len);
        return !string.IsNullOrEmpty(data_xml);
    }
    private Boolean getAGSData(String contents) {
        int from = contents.IndexOf(AGS_START,0)+ AGS_START.Length;
        int to = contents.IndexOf(AGS_END,0);
        data_ags = contents.Substring(from, to);
        return !string.IsNullOrEmpty(data_ags);
    }
        private Boolean getAGSDataStructureSeries(String contents) {
        int from = contents.IndexOf(DATASTRUCTURE_START,0)+ DATASTRUCTURE_START.Length;
        int to = contents.IndexOf(DATASTRUCTURE_END,0);
        data_series= contents.Substring(from, to);
        return !string.IsNullOrEmpty(data_series);
    }
    private Boolean getAGSDictionary(String contents) {
        int from = contents.IndexOf(DICTIONARY_START,0)+ DICTIONARY_START.Length;
        int to = contents.IndexOf(DICTIONARY_END,0);
        data_dictionary = contents.Substring(from, to);
        return !string.IsNullOrEmpty(data_dictionary);
    }   
    }  
}
