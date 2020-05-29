// https://developers.google.com/kml/documentation/kml_tut
// https://stackoverflow.com/questions/23594997/kml-document-inside-document
 // using System;
 using System.Xml;
 using System.Text;
 using System.Xml.Linq;
 using ge_repository.Models;

namespace ge_repository.spatial {
  
   public class ge_KML : _ge_XML {
      public XmlNode currentContainer{get;set;}
      private XNamespace xmlns = "http://www.opengis.net/kml/2.2";
      private string constContentType = "application/vnd.google-earth.kml+xml";
 
   public static class kmlContainer {
         public static string DOCUMENT = "Document"; 
         public static string FOLDER = "Folder"; 
         public static string PLACEMARK = "Placemark";
         public static string NETWORKLINK ="NetworkLink"; 
         public static string KML = "kml";
         public static string EXTENDEDDATA ="ExtendedData";
         public static string DATA ="Data";
         public static string NAME = "name";
          public static string VALUE ="value";

         public static string DESCRIPTION ="description";
         public static string POINT = "Point";
         public static string COORDS = "coordinates";
         public static string LINK ="Link";
         public static string HREF = "href";
   }
   public ge_KML() {
      _rootNode = _doc.CreateElement(kmlContainer.KML,xmlns.NamespaceName);
      _doc.AppendChild (_rootNode);
      currentContainer = _rootNode;
      
      XmlDeclaration xmldecl;
      xmldecl = _doc.CreateXmlDeclaration( "1.0","UTF-8", null);  
      _doc.InsertBefore(xmldecl, _rootNode); 
   }
  
   public bool createContainer(string container, string name, string description, string id) {
      try {
         
         XmlNode newContainer = _doc.CreateElement(container);
         
         if (!string.IsNullOrEmpty(id)) {
         XmlAttribute Id = _doc.CreateAttribute("Id");
         Id.Value = id;
         newContainer.Attributes.Append(Id);
         }

         XmlNode Name = _doc.CreateElement(kmlContainer.NAME);
         XmlNode Description = _doc.CreateElement(kmlContainer.DESCRIPTION);
         
         Name.InnerText = name;
         Description.InnerText = description;
         
         newContainer.AppendChild (Name);
         newContainer.AppendChild(Description);
         
         currentContainer.AppendChild(newContainer);
       
         currentContainer = newContainer;

         return true;

      } catch {
         return false;
      }
   }
 
  
   public bool process_loc(_ge_location ge_loc, string description, string id) {
      try {
         
         if (!ValidLatitudeLongitude(ge_loc)) {
            return false;
         }
         
         string coordinates = ge_loc.locLongitude + "," + ge_loc.locLatitude;
        
         if (ValidHeight(ge_loc)) {
         coordinates += "," + ge_loc.locHeight;
         }
         
         XmlNode PlaceMark = _doc.CreateElement(kmlContainer.PLACEMARK);
         XmlNode Name = _doc.CreateElement(kmlContainer.NAME);
         Name.InnerText = ge_loc.locName;
         PlaceMark.AppendChild(Name);
         
         XmlNode Description = _doc.CreateElement(kmlContainer.DESCRIPTION);
         Description.InnerText = description;
         PlaceMark.AppendChild(Description);

         XmlNode Point = _doc.CreateElement(kmlContainer.POINT);
         PlaceMark.AppendChild(Point); 
         
         XmlNode Coordinates = _doc.CreateElement(kmlContainer.COORDS);
         Coordinates.InnerText = coordinates; 
         Point.AppendChild(Coordinates);
         
         XmlNode ExtendedData = _doc.CreateElement(kmlContainer.EXTENDEDDATA);
         PlaceMark.AppendChild(ExtendedData);
         
         XmlElement Data = _doc.CreateElement(kmlContainer.DATA);
         Data.SetAttribute(kmlContainer.NAME,description);
         ExtendedData.AppendChild(Data);
         
         XmlElement Value = _doc.CreateElement(kmlContainer.VALUE);
         Value.InnerText = description;
         Data.AppendChild (Value);
                 
         currentContainer.AppendChild(PlaceMark);
         return true;
      } catch {
         return false;
      }
  }

   }
}
   