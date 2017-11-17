using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;



/** Author: Karol Pasierb - Software Engineering - 40270305
   * Created by Karol Pasierb on 2017/10/08
   */


namespace ELM_HelperClasses
{
    [DataContract(Namespace = "ELM_HelperClassesJSON")]
    public class Email : Message
    {

//------------------------------------------ Instance Fields -------------------------------------------------------------------
        DataBaseAccess_Singleton dataBaseAccess = DataBaseAccess_Singleton.DbAccess;

        private NatureOfIncident natureOfIncident;

        [DataMember]
        private String subject;
        private EmailType typeOfEmail;
        public static int messageMAX_Length = 1028;
        [DataMember]
        public override string header { get; set; }
        [DataMember]
        public override string body { get; set; }
        [DataMember]
        public override string sender { get; set; }
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public EmailType TypeOfEmail
        {
            get { return typeOfEmail; }
        }

//__________________________________________ Class Constructor __________________________________________________________________

        private Email(String header, String messageBody, String sender, String subject)
        {
            this.header = header;
            this.sender = sender;
            this.subject = subject;
            body = @""+messageBody;
            typeOfEmail = checkEmailType(subject);
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
                String sirDateString = "";
                try
                {
                    //checking format of the date
                    sirDateString = subject.Substring(4);
                }
                catch (Exception)
                {

                }
                DateTime sirDate = new DateTime();
                if (!DateTime.TryParse(sirDateString, out sirDate))
                {
                    MessageFactory.sendingMessageError += "\nInvalid format of the SIR email message. Date is wrong or missing. ";
                    return null;
                }
                if(sirDate.Year > DateTime.Now.Year)
                {
                    MessageFactory.sendingMessageError += "\nDate of accident cannot be set in future. ";
                    return null;
                }
                //--------------------------------------------------------------------------------------------------------------
                //Check sports centre format
                string sportCentreCode;
                Regex codePattern = new Regex(@"^\d{2}-\d{3}-\d{2}$");
                try
                {
                    //checking if Sports centre code is in proper format
                    sportCentreCode = messageBody.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    //splitting the string after colon and following space to get just the code of incident
                    sportCentreCode = sportCentreCode.Substring(sportCentreCode.IndexOf(':') + 2);
                }
                catch (Exception)
                {
                    MessageFactory.sendingMessageError +=
                        "\nPlease enter valid Sports Centre Code in suggested format. Only digits. ";
                    return null;
                }

                bool validSportsCentreCode = codePattern.IsMatch(sportCentreCode);
                if (!validSportsCentreCode)
                {
                    MessageFactory.sendingMessageError += "\nPlease enter valid Sports Centre Code in suggested format. Only digits. ";
                    return null;
                }



                //---------------------------------------------------------------------------------------------------------
                //Check Nature of Incident Format
                string natureOfIncident = "";
                //in case if string would be empty
                try
                {

                    //checking if Nature of the incident is one of the accepted ones
                    natureOfIncident =
                        messageBody.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)[1];
                    //splitting the string after colon and following space to get just the code of incident
                    natureOfIncident = natureOfIncident.Substring(natureOfIncident.IndexOf(':') + 2);
                    //comparing our string with accepted incidents
                    
                }
                catch (Exception)
                {
                    MessageFactory.sendingMessageError += "Please supply Nature Of Incident from the accepted list";
                    return null;
                }
                bool validIncindent = iterateNOI(natureOfIncident);
                if (!validIncindent)
                {
                    MessageFactory.sendingMessageError +=
                        "\nPlease enter valid Incident. From the list of accepted ones. ";
                    return null;
                }
              
            }
            return new Email(header, messageBody, sender, subject);
        }


        private static bool iterateNOI(String incident)
        {
            Type type = typeof(NatureOfIncident); // MyClass is static class with static properties
            foreach (var p in type.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))
            {
                var v = p.GetValue(null); // static classes cannot be instanced, so use null...
                //do something with v

                if (incident.Equals(v.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        //|||||||||||||||||||||||||||||||||||||||||| Instance Methods |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        public override void processMessage()
        {
            quarantineURL();
            if (typeOfEmail == EmailType.SignificantIncidentReport)
            {
                saveIncidentToSIRList();
            }
        }



        private void quarantineURL()
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
            body = quarantinedMessageBody;
        }

        private void saveIncidentToSIRList()
        {
            //Split body content into lines to extract first two
            string sportsCentreCode = body.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
            sportsCentreCode = sportsCentreCode.Substring(sportsCentreCode.IndexOf(':') + 2);
            //save first line of the body as incident code
            //String sportCentreIncidentCode = bodyInLines[0];
            //save second line of the body as nature of incident
            string natureOfIncident = "";
            try
            {
                natureOfIncident =
                    body.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[1];
                natureOfIncident = natureOfIncident.Substring(natureOfIncident.IndexOf(':') + 2);
            }
            catch (Exception )
            {
            }


            dataBaseAccess.SiRaccidents.Add(sportsCentreCode + ", " + natureOfIncident);

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