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

        public override string header { get; set; }
        public override string body { get; set; }
        public override string sender { get; set; }
        public override int bodyMAX_Length { get; set; }

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
    }
}