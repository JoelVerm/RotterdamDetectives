using RotterdamDetectives_LogicInterface;

namespace RotterdamDetectives_Presentation.Models
{
    public class GameViewModel
    {
        public string? GameMaster { get; set; } = null;
        public List<string> Players { get; set; } = new();

        public string? ErrorMessage { get; set; } = null;
    }
}
