 using System;
 using System.Collections.Generic;
 using System.IO;
 using System.Text;
 using System.Xml;
 using System.Xml.Linq;
 using System.Xml.Serialization;
 using ge_repository.Models; 

namespace ge_repository.spatial {

[XmlRoot("geXML")]
 public class geXML {
     public geXML() {projects = new List<project>();}
     [XmlElement("project")]
     public List<project> projects {get;set;}
      public data FindFirstWhereKeywordsContains(string[] values) {
        
        foreach (project p in projects) {
            List<data> a = p.data_list;
            foreach (string v in values) {
                List<data> b =  a.FindAll(m=>m.keywords.Contains(v));
                a = b ;
            }
            if (a.Count>0){
                return a[0];
            }
       }
            return null;    
    
    }
 }

public class project {
    public project() {data_list = new List<data>();}
    public Guid Id {get;set;}
    [XmlElement("data")]
   
    public List<data> data_list {get;set;}
    
   
}

public class data {
        public Guid Id {get;set;}
        public string keywords {get;set;}
}



}




