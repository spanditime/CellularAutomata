using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;

namespace CellularAutomata
{
    public class Rules
    {
        private abstract class IRule
        {
            public IRule(string str) { Instruction = str; }
            public abstract Tuple<bool, byte> calc(byte[] cells, Dictionary<byte, byte> neighbCount);
            public string Instruction { get; private set; }
        }
        private class FullRule : IRule
        {
            private byte[] rule;
            private byte result;
            public FullRule(string str, byte[] rule, byte result) : base(str)
            {
                this.rule = rule;
                this.result = result;
            }
            public override Tuple<bool, byte> calc(byte[] cells, Dictionary<byte, byte> neighbCount)
            {
                bool cond = true;
                for (int i = 0; i < 9; i++)
                {
                    cond = cond && (rule[i] == 255) ? (true) : (rule[i] == cells[i]);
                }
                return new Tuple<bool, byte>(cond, (cond) ? (result) : (cells[4]));
            }
        }
        private class CompRule : IRule
        {
            private Dictionary<byte, List<Tuple<Func<byte, byte, bool>, byte>>> rules;
            private byte cell;
            private byte result;
            public CompRule(string str,byte cell, Dictionary<byte, List<Tuple<Func<byte, byte, bool>, byte>>> rules, byte result) : base(str)
            {
                this.cell = cell;
                this.rules = rules;
                this.result = result;
            }
            public override Tuple<bool, byte> calc(byte[] cells, Dictionary<byte, byte> neighbCount)
            {
                bool cond = (cell == 255) || (cell == cells[4]);//either the cell param is any or is eq middle cell
                if (cond)
                {
                    foreach (var rule in rules)
                    {
                        foreach (var value in rule.Value)
                        {
                            byte neighbours = (neighbCount.ContainsKey(rule.Key)) ? (neighbCount[rule.Key]) : ((byte)0);
                            cond = cond && value.Item1(neighbours, value.Item2);
                        }
                        if (!cond)
                            break;
                    }
                }
                return new Tuple<bool, byte>(cond, (cond) ? (result) : (cells[4]));
            }
        }

        public Rules(byte states, string Rules)
        {
            this.states = states;
            this.rules = new List<IRule>();
            if (states > 100 || states < 2)
            {
                throw new ArgumentException(String.Format("States cannot be more than 100 or less than 2, given: {0}", states));
            }
            Rules = Regex.Replace(Rules, @"\s+", "");
            Rules = Rules.Replace("\r\n", "\n");
            char[] separators = { '\n', ';' };
            String[] instructions = Rules.Split(separators);
            Dictionary<String, Func<byte, byte, bool>> compFuncs = new Dictionary<string, Func<byte, byte, bool>>
            {
                {">=", (byte left,byte right)=> left >= right },
                {"<=", (byte left,byte right)=> left <= right },
                {"==", (byte left,byte right)=> left == right },
                {"!=", (byte left,byte right)=> left != right },
                {">", (byte left,byte right)=> left > right },
                {"<", (byte left,byte right)=> left < right }
            };
            // compile all regex'es
            Regex full = new Regex("^\\[((?:[0-9]{0,2},){8}[0-9]{0,2})\\]:([0-9]{0,2})$"); //[(,,,,,,,,)]:(3)
            Regex comp = new Regex("^\\[([0-9]{0,2})\\]\\(((?:\\[[0-9]{1,2}\\](?:[<>=!]=|[<>])[0-8],)*(?:\\[[0-9]{1,2}\\](?:[<>=!]=|[<>])[0-8]))\\):([0-9]{1,2})$"); //[]([2]<=2,[4]>=3):2
            Regex comparassion = new Regex("^\\[([0-9]{1,2})\\]([<>=!]=|[<>])([0-8])$");// single comparasion [(3)](>=)(3)

            foreach (String instruction in instructions)
            {
                if (full.IsMatch(instruction))
                {
                    Match m = full.Match(instruction);
                    String[] SCValues = m.Groups[1].Value.Split(',');
                    byte[] rule = new byte[9];
                    for (int i = 0; i < 9; i++)
                    {
                        if (SCValues[i].Length == 0)
                        {
                            rule[i] = 255;
                        }
                        else
                        {
                            rule[i] = byte.Parse(SCValues[i]);
                            if (rule[i] >= states)
                            {
                                // throw an exception
                                System.Environment.Exit(10);
                            }
                        }
                    }
                    byte result = byte.Parse(m.Groups[2].Value);
                    if (result >= states)
                    {
                        // throw an exception
                    }
                    this.rules.Add(new FullRule(instruction, rule, result));
                }
                else if (comp.IsMatch(instruction))
                {
                    Match m = comp.Match(instruction);
                    byte cell;
                    if (m.Groups[1].Value.Length == 0)
                    {
                        cell = 255;
                    }
                    else
                    {
                        cell = byte.Parse(m.Groups[1].Value);
                    }
                    byte result = byte.Parse(m.Groups[3].Value);
                    String[] expressions = m.Groups[2].Value.Split(',');
                    Dictionary<byte, List<Tuple<Func<byte, byte, bool>, byte>>> rules = new Dictionary<byte, List<Tuple<Func<byte, byte, bool>, byte>>>();
                    foreach (String expression in expressions)
                    {
                        m = comparassion.Match(expression);
                        byte left = byte.Parse(m.Groups[1].Value);
                        if (left >= states)
                        {
                            // throw an exception
                        }
                        byte right = byte.Parse(m.Groups[3].Value);
                        if (!rules.ContainsKey(left))
                            rules.Add(left, new List<Tuple<Func<byte, byte, bool>, byte>>());
                        rules[left].Add(new Tuple<Func<byte, byte, bool>, byte>(compFuncs[m.Groups[2].Value], right));
                    }
                    this.rules.Add(new CompRule(instruction,cell, rules, result));
                }
                else
                {
                    // throw an exception
                }
            }
        }
        public static Rules LoadFromFile(FileStream fs)
        {
            var m = Regex.Match(Path.GetFileName(fs.Name), "^[\\w\\d]+\\.car([0-9]{1,2})$");
            if (!m.Success)
            {
                // throw an exception
            }
            StreamReader sr = new StreamReader(fs);
            String rules = sr.ReadToEnd();
            return new Rules(byte.Parse(m.Groups[1].Value), rules);
        }
        
        public byte states { private set; get; }
        private List<IRule> rules;

        public byte Step(byte[] cells)
        {
            Dictionary<byte, byte> neighbCount = new Dictionary<byte, byte>();
            for (int i = 0; i < 9; i++)
            {
                if (i == 4)
                {
                    continue;
                }
                if (!neighbCount.ContainsKey(cells[i]))
                    neighbCount.Add(cells[i], 0);
                neighbCount[cells[i]] += 1;
            }
            byte result = cells[4];
            foreach (var rule in this.rules)
            {
                var calc = rule.calc(cells, neighbCount);
                if (calc.Item1)
                    result = calc.Item2;
            }
            return result;
        }
        public bool IsStableForGivenDefaultState(byte defaultState)
        {
            byte[] p = new byte[9];
            for (int i = 0; i < 9; i++)
                p[i] = defaultState;
            return defaultState == this.Step(p);
        }
        public string[] GetInstructions()
        {
            string[] returnValue = new string[rules.Count];
            for(int i = 0; i < rules.Count; i++)
            {
                returnValue[i] = rules[i].Instruction;
            }
            return returnValue;
        }
    }
}
