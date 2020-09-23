using Csv;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CoronaDashboard.Data
{
	// https://www.ecdc.europa.eu/en/publications-data/download-todays-data-geographic-distribution-covid-19-cases-worldwide

	public class EcdcModelRepositoryJson : IHopkinsModelRepository
	{
		IHopkinsModelReader _reader;

		public EcdcModelRepositoryJson(IHopkinsModelReader reader)
		{
			_reader = reader;
		}

		public HopkinsModel GetHopkinsModel(string metric = "")
		{
			const string DATE_FORMAT = "dd/M/yyyy";

			string json = _reader.GetRawModel();
			EcdcModel ecdcModel = JsonConvert.DeserializeObject<EcdcModel>(json);

			HopkinsModel model = new HopkinsModel();

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

			model.Dates = new List<DateTime>();

			// @@Bug: not all contries have the same number of dates
			string FirstCountry = model.Countries.First();
			foreach (var record in ecdcModel.records)
			{
				if (record.countriesAndTerritories == FirstCountry)
				{
					DateTime date = DateTime.ParseExact(record.dateRep, DATE_FORMAT, CultureInfo.InvariantCulture);
					model.Dates.Insert(0, date);
				}
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
