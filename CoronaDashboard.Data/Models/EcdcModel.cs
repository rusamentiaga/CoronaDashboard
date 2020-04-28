using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaDashboard.Data
{
	public class EcdcRecord
	{
		public string dateRep { get; set; }
		public string day { get; set; }
		public string month { get; set; }
		public string year { get; set; }
		public string cases { get; set; }
		public string deaths { get; set; }
		public string countriesAndTerritories { get; set; }
		public string geoId { get; set; }
		public string countryterritoryCode { get; set; }
		public string popData2018 { get; set; }
		public string continentExp { get; set; }
	}

	public class EcdcModel
	{
		public List<EcdcRecord> records { get; set; }
	}
}
