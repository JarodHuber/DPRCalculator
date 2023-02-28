using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DPRCalculator
{
    class GroupTextBoxes
    {
        private Rectangle box = new Rectangle(0, 0, 265, 60);

        private Color outlineColor = Color.BLACK;

        protected TextBox[] boxes;

        #region Constructors
        public GroupTextBoxes() : this(new Rectangle(0, 0, 308, 60), 3)
        {
        }
        public GroupTextBoxes(Rectangle rect, int boxCount) : this(rect, boxCount, 2)
        {
        }
        public GroupTextBoxes(Rectangle rect, int boxCount, int lining) : this(rect, boxCount, lining, 20)
        {
        }
        public GroupTextBoxes(Rectangle rect, int boxCount, int outline, int fontSize) 
        {
            box = rect;
            boxes = new TextBox[boxCount];

            float height = box.height - (outline * 2);
            float width = (box.width - (outline * (1 + boxCount))) / boxCount;
            float xPos = box.x + outline;

            for (int x = 0; x < boxCount; ++x)
            {
                boxes[x] = new TextBox(new Rectangle(xPos, box.y + outline, width, height), fontSize, TextAnchor.Middle);
                xPos += width + outline;
            }
        }
        #endregion

        public void Draw()
        {
            Raylib.DrawRectangleRec(box, outlineColor);

            for (int x = 0; x < boxes.Length; ++x)
            {
                boxes[x].Draw();
            }
        }

        public void UpdateText(int element, string text)
        {
            boxes[element].UpdateText(text);
        }
    }
}
