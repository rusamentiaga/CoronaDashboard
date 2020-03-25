using CoronaDashboard.Data;

namespace CoronaDashboard.Web.Services
{
	public interface IPopulationCountryService
	{
		PopulationCountry GetCountry(string name);
	}
}
