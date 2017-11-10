using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

/** Author: Karol Pasierb - Software Engineering - 40270305
   * Created by Karol Pasierb on 2017/10/08
   *
   ** Description:
   *   
   ** Future updates:
   *   
   ** Design Patterns Used:
   *   This class is a Singleton as we need only one helper class to access the database
   *   All of the objects will use the same data and there is no need for more classess to access it
   *   Also more instances of this class would only complicate things
   *
   ** Last Update: 27/10/2017
   */


namespace ELM_HelperClasses
{

    //[Serializable]
    public class DataBaseAccess_Singleton
    {

//------------------------------------------ Instance Fields -------------------------------------------------------------------
        private static DataBaseAccess_Singleton dbAccess;
        private List<String> hashtags;
        private List<String> mentions;
        private List<String> quarantinedURLs;
        private List<String> SIRaccidents;
        private List<int> messageIDs;

//___________________________________________________________________________________________________________________________________
        public static DataBaseAccess_Singleton DbAccess
        {
            get
            {
                if (dbAccess == null)
                {
                    dbAccess = new DataBaseAccess_Singleton();
                }
                return dbAccess;
            }
            
        }
        public List<string> Hashtags
        {
            get { return hashtags; }
            set { hashtags = value; }
        }
        public List<string> Mentions
        {
            get { return mentions; }
            set { mentions = value; }
        }
        public List<string> SiRaccidents
        {
            get { return SIRaccidents; }
            set { SIRaccidents = value; }
        }
        public List<int> MessageIDs
        {
            get { return messageIDs; }
            set { messageIDs = value; }
        }
        public List<string> QuarantinedURLs
        {
            get { return quarantinedURLs; }
            set { quarantinedURLs = value; }
        }

//__________________________________________ Class Constructor __________________________________________________________________
        private DataBaseAccess_Singleton()
        {
            ReadDB_OnStartup();

        }


//|||||||||||||||||||||||||||||||||||||||||| Instance Methods |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        
        //this method will read all stored data when the program starts
        public void ReadDB_OnStartup()
        {
            quarantinedURLs = new List<string>();
            try
            {

            }
            catch (Exception e)
            {

            }
        }

        //this method is responsible for saving data into the file
        public void SaveFiles()
        {

            try
            {

            }
            catch (Exception e)
            {

            }
        }
    }
}