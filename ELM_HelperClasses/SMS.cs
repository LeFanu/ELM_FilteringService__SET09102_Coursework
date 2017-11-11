using System;
using System.Linq;
using System.Web.Script.Serialization;


namespace ELM_HelperClasses
{
    /** Author: Karol Pasierb - Software Engineering - 40270305
   * Created by Karol Pasierb on 2017/10/08
   *
   ** Description:
   *   
   ** Future updates:
   *   
   ** Design Patterns Used:
   *   
   *
   ** Last Update: 27/10/2017
   */

    public class SMS : Message
    {


//------------------------------------------ Instance Fields -------------------------------------------------------------------

        DataBaseAccess_Singleton dataBaseAccess = DataBaseAccess_Singleton.DbAccess;

        public override string header { get; set; }
        public override string body { get; set; }
        public override string sender { get; set; }
        public static int bodyMAX_Length = 140;

//__________________________________________ Class Constructor __________________________________________________________________

        private SMS(String header, String messageBody, String sender)
        {
            this.header = header;
            body = messageBody;
            this.sender = sender;
        }

        public static SMS ValidateBeforeCreatingSms(String header, String messageBody, String sender)
        {
            MessageFactory.sendingMessageError = "SMS could not be sent. ";

            if (messageBody.Length > bodyMAX_Length)
            {
                MessageFactory.sendingMessageError += "\nContent of the message cannot be longer than 140 characters. ";
                return null;
            }
            if (String.IsNullOrEmpty(header) || String.IsNullOrEmpty(sender))
            {
                MessageFactory.sendingMessageError += "\nSender field cannot be empty. Invalid message ID";
                return null;
            }
            if (!sender.All(char.IsDigit))
            {
                MessageFactory.sendingMessageError += "\nSender field can contain only digits";
                return null;
            }
            return new SMS(header, messageBody, sender);
        }
//|||||||||||||||||||||||||||||||||||||||||| Instance Methods |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        public override void processMessage()
        {
            processTextspeakInMessageBody();

        }

    }
}