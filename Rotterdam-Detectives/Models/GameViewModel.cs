using RotterdamDetectives_LogicInterface;

namespace RotterdamDetectives_Presentation.Models
{
    public class GameViewModel
    {
        public string? GameMaster { get; set; } = null;
        public List<string> Players { get; set; } = [];
        public bool IsStarted { get; set; } = false;
        public bool IsGameMaster { get; set; } = false;

        public string? ErrorMessage { get; set; } = null;
    }
}
