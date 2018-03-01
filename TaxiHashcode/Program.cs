using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TaxiLibrary;

namespace TaxiHashcode
{
    class Program
    {
        static void Main(string[] args)
        {
            var beginCoord = new Coords {NumColumn = 0, NumRow = 0};
            var game = DataHelper.LoadGameFromFile("Data/e_high_bonus.in");
            Console.WriteLine($"Cars = {game.CountTaxi} Bonus = {game.Bonus} Steps = {game.SimulationSteps}");
            //1.Filtering 
            game.AvailableRoutes = game.AvailableRoutes.Where(r => r.DiffTime >= r.RoutDistance).ToList();
            //2.
            for (int step = 0; step < game.SimulationSteps; step++)
            {
                foreach (var car in game.Cars)
                {
                    if (car.CountStepsForAvailable <= step)
                    {
                        if (car.CurrentRoute != null){
                            car.Routes.Add(car.CurrentRoute);
                        }
                        
                        if (game.AvailableRoutes.Any())
                        {
                            car.CurrentRoute = OptimalRoute(car, game.AvailableRoutes);
                            car.CountStepsForAvailable =
                                RouteHelper.RouteDistance(car.CurrentCoords, car.CurrentRoute.StartPoint) + step <
                                car.CurrentRoute.StartTime
                                    ? car.CurrentRoute.StartTime + car.CurrentRoute.RoutDistance
                                    : RouteHelper.RouteDistance(car.CurrentCoords, car.CurrentRoute.StartPoint) + step +
                                      car.CurrentRoute.RoutDistance;

                            game.AvailableRoutes.Remove(car.CurrentRoute);
                        }
                        else
                        {
                            car.CurrentRoute = null;
                        }
                        //Console.WriteLine(step);
                    }
                }
            }
            
            DataHelper.SaveToFile(game.Cars,"e_high_bonus.out");
        }

        private static Route OptimalRoute(Taxi taxi, List<Route> routes)
        {
            var route = routes.Aggregate((curr, dest) =>
            {
                var currDist = RouteHelper.RouteDistance(taxi.CurrentCoords, curr.StartPoint);
                var destDist = RouteHelper.RouteDistance(taxi.CurrentCoords, dest.StartPoint);
                if (currDist == destDist)
                {
                    return curr.RoutDistance > dest.RoutDistance ? curr : dest;
                }
                return currDist > destDist ? dest : curr;
            });
            return route;
        }
    }
}