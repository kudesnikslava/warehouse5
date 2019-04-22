using System;
using System.IO;
using System.Xml.XPath;
using AutoMapper;
using CommonLibrary.Cache.Implementations;
using CommonLibrary.Cache.Interfaces;
using CommonLibrary.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using ware_house.Managers;
using ware_house.Managers.Cache;
using ware_house.Managers.Cache.Interfaces;
using ware_house.Managers.Interfaces;
using ware_house.Models;
using ware_house.Repositories.Implementations;
using ware_house.Repositories.Interfaces;
using Warehouse.Models;
using Warehouse.Models.Requests;
using Warehouse.Models.Responses;



namespace ware_house
{
    public class Startup
    {
	    private readonly string ApiName = "ware_house";
	    private readonly IHostingEnvironment _hostingEnv;


	    /// <summary>
	    /// Constructor
	    /// </summary>
	    /// <param name="configuration"></param>
	    /// <param name="hostingEnv"></param>
	    public Startup(IConfiguration configuration, IHostingEnvironment hostingEnv)
	    {
		    _hostingEnv = hostingEnv;
		    Configuration = configuration;

			//var builder = new ConfigurationBuilder();

			//if (_hostingEnv.IsEnvironment("Development"))
			//{
			//	builder.AddUserSecrets<Startup>();
			//}

			//builder.AddConfiguration(configuration);
			//Configuration = builder.Build();
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
        {
	        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(opts =>
	        {
		        opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
	        });

			services.AddSwaggerGen(options => { SetupSwagger(options, ApiName, _hostingEnv); });

	        services.Configure<RedisCacheConfiguration>(x =>
	        {
		        x.ConnectionString = RedisConnectionString(Configuration);
		        x.Environment = "Development";
		        x.ApiName = ApiName;
	        });

			Mapper.Initialize(c =>
	        {
		        c.CreateMissingTypeMaps = true;
		        c.CreateMap<Customer, CustomerResponse>();
		        c.CreateMap<CustomerCreateRequest, Customer>().ForMember(m => m.Id, expression => expression.Ignore());
		        c.CreateMap<Entity, EntityResponse>();
		        c.CreateMap<EntityCreateRequest, Entity>().ForMember(m => m.Id, expression => expression.Ignore());
		        //TODO Почему здесь нет CustomerUpdateRequest. Маппинг в контроллере есть в методе update
	        });

	        services.Configure<MongoConfiguration>(x =>
		        x.DbName = Configuration.GetValue<string>("MongoDbName") ?? "customer");

	        services.AddSingleton<IMongoClient>(x => new MongoClient(GetMongoConnectionString(Configuration)));

	        services.AddSingleton<IBaseCache, BaseCache>();
	        services.AddSingleton<ICustomersRepository, MongoDbCustomersRepository>();
	        services.AddSingleton<ICustomersCacheManager, CustomersCacheManager>();
	        services.AddSingleton<ICustomerManager, CustomerManager>();

		}



		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			if (env.IsDevelopment())
	        {
		        app.UseDeveloperExceptionPage();
	        }
	        else
	        {
		        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		        //app.UseHsts();
	        }

	        //app.UseHttpsRedirection();
	        app.UseMvc();


	        app.UseSwagger();
	        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", ApiName));
		}

	    private void SetupSwagger(SwaggerGenOptions options, string apiName, IHostingEnvironment hostingEnv)
	    {
		    options.SwaggerDoc("v1", new Info
		    {
			    Title = apiName,
			    Description = $"{apiName} (ASP.NET Core 2.2)",
			    Version = "1.0.BUILD_NUMBER"
		    });

		    options.DescribeAllEnumsAsStrings();

		    var comments =
			    new XPathDocument($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{hostingEnv.ApplicationName}.xml");
		    options.OperationFilter<XmlCommentsOperationFilter>(comments);


		    options.IgnoreObsoleteActions();
	    }

	    private string RedisConnectionString(IConfiguration configuration)
	    {
		    var redisUri = "127.0.0.1:6379";
		    if (configuration != null)
		    {
			    var configValue = configuration["SecretRedisConnectionString"] ??
			                      configuration.GetConnectionString("RedisConnectionString");
			    if (!string.IsNullOrEmpty(configValue))
			    {
				    redisUri = configValue;
			    }
		    }

		    return redisUri;
	    }


	    private string GetMongoConnectionString(IConfiguration configuration)
	    {
		    //return "mongodb+srv://slava:<password>@cluster0-wtsfy.mongodb.net/test?retryWrites=true";
		    return "mongodb + srv://slava:!@cluster0-wtsfy.mongodb.net/test?retryWrites=true";
	    }

	}
}
