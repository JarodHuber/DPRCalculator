using System;
using System.Collections.Generic;
using System.Text;

namespace DPRCalculator
{
    struct Circle
    {
        private Vector2 _center;
        private float _radius;

        public Vector2 Center 
        { 
            get => _center; 
            set => _center = value; 
        }
        public float Radius 
        { 
            get => _radius; 
            set => _radius = value;
        }

        public Circle(float x, float y, float radius) : this(new Vector2(x, y), radius)
        {
        }
        public Circle(Vector2 center, float radius)
        {
            _center = center;
            _radius = radius;
        }
    }
}
