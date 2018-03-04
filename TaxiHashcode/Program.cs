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
//            var beginCoord = new Coords {NumColumn = 0, NumRow = 0};
            var filename = "D";
            var game = DataHelper.LoadGameFromFile($"Data/{filename}.in");
            Console.WriteLine($"Cars = {game.CountTaxi} Bonus = {game.Bonus} Steps = {game.SimulationSteps}" +
                              $" Rides = {game.CountRides} MaxPoints = {game.AvailableRoutes.Sum(r => r.RoutDistance) + game.Bonus * game.CountRides}");
            //1.Filtering 
//            game.AvailableRoutes = game.AvailableRoutes
//                .Where(r => r.DiffTime >= RouteHelper.RouteDistance(r.StartPoint, beginCoord) + r.RoutDistance)
//                .ToList();
            //2.
            for (var step = 0; step < game.SimulationSteps; step++)
            {
                foreach (var car in game.Cars)
                {
                    if (car.CountStepsForAvailable <= step)
                    {
                        if (car.CurrentRoute != null)
                        {
                            car.Routes.Add(car.CurrentRoute);
                        }

                        if (game.AvailableRoutes.Any())
                        {
                            car.CurrentRoute = OptimalRoute(car, game.AvailableRoutes, step, game);
                            if(car.CurrentRoute==null) continue;
                            car.CurrentRoute.Bonus = IsBonus(car, step);
                            car.CountStepsForAvailable = CalcAvailableTime(car, step);

                            car.CurrentCoords = car.CurrentRoute.EndPoint;

                            game.AvailableRoutes.Remove(car.CurrentRoute);
                        }
                        else
                        {
                            car.CurrentRoute = null;
                        }
                    }
                }
            }
            Console.WriteLine($"Routes = {game.Cars.Sum(c => c.Routes.Count)}");
            Console.WriteLine($"Points = {GetPoints(game)}");

            DataHelper.SaveToFile(game.Cars, $"Data/{filename}.out");
        }

        private static int GetPoints(Game game)
        {
            var points = 0;
            foreach (var car in game.Cars)
            {
                foreach (var route in car.Routes)
                {
                    points += route.RoutDistance;
                    if (route.Bonus)
                    {
                        points += game.Bonus;
                    }
                }
            }
            return points;
        }

        private static bool IsBonus(Taxi car, int step)
        {
            return RouteHelper.RouteDistance(car.CurrentCoords, car.CurrentRoute.StartPoint)
                   + step <= car.CurrentRoute.StartTime;
        }

        private static int CalcAvailableTime(Taxi car, int step)
        {
            return RouteHelper.RouteDistance(car.CurrentCoords, car.CurrentRoute.StartPoint) + step <
                   car.CurrentRoute.StartTime
                ? car.CurrentRoute.StartTime + car.CurrentRoute.RoutDistance
                : RouteHelper.RouteDistance(car.CurrentCoords, car.CurrentRoute.StartPoint) +
                  step +
                  car.CurrentRoute.RoutDistance;
        }

        private static Route OptimalRoute(Taxi taxi, List<Route> routes, int step, Game game)
        {
            routes = routes.Where(r =>
                    RouteHelper.RouteDistance(taxi.CurrentCoords, r.StartPoint)+step + r.RoutDistance <=
                    r.EndTime)
                .ToList();
            if (!routes.Any()) return null;
            
            var route = routes.Aggregate((curr, dest) =>
            {
                if (game.SimulationSteps - RouteHelper.RouteDistance(taxi.CurrentCoords, dest.StartPoint) + step +
                    dest.RoutDistance == 0)
                {
                    return dest;
                }
                var currDist =
                    Math.Abs(RouteHelper.RouteDistance(taxi.CurrentCoords, curr.StartPoint) +  curr.StartTime);
                var destDist =
                    Math.Abs(RouteHelper.RouteDistance(taxi.CurrentCoords, dest.StartPoint) +  dest.StartTime);

                
                if (currDist == destDist)
                {
                    return curr.RoutDistance > dest.RoutDistance ? curr : dest;
                }
                return currDist >= destDist ? dest : curr;
            });
            return route;
        }

    }
}