using RotterdamDetectives_Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using RotterdamDetectives_LogicInterface;
using RotterdamDetectives_Globals;

namespace RotterdamDetectives_Presentation.Controllers
{
    public class PlayerController(IPlayer player, IStation station, ITicket ticket, IGame game) : Controller
    {
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
            var userName = Request.Cookies["username"]!;
            var currentStation = player.GetCurrentStation(userName) ?? "";
            var playerModel = new PlayerViewModel {
                OwnStation = currentStation,
                ConnectedStations = station.GetConnectionsOf(currentStation),
                Stations = station.GetWithPlayers(userName),
                TicketAmounts = ticket.GetSpare(userName),
                TicketHistory = ticket.GetHistory(userName).ToList(),
                IsMrX = game.IsMrX(userName),
                ErrorMessage = GetError(),
            };
            return View(playerModel);
        }

        public IActionResult MoveStation(string station, string transportType)
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            Log(player.MoveToStation(Request.Cookies["username"]!, station, transportType));
            return RedirectToAction("Index");
        }

        public IActionResult Game()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            var userName = Request.Cookies["username"]!;
            var model = new GameViewModel
            { GameMaster = game.GameMasterOf(userName) };
            if (model.GameMaster != null)
            {
                model.Players = game.GetPlayers(model.GameMaster).ToList();
                model.PlayerTickets = game.GetPlayerTickets(model.GameMaster);
                model.IsStarted = game.IsStarted(model.GameMaster);
                model.IsGameMaster = game.GameMasterOf(userName) == userName;
            }
            model.ErrorMessage = GetError();
            return View(model);
        }

        public IActionResult CreateGame()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            Log(game.Create(Request.Cookies["username"]!));
            return RedirectToAction("Game");
        }

        [HttpPost]
        public IActionResult JoinGame(string gameMasterName)
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            Log(game.Join(gameMasterName, Request.Cookies["username"]!));
            return RedirectToAction("Game");
        }

        public IActionResult LeaveGame()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            Log(game.Leave(Request.Cookies["username"]!));
            return RedirectToAction("Game");
        }

        public IActionResult StartGame()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            Log(game.Start(Request.Cookies["username"]!));
            return RedirectToAction("Game");
        }

        public IActionResult EndGame()
        {
            if (!LoggedIn())
                return RedirectToAction("Login", "Home");
            Log(game.End(Request.Cookies["username"]!));
            return RedirectToAction("Game");
        }

        bool LoggedIn()
        {
            if (Request.Cookies["username"] == null || Request.Cookies["password"] == null)
                return false;
            return player.Login(Request.Cookies["username"] ?? "", Request.Cookies["password"] ?? "");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
