using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebUygulamaProje1.Models;

namespace WebUygulamaProje1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            var modelList = new List<PrivacyViewModel>();

            var model=new PrivacyViewModel();
            model.Id = 10;
            model.Name = "a";

            modelList.Add(model);

            model = new PrivacyViewModel();
            model.Id = 15;
            model.Name = "b";

            modelList.Add(model);

            return View(modelList);  
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
