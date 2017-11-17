using System;
using System.Windows;

namespace SE_Coursework
{
    /// <summary>
    /// Interaction logic for SentMessageOutput.xaml
    /// </summary>
    public partial class SentMessageOutput : Window
    {
        public SentMessageOutput(String json)
        {
            InitializeComponent();
            jsonTXTblock.Text = json;
        }

        private void closeSentMessageOutputBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
