namespace LeakDetectorService.Models
{
    public class LeakReport
    {
        public Violation[] Violations { get; set; }

        public LeakReport(Violation[] violations)
        {
            Violations = violations;
        }
    }
}