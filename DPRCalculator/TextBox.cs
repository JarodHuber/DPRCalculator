using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace DPRCalculator
{
    public enum TextAnchor
    {
        Left,
        Center,
        Right,
    }
    public class TextBox
    {
        protected Rectangle _rect;
        private string _text;

        private int _fontSize;
        protected Color _textColor;
        private Color _backgroundColor;

        private TextAnchor _textAnchor;

        public virtual string Text
        {
            get => _text;
        }

        #region Constructors
        public TextBox() : this(new Rectangle(0, 0, 50, 50), 10)
        {
        }
        public TextBox(Rectangle rect, int fontSize) : this(rect, fontSize, "")
        {
        }
        public TextBox(Rectangle rect, int fontSize, TextAnchor anchor) : this(rect, fontSize, "", TextAnchor.Left)
        {
        }
        public TextBox(Rectangle rect, int fontSize, string text) : this(rect, fontSize, text, TextAnchor.Left)
        {
        }
        public TextBox(Rectangle rect, int fontSize, string text, TextAnchor anchor) : this(rect, fontSize, Color.WHITE, Color.BLACK, text, anchor)
        {
        }
        public TextBox(Rectangle rect, int fontSize, Color bgColor, Color textColor) : this(rect, fontSize, bgColor, textColor, "")
        {
        }
        public TextBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, TextAnchor anchor) : this(rect, fontSize, bgColor, textColor, "", anchor)
        {
        }
        public TextBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, string text) : this(rect, fontSize, bgColor, textColor, text, TextAnchor.Left)
        {
        }
        public TextBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, string text, TextAnchor anchor)
        {
            _rect = rect;
            _text = text;

            _fontSize = fontSize;
            _textColor = textColor;
            _backgroundColor = bgColor;

            _textAnchor = anchor;
        }
        #endregion

        public void UpdateText(string newText)
        {
            _text = newText;
        }

        public virtual void Draw()
        {
            Vector2 textPos = new Vector2()
            {
                x = Raylib.MeasureText(_text, _fontSize),
                y = _fontSize
            };

            switch (_textAnchor)
            {
                case TextAnchor.Left:
                    textPos.x = _rect.x + ((_rect.height - textPos.y) / 2);
                    break;
                case TextAnchor.Center:
                    textPos.x = _rect.x + ((_rect.width - textPos.x) / 2);
                    break;
                case TextAnchor.Right:
                    textPos.x = _rect.x + _rect.width - (textPos.x + ((_rect.height - textPos.y) / 2));
                    break;
            }
            textPos.y = _rect.y + (_rect.height - textPos.y) / 2;

            Raylib.DrawRectangleRec(_rect, _backgroundColor);
            Raylib.DrawText(_text, (int)textPos.x, (int)textPos.y, _fontSize, _textColor);
        }
    }
}
