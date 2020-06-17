using System;
using System.Collections.Generic;
using System.Linq;
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

namespace gui
{

    public class SummaryParameters
    {
        public List<string> modifiers = new List<string>();
        public string value;
        public string connector;
    }

    /// <summary>
    /// Interaction logic for SummarizerControl.xaml
    /// </summary>
    public partial class SummarizerControl : UserControl
    {
        private int id; 
        public SummarizerControl(int id, List<string> modifiers, List<string> quantifiers, List<string> qualifiers, List<string> summarizers, List<string> connectors)
        {
            this.id = id;
            InitializeComponent();

            LName.Content = "Summarizer " + id.ToString();

            // Modifiers
            CBQuantifier1.ItemsSource = modifiers;
            CBQuantifier2.ItemsSource = modifiers;

            CBQualifier1.ItemsSource = modifiers;
            CBQualifier2.ItemsSource = modifiers;
            CBQualifier5.ItemsSource = modifiers;
            CBQualifier6.ItemsSource = modifiers;

            CBSummarizer1.ItemsSource = modifiers;
            CBSummarizer2.ItemsSource = modifiers;
            CBSummarizer5.ItemsSource = modifiers;
            CBSummarizer6.ItemsSource = modifiers;

            // Quantifiers
            CBQuantifier3.ItemsSource = quantifiers;

            // Qualifiers
            CBQualifier3.ItemsSource = qualifiers;
            CBQualifier7.ItemsSource = qualifiers;

            // Summarizers
            CBSummarizer3.ItemsSource = summarizers;
            CBSummarizer7.ItemsSource = summarizers;

            // Connectors
            CBSummarizer4.ItemsSource = connectors;
            CBQualifier4.ItemsSource = connectors;
        }

        public int GetId()
        {
            return id;
        }

        public SummaryParameters GetQuantifierParameters()
        {
            var a = new SummaryParameters()
            {
                modifiers = new List<string> { CBQuantifier1.Text, CBQuantifier2.Text },
                value = CBQuantifier3.Text,
            };
            return a;
        }

        public List<SummaryParameters> GetQualifierParameters()
        {
            var a1 = new SummaryParameters()
            {
                modifiers = new List<string> { CBQualifier1.Text, CBQualifier2.Text },
                value = CBQualifier3.Text,
                connector = "",
            };

            var a2 = new SummaryParameters()
            {
                modifiers = new List<string> { CBQualifier5.Text, CBQualifier6.Text },
                value = CBQualifier7.Text,
                connector = CBQualifier4.Text,
            };
            return new List<SummaryParameters> { a1, a2 };
        }

        public List<SummaryParameters> GetSummarizerParameters()
        {
            var a1 = new SummaryParameters()
            {
                modifiers = new List<string> { CBSummarizer1.Text, CBSummarizer2.Text },
                value = CBSummarizer3.Text,
                connector = "",
            };

            var a2 = new SummaryParameters()
            {
                modifiers = new List<string> { CBSummarizer5.Text, CBSummarizer6.Text },
                value = CBSummarizer7.Text,
                connector = CBSummarizer4.Text,
            };

            return new List<SummaryParameters> { a1, a2 };
        }

    }
}
