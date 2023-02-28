using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace DPRCalculator
{
    class Attack : Interactable
    {
        private Rectangle box = new Rectangle(0, 0, 265, 60);

        private Color outlineColor = Color.BLACK;

        private InteractionBox attackBox;
        private InteractionBox damageBox;

        #region Constructors
        public Attack() : this(new Rectangle(0, 0, 265, 60))
        {
        }
        public Attack(Rectangle rect) : this(rect, 2)
        {
        }
        public Attack(Rectangle rect, int lining) : this(rect, lining, 20)
        {
        }
        public Attack(Rectangle rect, int outline, int fontSize)
        {
            box = rect;
            float height = box.height - (outline * 2);

            attackBox = new InteractionBox(new Rectangle(box.x + outline, box.y + outline, height, height), fontSize, "5", TextBoxDataType.Numeric);
            damageBox = new InteractionBox(new Rectangle(box.x + (outline * 2) + height, box.y + outline, box.width - (outline * 3) - height, height), fontSize, TextAnchor.Left, TextBoxDataType.Dice);
        }
        #endregion

        public void Update()
        {
            attackBox.Update();
            damageBox.Update();
        }

        public void Draw()
        {
            Raylib.DrawRectangleRec(box, outlineColor);

            attackBox.Draw();
            damageBox.Draw();
        }

        public bool AttemptInteract()
        {
            return attackBox.AttemptInteract() || damageBox.AttemptInteract();
        }

        public void GetData(out int attackBonus, out float damage, ref float critBonus)
        {
            if (attackBox.Text == "")
                attackBonus = 0;
            else if (!int.TryParse(attackBox.Text, out attackBonus))
                Console.WriteLine("can't parse attack data");

            damage = (float)ExpressionEvaluator.Eval(damageBox.Text, ref critBonus);
        }

        public string GetDamageFormula()
        {
            return damageBox.Text;
        }
    }
}
