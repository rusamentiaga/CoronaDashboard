using System;
using System.Collections.Generic;

namespace CoronaDashboard.Data
{
	public class Covid19DeathsModel
	{
		public DateTime UpdateTime { get; set; }
		public List<string> Countries { get; set; }
		public List<DateTime> Dates { get; set; }
		public Dictionary<string, List<int>> MapCountryDeaths;
	}
}
