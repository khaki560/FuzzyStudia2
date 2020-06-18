using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using lib.tools;

namespace lib
{
    class XmlReader
    {
        private string xmlPath;
        XElement xml;
        public XmlReader(string path)
        {
            xmlPath = path;
            xml = XElement.Load(xmlPath);
        }


        public string getConnectionString()
        {
            string connectionString = "";

            var settings = xml.Element("settings");

            var server = settings.Element("server");
            var database = settings.Element("database");
            var security = settings.Element("security");
            var MultipleActiveResultSets = settings.Element("MultipleActiveResultSets");


            if (server != null)
                connectionString += "Server=" + server.Value + ";";
            if (database != null)
                connectionString += "Database=" + database.Value + ";" ;
            if (security != null)
                connectionString += "Integrated Security=" + security.Value + ";";
            if (MultipleActiveResultSets != null)
                connectionString += "Integrated Security=" + MultipleActiveResultSets.Value + ";";

            connectionString = connectionString.Remove(connectionString.Length - 1, 1);



            return connectionString;
        }

        public string GetTableName()
        {
            var settings = xml.Element("settings");
            return settings.Element("table").Value;
        }
        
        private LinguisticVariableParameters ParseSummarizers(XElement summarizer)
        {
            string name = summarizer.Element("name").Value;
            var min = double.Parse(summarizer.Element("discourse").Element("min").Value);
            var max = double.Parse(summarizer.Element("discourse").Element("max").Value);

            var labels = new List<string>();
            var membershipFunctions = new List<string>();

            foreach (var l in summarizer.Elements("label"))
            {
                labels.Add(l.Element("name").Value);
                membershipFunctions.Add(l.Element("membershipFunction").Value);
            }
            LinguisticType type = LinguisticType.Summarizer;
            var attr = summarizer.Element("attribute").Value;

            return new LinguisticVariableParameters()
            {
                Name = name,
                range = new Tuple<double, double>(min, max),
                Labels = labels,
                memberShipFunction = membershipFunctions,
                type = type,
                attribute=attr
            };
        }

        private LinguisticVariableParameters ParseQualifiers(XElement qualifier)
        {
            string name = qualifier.Element("name").Value;
            var min = double.Parse(qualifier.Element("discourse").Element("min").Value);
            var max = double.Parse(qualifier.Element("discourse").Element("max").Value);

            var labels = new List<string>();
            var membershipFunctions = new List<string>();

            foreach (var l in qualifier.Elements("label"))
            {
                labels.Add(l.Element("name").Value);
                membershipFunctions.Add(l.Element("membershipFunction").Value);
            }
            LinguisticType type = LinguisticType.Qualifier;
            var attr = qualifier.Element("attribute").Value;

            return new LinguisticVariableParameters()
            {
                Name = name,
                range = new Tuple<double, double>(min, max),
                Labels = labels,
                memberShipFunction = membershipFunctions,
                type = type,
                attribute = attr
            };
        }

    private LinguisticVariableParameters ParseQuantifiers(XElement quantifier)
        {
            string name = quantifier.Element("name").Value;
            var min = double.Parse(quantifier.Element("discourse").Element("min").Value);
            var max = double.Parse(quantifier.Element("discourse").Element("max").Value);

            var labels = new List<string>();
            var membershipFunctions = new List<string>();

            foreach (var l in quantifier.Elements("label"))
            {
                labels.Add(l.Element("name").Value);
                membershipFunctions.Add(l.Element("membershipFunction").Value);
            }

            LinguisticType type;
            if (quantifier.Element("type").Value.ToLower() == "absolute")
            {
                type = LinguisticType.QuantifierAbsolute;
            }
            else if (quantifier.Element("type").Value.ToLower() == "relative")
            {
                type = LinguisticType.QuantifierRelative;
            }
            else
            {
                throw new Exception("Wrong Quantifier Type");
            }

            return new LinguisticVariableParameters()
            {
                Name = name,
                range = new Tuple<double, double>(min, max),
                Labels = labels,
                memberShipFunction = membershipFunctions,
                type = type
            };
        }
        public List<LinguisticVariableParameters> GetLinguisticVariables()
        {
            var toReturn = new List<LinguisticVariableParameters>();

            //quantifiers
            var quantifiers = xml.Element("quantifiers");
            foreach(var quantifier in quantifiers.Elements())
            {           
                toReturn.Add(ParseQuantifiers(quantifier));
            }

            //qualifiers
            var qualifiers = xml.Element("qualifiers");
            foreach (var qualifier in qualifiers.Elements())
            {
                toReturn.Add(ParseQualifiers(qualifier));
            }

            //sumarizers
            var summarizers = xml.Element("summarizers");
            foreach (var summarizer in summarizers.Elements())
            {
                toReturn.Add(ParseSummarizers(summarizer));
            }

            return toReturn;
        }

        public List<(string, double)> GetModifiers()
        {
            return new List<(string, double)>();
        }


        public List<(string, int)> GetDBAttributes()
        {
            var dbAttributes = new List<(string, int)>();

            var settings = xml.Element("settings");

            var attributes = settings.Element("attributes");

            foreach(var a in attributes.Elements())
            {
                dbAttributes.Add( (a.Name.ToString(), int.Parse(a.Value)));
            }
            return dbAttributes;
        }

        public Dictionary<string, double> GetHedges()
        {
            var hedges = new Dictionary<string, double>();
            var h = xml.Element("hedges");
            foreach (var a in h.Elements())
            {
                hedges.Add(a.Name.ToString(), double.Parse(a.Value));
            }

            return hedges;
        }
    }
}
