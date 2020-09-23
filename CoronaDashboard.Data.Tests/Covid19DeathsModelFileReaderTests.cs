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
			HopkinsModelFileCacheReader reader = new HopkinsModelFileCacheReader(new HopkinsModelDowloader("time_series_covid19_deaths_global.csv"));

			string data = reader.GetRawModel();
			Assert.AreNotEqual(data, string.Empty);
		}

		[TestMethod]
		public void TestCache()
		{
			HopkinsModelFileCacheReader reader = new HopkinsModelFileCacheReader(new HopkinsModelDowloader("time_series_covid19_deaths_global.csv"));

			string data = reader.GetRawModel();
			Assert.IsTrue(File.Exists(HopkinsModelFileCacheReader.DEFAULT_CACHE_FILE));
		}

		[TestMethod]
		public void TestCacheClean()
		{
			HopkinsModelFileCacheReader reader = new HopkinsModelFileCacheReader(new HopkinsModelDowloader("time_series_covid19_deaths_global.csv"));

			string data = reader.GetRawModel();
			reader.CacheInvalidate();
			Assert.IsFalse(File.Exists(HopkinsModelFileCacheReader.DEFAULT_CACHE_FILE));
		}

		[TestMethod]
		public void TestCacheHit()
		{
			HopkinsModelFileCacheReader reader = new HopkinsModelFileCacheReader(new HopkinsModelDowloader("time_series_covid19_deaths_global.csv"));

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
			HopkinsModelFileCacheReader reader = new HopkinsModelFileCacheReader(new HopkinsModelDowloader("time_series_covid19_deaths_global.csv"));

			reader.CacheInvalidate();
			string data = reader.GetRawModel();
			Assert.IsFalse(reader.CacheHit);

			File.SetLastWriteTime(HopkinsModelFileCacheReader.DEFAULT_CACHE_FILE, DateTime.Now);
			data = reader.GetRawModel();
			Assert.IsTrue(reader.CacheHit);

			File.SetLastWriteTime(HopkinsModelFileCacheReader.DEFAULT_CACHE_FILE, 
				DateTime.Now.AddHours(-HopkinsModelFileCacheReader.EXPIRES_HOURS - 1));
			data = reader.GetRawModel();
			Assert.IsFalse(reader.CacheHit);
		}
	}
}

