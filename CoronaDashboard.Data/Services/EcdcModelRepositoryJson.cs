using Csv;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CoronaDashboard.Data
{
	// https://www.ecdc.europa.eu/en/publications-data/download-todays-data-geographic-distribution-covid-19-cases-worldwide

	public class EcdcModelRepositoryJson : ICovid19DeathsModelRepository
	{
		ICovid19DeathsModelReader _reader;

		public EcdcModelRepositoryJson(ICovid19DeathsModelReader reader)
		{
			_reader = reader;
		}

		public Covid19DeathsModel GetCovid19DeathsModel()
		{
			string json = _reader.GetCovid19Deaths();
			EcdcModel ecdcModel = JsonConvert.DeserializeObject<EcdcModel>(json);

			Covid19DeathsModel model = new Covid19DeathsModel();

			model.Countries = new List<string>();
			model.MapCountryDeaths = new Dictionary<string, List<int>>();
			model.MapCountryIsoCode = new Dictionary<string, string>();

			foreach (var record in ecdcModel.records)
			{
				if (!model.MapCountryDeaths.ContainsKey(record.countriesAndTerritories))
				{
					model.MapCountryDeaths[record.countriesAndTerritories] = new List<int>();
					model.Countries.Add(record.countriesAndTerritories);

					model.MapCountryIsoCode[record.countriesAndTerritories] = record.countryterritoryCode;
				}
				model.MapCountryDeaths[record.countriesAndTerritories].Insert(0, Int32.Parse(record.deaths));
			}

			foreach (var entry in model.MapCountryDeaths)
			{
				for (int i = 1; i < entry.Value.Count; i++)
					entry.Value[i] += entry.Value[i - 1];
			}

			model.UpdateTime = DateTime.Now;

			return model;
		}
	}
}
