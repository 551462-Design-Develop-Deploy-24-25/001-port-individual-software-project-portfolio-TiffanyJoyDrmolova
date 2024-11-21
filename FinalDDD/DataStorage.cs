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
        private const string FilePath = "data.txt"; // Path to the file where data is stored

        // Save all data to the file
        public static void SaveData(Dictionary<string, User> users)
        {
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                // Save user data
                sw.WriteLine("[Users]");
                foreach (var user in users.Values)
                {
                    sw.WriteLine($"{user.UserID}, {user.Name}, {user.Role}");
                }

                // Save self-reports (only for Students)
                sw.WriteLine("\n[SelfReports]");
                foreach (var user in users.Values)
                {
                    if (user is Student student)
                    {
                        foreach (var report in student.SelfReports)
                        {
                            sw.WriteLine($"{student.UserID}, {report.ReportDate:yyyy-MM-dd}, {report.ReportText}");
                        }
                    }
                }

                // Save meetings (for both Students and Supervisors)
                sw.WriteLine("\n[Meetings]");
                foreach (var user in users.Values)
                {
                    if (user is Student student)
                    {
                        foreach (var meeting in student.Meetings)
                        {
                            sw.WriteLine($"{student.UserID}, {meeting.Supervisor.UserID}, {meeting.MeetingDate:yyyy-MM-dd HH:mm}, {meeting.MeetingDetails}");
                        }
                    }

                    if (user is PersonalSupervisor supervisor)
                    {
                        foreach (var meeting in supervisor.Meetings)
                        {
                            sw.WriteLine($"{supervisor.UserID}, {meeting.Student.UserID}, {meeting.MeetingDate:yyyy-MM-dd HH:mm}, {meeting.MeetingDetails}");
                        }
                    }
                }

                // Save notes (messages between users)
                sw.WriteLine("\n[Notes]");
                foreach (var user in users.Values)
                {
                    foreach (var note in user.SentNotes)
                    {
                        sw.WriteLine($"{note.SenderID}, {note.RecipientID}, {note.Timestamp:yyyy-MM-dd HH:mm}, {note.Content}");
                    }
                }
            }
        }

        // Load all data from the file
        public static void LoadData(Dictionary<string, User> users)
        {
            if (!File.Exists(FilePath)) return;

            string[] lines = File.ReadAllLines(FilePath);
            string currentSection = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("["))
                {
                    currentSection = line.Trim('[', ']');
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line)) continue;

                if (currentSection == "Users")
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        string userID = parts[0].Trim();
                        string name = parts[1].Trim();
                        UserRole role = Enum.TryParse(parts[2].Trim(), out UserRole parsedRole) ? parsedRole : throw new ArgumentException("Invalid role");

                        User user = role switch
                        {
                            UserRole.Student => new Student(userID, name, null),
                            UserRole.PersonalSupervisor => new PersonalSupervisor(userID, name),
                            UserRole.SeniorTutor => new SeniorTutor(userID, name),
                            UserRole.MaintenanceStaff => new MaintenanceStaff(userID, name),
                            _ => throw new ArgumentException("Invalid role")
                        };

                        users[userID] = user;
                    }
                }
                else if (currentSection == "Meetings")
                {
                    var parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        string userID1 = parts[0].Trim();
                        string userID2 = parts[1].Trim();
                        DateTime meetingDate = DateTime.Parse(parts[2].Trim());
                        string meetingDetails = parts[3].Trim();

                        if (users.TryGetValue(userID1, out var user1) && users.TryGetValue(userID2, out var user2))
                        {
                            if (user1 is Student student && user2 is PersonalSupervisor supervisor)
                            {
                                var meeting = new Meeting(student, supervisor, meetingDetails) { MeetingDate = meetingDate };
                                student.Meetings.Add(meeting);
                                supervisor.Meetings.Add(meeting);
                            }
                            else if (user1 is PersonalSupervisor supervisor1 && user2 is Student student1)
                            {
                                var meeting = new Meeting(student1, supervisor1, meetingDetails) { MeetingDate = meetingDate };
                                student1.Meetings.Add(meeting);
                                supervisor1.Meetings.Add(meeting);
                            }
                        }
                    }
                }
                else if (currentSection == "Notes")
                {
                    var parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        string senderID = parts[0].Trim();
                        string recipientID = parts[1].Trim();
                        DateTime timestamp = DateTime.Parse(parts[2].Trim());
                        string content = parts[3].Trim();

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
