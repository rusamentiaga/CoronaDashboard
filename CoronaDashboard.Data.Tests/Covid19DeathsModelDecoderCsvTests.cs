using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace CoronaDashboard.Data.Tests
{
	[TestClass]
	public class Covid19DeathsModelDecoderCsvTests
	{
		[TestMethod]
		public void TestDecoder()
		{
			IHopkinsModelReader reader = new DeathsModelReader();
			IEnumerable<IHopkinsModelReader> readers = new List<IHopkinsModelReader> { reader };
			HopkinsModelRepositoryCsv decoder = new HopkinsModelRepositoryCsv(readers);

			HopkinsModel model = decoder.GetHopkinsModel("Deaths");
			Assert.AreNotEqual(model.MapCountryDeaths["Spain"], null);
		}

		[TestMethod]
		public void TestData()
		{
			List<int> expected = new List<int>()
			{
				0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,3,5,10,17,28,35,54,55,133,195,289,342,533,623,830,1043,1375,1772
			};
			HopkinsModelFileReader reader = new HopkinsModelFileReader("time_series_19-covid-Deaths_2020-03-23.csv");

			IEnumerable<IHopkinsModelReader> readers = new List<IHopkinsModelReader> { reader };
			HopkinsModelRepositoryCsv decoder = new HopkinsModelRepositoryCsv(readers);

			HopkinsModel model = decoder.GetHopkinsModel("HopkinsModelFileReader");

			CollectionAssert.AreEqual(model.MapCountryDeaths["Spain"], expected);
		}
	}
}