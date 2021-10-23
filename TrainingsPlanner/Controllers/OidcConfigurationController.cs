using System.Collections.Generic;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TrainingsPlanner.Controllers
{    [Route("api/v1/configuration")]

    public class OidcConfigurationController : Controller
    {
        private readonly ILogger<OidcConfigurationController> _logger;
        public IConfiguration Configuration { get; }

        public OidcConfigurationController(IConfiguration configuration,
            ILogger<OidcConfigurationController> logger)
        {
            _logger = logger;
            Configuration = configuration;
        }
        
        // [HttpGet("_configuration/{clientId}")]
        // [ProducesResponseType(typeof(Dictionary<string,string>), StatusCodes.Status200OK)]
        // public IActionResult GetClientRequestParameters([FromRoute] string clientId)
        // {
        //     var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
        //     
        //     
        //     return Ok(parameters);
        // }
        [HttpGet]
        public IActionResult ConfigurationData()
        {
            return Ok(new Dictionary<string, string>
            {
                { "TrainingsPlannerApiBaseUrl", Configuration["TrainingsPlannerApiBaseUrl"] },
                { "TrainingsIdentityApiBaseUrl", Configuration["TrainingsIdentityApiBaseUrl"] },
                { "IdentityServerScopes", Configuration["IdentityServerScopes"] }
            });
        }
    }
}