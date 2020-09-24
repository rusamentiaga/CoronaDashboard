using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace CoronaDashboard.Data.Tests
{
	[TestClass]
	public class Covid19DeathsModelFileReaderTests
	{
		[TestMethod]
		public void TestReader()
		{
			const string filename = "time_series_covid19_deaths_global.csv";

			HopkinsModelFileCacheReader reader = new HopkinsModelFileCacheReader(
				new HopkinsModelDowloader(filename), filename);

			string data = reader.GetRawModel();
			Assert.AreNotEqual(data, string.Empty);
		}

		[TestMethod]
		public void TestCache()
		{
			const string filename = "time_series_covid19_deaths_global.csv";

			HopkinsModelFileCacheReader reader = new HopkinsModelFileCacheReader(
				new HopkinsModelDowloader(filename), filename);

			string data = reader.GetRawModel();
			Assert.IsTrue(File.Exists(filename));
		}

		[TestMethod]
		public void TestCacheClean()
		{
			const string filename = "time_series_covid19_deaths_global.csv";

			HopkinsModelFileCacheReader reader = new HopkinsModelFileCacheReader(
				new HopkinsModelDowloader(filename), filename);

			string data = reader.GetRawModel();
			reader.CacheInvalidate();
			Assert.IsFalse(File.Exists(filename));
		}

		[TestMethod]
		public void TestCacheHit()
		{
			const string filename = "time_series_covid19_deaths_global.csv";

			HopkinsModelFileCacheReader reader = new HopkinsModelFileCacheReader(
				new HopkinsModelDowloader(filename), filename);

			reader.CacheInvalidate();
			string data = reader.GetRawModel();
			Assert.IsFalse(reader.CacheHit);

			data = reader.GetRawModel();
			Assert.IsTrue(reader.CacheHit);

			reader.CacheInvalidate();
			data = reader.GetRawModel();
			Assert.IsFalse(reader.CacheHit);
		}

		[TestMethod]
		public void TestCacheExpire()
		{
			const string filename = "time_series_covid19_deaths_global.csv";

			HopkinsModelFileCacheReader reader = new HopkinsModelFileCacheReader(
				new HopkinsModelDowloader(filename), filename);

			reader.CacheInvalidate();
			string data = reader.GetRawModel();
			Assert.IsFalse(reader.CacheHit);

			File.SetLastWriteTime(filename, DateTime.Now);
			data = reader.GetRawModel();
			Assert.IsTrue(reader.CacheHit);

			File.SetLastWriteTime(filename, DateTime.Now.AddHours(-HopkinsModelFileCacheReader.EXPIRES_HOURS - 1));
			data = reader.GetRawModel();
			Assert.IsFalse(reader.CacheHit);
		}
	}
}

