using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalSupervisorSystem;

using System;
using System.Collections.Generic;

namespace PersonalSupervisorSystem
{
    // Enum representing different roles of users in the system
    public enum UserRole
    {
        Student,
        PersonalSupervisor,
        SeniorTutor,
        MaintenanceStaff
    }

    public class Note // Class representing a Note that can be sent between users
    {

        // Properties of a note: Sender ID, Recipient ID, Content, and Timestamp
        public string SenderID { get; private set; }
        public string RecipientID { get; private set; }
        public string Content { get; private set; }
        public DateTime Timestamp { get; private set; }

        // Constructor to initialize a note with sender, recipient, and content
        public Note(string senderID, string recipientID, string content)
        {
            SenderID = senderID;
            RecipientID = recipientID;
            Content = content;
            Timestamp = DateTime.Now;
        }

        // Override ToString method to display the note information in a readable format
        public override string ToString()
        {
            return $"[{Timestamp}] From: {SenderID} To: {RecipientID} | {Content}";
        }
    }

    // Abstract class representing a User in the system (Student, Personal Supervisor, etc.)
    public abstract class User
    {
        // Properties representing the user’s ID, name, role, and notes (sent and received)
        public string UserID { get; private set; }
        public string Name { get; private set; }
        public UserRole Role { get; private set; }
        public List<Note> SentNotes { get; private set; }  // List to store notes sent by the user
        public List<Note> ReceivedNotes { get; private set; } // List to store notes received by the user

        // Constructor to initialise the user with an ID, name, and role
        protected User(string userID, string name, UserRole role)
        {
            UserID = userID;
            Name = name;
            Role = role;
            SentNotes = new List<Note>();
            ReceivedNotes = new List<Note>();
        }

        // Abstract method to show a menu specific to the user's role
        public abstract void ShowMenu();

        // Abstract method to handle actions based on the user's menu selection
        public abstract bool HandleActions(int choice, Dictionary<string, User> users);

        // Method to send a note to another user
        public void SendNoteTo(Dictionary<string, User> users, string recipientID, string content)
        {
            // Check if the recipient ID exists in the system

            if (users.ContainsKey(recipientID))
            {
                var recipient = users[recipientID];
                var newNote = new Note(UserID, recipientID, content);
                SentNotes.Add(newNote);
                recipient.ReceivedNotes.Add(newNote);
                Console.WriteLine($"Note sent from {Name} to {recipient.Name}.");
            }
            else
            {
                Console.WriteLine("Recipient not found."); // If recipient ID doesn't exist, display an error
            }
        }

        // Method to view all the notes sent by the user
        public void ViewSentNotes()
        {
            Console.WriteLine("\n--- Sent Notes ---");
            if (SentNotes.Count == 0)  // If there are no sent notes, display a message
            {
                Console.WriteLine("No sent notes.");
            }
            else
            {
                // Otherwise, display all the sent notes
                foreach (var note in SentNotes)
                {
                    Console.WriteLine(note); // Print each note's information
                }
            }
        }

        // Method to view all the notes received by the user
        public void ViewReceivedNotes()
        {
            Console.WriteLine("\n--- Received Notes ---");
            if (ReceivedNotes.Count == 0)  // If there are no received notes, display a message
            {
                Console.WriteLine("No received notes.");
            }
            else
            {
                foreach (var note in ReceivedNotes)  // Otherwise, display all the received notes
                {
                    Console.WriteLine(note); // Print each note's information
                }
            }
        }
    }
}
