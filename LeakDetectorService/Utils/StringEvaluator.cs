using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeakDetectorService.Utils
{
    public struct Violation
    {
        public String ViolationString { get; }
        public int LineNumber { get; }
        public int CharacterNumber { get; }

        public Violation(String violationString, int lineNumber, int characterNumber)
        {
            this.ViolationString = violationString;
            this.LineNumber = lineNumber;
            this.CharacterNumber = characterNumber;
        }
    }
    public class StringEvaluator
    {
        private String[] sampleLines;
        private String[] restrictedStrings;

        public Violation[] Violations { get; set; }

        public StringEvaluator(String sample, String[] restrictedStrings)
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
            foreach(String resStr in restrictedStrings)
            {
                foreach(String sampleLine in sampleLines)
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

        public String Report()
        {
            String violationText = "Violations found:\n";

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