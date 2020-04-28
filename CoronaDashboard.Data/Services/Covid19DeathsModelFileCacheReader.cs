using System;
using System.IO;

namespace CoronaDashboard.Data
{
	public class Covid19DeathsModelFileCacheReader : ICovid19DeathsModelCacheReader
	{
		public const string DEFAULT_CACHE_FILE = "time_series_19-covid-Deaths.csv";
		public const int EXPIRES_HOURS = 6;

		ICovid19DeathsModelReader _reader = new Covid19DeathsModelDowloader();
		string _cacheFile;

		public Covid19DeathsModelFileCacheReader(ICovid19DeathsModelReader reader, string cacheFile = DEFAULT_CACHE_FILE)
		{
			_reader = reader;
			_cacheFile = cacheFile;
		}

		protected override void CacheWrite(string data)
		{
			File.WriteAllText(_cacheFile, data);
		}

		protected override bool CacheExits()
		{
			if (File.Exists(_cacheFile))
			{
				TimeSpan age = DateTime.Now - File.GetLastWriteTime(_cacheFile);
				if (age.TotalHours < EXPIRES_HOURS)
					return true;
			}
			return false;
		}

		public override void CacheInvalidate()
		{
			File.Delete(_cacheFile);
		}

		protected override string ReadData()
		{
			return _reader.GetCovid19Deaths();
		}

		protected override string CacheRead()
		{
			return File.ReadAllText(_cacheFile);
		}
	}
}
