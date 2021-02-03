 // https://stackoverflow.com/questions/40426827/how-to-get-the-value-of-cell-containing-a-date-and-keep-the-original-formatting
    
    using System;
    using NPOI.SS.UserModel;

    namespace ge_repository.Extensions {

    public static class NpoiExtensions
    {
    public static DateTime MIN_DATE = new DateTime (2000,1,1);
    public static DateTime MAX_DATE = new DateTime (2022,1,1);
    public static string GetFormattedCellValue(this ICell cell, IFormulaEvaluator eval = null, Boolean UseCachedFormulaResult = false, string ImposeDatetimeFormat = "")
    {
        
        if (cell != null)
        {
            CellType cellType = cell.CellType;

            if (UseCachedFormulaResult == true && cellType==CellType.Formula) {
                cellType=cell.CachedFormulaResultType;
            } 

            switch (cellType)
            {
                case CellType.String:
                    return cell.StringCellValue;

                case CellType.Numeric:
                
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        DateTime date = cell.DateCellValue;

                        if (ImposeDatetimeFormat!="") {
                            return date.ToString(ImposeDatetimeFormat);
                        }

                        ICellStyle style = cell.CellStyle;
                        // Excel uses lowercase m for month whereas .Net uses uppercase
                        string format = style.GetDataFormatString().Replace('m', 'M');
                        return date.ToString(format);
                    }
                    
                    if(cell.CellStyle.DataFormat>=164 && DateUtil.IsValidExcelDate(cell.NumericCellValue) && cell.DateCellValue != null)
                    {
                       DateTime date = cell.DateCellValue;
                        return date.ToString(ImposeDatetimeFormat);
                    }

                    return cell.NumericCellValue.ToString();
 

                case CellType.Boolean:
                    return cell.BooleanCellValue ? "TRUE" : "FALSE";

                case CellType.Formula:
                    if (eval != null )
                        return GetFormattedCellValue(eval.EvaluateInCell(cell),null,false,ImposeDatetimeFormat);
                    else
                        return cell.CellFormula;

                case CellType.Error:
                    return FormulaError.ForInt(cell.ErrorCellValue).String;
            }
        }
        // null or blank cell, or unknown cell type
        return string.Empty;
    }
    
    public static object GetCellValue(this ICell cell, IFormulaEvaluator eval = null) {
    
    object cValue = string.Empty;

        switch (cell.CellType)
                {
                    case (CellType.Unknown | CellType.Blank):
                        cValue = cell.ToString();
                        break;
                    case CellType.Formula :
                        return GetCellValue(cell, eval); 
                        break;
                    case CellType.Numeric:
                        if (DateUtil.IsCellDateFormatted(cell)) {
                            return cell.DateCellValue;
                        }
                        cValue = cell.NumericCellValue;
                        break;
                    case CellType.String:
                        cValue = cell.StringCellValue;
                        break;
                    case CellType.Boolean:
                        cValue = cell.BooleanCellValue;
                        break;
                    case CellType.Error:
                        cValue = cell.ErrorCellValue;
                        break;
                    default:
                        cValue = string.Empty;
                        break;
                }
        return cValue;
    }

    // public static DateTime FromExcelSerialDateTime(double SerialDate) {
    // if (SerialDate > 59) SerialDate -= 1; //Excel/Lotus 2/29/1900 bug   
    // return new DateTime(1899, 12, 31).AddDays(SerialDate);
    // }

    // public string ExcelDateFormatter(string DateTimeFORMAT, ICell cell) {
    //     if (cell.NumericCellValue>0) {
    //     DateTime cell_DT = FromExcelSerialDateTime(cell.NumericCellValue);
    //     return string.Format(DateTimeFORMAT, cell_DT);
    //     }
    //     return "";
    // }
    // public string ExcelDateFormatter3(string DateTimeFORMAT, ICell cell) {
    //     DateTime cell_DT = DateTime.FromOADate(cell.NumericCellValue);
    //     return string.Format(DateTimeFORMAT, cell_DT);
    // }
    // public string ExcelDateFormatterFromCell(ICell cell) {

    //     return formatter.FormatCellValue(cell,evaluator);
    // }
    // public string ExcelDateFormatter2(string DateTimeFORMAT, ICell cell) {
    // DateTime cell_DT = new DateTime(1899, 12, 31, 0, 0, 0, DateTimeKind.Utc).AddDays(cell.NumericCellValue-1);
    // return string.Format(DateTimeFORMAT, cell_DT);
    // }
//     public static string DataFormatter2(ICell cell) {
//     string DateTimeFORMAT ="{0:yyyy-MM-ddTHH:mm:ss}";
    
//     switch (cell.CellType)
//     {
//         case (CellType.Unknown | CellType.Formula | CellType.Blank):
//             return cell.ToString();
//         case CellType.Numeric:
//             if (DateUtil.IsCellDateFormatted(cell)) {
//                 return String.Format(DateTimeFORMAT, cell.DateCellValue);
//             }
//             return  Convert.ToString(cell.NumericCellValue);
//         case CellType.String:
//             return cell.StringCellValue;
//         case CellType.Boolean:
//             return Convert.ToString(cell.BooleanCellValue);
//         case CellType.Error:
//             return Convert.ToString(cell.ErrorCellValue);
//         default:
//             return string.Empty;
//     }
// }

// public string DataFormatter(ICell cell) {
       
//         if (cell==null) { return "";}

//         try {   
//             // if (cell.CellType==CellType.String ||cell.CellType==CellType.Formula) {
//             //     return formatter.FormatCellValue(cell,evaluator);
//             // }
            
//             // if (cell.CellType==CellType.Numeric || cell.CellType==CellType.Formula) {
//             //             if (DateUtil.IsCellDateFormatted(cell)) {
//             //                 return ExcelDateFormatter(DateTimeFORMAT,cell);
//             //             }
//             // }
//         if (DateUtil.IsCellDateFormatted(cell)) {
//             return ExcelDateFormatter(DateTimeFORMAT,cell);
//         }
//         return formatter.FormatCellValue(cell,evaluator);
//         } catch (Exception ex) {   
//             // return $"{ex.Message} error converting cell {cell.Address} to string format";
//             return formatter.FormatCellValue(cell,evaluator);
//         }
//     }
    }
    }
