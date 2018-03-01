namespace TaxiLibrary
{
    public class Route
    {
        public int RouteName { get; set; }
        public Coords StartPoint { get; set; }
        public Coords EndPoint { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int DiffTime => EndTime - StartTime;
        public int RoutDistance => RouteHelper.RouteDistance(StartPoint, EndPoint);
    }
}