﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TvMazeScrapper.DataAccess;
using TvMazeScrapper.Infrastructure;
using TvMazeScrapper.Infrastructure.Http;
using TvMazeScrapper.Infrastructure.Interfaces.Api;
using TvMazeScrapper.Infrastructure.Interfaces.App;
using TvMazeScrapper.Infrastructure.Interfaces.DataServices;
using TvMazeScrapper.Services.Api;
using TvMazeScrapper.Services.Api.TvMazeApi;
using TvMazeScrapper.Services.App;

namespace TvMazeScrapper.WebApi
{
    public class Startup
    {
        private const string DATABASE_NAME = "MazeScrapper";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ShowsContext>(opt => opt.UseInMemoryDatabase(DATABASE_NAME), ServiceLifetime.Singleton);

            services.AddSingleton<IJsonConverter, NewtonJsonConverter>();
            services.AddSingleton<IHttpClient, HttpClient>();
            services.AddSingleton<IScrapperApiService, ScrapperService>();
            services.AddSingleton<ITvMazeApiService, TvMazeService>();
            services.AddSingleton<IPageRepository, PageRepository>();
            services.AddSingleton<IMapper, Mapper>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DateFormatString = Defines.DATE_FORMAT;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}