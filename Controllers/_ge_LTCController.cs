using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ge_repository.Models;
using ge_repository.Authorization;
using static ge_repository.Authorization.Constants;
using ge_repository.Extensions;
using System.Data.SqlClient;
using System.Data;
using ge_repository.OtherDatabase;
using ge_repository.LowerThamesCrossing;
using ge_repository.Services;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;

namespace ge_repository.Controllers
{
   public abstract class _ge_LTCController: ge_Controller  {     
    
     public System.TimeZoneInfo ESRIzone {get;set;}
     public System.TimeZoneInfo gINTzone {get;set;}
     public List<MOND> MOND {get;set;}
     public List<MONG> MONG {get;set;}
     public List<MONV> MONV {get; set;}
     public List<POINT> POINT {get;set;} 
         public _ge_LTCController(

            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,  
            IHostingEnvironment env ,
            IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {  
            //UTC == Greenwich Mean Time
            setESRITimeZone("UTC");
            setgINTTimeZone("Greenwich Standard Time");
            
            // British Summer Time = GMT Standard Time
            // setgINTTimeZone("GMT Standard Time");
        }

        protected int convertToInt16( string numericStr, string Remove, int ErrValue )  {
            String s1="";

            if (!String.IsNullOrEmpty(Remove)) {
                    s1 = numericStr.Replace(Remove,"");
            } else {
                    s1 = numericStr;
            }

            try {
                return  Convert.ToInt16( s1 );
            }
            catch( Exception ex ) {
                return ErrValue;
            }

        }
 public TimeZoneInfo getTimeZoneInfo(string s1) {
    
    try    {

    TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(s1);
    return tz;

    }
    catch (TimeZoneNotFoundException)     {
        Console.WriteLine("The registry does not define " +  s1 +  " time zone");
        return null;
    }
    catch (InvalidTimeZoneException)
    {
        Console.WriteLine("Registry data on the " + s1 + " time zone has been corrupted.");
        return null;
    }
            
 }

 public void  setgINTTimeZone(string gintTimeZone) {
     gINTzone = getTimeZoneInfo(gintTimeZone);
 }
 public void  setESRITimeZone(string esriTimeZone) {
     ESRIzone = getTimeZoneInfo(esriTimeZone);
 }
 
 public DateTime? ConvertBST_to_UTC(DateTime? dateTime) {
    
    TimeZoneInfo destinationZone =  TimeZoneInfo.FindSystemTimeZoneById("UTC");
    TimeZoneInfo sourceZone = TimeZoneInfo.FindSystemTimeZoneById("British Summer Time");

    return TimeZoneInfo.ConvertTime(dateTime.Value,  sourceZone, destinationZone);

 }
 protected DateTime? gINTDateTime (DateTime? esriDateTime) {
        //    Console.WriteLine("{0} {1} is {2} local time.",
        //    esriDateTime,
        //    ESRIzone.IsDaylightSavingTime(esriDateTime) ? ESRIzone.DaylightName : ESRIzone.StandardName,
        //    TimeZoneInfo.ConvertTime(esriDateTime, ESRIzone, gINTzone));

    if (esriDateTime!=null) {
    return TimeZoneInfo.ConvertTime(esriDateTime.Value, ESRIzone, gINTzone);
    } 
    return null;
 }

protected DateTime? EsriTDateTime (DateTime? gintDateTime) {
    //    Console.WriteLine("{0} {1} is {2} local time.",
        //    esriDateTime,
        //    gINTzone.IsDaylightSavingTime(gintDateTime) ? gINTzone.DaylightName : gINTzone.StandardName,
        //    TimeZoneInfo.ConvertTime(esriDateTime, ESRIzone, gINTzone));
    if (gintDateTime!=null) {
    return TimeZoneInfo.ConvertTime(gintDateTime.Value, gINTzone, ESRIzone);
    }
    return null;
}
 protected List<MOND> getMONDForDeletion(List<MOND> existingMOND, List<MOND> newMOND ) {
     List<MOND> deleteMOND = new List<MOND>();
     
     foreach (MOND existM in existingMOND) {
        // var newM = newMOND.Where(m=>m.PointID == existM.PointID &&
        //                         m.MONG_DIS == existM.MONG_DIS &&
        //                         m.DateTime == existM.DateTime &&
        //                         m.MOND_TYPE == existM.MOND_TYPE ).FirstOrDefault(); 

        var newM = newMOND.Where(m=>m.PointID == existM.PointID &&
                                m.MONG_DIS == existM.MONG_DIS &&
                                m.DateTime == existM.DateTime &&
                                m.MOND_TYPE == existM.MOND_TYPE &&
                                m.ge_source == existM.ge_source &&
                                m.ge_otherId == existM.ge_otherId).FirstOrDefault();
        if (newM==null) {
            deleteMOND.Add (existM);
        }

     }
    
    if (deleteMOND.Count()>0) {
        return deleteMOND;
    }

    return null;
 }
private async Task<int> ReadAttributes<T> (List<items<T>> read, List<T> store) {
        
            if (store == null) store = new List<T>();
            
            foreach (items<T> items in read) {
                
                T item = items.attributes;
                
                if (item == null) {
                    continue; 
                }

                store.Add (item);
            }

        return store.Count();
}

protected string XmlSerializeToString<T>(List<T> list)
    
    {
        if (list.Count == 0) {
            return "";
        }

        var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        var serializer = new XmlSerializer(list[0].GetType());
        var settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.OmitXmlDeclaration = true;
        settings.ConformanceLevel = ConformanceLevel.Fragment;
        using (var stream = new StringWriter())
        using (var writer = XmlWriter.Create(stream, settings))
        {
            writer.WriteWhitespace("");
            foreach (T value in list)  {
            serializer.Serialize(writer, value, emptyNamespaces);
            }
            return stream.ToString();
        }
    }
}
}
