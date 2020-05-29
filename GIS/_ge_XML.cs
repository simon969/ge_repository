 using System;
 using System.IO;
 using System.Text;
 using System.Xml;
 using System.Xml.Linq;
 using System.Xml.Serialization;
 using ge_repository.Models; 

    namespace ge_repository.spatial {
    public abstract class _ge_XML {
        public XmlDocument _doc {get;set;}
        public XmlNode  _rootNode{get;set;}
      
        public string _href {get;set;}    
    public _ge_XML() {
        _doc = new XmlDocument();
     }  

    public static string MakeXml() {
    XNamespace xmlns = "http://a9.com/-/spec/opensearch/1.1/";
    XNamespace moz = "http://www.mozilla.org/2006/browser/search/";
    string domain = "http://localhost";
    string searchTerms = "abc";
    var doc = new XDocument(
        new XDeclaration("1.0", "UTF-8", "yes"),
        new XElement(
            xmlns + "OpenSearchDescription",
            new XElement(xmlns + "ShortName", "Search"),
            new XElement(
                xmlns + "Description",
                String.Format("Use {0} to search.", domain)),
            new XElement(xmlns + "Contact", "contact@sample.com"),
            new XElement(
                xmlns + "Url",
                new XAttribute("type", "text/html"),
                new XAttribute("method", "get"),
                new XAttribute(
                    "template",
                    String.Format("http://{0}/Search.aspx?q={1}",domain,searchTerms))),
            new XElement(
                moz + "SearchForm",
                String.Format("http://{0}/Search.aspx", domain)),
            new XElement(
                xmlns + "Image",
                new XAttribute("height", 16),
                new XAttribute("width", 16),
                new XAttribute("type", "image/x-icon"),
                String.Format("http://{0}/favicon.ico", domain))));
    return doc.ToString(); // If you _must_ have a string
    }
        public override string ToString(){
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                _doc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }
    
       public static bool ValidLatitudeLongitude(_ge_location ge_loc) {
      
      if  (ge_loc.locLongitude==null) {
         return false;
      }
      if (ge_loc.locLatitude==null) {
         return false;
      }

      return true;
    
   }
    public static bool ValidHeight(_ge_location ge_loc) {
      if (ge_loc.locHeight == null) {
         return false;
      } 
      return true;
    }
     public XmlNode AddNode(string name, string innertext, XmlNode parent) {
        XmlNode n = _doc.CreateElement(name);
        n.InnerText = innertext;
        return parent.AppendChild (n);
    }

}
   
}