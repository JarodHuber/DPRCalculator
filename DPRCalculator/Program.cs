using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Raylib_cs;

namespace DPRCalculator
{
    class Program
    {
        public static Vector2 screenSize = new Vector2(1500, 800);
        public static Color backgroundColor = Color.GRAY;
        public const int mousePosBuffer = 5;

        private static Calculator calc= new Calculator();

        static void Main()
        {
            Initialize();

            while (!Raylib.WindowShouldClose())
            {
                Update();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.GRAY);
                Draw();
                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }

        private static void Initialize()
        {
            Raylib.InitWindow((int)screenSize.x, (int)screenSize.y, "DPR Calculator");
            Raylib.SetTargetFPS(60);
        }

        private static void Update()
        {
            calc.Update();
        }

        private static void Draw()
        {
            calc.Draw();
        }
    }
}
