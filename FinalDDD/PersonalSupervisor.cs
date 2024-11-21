using PersonalSupervisorSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalSupervisorSystem;




namespace PersonalSupervisorSystem
{
    // Represents a personal supervisor, extending the User class
    public class PersonalSupervisor : User
    {
        public List<Student> Students { get; set; } // List of students assigned to this supervisor
        public List<Meeting> Meetings { get; set; } // List of meetings organised by the supervisor

        // Constructor to initialise a personal supervisor with a user ID and name
        public PersonalSupervisor(string userID, string name)
            : base(userID, name, UserRole.PersonalSupervisor)
        {
            Students = new List<Student>();
            Meetings = new List<Meeting>();  // Initialise the meetings list
        }

        public void AddStudent(Student student)
        {
            Students.Add(student);
        }

        // Method to book a meeting with a student, including meeting details
        public void BookMeeting(Student student, string meetingDetails)
        {
            var meeting = new Meeting(student, this, meetingDetails);  // Pass meetingDetails
            Meetings.Add(meeting);  // Add the meeting to the list
            Console.WriteLine($"Meeting booked with {student.Name} by Supervisor {Name}. Details: {meetingDetails}");
        }

        // View all meetings that have been booked by this supervisor
        public void ViewMeetings()
        {
            Console.WriteLine($"\n--- Meetings for Supervisor {Name} ---");
            if (Meetings.Count == 0)  // Check if no meetings have been booked
            {
                Console.WriteLine("No meetings booked yet.");   // Inform if the list is empty 
            }
            else
            {
                foreach (var meeting in Meetings)
                {
                    Console.WriteLine(meeting);  // Display the details of each meeting
                }
            }
        }

        // Method to review self-reports submitted by the supervisor's students
        public void ReviewStudentReports()
        {
            Console.WriteLine($"Supervisor {Name} reviewing student reports:");
            foreach (var student in Students)  // Iterate over each student assigned to the supervisor
            {
                Console.WriteLine($"- {student.Name} submitted {student.GetSelfReports().Count} report(s).");
                foreach (var report in student.GetSelfReports())
                {
                    Console.WriteLine(report);  // Use the SelfReport's ToString() method
                }
            }
        }

        // Displays the menu options available to the personal supervisor
        public override void ShowMenu()
        {
            Console.WriteLine("\n--- Personal Supervisor Menu ---");
            Console.WriteLine("1. Review Student Reports");
            Console.WriteLine("2. View Meetings");
            Console.WriteLine("3. View Sent Notes");
            Console.WriteLine("4. View Received Notes");
            Console.WriteLine("5. Send Note");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an option: ");
        }

        // Handles actions based on the supervisor's menu choice
        public override bool HandleActions(int choice, Dictionary<string, User> users)
        {
            switch (choice)
            {
                case 1:
                    ReviewStudentReports();  // Review student reports and show contents
                    break;
                case 2:
                    ViewMeetings();  // View all meetings booked with the supervisor
                    break;
                case 3:
                    ViewSentNotes(); // Display notes sent by the supervisor
                    break;
                case 4:
                    ViewReceivedNotes(); // Display notes received by the supervisor
                    break;
                case 5:
                    // Prompt the supervisor to send a note to another user
                    Console.Write("Enter the recipient ID: ");
                    string recipientID = Console.ReadLine();
                    Console.Write("Enter the note: ");
                    string note = Console.ReadLine();
                    SendNoteTo(users, recipientID, note); // Send the note to the specified user
                    break;
                case 6:
                    return false; // Exit the menu
            }
            return true;
        }
    }
}