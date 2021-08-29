using System;
using System.IO;
using System.Collections.Generic;

/**
    * Author: Eric Hinners
    * Date: 8/22/2021
    * Class: DotNetDatabase
    * School: WCTC
    * Overview: This program writes and reads a simple csv file that holds "tickets". 
    A ticket should hold the following values:
     - TicketID
     - Summary
     - Status
     - Priority
     - Submitter Name
     - Assigned Name
     - Watchers Name(s)
    Each ticket should hold only one item per each value, with the exception of watcher names, in which they should be able to enter any number of names they like.
    The user then has the option of adding a ticket, viewing all tickets, or closing the program.
*/

namespace ehinnersTicketingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creates and Initializes Some Vlaues
            string file = "Tickets.csv";  // File Name For Streamreader and Streamwriter
            string input = "Y"; // Initial Value to start main loop, will later hold user input on whether or not they want to create more tickets
            string temp; // Holds input to evaluate before doing anything with them

            bool doneWatchNames; // Sentinel value that controls the inner loop for user to enter as many names of those who are watching the ticket.
            int numTickets = 1; // Holds the current index of the ticketID. If no file is found, starts at 0, otherwise starts back up at the last found ID.
            string welcome = "Welcome To Pewaukee Ticketing"; // Holds a value used for an introduction prompt

            int attribute; // When iterating through each attribute of each ticket, this keeps track of the position

            // Preps and introduces the console for clear user interface 
            Console.Clear();
            Console.WriteLine("=============================");
            Console.WriteLine(welcome);
            Console.WriteLine("=============================");

            // Creates and Initializes a list to store values from the file
            List<string> csvs = new List<string>();

            string[] csvsplit; // Holds a temp amount of values from when each csv is split
            string[] watchsplit; // holds a temp amount of values from when the watchlist is split

            String tempcsv; //Temp placeholder String to hold input values as they're converted to a csv

            
            // Checks to see if a file is found. If yes, the file is quickly scanned along the ticket ID and the very last ticketID is stored.
            // if no file is found, a new file is created through the StreamWriter.
            if(File.Exists(file))
            {
                StreamReader sr = new StreamReader(file);
                while(!sr.EndOfStream)
                {
                    temp = sr.ReadLine();
                    numTickets = int.Parse(temp.Substring(0,1));
                    csvs.Add(temp);
                }
                Console.WriteLine("Last Entered Ticket ID: {0}",numTickets);
                sr.Close(); // Allows access to the writer to add more tickets on top of what already exists.
                numTickets++; // Iterates one more time so that the NEXT ticket falls in line
            }    
            StreamWriter sw = File.AppendText(file);  

            // Main loop to allow user to enter ticket info.
            do
            {
                // First User is prompted with a menu. They can enter 1, 2, or 3. anything else and the user is asked again
                Console.WriteLine("Please Enter:");
                Console.WriteLine("1: To Create A New Ticket");
                Console.WriteLine("2: To View All Current Tickets");
                Console.WriteLine("3: Save Tickets To File And End The Program");

                input = Console.ReadLine(); 

                if(input =="1")
                {
                    // User is prompted for Each Category of info the ticket needs, then that value is appended to tempcsv
                    tempcsv = numTickets.ToString();
                    tempcsv += ",";
                    
                    Console.WriteLine("Please Enter A Summary:");
                    tempcsv += Console.ReadLine();
                    tempcsv += ",";
                    
                    Console.WriteLine("Please Enter Status:");
                    tempcsv += Console.ReadLine();
                    tempcsv += ",";
                    
                    Console.WriteLine("Please Enter The Priority:");
                    tempcsv += Console.ReadLine();
                    tempcsv += ",";
                    
                    Console.WriteLine("Please Enter The Name Of Who Submitted The Ticket:");
                    tempcsv += Console.ReadLine();
                    tempcsv += ",";
                    
                    Console.WriteLine("Please Enter The Name Of Who The Ticket Is Assigned To:");
                    tempcsv += Console.ReadLine();
                    tempcsv += ",";
                    

                    // User is prompted for the first watching name
                    Console.WriteLine("Please Enter Who Is Watching The Ticket:");
                    Console.WriteLine("(Only One Name At A Time Please)");
                    tempcsv += Console.ReadLine();
            

                    // Resets Sentinel value for multiple runs
                    doneWatchNames = true;

                    // Until the user types the escape phrase, they can enter as many names to the watchlist as they desire
                    while(doneWatchNames)
                    {
                        Console.WriteLine("Please Enter Another Name Or '!DONE' To Finish Entering Names:");
                        temp = Console.ReadLine();
                        if(temp.ToUpper()=="!DONE" || temp.ToUpper()=="'!DONE'")
                        {
                            doneWatchNames = false;   
                        }
                        else
                        {
                            tempcsv += "|" + temp;
                        }                        
                    }

                    // Adds csvtemp to list and and file
                    csvs.Add(tempcsv);
                    sw.WriteLine(tempcsv);

                    // numTickets is increased by one to increase the ticketID for the next ticket
                    numTickets++;
                }
                else if(input=="2") // Lists all items in the array if user enters 2 
                {
                    
                    // Title Section
                    Console.Write("====================================================================");
                    Console.WriteLine("================================================================");
                    string padded = "TicketID";
                    Console.Write(padded);
                    padded = "Summary".PadLeft(12);
                    Console.Write(padded);
                    padded = "Status".PadLeft(35);
                    Console.Write(padded);
                    padded = "Priority".PadLeft(25);
                    Console.Write(padded);
                    padded = "Submitter".PadLeft(25);
                    Console.Write(padded);
                    padded = "Assigned".PadLeft(25);
                    Console.WriteLine(padded);
                    Console.Write("====================================================================");
                    Console.WriteLine("================================================================");


                    foreach(string csvLine in csvs) // iterates on each ticket
                    {
                        attribute = 1; // resets the index to keep track of which part of the csv is being held
                        csvsplit = csvLine.Split(",");
                        foreach(string ticketAttribute in csvsplit) // iterates on each part of the csv
                        {                     
                            // depending on which part of the csv is being iterated (controlled by attribute index)     
                            // each line gets different formatting  
                            padded = ticketAttribute.PadLeft(25);
                            if(attribute ==1)
                            {
                                Console.Write(ticketAttribute + " ");
                            }
                            else if(attribute==7)
                            {
                                // watchlist gets a new line with a separate loop to iterate on each watcher
                                Console.WriteLine();
                                Console.Write("Watching: ");
                                watchsplit = ticketAttribute.Split("|");
                                foreach(string watcher in watchsplit)
                                {
                                    padded = watcher.PadLeft(25);
                                    Console.Write(padded);
                                }

                                // formats the end of the output table
                                Console.WriteLine();
                                Console.Write("--------------------------------------------------------------------");
                                Console.WriteLine("----------------------------------------------------------------");
                            }
                            else
                            {            
                                // default formatting for each item that is not the ticketID or the watchlist                    
                                Console.Write(padded + " ");
                            }
                            attribute++;
                        }
                    }
                    Console.WriteLine();
                }
                else if(input=="3") // Ends the loop if the user is finished 
                {
                    break;
                }
                else // Informs the user of their mistake and readies the loop for another iteration
                {
                    Console.WriteLine("Sorry, Please Only Enter 1, 2, or 3");
                    //input="Y";
                }
            }while(input != "3");
            sw.Close(); // Saves the file        
        }
    }
}
