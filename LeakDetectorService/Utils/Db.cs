using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace LeakDetectorService.Utils
{
    public class Db
    {
        private string uri = "mongodb://127.0.0.1:27017/leak-detector-db?directConnection=true&serverSelectionTimeoutMS=2000&appName=mongosh+1.6.0";

        public IMongoQueryable<BsonDocument> GetCollection()
        {
            MongoClient client = new MongoClient(uri);
            IMongoDatabase db = client.GetDatabase("leak-detector-db");
            IMongoCollection<BsonDocument> coll = db.GetCollection<BsonDocument>("restrictedStrings");
            
            return coll.AsQueryable();
        }

        public List<string> GetRestrictedStrings()
        {
            List<string> restrictedStrings = new List<string>();
            IMongoQueryable<BsonDocument> collection = GetCollection();
            foreach (var document in collection)
            {
                BsonElement stringElement = document.GetElement(1);
                restrictedStrings.Add(stringElement.Value.AsString);
            }
            return restrictedStrings;
        }
    }
}