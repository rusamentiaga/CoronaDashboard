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
			Covid19DeathsModelFileCacheReader reader = new Covid19DeathsModelFileCacheReader(new Covid19DeathsModelDowloader());

			string data = reader.GetCovid19Deaths();
			Assert.AreNotEqual(data, string.Empty);
		}

		[TestMethod]
		public void TestCache()
		{
			Covid19DeathsModelFileCacheReader reader = new Covid19DeathsModelFileCacheReader(new Covid19DeathsModelDowloader());

			string data = reader.GetCovid19Deaths();
			Assert.IsTrue(File.Exists(Covid19DeathsModelFileCacheReader.CACHE_FILE));
		}

		[TestMethod]
		public void TestCacheClean()
		{
			Covid19DeathsModelFileCacheReader reader = new Covid19DeathsModelFileCacheReader(new Covid19DeathsModelDowloader());

			string data = reader.GetCovid19Deaths();
			reader.CacheInvalidate();
			Assert.IsFalse(File.Exists(Covid19DeathsModelFileCacheReader.CACHE_FILE));
		}

		[TestMethod]
		public void TestCacheHit()
		{
			Covid19DeathsModelFileCacheReader reader = new Covid19DeathsModelFileCacheReader(new Covid19DeathsModelDowloader());

			reader.CacheInvalidate();
			string data = reader.GetCovid19Deaths();
			Assert.IsFalse(reader.CacheHit);

			data = reader.GetCovid19Deaths();
			Assert.IsTrue(reader.CacheHit);

			reader.CacheInvalidate();
			data = reader.GetCovid19Deaths();
			Assert.IsFalse(reader.CacheHit);
		}

		[TestMethod]
		public void TestCacheExpire()
		{
			Covid19DeathsModelFileCacheReader reader = new Covid19DeathsModelFileCacheReader(new Covid19DeathsModelDowloader());

			reader.CacheInvalidate();
			string data = reader.GetCovid19Deaths();
			Assert.IsFalse(reader.CacheHit);

			File.SetLastWriteTime(Covid19DeathsModelFileCacheReader.CACHE_FILE, DateTime.Now);
			data = reader.GetCovid19Deaths();
			Assert.IsTrue(reader.CacheHit);

			File.SetLastWriteTime(Covid19DeathsModelFileCacheReader.CACHE_FILE, 
				DateTime.Now.AddHours(-Covid19DeathsModelFileCacheReader.EXPIRES_HOURS - 1));
			data = reader.GetCovid19Deaths();
			Assert.IsFalse(reader.CacheHit);
		}
	}
}

