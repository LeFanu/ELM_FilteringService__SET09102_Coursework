using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;



/** Author: Karol Pasierb - Software Engineering - 40270305
   * Created by Karol Pasierb on 2017/10/08
   */

namespace ELM_HelperClasses
{
    [DataContract]

    [KnownType(typeof(Tweet))]
    [KnownType(typeof(Email))]
    [KnownType(typeof(SMS))]
    public abstract class Message
    {
        DataBaseAccess_Singleton dbAccess = DataBaseAccess_Singleton.DbAccess;

        [DataMember]
        public abstract String header { get; set; }
        [DataMember]
        public abstract String body { get; set; }
        [DataMember]
        public abstract String sender { get; set; }


//__________________________________________ Class Constructor __________________________________________________________________


        public abstract void processMessage();

        public String exportToJSON()
        {

            MemoryStream memoryStream = new MemoryStream();
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Message));
            jsonSerializer.WriteObject(memoryStream, this);
            memoryStream.Position = 0;
            StreamReader sr = new StreamReader(memoryStream);
            string jsonSerialized = sr.ReadToEnd();

            sr.Close();
            memoryStream.Close();
            dbAccess.SaveJSONfile(jsonSerialized);
            return jsonSerialized;
        }

        private String expandTextAbbreviation(String word)
        {
            String expandedAbbreviation = "";
            foreach (var textspeak in dbAccess.TextSpeakAbbreviations)
            {
                if (word.ToUpper() == textspeak.Key)
                {
                    expandedAbbreviation = "<" + textspeak.Value + ">";
                }
            }
            return expandedAbbreviation;
        }

        protected void processTextspeakInMessageBody()
        {
            String[] words = body.Split("\t\n ".ToCharArray());
            String expandedAbbreviation = "";
            String newBody = "";
            foreach (string word in words)
            {
                expandedAbbreviation = expandTextAbbreviation(word);
                newBody += word + " " + expandedAbbreviation + " ";
            }
            body = newBody;
            
        }



    }
}
