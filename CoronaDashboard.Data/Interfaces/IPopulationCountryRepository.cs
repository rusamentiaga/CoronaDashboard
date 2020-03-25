using System.Collections.Generic;

namespace CoronaDashboard.Data
{
	public interface IPopulationCountryRepository
	{
		ICollection<PopulationCountry> GetPopulationCountry();
	}
}
