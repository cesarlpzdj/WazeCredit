using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WazeCredit.Models;
using WazeCredit.Models.ViewModels;
using WazeCredit.Service;

namespace WazeCredit.Controllers;

public class LifeTimeController : Controller
{
    private readonly TransientService _transientService;
    private readonly ScopedService _scopedService;
    private readonly SingletonService _singletonService;

    public LifeTimeController(TransientService transientService,
        ScopedService scopedService, SingletonService singletonService)
    {
        _transientService = transientService;
        _scopedService = scopedService;
        _singletonService = singletonService;
    }

    public IActionResult Index()
    {
        var messages = new List<string>
        {
            HttpContext.Items["CustomMiddlewareTransient"]?.ToString() ?? "CustomMiddlewareTransient is null",
            $"Transient Controller - {_transientService.GetGuid()}",
            HttpContext.Items["CustomMiddlewareScoped"]?.ToString() ?? "CustomMiddlewareScoped is null",
            $"Scoped Controller - {_scopedService.GetGuid()}",
            HttpContext.Items["CustomMiddlewareSingleton"]?.ToString() ?? "CustomMiddlewareSingleton is null",
            $"Singleton Controller - {_singletonService.GetGuid()}"
        };

        return View(messages);
    }
}