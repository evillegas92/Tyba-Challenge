using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using Tyba.App.Business.Models;

namespace Tyba.App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiVersion("1")]
    [Authorize]
    public class RestaurantsController : ControllerBase
    {
        [HttpGet("{cityName}")]
        [MapToApiVersion("1")]
        [SwaggerOperation("Restaurants Nearby")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Restaurant>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Conflict, Type = typeof(string))]
        public IActionResult RestaurantsNearby(string cityName)
        {
            return Ok(Enumerable.Empty<Restaurant>());
        }
    }
}