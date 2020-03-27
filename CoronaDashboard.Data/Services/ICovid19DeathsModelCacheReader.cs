namespace CoronaDashboard.Data
{
	public abstract class ICovid19DeathsModelCacheReader : ICovid19DeathsModelReader
	{
		public bool CacheHit { private set; get; }

		protected abstract string ReadData();
		protected abstract string CacheRead();
		protected abstract void CacheWrite(string data);
		protected abstract bool CacheExits();
		public abstract void CacheInvalidate();

		public string GetCovid19Deaths()
		{
			string data;

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
			return data;
		}
	}
}
