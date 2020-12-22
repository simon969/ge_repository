using ge_repository.Models;

namespace ge_repository.DAL {

public class ge_projection_factors {

// ellipsoid axis dimenstions (a & b) in meters
public double a {get;set;}
public double b {get;set;}

//  eastings (e0) and northings (n0) of false origin in meters; _
public double e0 {get;set;}
public double n0 {get;set;}
// central meridian scale factor (f0)
public double f0 {get;set;}

// Latitude (PHI0) and Longitude (LAM0) of flase origine in decimal degrees
public double PHI0 {get;set;}
public double LAM0 {get;set;}

}

public class ge_conversion {
public ge_projection_factors pf {get;set;}

//Calculated Longitude, Latitude and Height
public double LAM {get;set;}
public double PHI {get;set;}
public double H {get;set;}

// Calculated XYZ coordinates
public double X{get;set;} 
public double Y{get;set;} 
public double Z{get;set;}

// Calculated Eastings. Northings and Level
public double East{get;set;} 
public double North{get;set;} 
public double Level{get;set;} 

//Set appropriate error value to determine if value calculated Ok
public double errorValue{get;set;}


/* '********************************************************************************************* _
 THE FUNCTIONS IN THIS MODULE ARE WRITTEN TO BE "STAND ALONE" WITH AS LITTLE DEPENDANCE _
 ON OTHER FUNCTIONS AS POSSIBLE.  THIS MAKES THEM EASIER TO COPY TO OTHER VB APPLICATIONS. _
 WHERE A FUNCTION DOES CALL ANOTHER FUNCTION THIS IS STATED IN THE COMMENTS AT THE START OF _
 THE FUNCTION. _
 *********************************************************************************************
 */
public double XYZ_to_Lat(double X, double Y, double Z){
    if (pf == null) {
       return errorValue;
    }
        return ge_UTMconversion.XYZ_to_Lat (X,Y,Z, pf.a, pf.b);
}


public double XYZ_to_Long(double X, double Y) {
        return ge_UTMconversion.XYZ_to_Long(X,Y);
}

public double XYZ_to_H(double X, double Y, double Z) {
    if (pf == null) {
       return errorValue;
    }    
        return ge_UTMconversion.XYZ_to_H(X,Y,Z,pf.a,pf.b);
}

public double Lat_Long_H_to_X(double PHI, double LAM, double H) {
    if (pf == null) {
       return errorValue;
    }
        return ge_UTMconversion.Lat_Long_H_to_X(PHI,LAM,H,pf.a,pf.b);
}
public double Lat_Long_H_to_Y(double PHI, double LAM, double H) {
    if (pf == null) {
       return errorValue;
    }
        return ge_UTMconversion.Lat_Long_H_to_Y(PHI,LAM, H, pf.a, pf.b);
}

public double Lat_H_to_Z(double PHI, double H) {
    if (pf == null) {
       return errorValue;
    }
        return ge_UTMconversion.Lat_H_to_Z (PHI,H,pf.a,pf.b);
}

public double Lat_Long_to_East(double PHI, double LAM) {
 if (pf == null) {
       return errorValue;
    }
        return ge_UTMconversion.Lat_Long_to_East (PHI, LAM, pf.a, pf.b, pf.e0, pf.f0, pf.PHI0, pf.LAM0);
}
public double Lat_Long_to_North(double PHI, double LAM) {
if (pf == null) {
       return errorValue;
    }
        return ge_UTMconversion.Lat_Long_to_North (PHI,LAM,pf.a, pf.b, pf.e0, pf.n0, pf.f0, pf.PHI0, pf.LAM0);
}

public  double Lat_Long_to_EastNorth(double PHI, double LAM) {
 if (pf == null) {
       return errorValue;
    }
        this.East = ge_UTMconversion.Lat_Long_to_East (PHI, LAM, pf.a, pf.b, pf.e0, pf.f0, pf.PHI0, pf.LAM0);
        this.North = ge_UTMconversion.Lat_Long_to_North (PHI,LAM,pf.a, pf.b, pf.e0, pf.n0, pf.f0, pf.PHI0, pf.LAM0);
        return 1;

}

public double E_N_to_Lat(double East, double North) {
if (pf == null) {
       return errorValue;
    }
        return ge_UTMconversion.E_N_to_Lat (East,North,pf.a, pf.b, pf.e0, pf.n0, pf.f0, pf.PHI0, pf.LAM0);
}

public double E_N_to_Long(double East, double North) {
if (pf == null) {
       return errorValue;
    }
        return ge_UTMconversion.E_N_to_Long (East,North,pf.a, pf.b, pf.e0, pf.n0, pf.f0, pf.PHI0, pf.LAM0);
}
public double E_N_to_LatLong(double East, double North) {
if (pf == null) {
       return errorValue;
    }
        this.PHI = ge_UTMconversion.E_N_to_Long (East,North,pf.a, pf.b, pf.e0, pf.n0, pf.f0, pf.PHI0, pf.LAM0);
        this.LAM = ge_UTMconversion.E_N_to_Lat (East,North,pf.a, pf.b, pf.e0, pf.n0, pf.f0, pf.PHI0, pf.LAM0);
        return 1;
}
public double Lat_Long_to_C(double PHI, double LAM) {
if (pf == null) {
       return errorValue;
    }
        return ge_UTMconversion.Lat_Long_to_C (PHI,LAM,pf.LAM0,pf.a,pf.b,pf.f0);
}

public double E_N_to_C(double East, double North) {
if (pf == null) {
       return errorValue;
    }
       return ge_UTMconversion.E_N_to_C (East,North,pf.a,pf.b,pf.e0,pf.n0,pf.f0,pf.PHI0);
}

public double  Lat_Long_to_LSF(double PHI, double LAM) {
if (pf == null) {
       return errorValue;
    }
       return ge_UTMconversion.Lat_Long_to_LSF(PHI, LAM, pf.LAM0, pf.a, pf.b, pf.f0);
}

public double E_N_to_LSF(double East, double North) {
if (pf == null) {
       return errorValue;
    }
       return ge_UTMconversion.E_N_to_LSF(East, North, pf.a, pf.b, pf.e0, pf.n0, pf.f0, pf.PHI0);
}

public double E_N_to_t_minus_T(double AtEast, double AtNorth, double ToEast, double ToNorth) {
if (pf == null) {
       return errorValue;
    }
       return ge_UTMconversion.E_N_to_t_minus_T(AtEast, AtNorth, ToEast, ToNorth, pf.a, pf.b, pf.e0, pf.n0, pf.f0, pf.PHI0);
}

public double TrueAzimuth(double AtEast, double AtNorth, double ToEast, double ToNorth) {
if (pf == null) {
       return errorValue;
    }
       return ge_UTMconversion.TrueAzimuth(AtEast, AtNorth, ToEast, ToNorth, pf.a, pf.b, pf.e0, pf.n0, pf.f0, pf.PHI0);
}

}
public class ge_coords {
      public double coord_x {get;set;}
      
