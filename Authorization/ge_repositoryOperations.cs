using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using ge_repository.Models;

namespace ge_repository.Authorization
{

    public static class geOPS
    {
        public static OperationAuthorizationRequirement Create =   
          new OperationAuthorizationRequirement {Name=Constants.CreateOperationName};
        public static OperationAuthorizationRequirement Read = 
          new OperationAuthorizationRequirement {Name=Constants.ReadOperationName};  
        public static OperationAuthorizationRequirement Update = 
          new OperationAuthorizationRequirement {Name=Constants.UpdateOperationName}; 
        public static OperationAuthorizationRequirement Delete = 
          new OperationAuthorizationRequirement {Name=Constants.DeleteOperationName};
        public static OperationAuthorizationRequirement Approve = 
          new OperationAuthorizationRequirement {Name=Constants.ApproveOperationName};
        public static OperationAuthorizationRequirement Reject = 
          new OperationAuthorizationRequirement {Name=Constants.RejectOperationName};
        public static OperationAuthorizationRequirement Download = 
          new OperationAuthorizationRequirement {Name=Constants.DownloadOperationName}; 
        public static OperationAuthorizationRequirement Admin = 
          new OperationAuthorizationRequirement {Name=Constants.AdminOperationName};        
    }
   /*  public static class geOPS {
        public static readonly string Create = "Create";
        public static readonly string Read = "Read";
        public static readonly string Update = "Update";
        public static readonly string Delete = "Delete";
        public static readonly string Approve = "Approve";
        public static readonly string Admin = "Admin";
        public static readonly string Download = "Download";
    } */
    public class Constants
    {
        public static readonly string CreateOperationName = "Create";
        public static readonly string ReadOperationName = "Read";
        public static readonly string UpdateOperationName = "Update";
        public static readonly string DeleteOperationName = "Delete";
        public static readonly string ApproveOperationName = "Approve";
        public static readonly string RejectOperationName = "Reject";
        public static readonly string DownloadOperationName = "Download";
        public static readonly string AdminOperationName = "Admin";
        public static readonly string ge_repositoryAdministratorRole = "ge_administrator";
        public static readonly string ge_repositoryManagerRole = "ge_manager";
        public static readonly string ge_repositoryProjectRole = "ge_project";

        
/*         public enum PublishStatus {
        UncontrolledPrivate,
        UncontrolledOffice,
        UncontrolledProject,
        ApprovedOffice,
        ApprovedProject
        } */
        public enum PublishStatus {
        Uncontrolled,
        Approved
        
        }
        
        public enum ConfidentialityStatus {
        Owned,
        RequiresClientApproval,
        ThirdParty,
        ClientApproved
        }
        public enum QualitativeStatus {
        ThirdPartyFactual,
        ThirdPartyInterpretive,
        AECOMInterpretive,
        AECOMFactual
        }

        public enum VersionStatus {
          Final,
          Draft,
          Intermediate
        }


        // project and group user operations
        // project_user.user_operations 
        // group_user.user_operations

        public static string[] CRUDDAA_OperationsArray = new string[] {
                                          "Read",
                                          "Read;Download",
                                          "Create;Read;Download",
                                          "Create;Read;Update;Download",
                                          "Create;Read;Update;Download;Delete",
                                          "Create;Read;Update;Download;Delete;Approve;Admin"};
         public static string[] CRUDDAA_OperationsArrayIndividual = new string[] {
                                          "Create","Read","Update","Download","Delete","Admin","Approve"};
        // group and project record operations
        //      ge_group.operation
        //      ge_project.operation
        public static string[] RUD_OperationsArray = new string[] {
                                          "Read",
                                          "Read;Update",
                                          "Read;Update;Delete"}; 
        // data record operations
        //      ge_data.operation 
        public static string[] RUDD_OperationsArray = new string[] {
                                          "Read",
                                          "Read;Download",
                                          "Read;Download;Update",
                                          "Read;Download;Update;Delete"}; 
        
        // project data operations
        //      ge_project.data_operation
       
        public static string[] CRUDD_OperationsArray = new string[] {
                                          "Read",
                                          "Read;Download",
                                          "Create;Read;Download",
                                          "Create;Read;Update;Download",
                                          "Create;Read;Update;Download;Delete"};
        // project operations
        // ge_group.project_operations
        public static string[] CRUD_OperationsArray = new string[] {
                                          "Read",
                                          "Create;Read",
                                          "Create;Read;Update;",
                                          "Create;Read;Update;Delete"};
        
        
        public static int ReadDownloadUpdateDelete  = 3;
        public static int ReadDownloadUpdate  = 2;
        public static int ReadDownload  = 1;
        public static int Read  = 0;

