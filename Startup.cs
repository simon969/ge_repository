using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.AGS;
using ge_repository.Services;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Okta.AspNetCore;

namespace ge_repository
{


    public class Startup
    {
    private readonly IHostingEnvironment _env;
    private readonly IConfiguration _config;
  // private readonly ILoggerFactory _loggerFactory;

        public Startup(IHostingEnvironment env, IConfiguration config) 
       // ILoggerFactory loggerFactory)
        {
            _env = env;
            _config = config;
     //       _loggerFactory = loggerFactory;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // var logger = _loggerFactory.CreateLogger<Startup>();
            
            // if (_env.IsDevelopment()) {
            // // Development service configuration
            // logger.LogInformation("Development environment");
            // }
            // else    {
            // // Non-development service configuration
            // logger.LogInformation($"Environment: {_env.EnvironmentName}");
            // }
            
            var oktaMvcOptions = new OktaMvcOptions()
                                            {
                                                OktaDomain = _config.GetSection("Okta").GetValue<string>("OktaDomain"),
                                                ClientId = _config.GetSection("Okta").GetValue<string>("ClientId"),
                                                ClientSecret = _config.GetSection("Okta").GetValue<string>("ClientSecret"),
                                                Scope = new List<string> { "openid", "profile", "email" },
                                            };
            
            services.AddSingleton<IHostingEnvironment> (_env);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            services.AddDbContext<ge_DbContext>(options =>
            options.UseSqlServer(_config.GetConnectionString("ge_DbContext"))
            .EnableSensitiveDataLogging());

            services.AddDefaultIdentity<ge_user>(config =>
                {
                    config.SignIn.RequireConfirmedEmail = true;
                })
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ge_DbContext>(); 
            services.Configure<ags_config>(_config.GetSection("ags_config"));            
            services.Configure<smpt_config>(_config.GetSection("smpt_config"));
            services.Configure<ge_config>(_config.GetSection("ge_config"));
            services.Configure<FormOptions>(x => {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });

            services.AddScoped<IAuthorizationHandler, ge_repositoryIsOwnerAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, ge_repositoryAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, ge_repositoryGroupAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, ge_repositoryProjectAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, ge_repositoryDataAuthorizationHandler>();
            
            services.AddSingleton<IAuthorizationHandler, ge_repositoryAdministratorAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ge_repositoryManagerAuthorizationHandler>();

            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
           
            .AddCookie(options =>
                             {
                             options.LoginPath = new PathString("/Account/SignIn");
                             })
            .AddOktaMvc(oktaMvcOptions);  

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                         .RequireAuthenticatedUser()
                         .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddXmlSerializerFormatters()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
       
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) //, ILoggerFactory loggerFactory)
        {
            //	loggerFactory.AddConsole(Configuration.GetSection("Logging")); //log levels set in your configuration
	        //    loggerFactory.AddDebug(); //does all log levels
            //call ConfigureLogger in a centralized place in the code
	//  app.ConfigureLogger(loggerFactory);
	//  set it as the primary LoggerFactory to use everywhere
	//  ApplicationLogging.LoggerFactory = loggerFactory;

            if (env.IsDevelopment())
            {
              ///  logger.LogInformation ("In Development environment");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
        //    app.UseMvcWithDefaultRoute();

            app.UseMvc(routes =>
                {
                routes.MapRoute(
                    name: "default", 
                    template: "{controller=Home}/{action=Index}/{id?}");
          //      routes.MapRoute(
          //          name: "api", 
          //          template:"api/{controller}/{id?}");
            });
        }
    }
}
