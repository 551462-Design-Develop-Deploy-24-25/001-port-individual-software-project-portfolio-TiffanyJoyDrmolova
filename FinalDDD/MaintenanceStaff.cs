using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalSupervisorSystem;


namespace PersonalSupervisorSystem
{

    // Represents a Maintenance Staff user within the system, inheriting from the User class
    public class MaintenanceStaff : User
    {
        // Constructor to initialise a MaintenanceStaff object with a user ID and name
        public MaintenanceStaff(string userID, string name)
            : base(userID, name, UserRole.MaintenanceStaff)
        {
        }

        // Represents a Maintenance Staff user within the system, inheriting from the User class
        public void ReportIssues()
        {
            Console.WriteLine($"Maintenance Staff {Name} is reporting facility issues.");
            // Extend this method as needed for specific functionality.
        }

        // Method to add a new user to the system
        public void AddUser(Dictionary<string, User> users)
        {
            Console.Write("Enter new user ID: ");
            string newUserID = Console.ReadLine();
            Console.Write("Enter new user name: ");
            string newUserName = Console.ReadLine();

            // Display role options for the new user
            Console.WriteLine("Choose the role for the new user:");
            Console.WriteLine("1. Student");
            Console.WriteLine("2. Personal Supervisor");
            Console.WriteLine("3. Senior Tutor");
            Console.WriteLine("4. Maintenance Staff");

            int roleChoice = int.Parse(Console.ReadLine());
            User newUser = null;

            // Create a new user object based on the chosen role
            switch (roleChoice)
            {
                case 1:
                    newUser = new Student(newUserID, newUserName, null); // No supervisor initially
                    break;
                case 2:
                    newUser = new PersonalSupervisor(newUserID, newUserName);
                    break;
                case 3:
                    newUser = new SeniorTutor(newUserID, newUserName);
                    break;
                case 4:
                    newUser = new MaintenanceStaff(newUserID, newUserName);
                    break;
                default:
                    Console.WriteLine("Invalid choice. User not created.");
                    return; // Exit the method if the choice is invalid
            }

            if (newUser != null)
            {
                users[newUserID] = newUser;
                Console.WriteLine($"User {newUserName} with ID {newUserID} added to the system.");
            }
        }

        // Method to remove an existing user from the system
        public void RemoveUser(Dictionary<string, User> users)
        {
            Console.Write("Enter the user ID to remove: ");
            string userIDToRemove = Console.ReadLine();

            if (users.ContainsKey(userIDToRemove))
            {
                users.Remove(userIDToRemove);
                Console.WriteLine($"User with ID {userIDToRemove} has been removed from the system.");
            }
            else
            {
                Console.WriteLine("User ID not found."); // Notify if the user ID does not exist
            }
        }

        // Method to view all users currently in the system
        public void ViewAllUsers(Dictionary<string, User> users)
        {
            Console.WriteLine("\n--- All Users in the System ---");
            if (users.Count == 0)
            {
                Console.WriteLine("No users found.");  // Notify if the system has no users
            }
            else
            {
                // Display each user's details
                foreach (var user in users.Values)
                {
                    Console.WriteLine($"User ID: {user.UserID}, Name: {user.Name}, Role: {user.Role}");
                }
            }
        }

        // Method to display the maintenance staff menu
        public override void ShowMenu()
        {
            Console.WriteLine("\n--- Maintenance Staff Menu ---");
            Console.WriteLine("1. Report Issues");
            Console.WriteLine("2. View Sent Notes");
            Console.WriteLine("3. View Received Notes");
            Console.WriteLine("4. Send Note");
            Console.WriteLine("5. Add User");
            Console.WriteLine("6. Remove User");
            Console.WriteLine("7. View All Users");
            Console.WriteLine("8. Exit");
            Console.Write("Choose an option: "); // Prompt the user to choose a menu option
        }

        // Method to handle actions based on the user's menu choice
        public override bool HandleActions(int choice, Dictionary<string, User> users)
        {
            switch (choice)
            {
                case 1:
                    ReportIssues(); // Call the method to report issues
                    break;
                case 2:
                    ViewSentNotes(); // View notes sent by the user
                    break;
                case 3:
                    ViewReceivedNotes(); // View notes received by the user
                    break;
                case 4:
                    // Prompt for note details and send the note
                    Console.Write("Enter the recipient ID: ");
                    string recipientID = Console.ReadLine();
                    Console.Write("Enter the note: ");
                    string note = Console.ReadLine();
                    SendNoteTo(users, recipientID, note); // Send the note to the recipient
                    break;
                case 5:
                    AddUser(users); // Call the method to add a new user
                    break;
                case 6:
                    RemoveUser(users); // Call the method to remove an existing user
                    break;
                case 7:
                    ViewAllUsers(users); // Call the method to view all users
                    break;
                case 8:
                    return false;  // Exit the menu
            }
            return true; // Continue showing the menu
        }
    }
}