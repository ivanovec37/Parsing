using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Parsing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            VacancyListBox.Items.Clear();
            using var client = new HttpClient();
            string htmlBody = await client.GetStringAsync("https://proglib.io/vacancies/all?workType=all&workPlace=all&experience=&salaryFrom=&page=");

            string pattern = "title=\"(.+)20(.+)\"";
            string pattern1 = "\"title\">(.+)<\\/h2>";
            string pattern2 = "<a href=\"/vacancies/(.+)\" class=\"no-link\">";
            string pattern3 = "data-total=\"(.+)\"";

            Regex regex3 = new Regex(pattern3);
            MatchCollection matches3 = regex3.Matches(htmlBody + "1");

            int count = int.Parse(matches3[0].Groups[1].Value);
            Regex regex = new Regex(pattern);
            Regex regex1 = new Regex(pattern1);
            Regex regex2 = new Regex(pattern2);
            
            for (int i = 0; i < count; i++)
            {
                List<string> items = new();

                MatchCollection matches1 = regex1.Matches(htmlBody + i.ToString());
                foreach (Match match1 in matches1)
                {
                    items.Add(match1.Groups[1].Value + "\n");
                }
                MatchCollection matches = regex.Matches(htmlBody + i.ToString());
                for (int j = 0; j < items.Count; j++)
                {
                    items[j] += matches[j].Groups[1].Value + "\n";
                }

                MatchCollection matches2 = regex2.Matches(htmlBody + i.ToString());
                for (int j = 0; j < items.Count; j++)
                {
                    items[j] += "https://proglib.io/vacancies/" + matches2[j].Groups[1].Value + "\n";
                }
                
                for (int j = 0; j < items.Count; j++)
                {
                    VacancyListBox.Items.Add(items[j]);
                }
                htmlBody = await client.GetStringAsync($"https://proglib.io/vacancies/all?workType=all&workPlace=all&experience=&salaryFrom=&page={i}");
            }


        }
    }
}
