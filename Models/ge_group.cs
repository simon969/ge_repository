using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ge_repository.Authorization;

namespace ge_repository.Models {

public class ge_group : _ge_location {

        public Guid Id {get;set;}
         [Display(Name = "Group Name")] [StringLength(255)] public string name{get;set;}
        [Display(Name ="Group Data Manager")] [StringLength(450)] public string managerId {get;set;}
        [Display(Name ="Home Page")] public Guid? homepageId {get;set;}
        public virtual ge_user manager {get;set;}
        public virtual List<ge_project> projects {get;set;}
        public virtual List<ge_user_ops> users { get; set; }
        [Display(Name = "Project Operations Allowed")] [StringLength(255)] public string project_operations {get;set;}
       

}


}




