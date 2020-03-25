using System.IO;

namespace CoronaDashboard.Data
{
	public class Covid19DeathsModelFileReader : ICovid19DeathsModelReader
	{
		string _filename;

		public Covid19DeathsModelFileReader(string filename)
		{
			_filename = filename;
		}

		public string GetCovid19Deaths()
		{
			return File.ReadAllText(_filename);
		}
	}
}
