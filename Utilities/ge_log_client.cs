using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ge_repository.Models;
using ge_repository.interfaces;
using ge_repository.Authorization;
using ge_repository.services;
using ge_repository.repositories;
namespace ge_repository.OtherDatabase  { 

// https://blog.hildenco.com/2018/12/accessing-entity-framework-context-on.html

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
    public IMONDService _mondService {get;set;}
    private ge_DbContext _context {get;}
    public string table {get;set;}
    public string ge_source {get;set;}
    public string sheet {get;set;}
    public string bh_ref  {get;set;}
    public float? probe_depth  {get;set;}
    public string round_ref {get;set;}  

   
    public Boolean saveByAction {get;set;} = false;
    public DateTime? fromDT {get;set;}
    public DateTime? toDT {get;set;}
    private DateTime _started; 
    private DateTime _started_action; 
    private string _result {get;set;}
    public string userId {get;set;}
    private static int NOT_OK = -1;
    private static int OK = 1;
    
    public Boolean read_logger {get;set;} = true;
    public Boolean save_logger {get;set;} = true;
    public Boolean apply_overrides {get;set;} = true;
    public Boolean bulk_update = true;

    public string options {get;set;}

    public Boolean save_MOND {get;set;}= false;

    public ge_log_client (IDataService dataService, ILoggerFileService logService, IMONDService mondservice) {
        _dataService = dataService;
        _logService = logService;
        _mondService = mondservice;
    }
    public ge_log_client (IServiceScopeFactory serviceScopeFactory, dbConnectDetails connectLogger, dbConnectDetails connectGint) {

            var scope = serviceScopeFactory.CreateScope() ;

            _context = scope.ServiceProvider.GetService<ge_DbContext>();

            IUnitOfWork _unit = new UnitOfWork(_context); 
            _dataService = new DataService(_unit);
           
            IGintUnitOfWork _gunit = new GintUnitOfWork(connectGint);
            _mondService = new MONDService (_gunit);
            
            ILoggerFileUnitOfWork _lunit = new LogUnitOfWork(connectLogger);
            _logService = new LoggerFileService (_lunit);
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

            
            status = enumStatus.Start;
            
            await init_actions( true);

            while (status != enumStatus.Stop) {
                
                if (status == enumStatus.Start) {
                    await actionStarted("loadDataFile");
                    int resp =  await ensureDataFile();
                    await actionEnded("loadDataFile",resp);
                    if (resp== NOT_OK) {status = enumStatus.Stop;}
                    if (resp >= OK) status = enumStatus.DataFileLoaded;
                }
                
                if (status == enumStatus.DataFileLoaded && read_logger==true ) {
                    await actionStarted("loadTemplateFile");
                    int resp = await loadTemplateFile();
                    await actionEnded("loadTemplateFile",resp);
                    if (resp== NOT_OK) {status = enumStatus.Stop;}
                    if (resp >= OK) status = enumStatus.TemplateFileLoaded;
                }

                if (status == enumStatus.TemplateFileLoaded && read_logger==true) {
                    await actionStarted("loadTemplateAndLines");
                    int resp =  await loadTemplateAndLines();
                    await actionEnded("loadTemplateAndLines",resp);
                    if (resp== NOT_OK) {status = enumStatus.Stop;}
                    if (resp >= OK) status = enumStatus.TemplateAndLinesRead;
                }

                if (status == enumStatus.TemplateAndLinesRead && read_logger==true ) {
                    await actionStarted("NewFileLogger");
                    int resp = NewFileLogger();
                    await actionEnded("NewFileLogger",resp);
                    if (resp== NOT_OK) {status = enumStatus.Stop;}
                    if (resp >= OK) status = enumStatus.LoggerFileLoaded;
                }
                
                if (status == enumStatus.LoggerFileLoaded && save_logger==true) {
                    await actionStarted("saveFileLogger");
                    int resp = await saveFileLogger();
                    await actionEnded("saveFileLogger",resp);
                    if (resp== NOT_OK) status = enumStatus.Stop;
                    if (resp >= OK) status = enumStatus.LoggerFileSaved;
                }

                if (status == enumStatus.DataFileLoaded && read_logger==false) {
                    await actionStarted("loadFileLogger");
                    int resp = await loadFileLogger();
                    await actionEnded("loadFileLogger",resp);
                    if (resp== NOT_OK) {status = enumStatus.Stop;}
                    if (resp >= OK) {status = enumStatus.LoggerFileLoaded;}
                }
                               
                if (status == enumStatus.LoggerFileLoaded || status == enumStatus.LoggerFileSaved) {
                    await actionStarted("createMOND");
                    int resp = await createMOND();
                    await actionEnded("createMOND",resp);
                    if (resp== NOT_OK) {status = enumStatus.Stop;}
                    if (resp >= OK) status = enumStatus.MONDcreated;
                }

                if (status == enumStatus.MONDcreated) {
                    status = enumStatus.Stop;
                }
        
        }
        
        await close_actions();
        
        return status;
    
 }
    private async Task init_actions( Boolean SaveByAction) {
        
        saveByAction = SaveByAction;
        
        var resp = await ensureDataFile();
        
        if (options!=null) {
        if (options.Contains("read_logger")) read_logger = true;
        if (options.Contains("save_logger")) save_logger = true;
        if (options.Contains("apply_overrides")) apply_overrides = true;
        if (options.Contains("bulk_update")) bulk_update = true;
        }
        _started = DateTime.Now;
        _result =  $"{_started} Background ge_log_client started with";
        _result += $" sheet='{sheet}' table='{table}' bh_ref='{bh_ref}'";
        _result += $" probe_depth='{probe_depth}' round_ref='{round_ref}'" + System.Environment.NewLine;

        data_file.pflag = pflagCODE.PROCESSING;
        
        if (saveByAction==true) {
            data_file.pflag = pflagCODE.PROCESSING;
            data_file.phistory += _result;
            await _dataService.UpdateData (data_file);
        }


    }
    private async Task close_actions() {
        
        DateTime ended = DateTime.Now;
        TimeSpan dur = ended - _started;

         string s1 =  $"{ended} Background ge_log_client exited {String.Format("{0:0.000}",dur.TotalMinutes)} mins" + System.Environment.NewLine;
         _result += s1;
         
        data_file.pflag = pflagCODE.NORMAL;
         
        if (saveByAction==true) {
            data_file.phistory += s1;
        } else {
            data_file.phistory += _result;
        }   
        await _dataService.UpdateData (data_file);
    }

