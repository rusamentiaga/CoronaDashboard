﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoronaDashboard.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoronaDashboard.Web.Controllers
{
    public class ErrorController : Controller
    {
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Index()
        {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}