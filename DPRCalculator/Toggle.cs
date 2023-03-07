using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace DPRCalculator
{
    class Toggle : IInteractable
    {
        private Circle _circ;
        private float _outline;
        private Color _color, _outlineColor;

        private Color _checkColor;

        private bool _isToggled;

        private const float CheckScale = 2.0f / 3.0f;

        public Toggle() : this(new Circle(0, 0, 10), 2)
        {
        }
        public Toggle(Circle circ, float outline) : this(circ, outline, Color.RAYWHITE, Color.BLACK, Color.BLACK, false)
        {
        }
        public Toggle(Circle circ, float outline, bool isToggled) : this(circ, outline, Color.RAYWHITE, Color.BLACK, Color.BLACK, isToggled)
        {
        }
        public Toggle(Circle circ, float outline, Color color, Color outlineColor, Color checkColor, bool isToggled)
        {
            _circ = circ;
            _outline = outline;
            _color = color;
            _outlineColor = outlineColor;
            _checkColor = checkColor;
            _isToggled = isToggled;
        }

        public void Update() { }

        public bool AttemptInteract()
        {
            if (Raylib.CheckCollisionCircles(Raylib.GetMousePosition(), Program.mousePosBuffer, _circ.Center, _circ.Radius))
            {
                _isToggled = !_isToggled;

                return true;
            }
            return false;
        }

        public bool GetData()
        {
            return _isToggled;
        }

        public void Draw()
        {
            Raylib.DrawCircleV(_circ.Center, _circ.Radius, _outlineColor);
            Raylib.DrawCircleV(_circ.Center, _circ.Radius - _outline, _color);

            if (_isToggled)
                Raylib.DrawCircleV(_circ.Center, (_circ.Radius - _outline) * CheckScale, _checkColor);
        }
    }
}
