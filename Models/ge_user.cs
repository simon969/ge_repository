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