namespace CoronaDashboard.Data
{
	public class DeathsModelReader : HopkinsModelReader
	{
		const string file_deaths = "time_series_covid19_deaths_global.csv";

		public DeathsModelReader() : base(file_deaths) { }
	}
}
