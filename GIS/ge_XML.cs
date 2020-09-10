// https://developers.google.com/kml/documentation/kml_tut
// https://stackoverflow.com/questions/23594997/kml-document-inside-document
 // using System;
 using System.Xml;
 using System.Text;
 using System.Xml.Linq;
 using System.Xml.Serialization;
 using System.IO; 
 using ge_repository.Models;
 using System.Collections.Generic;
 
namespace ge_repository.spatial {
  
   public class ge_XML : _ge_XML {
      private string constContentType = "text/xml";

   public static class geXMLContainer {
         public static string constProject = "Project"; 
         public static string constData = "Data"; 
         public static string constGroup = "Group";
         public static string constGEXML = "geXML";
   }
   public ge_XML() {
      _rootNode = _doc.CreateElement(geXMLContainer.constGEXML);
      _doc.AppendChild (_rootNode);
          
      XmlDeclaration xmldecl;
      xmldecl = _doc.CreateXmlDeclaration( "1.0","UTF-8", null);  
      _doc.InsertBefore(xmldecl, _rootNode); 
   }
  
   public XmlNode createChild(string name, string id, XmlNode parent) {
      try {
         
         XmlNode child = _doc.CreateElement(name);
         
         if (!string.IsNullOrEmpty(id)) {
         XmlAttribute Id = _doc.CreateAttribute("Id");
         Id.Value = id;
         child.Attributes.Append(Id);
         }

         if (parent==null) {
            parent = _rootNode;
         }
         
         parent.AppendChild (child);

        return child;

      } catch {
         return null;
      }
   }
   public bool write(ge_group group, XmlNode parent) {
      
      write_base(group, parent);
      write_location (group, parent);
      
      try {
         AddNode("Id",group.Id.ToString(), parent);
         AddNode("name",group.name.ToString(), parent);

        return true; 
      } catch {
         return false; 
      }
       
   }
   
   public bool write(ge_data data, XmlNode parent) {
      
      write_base(data, parent);
      write_location (data, parent);
      
      try {
         AddNode ("Id",data.Id.ToString(), parent);
         AddNode ("filename",data.filename, parent);
         AddNode ("filesize",data.filesize.ToString(), parent);
         AddNode ("fileext", data.fileext, parent);
         AddNode ("filetype", data.filetype, parent);
         AddNode ("filedate",data.filedate.ToString(), parent);
         AddNode ("description", data.description,parent);
         AddNode ("keywords", data.keywords, parent);
         AddNode ("projectId",data.projectId.ToString(),parent);
         AddNode ("cstatus",data.cstatus.ToString(), parent);
         AddNode ("pstatus",data.pstatus.ToString(), parent);
         AddNode ("qstatus",data.qstatus.ToString(), parent);
         AddNode ("version",data.version, parent);
         AddNode ("vstatus",data.vstatus.ToString(), parent);
         return true; 
      } catch {
         return false; 
      }
       
   }
   public bool write(ge_project project, XmlNode parent) {

      write_base (project, parent);
      write_location (project, parent);
      
      try {
         AddNode("Id",project.Id.ToString(), parent);
         AddNode("keywords",project.keywords, parent);
  
        return true; 
      } catch {
         return false; 
      }
       
   }
   private bool write_base(_ge_base b, XmlNode parent) {
     try {  
         AddNode("createdId",b.createdId.ToString(), parent);
         AddNode("createdDT",b.createdDT.ToString(), parent);
         AddNode("editedId",b.editedId.ToString(), parent);
         AddNode("editedDT",b.editedDT.ToString(), parent);
         AddNode("operations",b.operations, parent);
       return true; 
      } catch {
         return false; 
      }
   }

   private bool write_location(_ge_location loc, XmlNode parent) {
       try {  
         AddNode("locName",loc.locName, parent);
         AddNode("locAddress",loc.locAddress, parent);
         AddNode("locPostCode",loc.locPostcode, parent);
         AddNode("locMapReference",loc.locMapReference, parent);
         AddNode("locEast",loc.locEast.ToString(), parent);
         AddNode("locNorth",loc.locNorth.ToString(), parent);
         AddNode("locLevel",loc.locLevel.ToString(), parent);
         AddNode("datumProjection",loc.datumProjection.ToString(), parent);
         AddNode("locLatitude",loc.locLatitude.ToString(), parent);
         AddNode("locLongitude",loc.locLongitude.ToString(), parent);
         AddNode("locHeight",loc.locHeight.ToString(), parent);
         AddNode("folder",loc.folder, parent);
         return true; 
      } catch {
         return false; 
      }
   }
   private bool write_object (object o, string name, XmlNode parent) {
      try {
         return true;
      } catch {
         return false;
      }
   }
   private bool write_object (object o, XmlSerializer serializer, XmlNode parent) {
      try {
         StringWriter writer = new StringWriter();
         serializer.Serialize(writer, o);
         
         return true;
      } catch {
         return false;
      }
   }
}

}

   