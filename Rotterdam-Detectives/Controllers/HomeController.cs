using RotterdamDetectives_Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using RotterdamDetectives_LogicInterface;

namespace RotterdamDetectives_Presentation.Controllers
{
    public class HomeController : Controller
    {
        IProcessedDataSource dataSource;

        public HomeController(IProcessedDataSource _dataSource) {
            dataSource = _dataSource;
        }

        public IActionResult Index()
        {
            if (!LoggedIn())
                return RedirectToAction("Login");
            return View();
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel)
        {
            if (!dataSource.LoginUser(viewModel.Username, viewModel.Password))
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
            if (dataSource.UserExists(viewModel.Username))
                return View(new RegisterViewModel("User already exists"));
            if (viewModel.Password != viewModel.ConfirmPassword)
                return View(new RegisterViewModel("Passwords do not match"));
            dataSource.RegisterUser(viewModel.Username, viewModel.Password);
            Response.Cookies.Append("username", viewModel.Username);
            Response.Cookies.Append("password", viewModel.Password);
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
