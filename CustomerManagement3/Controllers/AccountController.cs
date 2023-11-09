using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

public class AccountController : Controller
{
    private readonly IMemoryCache _memoryCache;

    public AccountController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public IActionResult Register()
    {
      
        return View();
    }

    [HttpPost]
    public IActionResult Register(Customer customer)
    {
        
        AddCustomerToCache(customer);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        ViewData["Title"] = "Giriş";
        return View();
    }

    [HttpPost]
    public IActionResult Login(Customer user)
    {
       
        if (IsAuthenticated(user))
        {
            TempData["IsAuthenticated"] = true;

            return RedirectToAction("Index", "Customer");
        }
        else
        {
            
            ModelState.AddModelError(string.Empty, "Geçersiz e-posta veya kimlik");
            return View();
        }
    }
    private bool IsAuthenticated(Customer user)
    {
        
        var cachedCustomers = _memoryCache.Get<List<Customer>>("Customers");

        if (cachedCustomers != null)
        {
           
            return cachedCustomers.Any(u => u.Email == user.Email && u.Identity == user.Identity);
        }

        return false;
    }

    
    private void AddCustomerToCache(Customer customer)
    {
       
        var cachedCustomers = _memoryCache.Get<List<Customer>>("Customers") ?? new List<Customer>();

       
        cachedCustomers.Add(customer);

        
        _memoryCache.Set("Customers", cachedCustomers);
    }
}
