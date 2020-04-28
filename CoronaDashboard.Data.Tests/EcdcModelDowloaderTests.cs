using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoronaDashboard.Data.Tests
{
	[TestClass]
	public class EcdcModelDowloaderTests
	{
		[TestMethod]
		public void TestDownload()
		{
			EcdcModelDowloader downloader = new EcdcModelDowloader();

			string data = downloader.GetCovid19Deaths();
			Assert.AreNotEqual(data, string.Empty);
		}
	}
}
