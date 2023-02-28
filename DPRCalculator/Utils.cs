using System;
using System.Collections.Generic;
using System.Text;

namespace DPRCalculator
{
    public static class Utils
    {
        public static int Digits_IfChain(this int n)
        {
            if (n >= 0)
            {
                if (n < 10) return 1;
                if (n < 100) return 2;
                if (n < 1_000) return 3;
                if (n < 10_000) return 4;
                if (n < 100_000) return 5;
                if (n < 1_000_000) return 6;
                if (n < 10_000_000) return 7;
                if (n < 100_000_000) return 8;
                if (n < 1_000_000_000) return 9;
                return 10;
            }
            else
            {
                if (n > -10) return 1;
                if (n > -100) return 2;
                if (n > -1_000) return 3;
                if (n > -10_000) return 4;
                if (n > -100_000) return 5;
                if (n > -1_000_000) return 6;
                if (n > -10_000_000) return 7;
                if (n > -100_000_000) return 8;
                if (n > -1_000_000_000) return 9;
                return 10;
            }
        }

        public static float Clamp(float value, float min, float max)
        {
            return (value < max) ? (value > min) ? value : min : max;
        }
        public static float Max(float a, float b)
        {
            return (a >= b) ? a : b;
        }
        public static float Min(float a, float b)
        {
            return (a <= b) ? a : b;
        }
    }
}
