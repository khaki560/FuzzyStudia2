using System;
using System.Collections.Generic;
using System.Text;

namespace lib.membership
{
    class ClassicMembershipFunction : IMembershipFunction
    {
        private delegate bool MembershipFunction(double x);

        private double mThreshold;
        MembershipFunction mMembershipFunction;

        public ClassicMembershipFunction(string sign, double threshold)
        {
            mThreshold = threshold;

            switch(sign)
            {
                case ">":
                case "g":
                    mMembershipFunction = new MembershipFunction(gt);
                    break;
                case ">=":
                case "ge":
                    mMembershipFunction = new MembershipFunction(ge);
                    break;
                case "<":
                case "l":
                    mMembershipFunction = new MembershipFunction(lt);
                    break;
                case "<=":
                case "le":
                    mMembershipFunction = new MembershipFunction(le);
                    break;
                case "==":
                case "eq":
                    mMembershipFunction = new MembershipFunction(eq);
                    break;
                case "!=":
                case "nq":
                    mMembershipFunction = new MembershipFunction(ne);
                    break;
            }
        }
        private bool gt(double x)
        {
            return x > mThreshold;
        }
        private bool ge(double x)
        {
            return x >= mThreshold;
        }
        private bool lt(double x)
        {
            return x < mThreshold;
        }
        private bool le(double x)
        {
            return x <= mThreshold;
        }

        private bool eq(double x)
        {
            return Math.Abs(x - mThreshold) < 0.001;
        }
        private bool ne(double x)
        {
            return !eq(x);
        }

        public double Calc(double x)
        {
            double result = -1;

            if (mMembershipFunction(x))
            {
                result = 1.0;
            }
            else
            {
                result = 0.0;
            }

            return result;
        }
    }
}
