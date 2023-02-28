using System;
using System.Collections.Generic;
using System.Text;

namespace DPRCalculator
{
    public enum DieCalc 
    { 
        Avg = 0,
        Max = 1,
        Min = 2
    }
    public static class ExpressionEvaluator
    {

        private static string[] s_operators = { "-", "+", "/", "*", "^", "d" };
        private static Func<double, double, double>[] s_operations =
        {
            (a1, a2) => a1 - a2,
            (a1, a2) => a1 + a2,
            (a1, a2) => a1 / a2,
            (a1, a2) => a1 * a2,
            (a1, a2) => Math.Pow(a1, a2),
            DieFormula,
            DieFormulaMax,
            DieFormulaMin,
        };

        public static double Eval(string expression, ref float critBonus, DieCalc dieType = DieCalc.Avg)
        {
            if (expression == "")
                return 0;

            List<string> tokens = getTokens(expression);
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;

            if (Array.IndexOf(s_operators, tokens[tokens.Count - 1]) >= 0)
                return 0;

            while (tokenIndex < tokens.Count)
            {
                string token = tokens[tokenIndex];

                // Parentheses need an iterated eval
                if (token == "(")
                {
                    string subExpr = getSubExpression(tokens, ref tokenIndex);
                    operandStack.Push(Eval(subExpr, ref critBonus));
                    continue;
                }
                if (token == ")")
                    return 0;

                //If this is an operator  
                if (Array.IndexOf(s_operators, token) >= 0)
                {
                    while (operatorStack.Count > 0 && Array.IndexOf(s_operators, token) < Array.IndexOf(s_operators, operatorStack.Peek()))
                    {
                        string op = operatorStack.Pop();
                        double arg2 = operandStack.Pop();

                        if (operandStack.Count == 0)
                            return 0;

                        double arg1 = operandStack.Pop();

                        if(op != "d")
                            operandStack.Push(s_operations[Array.IndexOf(s_operators, op)](arg1, arg2));
                        else
                        {
                            operandStack.Push(s_operations[Array.IndexOf(s_operators, op) + (int)dieType](arg1, arg2));
                            critBonus += (float)s_operations[Array.IndexOf(s_operators, op) + (int)dieType](arg1, arg2);
                        }
                    }
                    operatorStack.Push(token);
                }
                else
                {
                    operandStack.Push(double.Parse(token));
                }
                tokenIndex += 1;
            }

            while (operatorStack.Count > 0)
            {
                string op = operatorStack.Pop();
                double arg2 = operandStack.Pop();
                double arg1 = operandStack.Pop();
                operandStack.Push(s_operations[Array.IndexOf(s_operators, op)](arg1, arg2));
            }
            return operandStack.Pop();
        }

        private static string getSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parenlevels = 1;
            index += 1;
            while (index < tokens.Count && parenlevels > 0)
            {
                string token = tokens[index];
                if (tokens[index] == "(")
                {
                    parenlevels += 1;
                }

                if (tokens[index] == ")")
                {
                    parenlevels -= 1;
                }

                if (parenlevels > 0)
                {
                    subExpr.Append(token);
                }

                index += 1;
            }

            if ((parenlevels > 0))
            {
                throw new ArgumentException("Mis-matched parentheses in expression");
            }
            return subExpr.ToString();
        }

        private static List<string> getTokens(string expression)
        {
            string operators = "()d^*/+-";
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in expression.Replace(" ", string.Empty))
            {
                if (operators.IndexOf(c) >= 0)
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(c.ToString());
                }
                else
                {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }

        private static double DieFormula(double amount, double dieSize)
        {
            return amount * ((dieSize + 1.0f) / 2.0f);
        }
        private static double DieFormulaMax(double amount, double dieSize)
        {
            return amount * dieSize;
        }
        private static double DieFormulaMin(double amount, double dieSize)
        {
            return amount;
        }
    }
}