        public enum datumProjection {
       
        // No assigned projection system
        NONE = 0, 
        
        // http://spatialreference.org/ref/epsg/osgb-1936-british-national-grid/  
        // OSGB 1936 British National Grid"  
        OSGB36NG = 27700,
        OSGB36NGODN = 7405,
        
        // WGS84 is used by multiple spacial reference systems
        //http://spatialreference.org/ref/?search=WGS84&srtext=Search
         WGS84 = 101,
        
        // GRS80 is used by multiple spacial reference systems
        //http://spatialreference.org/ref/?search=GRS80&srtext=Search
        GRS80 = 102
      }
  }
      public enum logLEVEL {
        Info,
        Warning,
        Error,
        Fatal
      }
    public static class pflagCODE {
         public const int NORMAL = 0;
         public const int PROCESSING = -1;
    }


      
      public static class msgCODE {

        public const int DATA_UPDATE_USER_PROHIBITED = 102;
        public const int DATA_STATUS_USER_PROHIBITED = 108;
        public const int DATA_READ_USER_PROHIBITED = 103;
        public const int DATA_CREATE_USER_PROHIBITED = 104;
        public const int DATA_DELETE_USER_PROHIBITED = 107;
        public const int DATA_DOWNLOAD_USER_PROHIBITED = 109;
        public const int DATA_OPERATION_UPDATE_PROHIBITED = 111;
        public const int DATA_UPDATE_PROHIBITED = 112;
        public const int DATA_STATUS_PROHIBITED = 118;
        public const int DATA_READ_PROHIBITED = 113;
        public const int DATA_CREATE_PROHIBITED = 114;
        public const int DATA_DELETE_PROHIBITED = 117;
        public const int DATA_DOWNLOAD_PROHIBITED = 119;


        public const int PROJECT_OPERATION_CREATE_ADMINREQ = 501;
        public const int PROJECT_OPERATION_UPDATE_ADMINREQ = 502;
        public const int PROJECT_OPERATION_DELETE_ADMINREQ = 503;
        public const int PROJECT_OPERATION_UPDATE_MINADMIN = 504;
        public const int PROJECT_OPERATION_CREATE_USEREXITS = 505;

        public const int PROJECT_UPDATE_USER_PROHIBITED = 202;
        public const int PROJECT_READ_USER_PROHIBITED = 203;
        public const int PROJECT_APPROVE_USER_PROHIBITED = 204;
        public const int PROJECT_CREATE_USER_PROHIBITED = 205;
        public const int PROJECT_DELETE_USER_PROHIBITED = 207;
        public const int PROJECT_UPDATE_PROHIBITED = 212;
        public const int PROJECT_READ_PROHIBITED = 213;
        public const int PROJECT_APPROVE_PROHIBITED = 214;
        public const int PROJECT_CREATE_PROHIBITED = 215;
        public const int PROJECT_DELETE_PROHIBITED = 217;
        public const int GROUP_OPERATION_READ_ADMINREQ = 321;
        public const int GROUP_OPERATION_CREATE_ADMINREQ = 322;
        public const int GROUP_OPERATION_UPDATE_ADMINREQ = 323;
        public const int GROUP_OPERATION_DELETE_ADMINREQ = 324;
        public const int GROUP_OPERATION_UPDATE_MINADMIN = 325;
        public const int GROUP_OPERATION_CREATE_USEREXITS = 326;
        public const int GROUP_OPERATION_USERNOTEXIST = 327;
        public const int GROUP_READ_USER_PROHIBITED = 301;
        public const int GROUP_CREATE_USER_PROHIBITED = 302;
        public const int GROUP_UPDATE_USER_PROHIBITED = 303;
        public const int GROUP_DELETE_USER_PROHIBITED = 304;
        public const int GROUP_READ_PROHIBITED = 311;
        public const int GROUP_CREATE_PROHIBITED = 312;
        public const int GROUP_UPDATE_PROHIBITED = 313;
        public const int GROUP_DELETE_PROHIBITED = 314;
        public const int GROUP_NOT_EMPTY = 315;
        public const int USER_CREATE_NOTFOUND = 601;
        public const int USER_OPS_CREATE_AMBIGUOUS = 602;
        public const int USER_OPS_NOTFOUND = 603;
        public const int USER_NOTFOUND = 604;
        public const int TRANSFORM_READ_PROHIBITED= 401; 
        public const int TRANSFORM_READ_USER_PROHIBITED = 411; 
        public const int TRANSFORM_CREATE_PROHIBITED= 402; 
        public const int TRANSFORM_CREATE_USER_PROHIBITED = 412; 
        public const int TRANSFORM_UPDATE_PROHIBITED= 403; 
        public const int TRANSFORM_UPDATE_USER_PROHIBITED = 413; 
        public const int TRANSFORM_DELETE_PROHIBITED= 404; 
        public const int TRANSFORM_DELETE_USER_PROHIBITED = 414; 

