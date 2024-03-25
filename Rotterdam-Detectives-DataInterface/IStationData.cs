namespace RotterdamDetectives_DataInterface
{
    public interface IStationData
    {
        public string Name { get; }
        public double Latitude { get; }
        public double Longitude { get; }
        public int MapX { get; }
        public int MapY { get; }
    }
}
