namespace LeakDetectorService.Models
{
    public class Violation
    {
        public string ViolationString { get; }
        public int LineNumber { get; }
        public int CharacterNumber { get; }

        public Violation(string violationString, int lineNumber, int characterNumber)
        {
            this.ViolationString = violationString;
            this.LineNumber = lineNumber;
            this.CharacterNumber = characterNumber;
        }
    }
}