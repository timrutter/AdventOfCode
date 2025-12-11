using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2025;

public class Advent2025Day11 : Solution
{
    private readonly Dictionary<Device, (long countPaths, long countFft, long countDac, long countBoth)> _cache = new();

    public Advent2025Day11()
    {
        Answer1 = 690;
        Answer2 = 557332758684000;
    }

    public override object ExecutePart1()
    {
        var lines = DataFile.ReadAll<string>();
        var devices = new List<Device>();
        foreach (var line in lines)
        {
            var bits = line.Split(": ");

            var thisDevice = devices.Find(d => d.Name == bits[0]);
            if (thisDevice is null)
            {
                thisDevice = new Device(bits[0]);
                devices.Add(thisDevice);
            }

            foreach (var child in bits[1].Split(' '))
            {
                var childDevice = devices.Find(d => d.Name == child);
                if (childDevice is null)
                {
                    childDevice = new Device(child);
                    devices.Add(childDevice);
                }

                thisDevice.AddChild(childDevice);
            }
        }

        var start = devices.Find(d => d.Name == "you");
        var end = devices.Find(d => d.Name == "out");
        return start!.Traverse().Count(d => d == end);
    }

    public override object ExecutePart2()
    {
        var lines = DataFile.ReadAll<string>();
        var devices = new List<Device>();
        foreach (var line in lines)
        {
            var bits = line.Split(": ");

            var thisDevice = devices.Find(d => d.Name == bits[0]);
            if (thisDevice is null)
            {
                thisDevice = new Device(bits[0]);
                devices.Add(thisDevice);
            }

            foreach (var child in bits[1].Split(' '))
            {
                var childDevice = devices.Find(d => d.Name == child);
                if (childDevice is null)
                {
                    childDevice = new Device(child);
                    devices.Add(childDevice);
                }

                thisDevice.AddChild(childDevice);
            }
        }

        var counts = CountPaths(devices.Find(d => d.Name == "svr"));
        return counts.countBoth;
    }

    private (long countPaths, long countFft, long countDac, long countBoth) CountPaths(Device device)
    {
        if (_cache.TryGetValue(device, out var result)) return result;
        if (device.Name == "out") return (1, 0, 0, 0);

        (long countPaths, long countFft, long countDac, long countBoth) counts = (0, 0, 0, 0);
        foreach (var deviceChild in device.Children)
        {
            var childcounts = CountPaths(deviceChild);
            counts.countPaths += childcounts.countPaths;
            if (childcounts.countBoth > 0)
            {
                // once we have both don't care about finding ffts or dacs
                counts.countBoth += childcounts.countBoth;
                continue;
            }

            if (deviceChild.Name == "fft")
            {
                counts.countFft += childcounts.countPaths;
                counts.countBoth += childcounts.countDac;
            }
            else
            {
                counts.countFft += childcounts.countFft;
            }

            if (deviceChild.Name == "dac")
            {
                counts.countDac += childcounts.countPaths;
                counts.countBoth += childcounts.countFft;
            }
            else
            {
                counts.countDac += childcounts.countDac;
            }
        }

        _cache.Add(device, counts);
        return counts;
    }

    private class Device(string name) : Graph<Device>
    {
        public string Name { get; } = name;

        public override string ToString() => Name;
    }
}