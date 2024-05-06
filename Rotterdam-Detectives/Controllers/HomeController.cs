using RotterdamDetectives_Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using RotterdamDetectives_LogicInterface;

namespace RotterdamDetectives_Presentation.Controllers
{
    public class HomeController(ILogic _logic) : Controller
    {
        readonly ILogic logic = _logic;

        public IActionResult Index()
        {
            if (!LoggedIn())
                return RedirectToAction("Login");
            return RedirectToAction("Index", "Player");
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel)
        {
            if (!logic.LoginPlayer(viewModel.Username, viewModel.Password).IsOk)
                return View(new LoginViewModel("Invalid username or password"));
            Response.Cookies.Append("username", viewModel.Username);
            Response.Cookies.Append("password", viewModel.Password);
            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if (logic.GetPlayer(viewModel.Username) != null)
                return View(new RegisterViewModel("User already exists"));
            if (viewModel.Password != viewModel.ConfirmPassword)
                return View(new RegisterViewModel("Passwords do not match"));
            var result = logic.RegisterPlayer(viewModel.Username, viewModel.Password);
            if (!result.IsOk)
                return View(new RegisterViewModel(result.Error));
            Response.Cookies.Append("username", viewModel.Username);
            Response.Cookies.Append("password", viewModel.Password);
            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("username");
            Response.Cookies.Delete("password");
            return RedirectToAction("Login");
        }

        bool LoggedIn()
        {
            if (Request.Cookies["username"] == null || Request.Cookies["password"] == null)
                return false;
            return logic.LoginPlayer(Request.Cookies["username"] ?? "", Request.Cookies["password"] ?? "").IsOk;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
