using lib.membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib
{
    public class FuzzySet
    {
        private List<Tuple<Double, Double>> mSet = new List<Tuple<Double, Double>>();


        private FuzzySet(List<Tuple<Double, Double>> set)
        {
            mSet.Clear();
            foreach (var el in set)
            {
                mSet.Add(new Tuple<double, double>(el.Item1, el.Item2));
            }
        }

        public FuzzySet(double[] input, double[] value, IMembershipFunction memberShipFunction)
        {
            mSet.Clear();
            for (int i = 0; i < input.Length; i++)
            {
                mSet.Add(new Tuple<double, double>(input[i], memberShipFunction.Calc(value[i])));
            }
        }

        public List<Tuple<Double, Double>> Get()
        {
            return mSet;
        }

        public int Length()
        {
            return mSet.Count;
        }

        public FuzzySet complement()
        {
            var FuzzySet = new List<Tuple<Double, Double>>();

            foreach (var x in mSet)
            {
                FuzzySet.Add(new Tuple<Double, Double>(x.Item1, Math.Round(1 - x.Item2, 2)));
            }

            return new FuzzySet(FuzzySet);
        }

        public FuzzySet Intersection(FuzzySet b)
        {
            var FuzzySet = new List<Tuple<Double, Double>>();

            for (int i = 0; i < this.Length(); i++)
            {
                if (Math.Abs(this.mSet[i].Item1 - b.mSet[i].Item1) > 0.01)
                {
                    throw new Exception("Elements not match");
                }
                FuzzySet.Add(new Tuple<Double, Double>(this.mSet[i].Item1, Math.Min(this.mSet[i].Item2, b.mSet[i].Item2)));
            }

            return new FuzzySet(FuzzySet);
        }

        public FuzzySet Union(FuzzySet b)
        {
            var FuzzySet = new List<Tuple<Double, Double>>();

            for (int i = 0; i < this.Length(); i++)
            {
                if (Math.Abs(this.mSet[i].Item1 - b.mSet[i].Item1) > 0.01)
                {
                    throw new Exception("Elements not match");
                }
                FuzzySet.Add(new Tuple<Double, Double>(this.mSet[i].Item1, Math.Max(this.mSet[i].Item2, b.mSet[i].Item2)));
            }

            return new FuzzySet(FuzzySet);
        }

        public double Height()
        {
            double max_el = -1.0;
            foreach (var el in mSet)
            {
                double value = el.Item2;

                if (value > max_el)
                {
                    max_el = value;
                }
            }
            return max_el;
        }

        public bool IsNormal()
        {
            return Math.Abs(1 - Height()) < 0.01;
        }

        public bool IsEmpty()
        {
            bool IsEmpty = true;

            foreach (var el in mSet)
            {
                double value = el.Item2;
                if (!(Math.Abs(0 - value) < 0.01))
                {
                    IsEmpty = false;
                    break;
                }
            }
            return IsEmpty;
        }

        public bool IsConcave()
        {
            throw new NotImplementedException("No idea how");
        }

        public bool IsConvex()
        {
            throw new NotImplementedException("No idea how");
        }

        public List<double> Supp()
        {
            return AlfaCut(true, 0);
        }

        public List<double> AlfaCut(bool strong, double threshold)
        {
            List<double> alfaCut = new List<double>();

            foreach (var el in mSet)
            {
                var index = el.Item1;
                var value = el.Item2;

                if (strong)
                {
                    if (value > threshold)
                    {
                        alfaCut.Add(index);
                    }
                }
                else
                {
                    if (value >= threshold)
                    {
                        alfaCut.Add(index);
                    }
                }
            }
            return alfaCut;
        }

        public double Sum()
        {
            double sum = 0;
            foreach (var el in mSet)
            {
                sum += el.Item2;
            }
            return sum;
        }


        public double SignmaCount()
        {
            return Sum();
        }
        public FuzzySet pow(double power)
        {
            var FuzzySet = new List<Tuple<Double, Double>>();
            foreach (var x in mSet)
            {
                FuzzySet.Add(new Tuple<Double, Double>(x.Item1, Math.Round(Math.Pow(x.Item2, power), 2)));
            }
            return new FuzzySet(FuzzySet);
        }
    }
}
