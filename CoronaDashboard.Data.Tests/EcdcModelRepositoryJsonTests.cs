﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace CoronaDashboard.Data.Tests
{
	[TestClass]
	public class EcdcModelRepositoryJsonTests
	{
		/*
		[TestMethod]
		public void TestDecoder()
		{
			Covid19DeathsModelFileCacheReader reader = new Covid19DeathsModelFileCacheReader(new EcdcModelDowloader(),
				"opendata.ecdc.europa.eu-covid19.json");
			EcdcModelRepositoryJson decoder = new EcdcModelRepositoryJson(reader);

			Covid19DeathsModel model = decoder.GetCovid19DeathsModel();
			Assert.AreNotEqual(model.MapCountryDeaths["Spain"], null);

		}
		*/

		[TestMethod]
		public void TestData()
		{
			HopkinsModelFileReader reader = new HopkinsModelFileReader("opendata.ecdc.europa.eu-covid19.json");
			EcdcModelRepositoryJson decoder = new EcdcModelRepositoryJson(reader);

			HopkinsModel model = decoder.GetHopkinsModel();

			var item = model.MapCountryDeaths["Spain"];
			Assert.AreEqual(item[0], 0);
		}
	}
}