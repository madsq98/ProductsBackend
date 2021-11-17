using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProductsBackend.Core.IServices;
using ProductsBackend.Core.Models;
using ProductsBackend.Domain.IRepositories;
using ProductsBackend.Domain.Services;
using ProductsBackend.EntityCore;
using ProductsBackend.EntityCore.Repositories;

namespace ProductsBackend.CoreWebAPI
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ProductsBackend.CoreWebAPI", Version = "v1"});
            });
            
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            services.AddDbContext<ProductsContext>(opt =>
            {
                opt.UseLoggerFactory(loggerFactory)
                    .UseSqlite("Data Source=products.db");
            });

            services.AddScoped<IRepository<Product>, ProductsRepository>();
            services.AddScoped<IProductsService, ProductsService>();
            
            services.AddCors(options =>
            {
                options.AddPolicy("shop-cors", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductsBackend.CoreWebAPI v1"));
            }
            
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetService<ProductsContext>();
                //ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("shop-cors");

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}