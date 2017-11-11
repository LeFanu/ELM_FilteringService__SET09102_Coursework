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
using System.Windows.Shapes;
using ELM_FilteringService;

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
