using RotterdamDetectives_LogicInterface;

namespace RotterdamDetectives_Presentation.Models
{
    public class PlayerViewModel
    {
        public string OwnStation { get; set; }
        public string GameMaster { get; set; }
        public List<IStation> Stations { get; set; }
    }
}
