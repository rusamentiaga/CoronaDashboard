using System.IO;

namespace CoronaDashboard.Data
{
	public class HopkinsModelFileReader : IHopkinsModelReader
	{
		string _filename;

		public HopkinsModelFileReader(string filename)
		{
			_filename = filename;
		}

		public string GetRawModel()
		{
			return File.ReadAllText(_filename);
		}
	}
}
