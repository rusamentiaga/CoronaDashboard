namespace CoronaDashboard.Data
{
	public abstract class IHopkinsModelCacheReader : IHopkinsModelReader
	{
		public bool CacheHit { private set; get; }

		protected abstract string ReadData();
		protected abstract string CacheRead();
		protected abstract void CacheWrite(string data);
		protected abstract bool CacheExits();
		public abstract void CacheInvalidate();

		private readonly object cacheLock = new object();

		public string GetRawModel()
		{
			string data;

			lock (cacheLock)
			{
				if (CacheExits())
				{
					data = CacheRead();
					CacheHit = true;
				}
				else
				{
					data = ReadData();
					CacheWrite(data);
					CacheHit = false;
				}
			}

			return data;
		}
	}
}
