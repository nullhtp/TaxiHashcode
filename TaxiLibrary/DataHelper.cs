using System.Collections.Generic;
using System.IO;

namespace TaxiLibrary
{
    public static class DataHelper
    {
        public static Game LoadGameFromFile(string filename)
        {
            var strings = File.ReadAllLines(filename);
            var conditions = strings[0].Split(' ');
            var rows = int.Parse(conditions[0]);
            var cols = int.Parse(conditions[1]);
            var cars = int.Parse(conditions[2]);
            var rides = int.Parse(conditions[3]);
            var bonus = int.Parse(conditions[4]);
            var steps = int.Parse(conditions[5]);
            var routes = new List<Route>();
            var availableRoutes = new List<Route>();
            for (var i = 1; i < rides; i++)
            {
                routes.Add(CreateRoute(strings[i], i));
                availableRoutes.Add(CreateRoute(strings[i], i));
            }
            var game = new Game
            {
                Bonus = bonus,
                Rows = rows,
                Columns = cols,
                CountRides = rides,
                CountTaxi = cars,
                SimulationSteps = steps,
                Routes = routes,
                AvailableRoutes = availableRoutes,
                Cars = CreateTaxies(cars)
            };
            return game;
        }

        public static List<Taxi> CreateTaxies(int count)
        {
            var taxies = new List<Taxi>();
            for (int i = 0; i < count; i++)
            {
                taxies.Add(new Taxi
                {
                    CarNum = i,
                    CurrentCoords = new Coords {NumColumn = 0, NumRow = 0}
                });
            }
            return taxies;
        }

        public static Route CreateRoute(string strRoute, int name)
        {
            var points = strRoute.Split(' ');

            var pointStartRow = int.Parse(points[0]);
            var pointStartCol = int.Parse(points[1]);
            var pointEndRow = int.Parse(points[2]);
            var pointEndCol = int.Parse(points[3]);
            var pointStartTime = int.Parse(points[4]);
            var pointEndTime = int.Parse(points[5]);
            var route = new Route
            {
                RouteName = name - 1,
                StartPoint = new Coords
                {
                    NumColumn = pointStartCol,
                    NumRow = pointStartRow
                },
                EndPoint = new Coords
                {
                    NumColumn = pointEndCol,
                    NumRow = pointEndRow
                },
                EndTime = pointEndTime,
                StartTime = pointStartTime
            };
            return route;
        }

        public static void SaveToFile(List<Taxi> cars, string filename)
        {
            var str = "";
            foreach (var car in cars)
            {
                str += $"{car.CarNum+1} ";
                foreach (var route in car.Routes)
                {
                    str += $"{route.RouteName} ";
                }
                str += "\n";
            }
            File.WriteAllText(filename, str);
        }
    }
}