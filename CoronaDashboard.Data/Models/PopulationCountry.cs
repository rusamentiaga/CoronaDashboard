using System;
using System.Text;

namespace CoronaDashboard.Data
{
	public class PopulationCountry
	{
		public string Name { get; set; }
		public string IsoCode { get; set; }
		public double Population { get; set; }
		public double DensityPeoplePerSquareKm { get; set; }
	}
}