using System.Collections.Generic;

namespace TaxiLibrary
{
    public class Game
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int CountTaxi { get; set; }
        public int CountRides { get; set; }
        public int Bonus { get; set; }
        public int SimulationSteps { get; set; }
        
        public List<Taxi> Cars { get; set; }
        public List<Route> Routes { get; set; }
        public List<Route> AvailableRoutes { get; set; }
    }
}