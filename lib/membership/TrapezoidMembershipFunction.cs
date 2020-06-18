using System;
using System.Collections.Generic;
using System.Text;

namespace lib.membership
{
    class TrapezoidMembershipFunction : IMembershipFunction
    {
        private double mA;
        private double mB;
        private double mC;
        private double mD;

        public TrapezoidMembershipFunction(double a, double b, double c, double d)
        {
            mA = a;
            mB = b;
            mC = c;
            mD = d;
        }
        public double Calc(double x)
        {
            double el3 = (x - mA) / (mB - mA);
            double el4 = (mD - x) / (mD - mC);

            double el23 = Math.Min(1, el3);
            double el234 = Math.Min(el23, el4);
            double el1234 = Math.Max(0, el234);

            return Math.Round(el1234, 2);
        }
    }
}
