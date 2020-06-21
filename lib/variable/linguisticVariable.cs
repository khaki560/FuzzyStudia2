using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib.database;
using lib.membership;
using lib.tools;
namespace lib.variable
{
    class MemberShipFunction
    {
    }

    public abstract class LinguisticVariable
    {
        protected string name;
        protected Tuple<double, double> x;
        protected string[] H;
        protected IMembershipFunction[] G;

        public LinguisticVariable(LinguisticVariableParameters par)
        {
            name = par.Name;
            x = par.range;
            H = par.Labels.ToArray();
            G = new IMembershipFunction[par.memberShipFunction.Count];
            
            for( int i = 0; i < par.memberShipFunction.Count; i++)
            {
                var mFunction = par.memberShipFunction[i];
                mFunction  = mFunction.Remove(mFunction.Length-1, 1);
                var mFunctionArray = mFunction.Split('(');

                var args = new List<string>();
                foreach(var v in mFunctionArray[1].Split(','))
                {
                    args.Add(v);
                }
                switch(mFunctionArray[0])
                {
                    case "triangle":
                        var triangle1 = double.Parse(args[0]);
                        var triangle2 = double.Parse(args[1]);
                        var triangle3 = double.Parse(args[2]);
                        G[i] = new TriangleMembershipFunction(triangle1, triangle2, triangle3);
                    break;
                    case "trapezoid":
                        var trapezoid1 = double.Parse(args[0]);
                        var trapezoid2 = double.Parse(args[1]);
                        var trapezoid3 = double.Parse(args[2]);
                        var trapezoid4 = double.Parse(args[3]);
                        G[i] = new TrapezoidMembershipFunction(trapezoid1, trapezoid2, trapezoid3, trapezoid4);
                    break;
                    case "classic":
                        var classic1 = args[0];
                        var classic2 = double.Parse(args[1]);
                        G[i] = new ClassicMembershipFunction(classic1, classic2);
                    break;
                    case "gauss":
                        var gauss1 = double.Parse(args[0]);
                        var gauss2 = double.Parse(args[1]);
                        G[i] = new GaussMembershipFunction(gauss1, gauss2);
                    break;
                }
            }
        }

        public string[] GetDisplayNames()
        {
            var a = new List<string>();
            foreach(var l in H)
            {
                a.Add(l + " " + name);
            }
            return a.ToArray();
        }

        public virtual string GetDisplayNames(string label)
        {
            return label + " " + name;
        }

        public abstract FuzzySet CreateSet(string label, FuzzyModel data);

        public abstract double Compute(string label, double value);

        public abstract bool IsRelative();

        public bool isCorrect(string name)
        {
            // tylko dokladnie to samo
            return name.Contains(this.name);
        }

        public string getLabel(string name)
        {
            foreach(var h in H)
            {
                if (name.Contains(h))
                    return h;
            }
            return null;
        }


        public virtual double Integrate(string label)
        {
            throw new Exception("Incorect class");
        }

        public virtual double IntegrateSupp(string label)
        {
            throw new Exception("Incorect class");
        }
    }
}
