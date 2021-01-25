using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ge_repository.Models;
using ge_repository.interfaces;

using ge_repository.services;

using Microsoft.AspNetCore.Mvc;

namespace ge_repository.OtherDatabase  { 



public class ge_log_client {

    public Guid Id {get;set;}
    public ge_data data_file {get;set;}
    public ge_log_file log_file {get;set;}
    public Guid? templateId {get;set;}
    public ge_search template {get;set;}
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

    public Boolean save_log {get;set;} = false;

    public Boolean save_MOND {get;set;}= false;

    public ge_log_client (IDataService dataService, ILoggerFileService logService, IGintService<MOND> gintservice) {
        _dataService = dataService;
        _logService = logService;
        _gintService = gintservice;
    }

public enum enumStatus
        {  
            Start,
            loadDataFile,
            loadTemplateFile,
            readLogFile,
            saveLogFile,
            loadLogFile,
            createMOND,
            updateMOND,
            Stop
        }


 public ge_log_client.enumStatus start() {
 
            while (status != enumStatus.Stop) {
                
                if (status == enumStatus.Start || status == enumStatus.loadDataFile) {
                    loadDataFile();
                }

                if (status == enumStatus.loadTemplateFile) {
                    loadTemplateFile();
                }

                if (status == enumStatus.readLogFile) {
                    readLogFile();
                }
                
                if (status == enumStatus.saveLogFile) {
                    saveLogFile();
                }

                if (status == enumStatus.Start || status == enumStatus.loadLogFile) {
                    loadLogFile();
                }
                               
                if (status == enumStatus.createMOND) {
                    createMOND();
                }

                if (status == enumStatus.updateMOND) {
                    updateMOND();
                }
        
        
        }

        return status;
 }
    public void readLogFile() {

    
    
    }
    public int loadDataFile() {
            
        //    var _data =  _context.ge_data
        //                             .Include(d =>d.project)
        //                             .SingleOrDefaultAsync(m => m.Id == Id);
           
           return 1; 
    
    
    }
    public void loadTemplateFile() {

    
    
    }
    public void saveLogFile() {

    
    
    }

    private async Task<IActionResult> ReadFile(Guid Id,
                                          Guid templateId,
                                          string table = "",
                                          string sheet = "") {
            
            // var template = await new ge_dataController(  _context,
            //                                         _authorizationService,
            //                                         _userManager,
            //                                         _env ,
            //                                         _ge_config).getDataAsClass<ge_search>(templateId);
            // if (template==null) {
            // return BadRequest ($"Unable to load templateId {templateId} as a ge_search object");
            // }   

            // var resp_file = await new ge_dataController( _context,
            //                                     _authorizationService,
            //                                     _userManager,
            //                                     _env ,
            //                                     _ge_config).Get(Id);
            
            // var okResult = resp_file as OkObjectResult;    

            // if (okResult==null) {
            //     return BadRequest ($"Unable to load ge_logger file Id {Id}");
            // }
                        
            // ge_data file  = okResult.Value as ge_data;
            
            // string[] lines = null;
            // ge_search template_loaded = null;

            // if (file.fileext == ".csv") {
            //     lines = await new ge_dataController( _context,
            //                                         _authorizationService,
            //                                         _userManager,
            //                                         _env ,
            //                                         _ge_config).getDataByLines(Id);
            //     SearchTerms st = new SearchTerms();
            //     template_loaded = st.findSearchTerms(template,table, lines);
            //     if (template_loaded.search_tables.Count==0) {
            //         return BadRequest(template_loaded);
            //     }
            // }

            // if (file.fileext == ".xlsx") {
            //     using (MemoryStream ms = await new ge_dataController(  _context,
            //                                     _authorizationService,
            //                                     _userManager,
            //                                     _env ,
            //                                     _ge_config).GetMemoryStream(Id)) {
            //         ge_log_workbook wb = new ge_log_workbook(ms);
            //         SearchTerms st = new SearchTerms();  
            //         template_loaded  =  st.findSearchTerms (template, table, wb, sheet);
            //         if (template_loaded.search_tables.Count==0) {
            //             return BadRequest(template_loaded);
            //         }
            //         lines = wb.WorksheetToTable();
            //         wb.close();

            //     }
            // }

            // log_file = getNewLoggerFile (template_loaded, lines);

            // if (log_file==null) {
            //     template_loaded.status = $"Unable to process logger file {file.filename} please check the ge_search template for finding the required header fields";
            //     return BadRequest(template_loaded);
            // }

            // log_file.calcReadingAggregates();
            // log_file.dataId = Id;
            // log_file.templateId = templateId;

            // return Ok(log_file);
            return null;

}
    private async Task<IActionResult> ReadFile() {

        //    var read_resp = await ReadFile(Id, templateId, table, sheet);
           
        //    var okResult = read_resp as OkObjectResult;   
            
        //     if (okResult == null) {
        //         return (read_resp);
        //     }

        //     if (okResult.StatusCode != 200) {
        //         return (read_resp);
        //     }

        //     if (okResult.StatusCode == 200) {
        //         log_file = okResult.Value as ge_log_file;
        //     }

        //    ge_log_helper gf = new ge_log_helper();

        //    gf.log_file = log_file;

        //    gf.AddOverrides (probe_depth, bh_ref);
        return null;
    }

    public void loadLogFile() {

    
    
    }
    public void createMOND() {

    
    
    }
    public void updateMOND() {

    
    
    }
         public  void setProcessFlag(int value) {
                // set process flag = 1 so that the user cannot rerun another ags conversion before this one has completed
                // data_file.pflag = value;
                // _context.Attach(data_file).State = EntityState.Modified;
                // _context.SaveChanges();
        }

        
}
}

 