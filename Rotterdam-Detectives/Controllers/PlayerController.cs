using RotterdamDetectives_Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using RotterdamDetectives_LogicInterface;

namespace RotterdamDetectives_Presentation.Controllers
{
    public class PlayerController : Controller
    {
        ILogic logic;

        public PlayerController(ILogic _logic) {
            logic = _logic;
        }

        private void AddError(string message)
        {
            var errorMessage = Request.Cookies["errorMessage"] ?? "";
            if (errorMessage == "")
                errorMessage = message;
            else
                errorMessage += "; " + message;
            Response.Cookies.Append("errorMessage", errorMessage);
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
            playerModel.ConnectedStations = player?.CurrentStation?.Connections ?? new List<IConnection>();
            playerModel.Stations = logic.GetStationsWithPlayers();
            playerModel.TicketAmounts = player?.Tickets.GroupBy(t => t.ModeOfTransport).Select(g => new TicketAmount(g.Key, g.Count())).ToList() ?? new();
            playerModel.TicketHistory = player?.TicketHistory ?? new List<ITicket>();
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
            var result = logic.MovePlayerToStation(Request.Cookies["username"]!, station, modeOfTransport);
            if (!result.IsOk)
                AddError(result.Error!);
            return RedirectToAction("Index");
        }

        public IActionResult Game()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            var model = new GameViewModel();
            model.GameMaster = logic.GetPlayer(Request.Cookies["username"]!)?.GameMaster?.Name;
            if (model.GameMaster == null)
                model.Players = logic.GetPlayersInGameWith(Request.Cookies["username"]!).Select(p => p.Name).ToList();
            else
                model.Players = logic.GetPlayersInGameWith(model.GameMaster).Select(p => p.Name).ToList();
            model.ErrorMessage = GetError();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddPlayerToGame(string playerName)
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            logic.SetGameMaster(playerName, Request.Cookies["username"]!);
            return RedirectToAction("Game");
        }

        public IActionResult LeaveGame()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            logic.GetPlayer(Request.Cookies["username"]!)?.ResetGameMaster();
            return RedirectToAction("Game");
        }

        public IActionResult StartGame()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            var result = logic.StartGame(Request.Cookies["username"]!);
            if (!result.IsOk)
                AddError(result.Error!);
            return RedirectToAction("Game");
        }

        public IActionResult EndGame()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            logic.EndGame(Request.Cookies["username"]!);
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
