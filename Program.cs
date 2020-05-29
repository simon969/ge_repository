using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ge_repository.Models;
using ge_repository.Data;

namespace ge_repository
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var host = CreateWebHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ge_DbContext>();
            context.Database.EnsureCreated();

            // requires using Microsoft.Extensions.Configuration;
            var config = host.Services.GetRequiredService<IConfiguration>();
            // Set password with the Secret Manager tool.
            // dotnet user-secrets set SeedUserPW S*qecvrB+\H4~VN8
            var testUserPw = "S*qecvrB+)H4~VN8";
                    
            try
            {
                SeedData.Initialize(services, testUserPw).Wait();
          
            }
            catch (Exception ex)
            {
            }
        }

        host.Run();
        }
        // Default max body size is 30Mb; Configure Kestrel to have no max body size
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel?tabs=aspnetcore2x&view=aspnetcore-2.2#kestrel-options
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>  
                    {  
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));  
                    logging.AddConsole();  
                    logging.AddDebug();
                    })  
                .UseStartup<Startup>()
                .UseKestrel(options => {
                    options.Limits.MaxRequestBodySize = null;
                    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(20);
                   });
    }
}
