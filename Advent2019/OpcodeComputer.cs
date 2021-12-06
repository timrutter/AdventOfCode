using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Advent2019
{
    public enum ParameterMode
    {
        PositionMode,
        ImmediateMode
    }

    public static class OpcodeComputerExtensions
    {
        public static OpcodeComputer CreateAndExecuteOpcodeComputer(this IEnumerable<int> memory)
        {
            var oc = new OpcodeComputer(memory);
            return oc.Execute();
        }

        public static OpcodeComputer CreateOpcodeComputer(this IEnumerable<int> memory)
        {
            return new OpcodeComputer(memory);
        }
    }

    public class OpcodeComputer
    {
        public OpcodeComputer(IEnumerable<int> memory)
        {
            Initialise(memory);
        }

        public List<int> Memory { get; private set; }
        public int Input { get; set; }
        public int Output { get; set; }

        public OpcodeComputer Initialise(IEnumerable<int> memory)
        {
            Memory = memory.ToList();

            return this;
        }

        public OpcodeComputer Execute()
        {
            var i = 0;
            while (true)
            {
                 var opCode = Memory[i];
                 if (opCode == 99)
                     return this;
                 switch (GetDigit(100,opCode))
                 {
                     case 1:
                         //Memory[Memory[i + 3]] = InterpretParameter[Memory[i + 2]] + InterpretParameter[Memory[i + 1]];
                         i += 4;
                         break;
                     case 2 :
                         //Memory[Memory[i + 3]] = InterpretParameter[Memory[i + 2]] * InterpretParameter[Memory[i + 1]];
                         i += 4;
                         break;
                     case 3 :
                         Memory[Memory[i + 1]] = Input;
                         i += 1;
                         break;
                     case 4 :
                         Output = Memory[Memory[i + 1]];
                         i += 1;
                         break;
                 }
            }
        }

        private int GetDigit(int digit, int number)
        {
            return number / digit % digit;
        }

        private int InterpretParameter(int p, int mode)
        {
            switch (mode)
            {
                case 0:
                    return Memory[p];
                default:
                    return p;
            }
        }
    }
}