      public double coord_y {get;set;}
     
      public double coord_z {get;set;}
   public ge_coords(){}
   public ge_coords (ge_data d) {
      
      coord_x = d.locX.Value;
      coord_y = d.locY.Value;
      coord_z = d.locZ.Value;
   
   }
}
public class ge_projection_transform {

      public double trans_x {get;set;}
      
      public double trans_y {get;set;}
     
      public double trans_z {get;set;}

      public double scale_change {get;set;}

      public double rot_x {get;set;}
      public double rot_y {get;set;}
      public double rot_z {get;set;}

}

public class WGS84_to_OSGB : ge_projection_transform {

   public WGS84_to_OSGB () {
      // translation parallel to X =	-446.448
      trans_x = -446.448;
      // translation parallel to Y =	125.157
      trans_y = 125.157;
      // translation parallel to Z =	-542.060
      trans_z = -542.060;

      // scale change =	20.4894
      scale_change = 20.4894;

      // rotation about X =	-0.1502
      rot_x = -0.1502;
      // rotation about Y =	-0.2470
      rot_y = -0.2470;
      // rotation about Z =	-0.8421
      rot_z = -0.8421;

   }
}

public class OSGB_to_WGS84 : ge_projection_transform {

   public OSGB_to_WGS84 () {

   // translation parallel to X =	446.448
   trans_x = 446.448;
   
   // translation parallel to Y =	-125.157
   trans_y = 125.157;

   // translation parallel to Z =	542.060
   trans_z = 542.060;
   
   // scale change =	-20.4894
   scale_change = -20.4894;

   // rotation about X =	0.1502
   rot_x = 0.1502;

   // rotation about Y =	0.2470
   rot_y = 0.2470;

   // rotation about Z =	0.8421
   rot_z = 0.8421;



   }
}
public class HelmertTransform {
   public ge_projection_transform _t;
   public HelmertTransform (ge_projection_transform transform) {
      _t = transform;
   }  

   public ge_coords Transform(ge_coords _from) {

         ge_coords _to = new ge_coords();

         _to.coord_x = ge_UTMconversion.Helmert_X ( _from.coord_x,
                                                   _from.coord_y,
                                                   _from.coord_z,
                                                   _t.trans_x,
                                                   _t.rot_y,
                                                   _t.rot_z,
                                                   _t.scale_change ) ;

         _to.coord_y = ge_UTMconversion.Helmert_Y ( _from.coord_x,
                                                   _from.coord_y,
                                                   _from.coord_z,
                                                   _t.trans_y,
                                                   _t.rot_x,
                                                   _t.rot_z,
                                                   _t.scale_change ) ;

         _to.coord_z = ge_UTMconversion.Helmert_Z ( _from.coord_x,
                                                   _from.coord_y,
                                                   _from.coord_z,
                                                   _t.trans_z,
                                                   _t.rot_x,
                                                   _t.rot_y,
                                                   _t.scale_change );
         return _to;
   }



} 
public class Transform_WGS84_and_OSGB {

