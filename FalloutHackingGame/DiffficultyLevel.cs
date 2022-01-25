using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutHackingGame
{

    //This class should be pretty self-explanatory.
    public class DifficultyLevel
    {
        public int MinLength { get; set; }
        public int MaxLength { get; set; }

        public int MinOptions { get; set; }
        public int MaxOptions { get; set; }

        public string Name { get; set; }

        public DifficultyLevel(string DiffNameParameter, int MinLengthParameter, int MaxLengthParameter, int MinOptionsParameter, int MaxOptionsParameter)
        {
            MinLength = MinLengthParameter;
            MaxLength = MaxLengthParameter;
            MinOptions = MinOptionsParameter;
            MaxOptions = MaxOptionsParameter;
            Name = DiffNameParameter;
        }
    }
}
