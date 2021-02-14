using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ge_repository.Authorization;

namespace ge_repository.Models{

    public class ge_user :  IdentityUser {
    public ge_user() : base() { }
       
        [Display(Name = "First Name")] public string FirstName { get; set; }
        [Display(Name = "Last Name")] public string LastName { get; set; }
        [Display(Name = "Last Logged in")] public DateTime LastLoggedIn {get;set;}
    //    public virtual List<ge_user_ops> user_ops {get;set;} 

          
   public ge_user (string firstName, string lastName, string email, string phoneNumber) {
       
        Email = email.ToLower();
        NormalizedEmail = email.ToUpper();
        FirstName = firstName;
        LastName =lastName;
        PhoneNumber = phoneNumber;
        UserName = Email;
   }

   [Display (Name="Full Name")] public String FullName () {
        return (FirstName + " " + LastName);

    }

 }
    public class operation_request {
    public ge_user _user {get;}
    public ge_group _group {get;}
    public ge_project _project {get;}
    public ge_data _data {get;}
     [Display(Name="User Group Operations")] public ge_user_ops _group_user {get;}
     [Display(Name="User Project Operations")] public ge_user_ops _project_user {get;}
    [Display(Name="Effective Group Operations")] public string _effectiveGroup_ops {get; private set;}
    [Display(Name="Effective Project Operations")] public string _effectiveProject_ops {get; private set;}
    [Display(Name="Effective Data Operations")]  public string _effectiveData_ops {get; private set;}
    [Display(Name="Operations Requested")] public string _requested_ops {get; private set;}
    [Display(Name="Operation Object")] public string _requested_object {get;private set;}
    [Display(Name="Operation Result")] public Boolean _requested_result {get; private set;}

    public operation_request (ge_data data, ge_user_ops group_user, ge_user_ops project_user) {
        
        _data = data;
        
        if (data.project !=null) {
        _project = data.project;
            if (data.project.group != null) {
                _group = data.project.group;
            }
        }
        _group_user = group_user;
        _project_user = project_user;
        _requested_ops = _requested_ops; 

        if (group_user.user!=null) {
        _user = group_user.user;
        }
        
        if (project_user.user!=null) {
        _user = project_user.user;
        }

        GetEffectiveOps();

    }
    public operation_request (ge_project project, ge_user_ops group_user, ge_user_ops project_user) {
        
        _project = project;
            
        if (project.group != null) {
            _group = project.group;
        }
        
        _group_user = group_user;
        _project_user = project_user;
        _requested_ops = _requested_ops; 

         if (group_user.user!=null) {
        _user = group_user.user;
        }
        
        if (project_user.user!=null) {
        _user = project_user.user;
        }

        GetEffectiveOps();

    }
    public operation_request (ge_group group, ge_user_ops group_user) {
        
        _group = group;
        _group_user = group_user;
        
        if (group_user.user!=null) {
        _user = group_user.user;
        }
        
        if (group_user.user!=null) {
        _user = group_user.user;
        }

        GetEffectiveOps();

    }
    private string ParentWithMatchedChildItems(string parent, string child) {
        // get all child items that exist in the parent
        string[] child_split = child.Split(";");
        string resp = "";

        foreach (string c in child_split) {
            if (parent.Contains(c)) {
                if (resp.Length>0) resp += ";";
                resp += c;
            }
        }

        return resp;
    }
    public Boolean AreDataOperationsAllowed (string request_ops) {
        _requested_object = "data";
        _requested_ops = request_ops;
        _requested_result = GetResult (request_ops, _effectiveData_ops);
        return _requested_result;
    }
    public Boolean AreProjectOperationsAllowed (string request_ops) {
        _requested_object = "project";
        _requested_ops = request_ops;
        _requested_result = GetResult (request_ops, _effectiveProject_ops);
        return _requested_result;
    }
    public Boolean AreGroupOperationsAllowed (string request_ops) {
        _requested_object = "group";
        _requested_ops = request_ops;
        _requested_result = GetResult (request_ops, _effectiveGroup_ops);
        return _requested_result;

    }
    private static Boolean GetResult (string requested_ops, string effective_ops) {
               
        Boolean result = false; 

        string[] req_array = requested_ops.Split(";");
        
        foreach (string op in req_array) {
            if (effective_ops.Contains(op) == false) {
                result = false;
                return result;
            }
        }
        
        result = true;

        return result;

    }
    public void GetEffectiveOps() {
            
            string UserGroupOps = "";
            string GroupOps = "";
           
            string UserProjectOps = "";
            string ProjectOps = "";
            string ProjectUserOps = "";
           
            string DataOps = "";
            
            if (_group != null ) {
                GroupOps =  _group.project_operations;
                if (_group_user != null) {
                    UserGroupOps = _group_user.user_operations;
                    _effectiveGroup_ops = ParentWithMatchedChildItems(GroupOps, UserGroupOps);
                } else {
                    _effectiveGroup_ops = GroupOps;
                }
            } else {
                _effectiveGroup_ops =  "";
            }
            
            if (_project != null ) { 
                ProjectOps = _project.data_operations;
                if (_project_user !=null) {
                    UserProjectOps = _project_user.user_operations;
                    ProjectUserOps = ParentWithMatchedChildItems(ProjectOps, UserProjectOps);
                } else {
                    ProjectUserOps =  ProjectOps;
                }
            } else {
                ProjectUserOps = "";
            }  

            _effectiveProject_ops = ParentWithMatchedChildItems(_effectiveGroup_ops,ProjectUserOps);

           if (_data != null) {
                DataOps = _data.operations;
                _effectiveData_ops = ParentWithMatchedChildItems(_effectiveProject_ops,DataOps);
           } else {
               _effectiveData_ops = _effectiveProject_ops;
           }
    }
    
}

 public class ge_user_ops : _ge_base {
    public Guid Id {get;set;}
    [Display(Name="User Id")] public string userId{get;set;} 
    virtual public ge_user user {get;set;}
    [Display(Name ="User Operations")] public string user_operations {get;set;}
     [Display(Name="Project Id")] public Guid? projectId{get;set;} 
    virtual public ge_project project {get;set;}
     [Display(Name="Group Id")] public Guid? groupId{get;set;} 
    virtual public ge_group group {get;set;}        
     public Boolean AddOperations(string operations) {
        string[] ops = operations.Split (";");
        foreach (var op in ops) {
            if (IsValidOperation(op) && !operations.Contains (op)) {
                operations += ";" + op;
            }
        }
        
        return false;
      }

      private Boolean IsValidOperation(string op) {
          
         if (op == Constants.CreateOperationName ||
                op == Constants.ReadOperationName   ||
                op == Constants.UpdateOperationName ||
                op == Constants.DeleteOperationName ||
                op == Constants.DownloadOperationName ||
                op == Constants.RejectOperationName ||
                op == Constants.ApproveOperationName) {
                    return true;
                }
        return false;
     }

       public Boolean HasOperation(string op) {
           return operations.Contains(op);
       } 
       

 }
 }