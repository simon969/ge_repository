using System;
using System.IO;
using System.Text;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Globalization;
namespace ge_repository.OtherDatabase
{

    public class ge_log_workbook {

        // https://poi.apache.org/components/spreadsheet/quick-guide.html#Iterator
    public XSSFWorkbook workbook {get;private set;}
    public ISheet worksheet{get;private set;}
    public ICellRange<ICell> headers {get;private set;}
    public ICellRange<ICell> data {get; private set;}

    public int current_data_row;
    public int start_data_row;
    public int end_data_row;    
    public ge_log_workbook(FileStream fs) {
        workbook = new XSSFWorkbook(fs);
    }
    public ge_log_workbook(MemoryStream ms) {
        
            workbook = new XSSFWorkbook(ms);
    
    }
    public void close() {

        worksheet = null;
        headers = null;
        data = null;
        workbook.Close();
        workbook = null;

    }
    public IRow NextRow() {
        current_data_row++;
        if (current_data_row<=end_data_row) {
            return worksheet.GetRow(current_data_row);
        }
        return null;
    }
    public IRow FirstRow() {
        current_data_row = start_data_row;
        return worksheet.GetRow(current_data_row);
    }

    public string RowCSV(IRow row, int lastColumn) {
        
        StringBuilder sb =new StringBuilder();

        for (int i = 0; i < lastColumn; i++) {
            
            ICell cell = row.GetCell(i);
            
            if (cell == null & i!=0) {
                sb.Append (",");
            } else {
                if (i>0) {
                  sb.Append (","); 
                }
                sb.Append (DataFormatter(cell));
            }
        }
    
        return sb.ToString();
    
    }
     public string WorksheetToCSV() {
        
        int MIN_COLUMN_COUNT = 50;
     
        StringBuilder sb =new StringBuilder();
        
        int rowStart = Math.Min(15, worksheet.FirstRowNum);
        int rowEnd = Math.Max(1400, worksheet.LastRowNum);
        
        for (int rowNum = rowStart; rowNum < rowEnd; rowNum++) {
            IRow r = worksheet.GetRow(rowNum);
            if (r == null) {
            // This whole row is empty
            // Handle it as needed
                continue;
            }

            int lastColumn = Math.Max(r.LastCellNum, MIN_COLUMN_COUNT);
            if (rowNum == rowStart) {
            sb.Append (RowCSV(r, lastColumn));
            } else {
            sb.Append(Environment.NewLine + RowCSV(r,lastColumn));
            }
        }

        return sb.ToString();

    }

    public object getValue(int row, int column) {
        ICell cell = worksheet.GetRow(row).GetCell(column);
        return getCellValue(cell);
    }

    public ISheet setWorksheet (string name) {
        worksheet = workbook.GetSheet (name);
        return worksheet;
    }
    public Boolean setOnlyWorksheet() {
        worksheet = workbook.GetSheetAt(0); 
        return workbook.NumberOfSheets == 1;
    }
    public int matchReturnColumn(string find_string, int row_start, int row_offset) {

        for (int row = row_start; row <= row_start + row_offset; row++) {
            ICell cell = findCellContainsValue(find_string, row); 
            if (cell!=null) {
                return cell.ColumnIndex;
            }
        }

        return -1;
    }

    public int matchReturnRow(string find_string) {

        ICell cell = findCellContainsValue(find_string); 
        
        if (cell!=null) {
           return cell.RowIndex;
        }

        return -1;

    }
   
    public int matchReturnColumn(string find_string) {

        ICell cell = findCellContainsValue(find_string); 
        
        if (cell!=null) {
           return cell.ColumnIndex;
        }

        return -1;

    }

    public static DateTime FromExcelSerialDateTime(double SerialDate) {
    if (SerialDate > 59) SerialDate -= 1; //Excel/Lotus 2/29/1900 bug   
    return new DateTime(1899, 12, 31).AddDays(SerialDate);
    }

