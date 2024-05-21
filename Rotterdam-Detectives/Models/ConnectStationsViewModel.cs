namespace RotterdamDetectives_Presentation.Models
{
    public class ConnectStationsViewModel
    {
        public string StationName { get; set; } = "";
        public List<string> AllStations { get; set; } = [];
        public IReadOnlyList<string> ModesOfTransport = [];
    }
}
