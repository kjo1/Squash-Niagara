using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SN_BNB.Data;
using SN_BNB.Models;

namespace SN_BNB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();
            var host = CreateWebHostBuilder(args).Build();

            //Seed Data



            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<SNContext>();
                    context.Database.Migrate();
                    var identityContext = services.GetRequiredService<ApplicationDbContext>();
                    ApplicationSeedData.SeedAsync(identityContext, services).Wait();

                    identityContext.Database.Migrate();
                    SNSeedData.Initialize(services);

                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    //throw (ex);
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
