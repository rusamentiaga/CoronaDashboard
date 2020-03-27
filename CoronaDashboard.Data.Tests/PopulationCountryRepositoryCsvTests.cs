using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace CoronaDashboard.Data.Tests
{
	[TestClass]
	public class PopulationCountryRepositoryCsvTests
	{
		[TestMethod]
		public void TestRead()
		{
			PopulationCountryRepositoryCsv repository = new PopulationCountryRepositoryCsv();

			IEnumerable<PopulationCountry> data = repository.GetPopulationCountry();
			Assert.AreNotEqual(data, null);
		}

		[TestMethod]
		public void TestReadData()
		{
			const double TOLERANCE = 1e-6;

			PopulationCountryRepositoryCsv repository = new PopulationCountryRepositoryCsv();

			IEnumerable<PopulationCountry> data = repository.GetPopulationCountry();

			var country = data.First(m => m.Name == "Spain");

			Assert.AreEqual((int)country.Population, 46723749);
			Assert.AreNotEqual(93.5290582615872, 46723749, TOLERANCE);
		}

		[TestMethod]
		public void TestCountryCode()
		{
			PopulationCountryRepositoryCsv repository = new PopulationCountryRepositoryCsv();

			IEnumerable<PopulationCountry> data = repository.GetPopulationCountry();

			var country = data.First(m => m.Name == "Spain");

			Assert.AreEqual(country.IsoCode, "ESP");
		}
	}
}
