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
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddTransient<IHopkinsModelReader, CasesModelReader>();
			services.AddTransient<IHopkinsModelReader, DeathsModelReader>();
			services.AddTransient<IHopkinsModelReader, RecoveredModelReader>();

			services.AddTransient<IHopkinsModelRepository, HopkinsModelRepositoryCsv>();

			/*
			services.AddTransient<ICovid19DeathsModelReader>(
				s => new Covid19DeathsModelFileCacheReader(new EcdcModelDowloader(), "opendata.ecdc.europa.eu-covid19.json"));
			services.AddTransient<IHopkinsModelRepository, EcdcModelRepositoryJson>();
			*/

			services.AddTransient<IPopulationCountryRepository, PopulationCountryRepositoryCsv>();
			services.AddTransient<IPopulationCountryService, PopulationCountryService>();

			services.AddTransient<IViewModelService, ViewModelService>();
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

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{Metric=" + ViewModelService.DATA_DEATHS + "}/{action=Absolute}/{option?}");
				routes.MapRoute(
					name: "error",
					template: "{controller=Error}/{action=Index}");
			});
		}
	}
}
