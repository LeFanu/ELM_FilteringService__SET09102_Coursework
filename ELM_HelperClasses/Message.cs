using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public abstract class Message
    {
        public abstract String header { get; set; }

        public abstract String body { get; set; }

        public abstract String sender { get; set; }

//__________________________________________ Class Constructor __________________________________________________________________

        protected Message()
        {
        }

        protected abstract void processMessage();
        protected abstract void exportToJSON();

        protected String expandTextAbbreviation(String messageTopExpand)
        {
            String expandedAbbreviation = messageTopExpand;

            return expandedAbbreviation;
        }

       // public abstract static bool ValidateData();
    }
}
