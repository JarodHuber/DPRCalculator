using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace DPRCalculator
{
    class Calculator
    {
        // Input
        private OutlinedInteractionBox _targetAC = new OutlinedInteractionBox(new Rectangle(50, 50, 50, 50), 2, 20, "13", TextBoxDataType.Numeric);
        private OutlinedInteractionBox _attackCount = new OutlinedInteractionBox(new Rectangle(310, 110, 50, 50), 2, 20, "1", TextBoxDataType.Numeric);
        private Attack _mainAttack = new Attack(new Rectangle(50, 110, 250, 50), 2);

        private Toggle _dprToggle = new Toggle(new Circle(910, 30, 10), 2, true);
        private Toggle _dprAToggle = new Toggle(new Circle(1010, 30, 10), 2, true);
        private Toggle _dprDToggle = new Toggle(new Circle(1110, 30, 10), 2, true);

        private Toggle _powToggle = new Toggle(new Circle(1210, 30, 10), 2);
        private Toggle _powAToggle = new Toggle(new Circle(1310, 30, 10), 2);
        private Toggle _powDToggle = new Toggle(new Circle(1410, 30, 10), 2);

        // Output
        private GroupTextBoxes _damageOutput = new GroupTextBoxes(new Rectangle(500, 50, 308, 60), 3);

        private GroupTextBoxes _damagePerHit = new GroupTextBoxes(new Rectangle(500, 105, 308, 60), 3);
        private GroupTextBoxes _damagePerCrit = new GroupTextBoxes(new Rectangle(500, 160, 308, 60), 3);

        private GroupTextBoxes _hitChanceBox = new GroupTextBoxes(new Rectangle(500, 215, 308, 60), 3);
        private GroupTextBoxes _critChanceBox = new GroupTextBoxes(new Rectangle(500, 270, 308, 60), 3);

        private GroupTextBoxes _chanceToHitOnce = new GroupTextBoxes(new Rectangle(500, 325, 308, 60), 3);
        private GroupTextBoxes _chanceToCritOnce = new GroupTextBoxes(new Rectangle(500, 380, 308, 60), 3);

        private GroupTextBoxes _powerAttack = new GroupTextBoxes(new Rectangle(500, 435, 308, 60), 3);
        private GroupTextBoxes _powDifference = new GroupTextBoxes(new Rectangle(500, 490, 308, 60), 3);
        private GroupTextBoxes _powToHit = new GroupTextBoxes(new Rectangle(500, 545, 308, 60), 3);
        private GroupTextBoxes _powToHitOnce = new GroupTextBoxes(new Rectangle(500, 600, 308, 60), 3);

        private Graph _dprGraph = new Graph(new Rectangle(870, 70, 600, 300), new Color(66, 133, 244, 255), 3, 26, 5, 30, 0, 10);
        private Graph _dprAGraph = new Graph(new Rectangle(870, 70, 600, 300), new Color(219, 68, 55, 255), 3, 26, 5, 30, 0, 10);
        private Graph _dprDGraph = new Graph(new Rectangle(870, 70, 600, 300), new Color(244, 180, 0, 255), 3, 26, 5, 30, 0, 10);

        private Graph _powGraph = new Graph(new Rectangle(870, 70, 600, 300), new Color(106, 168, 79, 255), 3, 26, 5, 30, 0, 10);
        private Graph _powAGraph = new Graph(new Rectangle(870, 70, 600, 300), new Color(106, 168, 79, 255), 3, 26, 5, 30, 0, 10);
        private Graph _powDGraph = new Graph(new Rectangle(870, 70, 600, 300), new Color(106, 168, 79, 255), 3, 26, 5, 30, 0, 10);

        private int AC = 10;

        private int attackBonus = 0;
        private float baseDamage;
        private float critBonus;
        private int numOfAttacks = 1;

        private int critRange = 20;
        private int whifRange = 1;

        private Vector2[] _dprByAC =  new Vector2[26];
        private Vector2[] _dprAByAC = new Vector2[26];
        private Vector2[] _dprDByAC = new Vector2[26];
        private Vector2[] _powByAC =  new Vector2[26];
        private Vector2[] _powAByAC = new Vector2[26];
        private Vector2[] _powDByAC = new Vector2[26];

        private float[] _maxGraphSizes = new float[6];

        private float GraphScale
        {
            get
            {
                float toReturn = 0;

                if (_dprToggle.GetData())
                    toReturn = MathF.Max(toReturn, _maxGraphSizes[0]);
                if (_dprAToggle.GetData())
                    toReturn = MathF.Max(toReturn, _maxGraphSizes[1]);
                if (_dprDToggle.GetData())
                    toReturn = MathF.Max(toReturn, _maxGraphSizes[2]);

                if (_powToggle.GetData())
                    toReturn = MathF.Max(toReturn, _maxGraphSizes[3]);
                if (_powAToggle.GetData())
                    toReturn = MathF.Max(toReturn, _maxGraphSizes[4]);
                if (_powDToggle.GetData())
                    toReturn = MathF.Max(toReturn, _maxGraphSizes[5]);

                return toReturn;
            }
        }

        private float CritChance
        {
            get => (21.0f - critRange) / 20.0f;
        }
        private float WhifChance
        {
            get => whifRange / 20.0f;
        }

        private Interactable[] interactables;

        public Calculator()
        {
            interactables = new Interactable[]
            {
                _targetAC,
                _mainAttack,
                _attackCount,

                _dprToggle,
                _dprAToggle,
                _dprDToggle,

                _powToggle,
                _powAToggle,
                _powDToggle,
            };
        }

        public void Update()
        {
            // Update TextBoxes
            bool interactionTest = Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON);
            for (int x = 0; x < interactables.Length; ++x)
            {
                interactables[x].Update();

                if (interactionTest)
                    interactionTest = !interactables[x].AttemptInteract();
            }

            // Parse TextBox data to numbers
            if (_targetAC.Text == "")
                AC = 0;
            else if (!int.TryParse(_targetAC.Text, out AC))
                Console.WriteLine("AC is not an int / cannot be parsed");

            if (_attackCount.Text == "")
                numOfAttacks = 0;
            else if (!int.TryParse(_attackCount.Text, out numOfAttacks))
                Console.WriteLine("Attack number is not an int / cannot be parsed");

            critBonus = 0;
            _mainAttack.GetData(out attackBonus, out baseDamage, ref critBonus);

            // Calculate
            float hitChance = ((21.0f - AC) + attackBonus) / 20.0f;
            hitChance = Utils.Clamp(hitChance, CritChance, 1 - WhifChance);

            float dpr = ((baseDamage * hitChance) + (critBonus * CritChance)) * numOfAttacks;
            float dprA = ((baseDamage * Advantage(hitChance)) + (critBonus * Advantage(CritChance))) * numOfAttacks;
            float dprD = ((baseDamage * Disadvantage(hitChance)) + (critBonus * Disadvantage(CritChance))) * numOfAttacks;

            float critMax = 0;
            float critMin = 0;
            double maxDamage = ExpressionEvaluator.Eval(_mainAttack.GetDamageFormula(), ref critMax, DieCalc.Max);
            double minDamage = ExpressionEvaluator.Eval(_mainAttack.GetDamageFormula(), ref critMin, DieCalc.Min);

            float hitChancePow = ((21.0f - AC) + (attackBonus - 5)) / 20.0f;
            hitChancePow = Utils.Clamp(hitChancePow, CritChance, 1 - WhifChance);

            float pow = (((baseDamage + 10) * hitChancePow) + (critBonus * CritChance)) * numOfAttacks;
            float powA = (((baseDamage + 10) * Advantage(hitChancePow)) + (critBonus * Advantage(CritChance))) * numOfAttacks;
            float powD = (((baseDamage + 10) * Disadvantage(hitChancePow)) + (critBonus * Disadvantage(CritChance))) * numOfAttacks;

            // Update TextBoxes
            _damageOutput.UpdateText(0, dpr.ToString("0.####"));
            _damageOutput.UpdateText(1, dprA.ToString("0.####"));
            _damageOutput.UpdateText(2, dprD.ToString("0.####"));

            _damagePerHit.UpdateText(0, baseDamage.ToString());
            _damagePerHit.UpdateText(1, maxDamage.ToString());
            _damagePerHit.UpdateText(2, minDamage.ToString());

            _damagePerCrit.UpdateText(0, (baseDamage + critBonus).ToString());
            _damagePerCrit.UpdateText(1, (maxDamage + critMax).ToString());
            _damagePerCrit.UpdateText(2, (minDamage + critMin).ToString());

            _hitChanceBox.UpdateText(0, hitChance.ToString(".####"));
            _hitChanceBox.UpdateText(1, Advantage(hitChance).ToString(".####"));
            _hitChanceBox.UpdateText(2, Disadvantage(hitChance).ToString(".####"));

            _critChanceBox.UpdateText(0, CritChance.ToString(".####"));
            _critChanceBox.UpdateText(1, Advantage(CritChance).ToString(".####"));
            _critChanceBox.UpdateText(2, Disadvantage(CritChance).ToString(".####"));

            _chanceToHitOnce.UpdateText(0, (1 - MathF.Pow(1 - hitChance, numOfAttacks)).ToString(".####"));
            _chanceToHitOnce.UpdateText(1, (1 - MathF.Pow(1 - Advantage(hitChance), numOfAttacks)).ToString(".####"));
            _chanceToHitOnce.UpdateText(2, (1 - MathF.Pow(1 - Disadvantage(hitChance), numOfAttacks)).ToString(".####"));

            _chanceToCritOnce.UpdateText(0, (1 - MathF.Pow(1 - CritChance, numOfAttacks)).ToString(".####"));
            _chanceToCritOnce.UpdateText(1, (1 - MathF.Pow(1 - Advantage(CritChance), numOfAttacks)).ToString(".####"));
            _chanceToCritOnce.UpdateText(2, (1 - MathF.Pow(1 - Disadvantage(CritChance), numOfAttacks)).ToString(".####"));

            _powerAttack.UpdateText(0, pow.ToString("0.####"));
            _powerAttack.UpdateText(1, powA.ToString("0.####"));
            _powerAttack.UpdateText(2, powD.ToString("0.####"));

            _powDifference.UpdateText(0, (pow - dpr).ToString("0.####"));
            _powDifference.UpdateText(1, (powA - dprA).ToString("0.####"));
            _powDifference.UpdateText(2, (powD - dprD).ToString("0.####"));

            _powToHit.UpdateText(0, hitChancePow.ToString(".####"));
            _powToHit.UpdateText(1, Advantage(hitChancePow).ToString(".####"));
            _powToHit.UpdateText(2, Disadvantage(hitChancePow).ToString(".####"));

            _powToHitOnce.UpdateText(0, (1 - MathF.Pow(1 - hitChancePow, numOfAttacks)).ToString(".####"));
            _powToHitOnce.UpdateText(1, (1 - MathF.Pow(1 - Advantage(hitChancePow), numOfAttacks)).ToString(".####"));
            _powToHitOnce.UpdateText(2, (1 - MathF.Pow(1 - Disadvantage(hitChancePow), numOfAttacks)).ToString(".####"));

            _maxGraphSizes[0] = 0.0f;
            _maxGraphSizes[1] = 0.0f;
            _maxGraphSizes[2] = 0.0f;
            _maxGraphSizes[3] = 0.0f;
            _maxGraphSizes[4] = 0.0f;
            _maxGraphSizes[5] = 0.0f;

            // Graphs
            for (int x = 0; x < 26; ++x)
            {
                hitChance = ((21.0f - (5 + x)) + attackBonus) / 20.0f;
                hitChance = Utils.Clamp(hitChance, CritChance, 1 - WhifChance);

                _dprByAC[x].x = 5 + x;
                _dprByAC[x].y = ((baseDamage * hitChance) + (critBonus * CritChance)) * numOfAttacks;
                _dprAByAC[x].x = 5 + x;
                _dprAByAC[x].y = ((baseDamage * Advantage(hitChance)) + (critBonus * CritChance)) * numOfAttacks;
                _dprDByAC[x].x = 5 + x;
                _dprDByAC[x].y = ((baseDamage * Disadvantage(hitChance)) + (critBonus * CritChance)) * numOfAttacks;

                hitChance = ((21.0f - (5 + x)) + (attackBonus - 5)) / 20.0f;
                hitChance = Utils.Clamp(hitChance, CritChance, 1 - WhifChance);

                _powByAC[x].x = 5 + x;
                _powByAC[x].y = (((baseDamage + 10) * hitChance) + (critBonus * CritChance)) * numOfAttacks;
                _powAByAC[x].x = 5 + x;
                _powAByAC[x].y = (((baseDamage + 10) * Advantage(hitChance)) + (critBonus * CritChance)) * numOfAttacks;
                _powDByAC[x].x = 5 + x;
                _powDByAC[x].y = (((baseDamage + 10) * Disadvantage(hitChance)) + (critBonus * CritChance)) * numOfAttacks;

                _maxGraphSizes[0] = MathF.Max(_maxGraphSizes[0], _dprByAC[x].y);
                _maxGraphSizes[1] = MathF.Max(_maxGraphSizes[1], _dprAByAC[x].y);
                _maxGraphSizes[2] = MathF.Max(_maxGraphSizes[2], _dprDByAC[x].y);
                _maxGraphSizes[3] = MathF.Max(_maxGraphSizes[3], _powByAC[x].y);
                _maxGraphSizes[4] = MathF.Max(_maxGraphSizes[4], _powAByAC[x].y);
                _maxGraphSizes[5] = MathF.Max(_maxGraphSizes[5], _powDByAC[x].y);
            }

            int maxY = Graph.RoundGraphSize((int)GraphScale);

            _dprGraph.UpdateScale(maxY);
            _dprGraph.UpdatePoints(_dprByAC);

            _dprAGraph.UpdateScale(maxY);
            _dprAGraph.UpdatePoints(_dprAByAC);

            _dprDGraph.UpdateScale(maxY);
            _dprDGraph.UpdatePoints(_dprDByAC);

            _powGraph.UpdateScale(maxY);
            _powGraph.UpdatePoints(_powByAC);

            _powAGraph.UpdateScale(maxY);
            _powAGraph.UpdatePoints(_powAByAC);

            _powDGraph.UpdateScale(maxY);
            _powDGraph.UpdatePoints(_powDByAC);
        }

        public void Draw()
        {
            for (int x = 0; x < interactables.Length; ++x)
                interactables[x].Draw();

            _damageOutput.Draw();

            _damagePerHit.Draw();
            _damagePerCrit.Draw();

            _hitChanceBox.Draw();
            _critChanceBox.Draw();

            _chanceToHitOnce.Draw();
            _chanceToCritOnce.Draw();

            _powerAttack.Draw();
            _powDifference.Draw();
            _powToHit.Draw();
            _powToHitOnce.Draw();

            Raylib.DrawRectangleRec(new Rectangle(830, 50, 650, 360), Color.RAYWHITE);
            Raylib.DrawLine(870, 50, 870, 370, Color.BLACK);
            Raylib.DrawLine(870, 370, 1480, 370, Color.BLACK);

            for(int x = 5; x <= 30; ++x)
            {
                int textSize = (int)(Raylib.MeasureText(x.ToString(), 16) / 2.0f);
                Raylib.DrawText(x.ToString(), 870 + (int)(600.0f * ((x - 5) / 25.0f)) - textSize, 375, 16, Color.BLACK);
            }

            int data = Graph.RoundGraphSize((int)GraphScale, out float scale, out float magnitude);

            if (data != 0)
            {
                int iterator = 0;
                int info = data;
                while (info > 0)
                {
                    if (scale == .025f && magnitude == 100)
                        info = data - (2 * iterator);
                    else
                        info = data - (int)((scale * magnitude) * iterator);

                    info = (int)MathF.Max(info, 0);
                    int textSize = Raylib.MeasureText(info.ToString(), 16);
                    int yPos = 70 + (int)(300.0f * (1 - ((float)info / (float)data))) - 8;

                    Raylib.DrawText(info.ToString(), 865 - textSize, yPos, 16, Color.BLACK);
                    if(info != 0)
                        Raylib.DrawLineEx(new Vector2(870, yPos + 8), new Vector2(1480, yPos + 8), 4, Color.LIGHTGRAY);

                    ++iterator;
                }
            }

            if (_dprToggle.GetData())
                _dprGraph.Draw();
            if (_dprAToggle.GetData())
                _dprAGraph.Draw();
            if (_dprDToggle.GetData())
                _dprDGraph.Draw();

            if (_powToggle.GetData())
                _powGraph.Draw();
            if (_powAToggle.GetData())
                _powAGraph.Draw();
            if (_powDToggle.GetData())
                _powDGraph.Draw();
        }

        private float Advantage(float hitChance)
        {
            return 1 - MathF.Pow(1 - hitChance, 2);
        }
        private float Disadvantage(float hitChance)
        {
            return MathF.Pow(hitChance, 2);
        }
    }
}
