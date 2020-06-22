using System;
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace ge_repository.OtherDatabase
{

    public class ge_log_workbook {
    public HSSFWorkbook hssfworkbook {get;}
    public ISheet hssfsheet{get;}
    public ICellRange<ICell> headers {get;}
    public ICellRange<ICell> data {get;}

    public int current_data_row;
    public int start_data_row;
    public int end_data_row;    
        
    public ge_log_workbook(FileStream fs) {
        hssfworkbook = new HSSFWorkbook(fs);
    }
    public ge_log_workbook(MemoryStream ms) {
        hssfworkbook = new HSSFWorkbook(ms);
    }
    public IRow NextRow() {
        current_data_row++;
        if (current_data_row<=end_data_row) {
            return hssfsheet.GetRow(current_data_row);
        }
        return null;
    }
    public IRow FirstRow() {
        current_data_row = start_data_row;
        return hssfsheet.GetRow(current_data_row);
    }

    public object getValue(int column) {
        ICell cell = hssfsheet.GetRow(current_data_row).GetCell(column);
        return getCellValue(cell);
    }

    public ISheet findWorksheetName (string name) {
        return hssfworkbook.GetSheet (name);
    }

    public ICell findCellValue(ICellRange<ICell> range, string value) {
        
        foreach (ICell cell in range) {
            if (cell.StringCellValue ==  value) {
                return cell;
            }
        }
     
        return null;
    }

   private object getCellValue(ICell cell)
{
    object cValue = string.Empty;
    switch (cell.CellType)
    {
        case (CellType.Unknown | CellType.Formula | CellType.Blank):
            cValue = cell.ToString();
            break;
        case CellType.Numeric:
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

public string [] getTable(string name) {

    StringBuilder sb = new StringBuilder();







    string[] lines = sb.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
    return lines;

}

    // class Program
    // {
    //     static HSSFWorkbook hssfworkbook;

    //     static void Main(string[] args)
    //     {
    //         InitializeWorkbook();

    //         ISheet s = hssfworkbook.GetSheetAt(0);

    //         ICell cell = s.GetRow(4).GetCell(1);
    //         cell.CopyCellTo(3); //copy B5 to D5

    //         IRow c = s.GetRow(3);
    //         c.CopyCell(0, 1);   //copy A4 to B4

    //         s.CopyRow(0, 1);     //copy row A to row B, original row B will be moved to row C automatically
    //         WriteToFile();
    //     }

    //     static void WriteToFile()
    //     {
    //         //Write the stream data of workbook to the root directory
    //         FileStream file = new FileStream(@"test.xls", FileMode.Create);
    //         hssfworkbook.Write(file);
    //         file.Close();
    //     }

    //     static void InitializeWorkbook()
    //     {
    //         using (var fs = File.OpenRead(@"Data\test.xls"))
    //         {
    //             hssfworkbook = new HSSFWorkbook(fs);
    //         }
    //     }
    // }
} 



}
