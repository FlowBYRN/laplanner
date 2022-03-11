using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Trainingsplanner.Postgres.Controllers
{
    public class OidcConfigurationController : Controller
    {
        private readonly ILogger<OidcConfigurationController> _logger;
        public IConfiguration Configuration { get; }

        public OidcConfigurationController(IConfiguration configuration, IClientRequestParametersProvider clientRequestParametersProvider,
            ILogger<OidcConfigurationController> logger)
        {
            ClientRequestParametersProvider = clientRequestParametersProvider;
            _logger = logger;
            Configuration = configuration;
        }

        public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

        [HttpGet("_configuration/{clientId}")]
        public IActionResult GetClientRequestParameters([FromRoute] string clientId)
        {
            var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
            return Ok(parameters);
        }

        [HttpGet("api/v1/configuration")]
        public IActionResult ConfigurationData()
        {
            return Ok(new Dictionary<string, string>
            {
                { "ApiBaseUrl", Configuration["ApiBaseUrl"] },
                { "IdentityServerScopes", Configuration["IdentityServerScopes"] }
            });
        }
    }
}