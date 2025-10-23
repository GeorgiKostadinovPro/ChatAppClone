namespace ChatAppClone.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using ChatAppClone.Models;
    using ChatAppClone.Common.Pages;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult About() 
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 400)
            {
                return this.View(GeneralPages.Error400);
            }

            if (statusCode == 401)
            {
                return this.View(GeneralPages.Error401);
            }

            if (statusCode == 403)
            {
                return this.View(GeneralPages.Error403);
            }

            if (statusCode == 404)
            {
                return this.View(GeneralPages.Error404);
            }

            if (statusCode == 500)
            {
                return this.View(GeneralPages.Error500);
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
