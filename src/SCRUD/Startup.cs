using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Entity;
using SCRUD.Models;
using System.IO;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc.Filters;

namespace SCRUD
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
			Environment = env;
        }

        public IConfigurationRoot Configuration { get; set; }
		public IHostingEnvironment Environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			var defaultPolicy = new AuthorizationPolicyBuilder()
										.RequireAuthenticatedUser()
										//.RequireRole("role-i-am-in")
										.Build();

			// Add framework services.
			services.AddMvc(setup =>
			{
				setup.Filters.Add(new AuthorizeFilter(defaultPolicy));
			});

			//services.AddAuthorization(options =>
			//{
			//	options.AddPolicy("Testing", policy =>
			//	{
			//		policy.RequireRole("role-i-am-in");
			//	});
			//});

			services.AddScoped<Models.SelectLists>();

			var connection = Configuration["Data:DefaultConnection:ConnectionString"];

			var path = Environment.WebRootPath;
			var dir = Directory.GetParent(path).Parent.FullName;
			connection = string.Format(connection, dir);

			services.AddEntityFramework()
				.AddSqlServer()
				.AddDbContext<SCRUDContext>(options => options.UseSqlServer(connection));

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

//			app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
