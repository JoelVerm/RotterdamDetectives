using RotterdamDetectives_LogicInterface;

namespace RotterdamDetectives_Presentation.Models
{
    public class PlayerViewModel
    {
        public string OwnStation { get; set; }
        public IReadOnlyList<IConnection> ConnectedStations { get; set; }
        public IReadOnlyList<IStationWithPlayers> Stations { get; set; }
        public IReadOnlyList<TicketAmount> TicketAmounts { get; set; }
        public string? ErrorMessage { get; set; } = null;
    }
}
