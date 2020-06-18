using lib.variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.tools
{
    public enum LinguisticType
    {
        QuantifierAbsolute,
        QuantifierRelative,
        Qualifier,
        Summarizer
    }

    public class LinguisticVariableParameters
    {
        public string Name;
        public Tuple<double, double> range;
        public List<string> Labels;
        public List<string> memberShipFunction;
        public LinguisticType type;
        public string attribute;
    }

    public class LinguisticSummaryParameter
    {
        public string[] modifiers;
        public LinguisticVariable var;
        public string connector;
        public string label;
    }
}
