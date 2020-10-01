using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    /// <summary>
    /// Class used to store data from XML
    /// </summary>
    class CurrencyReport
    {
        #region Fields
        public string Subject;      // text of node Subject
        public string Sender;       // text of node Sender -> Name
        public string Date;         // value of attribute time of node Cube (child to node Cube)

        private int AmountOfCubes;  // amount of Cubes
        private List<Cube> Cubes;    // List of Cubes, private because field AmountOfCubes needs to be updated with it
        private string Report;       // String with all data organised
        #endregion

        #region Constructors
        public CurrencyReport()
        {
            Cubes = new List<Cube>();
        }
        #endregion

        #region Getters
        public int GetAmountOfCubes()   // get int AmountOfCubes
        {
            return AmountOfCubes;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// create new line in the string Cube.Report
        /// </summary>
        /// <param name="newLine"> Line that is added to the string </param>
        private void AddLine(string newLine)
        {
            Report = Report + newLine + '\n';
        }

        /// <summary>
        /// generate string Cube.Report
        /// </summary>
        private void CreateReport()
        {
            Report = "";    // clearing the variable

            // adding first lines of the report                                            
            AddLine("Subject: " + Subject);
            AddLine("Sender: " + Sender);
            AddLine("Date: " + Date);
            for (int i = 0; i < AmountOfCubes; i++)     // adding cubes - informations about rates
            {
                AddLine("Rate of " + Cubes[i].Currency + " to EUR: " + Cubes[i].Rate);
            }
            AddLine("------ fin ------");               // adding end line
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// add new Cube instance to the list
        /// </summary>
        /// <param name="curr"> Currency name abreviation </param>
        /// <param name="rate"> Currency rate compared to EUR as string </param>
        public void AddCube (string curr, string rate) 
        {
            double drate = double.Parse(rate, System.Globalization.CultureInfo.InvariantCulture);
            Cube c1 = new Cube(curr, drate);    // create new instance of Cube
            Cubes.Add(c1);                      // add new instance to the list
            AmountOfCubes = Cubes.Count;        // update information about amount of objects in the list
        }

        /// <summary>
        /// generates and returns Report
        /// </summary>
        /// <returns></returns>
        public string GetReport()   // get string Report
        {
            CreateReport(); // generate report
            return Report;
        }

        #endregion

    }
}
