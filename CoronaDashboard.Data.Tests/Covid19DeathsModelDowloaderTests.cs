using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoronaDashboard.Data.Tests
{
	[TestClass]
	public class Covid19DeathsModelDowloaderTests
	{
		[TestMethod]
		public void TestDownload()
		{
			Covid19DeathsModelDowloader downloader = new Covid19DeathsModelDowloader();

			string data = downloader.GetCovid19Deaths();
			Assert.AreNotEqual(data, string.Empty);
		}
	}
}
