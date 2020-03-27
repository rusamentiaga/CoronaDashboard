using Csv;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoronaDashboard.Data
{
	public class PopulationCountryRepositoryCsv : IPopulationCountryRepository
	{
		const string FILE_POPULATION = "API_SP.POP.TOTL_DS2_en_csv_v2_887275.csv";
		const string FILE_DENSITY = "API_EN.POP.DNST_DS2_en_csv_v2_887474.csv";
		const int ROWS_TO_SKIP = 4;
		const int MIN_VALID_INDEX = 4;

		class CountryData
		{
			public string Name { get; set; }
			public string IsoCode { get; set; }
			public double Data { get; set; }
		}

		private List<CountryData> ReadDataFile(string filename)
		{
			var options = new CsvOptions
			{
				RowsToSkip = ROWS_TO_SKIP,
			};

			List<CountryData> data = new List<CountryData>();

			var csv = File.ReadAllText(filename);
			foreach (var line in CsvReader.ReadFromText(csv, options))
			{
				int validIndex = line.ColumnCount - 1;
				while ((validIndex > 0) && (line[validIndex].Length == 0))
					validIndex--;

				if (validIndex >= MIN_VALID_INDEX)
				{
					CountryData item = new CountryData
					{
						Name = line[0].Trim('"'),
						IsoCode = line[1].Trim('"'),
						Data = Double.Parse(line[validIndex].Trim('"'))
					};
					data.Add(item);
				}
				else
				{
					CountryData item = new CountryData
					{
						Name = line[0].Trim('"'),
						IsoCode = line[1].Trim('"'),
						Data = Double.NaN
					};
					data.Add(item);
				}
			}
			return data;
		}

		public ICollection<PopulationCountry> GetPopulationCountry()
		{
			List<CountryData> sourcePopulation = ReadDataFile(FILE_POPULATION);
			List<CountryData> sourceDensity = ReadDataFile(FILE_DENSITY);

			if (sourcePopulation.Count != sourceDensity.Count)
				throw new FormatException();

			List<PopulationCountry> data = new List<PopulationCountry>();

			for (int i = 0; i < sourcePopulation.Count; i++)
			{
				PopulationCountry item = new PopulationCountry
				{
					Name = sourcePopulation[i].Name,
					IsoCode = sourcePopulation[i].IsoCode,
					Population = sourcePopulation[i].Data,
					DensityPeoplePerSquareKm = sourceDensity[i].Data
				};
				data.Add(item);
			}
			return data;
		}
	}
}
