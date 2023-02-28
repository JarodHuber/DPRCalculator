using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DPRCalculator
{
    class OutlinedInteractionBox : Interactable
    {
        private Rectangle _rect;
        private InteractionBox _interactionBox;
        private Color _outlineColor;

        public string Text
        {
            get => _interactionBox.Text;
        }

        #region Constructors
        public OutlinedInteractionBox() : this(new Rectangle(0, 0, 50, 50), 2, 20)
        {
        }
        public OutlinedInteractionBox(Rectangle rect, int outline, int fontSize) : this(rect, outline, fontSize, TextAnchor.Left)
        {
        }
        public OutlinedInteractionBox(Rectangle rect, int outline, int fontSize, TextAnchor anchor) : this(rect, outline, fontSize, "", anchor)
        {
        }
        public OutlinedInteractionBox(Rectangle rect, int outline, int fontSize, string defaultText) : this(rect, outline, fontSize, defaultText, TextAnchor.Left)
        {
        }
        public OutlinedInteractionBox(Rectangle rect, int outline, int fontSize, string defaultText, TextAnchor anchor) : this(rect, outline, fontSize, defaultText, anchor, TextBoxDataType.Normal)
        {
        }
        public OutlinedInteractionBox(Rectangle rect, int outline, int fontSize, string defaultText, TextBoxDataType type) : this(rect, outline, fontSize, defaultText, TextAnchor.Left, type)
        {
        }
        public OutlinedInteractionBox(Rectangle rect, int outline, int fontSize, string defaultText, TextAnchor anchor, TextBoxDataType type) : this(rect, outline, fontSize, Color.WHITE, Color.BLACK, Color.BLACK, defaultText, anchor, type)
        {
        }
        public OutlinedInteractionBox(Rectangle rect, int outline, int fontSize, Color bgColor, Color textColor, Color outlineColor) : this(rect, outline, fontSize, bgColor, textColor, outlineColor, TextAnchor.Left)
        {
        }
        public OutlinedInteractionBox(Rectangle rect, int outline, int fontSize, Color bgColor, Color textColor, Color outlineColor, TextAnchor anchor) : this(rect, outline, fontSize, bgColor, textColor, outlineColor, "", anchor, TextBoxDataType.Normal)
        {
        }
        public OutlinedInteractionBox(Rectangle rect, int outline, int fontSize, Color bgColor, Color textColor, Color outlineColor, string defaultText) : this(rect, fontSize, outline, bgColor, textColor, outlineColor, defaultText, TextAnchor.Left, TextBoxDataType.Normal)
        {
        }
        public OutlinedInteractionBox(Rectangle rect, int outline, int fontSize, Color bgColor, Color textColor, Color outlineColor, string defaultText, TextAnchor anchor, TextBoxDataType type)
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

            _interactionBox = new InteractionBox(innerRect, fontSize, bgColor, textColor, defaultText, anchor, type);
        }
        #endregion

        public bool AttemptInteract()
        {
            return _interactionBox.AttemptInteract();
        }

        public void Update()
        {
            _interactionBox.Update();
        }

        public void Draw()
        {
            Raylib.DrawRectangleRec(_rect, _outlineColor);
            _interactionBox.Draw();
        }

    }
}
