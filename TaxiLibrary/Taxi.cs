using System;
using System.Collections.Generic;

namespace TaxiLibrary
{
    public class Taxi
    {
        public int CarNum { get; set; }
        public Coords CurrentCoords { get; set; }
        public Route CurrentRoute { get; set; }
        public List<Route> Routes = new List<Route>();
        public int CountStepsForAvailable { get; set; }
    }
}