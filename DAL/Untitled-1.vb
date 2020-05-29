'********************************************************************************************* _
 THE FUNCTIONS IN THIS MODULE ARE WRITTEN TO BE "STAND ALONE" WITH AS LITTLE DEPENDANCE _
 ON OTHER FUNCTIONS AS POSSIBLE.  THIS MAKES THEM EASIER TO COPY TO OTHER VB APPLICATIONS. _
 WHERE A FUNCTION DOES CALL ANOTHER FUNCTION THIS IS STATED IN THE COMMENTS AT THE START OF _
 THE FUNCTION. _
 *********************************************************************************************

Function Helmert_X(X, Y, Z, DX, Y_Rot, Z_Rot, s)
'Computed Helmert transformed X coordinate.
'Input: - _
 cartesian XYZ coords (X,Y,Z), X translation (DX) all in meters ; _
 Y and Z rotations in seconds of arc (Y_Rot, Z_Rot) and scale in ppm (s).
 
'Convert rotations to radians and ppm scale to a factor
    Pi = 3.14159265358979
    sfactor = s * 0.000001
    RadY_Rot = (Y_Rot / 3600) * (Pi / 180)
    RadZ_Rot = (Z_Rot / 3600) * (Pi / 180)
    
'Compute transformed X coord
    Helmert_X = X + (X * sfactor) - (Y * RadZ_Rot) + (Z * RadY_Rot) + DX
 
End Function

Function Helmert_Y(X, Y, Z, DY, X_Rot, Z_Rot, s)
'Computed Helmert transformed Y coordinate.
'Input: - _
 cartesian XYZ coords (X,Y,Z), Y translation (DY) all in meters ; _
 X and Z rotations in seconds of arc (X_Rot, Z_Rot) and scale in ppm (s).
 
'Convert rotations to radians and ppm scale to a factor
    Pi = 3.14159265358979
    sfactor = s * 0.000001
    RadX_Rot = (X_Rot / 3600) * (Pi / 180)
    RadZ_Rot = (Z_Rot / 3600) * (Pi / 180)
    
'Compute transformed Y coord
    Helmert_Y = (X * RadZ_Rot) + Y + (Y * sfactor) - (Z * RadX_Rot) + DY
 
End Function

Function Helmert_Z(X, Y, Z, DZ, X_Rot, Y_Rot, s)
'Computed Helmert transformed Z coordinate.
'Input: - _
 cartesian XYZ coords (X,Y,Z), Z translation (DZ) all in meters ; _
 X and Y rotations in seconds of arc (X_Rot, Y_Rot) and scale in ppm (s).
 
'Convert rotations to radians and ppm scale to a factor
    Pi = 3.14159265358979
    sfactor = s * 0.000001
    RadX_Rot = (X_Rot / 3600) * (Pi / 180)
    RadY_Rot = (Y_Rot / 3600) * (Pi / 180)
    
'Compute transformed Z coord
    Helmert_Z = (-1 * X * RadY_Rot) + (Y * RadX_Rot) + Z + (Z * sfactor) + DZ
 
End Function

Function XYZ_to_Lat(X, Y, Z, a, b)
'Convert XYZ to Latitude (PHI) in Dec Degrees.
'Input: - _
 XYZ cartesian coords (X,Y,Z) and ellipsoid axis dimensions (a & b), all in meters.

'THIS FUNCTION REQUIRES THE "Iterate_XYZ_to_Lat" FUNCTION
'THIS FUNCTION IS CALLED BY THE "XYZ_to_H" FUNCTION

    RootXYSqr = Sqr((X ^ 2) + (Y ^ 2))
    e2 = ((a ^ 2) - (b ^ 2)) / (a ^ 2)
    PHI1 = Atn(Z / (RootXYSqr * (1 - e2)))
    
    PHI = Iterate_XYZ_to_Lat(a, e2, PHI1, Z, RootXYSqr)
    
    Pi = 3.14159265358979
    
    XYZ_to_Lat = PHI * (180 / Pi)

End Function

