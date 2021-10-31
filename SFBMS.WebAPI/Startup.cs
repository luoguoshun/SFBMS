using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using SFBMS.Common.CatchResource;
using SFBMS.Common.CatchResource.Implement;
using SFBMS.Common.EnumList;
using SFBMS.Common.SiteConfig;
using SFBMS.Entity.Context;
using SFBMS.Repository.SystemModule;
using SFBMS.Repository.SystemModule.Implement;
using SFBMS.WebAPI.Infrastructure.ExceptionsFilter;
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
            services.AddScoped<SiteConfigHelper>();
            services.AddHttpClient();
            services.AddControllers();
            services.AddCustomSwagger()
                    .AddEFCore(Configuration)
                    .AddCustomAddCors()
                    .AddCusomException()
                    .AddIdentityservice(Configuration)
                    .AddTokenAuthentication(Configuration);
        }
        /// <summary>
        /// 运行时调用，用于配置HTTP请求管道。
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory">日志工厂</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {          
            //请求管道中的每个中间件组件负责调用管道中的下一个组件，或使管道短路。 
            //当中间件短路时，它被称为“终端中间件”，因为它阻止中间件进一步处理请求。
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseMiddleware<GlobalExceptionMiddleware>();

            #region 配置Swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            typeof(AppTypes).GetEnumNames().ToList().ForEach(Version =>
            {
                options.SwaggerEndpoint($"/swagger/{Version}/swagger.json", $"版本选择:{Version}");
                //路径awagger访问配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,
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
            });
        }

        #region 集成Autofac 
        /// <summary>
        /// 自定义容器服务注册
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
                container.RegisterType<HotNews>().As<IHotNews>().AsImplementedInterfaces();

                //container.RegisterType<ChatHub>().As<Hub>().SingleInstance();
                //container.RegisterType<ChatClientServer>().As<IChatClientServer>().AsImplementedInterfaces();

                #region 带有接口层的服务注入
                var servicesDllFile = Path.Combine(AppContext.BaseDirectory, "SFBMS.Service.dll");
                var repositoryDllFile = Path.Combine(AppContext.BaseDirectory, "SFBMS.Repository.dll");
                if (!(File.Exists(servicesDllFile) || File.Exists(repositoryDllFile)))
                {
                    var msg = "Repository.dll和service.dll 可能丢失!!检查 bin 文件夹，并拷贝。";
                    //Log4NetHelper.LogErr(msg);
                    throw new Exception(msg);
                }
                //RegisterAssemblyTypes()在程序集中注册所有类型。返回结果: 注册生成器，允许配置注册。
                //AsImplementedInterfaces()指定将已扫描程序集中的类型注册为提供所有
                //InstancePerLifetimeScope()配置组件，以便每个依赖组件或调用解析（）在单个ILIFIETimeScope中，获得相同的共享实例。
                container.RegisterAssemblyTypes(Assembly.LoadFrom(servicesDllFile), Assembly.LoadFrom(repositoryDllFile))
                          .Where(t => (t.FullName.EndsWith("Service") || t.FullName.EndsWith("Repository")) && !t.IsAbstract)
                          .AsImplementedInterfaces()
                          .InstancePerLifetimeScope()
                          .EnableInterfaceInterceptors();

                container.RegisterAssemblyTypes(Assembly.LoadFrom(repositoryDllFile))
                          .InstancePerLifetimeScope();
                #endregion
            }
        }
        #endregion
    }
    internal static class CustomExtensionsMethods
    {
        /// <summary>
        /// Swagger服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerGeneratorOptions.ConflictingActionsResolver = (apis) => apis.First();
                #region 添加注释
                string xmlPath = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "SFBMS.WebAPI.xml");
                c.IncludeXmlComments(xmlPath);
                #endregion

                #region 添加携带token验证按钮
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "请输入Token",
                    Name = "Authorization",//劲舞团默认的参数名称
                    In = ParameterLocation.Header,//把token放在header中
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

                #region 支持版本控制
                typeof(AppTypes).GetEnumNames().ToList().ForEach(Version =>
                {
                    c.SwaggerDoc(Version, new OpenApiInfo
                    {
                        Title = $"{Version}:WebApi",
                        Version = Version,
                        Description = $"Sunflower:{Version}版本"
                    });
                });
                #endregion

            });
            return services;
        }
        /// <summary>
        /// EFCore服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddEFCore(this IServiceCollection services, IConfiguration configuration)
        {
            //配置文档获取连接字符串注入DBContext
            services.AddDbContext<SFBMSContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SFBMSConnection"));
            });
            return services;
        }
        /// <summary>
        /// 跨域服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomAddCors(this IServiceCollection services)
        {

            #region 指定的域名
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
        /// 异常处理
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCusomException(this IServiceCollection services)
        {
            #region 配置异常处理过滤器
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
        /// IdentityServer服务
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
                .AddInMemoryApiResources(Config.ApiSources)//注册资源
                .AddInMemoryClients(Config.Clients)//注册模式
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();//验证规则
            return services;
        }
        /// <summary>
        /// 自定义AccessToken验证规则
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            //认证
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.Authority = configuration["IdentityServer:Address"];//来源
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
                 options.Audience = "SFBMS.WebAPI";//受众
             });
            //授权
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
    }
    
}
