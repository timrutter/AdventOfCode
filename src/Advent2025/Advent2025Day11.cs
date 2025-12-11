using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2025;

public class Advent2025Day11 : Solution
{
    private readonly Dictionary<Device, Counts> _cache = new();

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
        return start!.Traverse().Count(d => Equals(d, end));
    }

    public override object ExecutePart2()
    {
        var lines = DataFile.ReadAll<string>();
        var devices = new Dictionary<string, Device>();
        foreach (var line in lines)
        {
            var bits = line.Split(": ");

            var thisDevice = FindOrAdd(bits[0]);

            foreach (var child in bits[1].Split(' '))
                thisDevice.AddChild(FindOrAdd(child));
        }

        var counts = CountPaths(devices["svr"]);
        return counts.CountBoth;

        Device FindOrAdd(string name)
        {
            devices.TryGetValue(name, out var thisDevice);
            if (thisDevice is not null) return thisDevice;
            thisDevice = new Device(name);
            devices.Add(thisDevice.Name, thisDevice);
            return thisDevice;
        }
    }

    struct Counts
    {
        public long CountPaths { get; set; } = 0;
        public long CountFft{ get; set; } = 0;
        public long CountDac{ get; set; } = 0; 
        public long CountBoth{ get; set; } = 0;


        public Counts(long countPaths, long countFft, long countDac, long countBoth)
        {
            CountPaths = countPaths;
            CountFft = countFft;
            CountDac = countDac;
            CountBoth = countBoth;
        }
    }
    private Counts CountPaths(Device device)
    {
        if (_cache.TryGetValue(device, out var result)) return result;
        if (device.Name == "out") return new Counts(1, 0, 0, 0);

        Counts counts = new(0, 0, 0, 0);
        foreach (var deviceChild in device.Children)
        {
            var childcounts = CountPaths(deviceChild);
            counts.CountPaths += childcounts.CountPaths;
            if (childcounts.CountBoth > 0)
            {
                // once we have both don't care about finding ffts or dacs
                counts.CountBoth += childcounts.CountBoth;
                continue;
            }

            if (deviceChild.IsFft)
            {
                counts.CountFft += childcounts.CountPaths;
                counts.CountBoth += childcounts.CountDac;
            }
            else
            {
                counts.CountFft += childcounts.CountFft;
            }

            if (deviceChild.IsDac)
            {
                counts.CountDac += childcounts.CountPaths;
                counts.CountBoth += childcounts.CountFft;
            }
            else
            {
                counts.CountDac += childcounts.CountDac;
            }
        }

        _cache[device] = counts;
        return counts;
    }

    private class Device(string name) : Graph<Device>
    {
        private readonly int _hash = name.GetHashCode();

        private bool Equals(Device other)
        {
            return _hash == other._hash;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Device)obj);
        }

        public override int GetHashCode()
        {
            return _hash;
        }

        public bool IsDac { get; } = name == "dac";

        public bool IsFft { get; } = name == "fft";

        public string Name { get; } = name;

        public override string ToString() => Name;
    }
}