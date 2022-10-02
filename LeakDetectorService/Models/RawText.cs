namespace LeakDetectorService.Models
{
    public class RawText
    {
        public string Text { get; set; }
        
        public override string ToString()
        {
            return Text;
        }
    }
}