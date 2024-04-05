namespace RotterdamDetectives_Presentation.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string? Error { get; set; }

        public LoginViewModel(string? error = null) {
            Error = error;
        }

        public LoginViewModel() { }
    }
}
