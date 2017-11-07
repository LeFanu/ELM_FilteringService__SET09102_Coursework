using System;
using System.Collections.Generic;
using System.Linq;
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
            senderInputWarningLBL.Visibility = Visibility.Hidden;
            charactersLeftLBL.Visibility = Visibility.Hidden;
            emailSubjectTXT.Visibility = Visibility.Hidden;
            subjectLabel.Visibility = Visibility.Hidden;
            sendMessageBTN.IsEnabled = false;
        }

        private bool isTweet;
        private bool isSMS;
        private bool isEmail;
        private String senderInputContent;
        private String messageBody;


        private void sendMessageBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                senderInputContent = senderTXT.Text;
                String messageID = createMessageID(senderInputContent);
                if (messageID.Equals("INVALID_ID"))
                {
                    senderInputWarningLBL.Visibility = Visibility.Visible;
                    senderInputWarningLBL.Content =
                        "Please enter valid sender as Phone Number, Tweeter ID or email address";
                }
                MessageBox.Show(messageID);
            }
            catch (Exception exception)
            {
                
            }

        }

        private String createMessageID(String senderInputContent)
        {
            char firstCharacter = senderInputContent[0];

            if (isTweet)
            {
                //this is a Tweet
                MessageBox.Show("This is a Tweet. First char " + firstCharacter);
                return "T" + generateMessageIDnumber();
            }
            if (isEmail)
            {
                //this is an email   
                return "E" + generateMessageIDnumber();
            }
            if (isSMS)
            {
                return "S" + generateMessageIDnumber();
            }

            //if none of the above the sender format is invalid
            return "INVALID_ID";
        }

        private String generateMessageIDnumber()
        {
            Random rand  = new Random();
            int randomNum;
            String messageIDnumber = "";
            for (int i = 0; i < 8; i++)
            {
                randomNum = rand.Next(0, 10);
                messageIDnumber += randomNum;
            }

            return messageIDnumber;
        }

        private bool checkIfItIsAnEmail(String senderInput)
        {
            return (Regex.IsMatch(senderInput, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase) ? true : false);
            
        }

        private bool checkIfPhoneNumber(String senderInput)
        {
            //int phoneNumberParsed = -1;
            char firstCharacter = senderInput[0];
            //it's a phone number
            if (firstCharacter == 43 || firstCharacter == 48)
            {
                //if first character is + then the rest must be a phone number
                //try
                //{
                //    MessageBox.Show("Substring " + senderInputContent.Substring(1));
                //    phoneNumberParsed = Int32.Parse(senderInputContent.Substring(1));
                //    MessageBox.Show("Parsed " + senderInputContent.Substring(1));
                //    MessageBox.Show("This is a Text. First char " + firstCharacter);
                //    return true;
                //}
                //catch (Exception)
                //{
                //    MessageBox.Show("Invalid phone number " + senderInputContent);
                //    return false;

                //}
            }
                //try
                //{
                //    phoneNumberParsed = Int32.Parse(senderInputContent);
                //    return true;
                //}
                //catch (Exception)
                //{
                //    MessageBox.Show("Invalid phone number " + senderInputContent);
                //    return false;

                //}

            
            return false;
        }


        private int messageMaximumLength;

        private void senderTXT_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (senderTXT.Text.Length > 0)
            {
                sendMessageBTN.IsEnabled = IsEnabled;
                char firstCharacter = senderTXT.Text[0];

                if (firstCharacter == 64)
                {
                    //this is a Tweet
                    isTweet = true;
                    senderTXT.MaxLength = 16;
                    messageMaximumLength = 140;
                    
                    
                }
                if (checkIfPhoneNumber(senderTXT.Text))
                {
                    isSMS = true;
                    senderTXT.MaxLength = 11;
                    messageMaximumLength = 140;
                }
                if (checkIfItIsAnEmail(senderTXT.Text))
                {
                    int emailLength = senderTXT.Text.LastIndexOf(".") + 4;
                    isEmail = true;
                    senderTXT.MaxLength = emailLength;
                    messageMaximumLength = 1028;
                    emailSubjectTXT.Visibility = Visibility.Visible;
                    subjectLabel.Visibility = Visibility.Visible;
                    emailSubjectTXT.MaxLength = 20;
                }
                charactersLeftLBL.Visibility = Visibility.Visible;
            }
            else
            {
                sendMessageBTN.IsEnabled = false;
                isTweet = false;
                isSMS = false;
                isEmail = false;
                senderTXT.MaxLength = 200;
                charactersLeftLBL.Visibility = Visibility.Hidden;
                emailSubjectTXT.Visibility = Visibility.Hidden;
                subjectLabel.Visibility = Visibility.Hidden;

            }
            calculateMessageBodyRemainingCharacters();
            messageBodyTXT.MaxLength = messageMaximumLength;
            senderInputWarningLBL.Visibility = Visibility.Hidden;

        }

        private void messageBodyTXT_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculateMessageBodyRemainingCharacters();
        }

        private void calculateMessageBodyRemainingCharacters()
        {
            charactersLeftLBL.Content = messageMaximumLength - messageBodyTXT.Text.Length;
        }
    }
}
