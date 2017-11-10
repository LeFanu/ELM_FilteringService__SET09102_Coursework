
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


using System;

namespace ELM_HelperClasses
{
    public class Tweet : Message
    {

//------------------------------------------ Instance Fields -------------------------------------------------------------------
        DataBaseAccess_Singleton dataBaseAccess = DataBaseAccess_Singleton.DbAccess;
        private int MAXsenderLength;

        public int MaXsenderLength
        {
            get => MAXsenderLength;
            set => MAXsenderLength = value;
        }

        public override string header { get; set; }
        public override string body { get; set; }
        public override string sender { get; set; }
        public static int messageMAX_Length = 140;



//__________________________________________ Class Constructor __________________________________________________________________
        private Tweet(String header, String messageBody, String sender)
        {

        }

        public static Tweet ValidateBeforeCreatingSms(String header, String messageBody, String sender)
        {

            MessageFactory.sendingMessageError = "Tweet could not be sent. ";

            if (messageBody.Length > messageMAX_Length)
            {
                MessageFactory.sendingMessageError += "\nContent of the message cannot be longer than 140 characters. ";
                return null;
            }
            if (sender.Length > 16 || sender.Length < 2)
            {
                MessageFactory.sendingMessageError += "\nInvalid or too long sender content. ";
                return null;
            }
            if (String.IsNullOrEmpty(header) || String.IsNullOrEmpty(sender))
            {
                MessageFactory.sendingMessageError += "\nSender field cannot be empty. Invalid message ID";
                return null;
            }
            return new Tweet(header, messageBody, sender);
        }

        //|||||||||||||||||||||||||||||||||||||||||| Instance Methods |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        protected override void exportToJSON()
        {
            throw new System.NotImplementedException();
        }

        protected override void processMessage()
        {
            throw new System.NotImplementedException();
        }

        public void addHastagToList()
        {
            
        }

        public void addMentionToList()
        {
            
        }
    }
}