        public const int TRANSFORM_NO_MATCHING = 431; 
        public const int TRANSFORM_AGS_NONE = 441; 
        public const int TRANSFORM_NOT_COMPATIBLE = 432;

        public const int TRANSFORM_RUN_STOREDPROCEDURE_NOTSUCCESSFULL = 433;
        public const int TRANSFORM_RUN_STOREDPROCEDURE_SUCCESSFULL = 434;

        public const int AGS_UNKNOWN_FILE = 801; 
        public const int AGS_PROCESSING_FILE = 802; 
        public const int AGS_NOTCONNECTED = 803;
        public const int XML_NOTRECEIVED = 804;
        public const int AGS_SENDFAILED = 805;

        public const int GIS_CREATE_UNSUCCESSFULL = 1001;
        public const int GIS_UNEXPECTEDFORMAT = 1002;

        public const int ESRI_NO_VALID_CONNECTION = 2001;

      }
      
      public class ge_messages:Dictionary<int,string> {

       

      public ge_messages() {
      
      // Data Messages  
        Add(msgCODE.DATA_DELETE_USER_PROHIBITED,
         "This record cannot be deleted. [CurrentUser] must have the user_operation 'Delete' assigned at project or group level");   
        Add(msgCODE.DATA_DELETE_PROHIBITED,
         "This record cannot be deleted [CurrentData] must have the operation 'Delete' assigned at data and project level and not have pstatus 'Approved'");   
        Add(msgCODE.DATA_UPDATE_USER_PROHIBITED,
         "This record cannot be updated. [CurrentUser] must have the user_operation 'Update' assigned at project or group level");   
        Add(msgCODE.DATA_UPDATE_PROHIBITED,
         "This record cannot be updated [CurrentData] must have the operation 'Update' assigned at data and project level and not have pstatus='Approved'");   
        Add(msgCODE.DATA_STATUS_USER_PROHIBITED,
         "This record cannot be approved. [CurrentUser] must have user_operation 'Approve' assigned at project or group level to change the publication status");
        Add(msgCODE.DATA_CREATE_PROHIBITED,
         "To upload new data the project and or group must have the operation 'Create',please check the assigned group and project operations");   
        Add(msgCODE.DATA_CREATE_USER_PROHIBITED,
         "To upload new data [CurrentUser] must have 'Create' operation assigned at group or project level, please check assigned user operations for [CurrentUser]");   
      
        Add(msgCODE.DATA_READ_PROHIBITED,
         "To view this record [CurrentData] must have the operation 'Read' assigned at data and project level, please check assigned operations for [CurrentData]");   
        Add(msgCODE.DATA_READ_USER_PROHIBITED,
         "To view this document [CurrentUser] must have 'Read' operation assigned at group or project level, please check assigned user operations for [CurrentUser]");   
        
        Add(msgCODE.DATA_DOWNLOAD_PROHIBITED,
         "To download this document the 'Download' operation must be assigned at data, group or project level, please check assigned user operations for [CurrentUser]");   
        Add(msgCODE.DATA_DOWNLOAD_USER_PROHIBITED,
         "To download this document [CurrentUser] must have 'Download' operation assigned at group or project level, please check assigned user operations for [CurrentUser]");   
         
      // 'Project Messages'
         Add(msgCODE.PROJECT_OPERATION_CREATE_ADMINREQ,
        "[CurrentUser] cannot create project member assigned operations without having the 'Admin' attribute in their project operations record");
        Add(msgCODE.PROJECT_OPERATION_UPDATE_ADMINREQ,
        "[CurrentUser] cannot edit project member assigned operations without having the 'Admin' attribute in their project operations record");
        Add(msgCODE.PROJECT_OPERATION_DELETE_ADMINREQ,
        "[CurrentUser] cannot delete project member assigned operations without having the 'Admin' attribute in their project operations record");
        Add(msgCODE.PROJECT_OPERATION_UPDATE_MINADMIN,
        "[CurrentProject] must have a minimum of one user with the 'Admin' attribute in their project operations record");
        Add(msgCODE.PROJECT_OPERATION_CREATE_USEREXITS,
        "User already exists, each user can only have one project operations record");
        Add(msgCODE.PROJECT_UPDATE_PROHIBITED,
         "[CurrentUser] user cannot edit the project details, please check the assigned user operations for this project and its group");   
        Add(msgCODE.PROJECT_UPDATE_USER_PROHIBITED,
         "[CurrentUser] user cannot edit the project details, please check that [CurrentUser] has 'Update' assigned user operations for this project and its group");   
        Add(msgCODE.PROJECT_READ_PROHIBITED,
         "[CurrentUser] cannot view the project details for [ObjectName], please check that the project has 'Read' assigned");   
        Add(msgCODE.PROJECT_READ_USER_PROHIBITED,
         "[CurrentUser] cannot view the project details, please check that [CurrentUser] has 'Read' assigned user operations for this project and its office");   
        Add(msgCODE.PROJECT_APPROVE_PROHIBITED,
         "[CurrentUser] user cannot Approve the project details, please check the assigned user operations for this project and its group");   
        Add(msgCODE.PROJECT_CREATE_PROHIBITED,
         "[CurrentGroup] is preventing users creating new projects, please check the assigned project operations for this group");   
        Add(msgCODE.PROJECT_CREATE_USER_PROHIBITED,
         "[CurrentUser] user cannot create new projects, please check that [CurrentUser] has 'Create' assigned user operations for this group");   
        Add(msgCODE.PROJECT_DELETE_PROHIBITED,
        "[CurrentUser] cannot delete project without having the 'Admin' and 'Delete' attributes in their project operations record and project operation has the 'Delete' attribute");
  
      // Group Messages 
        Add(msgCODE.GROUP_OPERATION_READ_ADMINREQ,
        "[CurrentUser] cannot read group member assigned operations without having the 'Admin' attribute in their group operations record");
        Add(msgCODE.GROUP_OPERATION_CREATE_ADMINREQ,
        "[CurrentUser] cannot create group member assigned operations without having the 'Admin' attribute in their group operations record");
        Add(msgCODE.GROUP_OPERATION_UPDATE_ADMINREQ,
        "[CurrentUser] cannot edit group member assigned operations without having the 'Admin' attribute in their group operations record");
        Add(msgCODE.GROUP_OPERATION_DELETE_ADMINREQ,
        "[CurrentUser] cannot delete group member assigned operations without having the 'Admin' attribute in their group operations record");
        Add(msgCODE.GROUP_OPERATION_UPDATE_MINADMIN,
        "[CurrentGroup] must have a minimum of one user with the 'Admin' attribute in their group operations record");
         Add(msgCODE.GROUP_OPERATION_CREATE_USEREXITS,
        "User already exists, each user can only have one group operations record");
         Add(msgCODE.GROUP_OPERATION_USERNOTEXIST,
        "[CurrentUser] is not a group member please create group operations record with the 'Read' attribute");
 
        Add(msgCODE.GROUP_READ_PROHIBITED,
        "[CurrentUser] cannot read group record please check that the group has the 'Read' attribute");
        Add(msgCODE.GROUP_CREATE_PROHIBITED,
        "[CurrentUser] cannot create new group without having the 'Admin' attribute in any group operations record");
         Add(msgCODE.GROUP_UPDATE_PROHIBITED,
        "[CurrentUser] cannot update group record please check that the group has the 'Update' attribute");
        Add(msgCODE.GROUP_DELETE_PROHIBITED,
        "[CurrentUser] cannot delete group please check that the group has the 'Delete' attrribute");  
        Add(msgCODE.GROUP_NOT_EMPTY,
        "This group contains projects and cannot be deleted, please remove or move all projects from this group before deleting");  
       
        Add(msgCODE.GROUP_READ_USER_PROHIBITED,
        "[CurrentUser] cannot read user group record without having the Admin and 'Read' attribute in their group operations record");
        Add(msgCODE.GROUP_UPDATE_USER_PROHIBITED,
        "[CurrentUser] cannot update group record without having the Admin and 'Update' attribute in their group operations record");
        Add(msgCODE.GROUP_DELETE_USER_PROHIBITED,
        "[CurrentUser] cannot delete group without having the 'Admin' and 'Delete' attributes in their group operations record and Group operation has the 'Delete' attribute");
      
      // User Messages
        Add(msgCODE.USER_CREATE_NOTFOUND,
        "Cannot add user operation record user not found");
        Add(msgCODE.USER_OPS_CREATE_AMBIGUOUS,
        "Cannot add user operation record for both project and group, to avoid complications these should be separate records");
        Add(msgCODE.USER_OPS_NOTFOUND,
        "User operation record not found");
        Add(msgCODE.USER_NOTFOUND,
        "User not found"); 
      //Transform Messages
        Add(msgCODE.TRANSFORM_NO_MATCHING,
        "[CurrentProject] contains no pairable transform data elements, add XML and data XSL stylesheets");
        Add(msgCODE.TRANSFORM_NOT_COMPATIBLE,
        "The stylesheet (.xls) and data (.xml) are not compatible");
        Add(msgCODE.TRANSFORM_READ_PROHIBITED,
        "[CurrentData] cannot be read, please check that 'Read' is assigned for the project data and transform");
        Add(msgCODE.TRANSFORM_READ_USER_PROHIBITED,
        "[CurrentUser] cannot read transforms, please check that [CurrentUser] has 'Read' attribute assigned for this project and or group");
        Add(msgCODE.TRANSFORM_CREATE_PROHIBITED,
        "[CurrentData] cannot be created, please check that 'Create' is assigned for the project data and that the project is not 'Approved'");
        Add(msgCODE.TRANSFORM_CREATE_USER_PROHIBITED,
        "[CurrentUser] cannot create transforms, please check that [CurrentUser] has 'Create' attribute assigned for this project and or group");
        Add(msgCODE.TRANSFORM_UPDATE_PROHIBITED,
        "[CurrentData] cannot be updated, please check that 'Update' is assigned for the project data and that the project is not 'Approved'");
        Add(msgCODE.TRANSFORM_UPDATE_USER_PROHIBITED,
        "[CurrentUser] cannot update transforms, please check that [CurrentUser] has 'Update' attribute assigned for this project and or group");
         Add(msgCODE.TRANSFORM_DELETE_PROHIBITED,
        "[CurrentData] cannot be deleted, please check that 'Delete' is assigned for the project data and that the project is not 'Approved'");
        Add(msgCODE.TRANSFORM_DELETE_USER_PROHIBITED,
        "[CurrentUser] cannot delete transforms, please check that [CurrentUser] has 'Delete' attribute assigned for this project and or group");
        Add(msgCODE.TRANSFORM_AGS_NONE,
        "AGS file version is not known or missing, please check that this is a valid ags file in a correctly formed xml format");
        Add(msgCODE.TRANSFORM_RUN_STOREDPROCEDURE_NOTSUCCESSFULL,
        "The sql storedprocedure executed was not successfull");
        Add(msgCODE.TRANSFORM_RUN_STOREDPROCEDURE_SUCCESSFULL,
        "The sql storedprocedure executed was successfull");
        // AGS Conversion Messages   
         Add(msgCODE.AGS_UNKNOWN_FILE,
        "AGS data file expected, the selected file does not have the correct file extension ('.AGS') for AGS data"); 
         Add(msgCODE.AGS_PROCESSING_FILE,
        "The selected AGS data file is currently being converted, please wait for the XML file conversion to complete, the resulting file will appear in the project data Index when successfully completed"); 
         Add(msgCODE.AGS_NOTCONNECTED,
        "The selected AGS server is not responding at the moment, please try again later"); 
         Add(msgCODE.AGS_SENDFAILED,
        "The web server was unable to transmit the AGS file successfully to the AGS server, please check the version and formatting of the AGS data"); 
         Add(msgCODE.XML_NOTRECEIVED,
        "The AGS server has been unable to generate XML data from AGS file recieved, please check the version and formatting of the AGS data"); 
      
        Add(msgCODE.GIS_CREATE_UNSUCCESSFULL,
        "The creation of the GIS file has been unsuccessful, please try again later"); 
        Add(msgCODE.GIS_UNEXPECTEDFORMAT,
        "Unexpected format found, please provide a registered format type (kml,xml,shp.. etc) for the GIS output file"); 
      
        Add(msgCODE.ESRI_NO_VALID_CONNECTION,
        "There are no valid esri feature connections for this project, please add esri end point file"); 
      }
   }
 

public class opertions  {

private SortedList<int, string> dict = new SortedList<int, string>(){
                                        {1,Constants.ReadOperationName},
                                        {2, Constants.CreateOperationName},
                                        {4, Constants.UpdateOperationName},
                                        {8, Constants.DeleteOperationName},
                                        {16,Constants.DownloadOperationName}};
 private string delimiter = ";";
 
