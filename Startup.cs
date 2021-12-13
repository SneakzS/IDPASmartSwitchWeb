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
using SmartSwitchWeb.SocketsManager;
using SmartSwitchWeb.Handlers;
using Radzen;
using SmartSwitchWeb.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSwitchWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _clientHandler = new WebSocketClientHandler();
            using (DeviceContext context = new DeviceContext())
            {
                List<Device> deviceList = context.GetAll().FindAll(d => d.Status != DeviceStatus.Offline );
                foreach (Device device in deviceList)
                {
                    device.Status = DeviceStatus.Offline;
                }
                context.SaveChanges();
            }
        }

        WebSocketClientHandler _clientHandler;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();
            services.AddSingleton<IWebSocketClientHandler>(_clientHandler);
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

            app.UseRouting();
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                
            };

            app.UseWebSockets(webSocketOptions);
            app.Use(new SocketMiddleware(_clientHandler).InvokeAsync);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
