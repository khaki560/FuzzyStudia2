using lib.database;
using lib.tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace lib.variable
{
    internal class Quantifier : LinguisticVariable
    {
        bool relative = false;
        public Quantifier(LinguisticVariableParameters par) : base(par)
        {
            if(par.type == LinguisticType.QuantifierAbsolute)
            {
                relative = false;
            }
            else if(par.type == LinguisticType.QuantifierRelative)
            {
                relative = true;
            }
            else
            {
                throw new Exception("Quantifier - wrong type");
            }
        }

        public override double Compute(string label, double value)
        {
            int i = 0;
            for (i = 0; i < H.Length; i++)
            {
                if (H[i] == label)
                {
                    break;
                }
            }

            return G[i].Calc(value);
        }

        public override FuzzySet CreateSet(string label, FuzzyModel data)
        {
            throw new NotImplementedException();
        }

        public override bool IsRelative()
        {
            return relative;
        }

        public override string GetDisplayNames(string label)
        {
            return label;
        }

        public override double Integrate(string label)
        {
            double alfa = 0;
            int min = 0;
            int max = 0;
            if (IsRelative())
            {
                alfa = 1.0 / 1000;
                min = (int)0;
                max = (int)((x.Item2 - x.Item1) / alfa);
            }
            else
            {
                alfa = (x.Item2 - x.Item1) / 1000;
                min = (int)x.Item1;
                max = (int)x.Item2;
            }
            
            double[] result = Enumerable.Range(0, max)
                  .Select(i => (double)i * alfa + min)
                  .ToArray();


            var l = new List<double>();
            foreach ( var r in result)
            {
                var p = Compute(label, r) * alfa;
                l.Add(p);
            }

            return l.Sum();
        }

        public override double IntegrateSupp(string label)
        {
            double alfa = 0;
            int min = 0;
            int max = 0;
            if (IsRelative())
            {
                alfa = 1.0 / 1000;
                min = (int)0;
                max = (int)((x.Item2 - x.Item1) / alfa);
            }
            else
            {
                alfa = (x.Item2 - x.Item1) / 1000;
                min = (int)x.Item1;
                max = (int)x.Item2;
            }

            double[] result = Enumerable.Range(0, max)
                  .Select(i => (double)i * alfa + min)
                  .ToArray();


            var l = new List<double>();
            foreach (var r in result)
            {
                var p = Compute(label, r);
                if (p > 0) p = 1;
                p *= alfa;
                l.Add(p);
            }

            return l.Sum();
        }
    }
}