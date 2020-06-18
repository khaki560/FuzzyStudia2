using lib.database;
using lib.tools;

namespace lib.variable
{
    internal class Summarizer : LinguisticVariable
    {
        string attribute;
        public Summarizer(LinguisticVariableParameters par) : base(par)
        {
            attribute = par.attribute;
        }

        public override double Compute(string label, double value)
        {
            throw new System.NotImplementedException();
        }

        public override FuzzySet CreateSet(string label, FuzzyModel data)
        {
            int i = 0;
            for(i = 0; i < H.Length; i++)
            {
                if(H[i] == label)
                {
                    break;
                }
            }

            return new FuzzySet(data.Get("key").ToArray(), data.Get(attribute).ToArray(), G[i]);
        }

        public override bool IsRelative()
        {
            throw new System.NotImplementedException();
        }
    }
}