using RotterdamDetectives_LogicInterface;

namespace RotterdamDetectives_Presentation.Models
{
    public class PlayerViewModel
    {
        public string OwnStation { get; set; } = "";
        public IReadOnlyList<IConnection> ConnectedStations { get; set; } = [];
        public IReadOnlyList<IStationWithPlayers> Stations { get; set; } = [];
        public required IReadOnlyDictionary<string, int> TicketAmounts { get; set; }
        public IReadOnlyList<string> TicketHistory { get; set; } = [];
        public bool IsMrX { get; set; } = false;
        public string? ErrorMessage { get; set; } = null;
    }
}
