using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaDashboard.Data
{
	public class CasesModelReader : HopkinsModelReader
	{
		const string file_cases = "time_series_covid19_confirmed_global.csv";

		public CasesModelReader() : base(file_cases) { }
	}

	public class RecoveredModelReader : HopkinsModelReader
	{
		const string file_cases = "time_series_covid19_recovered_global.csv";

		public RecoveredModelReader() : base(file_cases) { }
	}

}
