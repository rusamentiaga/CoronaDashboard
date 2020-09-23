using Csv;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CoronaDashboard.Data
{
	public class HopkinsModelRepositoryCsv : IHopkinsModelRepository
	{
		const int FIRST_COL_DATE = 4;

		// 2/24/20
		const string DATE_FORMAT = "M/d/y";
		const string COUNTRY_STR = "Country/Region";

		IEnumerable<IHopkinsModelReader> _readers;

		public HopkinsModelRepositoryCsv(IEnumerable<IHopkinsModelReader> readers)
		{
			if (readers.Count() == 0)
			{
				throw new ArgumentNullException(nameof(readers));
			}

			_readers = readers;
		}

		public HopkinsModel GetHopkinsModel(string metric)
		{
			IHopkinsModelReader reader = _readers.FirstOrDefault(o => o.GetType().Name.Contains(metric));
			if (reader == null)
				reader = _readers.First();

			string csv = reader.GetRawModel();

			HopkinsModel model = new HopkinsModel();
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
				string country = line[COUNTRY_STR];

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
			model.Countries.Sort();

			return model;
		}
	}
}
