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

        private async void GetParallelData(int i)
        {
            
            Regex regex = new Regex("title=\"(.+)20(.+)\"");
            Regex regex1 = new Regex("\"title\">(.+)<\\/h2>");
            Regex regex2 = new Regex("<a href=\"/vacancies/(.+)\" class=\"no-link\">");
            using var client = new HttpClient();
            List<string> items = new();
            string htmlBody2 = await client.GetStringAsync($"https://proglib.io/vacancies/all?workType=all&workPlace=all&experience=&salaryFrom=&page={i}");

            MatchCollection matches1 = regex1.Matches(htmlBody2);
            foreach (Match match1 in matches1)
            {
                items.Add(match1.Groups[1].Value + "\n");
            }
            MatchCollection matches = regex.Matches(htmlBody2);
            for (int j = 0; j < items.Count; j++)
            {
                items[j] += matches[j].Groups[1].Value + "\n";
            }

            MatchCollection matches2 = regex2.Matches(htmlBody2);
            for (int j = 0; j < items.Count; j++)
            {
                items[j] += "https://proglib.io/vacancies/" + matches2[j].Groups[1].Value + "\n";
            }

            for (int j = 0; j < items.Count; j++)
            {
                Dispatcher.Invoke(() => VacancyListBox.Items.Add(items[j]));
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                VacancyListBox.Items.Clear();
                using var client = new HttpClient();
                string htmlBody = await client.GetStringAsync("https://proglib.io/vacancies/all?workType=all&workPlace=all&experience=&salaryFrom=&page=");
                Regex regex3 = new Regex("data-total=\"(.+)\"");
                MatchCollection matches3 = regex3.Matches(htmlBody + "1");

                int count = int.Parse(matches3[0].Groups[1].Value);

                List<int>pageNumbers = new List<int>();
                for(int i = 1; i < count+1;i++)
                {
                    pageNumbers.Add(i);
                }

                ParallelLoopResult result = Parallel.ForEach<int>(
        pageNumbers,GetParallelData);
               
            }
            catch (Exception ex)
            {
                
               MessageBox.Show(ex.Message);
            }


        }
    }
}