 private int int_ops;
 private string str_ops;

  void operations(int ops){
      int_ops = ops;
  }

  public override string  ToString(){
    
    int ops = int_ops;
    str_ops = "";

    for (int i = dict.Count;i >-1;i--) {
      if (ops >= dict.Keys[i]) {
        add (dict.Values[i]);
        ops =- dict.Keys[i];
      }
    }

    return str_ops;
  }

  public int ToInt(){
    
    int ops = int_ops;
    
    for (int i = 0; i < dict.Count; i++) {
      if (str_ops.Contains(dict.Values[i])) {
        add (dict.Keys[i]);
      }
    }
    return int_ops;
  }

  private void add (string s1){
    if (s1.Length>0 && str_ops.Length >0) str_ops+=delimiter;
  }
  private void add (int i1) {
    int_ops +=i1;
  }
}

public static class geOPSResp {
    public const int Allowed = 0;
    public const int InvalidInput = -1;
    public const int Group = -2;
    public const int GroupProject = -4;
    public const int Project = -8;
    public const int ProjectData = -16; 
    public const int Data = -32;
    public const int ProjectApproved = -64;
    public const int DataApproved = -128;
    public const int UserOperation = -256;

}
public static class Extentions {


  public static int EnumToInt<TValue>(this TValue value)  where TValue : struct, IConvertible
{
    if(!typeof(TValue).IsEnum)
    {
        throw new ArgumentException(nameof(value));
    }

    return (int)(object)value;
}  

public static int GetUserGroupCount(this ge_DbContext context, string userId) {
   return context.ge_user_ops
                        .AsNoTracking()
                        .Where(u => u.userId == userId)
                        .Where(u => u.groupId != null)
                        .Count();
}
public static int GetUserProjectCount(this ge_DbContext context, string userId) {
   return context.ge_user_ops
                        .AsNoTracking()
                        .Where(u => u.userId == userId)
                        .Where(u => u.projectId != null)
                        .Count();
}
 public static bool DoesUserHaveAnyOperations(this ge_DbContext context, ge_group group, string userEmail) {

            if (context == null || group == null || userEmail == String.Empty) {
                        return false;
            } 
            var user_group = context.ge_user_ops
                                .AsNoTracking()
                                .Include (u => u.user)
                                .Where(u => u.user.Email == userEmail && u.groupId==group.Id);
            return user_group.Any();
}
 public static bool DoesUserHaveAnyOperations(this ge_DbContext context, ge_project project, string userEmail) {

            if (context == null || project == null || userEmail == String.Empty) {
                        return false;
            } 
            var user_project = context.ge_user_ops
                                .AsNoTracking()
                                .Include (u => u.user)
                                .Where(u => u.user.Email == userEmail && u.projectId==project.Id);
            return user_project.Any();
}
  public static int IsOperationAllowed(this ge_DbContext context, string operation, ge_group group, ge_project project) {

            if (context == null || group == null || project== null || operation == String.Empty) {
                        return geOPSResp.InvalidInput;
            } 
            
            // Check at group level;
            if (group.project_operations == null) {
              return geOPSResp.GroupProject;;
            } 
            
            if (!group.project_operations.Contains(operation)){
                return geOPSResp.GroupProject;
            }
            
            // Check for new project
            if (project.Id==Guid.Empty) {
                return geOPSResp.Allowed;
            }

            // Check at project level if it is approved and cannot be edited or deleted 
           if (operation == Constants.UpdateOperationName || operation==Constants.DeleteOperationName) {
                    if (project.pstatus == Constants.PublishStatus.Approved) {
                        return geOPSResp.ProjectApproved;
                    }
            }

            if (project.operations == null) {
              return geOPSResp.Project;
            }

            if (!project.operations.Contains(operation)){
              return geOPSResp.Project;
            }
            
            return geOPSResp.Allowed;
            
        }
        public static int IsOperationAllowed(this ge_DbContext context, string operation, ge_group group, ge_project project, ge_data data) {
        
        if (context == null || data == null || project== null || group == null || operation == String.Empty) {
               return geOPSResp.InvalidInput;
        }
        
        // Check at group level;
        if (group.project_operations == null) {
            return geOPSResp.GroupProject;;
        } 
        
        if (!group.project_operations.Contains(operation)){
            return geOPSResp.GroupProject;
        }

        // check project record status 
        if (project.data_operations==null){
            return geOPSResp.ProjectData;
        } 
        if (!project.data_operations.Contains(operation)) {
            return geOPSResp.ProjectData;
        }
       if (operation == Constants.UpdateOperationName || operation==Constants.DeleteOperationName) {
            if (project.pstatus == Constants.PublishStatus.Approved) {
                return geOPSResp.ProjectApproved;
            }
        }

        //Check data record status
        if (data.Id==Guid.Empty) {
            return geOPSResp.Allowed;
        }
       if (operation == Constants.UpdateOperationName || operation==Constants.DeleteOperationName) {
            if (data.pstatus == Constants.PublishStatus.Approved) {
                return geOPSResp.DataApproved;
            }
        }
        if (data.operations == null) {
             return geOPSResp.Data;
        }
        if (!data.operations.Contains (operation)) {
            return geOPSResp.Data;
        }

        return geOPSResp.Allowed;
    }
 public static int IsOperationAllowed(this ge_DbContext context, string operation, ge_project project, ge_data data) {
        
        if (context == null || data == null || project== null || operation == String.Empty) {
               return geOPSResp.InvalidInput;
        }

        // check project record status 
        if (project.data_operations==null){
            return geOPSResp.ProjectData;
        } 
        if (!project.data_operations.Contains(operation)) {
            return geOPSResp.ProjectData;
        }
       if (operation == Constants.UpdateOperationName || operation==Constants.DeleteOperationName) {
            if (project.pstatus == Constants.PublishStatus.Approved) {
                return geOPSResp.ProjectApproved;
            }
        }

        //Check data record status
        if (data.Id==Guid.Empty) {
            return geOPSResp.Allowed;
        }
       if (operation == Constants.UpdateOperationName || operation==Constants.DeleteOperationName) {
            if (data.pstatus == Constants.PublishStatus.Approved) {
                return geOPSResp.DataApproved;
            }
        }
        if (data.operations == null) {
             return geOPSResp.Data;
        }
        if (!data.operations.Contains (operation)) {
            return geOPSResp.Data;
        }

        return geOPSResp.Allowed;
    }
    public static int IsOperationAllowed(this ge_DbContext context, string operation, ge_group group) {
          
        if (context == null || group == null || operation == String.Empty) {
            return geOPSResp.InvalidInput;
        }

        if (group.operations==null){
            return geOPSResp.Group;
        } 
        
        if (!group.operations.Contains(operation)){
            return geOPSResp.Group;
        }
        
        return geOPSResp.Allowed;
    }
    public static int IsOperationAllowed(this ge_DbContext context, string operation, ge_user_ops user_op) {
          
        if (context == null || user_op == null || operation == String.Empty) {
            return geOPSResp.InvalidInput;
        }

        if (user_op.operations==null){
            return geOPSResp.UserOperation;
        } 
        
        if (!user_op.operations.Contains(operation)){
            return geOPSResp.UserOperation;
        }
        
        return geOPSResp.Allowed;
    }
    public static int IsOperationAllowed(this ge_DbContext context, string operation, ge_project project, ge_transform transform) {
        
        if (context == null || transform == null || project== null || operation == String.Empty) {
               return geOPSResp.InvalidInput;
        }

        // check project record status 
        if (project.data_operations==null){
            return geOPSResp.ProjectData;
        } 
        if (!project.data_operations.Contains(operation)) {
            return geOPSResp.ProjectData;
        }
        if (operation == Constants.UpdateOperationName || operation==Constants.DeleteOperationName) {
            if (project.pstatus == Constants.PublishStatus.Approved) {
                return geOPSResp.ProjectApproved;
            }
        }

        //Check data record status

        if (transform.Id==Guid.Empty) {
            return geOPSResp.Allowed;
        }
        
        // if (operation == geOPS.Update || operation==geOPS.Delete) {
        //    if (transform.pstatus == Constants.PublishStatus.Approved) {
        //        return geOPSResp.DataApproved;
        //    }
        // }

        if (transform.operations == null) {
             return geOPSResp.Data;
        }
        if (!transform.operations.Contains (operation)) {
            return geOPSResp.Data;
        }

        return geOPSResp.Allowed;
    }
       
 
 public static bool DoesUserHaveOperation(this ge_DbContext context, string  operation, ge_group group, string userId) {
  
    if (context == null || group == null || operation == null || userId == String.Empty ) {
            return false;
    }

    bool retvar = false;
    ge_user_ops user_group = null;
    
    if (group.Id==Guid.Empty) {
            return IsUserAnyGroupAdmin(context,userId);
    }
    
    user_group = context.ge_user_ops
                        .AsNoTracking()
                        .Where(u => u.groupId == group.Id)
                        .Where(u => u.userId == userId).FirstOrDefault();
    if (user_group!=null) {
        if (user_group.user_operations.Contains(operation)) { 
             context.Entry<ge_user_ops>(user_group).State = EntityState.Detached;
             user_group = null;
                retvar= true;
        }
    }

    return retvar;
 }

public static bool DoesUserHaveOperation(this ge_DbContext context, string operation, ge_project project, string userId) {
       
        if (context == null || project == null || operation == null || userId==null) {
            return false;
        }
        
        if (userId == String.Empty) {
            return false;
        } 

        bool retvar = false;

        ge_user_ops user_proj = null;
        ge_user_ops user_group = null;
        
        if (project.Id==Guid.Empty && operation==geOPS.Create.Name) {
            if (project.group==null){
                return false;
            }
            return DoesUserHaveOperation(context,geOPS.Create.Name,project.group,userId);
        }
        user_proj = context.ge_user_ops
                        .AsNoTracking()
                        .Where(p => p.userId == userId && p.projectId==project.Id).FirstOrDefault();
        
        if (user_proj != null) {
            if (user_proj.user_operations.Contains(operation)) {
            retvar= true;
            } else {
               retvar = false;}
        } 
        else {
            user_group = context.ge_user_ops
                    .AsNoTracking()
                    .Where(u=> u.userId == userId  && u.groupId == project.groupId).FirstOrDefault();
                if (user_group.user_operations.Contains(operation)) {
                    retvar =true;    
                } else {
                retvar = false;
                }
        }
       
        return retvar;
}
  public static bool IsUserAnyGroupAdmin(this ge_DbContext context, string userId) {

        var user_group = context.ge_user_ops
                        .AsNoTracking()
                        .Where(u => u.userId == userId && u.groupId != null && u.user_operations.Contains(Constants.AdminOperationName));
      
        return user_group.Any();
  }
  public static bool IsUserAdmin(this ge_DbContext context, ge_group group, string userId) {
       
        if (context == null || group == null || userId == String.Empty )
        {
            return false;
        }
        return DoesUserHaveOperation(context,Constants.AdminOperationName,group,userId);
}
  public static bool IsUserAdmin(this ge_DbContext context, ge_project project, string userId) {
       
        if (context == null || project == null || userId == String.Empty )
        {
            return false;
        }
        return DoesUserHaveOperation(context,Constants.AdminOperationName,project,userId);
}
 
