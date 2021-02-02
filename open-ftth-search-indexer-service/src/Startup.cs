using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Typesense;
using Typesense.Setup;
using System.Collections.Generic;
using System;

namespace open_ftth_search_indexer_service
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment enviroment)
        {
            Configuration = configuration;
            WebEnviroment = enviroment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebEnviroment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var apiKey = "Hu52dwsas2AdxdE";
            var host = "localhost";
            var port = "8108";

            if (WebEnviroment.IsProduction())
            {
                apiKey = Environment.GetEnvironmentVariable("TYPESENSE_APIKEY");
                host = Environment.GetEnvironmentVariable("TYPESENSE_HOST");
                port = Environment.GetEnvironmentVariable("TYPESENSE_PORT");
            }
            
            var node = new Node();
            node.Host = host;
            node.Port = port;
            node.Protocol = "http";
            services.AddTypesenseClient(options =>
                {
                    options.ApiKey = apiKey;
                    options.Nodes = new List<Node>();
                    options.Nodes.Add(node);
                });
            services.AddControllers();
            services.AddHealthChecks();

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

            app.UseHealthChecks("/health", new HealthCheckOptions { ResponseWriter = JsonResponseWriter });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private async Task JsonResponseWriter(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(context.Response.Body, new { Status = report.Status.ToString() },
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
