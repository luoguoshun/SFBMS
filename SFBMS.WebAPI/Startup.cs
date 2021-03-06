using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using SFBMS.Common.CatchResource;
using SFBMS.Common.CatchResource.Implement;
using SFBMS.Common.DocumentHelper;
using SFBMS.Common.EnumList;
using SFBMS.Common.SiteConfig;
using SFBMS.Entity.Context;
using SFBMS.Repository.SystemModule;
using SFBMS.Repository.SystemModule.Implement;
using SFBMS.SignalR;
using SFBMS.SignalR.ClientServer;
using SFBMS.SignalR.Hub;
using SFBMS.WebAPI.Infrastructure.ExceptionsFilter;
using SFBMS.WebAPI.Infrastructure.HostedService;
using SFBMS.WebAPI.Infrastructure.IdentityServer4;
using SFBMS.WebAPI.Infrastructure.PolicyHelper;
using static SFBMS.Common.EnumList.AppTypes;

namespace SFBMSAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddControllers();
            services.AddCustomSwagger()
                    .AddEFCore(Configuration)
                    .AddCustomAddCors()
                    .AddCusomException()
                    .AddIdentityservice(Configuration)
                    .AddTokenAuthentication(Configuration)
                    .AddCustomBackgroundTask(Configuration)
                    .AddCustomSignalR();
        }
        /// <summary>
        /// ????????????????????HTTP??????????
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory">????????</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //???????????????????????????????????????????????????????????????????? 
            //????????????????????????????????????????????????????????????????????????
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseMiddleware<MiddlewareExceptionFilter>();

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            typeof(AppTypes).GetEnumNames().ToList().ForEach(Version =>
            {
                options.SwaggerEndpoint($"/swagger/{Version}/swagger.json", $"????????:{Version}");
                //????awagger??????????????????????????????????????127.0.0.1:36779????????????
                options.RoutePrefix = string.Empty;
            })
            );
            #endregion

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = new PathString("/src"),
                OnPrepareResponse = (c) =>
                {
                    c.Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                }
            });

            app.UseRouting();

            app.UseCors("AllowCors");

            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/ChatHub", options =>
                {
                    //???? ?????? ?????? WebSockets ????
                    options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
                }).RequireCors(t => t.WithOrigins(new string[] { "http://127.0.0.1:36779" })
                  .AllowAnyHeader()
                  .WithMethods("GET", "POST")
                  .AllowCredentials());
            });
        }

        #region Autofac 
        /// <summary>
        /// ??????????????????
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
        }
        public class AutofacModuleRegister : Autofac.Module
        {
            protected override void Load(ContainerBuilder container)
            {
                container.RegisterType<PolicyHandler>().As<IAuthorizationHandler>().SingleInstance();
                container.RegisterType<SFBMSContext>().As<DbContext>().AsImplementedInterfaces();
                //????????????????(??????????????Id)
                container.RegisterType<ChatClientServer>().As<IChatClientServer>().SingleInstance();
                container.RegisterType<HotNews>().As<IHotNews>().AsImplementedInterfaces();
                container.RegisterType<Spreadsheet>().InstancePerLifetimeScope();

                #region ????????????????????
                var servicesDllFile = Path.Combine(AppContext.BaseDirectory, "SFBMS.Service.dll");
                var repositoryDllFile = Path.Combine(AppContext.BaseDirectory, "SFBMS.Repository.dll");
                if (!(File.Exists(servicesDllFile) || File.Exists(repositoryDllFile)))
                {
                    string msg = "Repository.dll??service.dll ????????!!???? bin ????????????????";                   
                    Console.WriteLine($"????????:{msg}");
                }
                //RegisterAssemblyTypes()????????????????????????????????: ??????????????????????????
                //AsImplementedInterfaces()????????????????????????????????????????
                //InstancePerLifetimeScope()??????????????????????????????????????????????ILIFIETimeScope????????????????????????
                container.RegisterAssemblyTypes(Assembly.LoadFrom(servicesDllFile), Assembly.LoadFrom(repositoryDllFile))
                          .Where(t => (t.FullName.EndsWith("Service") || t.FullName.EndsWith("Repository")) && !t.IsAbstract)
                          .AsImplementedInterfaces()
                          .InstancePerLifetimeScope()
                          .EnableInterfaceInterceptors();
                #endregion
            }
        }
        #endregion
    }
    internal static class CustomExtensionsMethods
    {
        /// <summary>
        /// Swagger????
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerGeneratorOptions.ConflictingActionsResolver = (apis) => apis.First();
                #region ????????
                string xmlPath = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "SFBMS.WebAPI.xml");
                c.IncludeXmlComments(xmlPath);
                #endregion

                #region ????????token????????
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "??????Token",
                    Name = "Authorization",//????????????????????
                    In = ParameterLocation.Header,//??token????header??
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"

                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                       new OpenApiSecurityScheme{
                        Reference =new OpenApiReference{Type=ReferenceType.SecurityScheme,Id="Bearer"}
                        }
                       ,new string[]{}
                    }
                });
                #endregion

                #region ????????????
                typeof(AppTypes).GetEnumNames().ToList().ForEach(Version =>
                {
                    c.SwaggerDoc(Version, new OpenApiInfo
                    {
                        Title = $"{Version}:WebApi",
                        Version = Version,
                        Description = $"Sunflower:{Version}????"
                    });
                });
                #endregion

            });
            return services;
        }
        /// <summary>
        /// EFCore????
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddEFCore(this IServiceCollection services, IConfiguration configuration)
        {
            //??????????????????????????DBContext
            services.AddDbContext<SFBMSContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SFBMSConnection"));
            });
            return services;
        }
        /// <summary>
        /// ????????
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomAddCors(this IServiceCollection services)
        {

            #region ??????????
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(MyAllowSpecificOrigins, builder =>
            //                      {
            //                          builder.WithOrigins("http://example.com", "http://www.contoso.com")
            //                                 .AllowAnyHeader()
            //                                 .AllowAnyMethod();
            //                      });
            //});
            #endregion

            services.AddCors(options =>
            {
                options.AddPolicy("AllowCors", builder =>
                {
                    builder.SetIsOriginAllowed(origin => true)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });
            return services;
        }
        /// <summary>
        /// ????????
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCusomException(this IServiceCollection services)
        {
            #region ??????????????????
            //API
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionsFilter));
            });
            //MVC
            services.AddMvc(option =>
            {
                option.Filters.Add(typeof(GlobalExceptionsFilter));
            });
            #endregion
            return services;
        }
        /// <summary>
        /// IdentityServer????
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityservice(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityServer(options =>
            {
                options.IssuerUri = configuration.GetSection("IdentityServer").GetValue<string>("Address");
            })
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.ApiSources)//????????
                .AddInMemoryClients(Config.Clients)//????????
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();//????????
            return services;
        }
        /// <summary>
        /// ??????AccessToken????????
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            //????
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.Authority = configuration["IdentityServer:Address"];//????
                 options.RequireHttpsMetadata = false;
                 options.Events = new JwtBearerEvents
                 {
                     OnMessageReceived = context =>
                     {
                         var accessToken = context.Request.Query["access_token"];
                         var path = context.HttpContext.Request.Path;
                         if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub"))
                         {
                             context.Token = accessToken;
                         }
                         return Task.CompletedTask;
                     }
                 };
                 options.Audience = "SFBMS.WebAPI";//????
             });
            //????
            services.AddAuthorization(options =>
            {
                typeof(AppTypes).GetEnumNames().ToList().ForEach(AppType =>
                {
                    //AddPolicy(string policyName ,Action<AuthorizationPolicyBuilder> configurePolicy)
                    options.AddPolicy(AppType, policy =>
                    {
                        AppTypes type = (AppTypes)Enum.Parse(typeof(AppTypes), AppType);
                        policy.Requirements.Add(new PolicyRequirement(new AppTypes[] { type }));
                    });
                });
            });
            return services;
        }
        /// <summary>
        /// ????????????
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomBackgroundTask(this IServiceCollection services, IConfiguration configuration)
        {
            #region ????????
            //services.AddSingleton<MonitorLoop>();
            //services.AddHostedService<QueuedServiceCenter>();
            //services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>(ctx =>
            //{
            //    //int.TryParse()????????????????????????????????????32????????????
            //    if (!int.TryParse(configuration["QueueCapacity"], out int queueCapacity))
            //    {
            //        queueCapacity = 100;/*????????*/
            //    }
            //    return new BackgroundTaskQueue(queueCapacity);
            //});
            #endregion
            //services.AddHostedService<PushNoticeService>();
            services.AddHostedService<StatisticsSubscribeService>();
            return services;
        }
        /// <summary>
        /// SingleR????
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomSignalR(this IServiceCollection services)
        {
            //??????????==>ChatHub ??????????????????????????
            services.AddSignalR().AddHubOptions<ChatHub>(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true; /*????????????????????????????????????????????????????*/
                hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(10);/* ???????????????????????????????????????????????? ping ????????????????????????*/
            }).AddJsonProtocol(options =>
                {
                    //??????????????????????
                    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                });
            return services;
        }
    }

}
