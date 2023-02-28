using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace DPRCalculator
{
    public enum TextAnchor
    {
        Left,
        Middle,
        Right,
    }
    public class TextBox
    {
        private Rectangle box;
        private string text;

        private int textSize;
        protected Color textColor;
        private Color boxColor;

        private TextAnchor anchor;

        public Rectangle Bounds
        {
            get => box;
        }
        public virtual string Text
        {
            get => text;
        }

        #region Constructors
        public TextBox()
        {
            box = new Rectangle(0, 0, 50, 50);
            text = "";

            textSize = 10;
            textColor = Color.BLACK;
            boxColor = Color.WHITE;

            anchor = TextAnchor.Left;
        }
        public TextBox(Rectangle rect, int fontSize) : this()
        {
            box = rect;
            textSize = fontSize;
        }
        public TextBox(Rectangle rect, int fontSize, TextAnchor anchor) : this(rect, fontSize)
        {
            this.anchor = anchor;
        }
        public TextBox(Rectangle rect, int fontSize, string text) : this(rect, fontSize)
        {
            this.text = text;
        }
        public TextBox(Rectangle rect, int fontSize, string text, TextAnchor anchor) : this(rect, fontSize, text)
        {
            this.anchor = anchor;
        }
        public TextBox(Rectangle rect, int fontSize, Color bgColor, Color textColor) : this(rect, fontSize)
        {
            boxColor = bgColor;
            this.textColor = textColor;
        }
        public TextBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, TextAnchor anchor) : this(rect, fontSize, bgColor, textColor)
        {
            this.anchor = anchor;
        }
        public TextBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, string text) : this(rect, fontSize, bgColor, textColor)
        {
            this.text = text;
        }
        public TextBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, string text, TextAnchor anchor) : this(rect, fontSize, bgColor, textColor, anchor)
        {
            this.text = text;
        }
        #endregion

        public void UpdateText(string newText)
        {
            text = newText;
        }

        public virtual void Draw()
        {
            Vector2 textPos = new Vector2()
            {
                x = Raylib.MeasureText(text, textSize),
                y = textSize
            };

            switch (anchor)
            {
                case TextAnchor.Left:
                    textPos.x = box.x + ((box.height - textPos.y) / 2);
                    break;
                case TextAnchor.Middle:
                    textPos.x = box.x + ((box.width - textPos.x) / 2);
                    break;
                case TextAnchor.Right:
                    textPos.x = box.x + box.width - (textPos.x + ((box.height - textPos.y) / 2));
                    break;
            }
            textPos.y = box.y + (box.height - textPos.y) / 2;

            Raylib.DrawRectangleRec(box, boxColor);
            Raylib.DrawText(text, (int)textPos.x, (int)textPos.y, textSize, textColor);
        }
    }
}
