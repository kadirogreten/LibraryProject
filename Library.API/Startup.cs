using Library.API.AuthExtensions;
using Library.API.Filters;
using Library.API.ViewModels;
using Library.Core.Context;
using Library.Domain.Models;
using Library.ServiceLayer.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Library.API
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IAuthenticationExtension, AuthenticationExtension>();
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddScoped<IUserClaimsPrincipalFactory<LibraryUser>, CustomClaimsPrincipalFactory>();

            services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<LibraryUser, IdentityRole>()
                .AddEntityFrameworkStores<LibraryDbContext>()
                .AddDefaultTokenProviders();

            
            


            services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                });
            services.AddVersionedApiExplorer(
               options =>
               {
                   // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                   // note: the specified format code will format the version as "'v'major[.minor][-status]"
                   options.GroupNameFormat = "'v'VVV";

                   // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                   // can also be used to control the format of the API version in route templates
                   options.SubstituteApiVersionInUrl = true;
               });


            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidAudience = "https://localhost:5001",
                        ValidIssuer = "https://localhost:5001",

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KadirOgreten2021!!!!"))
                    };
                });

            services.AddSwaggerGen(c =>
            {

               
                // add a custom operation filter which sets default values
                c.OperationFilter<SwaggerDefaultValues>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

               
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Api anahtarınızı giriniz.",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

            });



            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministratorOnly", policy => policy.RequireRole("Admin"));
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();

            });


            services.AddScoped<IpAccessActionFilter>(container =>
            {
                var loggerFactory = container.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<IpAccessActionFilter>();

                return new IpAccessActionFilter(
                    Configuration["AdminSafeList"], logger);
            });


            

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });


            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="provider"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMiddleware<SwaggerAuthorizedMiddleware>(Configuration["SwaggerSafeList"]);
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }


                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                app.UseMiddleware<SwaggerAuthorizedMiddleware>(Configuration["SwaggerSafeList"]);
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {

                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });


            }


            app.UseMiddleware<AdminSafeListMiddleware>(Configuration["AdminSafeList"]);



            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