    public string ExcelDateFormatter(string DateTimeFORMAT, ICell cell) {
        DateTime cell_DT = FromExcelSerialDateTime(cell.NumericCellValue);
        return string.Format(DateTimeFORMAT, cell_DT);
    }
    public string ExcelDateFormatter3(string DateTimeFORMAT, ICell cell) {
        DateTime cell_DT = DateTime.FromOADate(cell.NumericCellValue);
        return string.Format(DateTimeFORMAT, cell_DT);
    }
    public string ExcelDateFormatter2(string DateTimeFORMAT, ICell cell) {
    DateTime cell_DT = new DateTime(1899, 12, 31, 0, 0, 0, DateTimeKind.Utc).AddDays(cell.NumericCellValue-1);
    return string.Format(DateTimeFORMAT, cell_DT);
    }

       public string DataFormatter(ICell cell) {
            IFormulaEvaluator evaluator = workbook.GetCreationHelper().CreateFormulaEvaluator();
            DataFormatter formatter = new DataFormatter(System.Globalization.CultureInfo.GetCultureInfo("en-GB"));
            string DateTimeFORMAT ="{0:yyyy-MM-ddTHH:mm:ss}";

            if (cell!=null) {
                if (cell.CellType==CellType.Numeric) {
                    if (DateUtil.IsCellDateFormatted(cell)) {
                        return ExcelDateFormatter(DateTimeFORMAT,cell);
                        // try {
                        //     return String.Format(DateTimeFORMAT, cell.DateCellValue);
                        // } catch {
                        //     return ExcelDateFormatter(DateTimeFORMAT, cell);
                        // }
                    }
                }
                try {
                    return formatter.FormatCellValue(cell,evaluator);
                } catch {

                }
            }

            return "";

    }
    public string matchReturnValue(string find_string, int ret_offset_col, int ret_offset_row) {
       
        ICell cell = findCellContainsValue(find_string); 
        
        if (cell!=null) {
            ICell val_cell = worksheet.GetRow(cell.RowIndex + ret_offset_row).GetCell(cell.ColumnIndex + ret_offset_col);
            if (val_cell!=null) {
              return DataFormatter(val_cell);
            }
        }

        return "";
    }
    public ICell findCellContainsValue(string value) {
        
        foreach (IRow row in worksheet) {
            foreach (ICell cell in row) {
                string s1 = DataFormatter(cell);
                  if (s1.Contains(value)) {
                  return cell;
                }
            }
        }

        return null;
    }
    public ICell findCellContainsValue(string value, int row_index) {
        
        IRow row  = worksheet.GetRow(row_index);
        
        if (row!=null) { 
            foreach (ICell cell in row) {
                string s1 = DataFormatter(cell);
                if (s1.Contains(value)) {
                    return cell;
                }
            }
        }

        return null;
    }
   private object getCellValue(ICell cell) {
    
    object cValue = string.Empty;

    switch (cell.CellType)
    {
        case (CellType.Unknown | CellType.Formula | CellType.Blank):
            cValue = cell.ToString();
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

    private string DataFormatter2(ICell cell) {
    string DateTimeFORMAT ="{0:yyyy-MM-ddTHH:mm:ss}";
    
    switch (cell.CellType)
    {
        case (CellType.Unknown | CellType.Formula | CellType.Blank):
            return cell.ToString();
        case CellType.Numeric:
            if (DateUtil.IsCellDateFormatted(cell)) {
                return String.Format(DateTimeFORMAT, cell.DateCellValue);
            }
            return  Convert.ToString(cell.NumericCellValue);
        case CellType.String:
            return cell.StringCellValue;
        case CellType.Boolean:
            return Convert.ToString(cell.BooleanCellValue);
        case CellType.Error:
            return Convert.ToString(cell.ErrorCellValue);
        default:
            return string.Empty;
    }
}

public string [] WorksheetToTable() {

    string tableCSV = WorksheetToCSV();
  
    string[] lines = tableCSV.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
  
    return lines;
}

    
} 



}
