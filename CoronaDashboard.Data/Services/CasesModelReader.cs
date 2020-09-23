using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaDashboard.Data
{
	public class CasesModelReader : IHopkinsModelReader
	{
		const string file_cases = "time_series_covid19_confirmed_global.csv";
		IHopkinsModelReader _reader = new HopkinsModelFileCacheReader(new HopkinsModelDowloader(file_cases), file_cases);

		string IHopkinsModelReader.GetRawModel()
		{
			return _reader.GetRawModel();
		}
	}
}
