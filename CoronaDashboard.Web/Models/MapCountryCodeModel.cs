using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaDashboard.Web.Models
{
	public class MapCountryCodeModel
	{
		public string code3  { get; set; }
		public double value { get; set; }
	}

	public class MapViewModel
	{
		public DateTime UpdateTime { get; set; }
		public ICollection<MapCountryCodeModel> Data { get; set; }
	}

}
