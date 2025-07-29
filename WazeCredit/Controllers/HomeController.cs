using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WazeCredit.Data;
using WazeCredit.Data.Repository;
using WazeCredit.Models;
using WazeCredit.Models.ViewModels;
using WazeCredit.Service;
using WazeCredit.Utilities.AppSettingsClasses;

namespace WazeCredit.Controllers;

public class HomeController : Controller
{
    public HomeVM HomeVM { get; set; }
    private readonly IMarketForecaster _marketForecaster;
    private readonly ICreditValidator _creditValidator;
    private readonly IUnitOfWork _unitOfWork;
    public readonly WazeForecastSettings _wazeOptions;
    private ILogger _logger;

    [BindProperty]
    public CreditApplication? CreditModel { get; set; }

    public HomeController(IMarketForecaster marketForecaster,
        IOptions<WazeForecastSettings> wazeOptions,
        ICreditValidator creditValidator,
        IUnitOfWork unitOfWork,
        ILogger<HomeController> logger)
    {
        HomeVM = new HomeVM();
        _wazeOptions = wazeOptions.Value;
        _marketForecaster = marketForecaster;
        _creditValidator = creditValidator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public IActionResult Index()
    {
        _logger.LogInformation("Home Controller Index Action Called");

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
        _logger.LogInformation("Home Controller Index Action Ended");
        return View(HomeVM);
    }

    public IActionResult AllConfigSettings(
        [FromServices] IOptions<StripeSettings> stripeOptions,
        [FromServices] IOptions<SendGridSettings> sendGridOptions,
        [FromServices] IOptions<TwilioSettings> twilioOptions
    )
    {
        var optionsList = new List<string>
        {
            "Waze Config - Forecast Tracker: " + _wazeOptions.ForecastTrackerEnabled,
            "Stripe Publishable Key: " + stripeOptions.Value.PublishableKey,
            "Stripe Secret Key: " + stripeOptions.Value.SecretKey,
            "Send Grid Key: " + sendGridOptions.Value.SendGridKey, 
            "Twilio Phone: " + twilioOptions.Value.PhoneNumber, 
            "Twilio Auth Token: " + twilioOptions.Value.AuthToken, 
            "Twilio Account Sid: " + twilioOptions.Value.AccountSid, 
        };

        return View(optionsList);
    }

    public IActionResult CreditApplication()
    {
        CreditModel = new CreditApplication();
        return View(CreditModel);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [ActionName("CreditApplication")]
    public async Task<IActionResult> CreditApplicationPost(
        [FromServices] Func<CreditApprovedEnum, ICreditApproved> _creditService)
    {
        if (!ModelState.IsValid)
            return View(CreditModel);

        if (CreditModel == null)
            throw new ArgumentNullException("CreditModel");

        var (validationsPassed, errorMessages) = await _creditValidator.PassAllValidations(CreditModel);
        var creditResult = new CreditResult()
        {
            ErrorList = errorMessages,
            CreditID = 0,
            Success = validationsPassed
        };

        if (validationsPassed)
        {
            const int salaryLowerLimit = 50000;
            var creditServiceSalaryArg = CreditModel.Salary > salaryLowerLimit
                ? CreditApprovedEnum.High
                : CreditApprovedEnum.Low;
            CreditModel.CreditApproved = _creditService(creditServiceSalaryArg)
                .GetCreditApproved(CreditModel);
            _unitOfWork.CreditApplication.Add(CreditModel);
            _unitOfWork.Save();
            creditResult.CreditID = CreditModel.Id;
            creditResult.CreditApproved = CreditModel.CreditApproved;
        }
        return RedirectToAction(nameof(CreditResult), creditResult);
    }

    public IActionResult CreditResult(CreditResult creditResult) => View(creditResult);

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
