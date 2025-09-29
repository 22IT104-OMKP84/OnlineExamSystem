using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnlineExamSystem.Models;
using System.Security.Claims;

namespace OnlineExamSystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // If user is authenticated, redirect to appropriate dashboard
        if (User.Identity?.IsAuthenticated == true)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return role switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Instructor" => RedirectToAction("Index", "Instructor"),
                "Student" => RedirectToAction("Index", "Student"),
                _ => RedirectToAction("Index", "Login")
            };
        }

        // If not authenticated, redirect to login
        return RedirectToAction("Index", "Login");
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
