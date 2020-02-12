using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using AutoMapper;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Repositiries;
using DataAccessLayer.IRepositories;
using Microsoft.Extensions.DependencyInjection;
using BusinessLogicLayer.Helpers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.IO;
using BusinessLogicLayer.Hubs;
using CurrencyAPI.Helpers;
using CurrencyAPI.Intefaces;
using CurrencyAPI.CurrencyRateAPI;

namespace RESTfulAPIs
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
                
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),x=>x.MigrationsAssembly("DataAccessLayer")));

            #region Jwt token
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>(); 
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetById(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            #endregion



            #region Configure our dependecies...
            // configure DI for application services
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICurrencyConvertAPI, CurrencyConvertAPI>();
            // configure DI for application Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICurrencyRepository,CurrencyRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<CurrencySettings>(Configuration.GetSection("CurrencySettings"));            

            #endregion

            services.AddControllers();
            
            services.AddMvc();

            services.AddSignalR();

            #region Swager settings
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                
                    {
                
                        new OpenApiSecurityScheme
                
                        {
                
                            Reference = new OpenApiReference
                
                            {
                
                                Type = ReferenceType.SecurityScheme,
                
                                Id = "Bearer"
                
                            },
                
                            Scheme = "oauth2",
                
                            Name = "Bearer",
                
                            In = ParameterLocation.Header,


                
                        },
                
                        new List<string>()
                
                    }
                
                });
           

                
                //Locate the XML file being generated by ASP.NET...
                
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);


                //... and tell Swagger to use those XML comments.

                c.IncludeXmlComments(xmlPath);
                
            });
            #endregion
            services.AddAutoMapper(typeof(Startup));

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IUserService userService, ICurrencyRepository currencyRepository)
        {

            var builder = new ConfigurationBuilder()
                              .SetBasePath(env.ContentRootPath)
                              .AddJsonFile("appsettings.json",
                                           optional: false,
                                           reloadOnChange: true)
                              .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                builder.AddUserSecrets<Startup>();
            }                       
            app.UseRouting();

            app.UseHttpsRedirection();
            
            app.UseStaticFiles();


            app.UseAuthentication();
            app.UseAuthorization();

            DataInitializer.SeedData(userService, currencyRepository);

          

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<CurencyRateHub>("/CurrencyRateHub");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
