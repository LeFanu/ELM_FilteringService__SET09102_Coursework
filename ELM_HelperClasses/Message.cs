﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

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
    [DataContract]
    [KnownType(typeof(SMS))]
    [KnownType(typeof(Tweet))]
    [KnownType(typeof(Email))]
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
            String json = new JavaScriptSerializer().Serialize(this);

            dbAccess.SaveJSONfile(json);
            return json;
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
