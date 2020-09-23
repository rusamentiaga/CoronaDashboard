using CoronaDashboard.Web.Models;
using System.Collections.Generic;

namespace CoronaDashboard.Web.Services
{
	public interface IViewModelService
	{
		PlotViewModel GetAbsoluteDataViewModel(string metric);
		PlotViewModel GetRelativeViewModel(string metric, string option, int minDeaths);
		PlotViewModel GetGrowthViewModel(string metric);
		MapViewModel GetMapViewModel(string metric, string option);
		PlotViewModel GetRelativeGrowthViewModel(string metric, string option, int minDeathsValue);
		TimelineViewModel GetTimeline(string metric, string option);
	}
}
