using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ge_repository.Authorization;


namespace ge_repository.Models {
public class ge_event {
    
      public Guid Id {get;set;}
     [Display(Name = "Created By")] public string createdId {get;set;} 
     [Display(Name = "Created DateTime")] public DateTime createdDT {get;set;}
     [Display(Name = "Message")] public string message {get;set;}
     [Display(Name = "Context")] public string context {get;set;}
      public string returnUrl {get;set;}
     [Display(Name = "log Level")] public logLEVEL level {get;set;}
     
     public virtual ge_user created {get;set;}
}


}
