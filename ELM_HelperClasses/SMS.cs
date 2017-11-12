using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using System.Windows;


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
   [DataContract]
    public class SMS : Message
    {


//------------------------------------------ Instance Fields -------------------------------------------------------------------

        DataBaseAccess_Singleton dataBaseAccess = DataBaseAccess_Singleton.DbAccess;
        [DataMember]
        public override string header { get; set; }
        [DataMember]
        public override string body { get; set; }
        [DataMember]
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

        public String exportToJSON()
        {
            //String json = new JavaScriptSerializer().Serialize(this);
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(SMS));
            ser.WriteObject(stream1, this);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);

            // "{\"Description\":\"Share Knowledge\",\"Name\":\"C-sharpcorner\"}"  
            string json = sr.ReadToEnd();
            MessageBox.Show(json);
            sr.Close();
            stream1.Close();
            dataBaseAccess.SaveJSONfile(json);
            return json;
        }

    }
}