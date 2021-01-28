using System;
using System.IO;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.interfaces;

namespace ge_repository.OtherDatabase  { 



public class ge_log_client {

    public Guid Id {get;set;}
    public ge_data data_file {get;set;}
    public ge_log_file log_file {get;set;}
    public Guid? templateId {get;set;}
    public ge_search template {get;set;}
    public ge_search template_loaded {get;set;}
    public string[] lines {get;set;}
    public enumStatus status {get;set;}
    public IDataService _dataService {get;set;}
    public ILoggerFileService _logService {get;set;}
    public IGintService<MOND> _gintService {get;set;}
    public string table {get;set;}
    public string sheet {get;set;}
    public string bh_ref  {get;set;}
    public float? probe_depth  {get;set;}
    public string round_ref {get;set;}  
    public string userId {get;set;}
    private static int NOT_FOUND = -1;
    private static int OK = 1;

    public Boolean read_logger {get;set;} = true;
    public Boolean save_logger {get;set;} = true;
    public Boolean apply_overrides {get;set;} = true;

    public Boolean save_MOND {get;set;}= false;

    public ge_log_client (IDataService dataService, ILoggerFileService logService, IGintService<MOND> gintservice) {
        _dataService = dataService;
        _logService = logService;
        _gintService = gintservice;
    }

    public enum enumStatus
        {  
            Start,
            DataFileLoaded,
            TemplateFileLoaded,
            TemplateAndLinesRead,
            LoggerFileSaved,
            LoggerFileLoaded,
            MONDcreated,
            MONDupdated,
            Stop
        }


 public async Task<ge_log_client.enumStatus> start() {
 
            while (status != enumStatus.Stop) {
                
                if (status == enumStatus.Start) {
                    int resp = await loadDataFile();
                    if (resp==OK) status = enumStatus.DataFileLoaded;
                }
                
                if (status == enumStatus.DataFileLoaded && read_logger==true ) {
                    int resp = await loadTemplateFile();
                    if (resp==OK) status = enumStatus.TemplateFileLoaded;
                }

                if (status == enumStatus.TemplateFileLoaded && read_logger==true) {
                    int resp = await loadTemplateAndLines();
                    if (resp==OK) status = enumStatus.TemplateAndLinesRead;
                }

                if (status == enumStatus.TemplateAndLinesRead && read_logger==true ) {
                    int resp = createFileLogger();
                    if (resp==OK) status = enumStatus.LoggerFileLoaded;
                }
                
                if (status == enumStatus.LoggerFileLoaded && save_logger==true) {
                    int resp = await saveFileLogger();
                    if (resp==OK) status = enumStatus.LoggerFileSaved;
                }

                if (status == enumStatus.DataFileLoaded && read_logger==false) {
                    int resp = await loadFileLogger();
                    if (resp==OK) status = enumStatus.LoggerFileLoaded;
                }
                               
                if (status == enumStatus.LoggerFileLoaded || status == enumStatus.LoggerFileSaved) {
                    int resp = createMOND();
                    if (resp==OK) status = enumStatus.MONDcreated;
                }

                if (status == enumStatus.MONDcreated && save_MOND==true) {
                    int resp = await updateMOND();
                    if (resp==OK) status = enumStatus.MONDupdated;
                }

                if (status == enumStatus.MONDupdated && save_MOND==true) {
                    status = enumStatus.Stop;
                }

                if (status == enumStatus.MONDcreated && save_MOND==false) {
                    status = enumStatus.Stop;
                }
        
        }

        return status;
 }
    


    
    private async Task<int> loadTemplateAndLines () {

        // string[] lines = null;
        // ge_search template_loaded = null;
        
        if (data_file.fileext == ".csv") {
                var file_lines = await _dataService.GetFileAsLines(data_file.Id);
                lines = file_lines;
                SearchTerms st = new SearchTerms();
                template_loaded = st.findSearchTerms(template,table, lines);
        }

        if (data_file.fileext == ".xlsx") {
            using (MemoryStream ms = await _dataService.GetFileAsMemoryStream(data_file.Id)) {
                ge_log_workbook wb = new ge_log_workbook(ms);
                SearchTerms st = new SearchTerms();  
                if (sheet.Contains(",")) {
                string[] sheets = sheet.Split (",");
                template_loaded =  st.findSearchTerms (template, table, wb, sheets);  
                } else {
                template_loaded  =  st.findSearchTerms (template, table, wb, sheet);
                }
                wb.setWorksheet(template_loaded.search_tables[0].sheet);
                lines = wb.WorksheetToTable();
                wb.close();
            }
        }
        
        return 1;

  }
    
    
    public async Task<int> loadDataFile() {
            
            if (Id == null) {
            return -1;
            }
           
           data_file = await _dataService.GetDataById(Id);
           
           return 1; 
    
    
    }
    public async Task<int> loadTemplateFile() {
        
        if (templateId == null) {
            return -1;
        }

        template = await _dataService.GetFileAsClass<ge_search>(templateId.Value);
           
        return 1; 
    
    }
    public async Task<int> saveFileLogger() {

        ge_log_file exist = await _logService.GetByDataId(log_file.Id, table);

        if (exist != null) {
            await _logService.UpdateLogFile(exist, log_file);
        } else {
            await _logService.CreateLogFile (log_file);
        }

        return 1;
    }

      
    public int createFileLogger() {

            log_file = _logService.CreateLogFile( template_loaded,
                                                    lines,
                                                    Id,
                                                    templateId.Value);
            if (apply_overrides==true) {
            ge_log_helper gf = new ge_log_helper();
            gf.log_file = log_file;
            gf.AddOverrides (probe_depth, bh_ref);
            }

            return 1;

    }

    public async Task<int> loadFileLogger() {

            log_file = await _logService.GetByDataId(Id, table);
           
            if (apply_overrides==true) {
                ge_log_helper gf = new ge_log_helper();
                gf.log_file = log_file;
                gf.AddOverrides (probe_depth, bh_ref);
            }

            return 1;
  
    }

    public int createMOND() {

    
            return 1;
    }
    public async Task<int> updateMOND() {

    
            return 1;
    }
    public  async void setProcessFlag(int value) {

        // set process flag = 1 so that the user cannot rerun another ags conversion before this one has completed
         data_file.pflag = value;
         await _dataService.UpdateData(data_file);
    }

        
}
}

 