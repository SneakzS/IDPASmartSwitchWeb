using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartSwitchWeb.Data;
using System;
using System.Net;
using System.Net.WebSockets;
using Microsoft.AspNetCore.ResponseCompression;
using SmartSwitchWeb.Handlers;
using SmartSwitchWeb.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

namespace SmartSwitchWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            using (DeviceContext context = new DeviceContext())
            {
                List<Device> deviceList = context.GetAll().FindAll(d => d.Status != DeviceStatus.Offline);
                foreach (Device device in deviceList)
                {
                    device.Status = DeviceStatus.Offline;
                }
                context.SaveChanges();
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IWebSocketClientHandler>(new WebSocketClientHandler());

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "frontend/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseStatusCodePages();

            app.UseWebSockets();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
                        {
                            spa.Options.SourcePath = "frontend";

                            if (env.IsDevelopment())
                            {
                                spa.UseReactDevelopmentServer(npmScript: "start");
                            }
                        });

        }
    }
}
