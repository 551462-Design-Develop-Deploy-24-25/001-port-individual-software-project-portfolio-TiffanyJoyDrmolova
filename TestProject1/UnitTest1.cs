using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using PersonalSupervisorSystem;


namespace PersonalSupervisorSystem.Tests
{
    // Test class for testing different functionalities of the PersonalSupervisorSystem
    [TestClass]
    public class UnitTest1
    {
        // Declare the necessary fields for the tests
        private Dictionary<string, User> users;
        private PersonalSupervisor supervisor1;
        private Student student1;

        [TestInitialize] // Setup method that is called before each test to initialise test data
        public void Setup()
        {
            // Initialise the dictionary and objects needed for tests
            users = new Dictionary<string, User>();
            supervisor1 = new PersonalSupervisor("PS001", "Dr. Smith");
            var supervisor2 = new PersonalSupervisor("PS002", "Dr. Johnson");
            student1 = new Student("S001", "Alice", supervisor1);
            var student2 = new Student("S002", "Bob", supervisor1);
            var seniorTutor = new SeniorTutor("ST001", "Prof. Williams");
            var maintenanceStaff = new MaintenanceStaff("M001", "Jack");

            // Associate students with their supervisors  // Add supervisors to the senior tutor
            supervisor1.AddStudent(student1);
            supervisor1.AddStudent(student2);
            seniorTutor.AddSupervisor(supervisor1);
            seniorTutor.AddSupervisor(supervisor2);

            // Add users to the dictionary
            users[supervisor1.UserID] = supervisor1;
            users[supervisor2.UserID] = supervisor2;
            users[student1.UserID] = student1;
            users[student2.UserID] = student2;
            users[seniorTutor.UserID] = seniorTutor;
            users[maintenanceStaff.UserID] = maintenanceStaff;

            // Set the static Users dictionary in the Program class (used for login)
            typeof(Program).GetField("Users", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, users);
        }

        // Test case for logging in with a valid user ID
        [TestMethod]
        public void TestLogin_ValidUser()
        {
            // Attempt to login with a valid user ID
            var user = Program.Login("S001");
            // Assert that the returned user is not null
            Assert.IsNotNull(user);
            // Assert that the logged-in user is the expected one (Alice)
            Assert.AreEqual("Alice", user.Name);
        }

        // Test case for logging in with an invalid user ID
        [TestMethod]
        public void TestLogin_InvalidUser()  // Attempt to login with an invalid user ID
        {
            var user = Program.Login("InvalidID");   // Assert that no user is found for an invalid ID
            Assert.IsNull(user);
        }

        [TestMethod]    // Test case for a student submitting a self-report
        public void TestStudentSubmitSelfReport()
        {
            // Student submits a self-report
            student1.SubmitSelfReport("Feeling good about the semester."); // Assert that the student has one self-report
            Assert.AreEqual(1, student1.GetSelfReports().Count);
            Assert.AreEqual("Feeling good about the semester.", student1.GetSelfReports()[0].ReportText); // Assert that the content of the report matches the expected text
        }

        // Test case for a student requesting a meeting with a supervisor
        [TestMethod]
        public void TestStudentRequestMeeting()
        {
            // Student requests a meeting with the supervisor
            student1.RequestMeetingWithSupervisor("Discuss project progress");
            // Assert that the student has one meeting in their list
            Assert.AreEqual(1, student1.Meetings.Count);
            // Assert that the meeting details match the requested content
            Assert.AreEqual("Discuss project progress", student1.Meetings[0].MeetingDetails);
            // Assert that the supervisor also has one meeting scheduled
            Assert.AreEqual(1, supervisor1.Meetings.Count);
        }

        // Test case for a supervisor reviewing student reports
        [TestMethod]
        public void TestSupervisorReviewStudentReports()
        {
            // Student submits two reports
            student1.SubmitSelfReport("Report 1");
            student1.SubmitSelfReport("Report 2");
            // Capture the console output during the review
            using (var consoleOutput = new ConsoleOutput())
            {
                // Supervisor reviews the student reports
                supervisor1.ReviewStudentReports();
                var output = consoleOutput.GetOutput();
                // Assert that both reports are included in the console output
                Assert.IsTrue(output.Contains("Report 1"));
                Assert.IsTrue(output.Contains("Report 2"));
            }
        }

        // Test case for sending a note from one user to another
        [TestMethod]
        public void TestSendNote()
        {
            // Student sends a note to the supervisor
            student1.SendNoteTo(users, "PS001", "Hello Supervisor!");
            // Assert that the student has one sent note
            Assert.AreEqual(1, student1.SentNotes.Count);
            // Assert that the content of the note matches the expected text
            Assert.AreEqual("Hello Supervisor!", student1.SentNotes[0].Content);
            // Assert that the supervisor has received the note
            Assert.AreEqual(1, supervisor1.ReceivedNotes.Count);
        }
    }

    // Helper class to capture console output during tests
    public class ConsoleOutput : IDisposable
    {
        private readonly System.IO.StringWriter stringWriter;
        private readonly System.IO.TextWriter originalOutput;

        // Constructor that sets up the StringWriter to capture console output
        public ConsoleOutput()
        {
            stringWriter = new System.IO.StringWriter();
            originalOutput = Console.Out;
            Console.SetOut(stringWriter); // Redirect Console.Out to the StringWriter
        }

        // Method to get the captured output as a string
        public string GetOutput() => stringWriter.ToString();

        // Dispose method to restore the original console output
        public void Dispose()
        {
            Console.SetOut(originalOutput); // Restore the original output
            stringWriter.Dispose();  // Dispose the StringWriter
        }
    }
}