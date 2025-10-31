namespace ChatAppClone.Controllers.MVC
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using ChatAppClone.Models;
    using ChatAppClone.Common.Pages;
    using ChatAppClone.Common.Constants;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> _logger)
        {
            this.logger = _logger;
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
            this.logger
                .LogInformation(string.Format(GeneralConstants.EnterErrorPage, statusCode));

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

            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