Function Iterate_XYZ_to_Lat(a, e2, PHI1, Z, RootXYSqr)
'Iteratively computes Latitude (PHI).
'Input: - _
 ellipsoid semi major axis (a) in meters; _
 eta squared (e2); _
 estimated value for latitude (PHI1) in radians; _
 cartesian Z coordinate (Z) in meters; _
 RootXYSqr computed from X & Y in meters.

'THIS FUNCTION IS CALLED BY THE "XYZ_to_PHI" FUNCTION
'THIS FUNCTION IS ALSO USED ON IT'S OWN IN THE _
 "Projection and Transformation Calculations.xls" SPREADSHEET


    V = a / (Sqr(1 - (e2 * ((Sin(PHI1)) ^ 2))))
    PHI2 = Atn((Z + (e2 * V * (Sin(PHI1)))) / RootXYSqr)
    
    Do While Abs(PHI1 - PHI2) > 0.000000001
        PHI1 = PHI2
        V = a / (Sqr(1 - (e2 * ((Sin(PHI1)) ^ 2))))
        PHI2 = Atn((Z + (e2 * V * (Sin(PHI1)))) / RootXYSqr)
    Loop

    Iterate_XYZ_to_Lat = PHI2

End Function

Function XYZ_to_Long(X, Y)
'Convert XYZ to Longitude (LAM) in Dec Degrees.
'Input: - _
 X and Y cartesian coords in meters.

    Pi = 3.14159265358979
    
'tailor the output to fit the equatorial quadrant as determined by the signs of X and Y
    If X >= 0 Then 'longitude is in the W90 thru 0 to E90 hemisphere
        XYZ_to_Long = (Atn(Y / X)) * (180 / Pi)
    End If
    
    If X < 0 And Y >= 0 Then 'longitude is in the E90 to E180 quadrant
        XYZ_to_Long = ((Atn(Y / X)) * (180 / Pi)) + 180
    End If
    
    If X < 0 And Y < 0 Then 'longitude is in the E180 to W90 quadrant
        XYZ_to_Long = ((Atn(Y / X)) * (180 / Pi)) - 180
    End If
    

End Function

Function XYZ_to_H(X, Y, Z, a, b)
'Convert XYZ to Ellipsoidal Height.
'Input: - _
 XYZ cartesian coords (X,Y,Z) and ellipsoid axis dimensions (a & b), all in meters.

'REQUIRES THE "XYZ_to_Lat" FUNCTION

'Compute PHI (Dec Degrees) first
    PHI = XYZ_to_Lat(X, Y, Z, a, b)

'Convert PHI radians
    Pi = 3.14159265358979
    RadPHI = PHI * (Pi / 180)
    
'Compute H
    RootXYSqr = Sqr((X ^ 2) + (Y ^ 2))
    e2 = ((a ^ 2) - (b ^ 2)) / (a ^ 2)
    V = a / (Sqr(1 - (e2 * ((Sin(RadPHI)) ^ 2))))
    H = (RootXYSqr / Cos(RadPHI)) - V
    
    XYZ_to_H = H
    
End Function

Function Lat_Long_H_to_X(PHI, LAM, H, a, b)
'Convert geodetic coords lat (PHI), long (LAM) and height (H) to cartesian X coordinate.
'Input: - _
 Latitude (PHI)& Longitude (LAM) both in decimal degrees; _
 Ellipsoidal height (H) and ellipsoid axis dimensions (a & b) all in meters.

'Convert angle measures to radians
    Pi = 3.14159265358979
    RadPHI = PHI * (Pi / 180)
    RadLAM = LAM * (Pi / 180)

'Compute eccentricity squared and nu
    e2 = ((a ^ 2) - (b ^ 2)) / (a ^ 2)
    V = a / (Sqr(1 - (e2 * ((Sin(RadPHI)) ^ 2))))

'Compute X
    Lat_Long_H_to_X = (V + H) * (Cos(RadPHI)) * (Cos(RadLAM))

End Function

