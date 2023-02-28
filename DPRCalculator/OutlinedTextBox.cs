using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace DPRCalculator
{
    class OutlinedTextBox
    {
        private Rectangle _rect;
        private TextBox _textBox;
        private Color _outlineColor;

        public string Text
        {
            get => _textBox.Text;
        }

        #region Constructors
        public OutlinedTextBox() : this(new Rectangle(0,0,50,50), 2, 20)
        {
        }
        public OutlinedTextBox(Rectangle rect, int outline, int fontSize) : this(rect, outline, fontSize, TextAnchor.Left)
        {
        }
        public OutlinedTextBox(Rectangle rect, int outline, int fontSize, TextAnchor anchor) : this(rect, outline, fontSize, "", anchor)
        {
        }
        public OutlinedTextBox(Rectangle rect, int outline, int fontSize, string defaultText) : this(rect, outline, fontSize, defaultText, TextAnchor.Left)
        {
        }
        public OutlinedTextBox(Rectangle rect, int outline, int fontSize, string defaultText, TextAnchor anchor) : this(rect, outline, fontSize, Color.WHITE, Color.BLACK, Color.BLACK, defaultText, anchor)
        {
        }
        public OutlinedTextBox(Rectangle rect, int outline, int fontSize, Color bgColor, Color textColor, Color outlineColor) : this(rect, outline, fontSize, bgColor, textColor, outlineColor, TextAnchor.Left)
        {
        }
        public OutlinedTextBox(Rectangle rect, int outline, int fontSize, Color bgColor, Color textColor, Color outlineColor, TextAnchor anchor) : this(rect, outline, fontSize, bgColor, textColor, outlineColor, "", anchor)
        {
        }
        public OutlinedTextBox(Rectangle rect, int outline, int fontSize, Color bgColor, Color textColor, Color outlineColor, string defaultText) : this(rect, fontSize, outline, bgColor, textColor, outlineColor, defaultText, TextAnchor.Left)
        {
        }
        public OutlinedTextBox(Rectangle rect, int outline, int fontSize, Color bgColor, Color textColor, Color outlineColor, string defaultText, TextAnchor anchor)
        {
            _rect = rect;
            _outlineColor = outlineColor;
            Rectangle innerRect = new Rectangle()
            {
                x = rect.x + outline,
                y = rect.y + outline,
                width = rect.width - (outline * 2),
                height = rect.height - (outline * 2)
            };

            _textBox = new TextBox(innerRect, fontSize, bgColor, textColor, defaultText, anchor);
        }
        #endregion

        public void UpdateText(string text)
        {
            _textBox.UpdateText(text);
        }

        public void Draw()
        {
            Raylib.DrawRectangleRec(_rect, _outlineColor);
            _textBox.Draw();
        }
    }
}
