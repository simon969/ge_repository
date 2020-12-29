using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using System.Data;
using ge_repository.Authorization;
using ge_repository.DAL;
using ge_repository.Models;
namespace ge_repository.Controllers 
{

    //Base controller with view support
    public class ge_Controller : Controller
    {
        protected ge_DbContext _context { get; }
        protected IAuthorizationService _authorizationService { get; }
        protected UserManager<ge_user> _userManager { get; }
        protected ge_user _user {get;set;}
        protected IHostingEnvironment _env {get;}
        protected IOptions<ge_config> _ge_config {get;}

        public ge_Controller(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IHostingEnvironment env,
            IOptions<ge_config> ge_config) : base()
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
            _env = env;
            _ge_config = ge_config;
        } 
         public  RedirectToPageResult RedirectToPageMessage(string message, string return_URL, logLEVEL log) {
        // RedirectToAction(string actionName, string ge_message, string fragment);
            ge_eventDAL ged = new ge_eventDAL(_context);
            string userId= _userManager.GetUserId(User);
           	ge_event ge = ged.addEvent(userId,message,return_URL,log);
                          ged.Save();
            return RedirectToPage ("/Shared/Message",new {Id = ge.Id});
           
        }
        public ge_config ge_config() {
            return _ge_config.Value;
        }
        public  RedirectToPageResult RedirectToPageMessage(int msgCODE) {
        // RedirectToAction(string actionName, string ge_message, string fragment);
            return RedirectToPage ("/Shared/Message",new {MsgId = msgCODE});
           
        }
        
        public string getHostHref() {
           string DisplayUrl = Request.GetDisplayUrl();
           string PathQuery =  Request.GetEncodedPathAndQuery();
           string HostRef = DisplayUrl.Substring(0, DisplayUrl.IndexOf(PathQuery));  
           
            // Check for running on host that has an application folder
            ge_config g = (ge_config) _ge_config.Value;

           if (g.hosts_with_app_folder.Contains(HostRef)) {
                HostRef += "/" + _env.ApplicationName;
           }

           return HostRef;
        }
         public static List<T> ConvertDataTable<T>(DataTable dt)  
        {  
            List<T> data = new List<T>();  
            foreach (DataRow row in dt.Rows)  
            {  
                T item = GetItem<T>(row);  
                data.Add(item);  
            }  
            return data;  
        }  
        private static T GetItem<T>(DataRow dr)  
        {  
            try {
            Type temp = typeof(T);  
            T obj = Activator.CreateInstance<T>();  
        
            foreach (DataColumn column in dr.Table.Columns)  
            {  
                foreach (PropertyInfo pro in temp.GetProperties())  
                {  
                    if (pro.Name == column.ColumnName) { 
                        object value =  dr[column.ColumnName];
                        if (value == DBNull.Value) {value = null;}
                      //  Console.WriteLine (pro.Name);
                       // if (pro.PropertyType == typeof(double?)) {
                        // if (value is Single && value != null){
                        //     value = Convert.ToDouble(value.ToString());
                        // }
                        pro.SetValue(obj, value, null);  
                    } else  
                        continue;  
                    
                }  
            }  
            return obj;  
         
        } catch (Exception e){
            return default(T);
        } 

    }

      public async Task<ge_user>  GetUserAsync() {
          try {
                if (HttpContext!=null) {
                var claim = HttpContext.User.Claims.First(c => c.Type == "email");
                string emailAddress = claim.Value;
                _user = _context.ge_user.First(u => u.Email == emailAddress);
                _context.user = _user;
                return await _userManager.FindByEmailAsync(emailAddress);
                }
                
                if (_context !=null) {
                return _context.user;
                }
                
                return null;
                
          } catch {
              return null;
          }
        }
}
}
