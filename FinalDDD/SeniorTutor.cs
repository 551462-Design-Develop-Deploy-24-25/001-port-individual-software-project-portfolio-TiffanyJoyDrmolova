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
    // Represents a Senior Tutor who oversees personal supervisors and students
    public class SeniorTutor : User
    {
        // List of personal supervisors managed by the Senior Tutor
        public List<PersonalSupervisor> Supervisors { get; set; }

        // List of students supervised by the Senior Tutor
        public List<Student> Students { get; set; }

        // List to store meetings between students and supervisors
        public List<Meeting> Meetings { get; set; }

        // Constructor to initialise the SeniorTutor with user ID, name, and role
        public SeniorTutor(string userID, string name)
            : base(userID, name, UserRole.SeniorTutor)
        {
            // Initialise the lists for supervisors, students, and meetings
            Supervisors = new List<PersonalSupervisor>();
            Students = new List<Student>();
            Meetings = new List<Meeting>();  // Initialise the meetings list
        }



        public void ViewMeetings()
        {
            Console.WriteLine($"\n--- Meetings Overview for Senior Tutor {Name} ---");

            bool meetingsFound = false;

            // Display meetings requested by students
            Console.WriteLine("\nMeetings Requested by Students:");
            foreach (var student in Students)
            {
                if (student.Meetings.Count == 0)
                {
                    Console.WriteLine($"- {student.Name} (ID: {student.UserID}): No meetings requested.");
                }
                else
                {
                    Console.WriteLine($"- {student.Name} (ID: {student.UserID}):");
                    foreach (var meeting in student.Meetings)
                    {
                        Console.WriteLine($"  {meeting}");
                        meetingsFound = true;
                    }
                }
            }

            // Display meetings booked by supervisors
            Console.WriteLine("\nMeetings Organized by Supervisors:");
            foreach (var supervisor in Supervisors)
            {
                if (supervisor.Meetings.Count == 0)
                {
                    Console.WriteLine($"- {supervisor.Name} (ID: {supervisor.UserID}): No meetings booked.");
                }
                else
                {
                    Console.WriteLine($"- {supervisor.Name} (ID: {supervisor.UserID}):");
                    foreach (var meeting in supervisor.Meetings)
                    {
                        Console.WriteLine($"  {meeting}");
                        meetingsFound = true;
                    }
                }
            }

            // If no meetings found at all, display a message
            if (!meetingsFound)
            {
                Console.WriteLine("No meetings scheduled across all students and supervisors.");
            }
        }


        // Method to review reports submitted by the students under this Senior Tutor
        public void ReviewStudentReports()
        {
            Console.WriteLine($"Senior Tutor {Name} reviewing student reports:");

            // Iterate over each student and display the reports they submitted
            foreach (var student in Students)
            {
                Console.WriteLine($"- {student.Name} submitted {student.GetSelfReports().Count} report(s).");
                foreach (var report in student.GetSelfReports())
                {
                    Console.WriteLine($"  Report: {report}");  // Display the content of the report
                }
            }

        }

        // Method to show the menu options available for the Senior Tutor
        public override void ShowMenu()
        {
            Console.WriteLine("\n--- Senior Tutor Menu ---");
            Console.WriteLine("1. View All Meetings");
            Console.WriteLine("2. View All Student Reports");
            Console.WriteLine("3. View Supervisors");
            Console.WriteLine("4. View Sent Notes");
            Console.WriteLine("5. View Received Notes");
            Console.WriteLine("6. Send Note");
            Console.WriteLine("7. Exit");
            Console.Write("Choose an option: ");
        }

        // Method to handle actions based on the menu option chosen by the Senior Tutor
        public override bool HandleActions(int choice, Dictionary<string, User> users)
        {
            switch (choice)
            {
                case 1:
                    ViewMeetings();  // View all meetings of all supervisors
                    break;
                case 2:
                    ReviewStudentReports();  // View all student reports
                    break;
                case 3:
                    Console.WriteLine("Supervisors under Senior Tutor:");
                    foreach (var supervisor in Supervisors)
                    {
                        Console.WriteLine(supervisor.Name); // Display all supervisors managed by the Senior Tutor
                    }
                    break;
                case 4:
                    ViewSentNotes(); // View the notes sent by the Senior Tutor
                    break;
                case 5:
                    ViewReceivedNotes(); // View the notes received by the Senior Tutor
                    break;
                case 6:
                    Console.Write("Enter the recipient ID: ");  // Allow the Senior Tutor to send a note to a recipient
                    string recipientID = Console.ReadLine();
                    Console.Write("Enter the note: ");
                    string note = Console.ReadLine();
                    SendNoteTo(users, recipientID, note); // Send the note to the specified recipient
                    break;
                case 7:
                    return false; // Exit the menu and return false
            }
            return true;
        }

        // Method to add a personal supervisor to the Senior Tutor's list of supervisors
        public void AddSupervisor(PersonalSupervisor supervisor)
        {
            Supervisors.Add(supervisor);  // Adds a supervisor to the list of supervisors
        }
    }
}