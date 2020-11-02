using AgentNetCore.Context;
using AgentNetCore.Hypermedia;
using AgentNetCore.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System;
using Tapioca.HATEOAS;

namespace AgentNetCore
{
    public class Startup
    {
        //private readonly ILogger _logger;
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)//, ILogger<Startup> logger
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _configuration["connectionStrings:MySQL"];
            services.AddDbContext<MySQLContext>(options => options.UseMySql(connectionString));

            //Add Versioning
            services.AddApiVersioning();
            
            //Add Content Negotiation
            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("text/xml"));
                options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
            })
            .AddXmlSerializerFormatters();

            //Add HATEOAS
            var filterOptions = new HyperMediaFilterOptions();
            filterOptions.ObjectContentResponseEnricherList.Add(new UserEnricher());
            filterOptions.ObjectContentResponseEnricherList.Add(new GroupEnricher());
            filterOptions.ObjectContentResponseEnricherList.Add(new OrganizationalUnitEnricher());
            filterOptions.ObjectContentResponseEnricherList.Add(new ForestEnricher());
            services.AddSingleton(filterOptions);

            //Add Swagger
            services.AddSwaggerGen(c =>
            {
                // sepcify our operation filter here.  
                c.SwaggerDoc("v1.0.0", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = $"AgentNetCore - v1 API",
                    Description = "Trabalho de Conclusão de Curso - Implementação de Integração com o servidor de domínio através de API RESTful para as Empresas Marvel",
                    TermsOfService = new Uri("https://www.google.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "Diego Vieira",
                        Email = "diegovieira.ti@gmail.com",
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Apache-2.0",
                        Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
                    }
                });
            });

            //Add Controllers
            services.AddControllers();

            //Dependencias
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IOrganizationalUnitService, OrganizationalUnitService>();
            services.AddScoped<IForestService, ForestService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger(c =>
            {
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1 API");
            });

            var option = new RewriteOptions();
            option.AddRedirect("^$","swagger");
            app.UseRewriter(option);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("DefaultApi", "{controller}");
            });

            
        }
    }
}
