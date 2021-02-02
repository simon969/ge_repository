using System;
using System.IO;
using System.Text;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.Util;
using ge_repository.Extensions;

namespace ge_repository.OtherDatabase

{

    public class ge_log_workbook {

        // https://poi.apache.org/components/spreadsheet/quick-guide.html#Iterator
    public XSSFWorkbook workbook {get;private set;}
    //public IWorkbook workbook {get;private set;}
    public ISheet worksheet{get;private set;}
    public ICellRange<ICell> headers {get;private set;}
    public ICellRange<ICell> data {get; private set;}
    public IFormulaEvaluator evaluator {get; private set;}
    DataFormatter formatter = new DataFormatter(System.Globalization.CultureInfo.GetCultureInfo("en-GB"));
    public string DateTimeFORMAT {get;} = "yyyy-MM-ddTHH:mm:ss" ;
    public string DateTimeFORMAT2 {get;} = "yyyy-MM-ddTHH:mm" ;
    
    public int MAX_SEARCH_LINES {get;set;}
    public int current_data_row;
    public int start_data_row;
    public int end_data_row;    
    public ge_log_workbook(FileStream fs) {
        workbook = new XSSFWorkbook(fs);
        evaluator = workbook.GetCreationHelper().CreateFormulaEvaluator();
        LocaleUtil.SetUserTimeZone(TimeZone.CurrentTimeZone);
    }
    public ge_log_workbook(MemoryStream ms) {
        workbook = new XSSFWorkbook(ms);
        evaluator = workbook.GetCreationHelper().CreateFormulaEvaluator();
        LocaleUtil.SetUserTimeZone(TimeZone.CurrentTimeZone);
    }
    public void close() {
        
        evaluator = null;
        formatter = null;
        
        headers = null;
        data = null;
        worksheet = null;
        
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
    public String RowCSV(int row, int fromColumn, int toColumn = -1, Boolean Encapsulate = false) {
        IRow irow = worksheet.GetRow(row);
        return RowCSV(irow,fromColumn, toColumn, Encapsulate);
    }

    public string RowCSV(IRow row, int fromColumn, int toColumn = -1, Boolean Encapsulate = false) {
        
        StringBuilder sb = new StringBuilder();
        int allColumns = -1;

        if (toColumn == allColumns) {
            toColumn = row.Cells.Count;
        }

        for (int i = fromColumn; i < toColumn; i++) {
            
            ICell cell = row.GetCell(i);
            
            if (cell == null & i!=fromColumn) {
                if (Encapsulate==true) {
                    sb.Append(",\"\"");
                    continue;
                }
                sb.Append (",");
            } else {
                if (i>fromColumn) {
                    sb.Append (","); 
                }
                if (Encapsulate==true) {
                    sb.Append("\"" + cell.GetFormattedCellValue(evaluator,DateTimeFORMAT2)+ "\"");
                    continue;
                }

                sb.Append (cell.GetFormattedCellValue(evaluator,DateTimeFORMAT2));

            }
        }
    
        return sb.ToString();
    
    }
    public string RowCSV2(IRow row, int lastColumn,Boolean Encapsulate = false) {
        
        StringBuilder sb = new StringBuilder();

        foreach (ICell cell in row) { 
                if (Encapsulate==true) {
                    sb.Append("\"" + cell.GetFormattedCellValue(evaluator,DateTimeFORMAT2) + "\"");
                    continue;
                }

                sb.Append (cell.GetFormattedCellValue(evaluator,DateTimeFORMAT2));

        }
           
        return sb.ToString();
    
    }
     // public string WorkSheettoCSV2() {
     
    // HSSFWorkbook wb = new HSSFWorkbook();

    // ExcelExtractor extractor = new ExcelExtractor(wb);
    // extractor.setFormulasNotResults(true);
    // extractor.setIncludeSheetNames(false);
    // String text = extractor.getText();
    // wb.close();

     //}

     public string WorksheetToCSV() {
        
        int MIN_COLUMN_COUNT = 50;
        
        string empty_row = new String(',', MIN_COLUMN_COUNT);

        StringBuilder sb =new StringBuilder();
        
        //int rowStart =  Math.Min(15, worksheet.FirstRowNum);
        int rowStart =  0;
        int rowEnd =   Math.Max(1400, worksheet.LastRowNum);
        
        for (int rowNum = rowStart; rowNum <= rowEnd; rowNum++) {
            IRow r = worksheet.GetRow(rowNum);
            
            if (rowNum > rowStart) { 
                sb.Append(Environment.NewLine);
            }

            if (r == null) {
            // This whole row is empty
            // Handle it as needed
                sb.Append (empty_row);
                continue;
            }

            int lastColumn = Math.Max(r.LastCellNum, MIN_COLUMN_COUNT);
            string row_str = RowCSV(r,0,lastColumn,false);
            sb.Append (row_str);
        
        }
        
        return sb.ToString();

    }

    public object getValue(int row, int column) {
        ICell cell = worksheet.GetRow(row).GetCell(column);
        
        if (cell==null) {
            return null;
        }

        return cell.GetCellValue(evaluator);
    }

    public ISheet setWorksheet (string name) {
        if (name.Contains("*")) {
            string name_like = name.Replace("*","");
            foreach (ISheet sheet in workbook) {
                if (sheet.SheetName.Contains(name_like)) {
                    worksheet = sheet;
                    break;
                }

            }
        } else {
        worksheet = workbook.GetSheet (name);
        }

        return worksheet;
    }
    public Boolean setOnlyWorksheet() {
        worksheet = workbook.GetSheetAt(0); 
        return workbook.NumberOfSheets == 1;
    }
    public int matchReturnColumn(string find_string, int row_start, int row_offset, int col_start = 0, Boolean exact = false) {

        for (int row = row_start; row <= row_start + row_offset; row++) {
            ICell cell = findCellContainsValue(find_string, row,col_start,exact); 
            if (cell!=null) {
                return cell.ColumnIndex;
            }
        }

        return -1;
    }

    public int matchReturnRow(string find_string, Boolean exact = false) {

        ICell cell = findCellContainsValue(find_string, exact); 
        
        if (cell!=null) {
           return cell.RowIndex;
        }

        return -1;

    }
   
    public int matchReturnColumn(string find_string, Boolean exact = false) {

        ICell cell = findCellContainsValue(find_string, exact); 
        
        if (cell!=null) {
           return cell.ColumnIndex;
        }

        return -1;

    }

        // https://www.codota.com/code/java/methods/org.apache.poi.ss.usermodel.DataFormatter/performDateFormatting
        // private String getFormattedDateString(ICell cell, ConditionalFormattingEvaluator cfEvaluator) {
        // if (cell == null) {
        //     return null;
        // }
        // Format dateFormat = getFormat(cell, cfEvaluator);
        // if(dateFormat instanceof ExcelStyleDateFormatter) {
        //     // Hint about the raw excel value
        //     ((ExcelStyleDateFormatter)dateFormat).setDateToBeFormatted(
        //     cell.getNumericCellValue()
        //     );
        // }
        // Date d = cell.getDateCellValue();
        // return performDateFormatting(d, dateFormat);
        // }
        //https://stackoverflow.com/questions/5794659/how-do-i-set-cell-value-to-date-and-apply-default-excel-date-format


    

    public string matchReturnValue(string find_string, int ret_offset_col, int ret_offset_row, Boolean exact = false) {
       
        ICell cell = findCellContainsValue(find_string, exact); 
        
        if (cell!=null) {
            ICell val_cell = worksheet.GetRow(cell.RowIndex + ret_offset_row).GetCell(cell.ColumnIndex + ret_offset_col);
            if (val_cell!=null) {
              return val_cell.GetFormattedCellValue(evaluator,DateTimeFORMAT2);
            }
        }

        return "";
    }
    public string matchReturnValueCSV(string find_string, int ret_offset_col, int ret_offset_row, int ret_col_count, Boolean exact = false) {
       
        ICell cell = findCellContainsValue(find_string, exact); 
        
        if (cell!=null) {
            int from_col = cell.ColumnIndex + ret_offset_col;
            int to_col =  cell.ColumnIndex + ret_offset_col + ret_col_count;
            int row = cell.RowIndex + ret_offset_row; 
            string csv = RowCSV(row, from_col,to_col,false);
            return csv;
        }

        return "";
    }
    public ICell findCellContainsValue(string value,Boolean exact = false) {
        
        foreach (IRow row in worksheet) {
            if (row.RowNum > MAX_SEARCH_LINES) {
                break;
            }
            foreach (ICell cell in row) {
                string s1 = cell.GetFormattedCellValue(evaluator,DateTimeFORMAT2);
                  if (s1.Equals(value)) {
                  return cell;
                  }
                  if (s1.Contains(value) & exact==false) {
                  return cell;
                  }
            }
        }

        return null;
    }
    public ICell findCellContainsValue(string value, int row_index, int start_col = 0, Boolean exact = false) {
        
        IRow row  = worksheet.GetRow(row_index);
        
        if (row!=null) { 
            foreach (ICell cell in row) {
                if (cell.ColumnIndex >= start_col) {
                    string s1 = cell.GetFormattedCellValue(evaluator,DateTimeFORMAT2);
                    if (s1.Equals(value)) {
                    return cell;
                    }
                    if (s1.Contains(value) & exact==false) {
                        return cell;
                    }
                }
            }
        }

        return null;
    }


public string [] WorksheetToTable() {

    //ISheet copy = worksheet.CopySheet("copy");
    
    string tableCSV = WorksheetToCSV();
  
    string[] lines = tableCSV.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
  
    return lines;
}

    
} 



}
