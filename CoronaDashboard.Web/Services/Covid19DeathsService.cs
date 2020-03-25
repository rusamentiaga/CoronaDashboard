using CoronaDashboard.Data;
using CoronaDashboard.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaDashboard.Web.Services
{
	public class Covid19DeathsService: ICovid19DeathsService
	{
		public const int MIN_DEATHS = 10;
		private const int MIN_POPULATION = 10000000;
		private const int POPULATION_SCALE = 1000000;
		private const int DENSITY_SCALE = 1;

		public const string NORM_POPULATION = "million";
		public const string NORM_DENSITY = "people per sq. km";

		ICovid19DeathsModelRepository _repository;
		IPopulationCountryService _countryService;

		Dictionary<string, Func<PopulationCountry, double>> _normalizationStrategyMap;

		private double NormalizationPopulation(PopulationCountry country) => POPULATION_SCALE / country.Population;
		private double NormalizationDensity(PopulationCountry country) => DENSITY_SCALE / country.DensityPeoplePerSquareKm;

		public Covid19DeathsService(ICovid19DeathsModelRepository repository, IPopulationCountryService countryService)
		{
			_repository = repository;
			_countryService = countryService;

			_normalizationStrategyMap = new Dictionary<string, Func<PopulationCountry, double>>();
			_normalizationStrategyMap[NORM_POPULATION] = NormalizationPopulation;
			_normalizationStrategyMap[NORM_DENSITY] = NormalizationDensity;
		}

		public PlotViewModel GetAbsoluteDataViewModel()
		{
			Covid19DeathsModel model = _repository.GetCovid19DeathsModel();

			List<CountrySerieViewModel> series = new List<CountrySerieViewModel>();

			model.Countries.Sort();
			foreach (var country in model.Countries)
			{
				List<int> ints = model.MapCountryDeaths[country];

				List<double> data = ints.Select(i => (double)i).ToList();
				double max = data.Max();

				if (max > MIN_DEATHS)
				{
					var item = new CountrySerieViewModel
					{
						name = country,
						data = data.Where(d => d > MIN_DEATHS).ToList()
					};
					series.Add(item);
				}
			}
			return new PlotViewModel { Series = series, UpdateTime = model.UpdateTime };
		}

		public PlotViewModel GetGrowthViewModel()
		{
			Covid19DeathsModel model = _repository.GetCovid19DeathsModel();

			List<CountrySerieViewModel> series = new List<CountrySerieViewModel>();

			model.Countries.Sort();
			foreach (var country in model.Countries)
			{
				List<int> ints = model.MapCountryDeaths[country];
				double max = ints.Max();

				if (max > MIN_DEATHS)
				{
					List<double> data = new List<double>();

					for (int i = 0; i < ints.Count - 1; i++)
					{
						data.Add(ints[i + 1] - ints[i]);
					}

					var item = new CountrySerieViewModel
					{
						name = country,
						data = data.Where(d => d > MIN_DEATHS).ToList()
					};
					series.Add(item);
				}
			}
			return new PlotViewModel { Series = series, UpdateTime = model.UpdateTime };
		}

		public PlotViewModel GetRelativeViewModel(string option)
		{
			PlotViewModel model = GetAbsoluteDataViewModel();
			List<CountrySerieViewModel> series = model.Series.ToList();

			series.RemoveAll(item => _countryService.GetCountry(item.name).Population < MIN_POPULATION);

			var normalizationStrategy = _normalizationStrategyMap[option];

			foreach (var item in series)
			{
				List<double> data = item.data;
				PopulationCountry country = _countryService.GetCountry(item.name);

				double scale = normalizationStrategy.Invoke(country);

				for (int i = 0; i < data.Count; i++)
					item.data[i] *= scale;
			}
			return model;
		}
	}
}
