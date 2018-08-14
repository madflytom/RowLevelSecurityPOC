﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RowLevelSecurityPOC.Models;
using Microsoft.EntityFrameworkCore;

namespace RowLevelSecurityPOC
{
    public class Startup
    {

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // we need this to access HttpContext in DataContext
            services.AddDbContext<DataContext>(ServiceLifetime.Scoped);

            services.AddCors(options =>
            {
                // BEGIN01
                options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod()
                    .WithHeaders("accept", "content-type", "origin", "x-api-key");
                });
                // END01
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseTenantFinder();
            app.UseCors("AllowAllOrigins");


            app.UseMvc();
        }

        
    }
}
