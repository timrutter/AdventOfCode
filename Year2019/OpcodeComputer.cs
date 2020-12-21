using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    public class OpcodeComputer
    {
        public OpcodeComputer(IEnumerable<int> memory)
        {
            Initialise(memory);
        }

        public List<int> Memory { get; private set; }
        public OpcodeComputer Initialise(IEnumerable<int> memory)
        {
            Memory = memory.ToList();

            return this;
        }

        public OpcodeComputer Execute()
        {
            int i = 0;
            while (true)
            {
                if (Memory[i] == 99)
                    return this;
                switch (Memory[i])
                {
                    case 1:
                        Memory[Memory[i + 3]] = Memory[Memory[i + 2]] + Memory[Memory[i + 1]];
                        i += 4;
                        break;
                    case 2 :
                        Memory[Memory[i + 3]] = Memory[Memory[i + 2]] * Memory[Memory[i + 1]];
                        i += 4;
                        break;
                }
            }
        }
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
}