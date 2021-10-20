using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Boilerplate.Common.Data;
using Boilerplate.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace Boilerplate.MongoDB.Sample
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(CreateClient);
            services.AddSingleton<IMongoDatabase>(provider => 
                provider.GetRequiredService<IMongoClient>().GetDatabase("custdb"));
            services.AddTransient<ICollectionContext, CustomerCollectionContext>();
            services.AddTransient<IRepository<Customer, string>, MongoRepository<Customer, string>>();
            services.AddControllers();
        }

        private static IMongoClient CreateClient(IServiceProvider provider)
        {
            var mongoClientSettings = new MongoClientSettings
            {
                Server = MongoServerAddress.Parse("localhost:27017"),
                Scheme = ConnectionStringScheme.MongoDB,
                UseTls = false,
                RetryWrites = true,
                WriteConcern = WriteConcern.WMajority,
            };

            return new MongoClient(mongoClientSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }

    public class CustomerCollectionContext : ICollectionContext
    {
        public string Name => "customers";
        public IMongoDatabase DB { get; }

        public CustomerCollectionContext(IMongoDatabase database) => DB = database;
    }
}
