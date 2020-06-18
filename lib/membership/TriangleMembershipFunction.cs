using System;
using System.Collections.Generic;
using System.Text;

namespace lib.membership
{
    class TriangleMembershipFunction : IMembershipFunction
    {
        private double mA;
        private double mB;
        private double mC;
        public TriangleMembershipFunction(double a, double b, double c)
        {
            if((a > b) || (b > c))
            {
                throw new Exception("a,b and c must be true for a < b < c");
            }
            mA = a;
            mB = b;
            mC = c;
        }

        public double Calc(double x)
        {
            double el1 = 0;
            double el2 = ((x - mA) / (mB - mA));
            double el3 = ((mC - x) / (mC - mB));

            double el23 = Math.Min(el2, el3);
            return Math.Round(Math.Max(el1, el23), 2);
        }
    }
}