Function Lat_Long_H_to_Y(PHI, LAM, H, a, b)
'Convert geodetic coords lat (PHI), long (LAM) and height (H) to cartesian Y coordinate.
'Input: - _
 Latitude (PHI)& Longitude (LAM) both in decimal degrees; _
 Ellipsoidal height (H) and ellipsoid axis dimensions (a & b) all in meters.

'Convert angle measures to radians
    Pi = 3.14159265358979
    RadPHI = PHI * (Pi / 180)
    RadLAM = LAM * (Pi / 180)

'Compute eccentricity squared and nu
    e2 = ((a ^ 2) - (b ^ 2)) / (a ^ 2)
    V = a / (Sqr(1 - (e2 * ((Sin(RadPHI)) ^ 2))))

'Compute Y
    Lat_Long_H_to_Y = (V + H) * (Cos(RadPHI)) * (Sin(RadLAM))

End Function

Function Lat_H_to_Z(PHI, H, a, b)
'Convert geodetic coord components latitude (PHI) and height (H) to cartesian Z coordinate.
'Input: - _
 Latitude (PHI) decimal degrees; _
 Ellipsoidal height (H) and ellipsoid axis dimensions (a & b) all in meters.

'Convert angle measures to radians
    Pi = 3.14159265358979
    RadPHI = PHI * (Pi / 180)

'Compute eccentricity squared and nu
    e2 = ((a ^ 2) - (b ^ 2)) / (a ^ 2)
    V = a / (Sqr(1 - (e2 * ((Sin(RadPHI)) ^ 2))))

'Compute X
    Lat_H_to_Z = ((V * (1 - e2)) + H) * (Sin(RadPHI))

End Function

Function Lat_Long_to_East(PHI, LAM, a, b, e0, f0, PHI0, LAM0)
'Project Latitude and longitude to Transverse Mercator eastings.
'Input: - _
 Latitude (PHI) and Longitude (LAM) in decimal degrees; _
 ellipsoid axis dimensions (a & b) in meters; _
 eastings of false origin (e0) in meters; _
 central meridian scale factor (f0); _
 latitude (PHI0) and longitude (LAM0) of false origin in decimal degrees.

'Convert angle measures to radians
    Pi = 3.14159265358979
    RadPHI = PHI * (Pi / 180)
    RadLAM = LAM * (Pi / 180)
    RadPHI0 = PHI0 * (Pi / 180)
    RadLAM0 = LAM0 * (Pi / 180)

    af0 = a * f0
    bf0 = b * f0
    e2 = ((af0 ^ 2) - (bf0 ^ 2)) / (af0 ^ 2)
    n = (af0 - bf0) / (af0 + bf0)
    nu = af0 / (Sqr(1 - (e2 * ((Sin(RadPHI)) ^ 2))))
    rho = (nu * (1 - e2)) / (1 - (e2 * (Sin(RadPHI)) ^ 2))
    eta2 = (nu / rho) - 1
    p = RadLAM - RadLAM0
    
    IV = nu * (Cos(RadPHI))
    V = (nu / 6) * ((Cos(RadPHI)) ^ 3) * ((nu / rho) - ((Tan(RadPHI) ^ 2)))
    VI = (nu / 120) * ((Cos(RadPHI)) ^ 5) * (5 - (18 * ((Tan(RadPHI)) ^ 2)) + ((Tan(RadPHI)) ^ 4) + (14 * eta2) - (58 * ((Tan(RadPHI)) ^ 2) * eta2))
    
    Lat_Long_to_East = e0 + (p * IV) + ((p ^ 3) * V) + ((p ^ 5) * VI)
    
End Function

Function Lat_Long_to_North(PHI, LAM, a, b, e0, n0, f0, PHI0, LAM0)
'Project Latitude and longitude to Transverse Mercator northings
'Input: - _
 Latitude (PHI) and Longitude (LAM) in decimal degrees; _
 ellipsoid axis dimensions (a & b) in meters; _
 eastings (e0) and northings (n0) of false origin in meters; _
 central meridian scale factor (f0); _
 latitude (PHI0) and longitude (LAM0) of false origin in decimal degrees.

'REQUIRES THE "Marc" FUNCTION

