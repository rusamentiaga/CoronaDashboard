using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoronaDashboard.Web.Models;
using CoronaDashboard.Data;
using System.Text;
using Newtonsoft.Json;
using CoronaDashboard.Web.Services;

namespace CoronaDashboard.Web.Controllers
{
	public class HomeController : Controller
	{
		ICovid19DeathsService _covidService;

		public HomeController(ICovid19DeathsService covidService)
		{
			_covidService = covidService;
		}

		public IActionResult Growth(string option)
		{
			PlotViewModel model = _covidService.GetGrowthViewModel();

			ViewData["Title"] = "Growth";
			ViewData["YLabel"] = "Increase of deaths";
			ViewData["TooltipDecimals"] = 0;
			ViewData["YMin"] = 0;
			ViewData["YType"] = "linear";
			ViewData["YTickInterval"] = 100;

			return View("Plot", model);
		}

		public IActionResult Absolute(string option)
		{
			PlotViewModel model = _covidService.GetAbsoluteDataViewModel();

			ViewData["Title"] = "Absolute";
			ViewData["YLabel"] = "Deaths";
			ViewData["TooltipDecimals"] = 0;
			ViewData["YMin"] = Covid19DeathsService.MIN_DEATHS;
			ViewData["YType"] = option;

			if (option == "logarithmic")
			{
				ViewData["YTickInterval"] = 1;
			}
			else
			{
				ViewData["YTickInterval"] = 500;
			}

			return View("Plot", model);
		}

		public IActionResult Relative(string option)
		{
			PlotViewModel model = _covidService.GetRelativeViewModel(option);

			ViewData["Title"] = "Relative";
			ViewData["YLabel"] = "Deaths / "+ option;
			ViewData["TooltipDecimals"] = 2;
			ViewData["YMin"] = 0.01;
			ViewData["YType"] = "logarithmic";
			ViewData["YTickInterval"] = 1;

			return View("Plot", model);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
