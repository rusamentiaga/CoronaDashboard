using System.Collections.Generic;

namespace CoronaDashboard.Web.Models
{
	public class TimelineViewModel
	{
		public ICollection<PeakDeaths> Data { get; set; }
	}
}
