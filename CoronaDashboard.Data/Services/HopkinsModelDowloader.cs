using Newtonsoft.Json.Linq;
using System;

namespace CoronaDashboard.Data
{
	public class HopkinsModelDowloader : IHopkinsModelReader
	{
		const string API_URL = "https://api.github.com/repos/CSSEGISandData/COVID-19/commits?path=/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_deaths_global.csv";

		const string FILE_BASE_URL = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/";

		private string _file;

		public HopkinsModelDowloader(string file)
		{
			_file = file;
		}

		public string GetRawModel()
		{
			using (System.Net.WebClient wc = new System.Net.WebClient())
			{
				wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0;Windows NT 5.2; .NET CLR 1.0.3705;)");

				string json = wc.DownloadString(API_URL);

				dynamic commits = JArray.Parse(json);

				DateTime date = commits[0].commit.author.date;

				string fileURL = FILE_BASE_URL + _file;

				string content = wc.DownloadString(fileURL);

				return date.ToString("s") + Environment.NewLine + content;
			}
		}
	}
}
