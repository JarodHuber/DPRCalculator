using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace DPRCalculator
{
    class Calculator
    {
        // Input ————————————————————————————————————————————————————————————————————————————————————————————————————————————————————
        private OutlinedInteractionBox _targetAC = new OutlinedInteractionBox(new Rectangle(50, 50, 50, 50), 2, 20, "13", TextBoxDataType.Numeric);
        private OutlinedInteractionBox _attackCount = new OutlinedInteractionBox(new Rectangle(310, 110, 50, 50), 2, 20, "1", TextBoxDataType.Numeric);
        private Attack _mainAttack = new Attack(new Rectangle(50, 110, 250, 50), 2);
        private OutlinedInteractionBox _perTurnDamage = new OutlinedInteractionBox(new Rectangle(50, 180, 250, 50), 2, 20, "", TextAnchor.Left, TextBoxDataType.Dice);

        private LabeledToggle _dprToggle = new LabeledToggle("DPR", 12, 20, new Circle(840, 30, 10), true);
        private LabeledToggle _dprAToggle = new LabeledToggle("ADV", 12, 20, new Circle(910, 30, 10), true);
        private LabeledToggle _dprDToggle = new LabeledToggle("DIS", 12, 20, new Circle(980, 30, 10), true);

        private LabeledToggle _powToggle = new LabeledToggle("POW", 12, 20, new Circle(1080, 30, 10));
        private LabeledToggle _powAToggle = new LabeledToggle("POW ADV", 12, 20, new Circle(1150, 30, 10));
        private LabeledToggle _powDToggle = new LabeledToggle("POW DIS", 12, 20, new Circle(1270, 30, 10));

        // Output ————————————————————————————————————————————————————————————————————————————————————————————————————————————————————
        private OutlinedTextBox _damageText;
        private OutlinedTextBox _damagePerHitText;
        private OutlinedTextBox _damagePerCritText;
        private OutlinedTextBox _hitChanceText;
        private OutlinedTextBox _critChanceText;
        private OutlinedTextBox _hitChanceOnceText;
        private OutlinedTextBox _critChanceOnceText;
        private OutlinedTextBox _powText;
        private OutlinedTextBox _powDifferenceText;
        private OutlinedTextBox _powHitChanceText;
        private OutlinedTextBox _powHitChanceOnceText;

        private GroupTextBoxes _damageOutput;
        private GroupTextBoxes _damagePerHit;
        private GroupTextBoxes _damagePerCrit;
        private GroupTextBoxes _hitChanceBox;
        private GroupTextBoxes _critChanceBox;
        private GroupTextBoxes _chanceToHitOnce;
        private GroupTextBoxes _chanceToCritOnce;

        private GroupTextBoxes _powerAttack;
        private GroupTextBoxes _powDifference;
        private GroupTextBoxes _powToHit;
        private GroupTextBoxes _powToHitOnce;


        private Graph _dprGraph = new Graph(new Rectangle(870, 70, 600, 300), new Color(66, 133, 244, 255), 3, 26, 5, 30, 0, 10);
        private Graph _dprAGraph = new Graph(new Rectangle(870, 70, 600, 300), new Color(219, 68, 55, 255), 3, 26, 5, 30, 0, 10);
        private Graph _dprDGraph = new Graph(new Rectangle(870, 70, 600, 300), new Color(244, 180, 0, 255), 3, 26, 5, 30, 0, 10);

        private Graph _powGraph = new Graph(new Rectangle(870, 70, 600, 300), new Color(106, 168, 79, 255), 3, 26, 5, 30, 0, 10);
        private Graph _powAGraph = new Graph(new Rectangle(870, 70, 600, 300), new Color(106, 168, 79, 255), 3, 26, 5, 30, 0, 10);
        private Graph _powDGraph = new Graph(new Rectangle(870, 70, 600, 300), new Color(106, 168, 79, 255), 3, 26, 5, 30, 0, 10);

        private int _ac = 10;

        private int _attackBonus = 0;
        private float _baseDamage;
        private float _critBonus;
        private float _perTurnBaseDamage;
        private float _perTurnCritDamage;
        private int _numOfAttacks = 1;

        private int _critRange = 20;
        private int _whifRange = 1;

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
            get => (21.0f - _critRange) / 20.0f;
        }
        private float WhifChance
        {
            get => _whifRange / 20.0f;
        }

        private IInteractable[] interactables;

        public Calculator()
        {
            InitOutputBoxes();

            interactables = new IInteractable[]
            {
                _targetAC,
                _mainAttack,
                _attackCount,
                _perTurnDamage,

                _dprToggle,
                _dprAToggle,
                _dprDToggle,

                _powToggle,
                _powAToggle,
                _powDToggle,
            };
        }
        // Methods ————————————————————————————————————————————————————————————————————————————————————————————————————————————————————
        private void InitOutputBoxes()
        {
            float labelHeight = 30;
            float dataHeight = 50;
            float outlineOffset = 4;

            Rectangle rect = new Rectangle(500, 0, 308, labelHeight);

            _damageText = new OutlinedTextBox(rect, 2, 20,
                new Color(246, 178, 107, 255), Color.BLACK, Color.BLACK,
                "Damage Per Round", TextAnchor.Center);

            rect.height = dataHeight;
            rect.y += labelHeight - outlineOffset;

            _damageOutput = new GroupTextBoxes(rect, 3);

            rect.height = labelHeight;
            rect.y += dataHeight - outlineOffset;

            _damagePerHitText = new OutlinedTextBox(rect, 2, 20,
                new Color(246, 178, 107, 255), Color.BLACK, Color.BLACK,
                "Damage Per Hit", TextAnchor.Center);

            rect.height = dataHeight;
            rect.y += labelHeight - outlineOffset;

            _damagePerHit = new GroupTextBoxes(rect, 3);

            rect.height = labelHeight;
            rect.y += dataHeight - outlineOffset;

            _damagePerCritText = new OutlinedTextBox(rect, 2, 20,
                new Color(246, 178, 107, 255), Color.BLACK, Color.BLACK,
                "Damage Per Crit", TextAnchor.Center);

            rect.height = dataHeight;
            rect.y += labelHeight - outlineOffset;

            _damagePerCrit = new GroupTextBoxes(rect, 3);

            rect.height = labelHeight;
            rect.y += dataHeight - outlineOffset;

            _hitChanceText = new OutlinedTextBox(rect, 2, 20,
                new Color(246, 178, 107, 255), Color.BLACK, Color.BLACK,
                "Hit Chance", TextAnchor.Center);

            rect.height = dataHeight;
            rect.y += labelHeight - outlineOffset;

            _hitChanceBox = new GroupTextBoxes(rect, 3);

            rect.height = labelHeight;
            rect.y += dataHeight - outlineOffset;

            _critChanceText = new OutlinedTextBox(rect, 2, 20,
                  new Color(246, 178, 107, 255), Color.BLACK, Color.BLACK,
                  "Crit Chance", TextAnchor.Center);

            rect.height = dataHeight;
            rect.y += labelHeight - outlineOffset;

            _critChanceBox = new GroupTextBoxes(rect, 3);

            rect.height = labelHeight;
            rect.y += dataHeight - outlineOffset;

            _hitChanceOnceText = new OutlinedTextBox(rect, 2, 20,
                new Color(246, 178, 107, 255), Color.BLACK, Color.BLACK,
                "Chance to Hit Once", TextAnchor.Center);

            rect.height = dataHeight;
            rect.y += labelHeight - outlineOffset;

            _chanceToHitOnce = new GroupTextBoxes(rect, 3);

            rect.height = labelHeight;
            rect.y += dataHeight - outlineOffset;

            _critChanceOnceText = new OutlinedTextBox(rect, 2, 20,
                new Color(246, 178, 107, 255), Color.BLACK, Color.BLACK,
                "Chance to Crit Once", TextAnchor.Center);

            rect.height = dataHeight;
            rect.y += labelHeight - outlineOffset;

            _chanceToCritOnce = new GroupTextBoxes(rect, 3);

            rect.height = labelHeight;
            rect.y += dataHeight - outlineOffset;

            _powText = new OutlinedTextBox(rect, 2, 20,
               new Color(106, 168, 79, 255), Color.BLACK, Color.BLACK,
               "Power Attack (-5 +10)", TextAnchor.Center);

            rect.height = dataHeight;
            rect.y += labelHeight - outlineOffset;

            _powerAttack = new GroupTextBoxes(rect, 3);

            rect.height = labelHeight;
            rect.y += dataHeight - outlineOffset;

            _powDifferenceText = new OutlinedTextBox(rect, 2, 20,
                new Color(106, 168, 79, 255), Color.BLACK, Color.BLACK,
                "Pow Difference", TextAnchor.Center);

            rect.height = dataHeight;
            rect.y += labelHeight - outlineOffset;

            _powDifference = new GroupTextBoxes(rect, 3);

            rect.height = labelHeight;
            rect.y += dataHeight - outlineOffset;

            _powHitChanceText = new OutlinedTextBox(rect, 2, 20,
                 new Color(106, 168, 79, 255), Color.BLACK, Color.BLACK,
                 "Pow Hit Chance", TextAnchor.Center);

            rect.height = dataHeight;
            rect.y += labelHeight - outlineOffset;

            _powToHit = new GroupTextBoxes(rect, 3);

            rect.height = labelHeight;
            rect.y += dataHeight - outlineOffset;

            _powHitChanceOnceText = new OutlinedTextBox(rect, 2, 20,
                new Color(106, 168, 79, 255), Color.BLACK, Color.BLACK,
                "Pow Chance to Hit Once", TextAnchor.Center);

            rect.height = dataHeight;
            rect.y += labelHeight - outlineOffset;

            _powToHitOnce = new GroupTextBoxes(rect, 3);
        }

        public void Update()
        {
            // Update TextBoxes ————————————————————————————————————————————————————————————————————————————————————————————————————————————————————
            bool interactionTest = Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON);
            for (int x = 0; x < interactables.Length; ++x)
            {
                interactables[x].Update();

                if (interactionTest)
                    interactionTest = !interactables[x].AttemptInteract();
            }

            // Parse TextBox data to numbers ————————————————————————————————————————————————————————————————————————————————————————————————————————————————————
            if (_targetAC.Text == "")
                _ac = 0;
            else if (!int.TryParse(_targetAC.Text, out _ac))
                Console.WriteLine("AC is not an int / cannot be parsed");

            if (_attackCount.Text == "")
                _numOfAttacks = 0;
            else if (!int.TryParse(_attackCount.Text, out _numOfAttacks))
                Console.WriteLine("Attack number is not an int / cannot be parsed");

            _critBonus = 0;
            _mainAttack.GetData(out _attackBonus, out _baseDamage, ref _critBonus);

            _perTurnCritDamage = 0;
            _perTurnBaseDamage = (float)ExpressionEvaluator.Eval(_perTurnDamage.Text, ref _perTurnCritDamage);

            // Calculate ————————————————————————————————————————————————————————————————————————————————————————————————————————————————————
            float hitChance = ((21.0f - _ac) + _attackBonus) / 20.0f;
            hitChance = Utils.Clamp(hitChance, CritChance, 1 - WhifChance);

            float hitChancePow = ((21.0f - _ac) + (_attackBonus - 5)) / 20.0f;
            hitChancePow = Utils.Clamp(hitChancePow, CritChance, 1 - WhifChance);

            float dpr = (((_baseDamage * hitChance) + (_critBonus * CritChance)) * _numOfAttacks);
            dpr += ((1 - MathF.Pow(1 - hitChance, _numOfAttacks)) * _perTurnBaseDamage);
            dpr += ((1 - MathF.Pow(1 - hitChance, _numOfAttacks)) * (CritChance / hitChance) * _perTurnCritDamage);
            float dprA = ((_baseDamage * Advantage(hitChance)) + (_critBonus * Advantage(CritChance))) * _numOfAttacks;
            dprA += ((1 - MathF.Pow(1 - Advantage(hitChance), _numOfAttacks)) * _perTurnBaseDamage);
            dprA += ((1 - MathF.Pow(1 - Advantage(hitChance), _numOfAttacks)) * (Advantage(CritChance) / Advantage(hitChance)) * _perTurnCritDamage);
            float dprD = ((_baseDamage * Disadvantage(hitChance)) + (_critBonus * Disadvantage(CritChance))) * _numOfAttacks;
            dprD += ((1 - MathF.Pow(1 - Disadvantage(hitChance), _numOfAttacks)) * _perTurnBaseDamage);
            dprD += ((1 - MathF.Pow(1 - Disadvantage(hitChance), _numOfAttacks)) * (Disadvantage(CritChance) / Disadvantage(hitChance)) * _perTurnCritDamage);

            float pow = (((_baseDamage + 10) * hitChancePow) + (_critBonus * CritChance)) * _numOfAttacks;
            pow += ((1 - MathF.Pow(1 - hitChancePow, _numOfAttacks)) * _perTurnBaseDamage);
            pow += ((1 - MathF.Pow(1 - hitChancePow, _numOfAttacks)) * (CritChance / hitChancePow) * _perTurnCritDamage);
            float powA = (((_baseDamage + 10) * Advantage(hitChancePow)) + (_critBonus * Advantage(CritChance))) * _numOfAttacks;
            powA += ((1 - MathF.Pow(1 - Advantage(hitChancePow), _numOfAttacks)) * _perTurnBaseDamage);
            powA += ((1 - MathF.Pow(1 - Advantage(hitChancePow), _numOfAttacks)) * (Advantage(CritChance) / Advantage(hitChancePow)) * _perTurnCritDamage);
            float powD = (((_baseDamage + 10) * Disadvantage(hitChancePow)) + (_critBonus * Disadvantage(CritChance))) * _numOfAttacks;
            powD += ((1 - MathF.Pow(1 - Disadvantage(hitChancePow), _numOfAttacks)) * _perTurnBaseDamage);
            powD += ((1 - MathF.Pow(1 - Disadvantage(hitChancePow), _numOfAttacks)) * (Disadvantage(CritChance) / Advantage(hitChancePow)) * _perTurnCritDamage);

            float critMax = 0;
            float critMin = 0;
            double maxDamage = ExpressionEvaluator.Eval(_mainAttack.GetDamageFormula(), ref critMax, DieCalc.Max);
            double minDamage = ExpressionEvaluator.Eval(_mainAttack.GetDamageFormula(), ref critMin, DieCalc.Min);

            // Update TextBoxes ————————————————————————————————————————————————————————————————————————————————————————————————————————————————————
            _damageOutput.UpdateText(0, dpr.ToString("0.####"));
            _damageOutput.UpdateText(1, dprA.ToString("0.####"));
            _damageOutput.UpdateText(2, dprD.ToString("0.####"));

            _damagePerHit.UpdateText(0, _baseDamage.ToString());
            _damagePerHit.UpdateText(1, maxDamage.ToString());
            _damagePerHit.UpdateText(2, minDamage.ToString());

            _damagePerCrit.UpdateText(0, (_baseDamage + _critBonus).ToString());
            _damagePerCrit.UpdateText(1, (maxDamage + critMax).ToString());
            _damagePerCrit.UpdateText(2, (minDamage + critMin).ToString());

            _hitChanceBox.UpdateText(0, hitChance.ToString(".####"));
            _hitChanceBox.UpdateText(1, Advantage(hitChance).ToString(".####"));
            _hitChanceBox.UpdateText(2, Disadvantage(hitChance).ToString(".####"));

            _critChanceBox.UpdateText(0, CritChance.ToString(".####"));
            _critChanceBox.UpdateText(1, Advantage(CritChance).ToString(".####"));
            _critChanceBox.UpdateText(2, Disadvantage(CritChance).ToString(".####"));

            _chanceToHitOnce.UpdateText(0, (1 - MathF.Pow(1 - hitChance, _numOfAttacks)).ToString(".####"));
            _chanceToHitOnce.UpdateText(1, (1 - MathF.Pow(1 - Advantage(hitChance), _numOfAttacks)).ToString(".####"));
            _chanceToHitOnce.UpdateText(2, (1 - MathF.Pow(1 - Disadvantage(hitChance), _numOfAttacks)).ToString(".####"));

            _chanceToCritOnce.UpdateText(0, (1 - MathF.Pow(1 - CritChance, _numOfAttacks)).ToString(".####"));
            _chanceToCritOnce.UpdateText(1, (1 - MathF.Pow(1 - Advantage(CritChance), _numOfAttacks)).ToString(".####"));
            _chanceToCritOnce.UpdateText(2, (1 - MathF.Pow(1 - Disadvantage(CritChance), _numOfAttacks)).ToString(".####"));

            _powerAttack.UpdateText(0, pow.ToString("0.####"));
            _powerAttack.UpdateText(1, powA.ToString("0.####"));
            _powerAttack.UpdateText(2, powD.ToString("0.####"));

            _powDifference.UpdateText(0, (pow - dpr).ToString("0.####"));
            _powDifference.UpdateText(1, (powA - dprA).ToString("0.####"));
            _powDifference.UpdateText(2, (powD - dprD).ToString("0.####"));

            _powToHit.UpdateText(0, hitChancePow.ToString(".####"));
            _powToHit.UpdateText(1, Advantage(hitChancePow).ToString(".####"));
            _powToHit.UpdateText(2, Disadvantage(hitChancePow).ToString(".####"));

            _powToHitOnce.UpdateText(0, (1 - MathF.Pow(1 - hitChancePow, _numOfAttacks)).ToString(".####"));
            _powToHitOnce.UpdateText(1, (1 - MathF.Pow(1 - Advantage(hitChancePow), _numOfAttacks)).ToString(".####"));
            _powToHitOnce.UpdateText(2, (1 - MathF.Pow(1 - Disadvantage(hitChancePow), _numOfAttacks)).ToString(".####"));

            _maxGraphSizes[0] = 0.0f;
            _maxGraphSizes[1] = 0.0f;
            _maxGraphSizes[2] = 0.0f;
            _maxGraphSizes[3] = 0.0f;
            _maxGraphSizes[4] = 0.0f;
            _maxGraphSizes[5] = 0.0f;

            // Graphs ————————————————————————————————————————————————————————————————————————————————————————————————————————————————————
            for (int i = 0; i < 26; ++i)
            {
                // Set X axis for graphs (AC) ————————————————————————————————————————————————————————————————————————————————————————————————————————————————————
                float x = 5 + i;

                _dprByAC[i].x = x;
                _dprAByAC[i].x = x;
                _dprDByAC[i].x = x;

                _powByAC[i].x = x;
                _powAByAC[i].x = x;
                _powDByAC[i].x = x;

                // Calculate DPRs at the specified AC ————————————————————————————————————————————————————————————————————————————————————————————————————————————————————
                hitChance = ((21.0f - x) + _attackBonus) / 20.0f;
                hitChance = Utils.Clamp(hitChance, CritChance, 1 - WhifChance);

                hitChancePow = ((21.0f - x) + (_attackBonus - 5)) / 20.0f;
                hitChancePow = Utils.Clamp(hitChancePow, CritChance, 1 - WhifChance);

                dpr = (((_baseDamage * hitChance) + (_critBonus * CritChance)) * _numOfAttacks);
                dpr += ((1 - MathF.Pow(1 - hitChance, _numOfAttacks)) * _perTurnBaseDamage);
                dpr += ((1 - MathF.Pow(1 - hitChance, _numOfAttacks)) * (CritChance / hitChance) * _perTurnCritDamage);

                dprA = ((_baseDamage * Advantage(hitChance)) + (_critBonus * Advantage(CritChance))) * _numOfAttacks;
                dprA += ((1 - MathF.Pow(1 - Advantage(hitChance), _numOfAttacks)) * _perTurnBaseDamage);
                dprA += ((1 - MathF.Pow(1 - Advantage(hitChance), _numOfAttacks)) * (Advantage(CritChance) / Advantage(hitChance)) * _perTurnCritDamage);

                dprD = ((_baseDamage * Disadvantage(hitChance)) + (_critBonus * Disadvantage(CritChance))) * _numOfAttacks;
                dprD += ((1 - MathF.Pow(1 - Disadvantage(hitChance), _numOfAttacks)) * _perTurnBaseDamage);
                dprD += ((1 - MathF.Pow(1 - Disadvantage(hitChance), _numOfAttacks)) * (Disadvantage(CritChance) / Disadvantage(hitChance)) * _perTurnCritDamage);

                pow = (((_baseDamage + 10) * hitChancePow) + (_critBonus * CritChance)) * _numOfAttacks;
                pow += ((1 - MathF.Pow(1 - hitChancePow, _numOfAttacks)) * _perTurnBaseDamage);
                pow += ((1 - MathF.Pow(1 - hitChancePow, _numOfAttacks)) * (CritChance / hitChancePow) * _perTurnCritDamage);

                powA = (((_baseDamage + 10) * Advantage(hitChancePow)) + (_critBonus * Advantage(CritChance))) * _numOfAttacks;
                powA += ((1 - MathF.Pow(1 - Advantage(hitChancePow), _numOfAttacks)) * _perTurnBaseDamage);
                powA += ((1 - MathF.Pow(1 - Advantage(hitChancePow), _numOfAttacks)) * (Advantage(CritChance) / Advantage(hitChancePow)) * _perTurnCritDamage);

                powD = (((_baseDamage + 10) * Disadvantage(hitChancePow)) + (_critBonus * Disadvantage(CritChance))) * _numOfAttacks;
                powD += ((1 - MathF.Pow(1 - Disadvantage(hitChancePow), _numOfAttacks)) * _perTurnBaseDamage);
                powD += ((1 - MathF.Pow(1 - Disadvantage(hitChancePow), _numOfAttacks)) * (Disadvantage(CritChance) / Advantage(hitChancePow)) * _perTurnCritDamage);

                // Set Y axis for graphs (DPR) ————————————————————————————————————————————————————————————————————————————————————————————————————————————————————
                _dprByAC[i].y = dpr;
                _dprAByAC[i].y = dprA;
                _dprDByAC[i].y = dprD;

                _powByAC[i].y = pow;
                _powAByAC[i].y = powA;
                _powDByAC[i].y = powD;

                // Set max graph size for each graph ————————————————————————————————————————————————————————————————————————————————————————————————————————————————————
                _maxGraphSizes[0] = MathF.Max(_maxGraphSizes[0], dpr);
                _maxGraphSizes[1] = MathF.Max(_maxGraphSizes[1], dprA);
                _maxGraphSizes[2] = MathF.Max(_maxGraphSizes[2], dprD);
                _maxGraphSizes[3] = MathF.Max(_maxGraphSizes[3], pow);
                _maxGraphSizes[4] = MathF.Max(_maxGraphSizes[4], powA);
                _maxGraphSizes[5] = MathF.Max(_maxGraphSizes[5], powD);
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
            // Draw Interactables ————————————————————————————————————————————————————————————————————————————————————————————————————————————————————
            for (int x = 0; x < interactables.Length; ++x)
                interactables[x].Draw();

            // Draw Outputs ————————————————————————————————————————————————————————————————————————————————————————————————————————————————————
            _damageText.Draw();
            _damageOutput.Draw();

            _damagePerHitText.Draw();
            _damagePerHit.Draw();
            _damagePerCritText.Draw();
            _damagePerCrit.Draw();

            _hitChanceText.Draw();
            _hitChanceBox.Draw();
            _critChanceText.Draw();
            _critChanceBox.Draw();

            _hitChanceOnceText.Draw();
            _chanceToHitOnce.Draw();
            _critChanceOnceText.Draw();
            _chanceToCritOnce.Draw();

            _powText.Draw();
            _powerAttack.Draw();
            _powDifferenceText.Draw();
            _powDifference.Draw();
            _powHitChanceText.Draw();
            _powToHit.Draw();
            _powHitChanceOnceText.Draw();
            _powToHitOnce.Draw();

            // Draw graphs ————————————————————————————————————————————————————————————————————————————————————————————————————————————————————
            Raylib.DrawRectangleRec(new Rectangle(830, 50, 650, 360), Color.RAYWHITE);
            Raylib.DrawLine(870, 50, 870, 370, Color.BLACK);
            Raylib.DrawLine(870, 370, 1480, 370, Color.BLACK);

            // Draw graph x axis
            for(int x = 5; x <= 30; ++x)
            {
                int textSize = (int)(Raylib.MeasureText(x.ToString(), 16) / 2.0f);
                Raylib.DrawText(x.ToString(), 870 + (int)(600.0f * ((x - 5) / 25.0f)) - textSize, 375, 16, Color.BLACK);
            }

            int data = Graph.RoundGraphSize((int)GraphScale, out float scale, out float magnitude);

            // Draw graph y axis
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

            // Draw graph lines
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
