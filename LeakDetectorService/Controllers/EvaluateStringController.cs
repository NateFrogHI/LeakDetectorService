using System.Collections.Generic;
using System.Web.Http;
using LeakDetectorService.Utils;
using LeakDetectorService.Models;

namespace LeakDetectorService.Controllers
{
    public struct RestrictedStringEntry
    {
        public string _id;
        public string restrictedString;

    }
    public class EvaluateStringController : ApiController
    {
        private readonly IDb db;

        public EvaluateStringController(IDb db)
        {
            this.db = db;
        }
        public EvaluateStringController()
        {
            this.db = new Db();
        }

        // GET api/EvaluateString
        public List<string> Get()
        {
            return db.GetLeakReports();
        }

        // POST api/EvaluateString
        public string Post([FromBody] RawText value)
        {
            List<string> badwords = db.GetRestrictedStrings();
            StringEvaluator stringEvaluator = new StringEvaluator(value.Text, badwords.ToArray());
            stringEvaluator.EvaluateString();
            LeakReport leakReport = new LeakReport(stringEvaluator.Violations);
            db.SubmitReport(leakReport);
            return stringEvaluator.Report();
        }
    }
}
