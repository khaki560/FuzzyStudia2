using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.database
{
    public class FuzzyModel
    {
        List<List<double>> list = new List<List<double>>();
        Dictionary<string, int> index = new Dictionary<string, int>();

        public FuzzyModel(List<string> attributesName)
        {
            list.Add(new List<double>());
            index.Add("id", 0);

            int i = 1;
            foreach(var name in attributesName)
            {
                list.Add(new List<double>());
                index.Add(name, i);
                i++;
            }
        }

        public void Add(double []row)
        {
            if (row.Length != list.Count)
                throw new Exception("FuzzyModel: wrong data");
            for (int i = 0; i < row.Length; i++)
            {
                list[i].Add(row[i]);
            }
        }

        public List<double> Get(string key)
        {
            int i;
            index.TryGetValue(key, out i);
            return list[i];
        }

        public int Length()
        {
            return Get("id").Count;
        }

        public string GetName()
        {
            return "Measurements";
        }
    }
}
