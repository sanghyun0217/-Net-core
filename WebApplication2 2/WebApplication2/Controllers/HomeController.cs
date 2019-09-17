using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using Microsoft.AspNetCore.Http;
namespace WebApplication2.Controllers {
  public class HomeController : Controller {
    private readonly UserContent _context;

    public HomeController(UserContent context) {
      _context = context;
    }

    public IActionResult Index() {

      return View();
    }
    [HttpPost]
    /**************************************************************************************
       Function Name    : Article
       Description      : login function, write the username to the session
       Caution          : Parameter    :   string username   ->   the user input
                          Return       :   the view to the index page of articles
    **************************************************************************************/
    public IActionResult Article([FromForm]string username) {
      System.Diagnostics.Debug.Write("==================================\n\n");
      System.Diagnostics.Debug.Write(username);
      System.Diagnostics.Debug.Write("==================================\n\n");
      HttpContext.Session.SetString("username", username);

      return RedirectToRoute(new { controller = "Articles", action = "index" });
    }

    public IActionResult Error() {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
