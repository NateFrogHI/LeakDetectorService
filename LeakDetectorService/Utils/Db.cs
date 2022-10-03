using System;
using System.Collections.Generic;
using System.Linq;
using LeakDetectorService.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization.Conventions;

namespace LeakDetectorService.Utils
{
    public interface IDb
    {
        List<string> GetRestrictedStrings();
        string AddRestrictedString(Restricted restrictedString);
        string DeleteRestrictedString(Restricted restrictedString);
        List<string> GetLeakReports();
        string SubmitReport(LeakReport leakReport);
    }
    public class Db : IDb
    {
        private  string uri = "mongodb://127.0.0.1:27017/leak-detector-db?directConnection=true&serverSelectionTimeoutMS=2000&appName=mongosh+1.6.0";

        private IMongoDatabase GetDatabase()
        {
            MongoClient client = new MongoClient(uri);
            return client.GetDatabase("leak-detector-db");
        }
            
        private IMongoQueryable<BsonDocument> GetCollection(string collectionName)
        {
            IMongoDatabase db = GetDatabase();
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>(collectionName);

            return collection.AsQueryable();
        }

        public List<string> GetRestrictedStrings()
        {
            List<string> restrictedStrings = new List<string>();
            IMongoQueryable<BsonDocument> collection = GetCollection("restrictedStrings");
            foreach (var document in collection)
            {
                BsonElement stringElement = document.GetElement(1);
                restrictedStrings.Add(stringElement.Value.AsString);
            }
            return restrictedStrings;
        }

        public string AddRestrictedString(Restricted restrictedString)
        {
            // instruct the driver to camelCase the fields in MongoDB
            ConventionPack pack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("elementNameConvention", pack, x => true);

            IMongoDatabase db = GetDatabase();
            IMongoCollection<Restricted> collection = db.GetCollection<Restricted>("restrictedStrings");

            collection.InsertOne(restrictedString);

            return "1 record added to restrictedStrings";
        }

        public string DeleteRestrictedString(Restricted restrictedString)
        {
            // instruct the driver to camelCase the fields in MongoDB
            ConventionPack pack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("elementNameConvention", pack, x => true);

            IMongoDatabase db = GetDatabase();
            IMongoCollection<Restricted> collection = db.GetCollection<Restricted>("restrictedStrings");

            DeleteResult deleteResult = collection.DeleteOne(
                resStrEntry => resStrEntry.RestrictedString == restrictedString.RestrictedString
            );

            return $"Deleted {deleteResult.DeletedCount} from restrictedStrings";
        }

        public List<string> GetLeakReports()
        {
            List<string> leakReports = new List<string>();
            IMongoQueryable<BsonDocument> collection = GetCollection("leakReports");
            foreach (var document in collection)
            {
                BsonElement stringElement = document.GetElement(1);
                BsonArray values = stringElement.Value.AsBsonArray;
                List<string> violations = new List<string>();
                foreach (BsonValue value in values)
                {
                    violations.Add(value.ToString());
                }
                leakReports.Add(String.Join("\n", violations.ToArray()));
            }
            return leakReports;
        }

        public string SubmitReport(LeakReport leakReport)
        {
            // instruct the driver to camelCase the fields in MongoDB
            ConventionPack pack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("elementNameConvention", pack, x => true);

            IMongoDatabase db = GetDatabase();
            IMongoCollection<LeakReport> collection = db.GetCollection<LeakReport>("leakReports");

            collection.InsertOne(leakReport);

            return "1 record added to leakReports";
        }
    }
}