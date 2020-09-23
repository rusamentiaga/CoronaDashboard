using System;
using System.IO;

namespace CoronaDashboard.Data
{
	public class HopkinsModelMemCacheReader : IHopkinsModelCacheReader
	{
		public const int EXPIRES_HOURS = 4;

		IHopkinsModelReader _reader;
		string _data;
		DateTime _updateTime;

		public HopkinsModelMemCacheReader(IHopkinsModelReader reader)
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
			return _reader.GetRawModel();
		}

		protected override string CacheRead()
		{
			return _data;
		}
	}
}
