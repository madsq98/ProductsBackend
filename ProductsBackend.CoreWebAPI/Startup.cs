using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductsBackend.Core.IServices;
using ProductsBackend.Core.Models;
using ProductsBackend.CoreWebAPI.Middleware;
using ProductsBackend.CoreWebAPI.PolicyHandlers;
using ProductsBackend.Domain.IRepositories;
using ProductsBackend.Domain.Services;
using ProductsBackend.EntityCore;
using ProductsBackend.EntityCore.Repositories;
using ProductsBackend.Security;
using ProductsBackend.Security.Model;
using ProductsBackend.Security.Services;

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
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
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
            
            services.AddDbContext<AuthDbContext>(opt =>
            {
                opt.UseSqlite("Data Source=auth.db"); 
            });

            services.AddScoped<IRepository<Product>, ProductsRepository>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IAuthService, AuthService>();
            
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])) //Configuration["JwtToken:SecretKey"]
                };
            });
            services.AddSingleton<IAuthorizationHandler, CanWriteProductsHandler>();
            services.AddSingleton<IAuthorizationHandler, CanReadProductsHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(CanWriteProductsHandler), 
                    policy => policy.Requirements.Add(new CanWriteProductsHandler()));
                options.AddPolicy(nameof(CanReadProductsHandler), 
                    policy => policy.Requirements.Add(new CanReadProductsHandler()));
            });
            
            services.AddCors(options =>
            {
                options.AddPolicy("shop-cors", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AuthDbContext authDbContext)
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

            authDbContext.Database.EnsureDeleted();
            authDbContext.Database.EnsureCreated();
            authDbContext.LoginUsers.Add(new LoginUser
            {
                UserName = "ljuul",
                HashedPassword = "123456",
                DbUserId = 1,
            });
            authDbContext.LoginUsers.Add(new LoginUser
            {
                UserName = "ljuul2",
                HashedPassword = "123456",
                DbUserId = 2,
            });
            authDbContext.Permissions.AddRange(new Permission()
            {
                Name = "CanWriteProducts"
            }, new Permission()
            {
                Name = "CanReadProducts"
            });
            authDbContext.SaveChanges();
            authDbContext.UserPermissions.Add(new UserPermission { PermissionId = 1, UserId = 1 });
            authDbContext.UserPermissions.Add(new UserPermission { PermissionId = 2, UserId = 1 });
            authDbContext.UserPermissions.Add(new UserPermission { PermissionId = 2, UserId = 2 });
            authDbContext.SaveChanges();
            
            app.UseCors("shop-cors");

            app.UseMiddleware<JWTMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}