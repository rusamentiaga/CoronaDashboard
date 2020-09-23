using System;
using System.Collections.Generic;

namespace CoronaDashboard.Data
{
	public class HopkinsModel
	{
		public DateTime UpdateTime { get; set; }
		public List<string> Countries { get; set; }
		public List<DateTime> Dates { get; set; }
		public Dictionary<string, List<int>> MapCountryDeaths;

		public Dictionary<string, string> MapCountryIsoCode;

		public string GetCountryIsoCode(string coutryname)
		{
			if (MapCountryIsoCode != null)
				if (MapCountryIsoCode.ContainsKey(coutryname))
					return MapCountryIsoCode[coutryname];

			return String.Empty;
		}
	}
}