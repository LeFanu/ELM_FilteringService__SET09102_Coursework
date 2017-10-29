using System;
using System.Collections.Specialized;


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

        private String subject;

        public override string header { get; set; }
        public override string body { get; set; }
        public override string sender { get; set; }
        public override int bodyMAX_Length { get; set; }



        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

//__________________________________________ Class Constructor __________________________________________________________________



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
            
        }

        public void saveIncidentToSIRList()
        {
           


        }
    }
}