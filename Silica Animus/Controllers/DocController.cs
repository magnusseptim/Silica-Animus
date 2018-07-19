using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Silica_Animus.Controllers
{
    public class DocController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
