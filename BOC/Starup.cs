using System;
using System.IO;
using BOC.Areas.BaggageMiss.Models;
using BOC.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BOC
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
           
            services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddSession();
            Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions.Configure<UriConfig>(services, Configuration.GetSection("Uri"));
            services.AddHttpContextAccessor();


            //Khai bao de su dung session
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            // Đăng ký dịch vụ Cookie
            services.AddAuthentication("CookieAuthentication")
                 .AddCookie("CookieAuthentication", config =>
                 {
                     config.Cookie.Name = "cookie";
                     config.LoginPath = "/Login/Index";
                 });

            //add services and middleware for MVC.
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
                        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            services.AddMvc();
            services.AddMvc(options => options.EnableEndpointRouting = false);//Đăng ký dịch vụ để Net Core hiểu Area MVC
            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();           // Đăng ký dịch vụ lưu cache trong bộ nhớ (Session sẽ sử dụng nó)
            ////Đăng ký dịch vụ Session
            services.AddSession(cfg =>
            {
                cfg.Cookie.Name = "TypeOfDevice";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
                cfg.IdleTimeout = new TimeSpan(0, 60, 0);    // Thời gian tồn tại của Session
            });
            services.AddSession(cfg =>
            {
                cfg.Cookie.Name = "Fleet";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
                cfg.IdleTimeout = new TimeSpan(0, 60, 0);    // Thời gian tồn tại của Session
            });
            services.AddSession(cfg =>
            {
                cfg.Cookie.Name = "Username";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
                cfg.IdleTimeout = new TimeSpan(0, 60, 0);    // Thời gian tồn tại của Session
            });

            services.AddSession(cfg =>
            {
                cfg.Cookie.Name = "Token";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
                cfg.IdleTimeout = new TimeSpan(0, 60, 0);    // Thời gian tồn tại của Session
            });


            services.AddSession(cfg =>
            {
                cfg.Cookie.Name = "DeviceID";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
                cfg.IdleTimeout = new TimeSpan(0, 60, 0);    // Thời gian tồn tại của Session
            });

            services.AddSession(cfg =>
            {
                cfg.Cookie.Name = "FlightLounge";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
                cfg.IdleTimeout = new TimeSpan(0, 60, 0);    // Thời gian tồn tại của Session
            });

            services.AddSession(cfg =>
            {
                cfg.Cookie.Name = "Airport";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
                cfg.IdleTimeout = new TimeSpan(0, 60, 0);    // Thời gian tồn tại của Session
            });


            services.AddSession(cfg =>
            {
                cfg.Cookie.Name = "AirportChoose";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
                cfg.IdleTimeout = new TimeSpan(0, 60, 0);    // Thời gian tồn tại của Session
            });


            

            services.AddControllersWithViews();//registered controllers with views and the razor pages
            services.AddRazorPages();
            services.AddMvc();
            services.AddHttpContextAccessor(); //Config để view get session ra sử dụng                                 
            services.AddScoped<IViewRenderService, ViewRenderService>();// Add Applciation Services để Render string to view



        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
           
            ////Ghi Log File
            //var path = Directory.GetCurrentDirectory();
            //loggerFactory.AddFile($"C:\\Logs\\Log.txt");
            //Đăng ký page lỗi vs status
            app.UseStatusCodePages();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Login/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Login/Error";
                    await next();
                }
            });





            //app.UseHttpsRedirection(); //Redirect http sang https
            app.UseStaticFiles();

            app.UseRouting();
            // are you allowed?  
            app.UseAuthorization();
            app.UseSession();    // Đăng ký Middleware Session vào Pipeline để sử dụng Session Object
            app.UseMvc();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //name: "MeetingRoom",
                //pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                                            name: "DarkSiteEmergency",
                                            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                                         name: "E-library",
                                         pattern: "{area:exists}/{controller=Home}/{action=Search}/{id?}");

                endpoints.MapAreaControllerRoute(
                                              name: "E-Library",
                                              areaName: "E-Library",
                                              pattern: "E-Library/{controller=Search}/{action=SearchResult}");

                endpoints.MapAreaControllerRoute(
                                              name: "STV",
                                              areaName: "STV",
                                              pattern: "STV/{controller=Home}/{action=Index}");

                endpoints.MapAreaControllerRoute(
                                              name: "BaggageMiss",
                                              areaName: "BaggageMiss",
                                              pattern: "BaggageMiss/{controller=Home}/{action=Index}/{UserCurrent?}");

                endpoints.MapAreaControllerRoute(
                                              name: "BaggageMiss",
                                              areaName: "BaggageMiss",
                                              pattern: "BaggageMiss/{controller=Pages}/{action=Index}/{t_flag?}/{t_bagmiss_id?}");
                endpoints.MapAreaControllerRoute(
                                         name: "BaggageMiss",
                                         areaName: "BaggageMiss",
                                         pattern: "BaggageMiss/{controller=MPages}/{action=Index}/{t_flag?}/{t_bagmiss_id?}");

                endpoints.MapAreaControllerRoute(
                                     name: "BaggageMiss",
                                     areaName: "BaggageMiss",
                                     pattern: "BaggageMiss/{controller=CompletePages}/{action=Index}/{t_flag?}/{t_bagmiss_id?}");


                endpoints.MapAreaControllerRoute(
                                 name: "FO",
                                 areaName: "FO",
                                 pattern: "FO/{controller=FO_DOC_READER}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                                name: "SeatMap",
                                areaName: "SeatMap",
                                pattern: "SeatMap/{controller=Home}/{action=Index}/{id?}");


                endpoints.MapControllerRoute(
                    name: "flight",
                    pattern: "{controller=FlightDate}/{action=Detail}/{AirportChoose = UrlParameter.Optional, TimeZone = UrlParameter.Optional, date = UrlParameter.Optional,key = UrlParameter.Optional,SelectedRouting= UrlParameter.Optional,ViewType= UrlParameter.Optional,Routing=UrlParameter.Optional}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");

                endpoints.MapRazorPages();

            });

        }

    }
}
