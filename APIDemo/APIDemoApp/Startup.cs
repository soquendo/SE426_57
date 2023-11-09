using DataLibrary.Data;     //added to allow us to bring in the interface for Food and Order Data
using DataLibrary.Db;       //added by Scott in order to set up Singletons for dependency injection for DB dependencies
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIDemoApp
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

            services.AddControllers();                              //this doesnt include views - remember api's dont have visual interface/GUI

            services.AddSingleton(new ConnectionStringData          //** this points us to "Default" connection string within our configurations JSON file
            {
                SqlConnectionName = "Default"
            });

            services.AddSingleton<IDataAccess, SqlDb>();            //points us to our SQL server and basic utils
            services.AddSingleton<IFoodData, FoodData>();           //configures our FoodData object that contains tools regarding our Food Table
            services.AddSingleton<IOrderData, OrderData>();         //configures our OrderData object that contains tools to allow us to work with orders (CRUD)

            //***this is here to give access to swagger, if you included it when creating project
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "APIDemoApp", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIDemoApp v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
