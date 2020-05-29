using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;

namespace ge_repository.Models {

    public abstract class _ge_base {
    
        public virtual ge_user created {get;set;} 
        [Display(Name = "Created By")] public string createdId {get;set;} 
        [Display(Name = "Created DateTime")] public DateTime createdDT {get;set;}
     
        public virtual ge_user edited {get;set;}
        [Display(Name = "Last Edited By")] public string editedId {get;set;} 
        [Display(Name = "Last Edited DateTime")] public DateTime? editedDT {get;set;}
        [Display(Name = "Operations Allowed")] [StringLength(255)] public string operations {get;set;}
        [Display (Name = "Processing Flag")] public int pflag {get;set;}
        
    }
       



}  