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
		IViewModelService _covidService;

		public HomeController(IViewModelService covidService)
		{
			_covidService = covidService;
		}

		public IActionResult Timeline(string metric, string option)
		{
			TimelineViewModel model = _covidService.GetTimeline(metric, option);

			ViewData["Metric"] = metric;
			ViewData["Method"] = System.Reflection.MethodBase.GetCurrentMethod().Name;

			return View(model);
		}

		public IActionResult Map(string metric, string option)
		{
			MapViewModel model = _covidService.GetMapViewModel(metric, option);

			ViewData["Metric"] = metric;
			ViewData["Method"] = System.Reflection.MethodBase.GetCurrentMethod().Name;

			return View(model);
		}

		public IActionResult Growth(string metric, string option)
		{
			PlotViewModel model = _covidService.GetGrowthViewModel(metric);

			/*
			List<CountrySerieViewModel> series = new List<CountrySerieViewModel>();
			double[] kernel = new double[] { 1, 1, 1, 1, 1, };
			foreach (CountrySerieViewModel seriesItem in model.Series)
			{
				var itemFiltered = new CountrySerieViewModel
				{
					name = seriesItem.name,
					data = Convolve(seriesItem.data.ToArray(), kernel).ToList()
				};
				series.Add(itemFiltered);
			}
			model = new PlotViewModel
			{
				Series = series,
				UpdateTime = model.UpdateTime,
				SeriesLast = model.SeriesLast
			};
			*/

			ViewData["Metric"] = metric;
			ViewData["Method"] = System.Reflection.MethodBase.GetCurrentMethod().Name;
			ViewData["Title"] = "Growth";
			ViewData["YLabel"] = metric + " per day";
			ViewData["TooltipDecimals"] = 0;
			ViewData["YMin"] = 0.1;
			ViewData["YType"] = "linear";
			ViewData["YTickInterval"] = 100;
			ViewData["XLabel"] = "Days from 10 " + metric;

			return View("Plot", model);
		}

		public IActionResult GrowthRelative(string metric, string option)
		{
			PlotViewModel model = _covidService.GetRelativeGrowthViewModel(metric, option,
				ViewModelService.MIN_DEATHS_MILLION);

			ViewData["Metric"] = metric;
			ViewData["Method"] = System.Reflection.MethodBase.GetCurrentMethod().Name;
			ViewData["Title"] = "Growth relative";
			ViewData["YLabel"] = metric + " per day / million";
			ViewData["TooltipDecimals"] = 1;
			ViewData["YMin"] = 0;
			ViewData["YType"] = "linear";
			ViewData["YTickInterval"] = 10;
			ViewData["XLabel"] = "Days from 10 " + metric;

			return View("Plot", model);
		}

		public double[] Convolve(double[] a, double[] kernel)
		{
			double[] result;
			int m = (int)System.Math.Ceiling(kernel.Length / 2.0);

			result = new double[a.Length];

			for (int i = 0; i < result.Length; i++)
			{
				result[i] = 0;
				for (int j = 0; j < kernel.Length; j++)
				{
					int k = i - j + m - 1;
					if (k >= 0 && k < a.Length)
						result[i] += a[k] * kernel[j];
				}
			}

			return result;
		}


		public IActionResult Absolute(string metric, string option)
		{
			PlotViewModel model = _covidService.GetAbsoluteDataViewModel(metric);

			ViewData["Metric"] = metric;
			ViewData["Method"] = System.Reflection.MethodBase.GetCurrentMethod().Name;
			ViewData["Title"] = "Absolute";
			ViewData["YLabel"] = metric;
			ViewData["TooltipDecimals"] = 0;
			ViewData["YMin"] = ViewModelService.MIN_DEATHS;
			ViewData["YType"] = option;
			ViewData["XLabel"] = "Days from 10 " + metric;

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

		public IActionResult Relative(string metric, string option = ViewModelService.NORM_POPULATION)
		{
			PlotViewModel model = _covidService.GetRelativeViewModel(metric, option, ViewModelService.MIN_DEATHS_MILLION);

			ViewData["Metric"] = metric;
			ViewData["Method"] = System.Reflection.MethodBase.GetCurrentMethod().Name;
			ViewData["Title"] = "Relative";
			ViewData["YLabel"] = metric + " / " + option;
			ViewData["TooltipDecimals"] = 2;
			ViewData["YMin"] = 1;
			ViewData["YType"] = "linear";
			ViewData["YTickInterval"] = 1;
			ViewData["XLabel"] = $"Days from 1 {metric} / million";

			return View("Plot", model);
		}
	}
}
