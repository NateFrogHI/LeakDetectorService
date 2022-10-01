using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeakDetectorService.Models
{
    public class RawText
    {
        public String Text { get; set; }
        
        public override String ToString()
        {
            return Text;
        }
    }
}