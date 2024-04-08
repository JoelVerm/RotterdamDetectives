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
            playerModel.ConnectedStations = dataSource.GetConnectedStations(playerModel.OwnStation);
            playerModel.Stations = dataSource.GetStationsAndPlayers(Request.Cookies["username"]!);
            playerModel.ErrorMessage = dataSource.GetLastError();
            return View(playerModel);
        }

        public IActionResult MoveStation(string station)
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            dataSource.MovePlayerToStation(Request.Cookies["username"]!, station);
            return RedirectToAction("Index");
        }

        public IActionResult Game()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            var model = new GameViewModel();
            model.GameMaster = dataSource.GetGameMasterByPlayer(Request.Cookies["username"]!);
            model.Players = dataSource.GetPlayersInGame(Request.Cookies["username"]!);
            model.ErrorMessage = dataSource.GetLastError();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddPlayerToGame(string playerName)
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            dataSource.AddPlayerToGame(Request.Cookies["username"]!, playerName);
            return RedirectToAction("Game");
        }

        public IActionResult LeaveGame()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            dataSource.LeaveGame(Request.Cookies["username"]!);
            return RedirectToAction("Game");
        }

        public IActionResult EndGame()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            dataSource.EndGame(Request.Cookies["username"]!);
            return RedirectToAction("Game");
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