   public ge_data wg84 {get;set;}
   public ge_data osg36 {get;set;}
   public string errorMsg {get;set;}
   public Transform_WGS84_and_OSGB(){}

   
   public ge_data WGS84_Lat_Long_Height_to_OSGB_East_North(double _lat, double _long, double _height) {
         
      ge_data _wg84 =  new ge_data();

      _wg84.locLatitude = _lat;
      _wg84.locLongitude = _long;
      _wg84.locHeight = _height;
      
      return WGS84_Lat_Long_Height_to_OSGB_East_North (_wg84);
   
   }

   public ge_data WGS84_Lat_Long_Height_to_OSGB_East_North (ge_data _wg84) { 
      
      // Step 1 Convert WGS84 Latitude, longitude and Ellipsoidal height to WGS84 Cartesian XYZ		
      
      wg84 = _wg84;

      ge_projectionWGS84 pwg84 =  new ge_projectionWGS84(wg84);

      if (!pwg84.calcXYZ_fromLatLong()){
         errorMsg ="Error pwg84.calcXYZ_fromLatLong()";
         return null;
      };


      // Step 2 Apply Helmert Datum Transformation (WGS84 to OSGB36)								

      ge_projection_transform t = new WGS84_to_OSGB();
      HelmertTransform ht = new HelmertTransform(t);
      ge_coords wg84_coords = new ge_coords(wg84);
      ge_coords osgb_coords = ht.Transform(wg84_coords);

      // Step 3 Convert OSGB36 Cartesian XYZ to OSGB36 Latitude, longitude and approx ODN height								

      osg36 =  new ge_data();
      osg36.locX = osgb_coords.coord_x;
      osg36.locY = osgb_coords.coord_y;
      osg36.locZ = osgb_coords.coord_z;

      ge_projectionOSGB36 posgb36=  new ge_projectionOSGB36(osg36);

      if (!posgb36.calcLatLongH_fromXYZ()) {
         errorMsg ="Error posgb36.calcLatLongH_fromXYZ()";
         return null;
      };
      
      // Step 4 Convert OSGB36 Latitude and longitude to OSGB36 easting and northing								

      if (!posgb36.calcEN_fromLatLong()) {
         errorMsg ="Error posgb36.calcEN_fromLatLong()";
         return null;
      }

      return osg36;
   }
   public ge_data OSGB_East_North_to_WGS84_Lat_Long_Height(double _east, double _north, double _height) {
      
      ge_data _osgb =  new ge_data();
      _osgb.locEast = _east;
      _osgb.locNorth = _north;
      _osgb.locHeight = _height;

      return OSGB_East_North_to_WGS84_Lat_Long_Height(_osgb);
   }

   public ge_data OSGB_East_North_to_WGS84_Lat_Long_Height(ge_data _osg36) {

      // Step 1 Convert OSGB36 easting, northing and height to OSGB36 latitude and longitude		
      
      osg36 = _osg36;

      ge_projectionOSGB36 posgb36 =  new ge_projectionOSGB36(osg36);
      
      if (!posgb36.calcLatLong_fromEN()) {
         errorMsg = "Error: posgb36.calcLatLong_fromEN()";
         return null;
      }

      // Step 2 Convert OSGB36 latitude, longitude and approx ODN height to OSGB36 cartesian XYZ								
      
      if (!posgb36.calcXYZ_fromLatLong()){
         errorMsg = "Error: posgb36.calcXYZ_fromLatLong()";
         return null;
      }

      // Step 3 Apply Helmert Datum Transformation (OSGB36 to WGS84)
      ge_projection_transform t = new OSGB_to_WGS84();
      HelmertTransform ht = new HelmertTransform(t);
      ge_coords osg36_coords = new ge_coords(osg36);
      ge_coords wg84_coords = ht.Transform(osg36_coords);
      wg84.locX = wg84_coords.coord_x;
      wg84.locY = wg84_coords.coord_y;
      wg84.locZ = wg84_coords.coord_z;

      // Step Convert WGS84 Cartesian XYZ to WGS84 Latitude, longitude and Ellipsoidal height		
      
      ge_projectionWGS84 pwg84 =  new ge_projectionWGS84(wg84);
      if (!pwg84.calcLatLongH_fromXYZ()){
         errorMsg = "Error: pwg84.calcLatLongH_fromXYZ()";
         return null;
      };
      
      return wg84;				

   }


   }

}





