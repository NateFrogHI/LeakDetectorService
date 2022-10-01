using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;

namespace LeakDetectorService.Controllers
{
    public class EvaluateStringController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public string Post([FromBody] Models.RawText value)
        {
            String[] badwords = { "Arthas", "THatOnEwOrD", "that one sentence" };
            Trace.WriteLine(value.ToString());
            Utils.StringEvaluator stringEvaluator = new Utils.StringEvaluator(value.Text, badwords);
            stringEvaluator.EvaluateString();
            return stringEvaluator.Report();
        }

        // PUT api/values/id
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
