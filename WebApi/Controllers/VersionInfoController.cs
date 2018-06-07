using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class VersionInfoController : Controller
    {
        ILogger<VersionInfoController> _logger;

        public VersionInfoController(ILogger<VersionInfoController> logger)
        {
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<string> Get()
        {
            _logger.LogWarning("Version info called");
            return new string[] { string.Format("version hash: {0}", Program.GitHash()) };
        }
    }
}
