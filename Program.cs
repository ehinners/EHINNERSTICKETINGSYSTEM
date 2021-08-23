using System;
using System.IO;

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
            string temp; // Holds input from user after entering values that describe the ticket. Immediately put into the file.
            bool doneWatchNames; // Sentinel value that controls the inner loop for user to enter as many names of those who are watching the ticket.
            int numTickets = 1; // Holds the current index of the ticketID. If no file is found, starts at 0, otherwise starts back up at the last found ID.
            string welcome = "Welcome To Pewaukee Ticketing"; // Holds a value used for an introduction prompt
            string[] oldTickets = new string[20];

            // Preps and introduces the console for clear user interface 
            Console.Clear();
            Console.WriteLine("=============================");
            Console.WriteLine(welcome);
            Console.WriteLine("=============================");

            
            // Checks to see if a file is found. If yes, the file is quickly scanned along the ticket ID and the very last ticketID is stored.
            // if no file is found, a new file is created through the StreamWriter.
            if(File.Exists(file))
            {
                StreamReader sr = new StreamReader(file);
                while(!sr.EndOfStream)
                {
                    temp = sr.ReadLine();
                    numTickets = int.Parse(temp.Substring(0,1));
                }
                Console.WriteLine("Last Entered Ticket ID: {0}",numTickets);
                sr.Close(); // Allows access to the writer to add more tickets on top of what already exists.
                numTickets++; // Iterates one more time so that the NEXT ticket falls in line
            }    
            StreamWriter sw = File.AppendText(file);  

            // Main loop to allow user to enter ticket info.
            do
            {
                // First User is prompted to create a ticket, y for yes, n for no, anything else and the user is asked again
                Console.WriteLine("Would You Like To Enter A Ticket?(Y/N)");
                input = Console.ReadLine().ToUpper(); // Converts input to uppercase for less debugging

                if(input =="Y")
                {
                    // User is prompted for Each Category of info the ticket needs, then that value is passed to the streamwriter for documentation
                    sw.Write(numTickets+",");
                    Console.WriteLine("Please Enter A Summary:");
                    temp = Console.ReadLine();
                    sw.Write(temp+",");
                    Console.WriteLine("Please Enter Status:");
                    temp = Console.ReadLine();
                    sw.Write(temp+",");
                    Console.WriteLine("Please Enter The Priority:");
                    temp = Console.ReadLine();
                    sw.Write(temp+",");
                    Console.WriteLine("Please Enter The Name Of Who Submitted The Ticket:");
                    temp = Console.ReadLine();
                    sw.Write(temp+",");
                    Console.WriteLine("Please Enter The Name Of Who The Ticket Is Assigned To:");
                    temp = Console.ReadLine();
                    sw.Write(temp+",");

                    // User is prompted for the first watching name
                    Console.WriteLine("Please Enter Who Is Watching The Ticket:");
                    Console.WriteLine("(Only One Name At A Time Please)");
                    temp = Console.ReadLine();
                    sw.Write(temp);

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
                            sw.Write("|"+temp); 
                        }
                        
                    }
                    // Blank line with newline character is appended to start a fresh line for next ticket
                    sw.WriteLine();
                    // numTickets is increased by one to increase the ticketID for the next ticket
                    numTickets++;
                }
                else if(input=="N") // Ends the loop if the user is finished entering tickets
                {
                    break;
                }
                else // Informs the user of their mistake and readies the loop for another attempt to confirm ticket creation
                {
                    Console.WriteLine("Sorry, Please Only Enter Y or N");
                    input="Y";
                }
            }while(input == "Y");
            sw.Close(); // Saves the file        
        }
    }
}
