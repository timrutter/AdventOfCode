using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace AdventOfCode.Advent2021
{
    public class Advent2021Day16 : Solution
    {

        public Advent2021Day16()
        {
            Answer1 = 963;
            Answer2 = 1549026292886;
        }

        private static string ToBinary(string s)
        {
            return string.Concat(s.Select(
                c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')).ToList());
        }
        public override object ExecutePart1()
        {
            return ParsePacket(ToBinary(File.ReadAllText(DataFile))).packet.SumVersions();
        }

        private static (Packet packet, int read) ParsePacket(string packet)
        {
            var version = Convert.ToInt32(packet[0..3], 2);
            var packetTypeID = Convert.ToInt32(packet[3..6], 2);
            var numberStrings = new List<string>();
            if (packetTypeID == 4)
            {
                int i = 6;
                for ( ; i < packet.Length; i += 5)
                {
                    numberStrings.Add(packet[(i + 1)..(i + 5)]);
                    if (packet[i] == '0') break;
                }

                return (new Packet{Version = version, PacketTypeID = packetTypeID, Value = Convert.ToInt64(string.Concat(numberStrings), 2)}, i + 5);
            }
            else 
            {
                if (packet[6] == '0')
                {
                    var length = Convert.ToInt32(packet[7..22], 2);
                    var children = new List<Packet>();
                    int read = 0;
                    while (read < length)
                    {
                        (var packet1, int i) = ParsePacket(packet[(22 + read)..]);
                        children.Add(packet1);
                        read += i;
                    }

                    return (new Packet {Version = version, PacketTypeID = packetTypeID, Value = length, Children = children}, 22 + length);
                }
                if (packet[6] == '1')
                {
                    var count = Convert.ToInt32(packet[7..18], 2);
                    
                    var children = new List<Packet>();
                    int read = 0;
                    for (int i = 0; i < count; i++)
                    {
                        (var packet1, int read1) = ParsePacket(packet[(18 + read)..]);
                        children.Add(packet1);
                        read += read1;
                    }

                    return (new Packet { Version = version, PacketTypeID = packetTypeID, Value = count, Children = children }, 18 + read);
                }
            }
            return (null, 0);
        }
        public override object ExecutePart2()
        {
            var packet = ParsePacket(ToBinary(File.ReadAllText(DataFile))).packet;
            return packet.Calculate();
        }
    }

    internal class Packet
    {
        public int Version { get; set; }
        public int PacketTypeID { get; set; }
        public List<Packet> Children = new List<Packet>();
        public long Value { get; set; }

        public int SumVersions()
        {
            return this.Version + Children.Sum(c => c.SumVersions());
        }

        public long Calculate()
        {
            switch (PacketTypeID)
            {
                case 0:
                    return Children.Sum(c => c.Calculate());

                case 1:
                    return Children.Select(c => c.Calculate()).Aggregate((long)1, (a, b) => a * b);

                case 2:
                    return Children.Select(c => c.Calculate()).Min();

                case 3:
                    return Children.Select(c => c.Calculate()).Max();

                case 4:
                    return Value;

                case 5:
                    return Children[0].Calculate() > Children[1].Calculate() ? 1 : 0;

                case 6:
                    return Children[0].Calculate() < Children[1].Calculate() ? 1 : 0;

                case 7:
                    return Children[0].Calculate() == Children[1].Calculate() ? 1 : 0;
            }

            return 0;
        }
    }
}