using System;
using System.IO;

namespace CoronaDashboard.Data
{
	public class Covid19DeathsModelMemCacheReader : ICovid19DeathsModelCacheReader
	{
		public const int EXPIRES_HOURS = 4;

		ICovid19DeathsModelReader _reader = new Covid19DeathsModelDowloader();
		string _data;
		DateTime _updateTime;

		public Covid19DeathsModelMemCacheReader(ICovid19DeathsModelReader reader)
		{
			_reader = reader;
			_data = null;
		}

		protected override void CacheWrite(string data)
		{
			_data = data;
			_updateTime = DateTime.Now;
		}

		protected override bool CacheExits()
		{
			if (_data != null)
			{
				TimeSpan age = DateTime.Now - _updateTime;
				if (age.TotalHours < EXPIRES_HOURS)
					return true;
			}
			return false;
		}

		public override void CacheInvalidate()
		{
			_data = null;
		}

		protected override string ReadData()
		{
			return _reader.GetCovid19Deaths();
		}

		protected override string CacheRead()
		{
			return _data;
		}
	}
}
