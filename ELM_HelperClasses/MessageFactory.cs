using System;

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
                return SMS.ValidateBeforeCreatingSms(header, messageBody, sender);
            }
            if (firstLetter == 84)
            {
                return Tweet.ValidateBeforeCreatingSms(header, messageBody, sender);
            }
            if (firstLetter == 69)
            {
                return Email.ValidateBeforeCreatingEmail(header, messageBody, sender, subject);
            }
            return null;
        }
    }
}