using System;
using System.Collections.Generic;
using System.Text;

namespace lib.membership
{
    class GaussMembershipFunction : IMembershipFunction
    {
        private double mC;
        private double mS;

        public GaussMembershipFunction(double c, double s)
        {
            mC = c;
            mS = s;
        }

        public double Calc(double x)
        {
            double coef = (x - mC) / mS;
            double coef2 = Math.Pow(coef, 2);

            return Math.Round(Math.Exp(-1 * coef2), 2);
        }
    }
}
