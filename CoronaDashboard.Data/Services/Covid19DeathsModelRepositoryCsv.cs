using Csv;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CoronaDashboard.Data
{
	public class Covid19DeathsModelRepositoryCsv : ICovid19DeathsModelRepository
	{
		const int FIRST_COL_DATE = 4;

		ICovid19DeathsModelReader _reader;

		public Covid19DeathsModelRepositoryCsv(ICovid19DeathsModelReader reader)
		{
			_reader = reader;
		}

		// 2/24/20
		const string DATE_FORMAT = "M/d/y";
		public Covid19DeathsModel GetCovid19DeathsModel()
		{
			Covid19DeathsModel model = new Covid19DeathsModel();
			string csv = _reader.GetCovid19Deaths();

			string dateUpdated = csv.Substring(0, csv.IndexOf(Environment.NewLine));
			model.UpdateTime = DateTime.ParseExact(dateUpdated, "s", CultureInfo.InvariantCulture);

			var options = new CsvOptions
			{
				RowsToSkip = 1,
			};

			model.Countries = new List<String>();
			model.MapCountryDeaths = new Dictionary<string, List<int>>();
			foreach (var line in CsvReader.ReadFromText(csv, options))
			{
				if (model.Dates == null)
				{
					model.Dates = new List<DateTime>();

					for (int i = FIRST_COL_DATE; i < line.Headers.Length; i++)
					{
						DateTime date = DateTime.ParseExact(line.Headers[i], DATE_FORMAT, CultureInfo.InvariantCulture);
						model.Dates.Add(date);
					}
				}
				string country = line["Country/Region"];

				if (!model.MapCountryDeaths.ContainsKey(country))
				{
					model.MapCountryDeaths[country] = new List<int>();
					for (int i = FIRST_COL_DATE; i < line.Headers.Length; i++)
					{
						model.MapCountryDeaths[country].Add(0);
					}

					model.Countries.Add(country);
				}

				for (int i = FIRST_COL_DATE; i < line.Headers.Length; i++)
				{
					string deaths = line[i];
					if (deaths != String.Empty)
						model.MapCountryDeaths[country][i - FIRST_COL_DATE] += Int32.Parse(deaths);
				}
			}
			return model;
		}
	}
}
