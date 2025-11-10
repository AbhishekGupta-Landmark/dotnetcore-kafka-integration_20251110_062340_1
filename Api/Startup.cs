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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HostedServices = Microsoft.Extensions.Hosting;
using Api.Services;
using Azure.Messaging.ServiceBus;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<HostedServices.IHostedService, ProcessOrdersService>();
            
            var serviceBusClientOptions = new ServiceBusClientOptions();
            var connectionString = Configuration["ServiceBus:ConnectionString"];
            var queueName = Configuration["ServiceBus:QueueName"];

            services.AddSingleton(new ServiceBusClient(connectionString, serviceBusClientOptions));
            services.AddSingleton(queueName);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMvc();
        }
    }
}