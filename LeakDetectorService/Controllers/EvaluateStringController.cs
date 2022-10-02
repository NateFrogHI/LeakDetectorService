using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using MongoDB.Bson;
using MongoDB.Driver;
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
        // GET api/EvaluateString
        public List<string> Get()
        {
            Db db = new Db();
            return db.GetLeakReports();
        }

        // POST api/EvaluateString
        public string Post([FromBody] RawText value)
        {
            //String[] badwords = { "Arthas", "THatOnEwOrD", "that one sentence" };
            Db db = new Db();
            List<string> badwords = db.GetRestrictedStrings();
            StringEvaluator stringEvaluator = new StringEvaluator(value.Text, badwords.ToArray());
            stringEvaluator.EvaluateString();
            stringEvaluator.SubmitViolations();
            return stringEvaluator.Report();
        }
    }
}
