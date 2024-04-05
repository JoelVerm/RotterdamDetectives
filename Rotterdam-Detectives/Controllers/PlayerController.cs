using RotterdamDetectives_Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using RotterdamDetectives_LogicInterface;

namespace RotterdamDetectives_Presentation.Controllers
{
    public class PlayerController : Controller
    {
        IProcessedDataSource dataSource;

        public PlayerController(IProcessedDataSource _dataSource) {
            dataSource = _dataSource;
        }

        public IActionResult Index()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            var playerModel = new PlayerViewModel();
            playerModel.OwnStation = dataSource.GetStationByPlayer(Request.Cookies["username"]!);
            playerModel.GameMaster = dataSource.GetGameMasterByPlayer(Request.Cookies["username"]!);
            playerModel.Stations = dataSource.GetStationsAndPlayers(Request.Cookies["username"]!);
            return View(playerModel);
        }

        public IActionResult MoveStation(string station)
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            dataSource.MovePlayerToStation(Request.Cookies["username"]!, station);
            return RedirectToAction("Index");
        }

        bool LoggedIn()
        {
            if (Request.Cookies["username"] == null || Request.Cookies["password"] == null)
                return false;
            return dataSource.LoginUser(Request.Cookies["username"] ?? "", Request.Cookies["password"] ?? "");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
