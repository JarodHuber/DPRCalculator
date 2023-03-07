using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DPRCalculator
{
    public enum LabelSide
    {
        Right,
        Left,
        Up,
        Down,
    }
    class LabeledToggle : IInteractable
    {
        private Toggle _toggle;
        private Vector2 _position;

        private string _label;
        private int _fontSize;
        private Color _labelColor;

        public LabeledToggle(string label, int labelOffset, int fontSize, Circle circ) :
            this(label, labelOffset, fontSize, circ, 2)
        {
        }
        public LabeledToggle(string label, int labelOffset, int fontSize, Circle circ, bool isToggled) :
            this(label, labelOffset, fontSize, circ, 2, isToggled)
        {
        }
        public LabeledToggle(string label, int labelOffset, int fontSize, Circle circ, float outline) :
            this(label, labelOffset, LabelSide.Right, fontSize, circ, outline)
        {
        }
        public LabeledToggle(string label, int labelOffset, int fontSize, Circle circ, float outline, bool isToggled) :
            this(label, labelOffset, LabelSide.Right, fontSize, circ, outline, isToggled)
        {
        }
        public LabeledToggle(string label, int labelOffset, LabelSide labelSide, int fontSize, Circle circ, float outline) :
            this(label, labelOffset, labelSide, fontSize, circ, outline, Color.WHITE, Color.BLACK, Color.BLACK, Color.BLACK)
        {
        }
        public LabeledToggle(string label, int labelOffset, LabelSide labelSide, int fontSize, Circle circ, float outline, bool isToggled) :
            this(label, labelOffset, labelSide, fontSize, circ, outline, Color.WHITE, Color.BLACK, Color.BLACK, Color.BLACK, isToggled)
        {
        }
        public LabeledToggle(string label, int labelOffset, LabelSide labelSide, int fontSize, Circle circ, float outline, Color color, Color outlineColor, Color checkColor, Color labelColor) : 
            this(label, labelOffset, labelSide, fontSize, circ, outline, color, outlineColor, checkColor, labelColor, false)
        {
        }
        public LabeledToggle(string label, int labelOffset, LabelSide labelSide, int fontSize, Circle circ, float outline, Color color, Color outlineColor, Color checkColor, Color labelColor, bool isToggled)
        {
            _toggle = new Toggle(circ, outline, color, outlineColor, checkColor, isToggled);

            _label = label;
            _fontSize = fontSize;
            _labelColor = labelColor;

            _position = circ.Center;
            switch (labelSide)
            {
                case LabelSide.Right:
                    _position.x += labelOffset;
                    _position.y -= fontSize / 2.0f;
                    break;
                case LabelSide.Left:
                    _position.x -= Raylib.MeasureText(label, fontSize) + labelOffset;
                    _position.y -= fontSize / 2.0f;
                    break;
                case LabelSide.Up:
                    _position.x -= Raylib.MeasureText(label, fontSize) / 2.0f;
                    _position.y -= fontSize + labelOffset;
                    break;
                case LabelSide.Down:
                    _position.x -= Raylib.MeasureText(label, fontSize) / 2.0f;
                    _position.y += labelOffset;
                    break;
            }
        }

        public bool GetData()
        {
            return _toggle.GetData();
        }

        public bool AttemptInteract()
        {
            return _toggle.AttemptInteract();
        }

        public void Update() { }

        public void Draw()
        {
            _toggle.Draw();
            Raylib.DrawText(_label, (int)_position.x, (int)_position.y, _fontSize, _labelColor);
        }
    }
}
