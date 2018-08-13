using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RowLevelSecurityPOC.Models
{
    public class TenantFinder
    {
        private readonly RequestDelegate next;
        private DataContext dataContext;
        public TenantFinder(RequestDelegate next, DataContext dataContext)
        {
            this.next = next;
            this.dataContext = dataContext;
        }

        public async Task Invoke(HttpContext context)
        {
            var apiKey = context.Request.Headers["X-API-Key"].FirstOrDefault();
            if (string.IsNullOrEmpty(apiKey))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid API key");
                return;
            }
            Guid apiKeyGuid;
            if (!Guid.TryParse(apiKey, out apiKeyGuid))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid API key");
                return;
            }
            var tenant = dataContext.Tenants.Where(t => t.APIKey == apiKeyGuid).FirstOrDefault();
            if (tenant == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API key");
                return;
            }
            else
            {
                // add tenant to http context for use when the conection to the data is opened
                context.Items["TENANT"] = tenant;
            }

            await next.Invoke(context);
        }
    }

    public static class TenantFinderExtension
    {
        public static IApplicationBuilder UseTenantFinder(this IApplicationBuilder app)
        {
            app.UseMiddleware<TenantFinder>();
            return app;
        }
    }
}
