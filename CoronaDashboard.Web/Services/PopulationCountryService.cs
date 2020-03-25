using CoronaDashboard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaDashboard.Web.Services
{
	public class PopulationCountryService: IPopulationCountryService
	{
		ICollection<PopulationCountry> _countries;
		Dictionary<string, string> _alternativesNames;

		public PopulationCountryService(IPopulationCountryRepository repository)
		{
			_countries = repository.GetPopulationCountry();

			_alternativesNames = new Dictionary<string, string>();
			_alternativesNames["Iran"] = "Iran, Islamic Rep.";
			_alternativesNames["US"] = "United States";
			_alternativesNames["Korea, South"] = "Korea, Rep.";
			_alternativesNames["Egypt"] = "Egypt, Arab Rep.";
		}

		public PopulationCountry GetCountry(string name)
		{
			var query = from country in _countries
						where (country.Name == name)
						select country;

			var popArray = query.ToArray();

			if (popArray.Length == 0)
			{
				if (_alternativesNames.ContainsKey(name))
					return GetCountry(_alternativesNames[name]);
				else
					return new PopulationCountry { Name = name, Population = 1, DensityPeoplePerSquareKm = 1 };
			}

			return popArray.First();
		}
	}
}
