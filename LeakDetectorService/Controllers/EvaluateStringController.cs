using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LeakDetectorService.Controllers
{
    public struct RestrictedStringEntry
    {
        public string _id;
        public string restrictedString;

    }
    public class EvaluateStringController : ApiController
    {
        // GET api/values
        public List<string> Get()
        {
            List<string> documents = new List<string>();
            var uri = "mongodb://127.0.0.1:27017/leak-detector-db?directConnection=true&serverSelectionTimeoutMS=2000&appName=mongosh+1.6.0";
            var client = new MongoClient(uri);

            // database and collection goes here
            var db = client.GetDatabase("leak-detector-db");
            var coll = db.GetCollection<BsonDocument>("restrictedStrings");
            // finid code goes here
            var cursor = coll.AsQueryable();
            // iterate code goes here
            foreach (var document in cursor)
            {
                documents.Add(document.ToString());
            }
            return documents;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public string Post([FromBody] Models.RawText value)
        {
            //String[] badwords = { "Arthas", "THatOnEwOrD", "that one sentence" };
            Utils.Db db = new Utils.Db();
            List<string> badwords = db.GetRestrictedStrings();
            Debug.WriteLine(value.ToString());
            Utils.StringEvaluator stringEvaluator = new Utils.StringEvaluator(value.Text, badwords.ToArray());
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
