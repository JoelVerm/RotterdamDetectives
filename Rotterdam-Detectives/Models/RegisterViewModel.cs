namespace RotterdamDetectives_Presentation.Models
{
    public class RegisterViewModel
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
        public string? Error { get; set; }

        public RegisterViewModel(string? error = null)
        {
            Error = error;
        }

        public RegisterViewModel() { }
    }
}
