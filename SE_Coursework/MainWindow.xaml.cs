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
using ELM_HelperClasses;


namespace ELM_FilteringService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataBaseAccess_Singleton dbAccess = DataBaseAccess_Singleton.DbAccess;
        }

        private String senderInputContent;
        private String messageBody;


        private void sendMessageBTN_Click(object sender, RoutedEventArgs e)
        {
            senderInputContent = senderTXT.Text;
            String messageID = createMessageID(senderInputContent);
            if (messageID.Equals("INVALID_ID"))
            {
                MessageBox.Show("Please enter valid sender as Phone Number, Tweeter ID or email address " + messageID);
            }

        }

        private String createMessageID(String senderInputContent)
        {
            char firstCharacter = senderInputContent[0];

            if (firstCharacter == 64)
            {
                //this is a Tweet
                MessageBox.Show("This is a Tweet. First char " + firstCharacter);
            }
            else if (checkIfItIsAnEmail(senderInputContent))
            {
                //this is an email   
            }
            else if (checkIfPhoneNumber(senderInputContent))
            {
                //it's a phone number
                if (firstCharacter == 43)
                {
                    MessageBox.Show("This is a Text. First char " + firstCharacter);
                }

            }

            //if none of the above the sender format is invalid
            return "INVALID_ID";
        }

        private int generateMessageIDnumber()
        {
            Random rand  = new Random();
            int messageIDnumber = rand.Next(0,10);

            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return 990099887;
        }

        private bool checkIfItIsAnEmail(String senderInput)
        {

            return false;
        }

        private bool checkIfPhoneNumber(String senderInput)
        {

            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }
    }
}
