using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace CoronaDashboard.Data.Tests
{
	[TestClass]
	public class ICovid19DeathsModelDecoderCsvTests
	{
		[TestMethod]
		public void TestDecoder()
		{
			Covid19DeathsModelFileCacheReader reader = new Covid19DeathsModelFileCacheReader(new Covid19DeathsModelDowloader());
			Covid19DeathsModelRepositoryCsv decoder = new Covid19DeathsModelRepositoryCsv(reader);

			Covid19DeathsModel model = decoder.GetCovid19DeathsModel();
			Assert.AreNotEqual(model.MapCountryDeaths["Spain"], null);
		}

		[TestMethod]
		public void TestData()
		{
			List<int> expected = new List<int>()
			{
				0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,3,5,10,17,28,35,54,55,133,195,289,342,533,623,830,1043,1375,1772
			};
			Covid19DeathsModelFileReader reader = new Covid19DeathsModelFileReader("time_series_19-covid-Deaths_2020-03-23.csv");
			Covid19DeathsModelRepositoryCsv decoder = new Covid19DeathsModelRepositoryCsv(reader);

			Covid19DeathsModel model = decoder.GetCovid19DeathsModel();

			CollectionAssert.AreEqual(model.MapCountryDeaths["Spain"], expected);
		}
	}
}