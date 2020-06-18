using lib.database;
using lib.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace lib
{
    public class LinguisticSummary
    {
        LinguisticSummaryParameter quantifierParam;
        List<LinguisticSummaryParameter> qualifierParam;
        List<LinguisticSummaryParameter> summarizerParam;
        Dictionary<string, double> hedges;

        int summarizerNumber = 0;
        int qualifierNumber = 0;

        FuzzySet[] summarizerSet;
        FuzzySet[] qualifierSet;

        FuzzySet combineSummarizerSet;
        FuzzySet combineQualifierSet;
        FuzzySet combineSet;

        public LinguisticSummary(LinguisticSummaryParameter quantifier, List<LinguisticSummaryParameter> qualifier, List<LinguisticSummaryParameter> summarizer, Dictionary<string, double> hedges)
        {
            quantifierParam = quantifier;
            qualifierParam = qualifier;
            summarizerParam = summarizer;
            this.hedges = hedges;

            qualifierNumber = 0;
            foreach(var w in qualifier)
            {
                if (w.var != null) qualifierNumber++;
            }

            summarizerNumber = 0;
            foreach (var s in summarizer)
            {
                if (s.var != null) summarizerNumber++;
            }

            summarizerSet = new FuzzySet[summarizerNumber];
            qualifierSet = new FuzzySet[qualifierNumber];
        }


        private void CreateFuzzySets(FuzzyModel data)
        {
            // calculate W fuzzy sets
            if (qualifierNumber > 0)
            {
                for (int i = 0; i < qualifierParam.Count; i++)
                {
                    var variable = qualifierParam[i].var;
                    qualifierSet[i] = variable.CreateSet(qualifierParam[i].label, data);
                }
            }

            // calculate S fuzzy set 
            for (int i = 0; i < summarizerParam.Count; i++)
            {
                var variable = summarizerParam[i].var;
                summarizerSet[i] = variable.CreateSet(summarizerParam[i].label, data);
            }
        }

        private FuzzySet CombineSet(List<LinguisticSummaryParameter> param, FuzzySet[] set)
        {
            var fuzzySet = set[0];
            var connector = param[0].connector;
            for(int i = 1; i < set.Length; i++)
            {
                if(connector == "AND")
                {
                    fuzzySet = fuzzySet.Intersection(set[i]);
                }
                else if(connector == "OR")
                {
                    fuzzySet = fuzzySet.Union(set[i]);
                }
                else
                {
                    break;
                }
                connector = param[i].connector;
            }
            return fuzzySet;
        }

        private FuzzySet[] ModifySet(List<LinguisticSummaryParameter> param, FuzzySet[] set)
        {
            for (int i = 0; i < set.Length; i++)
            {
                foreach (var mod in param[i].modifiers.Reverse())
                {
                    if (mod == "NOT")
                    {
                        set[i] = set[i].complement();
                    }
                    else if (string.IsNullOrEmpty(mod))
                    {
                        continue;
                    }
                    else
                    {
                        set[i] = set[i].pow(hedges[mod]);
                    }
                }
            }
            return set;

        }

        private void CalculateSets(FuzzyModel data)
        {
            CreateFuzzySets(data);
            if (qualifierNumber > 0)
            {
                qualifierSet = ModifySet(qualifierParam, qualifierSet);
                combineQualifierSet = CombineSet(qualifierParam, qualifierSet);
            }

            summarizerSet = ModifySet(summarizerParam, summarizerSet);
            combineSummarizerSet = CombineSet(summarizerParam, summarizerSet);

            if (qualifierNumber > 0)
            {
                combineSet = combineSummarizerSet.Intersection(combineQualifierSet);
            }
            else
            {
                combineSet = combineSummarizerSet;
            }
        }

        public string MakeSummary(FuzzyModel data)
        {
            CalculateSets(data);
            var T = DegreeOfTruth(data);
            return CreateLingusiticSummary(T, data);
        }

        private string CreateLingusiticSummary(double degreeOfTruth, FuzzyModel data)
        {
            string q = quantifierParam.var.GetDisplayNames(quantifierParam.label) + " " + data.GetName() + " ";
            string w = "";
            string s = "have ";

            if (qualifierNumber > 0)
            {
                w = "having ";
                foreach (var ww in qualifierParam)
                {
                    w = w + ww.var.GetDisplayNames(ww.label);
                    w = w + " ";
                    w = w + ww.connector;
                    w = w + " ";
                }
            }

            foreach (var ss in summarizerParam)
            {
                s = s + ss.var.GetDisplayNames(ss.label);
                s = s + " ";
                s = s + ss.connector;
                s = s + " ";
            }

            var summary = q + w + s + " [" + Math.Round(degreeOfTruth, 2).ToString() + "]";
            return summary;
        }
    
        private double ComputeQualifier(double value)
        {
            value = quantifierParam.var.Compute(quantifierParam.label, value);

            foreach(var mod in quantifierParam.modifiers.Reverse())
            {
                if (mod == "NOT")
                {
                    value = 1 - value;
                }
                else if (string.IsNullOrEmpty(mod))
                {
                    continue;
                }
                else
                {
                    value = Math.Pow(value, hedges[mod]);
                }
            }

            return value;
        }
        private double DegreeOfTruth(FuzzyModel data)
        {
            // calculate degree of thruth 
            double m = -1;
            if (qualifierNumber > 0)
            {
                m = combineQualifierSet.Sum();
            }
            else
            {
                if (quantifierParam.var.IsRelative())
                {
                    m = data.Length();
                }
                else
                {
                    m = 1;
                }
            }
            double r = combineSet.Sum();

            var T = ComputeQualifier(r / m);
            return T;
        }
        private double DegreeOfImprecission(FuzzyModel data)
        {
            double result = 1;
            foreach(var s in summarizerSet)
            {
                double a = (double)s.Supp().Count / data.Length();
                result *= a;
            }

            result = Math.Pow(result, 1 / summarizerSet.Length);
            return result;
        }
        private double DegreeOfCovering(FuzzyModel data)
        {
            int h = data.Length();
            if(qualifierNumber > 0)
            {
                h = combineQualifierSet.Supp().Count;
            }
            double t = (double)combineSet.Supp().Count;

            if (h == 0)
                return 1;
            return t / h;
        }

        private double DegreeOfAppropiateness(FuzzyModel data)
        {
            double result = 1;
            foreach (var s in summarizerSet)
            {
                result *= ((double)s.Supp().Count / (double)data.Length());
            }

            result = result - DegreeOfCovering(data);
            result = Math.Abs(result);
            return result;
        }

        private double LengthOfSummary(FuzzyModel data)
        {
            return 2 * Math.Pow(0.5, summarizerSet.Length);
        }

        private double DegreeOfQuantifierImprecision(FuzzyModel data)
        {
            return 0;
        }

        private double DegreeOfQuantifierCardinality(FuzzyModel data)
        {
            return 0;
        }

        private double DegreeOfSummarizerCardinality(FuzzyModel data)
        {
            double result = 1;
            foreach (var s in summarizerSet)
            {
                result *= ((double)s.SignmaCount() / data.Length());
            }

            result = Math.Pow(result, 1 / summarizerSet.Length);
            return result;
        }

        private double DegreeOfQualifierImprecision(FuzzyModel data)
        {
            if(qualifierNumber > 0)
            {
                double result = 1;
                foreach(var w in qualifierSet)
                {
                    result *= ((double)w.Supp().Count / data.Length());
                }
                result = Math.Pow(result, 1 / qualifierSet.Length);
                return 1 - result;
            }
            else
            {
                return 1;
            }
        }

        private double DegreeOfQualifierCardinality(FuzzyModel data)
        {
            if (qualifierNumber > 0)
            {
                double result = 1;
                foreach (var w in qualifierSet)
                {
                    result *= ((double)w.SignmaCount() / data.Length());
                }
                result = Math.Pow(result, 1 / qualifierSet.Length);
                return 1 - result;
            }
            else
            {
                return 0;
            }
        }

        public double CreateQualityMeasure(int number, FuzzyModel data)
        {
            double result = -1;
            switch(number)
            {
                case 0:
                    result = DegreeOfTruth(data);
                    break;
                case 1:
                    result = DegreeOfImprecission(data);
                    break;
                case 2:
                    result = DegreeOfCovering(data);
                    break;
                case 3:
                    result = DegreeOfAppropiateness(data);
                    break;
                case 4:
                    result = LengthOfSummary(data);
                    break;
                case 5:
                    result = DegreeOfQuantifierImprecision(data);
                    break;
                case 6:
                    result = DegreeOfQuantifierCardinality(data);
                    break;
                case 7:
                    result = DegreeOfSummarizerCardinality(data);
                    break;
                case 8:
                    result = DegreeOfQualifierImprecision(data);
                    break;
                case 9:
                    result = DegreeOfQualifierCardinality(data);
                    break;
            }
            return Math.Round(result, 2);
        }

        public double CreateFinalQualityMeasure(int []indexes, double[]weights, FuzzyModel data)
        {
            double result = 0;
            for(int i = 0; i < indexes.Length; i++)
            {
                result += CreateQualityMeasure(indexes[i], data) * weights[i];
            }

            return result;
        }

    }
}