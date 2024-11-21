using PersonalSupervisorSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using PersonalSupervisorSystem;




namespace PersonalSupervisorSystem
{
    // Represents a Student who is supervised by a Personal Supervisor
    public class Student : User
    {
        // The supervisor assigned to the student
        public PersonalSupervisor Supervisor { get; private set; }

        // List of self-reports submitted by the student
        public List<SelfReport> SelfReports { get; set; }

        // List of meetings requested by the student with their supervisor
        public List<Meeting> Meetings { get; private set; }  // List to store meetings

        // Constructor to initialize the student with user ID, name, and supervisor
        public Student(string userID, string name, PersonalSupervisor supervisor)
            : base(userID, name, UserRole.Student)
        {
            Supervisor = supervisor; // Set the student's supervisor
            SelfReports = new List<SelfReport>(); // Initialize the list for self-reports
            Meetings = new List<Meeting>();  // Initialize meetings list
        }

        // Method to submit a self-report
        public void SubmitSelfReport(string reportText)
        {
            SelfReports.Add(new SelfReport(reportText));
            Console.WriteLine($"Student {Name} submitted a self-report.");
        }

        // Method to get the list of self-reports
        public List<SelfReport> GetSelfReports()
        {
            return SelfReports;
        }

        // Method to request a meeting with the supervisor
        public void RequestMeetingWithSupervisor(string meetingDetails)
        {
            var meeting = new Meeting(this, Supervisor, meetingDetails); // Create a new meeting object
            Meetings.Add(meeting);  // Add the meeting to the student's list of meetings
            Supervisor.BookMeeting(this, meetingDetails);  // Book the meeting with the supervisor
            Console.WriteLine($"Student {Name} requested a meeting with {Supervisor.Name}.");
        }


        public void ViewMeetings()
        {
            Console.WriteLine($"\n--- Meetings for Student {Name} ---");
            var supervisorMeetings = Supervisor.Meetings.Where(m => m.Student == this).ToList();

            if (supervisorMeetings.Count == 0)
            {
                Console.WriteLine("No meetings scheduled yet.");
            }
            else
            {
                foreach (var meeting in supervisorMeetings)
                {
                    Console.WriteLine(meeting);
                }
            }
        }


        // Overriding ShowMenu method to display options for a Student
        public override void ShowMenu()
        {
            Console.WriteLine("\n--- Student Menu ---");
            Console.WriteLine("1. Submit Self-Report");
            Console.WriteLine("2. Request Meeting with Supervisor");
            Console.WriteLine("3. View Self-Reports");
            Console.WriteLine("4. View Meetings");
            Console.WriteLine("5. View Sent Notes");
            Console.WriteLine("6. View Received Notes");
            Console.WriteLine("7. Send Note");
            Console.WriteLine("8. Exit");
            Console.Write("Choose an option: ");
        }

        // Overriding HandleActions method to handle the selected options in the menu
        public override bool HandleActions(int choice, Dictionary<string, User> users)
        {
            switch (choice)
            {
                case 1:
                    Console.Write("Enter your self-report text: ");
                    string reportText = Console.ReadLine();
                    SubmitSelfReport(reportText);  // Submit a self-report
                    break;
                case 2:
                    Console.Write("Enter meeting details: ");
                    string meetingDetails = Console.ReadLine();
                    RequestMeetingWithSupervisor(meetingDetails);  // Request a meeting with the supervisor
                    break;
                case 3:
                    Console.WriteLine("\n--- Your Self-Reports ---");
                    foreach (var report in GetSelfReports())
                    {
                        Console.WriteLine(report);  // Display all self-reports
                    }
                    break;
                case 4:
                    ViewMeetings();  // View all meetings
                    break;
                case 5:
                    ViewSentNotes();  // View all sent notes
                    break;
                case 6:
                    ViewReceivedNotes();  // View all received notes
                    break;
                case 7:
                    Console.Write("Enter the recipient ID: ");
                    string recipientID = Console.ReadLine();
                    Console.Write("Enter the note: ");
                    string note = Console.ReadLine();
                    SendNoteTo(users, recipientID, note);  // Send a note to another user
                    break;
                case 8:
                    return false;  // Exit the menu
            }
            return true;  // Continue the session
        }
    }
}