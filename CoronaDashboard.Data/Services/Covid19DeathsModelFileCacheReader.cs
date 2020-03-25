using System;
using System.IO;

namespace CoronaDashboard.Data
{
	public class Covid19DeathsModelFileCacheReader : ICovid19DeathsModelReader
	{
		public const string CACHE_FILE = "time_series_19-covid-Deaths.csv";
		public const int EXPIRES_HOURS = 4;

		ICovid19DeathsModelReader _reader = new Covid19DeathsModelDowloader();
		public bool CacheHit { private set; get; }

		public Covid19DeathsModelFileCacheReader(ICovid19DeathsModelReader reader)
		{
			_reader = reader;
		}

		public string GetCovid19Deaths()
		{
			string data;

			if (CacheExits())
			{
				data = File.ReadAllText(CACHE_FILE);
				CacheHit = true;
			}
			else
			{
				data = _reader.GetCovid19Deaths();
				CacheWrite(data);
				CacheHit = false;
			}
			return data;
		}

		private static void CacheWrite(string data)
		{
			File.WriteAllText(CACHE_FILE, data);
		}

		private bool CacheExits()
		{
			if (File.Exists(CACHE_FILE))
			{
				TimeSpan age = DateTime.Now - File.GetLastWriteTime(CACHE_FILE);
				if (age.TotalHours < EXPIRES_HOURS)
					return true;
			}
			return false;
		}

		public void CleanCache()
		{
			File.Delete(CACHE_FILE);
		}
	}
}
