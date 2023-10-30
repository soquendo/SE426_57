using DataLibrary.Data;     //Added
using DataLibrary.Db;       //Added
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPDemoApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            //this allows us to create one shared version of the var/object to avoid recreation of the same thing for each user
            //needed to be created to supply the necessary info for Dependency Injection, seeing the remaining instances need connection name
            services.AddSingleton(new ConnectionStringData { SqlConnectionName = "Default" });

            //sets this up so when needed, it will declare a single shared Generic of type IDataAccess, and when the instantiation
            // time occurs, narrow the type to the strongly typed SqlDb object (Polymorph)
            //So if we had a MySqlDb class and wanted to use it instead of SQL Server, we would use "MySqlDb" instead of "SqlDb"
            services.AddSingleton<IDataAccess, SqlDb>();

            services.AddSingleton<IFoodData, FoodData>(); //same concept of Generics and Polymorph
            services.AddSingleton<IOrderData, OrderData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
