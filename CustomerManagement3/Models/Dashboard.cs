using Microsoft.AspNetCore.Mvc;

public class DashboardController : Controller
{
    
    public IActionResult Index()
    {
       
        if (User.Identity.IsAuthenticated)
        {
            return View();
        }
        else
        {
            return RedirectToAction("Login", "Account");
        }
    }
}
