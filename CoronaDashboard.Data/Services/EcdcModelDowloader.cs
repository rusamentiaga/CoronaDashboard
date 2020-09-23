using Newtonsoft.Json.Linq;
using System;

namespace CoronaDashboard.Data
{
	public class EcdcModelDowloader : IHopkinsModelReader
	{
		const string FILE_URL = "https://opendata.ecdc.europa.eu/covid19/casedistribution/json";

		public string GetRawModel()
		{
			using (System.Net.WebClient wc = new System.Net.WebClient())
			{
				wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0;Windows NT 5.2; .NET CLR 1.0.3705;)");
				string json = wc.DownloadString(FILE_URL);

				string content = wc.DownloadString(FILE_URL);

				return content;
			}
		}
	}
}
