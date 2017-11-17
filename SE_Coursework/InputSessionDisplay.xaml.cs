using System;
using System.Linq;
using System.Windows;
using ELM_HelperClasses;

namespace SE_Coursework
{
    /// <summary>
    /// Interaction logic for InputSessionDisplay.xaml
    /// </summary>
    public partial class InputSessionDisplay : Window
    {
        private DataBaseAccess_Singleton dataBaseAccess;
        public InputSessionDisplay()
        {
            InitializeComponent();
            dataBaseAccess = DataBaseAccess_Singleton.DbAccess;

            displayHashtagOccurrences();
            displayMentionsOccurrences();
            displaySIRincidents();
        }

        private void closeApplication_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void displayHashtagOccurrences()
        {

            var g = dataBaseAccess.Hashtags.GroupBy(i => i);
            String occurences = "";
            foreach (var grp in g)
            {
                occurences = "\n" + grp.Key + " -  Popularity: " + grp.Count();
                hashtagsListBox.Items.Add(occurences);

            }
        }

        private void displayMentionsOccurrences()
        {

            var g = dataBaseAccess.Mentions.GroupBy(i => i);
            String occurences = "";
            foreach (var grp in g)
            {
                occurences = "\n" + grp.Key + ": " + grp.Count();
                mentionsListBox.Items.Add(occurences);
            }
        }

        private void displaySIRincidents()
        {
            foreach (string siRaccident in dataBaseAccess.SiRaccidents)
            {
                SIRincidentsListBox.Items.Add("\n" +siRaccident);
            }
        }
    }
}
