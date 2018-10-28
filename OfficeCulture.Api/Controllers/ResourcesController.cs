using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OfficeCulture.Api.Controllers
{
    public class ResourcesController : ApiController
    {
        public string Get()
        {
            return "resources";
        }
    }
}
