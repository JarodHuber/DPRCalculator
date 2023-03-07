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

    class InteractionBox : TextBox, IInteractable
    {
        public const string c_Alpha = "abcdefghijklmnopqrstuvwxyz";
        public const string c_Numeric = "0123456789";
        public const string c_Operators = "-+/*^d";

        private TextBoxDataType _textType = TextBoxDataType.Normal;
        private int _inputState = 0;

        private Timer _cursorTimer = new Timer(0.5f);
        private bool _cursorVisible = false;

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
        public InteractionBox(Rectangle rect, int fontSize, string defaultText) : base(rect, fontSize)
        {
            _internalText = defaultText;
        }
        public InteractionBox(Rectangle rect, int fontSize, string defaultText, TextAnchor anchor) : base(rect, fontSize, anchor)
        {
            _internalText = defaultText;
        }
        public InteractionBox(Rectangle rect, int fontSize, Color bgColor, Color textColor) : base(rect, fontSize, bgColor, textColor)
        {
        }
        public InteractionBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, TextAnchor anchor) : base(rect, fontSize, bgColor, textColor, anchor)
        {
        }
        public InteractionBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, string defaultText) : base(rect, fontSize, bgColor, textColor)
        {
            _internalText = defaultText;
        }
        public InteractionBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, string defaultText, TextAnchor anchor) : base(rect, fontSize, bgColor, textColor, anchor)
        {
            _internalText = defaultText;
        }
        #endregion

        public InteractionBox(Rectangle rect, int fontSize, TextBoxDataType charTypes) : base(rect, fontSize)
        {
            _textType = charTypes;
        }
        public InteractionBox(Rectangle rect, int fontSize, string defaultText, TextBoxDataType charTypes) : base(rect, fontSize, defaultText)
        {
            _internalText = defaultText;
            _textType = charTypes;
        }
        public InteractionBox(Rectangle rect, int fontSize, TextAnchor anchor, TextBoxDataType charTypes) : base(rect, fontSize, anchor)
        {
            _textType = charTypes;
        }
        public InteractionBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, TextBoxDataType charTypes) : base(rect, fontSize, bgColor, textColor)
        {
            _textType = charTypes;
        }
        public InteractionBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, TextAnchor anchor, TextBoxDataType charTypes) : base(rect, fontSize, bgColor, textColor, anchor)
        {
            _textType = charTypes;
        }
        public InteractionBox(Rectangle rect, int fontSize, Color bgColor, Color textColor, string defaultText, TextAnchor anchor, TextBoxDataType charTypes) : base(rect, fontSize, bgColor, textColor, anchor)
        {
            _textType = charTypes;
            _internalText = defaultText;
        }
        #endregion

        public bool AttemptInteract()
        {
            if(Raylib.CheckCollisionCircleRec(Raylib.GetMousePosition(), Program.mousePosBuffer, _rect))
            {
                if(_inputState == 0)
                {
                    _inputState = _internalText == "" ? 2 : 1;
                    _cursorVisible = true;
                }
                else if (_inputState == 1)
                    _inputState = 2;

                return true;
            }

            _inputState = 0;
            return false;
        }
        public void Update()
        { 
            if (_inputState == 0)
                return;

            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                if (!AttemptInteract())
                    return;
            }

            int key = Raylib.GetKeyPressed();

            if (key != 0)
            {
                _cursorTimer.Reset();
                _cursorVisible = false;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER)) 
            {
                _inputState = 0;
                return;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_BACKSPACE) && _internalText != "") 
            {
                _internalText = _internalText.Remove(_internalText.Length - 1);

                if (_inputState == 1)
                    _inputState = 2;

                return;
            }
            if (_inputState == 1 && Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
                _inputState = 2;

            while (key != 0) 
            {
                switch (_textType)
                {
                    case TextBoxDataType.AlphaNumeric:
                        if (!c_Alpha.Contains((char)key) && !c_Numeric.Contains((char)key))
                            return;

                        break;
                    case TextBoxDataType.Numeric:
                        if (!c_Numeric.Contains((char)key))
                            return;

                        break;
                    case TextBoxDataType.Alpha:
                        if (!c_Alpha.Contains((char)key))
                            return;

                        break;
                    case TextBoxDataType.Dice:
                        if (!c_Numeric.Contains((char)key) && !c_Operators.Contains((char)key))
                            return;

                        break;
                }

                if (_inputState == 1)
                {
                    _internalText = "";
                    _cursorVisible = true;
                    _inputState = 2;
                }

                _internalText += (char)key;
                key = Raylib.GetKeyPressed();
            }
        }

        public override void Draw()
        {
            UpdateText(_internalText);

            switch (_inputState)
            {
                case 1: // Dim text color
                    _internalColor = _textColor;
                    _textColor.a = (byte)(255 * 0.5f);

                    base.Draw();

                    _textColor = _internalColor;
                    return;
                case 2: // Display Cursor
                    if (_cursorTimer.Check())
                        _cursorVisible = !_cursorVisible;

                    UpdateText(_internalText + (_cursorVisible ? "|" : ""));

                    base.Draw();
                    return;
                default:
                    base.Draw();
                    return;
            }

        }
    }
}
