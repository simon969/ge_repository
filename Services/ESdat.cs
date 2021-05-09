using System;
using ge_repository.OtherDatabase ;
using System.Collections.Generic;
using System.Data;


using ge_repository.AGS;

namespace ge_repository.ESdat { 

public class ge_ESdatChemistry {
    public double? EastingCoords {get;set;}// Easting Coordinates
    public double? NorthingCoords {get;set;} // Northing Coordinates
    
    public double? Latitude {get;set;} // Latitude
    public double? Longitude {get;set;} // Longitude
    public string Site_ID {get;set;} // Site_ID
    public string Project_ID {get;set;} // Project_ID
    public string Monitoring_Zone {get;set;} // Monitoring_Zone
    public string SampleCode {get;set;} // SampleCode
    public string Field_ID {get;set;}   // Field_ID
    public string Well {get;set;} // Well
    public string Monitoring_Unit {get;set;} // Monitoring_Unit
    public double? Sample_Elevation {get;set;} // Sample_Elevation
    public double? Sample_Depth_Avg {get;set;} // Sample_Depth_Avg
    public DateTime? Sampled_Date_Time {get;set;} // Sampled_Date_Time
    // New Sample Location IDs
    // Old Sample Location IDs
    public string Monitoring_Round {get;set;}   // Monitoring_Round 
    public string SDG {get;set;} // SDG
    public string ChemName {get;set;}// ChemName
    public string Total_or_Filtered {get;set;}// Total_or_Filtered
    public string Prefix {get;set;}// Prefix
    public string Concentration {get;set;} // Concentration
    public string Output_Unit {get;set;} // Output Unit
    public string ChemCode {get;set;} // ChemCode
    public string ChemName_Abbrev {get;set;} // ChemName_Abbrev
    public string Chem_Group {get;set;} // Chem_Group
    public string Method_Type {get;set;} // Method_Type
    public string Method_Name {get;set;} // Method_Name
    public string EQL {get;set;} // EQL
    public string EQL_Units {get;set;} // EQL_Units
    public string Lab_Comments {get;set;} // Lab_Comments
    public string Lab_Report_Number {get;set;} // Lab_Report_Number
    public string Lab_Name {get;set;} // Lab_Name
    public string Location_Type {get;set;} // Location_Type
    public string Sample_Type {get;set;} // Sample_Type
    public string Matrix_Type {get;set;} // Matrix_Type
    public string Matrix_State {get;set;} // Matrix_State
    public string Env_Stds_Condition_Matrix_Type {get;set;} // Env_Stds_Conditional_Matrix_Type
    public string SampleComments {get;set;} // SampleComments
    public string Qualifier {get;set;} // Qualifier
    public string Result_Comments {get;set;} // Result_Comments
    public string Data_Source {get;set;} // Data_Source
    public string ID {get;set;} // ID
    public string SID {get;set;} // SID
    public string PID {get;set;} // PID
    public string Metadata_ID {get;set;} // Metadata_ID

    public int set_values(string[] header, string[] values) {
            try {
                for (int i=0;i<header.Length;i++) {
                    if (header[i] == "Easting Coordinates" && values[i] != "") EastingCoords = Convert.ToDouble(values[i]);
                    if (header[i] == "Northing Coordinates" && values[i] != "") NorthingCoords = Convert.ToDouble(values[i]);
                    if (header[i] == "SampleCode" && values[i]!= "") SampleCode = values[i];
                    if (header[i] == "Field_ID" && values[i] != "") Field_ID = values[i];
                    if (header[i] == "Well" && values[i] != "") Well = values[i];
                    if (header[i] == "Monitoring_Unit" && values[i] != "") Monitoring_Unit = values[i];
                    if (header[i] == "Sample_Elevation" && values[i] != "") Sample_Elevation = Convert.ToDouble(values[i]);
                    if (header[i] == "Sample_Depth_Avg" && values[i] != "") Sample_Depth_Avg = Convert.ToDouble(values[i]);
                    if (header[i] == "Sample_Date_Time" && values[i] != "") Sampled_Date_Time = Convert.ToDateTime(values[i]);
                    if (header[i] == "Monitoring_Round" && values[i] != "") Monitoring_Round= values[i];
                    if (header[i] == "SDG" && values[i] != "") SDG = values[i];
                    if (header[i] == "ChemName" && values[i] != "") ChemName = values[i];
                    if (header[i] == "Total_or_Filtered " && values[i] != "") Total_or_Filtered  = values[i]; 
                    if (header[i] == "Prefix" && values[i] != "") Prefix = values[i]; 
                    if (header[i] == "Concentration" && values[i] != "") Concentration = values[i]; 
                    if (header[i] == "Output_Unit" && values[i] != "") Output_Unit = values[i]; 
                    if (header[i] == "ChemCode" && values[i] != "") ChemCode = values[i]; 
                    if (header[i] == "ChemName_Abbrev" && values[i] != "") ChemName_Abbrev = values[i]; 
                    if (header[i] == "Chem_Group" && values[i] != "") Chem_Group = values[i]; 
                    if (header[i] == "Method_Type" && values[i] != "") Method_Type = values[i];
                    if (header[i] == "Matrix_State" && values[i] != "") Matrix_State = values[i];
                    if (header[i] == "Env_Stds_Condition_Matrix_Type" && values[i] != "") Env_Stds_Condition_Matrix_Type = values[i]; 
                    if (header[i] == "SampleComments" && values[i] != "") SampleComments = values[i]; 
                    if (header[i] == "Qualifier" && values[i] != "") Qualifier = values[i]; 
                    if (header[i] == "Result_Comments" && values[i] != "") Result_Comments =  values[i]; 
                    if (header[i] == "Data_Source" && values[i] != "") Data_Source= values[i]; 
                    if (header[i] == "ID" && values[i] != "") ID = values[i]; 
                    if (header[i] == "SID" && values[i] != "") SID  = values[i]; 
                    if (header[i] == "PID" && values[i] != "") PID = values[i]; 
                    if (header[i] == "Metadata_ID" && values[i] != "") Metadata_ID = values[i]; 
                }

            } catch {
                return -1;
            }
            
            return 0;
            }
    public int CopyTo( ERES item) { 
        try {
        item.PointID = Field_ID;
        if (Sample_Depth_Avg != null) item.Depth = Sample_Depth_Avg.Value;
        if (Sample_Depth_Avg != null) item.Depth = Sample_Depth_Avg.Value;
        if (SampleCode !=null) item.SAMP_ID = SampleCode;
        if (Sampled_Date_Time !=null) item.ERES_DTIM = Sampled_Date_Time;
        if (Sampled_Date_Time !=null) item.ERES_LDTM = Sampled_Date_Time;
        return 1;
        } 
        catch (Exception e) {
        return -1;
        }

        
    }
    public int CopyTo( POINT item) {
        try {
        item.PointID = Field_ID;
        if (EastingCoords != null) item.East= EastingCoords.Value;
        if (NorthingCoords != null) item.North = NorthingCoords.Value;

        return 1;
        } 
        catch (Exception e) {
        return -1;
        }
    }

    }


    // public class ge_esdat_file : ge_data_table {
    // private string[] headers {get;set;}
    // private string[] rows {get;set;}
    // public ge_esdat_file() {}
    // public ge_esdat_file(string[] columns) {
    //     foreach(string column_name in columns) {
    //         dt.Columns.Add(column_name);
    //     }
    // }

    // public List<ge_ESdatChemistry> chemistry {get;set;}



    // }
}
   
