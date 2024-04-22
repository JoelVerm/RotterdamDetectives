using RotterdamDetectives_LogicInterface;

namespace RotterdamDetectives_Presentation.Models
{
    public class AdminViewModel
    {
        public List<IStation> Stations { get; set; } = [];

        public string? ErrorMessage { get; set; } = null;
    }
}
