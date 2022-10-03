using System.Collections.Generic;
using System.Web.Http;
using LeakDetectorService.Utils;
using LeakDetectorService.Models;

namespace LeakDetectorService.Controllers
{
    public class RestrictedStringsController : ApiController
    {
        private readonly IDb db;

        public RestrictedStringsController(IDb db)
        {
            this.db = db;
        }
        public RestrictedStringsController()
        {
            this.db = new Db();
        }

        // GET api/RestrictedStrings
        public List<string> Get()
        {
            return db.GetRestrictedStrings();
        }

        // PUT api/RestrictedStrings
        public string Put([FromBody] Restricted restrictedString)
        {
            return db.AddRestrictedString(restrictedString);
        }

        // DELETE api/RestrictedStrings
        public string Delete([FromBody] Restricted restrictedString)
        {
            return db.DeleteRestrictedString(restrictedString);
        }
    }
}