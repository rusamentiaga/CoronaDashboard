namespace CoronaDashboard.Data
{
	public interface IHopkinsModelRepository
	{
		HopkinsModel GetHopkinsModel(string metric);
	}
}
