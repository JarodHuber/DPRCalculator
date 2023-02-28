using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace DPRCalculator
{
    class Graph
    {
        private Color _color;
        private float _pointRad;

        private Rectangle _rect;
        private System.Numerics.Vector2[] _points;

        private float _minX, _maxX;
        private float _minY, _maxY;

        public Graph() : this(new Rectangle(0, 0, 300, 200))
        {

        }
        public Graph(Rectangle rect) : this(rect, Color.RED, 5, 1)
        {
        }
        public Graph(Rectangle rect, Color color, float pointRad, int pointCount) : this(rect, color, pointRad, pointCount, 0, 1, 0, 1)
        {

        }
        public Graph(Rectangle rect, Color color, float pointRad, int pointCount, float minX, float maxX, float minY, float maxY)
        {
            _color = color;
            _pointRad = pointRad;

            _rect = rect;

            _minX = minX;
            _maxX = maxX;
            _minY = minY;
            _maxY = maxY;

            _points = new System.Numerics.Vector2[pointCount];
        }

        public void UpdatePoints(Vector2[] data)
        {
            Vector2 dataScale = new Vector2();
            for(int i = 0; i < _points.Length; ++i)
            {
                dataScale.x = MathF.Max((data[i].x - _minX) / (_maxX - _minX), 0);
                dataScale.y = MathF.Max((data[i].y - _minY) / (_maxY - _minY), 0);

                _points[i].X = _rect.x + (dataScale.x * _rect.width);
                _points[i].Y = (_rect.y + _rect.height) - (dataScale.y * _rect.height);
            }
        }

        public void UpdateScale(float maxY)
        {
            _maxY = maxY;
        }

        public void Draw()
        {
            //Raylib.DrawLineStrip(_points, _points.Length, _color);

            for(int x = 1; x < _points.Length; ++x)
            {
                Raylib.DrawLineEx(_points[x - 1], _points[x], _pointRad, _color);
                Raylib.DrawCircleV(_points[x - 1], _pointRad, _color);
            }
            Raylib.DrawCircleV(_points[_points.Length - 1], _pointRad, _color);
        }

        public static int RoundGraphSize(int x)
        {
            int scale = x == 0 ? 0 : Utils.Digits_IfChain(x);
            scale = (int)MathF.Pow(10, scale);

            if (x < 0.125f * scale)
                return x + (int)((0.025f * scale) - (x % (0.025f * scale)));

            if (x < 0.25f * scale)
                return x + (int)((0.05f * scale) - (x % (0.05f * scale)));

            if (x < 0.5f * scale)
                return x + (int)((0.1f * scale) - (x % (0.1f * scale)));

            if (x < 0.8f * scale)
                return x + (int)((0.2f * scale) - (x % (0.2f * scale)));

            scale *= 10;
            return x + (int)((0.025f * scale) - (x % (0.025f * scale))); ;
        }
        public static int RoundGraphSize(int x, out float scale, out float magnitude)
        {
            scale = 0;
            magnitude = x == 0 ? 0 : Utils.Digits_IfChain(x);
            magnitude = (int)MathF.Pow(10, magnitude);

            if (x < 0.125f * magnitude)
            {
                scale = 0.025f;
                return x + (int)((0.025f * magnitude) - (x % (0.025f * magnitude)));
            }

            if (x < 0.25f * magnitude)
            {
                scale = 0.05f;
                return x + (int)((0.05f * magnitude) - (x % (0.05f * magnitude)));
            }

            if (x < 0.5f * magnitude)
            {
                scale = 0.1f;
                return x + (int)((0.1f * magnitude) - (x % (0.1f * magnitude)));
            }

            if (x < 0.8f * magnitude)
            {
                scale = 0.2f;
                return x + (int)((0.2f * magnitude) - (x % (0.2f * magnitude)));
            }

            magnitude *= 10;
            scale = 0.025f;
            return x + (int)((0.025f * magnitude) - (x % (0.025f * magnitude)));
        }
    }
}
