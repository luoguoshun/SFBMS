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
        /// ����ʱ���ã���������HTTP����ܵ���
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory">��־����</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            loggerFactory.ConfigureNLog("Nlog.config");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<GlobalExceptionMiddleware>();//ȫ���쳣�����м��
            #region ����Swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            typeof(AppTypes).GetEnumNames().ToList().ForEach(Version =>
            {
                options.SwaggerEndpoint($"/swagger/{Version}/swagger.json", $"�汾ѡ��:{Version}");
                //·��awagger�������ã�����Ϊ�գ���ʾֱ���ڸ�������localhost:8001�����ʸ��ļ�,
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

        #region ����Autofac 
        /// <summary>
        /// �Զ�����������ע��
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

                #region ���нӿڲ�ķ���ע��
                var servicesDllFile = Path.Combine(AppContext.BaseDirectory, "SFBMS.Service.dll");
                var repositoryDllFile = Path.Combine(AppContext.BaseDirectory, "SFBMS.Repository.dll");
                if (!(File.Exists(servicesDllFile) || File.Exists(repositoryDllFile)))
                {
                    var msg = "Repository.dll��service.dll ���ܶ�ʧ!!��� bin �ļ��У���������";
                    //Log4NetHelper.LogErr(msg);
                    throw new Exception(msg);
                }
                //RegisterAssemblyTypes()�ڳ�����ע���������͡����ؽ��: ע������������������ע�ᡣ
                //AsImplementedInterfaces()ָ������ɨ������е�����ע��Ϊ�ṩ����
                //InstancePerLifetimeScope()����������Ա�ÿ�������������ý��������ڵ���ILIFIETimeScope�У������ͬ�Ĺ���ʵ����
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
        /// Swagger����
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerGeneratorOptions.ConflictingActionsResolver = (apis) => apis.First();
                #region ����ע��
                string xmlPath = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "SFBMS.WebAPI.xml");
                c.IncludeXmlComments(xmlPath);
                #endregion

                #region ����Я��token��֤��ť
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "������Token",
                    Name = "Authorization",//������Ĭ�ϵĲ�������
                    In = ParameterLocation.Header,//��token����header��
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

                #region ֧�ְ汾����
                typeof(AppTypes).GetEnumNames().ToList().ForEach(Version =>
                {
                    c.SwaggerDoc(Version, new OpenApiInfo
                    {
                        Title = $"{Version}:WebApi",
                        Version = Version,
                        Description = $"Sunflower:{Version}�汾"
                    });
                });
                #endregion

            });
            return services;
        }
        /// <summary>
        /// EFCore����
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddEFCore(this IServiceCollection services, IConfiguration configuration)
        {
            //�����ĵ���ȡ�����ַ���ע��DBContext
            services.AddDbContext<SFBMSContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SFBMSConnection"));
            });
            return services;
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomAddCors(this IServiceCollection services)
        {

            #region ָ��������
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

            #region �����κ���վ����
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
            #endregion
            return services;
        }
        /// <summary>
        /// �쳣����
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCusomException(this IServiceCollection services)
        {
            #region �����쳣����������
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
        /// IdentityServer����
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
                .AddInMemoryApiResources(Config.ApiSources)//ע����Դ
                .AddInMemoryClients(Config.Clients)//ע��ģʽ
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();//��֤����
            return services;
        }
        /// <summary>
        /// �Զ���AccessToken��֤����
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            //��֤
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.Authority = configuration["IdentityServer:Address"];//��Դ
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
                 options.Audience = "SFBMS.WebAPI";//����
             });
            //��Ȩ
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