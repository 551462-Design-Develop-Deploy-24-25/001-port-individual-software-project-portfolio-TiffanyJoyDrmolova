using PersonalSupervisorSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalSupervisorSystem;


namespace PersonalSupervisorSystem
{
    public class PersonalSupervisor : User
    {
        public List<Student> Students { get; set; }
        public List<Meeting> Meetings { get; set; }

        public PersonalSupervisor(string userID, string name)
            : base(userID, name, UserRole.PersonalSupervisor)
        {
            Students = new List<Student>();
            Meetings = new List<Meeting>();
        }


        public void AddStudent(Student student)
        {
            Students.Add(student);
        }


        public void BookMeeting(Student student, string meetingDetails)
        {
            var meeting = new Meeting(student, this, meetingDetails);
            Meetings.Add(meeting);
            Console.WriteLine($"Meeting booked with {student.Name} by Supervisor {Name}. Details: {meetingDetails}");
        }

        public void ViewMeetings()
        {
            Console.WriteLine($"\n--- Meetings for Supervisor {Name} ---");
            if (Meetings.Count == 0)
            {
                Console.WriteLine("No meetings booked yet.");
            }
            else
            {
                foreach (var meeting in Meetings)
                {
                    Console.WriteLine(meeting);
                }
            }
        }

        public void ReviewStudentReports()
        {
            Console.WriteLine($"Supervisor {Name} reviewing student reports:");
            foreach (var student in Students)
            {
                Console.WriteLine($"- {student.Name} submitted {student.GetSelfReports().Count} report(s).");
                foreach (var report in student.GetSelfReports())
                {
                    Console.WriteLine(report);
                }
            }
        }
        public override void ShowMenu()
        {
            Console.WriteLine("\n--- Personal Supervisor Menu ---");
            Console.WriteLine("1. Review Student Reports");
            Console.WriteLine("2. View Meetings");
            Console.WriteLine("3. Book Meeting"); // Book Meeting as option 3
            Console.WriteLine("4. View Sent Notes");
            Console.WriteLine("5. View Received Notes");
            Console.WriteLine("6. Send Note");
            Console.WriteLine("7. Exit");
            Console.Write("Choose an option: ");
        }

        public override bool HandleActions(int choice, Dictionary<string, User> users)
        {
            switch (choice)
            {
                case 1:
                    ReviewStudentReports();
                    break;
                case 2:
                    ViewMeetings();
                    break;
                case 3: // Book Meeting is now case 3
                    Console.Write("Enter the student ID for the meeting: ");
                    string meetingStudentID = Console.ReadLine();
                    var studentForMeeting = Students.FirstOrDefault(s => s.UserID == meetingStudentID);
                    if (studentForMeeting != null)
                    {
                        Console.Write("Enter meeting details: ");
                        string meetingDetails = Console.ReadLine();
                        BookMeeting(studentForMeeting, meetingDetails);
                    }
                    else
                    {
                        Console.WriteLine("Student not found. Please ensure the student is already added.");
                    }
                    break;
                case 4:
                    ViewSentNotes();
                    break;
                case 5:
                    ViewReceivedNotes();
                    break;
                case 6:
                    Console.Write("Enter the recipient ID: ");
                    string recipientID = Console.ReadLine();
                    Console.Write("Enter the note: ");
                    string note = Console.ReadLine();
                    SendNoteTo(users, recipientID, note);
                    break;
                case 7:
                    return false;
            }
            return true;
        }
    }
}
