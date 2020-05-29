using System;

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
public double Lat_Long_H_to_Y(double PHI, double LAM, double H, double a, double b) {
    if (pf == null) {
       return errorValue;
    }
        return ge_UTMconversion.Lat_Long_H_to_Y(PHI,LAM, H, pf.a, pf.b);
}

public double Lat_H_to_Z(double PHI, double H, double a, double b) {
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


}

