using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalSupervisorSystem;



namespace PersonalSupervisorSystem
{
    public static class DataStorage
    {
        private const string FilePath = "data.txt";  // Path to the file where data is stored

        // Method to save all data to a file
        public static void SaveData(Dictionary<string, User> users)
        {
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                // Save User Data
                sw.WriteLine("[Users]");
                foreach (var user in users.Values)
                {
                    sw.WriteLine($"{user.UserID}, {user.Name}, {user.Role}");
                }

                // Save self-reports (only applicable to Students)
                sw.WriteLine("\n[Self reports]");
                foreach (var user in users.Values)
                {
                    if (user is Student student)
                    {
                        foreach (var selfReport in student.SelfReports)
                        {
                            sw.WriteLine($"{student.UserID}, {user.Name} {selfReport.ReportDate}, {selfReport.ReportText}");
                        }
                    }
                }

                // Save meetings (only applicable to Students and their Supervisors)
                sw.WriteLine("\n[Meetings]");
                foreach (var user in users.Values)
                {
                    if (user is Student student)
                    {
                        foreach (var meeting in student.Meetings)
                        {
                            sw.WriteLine($"{student.UserID}, {meeting.Supervisor.UserID}, {student.Name} , {meeting.MeetingDate:yyyy-MM-dd HH:mm}, {meeting.MeetingDetails}");
                        }
                    }
                }

                // Save notes (messages sent between users)
                sw.WriteLine("\n[Notes]");
                foreach (var user in users.Values)
                {
                    foreach (var note in user.SentNotes)
                    {
                        sw.WriteLine($"{note.SenderID} , {note.RecipientID} , {user.Name} , {note.Timestamp},  {note.Content}");
                    }
                }
            }
        }

        // Method to load all data from the file
        public static void LoadData(Dictionary<string, User> users)
        {
            // Check if the data file exists before attempting to read
            if (!File.Exists(FilePath)) return;

            // Read all lines from the file
            string[] lines = File.ReadAllLines(FilePath);
            string currentSection = null;

            // Process each line in the file
            foreach (var line in lines)
            {
                // Check if the line indicates a section (e.g., [Users], [Meetings])
                if (line.StartsWith("["))
                {
                    currentSection = line.Trim('[', ']');
                    continue;
                }

                // Skip empty lines
                if (string.IsNullOrWhiteSpace(line)) continue; // Skip empty lines

                // Handle each section's data parsing
                if (currentSection == "Users")
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3) // Ensure there are exactly 3 parts
                    {
                        // Parse user details
                        string userID = parts[0].Trim();
                        string name = parts[1].Trim();
                        UserRole role = Enum.TryParse(parts[2].Trim(), out UserRole parsedRole) ? parsedRole : throw new ArgumentException("Invalid role");

                        // Create a User object based on their role
                        User user = role switch
                        {
                            UserRole.Student => new Student(userID, name, null),
                            UserRole.PersonalSupervisor => new PersonalSupervisor(userID, name),
                            UserRole.SeniorTutor => new SeniorTutor(userID, name),
                            UserRole.MaintenanceStaff => new MaintenanceStaff(userID, name),
                            _ => throw new ArgumentException("Invalid role")
                        };

                        users[userID] = user; // Add the user to the dictionary
                    }
                }

                else if (currentSection == "Meetings")
                {
                    var parts = line.Split(',');
                    if (parts.Length == 4) // Ensure there are exactly 4 parts
                    {
                        // Parse meeting details
                        string studentID = parts[0].Trim();
                        string supervisorID = parts[1].Trim();
                        DateTime meetingDate = DateTime.TryParse(parts[2].Trim(), out DateTime parsedDate) ? parsedDate : throw new ArgumentException("Invalid date format");
                        string details = parts[3].Trim();

                        // Validate and create the meeting if users exist
                        if (users.ContainsKey(studentID) && users.ContainsKey(supervisorID))
                        {
                            var student = (Student)users[studentID];
                            var supervisor = (PersonalSupervisor)users[supervisorID];
                            var meeting = new Meeting(student, supervisor, details) { MeetingDate = meetingDate };

                            // Add the meeting to both the student and supervisor
                            student.Meetings.Add(meeting);
                            supervisor.Meetings.Add(meeting);
                        }
                    }
                }
                else if (currentSection == "Notes")
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3) // Ensure there are exactly 3 parts
                    {

                        // Parse note details
                        string senderID = parts[0].Trim();
                        string recipientID = parts[1].Trim();
                        string content = parts[2].Trim();

                        // Validate and create the note if users exist
                        if (users.ContainsKey(senderID) && users.ContainsKey(recipientID))
                        {
                            var note = new Note(senderID, recipientID, content);
                            users[senderID].SentNotes.Add(note);
                            users[recipientID].ReceivedNotes.Add(note);
                        }
                    }
                }
            }
        }
    }
}