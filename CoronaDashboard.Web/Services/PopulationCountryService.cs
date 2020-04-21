using CoronaDashboard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaDashboard.Web.Services
{
	public class PopulationCountryService : IPopulationCountryService
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
			_alternativesNames["Russia"] = "Russian Federation";
			_alternativesNames["Bahamas"] = "Bahamas, The";
			_alternativesNames["Gambia"] = "Gambia, The";
			_alternativesNames["Brunei"] = "Brunei Darussalam";
			_alternativesNames["Czechia"] = "Czech Republic";
			_alternativesNames["Kyrgyzstan"] = "Kyrgyz Republic";
			_alternativesNames["Laos"] = "Lao PDR";
			_alternativesNames["Slovakia"] = "Slovak Republic";
			_alternativesNames["Syria"] = "Syrian Arab Republic";
			_alternativesNames["Venezuela"] = "Venezuela, RB";
			_alternativesNames["Yemen"] = "Yemen, Rep.";
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
					return null;
			}

			return popArray.First();
		}
	}
}
