using System;

namespace TaxiLibrary
{
    public static class RouteHelper
    {
        public static int RouteDistance(Coords start, Coords end)
        {
            return Math.Abs(start.NumRow - end.NumRow) + Math.Abs(start.NumColumn - end.NumColumn);
        }
    }
}