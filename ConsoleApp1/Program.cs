using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            /**
             **     Auxiliary Booleans
             **     true if xmlFile.Read is on proper node    
             **/
            bool subject = false;           // true on the end of loop cycle if in next cycle xmlFile will be on text in node gesmes:subject
            bool sender = false;            // true on the end of loop cycle if in next cycle xmlFile will be on text in node gesmes:name
            bool cubeFirstGen = false;      // true if accessing first generation of nodes Cube in file
            bool cubeSecondGen = false;     // true if accessing second generation of nodes Cube in file
            bool cubeThirdGen = false;      // true if accessing third generation of nodes Cube in file

            XmlReader xmlFile = XmlReader.Create("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");  // loading an XML file
            CurrencyReport Report = new CurrencyReport();                                                           // instance of a class storing data from XML

            // Reading XML file and filling fileds of a CurrencyReport instance
            while(xmlFile.Read())
            {
                /*
                 * reading text in node gesmes:subject
                 * order of if commands - if they were written the other way around the program would try to read the text immediately (while xmlFile is of NodeType Element)
                 */
                if (subject)    // if in previous cycle of loop the xmlFile was on node gesmes:subject then read text
                {
                    Report.Subject = xmlFile.ReadContentAsString(); // read text from node gesmes:subject
                    subject = false;    // reset marker
                }
                if ((xmlFile.NodeType == XmlNodeType.Element) && (xmlFile.Name == "gesmes:subject"))    // xmlFile is on node gesmes:subject
                {
                    subject = true; // set marker so that in next cycle previous if command could read text
                }

                /*
                 * reading text in node gesmes:name
                 * order of if commands - if they were written the other way around the program would try to read the text immediately (while xmlFile is of NodeType Element)
                 */
                if (sender)    // if in previous cycle of loop the xmlFile was on node gesmes:name then...
                {
                    Report.Sender = xmlFile.ReadContentAsString(); // read text from node gesmes:name
                    sender = false;    // reset marker
                }
                if ((xmlFile.NodeType == XmlNodeType.Element) && (xmlFile.Name == "gesmes:name"))    // xmlFile is on node gesmes:name
                {
                    sender = true; // set marker so that in next cycle previous if command could read text
                }

                /*
                 * accessing Cubes - setting markers
                 * order of if commands - if they were written the other way around the program would set all markers while entering first generation
                 */
                if (xmlFile.NodeType == XmlNodeType.Element && (xmlFile.Name == "Cube") && cubeFirstGen && cubeSecondGen && !cubeThirdGen)    // If xmlFIle is on third generation of nodes Cube
                {
                    cubeThirdGen = true;    // xmlFile in Second generation of nodes Cube
                }
                if (xmlFile.NodeType == XmlNodeType.Element && (xmlFile.Name == "Cube") && cubeFirstGen && !cubeSecondGen)    // If xmlFIle is on second generation of nodes Cube
                {
                    cubeSecondGen = true;    // xmlFile in Second generation of nodes Cube
                }
                if (xmlFile.NodeType == XmlNodeType.Element && (xmlFile.Name == "Cube") && !cubeFirstGen)    // If xmlFIle is on first generation of nodes Cube
                {
                    cubeFirstGen = true;    // xmlFile in first generation of nodes Cube
                }

                /*
                 * accessing Cubes - resetting markers
                 */
                if (xmlFile.NodeType == XmlNodeType.EndElement && (xmlFile.Name == "Cube") && cubeFirstGen && !cubeSecondGen && !cubeThirdGen)    // If xmlFIle is leaving first generation of nodes Cube
                {
                    cubeFirstGen = false;
                }
                if (xmlFile.NodeType == XmlNodeType.EndElement && (xmlFile.Name == "Cube") && cubeFirstGen && cubeSecondGen && cubeThirdGen)    // If xmlFIle is leaving second generation of nodes Cube
                {
                    cubeSecondGen = false;
                    cubeThirdGen = false; 
                }

                /*
                 * accesing Cubes - reading value of attribute of second generation of nodes Cubes
                 */
                if(xmlFile.NodeType == XmlNodeType.Element && (xmlFile.Name == "Cube") && cubeFirstGen && cubeSecondGen && !cubeThirdGen)
                {
                    Report.Date = xmlFile.GetAttribute("time");
                }

                /*
                 * accesing Cubes - reading value of attribute of second generation of nodes Cubes
                 */
                if (xmlFile.NodeType == XmlNodeType.Element && (xmlFile.Name == "Cube") && cubeFirstGen && cubeSecondGen && cubeThirdGen)
                {
                    // exception checks if rate value can be converted to double
                    try
                    {
                        Report.AddCube(xmlFile.GetAttribute("currency"), xmlFile.GetAttribute("rate")); // adding a new Cube to the list
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception: " + ex.Message);                 // sending a message about exception
                        Report.AddCube(xmlFile.GetAttribute("currency"), xmlFile.GetAttribute("0.0"));  // sending an impossible rate value in case of exception
                    }
                }
            }

            // Write Report to the console
            Console.WriteLine(Report.GetReport());
        }
    }
}
