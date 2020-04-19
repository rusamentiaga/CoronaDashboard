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
		public const int MIN_DEATHS_MILLION = 1;
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

			foreach (var country in model.Countries)
			{
				List<int> ints = model.MapCountryDeaths[country];

				List<double> data = ints.Select(i => (double)i).ToList();
				double max = data.Max();

				PopulationCountry countryPop = _countryService.GetCountry(country);

				if ((max > MIN_DEATHS) && (countryPop != null))
				{
					if (countryPop.Population > MIN_POPULATION)
					{
						var item = new CountrySerieViewModel
						{
							name = country,
							data = data.Where(d => d > MIN_DEATHS).ToList()
						};
						series.Add(item);
					}
				}
			}
			return new PlotViewModel { Series = series, UpdateTime = model.UpdateTime };
		}

		public PlotViewModel GetGrowthViewModel()
		{
			Covid19DeathsModel model = _repository.GetCovid19DeathsModel();

			List<CountrySerieViewModel> series = new List<CountrySerieViewModel>();

			foreach (var country in model.Countries)
			{
				List<int> deaths = model.MapCountryDeaths[country];
				double maxDeaths = deaths.Max();

				if (maxDeaths > MIN_DEATHS)
				{
					List<double> data = new List<double>(deaths.Count - 1);

					for (int i = 0; i < deaths.Count - 1; i++)
					{
						data.Add(deaths[i + 1] - deaths[i]);
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
			Covid19DeathsModel model = _repository.GetCovid19DeathsModel();

			List<CountrySerieViewModel> series = new List<CountrySerieViewModel>();

			foreach (var country in model.Countries)
			{
				List<int> ints = model.MapCountryDeaths[country];

				List<double> data = ints.Select(i => (double)i).ToList();
				double maxDeaths = data.Max();

				var normalizationStrategy = _normalizationStrategyMap[option];
				PopulationCountry populationCountry = _countryService.GetCountry(country);

				if ((maxDeaths > MIN_DEATHS) && (populationCountry != null))
				{
					if (populationCountry.Population > MIN_POPULATION)
					{
						double scale = normalizationStrategy.Invoke(populationCountry);
						for (int i = 0; i < data.Count; i++)
							data[i] *= scale;

						var item = new CountrySerieViewModel
						{
							name = country,
							data = data.Where(d => d > MIN_DEATHS_MILLION).ToList()
						};
						series.Add(item);
					}
				}
			}
			return new PlotViewModel { Series = series, UpdateTime = model.UpdateTime };
		}

		public MapViewModel GetMapViewModel()
		{
			PlotViewModel relativeModel = GetRelativeViewModel(NORM_POPULATION);
			List<MapCountryCodeModel> series = new List<MapCountryCodeModel>();

			foreach (var itemRelative in relativeModel.Series)
			{
				PopulationCountry populationCountry = _countryService.GetCountry(itemRelative.name);
				if ( (populationCountry != null) && (itemRelative.data.Count >0) )
				{
					var itemMapModel = new MapCountryCodeModel
					{
						code3 = populationCountry.IsoCode,
						value = itemRelative.data[itemRelative.data.Count-1]
					};
					series.Add(itemMapModel);
				}
			}

			return new MapViewModel
			{
				UpdateTime = relativeModel.UpdateTime,
				Data = series
			};

		}
	}
}
