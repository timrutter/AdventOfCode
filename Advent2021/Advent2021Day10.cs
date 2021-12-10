using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2021;

public class Advent2021Day10 : Solution
{
    public Advent2021Day10()
    {
        Answer1 = null;
        Answer2 = null;
    }

    public override object ExecutePart1()
    {
        var score = 0;
        string[] lines = DataFile.ReadAll<string>();
        foreach (string line in lines)
        {
            var stack = new Stack<char>();
            var invalid = false;
            foreach (char c in line)
            {
                switch (c)
                {
                    case '(':
                        stack.Push('(');
                        break;
                    case '[':
                        stack.Push('[');
                        break;
                    case '{':
                        stack.Push('{');
                        break;
                    case '<':
                        stack.Push('<');
                        break;
                    case ')':
                        if (stack.Peek() != '(')
                        {
                            score += 3;
                            invalid = true;
                            break;
                        }

                        stack.Pop();
                        break;
                    case ']':
                        if (stack.Peek() != '[')
                        {
                            score += 57;
                            invalid = true;
                            break;
                        }

                        stack.Pop();
                        break;
                    case '}':
                        if (stack.Peek() != '{')
                        {
                            score += 1197;
                            invalid = true;
                            break;
                        }

                        stack.Pop();
                        break;
                    case '>':
                        if (stack.Peek() != '<')
                        {
                            score += 25137;
                            invalid = true;
                            break;
                        }

                        stack.Pop();
                        break;
                }

                if (invalid) break;
            }

            stack.Clear();
        }

        return score;
    }

    public override object ExecutePart2()
    {
        var scores = new List<long>();
        string[] lines = DataFile.ReadAll<string>();
        foreach (string line in lines)
        {
            var stack = new Stack<char>();
            var invalid = false;
            foreach (char c in line)
            {
                switch (c)
                {
                    case '(':
                        stack.Push('(');
                        break;
                    case '[':
                        stack.Push('[');
                        break;
                    case '{':
                        stack.Push('{');
                        break;
                    case '<':
                        stack.Push('<');
                        break;
                    case ')':
                        if (stack.Peek() != '(')
                        {
                            invalid = true;
                            break;
                        }

                        stack.Pop();
                        break;
                    case ']':
                        if (stack.Peek() != '[')
                        {
                            invalid = true;
                            break;
                        }

                        stack.Pop();
                        break;
                    case '}':
                        if (stack.Peek() != '{')
                        {
                            invalid = true;
                            break;
                        }

                        stack.Pop();
                        break;
                    case '>':
                        if (stack.Peek() != '<')
                        {
                            invalid = true;
                            break;
                        }

                        stack.Pop();
                        break;
                }

                if (invalid) break;
            }

            long score = 0;
            if (!invalid)
            {
                while (stack.Any())
                    switch (stack.Pop())
                    {
                        case '(':
                            score = score * 5 + 1;
                            break;
                        case '[':
                            score = score * 5 + 2;
                            break;
                        case '{':
                            score = score * 5 + 3;
                            break;
                        case '<':
                            score = score * 5 + 4;
                            break;
                    }

                scores.Add(score);
            }
        }

        scores.Sort();
        return scores[scores.Count / 2];
    }
}