using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LeakDetectorService.Models;

namespace LeakDetectorService.Utils
{
    public class StringEvaluator
    {
        private string[] sampleLines;
        private string[] restrictedStrings;

        public Violation[] Violations { get; set; }

        public StringEvaluator(string sample, string[] restrictedStrings)
        {
            string[] newlineCharacters = new string[] { "\r\n", "\r", "\n" };
            sampleLines = sample.Split(
                newlineCharacters,
                StringSplitOptions.None
            );
            this.restrictedStrings = restrictedStrings;
        }

        public void EvaluateString()
        {
            List<Violation> violationList = new List<Violation>();
            int lineIndex = 1;
            foreach(string resStr in restrictedStrings)
            {
                foreach(string sampleLine in sampleLines)
                {
                    int startIndex = 0;
                    bool lineSearched = false;
                    while(!lineSearched)
                    {
                        int violationIndex = sampleLine.IndexOf(resStr, startIndex, StringComparison.OrdinalIgnoreCase);
                        if(violationIndex == -1)
                        {
                            lineSearched = true;
                            startIndex = 0;
                        }
                        else
                        {
                            violationList.Add(new Violation(
                                sampleLine.Substring(violationIndex, resStr.Length),
                                lineIndex,
                                violationIndex + 1
                            ));
                            startIndex = violationIndex + resStr.Length;
                        }
                    }
                }
            }
            if (violationList.Count > 0) this.Violations = violationList.ToArray();
        }

        public Boolean HasViolations()
        { 
            return (Violations != null && Violations.Length > 0);
        }

        public void SubmitViolations()
        {
            LeakReport leakReport = new LeakReport(Violations);
            Db db = new Db();
            db.SubmitReport(leakReport);
        }

        public string Report()
        {
            string violationText = "Violations found:\n";

            if (!HasViolations()) return "No violations found";

            foreach(Violation violation in Violations)
            {
                violationText += $"violation: {violation.ViolationString}\n" +
                    $"Line Number: {violation.LineNumber}\nCharacter Number: {violation.CharacterNumber}\n";
            }
            return violationText;
        }
    }
}