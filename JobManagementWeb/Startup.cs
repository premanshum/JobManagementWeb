using JobManagementWeb.Infrastructure;
using JobManagementWeb.Infrastructure.Interfaces.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace JobManagementWeb
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
			services.AddControllersWithViews().AddRazorRuntimeCompilation();
			services.AddMvc(mvcoption =>
			{
				mvcoption.EnableEndpointRouting = false;
			});
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddStackExchangeRedisCache(options => options.Configuration = "localhost:6379");
			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromSeconds(1000);
				options.Cookie.Name = ".JobManagementWeb.Session";
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});
			services.AddScoped(typeof(ISessionValues), typeof(SessionValues));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(
			IApplicationBuilder app,
			IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization(); 
			app.UseSession();
			
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
			//app.UseEndpoints(endpoints =>
			//{
			//	endpoints.MapControllerRoute(
			//		name: "default",
			//		pattern: "{controller=Home}/{action=Index}/{id?}");
			//});
		}
	}
}
