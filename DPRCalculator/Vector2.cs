using System;
using System.Collections.Generic;
using System.Text;

namespace DPRCalculator
{
    public struct Vector2
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vector2(System.Numerics.Vector2 vec)
        {
            return new Vector2(vec.X, vec.Y);
        }
        public static implicit operator System.Numerics.Vector2(Vector2 vec)
        {
            return new System.Numerics.Vector2(vec.x, vec.y);
        }
    }
}
