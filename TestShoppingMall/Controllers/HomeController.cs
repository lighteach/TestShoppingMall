using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestShoppingMall.Models;

namespace TestShoppingMall.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string category)
        {
            ViewData["category"] = category;
            return View();
        }

    }
}
