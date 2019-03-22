using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WaaS.Presentation.Controllers
{
    public class ScrapeJobEventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
