using System;
using System.Collections.Generic;
using PersonalSupervisorSystem;


namespace PersonalSupervisorSystem
{
    public class Program
    {
        // A static dictionary that stores users, where the key is the User ID and the value is the User object
        private static Dictionary<string, User> Users = new Dictionary<string, User>();

        // Method to handle user login by verifying the user ID
        public static User Login(string userID)
        {
            // Check if the user ID exists in the Users dictionary
            if (Users.ContainsKey(userID))
            {
                // If the user ID exists, return the corresponding User object
                return Users[userID];
            }
            else
            {
                // If the user ID does not exist, display an error message and return null
                Console.WriteLine("Invalid User ID.");
                return null;
            }
        }

        static void HandleFileOperations()
        {
            string filePath = @"C:\path\to\your\data.txt"; // Adjust the file path as needed
            FileHandler fileHandler = new FileHandler(filePath);

            // Example of adding data to the file
            string newData = "Meeting: 2024-11-18 - New Meeting Added";
            fileHandler.AddData(newData);

            // Display the contents of the file
            fileHandler.DisplayFileContents();
        }



        static void Main(string[] args)
        {
            DataStorage.LoadData(Users);

            // Create Users
            var supervisor1 = new PersonalSupervisor("PS001", "Dr. Smith");
            var supervisor2 = new PersonalSupervisor("PS002", "Dr. Johnson");
            var student1 = new Student("S001", "Alice", supervisor1);
            var student2 = new Student("S002", "Bob", supervisor1);
            var seniorTutor = new SeniorTutor("ST001", "Prof. Williams");
            var maintenanceStaff = new MaintenanceStaff("M001", "Jack");

            // Assign students to supervisors
            supervisor1.AddStudent(student1);
            supervisor1.AddStudent(student2);

            // Add supervisors to senior tutor
            seniorTutor.AddSupervisor(supervisor1);
            seniorTutor.AddSupervisor(supervisor2);

            // Add students to senior tutor
            seniorTutor.Students.Add(student1);
            seniorTutor.Students.Add(student2);

            // Add to system
            Users[supervisor1.UserID] = supervisor1;
            Users[supervisor2.UserID] = supervisor2;
            Users[student1.UserID] = student1;
            Users[student2.UserID] = student2;
            Users[seniorTutor.UserID] = seniorTutor;
            Users[maintenanceStaff.UserID] = maintenanceStaff;

            while (true)
            {
                Console.WriteLine("Enter your User ID to login:");
                string userID = Console.ReadLine();
                var user = Login(userID);

                if (user != null)
                {
                    bool userSession = true;
                    while (userSession)
                    {
                        user.ShowMenu();
                        int choice;
                        if (int.TryParse(Console.ReadLine(), out choice))
                        {
                            userSession = user.HandleActions(choice, Users);
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice. Enter a number.");
                        }
                    }
                }

                // Exit program after a session ends
                Console.WriteLine("Would you like to log in again? (y/n)");
                string exitChoice = Console.ReadLine().ToLower();
                if (exitChoice != "y")
                {
                    break; // Exit the program if the user doesn't want to log in again
                }
            }

            // Save data before exiting
            DataStorage.SaveData(Users);
        }
    }
}