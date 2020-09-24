namespace CoronaDashboard.Data
{
	public class HopkinsModelReader : IHopkinsModelReader
	{
		IHopkinsModelReader _reader;

		public HopkinsModelReader(string filename)
		{
			_reader = new HopkinsModelFileCacheReader(new HopkinsModelDowloader(filename), filename);
		}

		string IHopkinsModelReader.GetRawModel()
		{
			return _reader.GetRawModel();
		}
	}
}
