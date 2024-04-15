using RotterdamDetectives_Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using RotterdamDetectives_LogicInterface;

namespace RotterdamDetectives_Presentation.Controllers
{
    public class AdminController : Controller
    {
        IAdminController admin;

        public AdminController(ILogic _logic) {
            admin = _logic.AdminController;
        }

        public IActionResult Index()
        {
            if (Request.Query["Password"] == "TestAdmin")
                Response.Cookies.Append("Password", "TestAdmin");
            else if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            if (Request.Query.ContainsKey("Logout"))
                Response.Cookies.Delete("Password");

            AdminViewModel model = new AdminViewModel();
            model.Stations = admin.GetStations();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddStation(string addStation)
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            admin.AddStation(addStation);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteStation()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            var deleteStation = Request.Query["Name"];
            if (!string.IsNullOrEmpty(deleteStation))
                admin.DeleteStation(deleteStation!);
            return RedirectToAction("Index");
        }

        public IActionResult ConnectStations()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            var connectStation = Request.Query["Name"].ToString();
            if (!string.IsNullOrEmpty(connectStation))
            {
                Response.Cookies.Append("ConnectStation", connectStation!);
                var model = new ConnectStationsViewModel();
                model.StationName = connectStation;
                model.AllStations = admin.GetStations().Select(e => e.Name).ToList();
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ConnectStations(string to, string transportMode)
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            var from = Request.Cookies["ConnectStation"];
            Response.Cookies.Delete("ConnectStation");
            if (!string.IsNullOrEmpty(from))
                admin.ConnectStations(from, to, transportMode);
            return RedirectToAction("Index");
        }

        public IActionResult DisconnectStations()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            var from = Request.Query["From"];
            var to = Request.Query["To"];
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
                admin.DisconnectStations(from!, to!);
            return RedirectToAction("Index");
        }

        bool LoggedIn()
        {
            return Request.Cookies["Password"] == "TestAdmin";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