'Convert angle measures to radians
    Pi = 3.14159265358979
    RadPHI = PHI * (Pi / 180)
    RadLAM = LAM * (Pi / 180)
    RadPHI0 = PHI0 * (Pi / 180)
    RadLAM0 = LAM0 * (Pi / 180)
    
    af0 = a * f0
    bf0 = b * f0
    e2 = ((af0 ^ 2) - (bf0 ^ 2)) / (af0 ^ 2)
    n = (af0 - bf0) / (af0 + bf0)
    nu = af0 / (Sqr(1 - (e2 * ((Sin(RadPHI)) ^ 2))))
    rho = (nu * (1 - e2)) / (1 - (e2 * (Sin(RadPHI)) ^ 2))
    eta2 = (nu / rho) - 1
    p = RadLAM - RadLAM0
    M = Marc(bf0, n, RadPHI0, RadPHI)
    
    I = M + n0
    II = (nu / 2) * (Sin(RadPHI)) * (Cos(RadPHI))
    III = ((nu / 24) * (Sin(RadPHI)) * ((Cos(RadPHI)) ^ 3)) * (5 - ((Tan(RadPHI)) ^ 2) + (9 * eta2))
    IIIA = ((nu / 720) * (Sin(RadPHI)) * ((Cos(RadPHI)) ^ 5)) * (61 - (58 * ((Tan(RadPHI)) ^ 2)) + ((Tan(RadPHI)) ^ 4))
    
    Lat_Long_to_North = I + ((p ^ 2) * II) + ((p ^ 4) * III) + ((p ^ 6) * IIIA)
   
End Function

Function E_N_to_Lat(East, North, a, b, e0, n0, f0, PHI0, LAM0)
'Un-project Transverse Mercator eastings and northings back to latitude.
'Input: - _
 eastings (East) and northings (North) in meters; _
 ellipsoid axis dimensions (a & b) in meters; _
 eastings (e0) and northings (n0) of false origin in meters; _
 central meridian scale factor (f0) and _
 latitude (PHI0) and longitude (LAM0) of false origin in decimal degrees.

'REQUIRES THE "Marc" AND "InitialLat" FUNCTIONS

'Convert angle measures to radians
    Pi = 3.14159265358979
    RadPHI0 = PHI0 * (Pi / 180)
    RadLAM0 = LAM0 * (Pi / 180)

'Compute af0, bf0, e squared (e2), n and Et
    af0 = a * f0
    bf0 = b * f0
    e2 = ((af0 ^ 2) - (bf0 ^ 2)) / (af0 ^ 2)
    n = (af0 - bf0) / (af0 + bf0)
    Et = East - e0

'Compute initial value for latitude (PHI) in radians
    PHId = InitialLat(North, n0, af0, RadPHI0, n, bf0)
    
'Compute nu, rho and eta2 using value for PHId
    nu = af0 / (Sqr(1 - (e2 * ((Sin(PHId)) ^ 2))))
    rho = (nu * (1 - e2)) / (1 - (e2 * (Sin(PHId)) ^ 2))
    eta2 = (nu / rho) - 1
    
'Compute Latitude
    VII = (Tan(PHId)) / (2 * rho * nu)
    VIII = ((Tan(PHId)) / (24 * rho * (nu ^ 3))) * (5 + (3 * ((Tan(PHId)) ^ 2)) + eta2 - (9 * eta2 * ((Tan(PHId)) ^ 2)))
    IX = ((Tan(PHId)) / (720 * rho * (nu ^ 5))) * (61 + (90 * ((Tan(PHId)) ^ 2)) + (45 * ((Tan(PHId)) ^ 4)))
    
    E_N_to_Lat = (180 / Pi) * (PHId - ((Et ^ 2) * VII) + ((Et ^ 4) * VIII) - ((Et ^ 6) * IX))

End Function

Function E_N_to_Long(East, North, a, b, e0, n0, f0, PHI0, LAM0)
'Un-project Transverse Mercator eastings and northings back to longitude.
'Input: - _
 eastings (East) and northings (North) in meters; _
 ellipsoid axis dimensions (a & b) in meters; _
 eastings (e0) and northings (n0) of false origin in meters; _
 central meridian scale factor (f0) and _
 latitude (PHI0) and longitude (LAM0) of false origin in decimal degrees.

