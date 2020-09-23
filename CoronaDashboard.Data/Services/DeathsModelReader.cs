namespace CoronaDashboard.Data
{
	public class DeathsModelReader : IHopkinsModelReader
	{
		const string file_deaths = "time_series_covid19_deaths_global.csv";
		IHopkinsModelReader _reader = new HopkinsModelFileCacheReader(new HopkinsModelDowloader(file_deaths), file_deaths);

		string IHopkinsModelReader.GetRawModel()
		{
			return _reader.GetRawModel();
		}
	}

}