    private async Task actionStarted (string s1) {
        _started_action = DateTime.Now;
        string s2 = $"{_started_action} {s1} started" + System.Environment.NewLine;
        _result += s2;

        if (saveByAction==true) {
            data_file.pflag = pflagCODE.PROCESSING;
            data_file.phistory += s2;
            await _dataService.UpdateData(data_file);
        }   
    }
    
    private async Task actionEnded (string s1, int i) {
        string outcome = "";
        DateTime ended = DateTime.Now;
        TimeSpan dur = ended - _started_action;
        if (i == NOT_OK) outcome="Not Ok";
        if (i >= OK) outcome="Ok";

        string s2 = $"{DateTime.Now} {s1} ended {String.Format("{0:0.000}",dur.TotalMinutes)} mins result ({i}) {outcome}" + System.Environment.NewLine;
        _result += s2;
        
        if (saveByAction==true) {
          data_file.phistory += s2;
          await _dataService.UpdateData(data_file);
        }  
    }
    
    private async Task<int> loadTemplateAndLines () {

        // string[] lines = null;
        // ge_search template_loaded = null;
        
        if (data_file.fileext == ".csv") {
                lines = await _dataService.GetFileAsLines(data_file.Id);
                SearchTerms st = new SearchTerms();
                template_loaded = st.findSearchTerms(template,table, lines);
                if (template_loaded.search_tables.Count==0) {
                    return -1;
                }
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
                
                if (template_loaded.search_tables.Count==0) {
                    return -1;
                }
                
                wb.setWorksheet(template_loaded.search_tables[0].sheet);
                wb.evaluateSheet();
                lines = wb.WorksheetToTable();
                wb.close();
            }
        }
        
        return lines.Count();

  }
    
    
    private async Task<int> ensureDataFile() {
            


            if (Id == null) {
            return -1;
            }
            
            if (data_file != null) {
                if (data_file.Id == Id) {;
                    return 1;
                }
            }


           data_file = await _dataService.GetDataById(Id);
           
            if (data_file == null) {
               return -1;
            }
           
            return 1; 
    
    
    }
    private async Task<int> loadTemplateFile() {
        
        if (templateId == null) {
            return -1;
        }

        template = await _dataService.GetFileAsClass<ge_search>(templateId.Value);
        
        if (template == null) {
            return -1;
        }

        return 1; 
    
    }
    private async Task<int> saveFileLogger() {

        ge_log_file exist = await _logService.GetByDataId(Id, table);
    
        if (exist != null) {
            log_file.Id = exist.Id;
            return await _logService.UpdateLogFile(log_file, true);
        } else {
            return await _logService.CreateLogFile (log_file);
        }

    }
    private async Task<int> NewFileLoggerAsync() {

            log_file = await _logService.NewLogFile(  Id,
                                                templateId.Value,
                                                table,
                                                sheet,
                                                _dataService);
            
            
            if (log_file == null) {
                return -1;
            }

            if (apply_overrides==true) {
            ge_log_helper gf = new ge_log_helper();
            gf.log_file = log_file;
            gf.AddOverrides (probe_depth, bh_ref);
            }

          return log_file.readings.Count();

    }
      
    private int NewFileLogger() {

            log_file = _logService.NewLogFile( template_loaded,
                                                    lines,
                                                    Id,
                                                    templateId.Value);
            
            if (log_file == null) {
                return -1;
            }

            if (apply_overrides==true) {
            ge_log_helper gf = new ge_log_helper();
            gf.log_file = log_file;
            gf.AddOverrides (probe_depth, bh_ref);
            }
            
            return log_file.readings.Count();

    }

    private async Task<int> loadFileLogger() {

            log_file = await _logService.GetByDataId(Id, table);

            if (log_file == null) {
                return -1;
            }
           
            if (apply_overrides==true) {
                ge_log_helper gf = new ge_log_helper();
                gf.log_file = log_file;
                gf.AddOverrides (probe_depth, bh_ref);
            }

            if (log_file.readings==null){
                return 1;
            }
            return log_file.readings.Count();
  
    }
    private async Task<int> createMOND(){


        var mond = await _mondService.CreateMOND(  log_file, 
                                                        ge_source,
                                                        round_ref,
                                                        fromDT,
                                                        toDT,
                                                        save_MOND);

        if (mond == null) {
            return -1;
        }

        return mond.Count();
    }

         
}
}

 