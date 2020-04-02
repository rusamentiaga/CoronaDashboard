using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoronaDashboard.Data;
using CoronaDashboard.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoronaDashboard.Web
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
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});


			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddTransient<ICovid19DeathsModelReader>(
				s => new Covid19DeathsModelFileCacheReader(new Covid19DeathsModelDowloader()));
			services.AddTransient<ICovid19DeathsModelRepository, Covid19DeathsModelRepositoryCsv>();

			services.AddTransient<IPopulationCountryRepository, PopulationCountryRepositoryCsv>();
			services.AddTransient<IPopulationCountryService, PopulationCountryService>();

			services.AddTransient<ICovid19DeathsService, Covid19DeathsService>();
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
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();
			//app.UseCookiePolicy();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Relative}/{option?}");
			});
		}
	}
}
