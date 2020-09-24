using System;
using System.IO;

namespace CoronaDashboard.Data
{
	public class HopkinsModelFileCacheReader : IHopkinsModelCacheReader
	{
		public const int EXPIRES_HOURS = 6;

		IHopkinsModelReader _reader;
		string _cacheFile;

		public string CacheFile   
		{
			get { return _cacheFile; }   
		}

		public HopkinsModelFileCacheReader(IHopkinsModelReader reader, string cacheFile)
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
			return _reader.GetRawModel();
		}

		protected override string CacheRead()
		{
			return File.ReadAllText(_cacheFile);
		}
	}
}
