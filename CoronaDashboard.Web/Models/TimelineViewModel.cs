using System.Collections.Generic;

namespace CoronaDashboard.Web.Models
{
	public class TimelineViewModel
	{
		public ICollection<PeakDeaths> Data { get; set; }
		public ICollection<CountryValue> MaxDeathsRelative { get; set; }
		public ICollection<CountryValue> MaxDeathsAbsolute { get; set; }
	}
}
