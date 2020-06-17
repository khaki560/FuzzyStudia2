using System;
using System.Collections.Generic;
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

namespace gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<SummarizerControl> summarizers = new List<SummarizerControl>();
        private List<List<string>> ParseXml()
        {
            var modifiers = new List<string>   {"", "NOT", "Absolutely" };
            var quantifiers = new List<string> {"", "Over 100", "half" };
            var qualifiers = new List<string>  {"", "salary over 1000", "small salary" };
            var summarizers = new List<string> {"", "young", "old" };
            var connectors = new List<string>  {"", "AND", "OR" };

            return new List<List<string>> { modifiers, quantifiers, qualifiers, summarizers, connectors };
        }

        public MainWindow()
        {
            InitializeComponent();
            var a = ParseXml();

            var s1 = new SummarizerControl(1, a[0], a[1], a[2], a[3], a[4]);
            SPMain.Children.Add(s1);
            var s2 = new SummarizerControl(2, a[0], a[1], a[2], a[3], a[4]);
            SPMain.Children.Add(s2);

            summarizers.Add(s1);
            summarizers.Add(s2);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var a = summarizers[0].GetQuantifierParameters();
            var b = summarizers[0].GetId();
            var c = summarizers[0].GetQualifierParameters();
            var d = summarizers[0].GetSummarizerParameters();

            var z = summarizers[1].GetQuantifierParameters();
            var f = summarizers[1].GetId();
            var g = summarizers[1].GetQualifierParameters();
            var h = summarizers[1].GetSummarizerParameters();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
