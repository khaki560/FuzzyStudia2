using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.database
{
    class FuzzySql
    {
        private string _connectionString;

        public FuzzySql(string connectionString)
        {
            _connectionString = connectionString;
            var a = new List<(string, int)>();
            a.Add(("a", 1));
        }

        public FuzzyModel GetData(string tableName, List<(string, int)>attrs)
        {
            var attrsName = new List<string>();
            var attrsIndexes = new List<int>();
            foreach (var attr in attrs)
            {
                attrsName.Add(attr.Item1);
                attrsIndexes.Add(attr.Item2);
            }

            var m = new FuzzyModel(attrsName);

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    string select = "Select * from " + tableName; 
                    SqlCommand cmd = new SqlCommand(select, con);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    int id = 0;
                    while (rdr.Read())
                    {
                        
                        var a = new double[attrsIndexes.Count+1];
                        a[0] = id;
                        id++;
                        for(int i = 1; i < attrsIndexes.Count+1; i++)
                        {
                            a[i] = Math.Round(Convert.ToDouble(rdr[attrsIndexes[i-1]]), 2);
                        }


                        m.Add(a);

                        //if (id > 1000) break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return m;
        }
    }
}
