using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMyElement.DataModel
{
    static class GroupMatching
    {
        static public readonly List<int> Actinoid = new List<int> { 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103 };
        static public readonly List<int> AlkaliMetal = new List<int> { 3, 11, 19, 37, 55, 87 };
        static public readonly List<int> AlkalineEarth = new List<int> { 4, 12, 20, 38, 56, 88 };
        static public readonly List<int> Halogen = new List<int> { 9, 17, 35, 53, 85, 117 };
        static public readonly List<int> Lanthanoid = new List<int> { 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71 };
        static public readonly List<int> Metalloid = new List<int> { 5, 14, 32, 33, 51, 52, 84 };
        static public readonly List<int> Noble = new List<int> { 2, 10, 18, 36, 54, 86, 118 };
        static public readonly List<int> NonMetal = new List<int> { 1, 6, 7, 8, 9, 15, 16, 34 };
        static public readonly List<int> PostTransitionMetal = new List<int> { 13, 31, 49, 50, 81, 82, 83, 113, 114, 115, 116 };
        static public readonly List<int> TransitionMetal = new List<int> { 
            21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            39, 40, 41, 42, 43, 44, 45, 46, 47, 48,
            72, 73, 74, 75, 76, 77, 78, 79, 80,
            104, 105, 106, 107, 108, 109, 110, 111, 112 };
    }
}