'REQUIRES THE "Marc" AND "InitialLat" FUNCTIONS

'Convert angle measures to radians
    Pi = 3.14159265358979
    RadPHI0 = PHI0 * (Pi / 180)
    RadLAM0 = LAM0 * (Pi / 180)

'Compute af0, bf0, e squared (e2), n and Et
    af0 = a * f0
    bf0 = b * f0
    e2 = ((af0 ^ 2) - (bf0 ^ 2)) / (af0 ^ 2)
    n = (af0 - bf0) / (af0 + bf0)
    Et = East - e0

'Compute initial value for latitude (PHI) in radians
    PHId = InitialLat(North, n0, af0, RadPHI0, n, bf0)
    
'Compute nu, rho and eta2 using value for PHId
    nu = af0 / (Sqr(1 - (e2 * ((Sin(PHId)) ^ 2))))
    rho = (nu * (1 - e2)) / (1 - (e2 * (Sin(PHId)) ^ 2))
    eta2 = (nu / rho) - 1
    
'Compute Longitude
    X = ((Cos(PHId)) ^ -1) / nu
    XI = (((Cos(PHId)) ^ -1) / (6 * (nu ^ 3))) * ((nu / rho) + (2 * ((Tan(PHId)) ^ 2)))
    XII = (((Cos(PHId)) ^ -1) / (120 * (nu ^ 5))) * (5 + (28 * ((Tan(PHId)) ^ 2)) + (24 * ((Tan(PHId)) ^ 4)))
    XIIA = (((Cos(PHId)) ^ -1) / (5040 * (nu ^ 7))) * (61 + (662 * ((Tan(PHId)) ^ 2)) + (1320 * ((Tan(PHId)) ^ 4)) + (720 * ((Tan(PHId)) ^ 6)))

    E_N_to_Long = (180 / Pi) * (RadLAM0 + (Et * X) - ((Et ^ 3) * XI) + ((Et ^ 5) * XII) - ((Et ^ 7) * XIIA))

End Function

Function InitialLat(North, n0, afo, PHI0, n, bfo)
'Compute initial value for Latitude (PHI) IN RADIANS.
'Input: - _
 northing of point (North) and northing of false origin (n0) in meters; _
 semi major axis multiplied by central meridian scale factor (af0) in meters; _
 latitude of false origin (PHI0) IN RADIANS; _
 n (computed from a, b and f0) and _
 ellipsoid semi major axis multiplied by central meridian scale factor (bf0) in meters.
 
'REQUIRES THE "Marc" FUNCTION
'THIS FUNCTION IS CALLED BY THE "E_N_to_Lat", "E_N_to_Long" and "E_N_to_C" FUNCTIONS
'THIS FUNCTION IS ALSO USED ON IT'S OWN IN THE  "Projection and Transformation Calculations.xls" SPREADSHEET

'First PHI value (PHI1)
    PHI1 = ((North - n0) / afo) + PHI0
    
'Calculate M
    M = Marc(bfo, n, PHI0, PHI1)
    
'Calculate new PHI value (PHI2)
    PHI2 = ((North - n0 - M) / afo) + PHI1
    
'Iterate to get final value for InitialLat
    Do While Abs(North - n0 - M) > 0.00001
        PHI2 = ((North - n0 - M) / afo) + PHI1
        M = Marc(bfo, n, PHI0, PHI2)
        PHI1 = PHI2
    Loop
    
    InitialLat = PHI2
    
End Function

Function Marc(bf0, n, PHI0, PHI)
'Compute meridional arc.
'Input: - _
 ellipsoid semi major axis multiplied by central meridian scale factor (bf0) in meters; _
 n (computed from a, b and f0); _
 lat of false origin (PHI0) and initial or final latitude of point (PHI) IN RADIANS.

'THIS FUNCTION IS CALLED BY THE - _
 "Lat_Long_to_North" and "InitialLat" FUNCTIONS
