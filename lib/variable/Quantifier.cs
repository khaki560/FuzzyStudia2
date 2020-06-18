using lib.database;
using lib.tools;
using System;

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
    }
}