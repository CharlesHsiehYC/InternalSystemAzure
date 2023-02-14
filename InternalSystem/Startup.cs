using InternalSystem.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSystem
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
            //使用cookie做驗證
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                //未登入時會自動導到登入頁面
                option.LoginPath = new PathString("/api/LoginTest/NoLogin");

                /*
                //權限被拒絕倒到登入頁面
                option.AccessDeniedPath= new PathString("/api/LoginTest/NoAccess");
                */
            });

            /*
            //全部的API都需驗證(登入)才能使用功能
            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            });
            */

            //注入取得使用者資訊
            //services.AddHttpContextAccessor();

            services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "InternalSystem", Version = "v1" });
            //});
            services.AddDbContext<MSIT44Context>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MSIT44DbContext")));
            //services.AddDbContext<MSIT44Context2>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("MSIT44DbContext")));
            //services.AddControllers().AddNewtonsoftJson(options =>
            //    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InternalSystem v1"));
            }

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            //Cookie設定，順序必須要一樣
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
