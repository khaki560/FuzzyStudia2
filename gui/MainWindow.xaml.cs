using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using lib;

namespace gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> connectors;
        private List<string> modifiers;
        private List<string> summarizers;
        private List<string> quantifiers;
        private List<string> qualifiers;

        string[] qualityNames;

        private List<SummarizerControl> summary = new List<SummarizerControl>();

        public void init()
        {
            LinguisticManager.init(TBSettingsPath.Text);
            connectors = LinguisticManager.GetConnectorsName();
            connectors.Insert(0, "");
            modifiers = LinguisticManager.GetModifiersName();
            modifiers.Insert(0, "");
            summarizers = LinguisticManager.GetSummarizersName();
            summarizers.Insert(0, "");
            quantifiers = LinguisticManager.GetQuantifiersName();
            quantifiers.Insert(0, "");
            qualifiers = LinguisticManager.GetQualifiersName();
            qualifiers.Insert(0, "");
            AddSummary(1);
        }
        public MainWindow()
        {
            qualityNames = new string[11] { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11"};
            InitializeComponent();
            init();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LinguisticSummary[] summaryArray = new LinguisticSummary[summary.Count];
            string[] results = new string[summary.Count];
            string outputFile = TBOutputPath.Text;

            int i = 0;
            foreach (var su in summary)
            {
                var q = su.GetQuantifierParameters();
                var w = su.GetQualifierParameters();
                var s = su.GetSummarizerParameters();

                string qS = q.modifiers[0] + "," + q.modifiers[1] + "," + q.value;
                var qList = new List<string>();
                qList.Add(qS);

                string wS1 = w[0].modifiers[0] + "," + w[0].modifiers[1] + "," + w[0].value + ',' + w[1].connector;
                string wS2 = w[1].modifiers[0] + "," + w[1].modifiers[1] + "," + w[1].value;
                var wList = new List<string>();
                wList.Add(wS1);
                if(!String.IsNullOrEmpty(w[1].connector))
                {
                    wList.Add(wS2);
                }

                string sS1 = s[0].modifiers[0] + "," + s[0].modifiers[1] + "," + s[0].value + ',' + s[1].connector;
                string sS2 = s[1].modifiers[0] + "," + s[1].modifiers[1] + "," + s[1].value;

                var sList = new List<string>();
                sList.Add(sS1);
                if (!String.IsNullOrEmpty(s[1].connector))
                {
                    sList.Add(sS2);
                }

                summaryArray[i] = LinguisticManager.CreateSummary(qList, wList, sList);
                i++;
            }

            SaveToFile(summaryArray, outputFile);
        }

        private void SaveToFile(LinguisticSummary[] ob, string outFile)
        {
            if (File.Exists(outFile))
            {
                File.Delete(outFile);
            }

            using (StreamWriter outputFile = new StreamWriter(outFile))
            {
                var weights = GetQualityWeights();
                for (int i = 0; i < ob.Length; i++)
                {
                    var summary = ob[i];
                    var s = summary.MakeSummary(LinguisticManager.GetData());
                    string qualityR = "Quality Result: ";
                    string qualityW = "Quality Weights: ";
                    for (int j = 0; j < weights.Length; j++ )
                    {
                        var q = summary.CreateQualityMeasure(j, LinguisticManager.GetData());
                        qualityR += qualityNames[j] + ": " + q.ToString() + " ";
                        qualityW += qualityNames[j] + ": " + weights[j].ToString() + " ";  
                    }
                    outputFile.WriteLine(s);
                    outputFile.WriteLine(qualityR);
                    outputFile.WriteLine(qualityW);
                    int[] sequence = Enumerable.Range(0, weights.Length).ToArray();
                    outputFile.WriteLine(summary.CreateFinalQualityMeasure(sequence, weights, LinguisticManager.GetData()));
                }
            }
        }

        private double[] GetQualityWeights()
        {
            var result = new double[11];
            result[0] = double.Parse(TBT1.Text);
            result[1] = double.Parse(TBT2.Text);
            result[2] = double.Parse(TBT3.Text);
            result[3] = double.Parse(TBT4.Text);
            result[4] = double.Parse(TBT5.Text);
            result[5] = double.Parse(TBT6.Text);
            result[6] = double.Parse(TBT7.Text);
            result[7] = double.Parse(TBT8.Text);
            result[8] = double.Parse(TBT9.Text);
            result[9] = double.Parse(TBT10.Text);
            result[10] = double.Parse(TBT11.Text);


            double sum = result.Sum();
            for(int i = 0; i < result.Length; i++)
            {
                result[i] /= sum;
            }

            return result;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void refresh()
        {
            foreach(var el in summary)
            {
                SPMain.Children.Remove(el);
                SPMain.Children.Add(el);
            }
            
        }

        private void AddSummary(int number)
        {
            foreach (var el in summary)
            {
                SPMain.Children.Remove(el);
            }
            summary.Clear();
            for (int i = 0; i < number; i++)
            {
                summary.Add(new SummarizerControl(i + 1, modifiers, quantifiers, qualifiers, summarizers, connectors));
            }
            refresh();
        }
        private void BSummariesNumber_Click(object sender, RoutedEventArgs e)
        {
            var number = int.Parse(TBSummariesNumber.Text);
            AddSummary(number);
        }

        private void BOSettingsPath_Click(object sender, RoutedEventArgs e)
        {
            init();
        }
    }
}
