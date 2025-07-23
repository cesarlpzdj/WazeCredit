using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WazeCredit.Models;
using WazeCredit.Models.ViewModels;
using WazeCredit.Service;

namespace WazeCredit.Controllers;

public class HomeController : Controller
{
    public HomeVM HomeVM { get; set; }
    private readonly IMarketForecaster _marketForecaster;

    public HomeController(IMarketForecaster marketForecaster)
    {
        HomeVM = new HomeVM();
        _marketForecaster = marketForecaster;
    }
    public IActionResult Index()
    {
        var currentMarket = _marketForecaster.GetMarketForecast();

        switch (currentMarket.MarketCondition)
        {
            case MarketCondition.StableUp:
                HomeVM.MarketForecast = "The market is stable and trending upwards.";
                break;
            case MarketCondition.StableDown:
                HomeVM.MarketForecast = "The market is stable and trending downwards.";
                break;
            case MarketCondition.Volatile:
                HomeVM.MarketForecast = "The market is volatile, proceed with caution.";
                break;
            default:
                HomeVM.MarketForecast = "Apply for a credit card using our app!";
                break;
        }

        return View(HomeVM);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
