using System.Diagnostics;
using System.Security.Claims;
using App.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimoshStore.Models;
using System.Web;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyApp.Namespace;

namespace SimoshStore.Controllers;

public class HomeController(IHttpClientFactory httpClientFactory) : BaseController
{
    private HttpClient Client => httpClientFactory.CreateClient("Api.Data");

    public IActionResult Index()
    {
        return View();
    }
    public  IActionResult AboutUs()
    {
        return View();
    }
    public async Task<IActionResult> NewsLetter([FromForm] string email)
    {
        var response = await Client.PostAsJsonAsync($"/api/newsletter/subscribe?email={email}", email);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index",$"Email: {email} is successfully subscribed.");
        }
        return RedirectToAction("Index");
    }
    public IActionResult AboutMe()
    {
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }
    [Authorize("Admin")]
    public IActionResult AdminDashboard()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult ContactUs()
    {
        return View(new ContactDTO());
    }
    [HttpPost]
    public async Task<IActionResult> ContactUs(ContactDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }
        var response = await Client.PostAsJsonAsync("/api/create/contactform", dto);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index",$"Thank you {dto.Name} for contacting us.");
        }
        SetErrorMessage("Failed to send message. Please try again later.");
        return View(dto);
    }   
}
