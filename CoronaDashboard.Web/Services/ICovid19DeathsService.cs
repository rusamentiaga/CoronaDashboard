using CoronaDashboard.Web.Models;
using System.Collections.Generic;

namespace CoronaDashboard.Web.Services
{
	public interface ICovid19DeathsService
	{
		PlotViewModel GetAbsoluteDataViewModel();
		PlotViewModel GetRelativeViewModel(string id);
		PlotViewModel GetGrowthViewModel();
	}
}
