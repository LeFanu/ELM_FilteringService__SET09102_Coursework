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
        //variables used for operations with UI
        private DataBaseAccess_Singleton dbAccess;
        private bool isTweet;
        private bool isSMS;
        private bool isEmail;
        private String senderInputContent;
        private String messageBody;
        private int messageMaximumLength;

        public MainWindow()
        {
            InitializeComponent();
            //initialize objects and properties to the default state
            dbAccess = DataBaseAccess_Singleton.DbAccess;
            //hiding some components as they will be visible only when needed
            senderInputWarningLBL.Visibility = Visibility.Hidden;
            charactersLeftLBL.Visibility = Visibility.Hidden;
            emailSubjectTXT.Visibility = Visibility.Hidden;
            subjectLabel.Visibility = Visibility.Hidden;
            sendingLoadingErrorMessage.Visibility = Visibility.Hidden;
            sendMessageBTN.IsEnabled = false;
        }


        //logic that happens when user tries to send the message
        private void sendMessageBTN_Click(object sender, RoutedEventArgs e)
        {
            //surrounded with try catch in case if somehow there would be no sender input content
            try
            {
                senderInputContent = senderTXT.Text;
                //trying to create message ID only if the input is valid
                String messageID = createMessageID();
                
                //if it returns invalid we display error message to the user.
                if (messageID.Equals("INVALID_ID"))
                {
                    senderInputWarningLBL.Visibility = Visibility.Visible;
                    senderInputWarningLBL.Content =
                        "Please enter valid sender as Phone Number, Tweeter ID or email address";
                }
                else
                {
                    MessageFactory messageFactory = new MessageFactory();
                    
                    Message messageToSend = messageFactory.createNewMessageToSend(messageID,messageBodyTXT.Text, senderTXT.Text, emailSubjectTXT.Text);
                    if (messageToSend == null)
                    {
                        
                        sendingLoadingErrorMessage.Visibility = Visibility.Visible;
                        sendingLoadingErrorMessage.Content = MessageFactory.sendingMessageError;
                    }

                }
                //MessageBox.Show(messageID);
            }
            //user cannot click the button if sender field is empty.
            //however if this would be somehow possible this catch block will catch it
            catch (Exception exception)
            {
                sendingLoadingErrorMessage.Visibility = Visibility.Visible;
                sendingLoadingErrorMessage.Content =
                   "Please enter valid sender as Phone Number, Tweeter ID or email address";
                MessageBox.Show(exception.ToString());
            }

        }

        //trying to create mnessage ID if the input is valid
        private String createMessageID()
        {
            //sender input is checked during the change of content. Thereofre when send message button is clicked bool variables are already set
            if (isTweet)
            {
                //this is a Tweet
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

        //method to generate random 8 digits for the messageID
        private String generateMessageIDnumber()
        {
            Random rand = new Random();
            int randomNum;
            String messageIDnumber = "";
            for (int i = 0; i < 8; i++)
            {
                randomNum = rand.Next(0, 10);
                messageIDnumber += randomNum;
            }
            return messageIDnumber;
        }


        //regEx checking for email address format
        private bool checkIfItIsAnEmail(String senderInput)
        {
            return (Regex.IsMatch(senderInput, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase) ? true : false);   
        }


        //checking if sender input is a phone number
        private bool checkIfPhoneNumber(String senderInput)
        {

            char firstCharacter = senderInput[0];
            //if it starts from 0 or + it's a phone number
            if (firstCharacter == 43 || firstCharacter == 48)
            {
                //checking remaining characters for being digits
                if (senderInput.Substring(1).All(char.IsDigit))
                {
                    return true;
                }
            }
            return false;
        }




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
                    messageMaximumLength = Tweet.messageMAX_Length; 
                }
                else
                {
                    isTweet = false;
                }
                if (checkIfPhoneNumber(senderTXT.Text))
                {
                    isSMS = true;
                    senderTXT.MaxLength = 11;
                    messageMaximumLength = SMS.bodyMAX_Length;
                }
                else
                {
                    isSMS = false;
                }
                if (checkIfItIsAnEmail(senderTXT.Text))
                {
                    int emailLength = senderTXT.Text.LastIndexOf(".") + 4;
                    isEmail = true;
                    senderTXT.MaxLength = emailLength;
                    messageMaximumLength = Email.messageMAX_Length;
                    emailSubjectTXT.Visibility = Visibility.Visible;
                    subjectLabel.Visibility = Visibility.Visible;
                    emailSubjectTXT.MaxLength = 20;
                }
                else
                {
                    isEmail = false;
                    emailSubjectTXT.Visibility = Visibility.Hidden;
                    subjectLabel.Visibility = Visibility.Hidden;
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
            sendingLoadingErrorMessage.Visibility = Visibility.Hidden;

        }

        //when user types in the message the remaining characters counter changes
        private void messageBodyTXT_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculateMessageBodyRemainingCharacters();
        }

        private void calculateMessageBodyRemainingCharacters()
        {
            charactersLeftLBL.Content = messageMaximumLength - messageBodyTXT.Text.Length;
        }

        private void emailSubjectTXT_TextChanged(object sender, TextChangedEventArgs e)
        {
            sendingLoadingErrorMessage.Visibility = Visibility.Hidden;
        }
    }
}
