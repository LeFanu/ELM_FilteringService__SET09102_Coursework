using System;
using System.Windows;
using ELM_HelperClasses;

namespace SE_Coursework
{
    /// <summary>
    /// Interaction logic for SentMessageOutput.xaml
    /// </summary>
    public partial class SentMessageOutput : Window
    {
        public SentMessageOutput(String json, Message messageToSend)
        {
            InitializeComponent();
            jsonTXTblock.Text = json;
            messageID_LBL.Content = messageToSend.header;
            senderLBL.Content = messageToSend.sender;
            if (messageToSend.GetType() == typeof(Email))
            {
                Email email = (Email) messageToSend;
                subjectLBL.Content = email.Subject;
            }
            else
            {
                subjectLBL.Content = "N/A";
            }
            messageBodyTXT.Text = messageToSend.body;
        }

        private void closeSentMessageOutputBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
