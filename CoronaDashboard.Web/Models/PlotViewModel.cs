using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaDashboard.Web.Models
{
	public class PlotViewModel
	{
		public DateTime UpdateTime { get; set; }
		public ICollection<CountrySerieViewModel> Series { get; set; }
	}
}
