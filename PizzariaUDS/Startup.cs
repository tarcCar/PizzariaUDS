using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PizzariaUDS.Repositories;
using PizzariaUDS.Repositories.Interfaces;
using PizzariaUDS.Services;
using PizzariaUDS.Services.Interfaces;

namespace PizzariaUDS
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
            //configurando o gzip da compressao das respostas
            services.Configure<GzipCompressionProviderOptions>(
               options => options.Level = CompressionLevel.Optimal
               );

            services.AddResponseCompression(
                options =>
                {
                    options.Providers.Add<GzipCompressionProvider>();
                    options.EnableForHttps = true;
                });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    //configura para não colocar campos com valor nulo nas respostas
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            //Ativa o uso de cache em memoria
            services.AddMemoryCache();

            //Injeçoes de depencias
            #region Injeção dependencias
            services.AddSingleton<IRepository, MySQLRepository>();
            #region Services
            services.AddTransient<ITamanhoService, TamanhoService>();
            #endregion
            #region Repositories
            services.AddTransient<ITamanhoRepository, TamanhoRepository>();
            #endregion
            #endregion
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseMvc();
        }
    }
}
