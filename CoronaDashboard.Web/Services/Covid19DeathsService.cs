using CoronaDashboard.Data;
using CoronaDashboard.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaDashboard.Web.Services
{
	public class ViewModelService : IViewModelService
	{
		public const int MIN_DEATHS = 10;
		public const int MIN_DEATHS_MILLION = 1;
		private const int MIN_POPULATION = 10000000;
		private const int POPULATION_SCALE = 1000000;
		private const int DENSITY_SCALE = 1;
		private const int NUM_COUNTRY_BAR = 20;

		public const string NORM_POPULATION = "million";
		public const string NORM_DENSITY = "people per sq. km";

		public const string DATA_CASES = "Cases";
		public const string DATA_DEATHS = "Deaths";

		IHopkinsModelRepository _repository;
		IPopulationCountryService _countryService;

		Dictionary<string, Func<PopulationCountry, double>> _normalizationStrategyMap;

		private double NormalizationPopulation(PopulationCountry country) => POPULATION_SCALE / country.Population;
		private double NormalizationDensity(PopulationCountry country) => DENSITY_SCALE / country.DensityPeoplePerSquareKm;

		public ViewModelService(IHopkinsModelRepository repository, IPopulationCountryService countryService)
		{
			_repository = repository;
			_countryService = countryService;

			_normalizationStrategyMap = new Dictionary<string, Func<PopulationCountry, double>>();
			_normalizationStrategyMap[NORM_POPULATION] = NormalizationPopulation;
			_normalizationStrategyMap[NORM_DENSITY] = NormalizationDensity;
		}

		public PlotViewModel GetAbsoluteDataViewModel(string metric)
		{
			HopkinsModel model = _repository.GetHopkinsModel(metric);

			List<CountrySerieViewModel> series = new List<CountrySerieViewModel>();

			foreach (var country in model.Countries)
			{
				List<int> ints = model.MapCountryDeaths[country];

				List<double> data = ints.Select(i => (double)i).ToList();
				double max = data.Max();

				PopulationCountry countryPop = _countryService.GetCountry(country, model.GetCountryIsoCode(country));

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
			return new PlotViewModel
			{
				Series = series,
				UpdateTime = model.UpdateTime,
				SeriesLast = GetLastSeriesTop(series)
			};
		}

		public PlotViewModel GetRelativeViewModel(string metric, string option, int minDeathsValue = MIN_DEATHS_MILLION)
		{
			HopkinsModel model = _repository.GetHopkinsModel(metric);

			List<CountrySerieViewModel> series = new List<CountrySerieViewModel>();

			foreach (var country in model.Countries)
			{
				List<int> deathsCountryInt = model.MapCountryDeaths[country];

				List<double> deathsCountry = deathsCountryInt.Select(i => (double)i).ToList();
				double maxDeaths = deathsCountry.Max();

				var normalizationStrategy = _normalizationStrategyMap[option];
				PopulationCountry populationCountry = _countryService.GetCountry(country, model.GetCountryIsoCode(country));

				if ((maxDeaths > MIN_DEATHS) && (populationCountry != null))
				{
					if (populationCountry.Population > MIN_POPULATION)
					{
						double scale = normalizationStrategy.Invoke(populationCountry);
						for (int i = 0; i < deathsCountry.Count; i++)
							deathsCountry[i] *= scale;

						var item = new CountrySerieViewModel
						{
							name = country,
							data = deathsCountry.Where(d => d > minDeathsValue).ToList()
						};
						if (deathsCountry.Count > 0)
							series.Add(item);
					}
				}
			}
			return new PlotViewModel
			{
				Series = series,
				UpdateTime = model.UpdateTime,
				SeriesLast = GetLastSeriesTop(series)
			};
		}

		public PlotViewModel GetGrowthViewModel(string metric)
		{
			HopkinsModel model = _repository.GetHopkinsModel(metric);

			List<CountrySerieViewModel> series = new List<CountrySerieViewModel>();

			foreach (var country in model.Countries)
			{
				List<int> deaths = model.MapCountryDeaths[country];
				double maxDeaths = deaths.Max();

				deaths = deaths.Where(d => d > MIN_DEATHS).ToList();

				PopulationCountry populationCountry = _countryService.GetCountry(country, model.GetCountryIsoCode(country));

				if ((maxDeaths > MIN_DEATHS) && (deaths.Count > 1) && (populationCountry != null))
				{
					if (populationCountry.Population > MIN_POPULATION)
					{
						List<double> data = new List<double>(deaths.Count - 1);

						for (int i = 0; i < deaths.Count - 1; i++)
						{
							data.Add(deaths[i + 1] - deaths[i]);
						}

						var item = new CountrySerieViewModel
						{
							name = country,
							data = data
						};
						series.Add(item);
					}
				}
			}
			return new PlotViewModel
			{
				Series = series,
				UpdateTime = model.UpdateTime,
				SeriesLast = GetLastSeriesTop(series)
			};
		}

		public PlotViewModel GetRelativeGrowthViewModel(string metric, string option, int minDeathsValue = MIN_DEATHS_MILLION)
		{
			HopkinsModel model = _repository.GetHopkinsModel(metric);

			List<CountrySerieViewModel> series = new List<CountrySerieViewModel>();

			foreach (var country in model.Countries)
			{
				List<int> deathsCountry = model.MapCountryDeaths[country];

				double maxDeaths = deathsCountry.Max();

				var normalizationStrategy = _normalizationStrategyMap[option];
				PopulationCountry populationCountry = _countryService.GetCountry(country, model.GetCountryIsoCode(country));

				deathsCountry = deathsCountry.Where(d => d > MIN_DEATHS).ToList();

				if ((maxDeaths > MIN_DEATHS) && (populationCountry != null) && (deathsCountry.Count > 1))
				{
					if (populationCountry.Population > MIN_POPULATION)
					{
						double scale = normalizationStrategy.Invoke(populationCountry);

						List<double> dataRelative = new List<double>(deathsCountry.Count - 1);

						for (int i = 0; i < deathsCountry.Count - 1; i++)
						{
							dataRelative.Add(deathsCountry[i + 1] - deathsCountry[i]);
						}

						for (int i = 0; i < dataRelative.Count; i++)
							dataRelative[i] *= scale;

						var item = new CountrySerieViewModel
						{
							name = country,
							data = dataRelative
						};
						if (dataRelative.Count > 0)
							series.Add(item);
					}
				}
			}
			return new PlotViewModel
			{
				Series = series,
				UpdateTime = model.UpdateTime,
				SeriesLast = GetLastSeriesTop(series)
			};
		}

		public MapViewModel GetMapViewModel(string metric, string option)
		{
			HopkinsModel model = _repository.GetHopkinsModel(metric);
			PlotViewModel relativeModel = GetRelativeViewModel(metric, option, 0);
			List<MapCountryCodeModel> series = new List<MapCountryCodeModel>();

			foreach (var itemRelative in relativeModel.Series)
			{
				PopulationCountry populationCountry = _countryService.GetCountry(itemRelative.name, model.GetCountryIsoCode(itemRelative.name));
				if ((populationCountry != null) && (itemRelative.data.Count > 0))
				{
					var itemMapModel = new MapCountryCodeModel
					{
						code3 = populationCountry.IsoCode,
						value = itemRelative.data[itemRelative.data.Count - 1]
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

		private ICollection<CountryValue> GetLastSeriesTop(List<CountrySerieViewModel> series)
		{
			List<CountryValue> lastValues = new List<CountryValue>();

			foreach (var country in series)
			{
				if (country.data.Count > 0)
				{
					CountryValue cv = new CountryValue
					{
						name = country.name,
						y = country.data.Last()
					};
					lastValues.Add(cv);
				}
			}
			List<CountryValue> sortedList = lastValues.OrderByDescending(o => o.y).ToList();

			return sortedList.Take(NUM_COUNTRY_BAR).ToList();
		}

		public TimelineViewModel GetTimeline(string metric, string option)
		{
			const int SecondsHour = 3600;
			const string DateFormat = "MMMM dd yyyy";

			HopkinsModel model = _repository.GetHopkinsModel(metric);

			List<PeakDeaths> data = new List<PeakDeaths>();

			HashSet<DateTime> usedDates = new HashSet<DateTime>();
			Random random = new Random();

			foreach (var country in model.Countries)
			{
				List<int> deathsCountry = model.MapCountryDeaths[country];

				int maxDeaths = deathsCountry.Max();

				PopulationCountry populationCountry = _countryService.GetCountry(country, model.GetCountryIsoCode(country));

				if ((maxDeaths > MIN_DEATHS) && (populationCountry != null) && (deathsCountry.Count > 1))
				{
					if (populationCountry.Population > MIN_POPULATION)
					{
						List<int> dataDaily = new List<int>(deathsCountry.Count);

						dataDaily.Add(deathsCountry[0]);
						for (int i = 0; i < deathsCountry.Count - 1; i++)
						{
							dataDaily.Add(deathsCountry[i + 1] - deathsCountry[i]);
						}

						int maxDeathsDaily = dataDaily.Max();
						int maxIndexDaily = dataDaily.IndexOf(maxDeathsDaily);

						if (maxDeathsDaily > MIN_DEATHS)
						{
							DateTime peakDate;

							if (maxIndexDaily >= model.Dates.Count)
							{
								peakDate = DateTime.Now;
							}
							else
								peakDate = model.Dates[maxIndexDaily];

							// Avoid repeat exact same date
							while (usedDates.Contains(peakDate))
							{
								peakDate = model.Dates[maxIndexDaily].AddSeconds(random.Next(0, SecondsHour));
							}
							usedDates.Add(peakDate);

							string peakDateStr = peakDate.ToString(DateFormat,
									System.Globalization.DateTimeFormatInfo.InvariantInfo);

							PeakDeaths item = new PeakDeaths
							{
								x = new DateTimeOffset(peakDate.AddDays(1)).ToUnixTimeMilliseconds(),
								name = country,
								label = country,
								max = maxDeathsDaily,
								date = peakDateStr,
								description = $"{maxDeathsDaily} deaths - {peakDateStr}"
							};
							data.Add(item);
						}
					}
				}
			}

			List<CountryValue> MaxDeathsAbsolute = new List<CountryValue>();
			List<CountryValue> MaxDeathsRelative = new List<CountryValue>();
			var normalizationStrategy = _normalizationStrategyMap[option];

			foreach (var item in data)
			{
				PopulationCountry populationCountry = _countryService.GetCountry(item.name, model.GetCountryIsoCode(item.name));
				if ((populationCountry != null))
				{
					double scale = normalizationStrategy.Invoke(populationCountry);

					var itemCountryValueRelative = new CountryValue
					{
						name = item.name,
						y = item.max * scale,
						date = item.date
					};
					MaxDeathsRelative.Add(itemCountryValueRelative);

					var itemCountryValueAbsolute = new CountryValue
					{
						name = item.name,
						y = item.max,
						date = item.date
					};
					MaxDeathsAbsolute.Add(itemCountryValueAbsolute);
				}
			}
			List<CountryValue> sortedMaxDeathsRelative = MaxDeathsRelative.OrderByDescending(o => o.y).ToList();
			List<CountryValue> sortedMaxDeathsAbsolute = MaxDeathsAbsolute.OrderByDescending(o => o.y).ToList();

			return new TimelineViewModel
			{
				Data = data,
				MaxDeathsRelative = sortedMaxDeathsRelative.Take(NUM_COUNTRY_BAR).ToList(),
				MaxDeathsAbsolute = sortedMaxDeathsAbsolute.Take(NUM_COUNTRY_BAR).ToList()
			};
		}
	}
}
