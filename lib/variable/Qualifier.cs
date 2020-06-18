using lib.database;
using lib.tools;

namespace lib.variable
{
    internal class Qualifier : LinguisticVariable
    {
        string attribute;
        public Qualifier(LinguisticVariableParameters par) : base(par)
        {
            attribute = par.attribute;
        }

        public override FuzzySet CreateSet(string label, FuzzyModel data)
        {
            int i = 0;
            for (i = 0; i < H.Length; i++)
            {
                if (H[i] == label)
                {
                    break;
                }
            }

            return new FuzzySet(data.Get("id").ToArray(), data.Get(attribute).ToArray(), G[i]);
        }

        public override double Compute(string label, double value)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsRelative()
        {
            throw new System.NotImplementedException();
        }
    }
}