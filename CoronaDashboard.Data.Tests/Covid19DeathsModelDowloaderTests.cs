using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoronaDashboard.Data.Tests
{
	[TestClass]
	public class Covid19DeathsModelDowloaderTests
	{
		[TestMethod]
		public void TestDownload()
		{
			HopkinsModelDowloader downloader = new HopkinsModelDowloader("time_series_covid19_deaths_global.csv");

			string data = downloader.GetRawModel();
			Assert.AreNotEqual(data, string.Empty);
		}
	}
}
