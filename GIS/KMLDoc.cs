// https://developers.google.com/kml/documentation/kml_tut
// https://stackoverflow.com/questions/23594997/kml-document-inside-document
 // using System;
using System.Xml.Serialization;
using System.Text;
using System.Xml.Linq;
using ge_repository.Models;
using System.Collections.Generic;

namespace ge_repository.spatial {

[XmlRoot(Namespace="http://www.opengis.net/kml/2.2", 
        ElementName = "kml")]
public class KMLDoc {

public Document Document {get;set;} = new Document();


}

public class Document {
    public string name{get;set;}
     [XmlElement("Folder")]public List<Folder> Folders {get;set;} = new List<Folder>();
}

public class Folder { 
    [XmlElement("name")] public string name {get;set;}

    [XmlElement("Placemark")] public List<Placemark> Placemarks {get;set;}
}

public class Placemark {
    public string name {get;set;}
    public string description {get;set;} 
    public Point Point {get;set;}
    public Placemark(){}
    public Placemark(string Name, string Description, string Coordinates) {
        name = Name;
        description = Description;
        Point = new Point {coordinates = Coordinates};

    }
}

public class Point {
    public string coordinates {get;set;} 
}

}
