using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021;

public class Advent2021Day12 : Solution
{
    public Advent2021Day12()
    {
        Answer1 = 3887;
        Answer2 = 104834;
    }

    public override object ExecutePart1()
    {
        var lines = DataFile.ReadAll<string>().Select(l => l.Split("-")).ToList();
        var routes = FindRoutes(lines, ["start"]);
        return routes.Count;
    }

    private static List<List<string>> FindRoutes(List<string[]> connections, List<string> route)
    {
        if (route.Last() == "end") return [route];
        var routes = new List<List<string>>();
        var links = connections.Select(c => c[0] == route.Last() ? c[1] : c[1] == route.Last() ? c[0] : null)
            .Where(c => c != null).ToList();
        var newRoutes = new List<List<string>>();
        foreach (var link in links.Where(link => !route.Contains(link) || !char.IsLower(link[0])))
        {
            newRoutes.Add(route.ToList());
            newRoutes.Last().Add(link);
        }

        foreach (var newRoute in newRoutes) routes.AddRange(FindRoutes(connections, newRoute));
        return routes;
    }

    public override object ExecutePart2()
    {
        var lines = DataFile.ReadAll<string>().Select(l => l.Split("-")).ToList();
        var routes = FindRoutes2(lines, ["start"], "");
        return routes.Count;
    }

    private static List<List<string>> FindRoutes2(List<string[]> connections, List<string> route, string smallCave)
    {
        if (route.Last() == "end") return [route];
        var routes = new List<List<string>>();
        var links = connections.Select(c => c[0] == route.Last() ? c[1] : c[1] == route.Last() ? c[0] : null)
            .Where(c => c != null).ToList();
        var newRoutes = new List<(List<string> route, string smallCave)>();
        foreach (var link in links)
        {
            if (link == "start") continue;
            var newSmallCave = smallCave;
            if (char.IsLower(link[0]))
                if (route.Contains(link))
                {
                    if (newSmallCave != "") continue;
                    newSmallCave = link;
                }

            newRoutes.Add((route.ToList(), newSmallCave));
            newRoutes.Last().route.Add(link);
        }

        foreach (var newRoute in newRoutes)
            routes.AddRange(FindRoutes2(connections, newRoute.route, newRoute.smallCave));
        return routes;
    }
}