'THIS FUNCTION IS ALSO USED ON IT'S OWN IN THE "Projection and Transformation Calculations.xls" SPREADSHEET

    Marc = bf0 * (((1 + n + ((5 / 4) * (n ^ 2)) + ((5 / 4) * (n ^ 3))) * (PHI - PHI0)) _
    - (((3 * n) + (3 * (n ^ 2)) + ((21 / 8) * (n ^ 3))) * (Sin(PHI - PHI0)) * (Cos(PHI + PHI0))) _
    + ((((15 / 8) * (n ^ 2)) + ((15 / 8) * (n ^ 3))) * (Sin(2 * (PHI - PHI0))) * (Cos(2 * (PHI + PHI0)))) _
    - (((35 / 24) * (n ^ 3)) * (Sin(3 * (PHI - PHI0))) * (Cos(3 * (PHI + PHI0)))))

End Function

Function Lat_Long_to_C(PHI, LAM, LAM0, a, b, f0)
'Compute convergence (in decimal degrees) from latitude and longitude
'Input: - _
 latitude (PHI), longitude (LAM) and longitude (LAM0) of false origin in decimal degrees; _
 ellipsoid axis dimensions (a & b) in meters; _
 central meridian scale factor (f0).

'Convert angle measures to radians
    Pi = 3.14159265358979
    RadPHI = PHI * (Pi / 180)
    RadLAM = LAM * (Pi / 180)
    RadLAM0 = LAM0 * (Pi / 180)
        
'Compute af0, bf0 and e squared (e2)
    af0 = a * f0
    bf0 = b * f0
    e2 = ((af0 ^ 2) - (bf0 ^ 2)) / (af0 ^ 2)
    
'Compute nu, rho, eta2 and p
    nu = af0 / (Sqr(1 - (e2 * ((Sin(RadPHI)) ^ 2))))
    rho = (nu * (1 - e2)) / (1 - (e2 * (Sin(RadPHI)) ^ 2))
    eta2 = (nu / rho) - 1
    p = RadLAM - RadLAM0

'Compute Convergence
    XIII = Sin(RadPHI)
    XIV = ((Sin(RadPHI) * ((Cos(RadPHI)) ^ 2)) / 3) * (1 + (3 * eta2) + (2 * (eta2 ^ 2)))
    XV = ((Sin(RadPHI) * ((Cos(RadPHI)) ^ 4)) / 15) * (2 - ((Tan(RadPHI)) ^ 2))

    Lat_Long_to_C = (180 / Pi) * ((p * XIII) + ((p ^ 3) * XIV) + ((p ^ 5) * XV))

End Function

Function E_N_to_C(East, North, a, b, e0, n0, f0, PHI0)
'Compute convergence (in decimal degrees) from easting and northing
'Input: - _
 Eastings (East) and Northings (North) in meters; _
 ellipsoid axis dimensions (a & b) in meters; _
 easting (e0) and northing (n0) of true origin in meters; _
 central meridian scale factor (f0); _
 latitude of central meridian (PHI0) in decimal degrees.
 
'REQUIRES THE "Marc" AND "InitialLat" FUNCTIONS

'Convert angle measures to radians
    Pi = 3.14159265358979
    RadPHI0 = PHI0 * (Pi / 180)
        
'Compute af0, bf0, e squared (e2), n and Et
    af0 = a * f0
    bf0 = b * f0
    e2 = ((af0 ^ 2) - (bf0 ^ 2)) / (af0 ^ 2)
    n = (af0 - bf0) / (af0 + bf0)
    Et = East - e0
    
'Compute initial value for latitude (PHI) in radians
    PHId = InitialLat(North, n0, af0, RadPHI0, n, bf0)
    
'Compute nu, rho and eta2 using value for PHId
    nu = af0 / (Sqr(1 - (e2 * ((Sin(PHId)) ^ 2))))
    rho = (nu * (1 - e2)) / (1 - (e2 * (Sin(PHId)) ^ 2))
    eta2 = (nu / rho) - 1

