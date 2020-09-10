
using System;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Text;
using System.IO;

namespace ge_repository.Controllers
{
public class XmlResult : ActionResult
{
    private object objectToSerialize;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlResult"/> class.
    /// </summary>
    /// <param name="objectToSerialize">The object to serialize to XML.</param>
    public XmlResult(object objectToSerialize)
    {
        this.objectToSerialize = objectToSerialize;
    }

    /// <summary>
    /// Gets the object to be serialized to XML.
    /// </summary>
    public object ObjectToSerialize
    {
        get { return this.objectToSerialize; }
    }

    /// <summary>
    /// Serialises the object that was passed into the constructor to XML and writes the corresponding XML to the result stream.
    /// </summary>
    /// <param name="context">The controller context for the current request.</param>
    public override async Task ExecuteResultAsync(ActionContext context)
    {
        if (this.objectToSerialize != null)
        {
            context.HttpContext.Response.Clear();
            var xs = new System.Xml.Serialization.XmlSerializer(this.objectToSerialize.GetType());
            context.HttpContext.Response.ContentType = "text/xml";
           
            
        }
    }
}

public sealed class XmlActionResult : ActionResult
{
    private readonly XmlDocument _document;

    public Formatting Formatting { get; set; } = Formatting.None;
    public string MimeType { get; set; } = "text/xml";

    public XmlActionResult(XmlDocument document)
    {
        if (document == null)
            throw new ArgumentNullException("document");

        _document = document;

    }
    public XmlActionResult (string xml)
    {
        _document = new XmlDocument();
        _document.LoadXml(xml);
    }

    public override void ExecuteResult(ActionContext context)
    {
        var response = context.HttpContext.Response;
        response.Clear();
        response.ContentType = MimeType;

        using (var writer = new XmlTextWriter(response.Body, Encoding.UTF8) { Formatting = Formatting })
             _document.WriteTo(writer);
    }

    public String Value() {

       using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
            _document.WriteTo(xmlTextWriter);
            xmlTextWriter.Flush();
            return stringWriter.GetStringBuilder().ToString();
            }
    
    }
}

}

