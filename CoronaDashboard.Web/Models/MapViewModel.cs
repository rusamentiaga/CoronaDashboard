using System;
using System.Collections.Generic;

namespace CoronaDashboard.Web.Models
{
	public class MapViewModel
	{
		public DateTime UpdateTime { get; set; }
		public ICollection<MapCountryCodeModel> Data { get; set; }
	}
}
