using Raylib_cs;

namespace DPRCalculator
{
    #region Enums
    public enum TextBoxDataType
    {
        Normal,
        AlphaNumeric,
        Numeric,
        Alpha,
        Dice,
    }
    #endregion

    class InteractionBox : TextBox, Interactable
    {
        public const string alpha = "abcdefghijklmnopqrstuvwxyz";
        public const string numeric = "0123456789";
        public const string operators = "-+/*^d";

        private TextBoxDataType type = TextBoxDataType.Normal;
        private int state = 0;
        private Timer cursorTimer = new Timer(0.5f);
        private bool cursorVisible = false;

        private string _internalText = "";
        private Color _internalColor;

        public override string Text
        {
            get => _internalText;
        }

        #region Constructors
        #region Base
        public InteractionBox() : base()
        {
        }
        public InteractionBox(Rectangle rect, int fontSize) : base(rect, fontSize)
        {
        }
        public InteractionBox(Rectangle rect, int fontSize, TextAnchor anchor) : base(rect, fontSize, anchor)
        {
        }
        public InteractionBox(Rectangle rect, int fontSize, string defaultText) : base(rect, fontSize, defaultText)
        {
            _internalText = defaultText;
        }
        public InteractionBox(Rectangle rect, int fontSize, string defaultText, TextAnchor anchor) : base(rect, fontSize, defaultText, anchor)
        {
            _internalText = defaultText;
        }
        public InteractionBox(Rectangle rect, int fontSize, Color bgColor, Color textColor) : base(rect, fontSize, bgColor, textColor)
        {
        }
        public InteractionBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, TextAnchor anchor) : base(rect, fontSize, bgColor, textColor, anchor)
        {
        }
        public InteractionBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, string defaultText) : base(rect, fontSize, bgColor, textColor, defaultText)
        {
            _internalText = defaultText;
        }
        public InteractionBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, string defaultText, TextAnchor anchor) : base(rect, fontSize, bgColor, textColor, defaultText, anchor)
        {
            _internalText = defaultText;
        }
        #endregion

        public InteractionBox(Rectangle rect, int fontSize, TextBoxDataType charTypes) : base(rect, fontSize)
        {
            type = charTypes;
        }
        public InteractionBox(Rectangle rect, int fontSize, string defaultText, TextBoxDataType charTypes) : base(rect, fontSize, defaultText)
        {
            _internalText = defaultText;
            type = charTypes;
        }
        public InteractionBox(Rectangle rect, int fontSize, TextAnchor anchor, TextBoxDataType charTypes) : base(rect, fontSize, anchor)
        {
            type = charTypes;
        }
        public InteractionBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, TextBoxDataType charTypes) : base(rect, fontSize, bgColor, textColor)
        {
            type = charTypes;
        }
        public InteractionBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, TextAnchor anchor, TextBoxDataType charTypes) : base(rect, fontSize, bgColor, textColor, anchor)
        {
            type = charTypes;
        }
        public InteractionBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, string defaultText, TextAnchor anchor, TextBoxDataType charTypes) : base(rect, fontSize, bgColor, textColor, anchor)
        {
            type = charTypes;
            _internalText = defaultText;
        }
        #endregion

        public bool AttemptInteract()
        {
            if(Raylib.CheckCollisionCircleRec(Raylib.GetMousePosition(), Program.mousePosBuffer, Bounds))
            {
                if(state == 0)
                {
                    state = _internalText == "" ? 2 : 1;
                    cursorVisible = true;
                }
                else if (state == 1)
                    state = 2;

                return true;
            }

            state = 0;
            return false;
        }
        public void Update()
        { 
            if (state == 0)
                return;

            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                if (!AttemptInteract())
                    return;
            }

            int key = Raylib.GetKeyPressed();

            if (key != 0)
            {
                cursorTimer.Reset();
                cursorVisible = false;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER)) 
            {
                state = 0;
                return;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_BACKSPACE) && _internalText != "") 
            {
                _internalText = _internalText.Remove(_internalText.Length - 1);

                if (state == 1)
                    state = 2;

                return;
            }
            if (state == 1 && Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
                state = 2;

            while (key != 0) 
            {
                switch (type)
                {
                    case TextBoxDataType.AlphaNumeric:
                        if (!alpha.Contains((char)key) && !numeric.Contains((char)key))
                            return;

                        break;
                    case TextBoxDataType.Numeric:
                        if (!numeric.Contains((char)key))
                            return;

                        break;
                    case TextBoxDataType.Alpha:
                        if (!alpha.Contains((char)key))
                            return;

                        break;
                    case TextBoxDataType.Dice:
                        if (!numeric.Contains((char)key) && !operators.Contains((char)key))
                            return;

                        break;
                }

                if (state == 1)
                {
                    _internalText = "";
                    cursorVisible = true;
                    state = 2;
                }

                _internalText += (char)key;
                key = Raylib.GetKeyPressed();
            }
        }

        public override void Draw()
        {
            UpdateText(_internalText);

            switch (state)
            {
                case 1: // Dim text color
                    _internalColor = textColor;
                    textColor.a = (byte)(255 * 0.5f);

                    base.Draw();

                    textColor = _internalColor;
                    return;
                case 2: // Display Cursor
                    if (cursorTimer.Check())
                        cursorVisible = !cursorVisible;

                    UpdateText(_internalText + (cursorVisible ? "|" : ""));

                    base.Draw();
                    return;
                default:
                    base.Draw();
                    return;
            }

        }
    }
}
