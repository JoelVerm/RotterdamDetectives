using RotterdamDetectives_Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using RotterdamDetectives_LogicInterface;
using RotterdamDetectives_Globals;

namespace RotterdamDetectives_Presentation.Controllers
{
    public class PlayerController(ILogic _logic) : Controller
    {
        readonly ILogic logic = _logic;

        private void AddError(string message)
        {
            var errorMessage = Request.Cookies["errorMessage"] ?? "";
            if (errorMessage == "")
                errorMessage = message;
            else
                errorMessage += "; " + message;
            Response.Cookies.Append("errorMessage", errorMessage);
        }
        private void Log(Result res)
        {
            if (!res.IsOk)
                AddError(res.Error!);
        }
        private string? GetError()
        {
            string? errorMessage = Request.Cookies["errorMessage"];
            Response.Cookies.Delete("errorMessage");
            return errorMessage;
        }

        public IActionResult Index()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            var playerModel = new PlayerViewModel();
            var player = logic.GetPlayer(Request.Cookies["username"]!);
            playerModel.OwnStation = player?.CurrentStation?.Name ?? "";
            playerModel.ConnectedStations = player?.CurrentStation?.Connections ?? [];
            playerModel.Stations = logic.GetStationsWithPlayers(Request.Cookies["username"]!);
            playerModel.TicketAmounts = player?.Tickets.GroupBy(t => t.ModeOfTransport).Select(g => new TicketAmount(g.Key, g.Count())).ToList() ?? [];
            playerModel.TicketHistory = player?.TicketHistory ?? [];
            playerModel.ErrorMessage = GetError();
            return View(playerModel);
        }

        public IActionResult MoveStation(string station, string transportType)
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            var modeOfTransport = transportType.ToLower() switch
            {
                "train" => ModeOfTransport.Train,
                "metro" => ModeOfTransport.Metro,
                "tram" => ModeOfTransport.Tram,
                "bus" => ModeOfTransport.Bus,
                "walking" => ModeOfTransport.Walking,
                _ => ModeOfTransport.Tram
            };
            Log(logic.MovePlayerToStation(Request.Cookies["username"]!, station, modeOfTransport));
            return RedirectToAction("Index");
        }

        public IActionResult Game()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            var model = new GameViewModel();
            var player = logic.GetPlayer(Request.Cookies["username"]!);
            if (player == null)
                return RedirectToAction("Login", "Home");
            model.GameMaster = player.Game?.GameMaster.Name;
            model.Players = player.Game?.Players.Select(p => p.Name).ToList() ?? [];
            model.IsStarted = player.Game?.IsStarted ?? false;
            model.IsGameMaster = player.Game?.GameMaster == player;
            model.ErrorMessage = GetError();
            return View(model);
        }

        public IActionResult CreateGame()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            Log(logic.CreateGame(Request.Cookies["username"]!));
            return RedirectToAction("Game");
        }

        [HttpPost]
        public IActionResult JoinGame(string gameMasterName)
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            Log(logic.JoinGame(Request.Cookies["username"]!, gameMasterName));
            return RedirectToAction("Game");
        }

        public IActionResult LeaveGame()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            Log(logic.LeaveGame(Request.Cookies["username"]!));
            return RedirectToAction("Game");
        }

        public IActionResult StartGame()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            Log(logic.StartGame(Request.Cookies["username"]!));
            return RedirectToAction("Game");
        }

        public IActionResult EndGame()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            Log(logic.EndGame(Request.Cookies["username"]!));
            return RedirectToAction("Game");
        }

        bool LoggedIn()
        {
            if (Request.Cookies["username"] == null || Request.Cookies["password"] == null)
                return false;
            var result = logic.LoginPlayer(Request.Cookies["username"] ?? "", Request.Cookies["password"] ?? "");
            if (!result.IsOk)
                AddError(result.Error!);
            return result.IsOk;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
