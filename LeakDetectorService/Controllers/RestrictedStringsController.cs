using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using LeakDetectorService.Utils;
using LeakDetectorService.Models;

namespace LeakDetectorService.Controllers
{
    public class RestrictedStringsController : ApiController
    {
        // GET api/RestrictedStrings
        public List<string> Get()
        {
            List<string> documents = new List<string>();
            Db db = new Db();
            IMongoQueryable<BsonDocument> collection = db.GetCollection("restrictedStrings");
            // iterate code goes here
            foreach (var document in collection)
            {
                documents.Add(document.ToString());
            }
            return documents;
        }

        // PUT api/RestrictedStrings
        public string Put([FromBody] Restricted restrictedString)
        {
            Db db = new Db();
            return db.AddRestrictedString(restrictedString);
        }

        // DELETE api/RestrictedStrings
        public string Delete([FromBody] Restricted restrictedString)
        {
            Db db = new Db();
            DeleteResult deleteResult = db.DeleteRestrictedString(restrictedString);
            return $"Deleted {deleteResult.DeletedCount} from restrictedStrings";

        }
    }
}