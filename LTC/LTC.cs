
using System;
using System.Collections.Generic;
using ge_repository.ESRI;
using System.Linq;

namespace ge_repository.LowerThamesCrossing {
    public static class LTC {

            public static System.TimeZoneInfo ESRIzone {get;set;} = getTimeZoneInfo("UTC");
            public static System.TimeZoneInfo gINTzone {get;set;}= getTimeZoneInfo("Greenwich");
    
    public static TimeZoneInfo getTimeZoneInfo(string s1) {
    
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
        
    public static DateTime? ConvertBST_to_UTC(DateTime? dateTime) {
        
        TimeZoneInfo destinationZone =  TimeZoneInfo.FindSystemTimeZoneById("UTC");
        TimeZoneInfo sourceZone = TimeZoneInfo.FindSystemTimeZoneById("British Summer Time");

        return TimeZoneInfo.ConvertTime(dateTime.Value,  sourceZone, destinationZone);

    }
    public static DateTime? gINTDateTime (DateTime? esriDateTime) {
            //    Console.WriteLine("{0} {1} is {2} local time.",
            //    esriDateTime,
            //    ESRIzone.IsDaylightSavingTime(esriDateTime) ? ESRIzone.DaylightName : ESRIzone.StandardName,
            //    TimeZoneInfo.ConvertTime(esriDateTime, ESRIzone, gINTzone));

        if (esriDateTime!=null) {
        return TimeZoneInfo.ConvertTime(esriDateTime.Value, ESRIzone, gINTzone);
        } 
        return null;
    }

    public static  DateTime? EsriTDateTime (DateTime? gintDateTime) {
        //    Console.WriteLine("{0} {1} is {2} local time.",
            //    esriDateTime,
            //    gINTzone.IsDaylightSavingTime(gintDateTime) ? gINTzone.DaylightName : gINTzone.StandardName,
            //    TimeZoneInfo.ConvertTime(esriDateTime, ESRIzone, gINTzone));
        if (gintDateTime!=null) {
        return TimeZoneInfo.ConvertTime(gintDateTime.Value, gINTzone, ESRIzone);
        }
        return null;
    }
    public static int convertToInt16( string numericStr, string Remove, int ErrValue )  {
            String s1="";

            if (!String.IsNullOrEmpty(Remove)) {
                    s1 = numericStr.Replace(Remove,"");
            } else {
                    s1 = numericStr;
            }

            try {
                return  Convert.ToInt16( s1 );
            }
            catch( Exception e ) {
                Console.Write (e.Message);
                return ErrValue;
            }

        }
        public static string IfOther(string s1, string other) {
        
        if (s1==null && other==null) {
            return "";
        }
        
        if (s1==null && other!=null) {
            return other.Replace ("_"," ");
        }
        
        if (s1!="Other") {
            return s1.Replace("_"," ");
        } 

        return other.Replace("_"," ");
        
        }
    }
}
