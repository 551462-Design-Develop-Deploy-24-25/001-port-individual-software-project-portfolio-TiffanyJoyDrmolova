using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalSupervisorSystem;

using System;

namespace PersonalSupervisorSystem
{
    // Represents a meeting between a student and a personal supervisor
    public class Meeting
    {
        // Properties to store the student, supervisor, meeting date, and details
        public Student Student { get; private set; }  // The student participating in the meeting
        public PersonalSupervisor Supervisor { get; private set; } // The supervisor participating in the meeting
        public DateTime MeetingDate { get; set; } // Date and time of the meeting
        public string MeetingDetails { get; private set; } // Details or agenda of the meeting

        // Constructor to initialise the meeting object with the student, supervisor, and meeting details
        public Meeting(Student student, PersonalSupervisor supervisor, string meetingDetails)
        {
            Student = student;  // Set the student property
            Supervisor = supervisor; // Set the supervisor property
            MeetingDate = DateTime.Now; // Default to the current date and time
            MeetingDetails = meetingDetails;  // Set the meeting details
        }

        // Override of the ToString method to provide a formatted string representation of the meeting
        public override string ToString()
        {
            // Format: [DateTime] Meeting with {Student Name} and {Supervisor Name}: {Meeting Details}
            return $"[{MeetingDate}] Meeting with {Student.Name} (Student) and {Supervisor.Name} (Supervisor): {MeetingDetails}";
        }
    }
}