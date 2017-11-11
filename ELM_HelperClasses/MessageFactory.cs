using System;

using System.Windows;


namespace ELM_HelperClasses
{
    public class MessageFactory
    {
        public static String sendingMessageError;

        public Message createNewMessageToSend(String header, String messageBody, String sender, String subject)
        {
            char firstLetter = header[0];

            if (firstLetter == 83)
            {
                SMS currentSms = SMS.ValidateBeforeCreatingSms(header, messageBody, sender);
                
                //MessageBox.Show(json);
                return currentSms;
            }
            if (firstLetter == 84)
            {
                Tweet currentTweet = Tweet.ValidateBeforeCreatingSms(header, messageBody, sender);
                if (currentTweet != null)
                {
                    
                    
                }
                return currentTweet;
            }
            if (firstLetter == 69)
            {
                Email currentEmail = Email.ValidateBeforeCreatingEmail(header, messageBody, sender, subject);
                if (currentEmail != null)
                {
                    
                }
                return currentEmail;
            }
            return null;
        }
    }
}