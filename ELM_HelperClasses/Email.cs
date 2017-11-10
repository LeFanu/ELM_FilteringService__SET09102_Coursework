using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;


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


namespace ELM_HelperClasses
{
    public class Email : Message
    {

//------------------------------------------ Instance Fields -------------------------------------------------------------------
        DataBaseAccess_Singleton dataBaseAccess = DataBaseAccess_Singleton.DbAccess;
        private String subject;
        private EmailType typeOfEmail;
        public static int messageMAX_Length = 1028;
        public override string header { get; set; }
        public override string body { get; set; }
        public override string sender { get; set; }
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

//__________________________________________ Class Constructor __________________________________________________________________

        private Email(String header, String messageBody, String sender, String subject)
        {
            this.header = header;
            this.sender = sender;
            this.subject = subject;
            body = messageBody;
            typeOfEmail = checkEmailType(subject);
            quarantineURL();
        }

        public static Email ValidateBeforeCreatingEmail(String header, String messageBody, String sender, String subject)
        {
            MessageFactory.sendingMessageError = "Email could not be sent. ";

            //checking subject field length
            if (subject.Length > 20 || subject.Length <=0)
            {
                MessageFactory.sendingMessageError += "\nSubject field is missing or too long. ";
                return null;
            }
            //checking message body length
            if (messageBody.Length > messageMAX_Length)
            {
                MessageFactory.sendingMessageError += "\nContent of the message is too long. It can be only 1028 characters. ";
                return null;
            }
            //checking if header or sender strings are empty
            if (String.IsNullOrEmpty(header) || String.IsNullOrEmpty(sender))
            {
                MessageFactory.sendingMessageError += "\nSender field cannot be empty. Invalid message ID";
                return null;
            }

            //checking SIR email subject format
            if (subject.StartsWith("SIR"))
            {
                String sirDateString = subject.Substring(4);
                DateTime sirDate = new DateTime();
                if (!DateTime.TryParse(sirDateString, out sirDate))
                {
                    MessageFactory.sendingMessageError += "\nInvalid format of the SIR email message. ";
                    return null;
                }
                if(sirDate.Year > DateTime.Now.Year)
                {
                    MessageFactory.sendingMessageError += "\nDate of accident cannot be set in future. ";
                    return null;
                }
            }

            return new Email(header, messageBody, sender, subject);
        }

        //|||||||||||||||||||||||||||||||||||||||||| Instance Methods |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        protected override void processMessage()
        {
            throw new System.NotImplementedException();
        }

        protected override void exportToJSON()
        {
            throw new System.NotImplementedException();
        }

        public void quarantineURL()
        {
            String quarantinedMessageBody = "";
            String quarantine = "<URLquarantined>";
            var splittedMessageBody = body.Split("\t\n ".ToCharArray());
            foreach (string s in splittedMessageBody)
            {
                if (s.StartsWith("http://") || s.StartsWith("www.") || s.StartsWith("https://"))
                {
                    //adding URL to list of quarantined URLs
                    dataBaseAccess.QuarantinedURLs.Add(s);
                    quarantinedMessageBody += (quarantine + " ");
                    continue;
                }
                quarantinedMessageBody += (s + " ");
            }
            MessageBox.Show(quarantinedMessageBody);
            body = quarantinedMessageBody;
        }

        public void saveIncidentToSIRList()
        {
           
            dataBaseAccess.SiRaccidents.Add();

        }

        private EmailType checkEmailType(String subject)
        {
            if (subject.StartsWith("SIR"))
            {
                return EmailType.SignificantIncidentReport;
            }
            return EmailType.StandardEmail;
        }
    }
}