'Compute Convergence
    XVI = (Tan(PHId)) / nu
    XVII = ((Tan(PHId)) / (3 * (nu ^ 3))) * (1 + ((Tan(PHId)) ^ 2) - eta2 - (2 * (eta2 ^ 2)))
    XVIII = ((Tan(PHId)) / (15 * (nu ^ 5))) * (2 + (5 * ((Tan(PHId)) ^ 2)) + (3 * ((Tan(PHId)) ^ 4)))
    
    E_N_to_C = (180 / Pi) * ((Et * XVI) - ((Et ^ 3) * XVII) + ((Et ^ 5) * XVIII))
   
End Function

Function Lat_Long_to_LSF(PHI, LAM, LAM0, a, b, f0)
'Compute local scale factor from latitude and longitude
'Input: - _
 latitude (PHI), longitude (LAM) and longitude (LAM0) of false origin in decimal degrees; _
 ellipsoid axis dimensions (a & b) in meters; _
 central meridian scale factor (f0).
 
 'Convert angle measures to radians
    Pi = 3.14159265358979
    RadPHI = PHI * (Pi / 180)
    RadLAM = LAM * (Pi / 180)
    RadLAM0 = LAM0 * (Pi / 180)
        
'Compute af0, bf0 and e squared (e2)
    af0 = a * f0
    bf0 = b * f0
    e2 = ((af0 ^ 2) - (bf0 ^ 2)) / (af0 ^ 2)
    
'Compute nu, rho, eta2 and p
    nu = af0 / (Sqr(1 - (e2 * ((Sin(RadPHI)) ^ 2))))
    rho = (nu * (1 - e2)) / (1 - (e2 * (Sin(RadPHI)) ^ 2))
    eta2 = (nu / rho) - 1
    p = RadLAM - RadLAM0

'Compute local scale factor
    XIX = ((Cos(RadPHI) ^ 2) / 2) * (1 + eta2)
    XX = ((Cos(RadPHI) ^ 4) / 24) * (5 - (4 * ((Tan(RadPHI)) ^ 2)) + (14 * eta2) - (28 * ((Tan(RadPHI * eta2)) ^ 2)))
    
    Lat_Long_to_LSF = f0 * (1 + ((p ^ 2) * XIX) + ((p ^ 4) * XX))
    
End Function

Function E_N_to_LSF(East, North, a, b, e0, n0, f0, PHI0)
'Compute local scale factor from from easting and northing
'Input: - _
 Eastings (East) and Northings (North) in meters; _
 ellipsoid axis dimensions (a & b) in meters; _
 easting (e0) and northing (n0) of true origin in meters; _
 central meridian scale factor (f0); _
 latitude of central meridian (PHI0) in decimal degrees.
 
'REQUIRES THE "Marc" AND "InitialLat" FUNCTIONS

'Convert angle measures to radians
    Pi = 3.14159265358979
    RadPHI0 = PHI0 * (Pi / 180)
        
'Compute af0, bf0, e squared (e2), n and Et
    af0 = a * f0
    bf0 = b * f0
    e2 = ((af0 ^ 2) - (bf0 ^ 2)) / (af0 ^ 2)
    n = (af0 - bf0) / (af0 + bf0)
    Et = East - e0
    
'Compute initial value for latitude (PHI) in radians
    PHId = InitialLat(North, n0, af0, RadPHI0, n, bf0)
    
'Compute nu, rho and eta2 using value for PHId
    nu = af0 / (Sqr(1 - (e2 * ((Sin(PHId)) ^ 2))))
    rho = (nu * (1 - e2)) / (1 - (e2 * (Sin(PHId)) ^ 2))
    eta2 = (nu / rho) - 1

'Compute local scale factor
    XXI = 1 / (2 * rho * nu)
    XXII = (1 + (4 * eta2)) / (24 * (rho ^ 2) * (nu ^ 2))
    
    E_N_to_LSF = f0 * (1 + ((Et ^ 2) * XXI) + ((Et ^ 4) * XXII))
    
End Function

