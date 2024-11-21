using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalSupervisorSystem;

namespace PersonalSupervisorSystem
{
    // Represents a self-report submitted by a student
    public class SelfReport
    {
        // The date when the self-report was created
        public DateTime ReportDate { get; private set; }
        // The text content of the self-report
        public string ReportText { get; private set; }

        // Constructor to create a new self-report with the provided text
        public SelfReport(string reportText)
        {
            // Set the report date to the current date and time when the report is created
            ReportDate = DateTime.Now;

            // Set the report's text based on the parameter passed
            ReportText = reportText;
        }

        public override string ToString() // Override ToString method to display the report's details in a readable format
        { 
            return $"[{ReportDate}] {ReportText}"; // Format the output to show the date and text of the report
        }

    }
}