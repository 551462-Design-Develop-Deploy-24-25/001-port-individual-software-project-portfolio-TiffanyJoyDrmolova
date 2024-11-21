using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalSupervisorSystem;

namespace PersonalSupervisorSystem
{
    // A class to handle file-related operations, such as adding data and displaying contents
    public class FileHandler
    {
        private string _filePath; // Path of the file to be handled

        // Constructor to initialise the file path
        public FileHandler(string filePath)
        {
            _filePath = filePath;
        }

        // Method to add data to the file
        public void AddData(string data)
        {
            try
            {
                // Open the file for appending (this creates the file if it doesn't exist)
                using (StreamWriter writer = new StreamWriter(_filePath, append: true))
                {
                    writer.WriteLine(data);  // Write the new data
                }
                Console.WriteLine("Data has been successfully added to the file.");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the file write process
                Console.WriteLine("Error writing to file: " + ex.Message);
            }
        }

        // Method to display the file contents (just to verify that data is there)
        public void DisplayFileContents()
        {
            try
            {
                // Check if the file exists before attempting to read it
                if (File.Exists(_filePath))
                {
                    string[] contents = File.ReadAllLines(_filePath);
                    foreach (var line in contents)
                    {
                        Console.WriteLine(line);
                    }
                }
                else
                {
                    Console.WriteLine("File not found."); // Notify if the file does not exist
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the file read process
                Console.WriteLine("Error reading the file: " + ex.Message);
            }
        }
    }

}