Function E_N_to_t_minus_T(AtEast, AtNorth, ToEast, ToNorth, a, b, e0, n0, f0, PHI0)
'Compute (t-T) correction in decimal degrees at point (AtEast, AtNorth) to point (ToEast,ToNorth)
'Input: - _
 Eastings (AtEast) and Northings (AtNorth) in meters, of point where (t-T) is being computed; _
 Eastings (ToEast) and Northings (ToNorth) in meters, of point at other end of line to which (t-T) is being computed; _
 ellipsoid axis dimensions (a & b) and easting & northing (e0 & n0) of true origin in meters; _
 central meridian scale factor (f0); _
 latitude of central meridian (PHI0) in decimal degrees.

'REQUIRES THE "Marc" AND "InitialLat" FUNCTIONS

'Convert angle measures to radians
    Pi = 3.14159265358979
    RadPHI0 = PHI0 * (Pi / 180)
        
'Compute af0, bf0, e squared (e2), n and Nm (Northing of mid point)
    af0 = a * f0
    bf0 = b * f0
    e2 = ((af0 ^ 2) - (bf0 ^ 2)) / (af0 ^ 2)
    n = (af0 - bf0) / (af0 + bf0)
    Nm = (AtNorth + ToNorth) / 2
    
'Compute initial value for latitude (PHI) in radians
    PHId = InitialLat(Nm, n0, af0, RadPHI0, n, bf0)
    
'Compute nu, rho and eta2 using value for PHId
    nu = af0 / (Sqr(1 - (e2 * ((Sin(PHId)) ^ 2))))
    rho = (nu * (1 - e2)) / (1 - (e2 * (Sin(PHId)) ^ 2))
    
'Compute (t-T)
    XXIII = 1 / (6 * nu * rho)
    
    E_N_to_t_minus_T = (180 / Pi) * ((2 * (AtEast - e0)) + (ToEast - e0)) * (AtNorth - ToNorth) * XXIII

End Function

Function TrueAzimuth(AtEast, AtNorth, ToEast, ToNorth, a, b, e0, n0, f0, PHI0)
'Compute true azimuth in decimal degrees at point (AtEast, AtNorth) to point (ToEast,ToNorth)
'Input: - _
 Eastings (AtEast) and Northings (AtNorth) in meters, of point where true azimuth is being computed; _
 Eastings (ToEast) and Northings (ToNorth) in meters, of point at other end of line to which true azimuth is being computed; _
 ellipsoid axis dimensions (a & b) and easting & northing (e0 & n0) of true origin in meters; _
 central meridian scale factor (f0); _
 latitude of central meridian (PHI0) in decimal degrees.

'REQUIRES THE "Marc", "InitialLat", "E_N_to_t_minus_T" and "E_N_to_C" FUNCTIONS
  
'Compute eastings and northings differences
    Diffe = ToEast - AtEast
    Diffn = ToNorth - AtNorth

'Compute grid bearing
    If Diffe = 0 Then
        If Diffn < 0 Then
            GridBearing = 180
        Else
            GridBearing = 0
        End If
        GoTo EndOfComputeBearing
    End If
    
    Ratio = Diffn / Diffe
    Pi = 3.14159265358979
    GridAngle = (180 / Pi) * Atn(Ratio)
    
    If Diffe > 0 Then
        GridBearing = 90 - GridAngle
    End If
    
    If Diffe < 0 Then
        GridBearing = 270 - GridAngle
    End If
EndOfComputeBearing:

'Compute convergence
    Convergence = E_N_to_C(AtEast, AtNorth, a, b, e0, n0, f0, PHI0)
    
'Compute (t-T) correction
    t_minus_T = E_N_to_t_minus_T(AtEast, AtNorth, ToEast, ToNorth, a, b, e0, n0, f0, PHI0)

'Compute initial azimuth
    InitAzimuth = GridBearing + Convergence - t_minus_T
    
'Set TrueAzimuth >=0 and <=360
    If InitAzimuth < 0 Then
        TrueAzimuth = InitAzimuth + 360
    ElseIf InitAzimuth > 360 Then
        TrueAzimuth = InitAzimuth - 360
    Else
        TrueAzimuth = InitAzimuth
    End If

End Function