    public static string lastAdminUserId (this ge_DbContext context, ge_project project) {
        
        if (context == null || project == null) {
           return String.Empty;
        }
        
        string userId = ""; 

        var admin_user_proj = context.ge_user_ops
                        .AsNoTracking()
                        .Where(p => p.user_operations.Contains(Constants.AdminOperationName) && p.projectId==project.Id);
        
        if (admin_user_proj.Count()> 1) {
            userId= String.Empty;
        } else {
        userId = admin_user_proj.FirstOrDefault().userId;
        }
        admin_user_proj = null;
        return userId;
    }
    public static string lastAdminUserId (this ge_DbContext context, ge_group group) {
        
        if (context == null || group == null) {
           return String.Empty;
        }
        
        string userId = ""; 
        
        var admin_user_group = context.ge_user_ops
                        .AsNoTracking()
                        .Where(p => p.user_operations.Contains(Constants.AdminOperationName) && p.groupId==group.Id);
         
        if (admin_user_group.Count()> 1) {
           userId = String.Empty;
        } else {
           userId = admin_user_group.FirstOrDefault().userId;  
        }
   
        return userId; 
             
    }

    public static bool IsUserApprover(this ge_DbContext context, ge_project project, string userId) {
       
        if (context == null || project == null || userId == String.Empty ) {
            return false;
        }
        return DoesUserHaveOperation(context,Constants.ApproveOperationName,project,userId);
    }
        public static bool IsUserLastAdmin(this ge_DbContext context, ge_group group, string userId) {
            string LastAdminUserId = context.lastAdminUserId(group);
            if (!String.IsNullOrEmpty(LastAdminUserId)) {
                if (LastAdminUserId==userId) {
                    return true;
                }
            }
            return false;
        }
        public static bool IsUserLastAdmin(this ge_DbContext context,ge_project project, string userId) {
            string LastAdminUserId = context.lastAdminUserId(project);
            if (!String.IsNullOrEmpty(LastAdminUserId)) {
                if (LastAdminUserId==userId) {
                    return true;
                }
            }
            return false;
        }


}
}
