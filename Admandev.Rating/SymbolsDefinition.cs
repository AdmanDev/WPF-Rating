using System;
using System.Windows.Media;

namespace Admandev.Rating
{
    public enum Symbols
    {
        Star,
        Heart
    }

    public static class SymbolsDefinition
    {
        public readonly static Geometry STAR = Geometry.Parse("F1 M 17.416,32.25L 32.910,32.25L 38,18L 43.089,32.25L 58.583,32.25L 45.679,41.494L 51.458,56L 38,48.083L 26.125,56L 30.597,41.710L 17.416,32.25 Z");

        public readonly static Geometry HEART = Geometry.Parse("M 241,200 A 20,20 0 0 0 200,240 C 210,250 240,270 240,270 C 240,270 260,260 280,240 A 20,20 0 0 0 239,200");
    }
}
