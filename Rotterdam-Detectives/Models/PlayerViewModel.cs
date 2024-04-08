using RotterdamDetectives_LogicInterface;

namespace RotterdamDetectives_Presentation.Models
{
    public class PlayerViewModel
    {
        public string OwnStation { get; set; }
        public List<IStationConnection> ConnectedStations { get; set; }
        public List<IStation> Stations { get; set; }
        public string? ErrorMessage { get; set; } = null;
    }
}
