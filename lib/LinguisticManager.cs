using lib.database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib.variable;
using lib.tools;
using System.ComponentModel;

namespace lib
{
    public static class LinguisticManager
    {
        // general objects
        private static XmlReader xmlReader;
        private static string connectionString;

        // database
        private static FuzzyModel data;

        //Linguistic objects 
        static List<Summarizer> summarizers = new List<Summarizer>();
        static List<Quantifier> quantifiers = new List<Quantifier>();
        static List<Qualifier>  qualifiers = new List<Qualifier>();
        static Dictionary<string, double> hedges;

        public static void init(string path)
        {
            xmlReader = new XmlReader(path);

            connectionString = xmlReader.getConnectionString();
            var db = new FuzzySql(connectionString);

            var tableName = xmlReader.GetTableName();
            var dbAttributes = xmlReader.GetDBAttributes();
            data = db.GetData(tableName, dbAttributes);


            hedges = xmlReader.GetHedges();
            var linguisticParameters = xmlReader.GetLinguisticVariables();
            
            //TODO: wywtorzyc summarizers, quantifiers, qualifiers;
            for(int i = 0; i < linguisticParameters.Count; i++)
            {
                var type = linguisticParameters[i].type;

                if(type == LinguisticType.QuantifierRelative || type == LinguisticType.QuantifierAbsolute)
                {
                    quantifiers.Add(new Quantifier(linguisticParameters[i]));
                }
                else if(type == LinguisticType.Qualifier)
                {
                    qualifiers.Add(new Qualifier(linguisticParameters[i]));
                }
                else if(type == LinguisticType.Summarizer)
                {
                    summarizers.Add(new Summarizer(linguisticParameters[i]));
                }
            }
        }

        public static FuzzyModel GetData()
        {
            return data;
        }

        public static List<string> GetConnectorsName()
        {
            return new List<string>() { "AND", "OR" };
        }

        public static List<string> GetModifiersName()
        {
            var m = new List<string>();
            m.Add("NOT");
            m.AddRange(hedges.Keys);
            return m;
        }

        public static List<string> GetQuantifiersName()
        {
            var a = new List<string>();
            for (int i = 0; i < quantifiers.Count; i++)
            {
                a.AddRange(quantifiers[i].GetDisplayNames());
            }
            return a;
        }

        public static List<string> GetQualifiersName()
        {
            var a = new List<string>();
            for(int i = 0; i < qualifiers.Count; i++)
            {
                a.AddRange(qualifiers[i].GetDisplayNames());
            }
            return a;
        }

        public static List<string> GetSummarizersName()
        {
            var a = new List<string>();
            for (int i = 0; i < summarizers.Count; i++)
            {
                a.AddRange(summarizers[i].GetDisplayNames());
            }
            return a;
        }

        public static LinguisticSummary CreateSummary(List<string> quantifier, List<string>qualifier, List<string> summarizer)
        {
            
            if (quantifier.Count != 1)
            {
                throw new Exception("CreateSummary"); 
            }

            // quantifier
            var q = new LinguisticSummaryParameter();
            var qArgs = quantifier.First().Split(',');
            var qMods = qArgs.Take(2).ToArray();
            var qName = qArgs[2];
            foreach (var qq in quantifiers)
            {
                if (qq.isCorrect(qName))
                {
                    q.var = qq;
                    q.modifiers = qMods;
                    q.label = qq.getLabel(qName);
                    break;
                }
            }

            // qualifier
            var wList = new List<LinguisticSummaryParameter>();
            for (int i = 0; i < qualifier.Count; i++)
            {
                var wArgs = qualifier[i].Split(',');
                var w = new LinguisticSummaryParameter();
                
                w.modifiers = wArgs.Take(2).ToArray();
                if (wArgs.Length > 3)
                {
                    w.connector = wArgs[3];
                }

                var wName = wArgs[2];
                foreach (var ww in qualifiers)
                {
                    if (ww.isCorrect(wName))
                    {
                        w.var = ww;
                        w.label = ww.getLabel(wName);
                        break;
                    }
                }
                wList.Add(w);
            }

            // summarizer
            var sList = new List<LinguisticSummaryParameter>();
            for (int i = 0; i < summarizer.Count; i++)
            {
                var sArgs = summarizer[i].Split(',');
                var s = new LinguisticSummaryParameter();

                s.modifiers = sArgs.Take(2).ToArray();
                if (sArgs.Length > 3)
                {
                    s.connector = sArgs[3];
                }

                var sName = sArgs[2];
                foreach (var ss in summarizers)
                {
                    if (ss.isCorrect(sName))
                    {
                        s.var = ss;
                        s.label = ss.getLabel(sName);
                        break;
                    }
                }
                sList.Add(s);
            }

            return new LinguisticSummary(q, wList, sList, hedges);
        }

    }
}
