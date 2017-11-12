using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

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

    [Serializable]
    public class DataBaseAccess_Singleton
    {

//------------------------------------------ Instance Fields -------------------------------------------------------------------
        private static DataBaseAccess_Singleton dbAccess;
        private List<String> hashtags = new List<String>();
        private List<String> mentions = new List<String>();
        private List<String> quarantinedURLs = new List<String>();
        private List<String> SIRaccidents = new List<String>();
        private Dictionary<String, String> textSpeakAbbreviations = new Dictionary<String, String>();
        private List<int> messageIDs;
        private String[] textspeak;

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
        public List<string> QuarantinedURLs
        {
            get { return quarantinedURLs; }
            set { quarantinedURLs = value; }
        }
        public Dictionary<string, string> TextSpeakAbbreviations
        {
            get { return textSpeakAbbreviations; }
            set { textSpeakAbbreviations = value; }
        }

//__________________________________________ Class Constructor __________________________________________________________________
        private DataBaseAccess_Singleton()
        {
            //as this class is a singleton this method will be called just once and the Database content will be loaded into a program
            //with the program flow this constructor will be used during the loading of the main window
            //therefore the whole program will have access to all the data from the very begining
            ReadDB_OnStartup();
        }


//|||||||||||||||||||||||||||||||||||||||||| Instance Methods |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        //-----------READING FILES ON STARTUP AND CREATING LISTS----------------------------
        //fields and references for serialization
        [NonSerialized()]
        FileStream stream;
        [NonSerialized()]
        String filename;
        [NonSerialized()]
        BinaryFormatter formatter = new BinaryFormatter();


        //this method will read all stored data when the program starts
        private void ReadDB_OnStartup()
        {
            //try catch for reading data from file for each list
            try
            {
                // we are accessing our file
                filename = "ELMquarantinedURLs.data";
                stream = File.OpenRead(filename);
                try
                {
                    quarantinedURLs = (List<String>)formatter.Deserialize(stream);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                stream.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Unfortunately data file for Quarantined URLs do not exist. Please create new Database.", "Empty File", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            try
            {
                // we are accessing our file
                filename = "ELMhashtags.data";
                stream = File.OpenRead(filename);
                try
                {
                    hashtags = (List<String>)formatter.Deserialize(stream);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                stream.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Unfortunately data file for hashtags do not exist. Please create new Database.", "Empty File", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            try
            {
                // we are accessing our file
                filename = "ELMtweetMentions.data";
                stream = File.OpenRead(filename);
                try
                {
                    mentions = (List<String>)formatter.Deserialize(stream);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                stream.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Unfortunately data file for Tweet mentions do not exist. Please create new Database.", "Empty File", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            try
            {
                // we are accessing our file
                filename = "ELMsirIncidents.data";
                stream = File.OpenRead(filename);
                try
                {
                    SIRaccidents = (List<String>)formatter.Deserialize(stream);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                stream.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Unfortunately data file for SIR incidents do not exist. Please create new Database.", "Empty File", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            //reading all textspeak abbreviations into the pragram for later user
            try
            {
                filename = "textwords.csv";
                textspeak = File.ReadAllText(filename).Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string text in textspeak)
                {
                    //adding all abbreviations as key and their meaning as a value into the dictionary
                    var values = text.Split(new [] {','}, 2);
                    TextSpeakAbbreviations.Add(values[0], values[1]);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        //this method is responsible for saving data into the file
        public void SaveFiles()
        {

            try
            {
                filename = "ELMquarantinedURLs.data";
                stream = File.Create(filename);
                formatter.Serialize(stream, quarantinedURLs);
                stream.Close();
                //MessageBox.Show("ELMquarantinedURLs were saved!!!");


                filename = "ELMhashtags.data";
                stream = File.Create(filename);
                formatter.Serialize(stream, hashtags);
                stream.Close();
                //MessageBox.Show("ELMhashtags were saved!!!");

                filename = "ELMtweetMentions.data";
                stream = File.Create(filename);
                formatter.Serialize(stream, mentions);
                stream.Close();
                //MessageBox.Show("ELMtweetMentions were saved!!!");

                filename = "ELMsirIncidents.data";
                stream = File.Create(filename);
                formatter.Serialize(stream, SIRaccidents);
                stream.Close();
                //MessageBox.Show("ELMsirIncidents were saved!!!");
            }
            catch (Exception e)
            {

            }
        }

        public void SaveJSONfile(String json)
        {
            filename = "processedMessages.JSON";
            //write string to file
            File.AppendAllText(filename, json);
            
        }


    }
}