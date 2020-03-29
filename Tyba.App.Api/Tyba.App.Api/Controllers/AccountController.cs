using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.Swagger.Annotations;
using Tyba.App.Api.Models;
using Tyba.App.Business.Interfaces.Services;
using Tyba.App.Business.Models;
using Tyba.App.Business.Models.Dtos;

namespace Tyba.App.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiVersion("1")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AccountController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("users")]
        [MapToApiVersion("1")]
        [SwaggerOperation("Register a new User")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType((int)HttpStatusCode.Conflict, Type = typeof(string))]
        public async Task<IActionResult> RegisterUser(User newUser)
        {
            UserRegistrationResponse result = await _userService.RegisterUser(newUser);
            if (result?.Success ?? false)
                return NoContent();
            return Conflict(result?.ErrorMessage);
        }

        [HttpPost("login")]
        [MapToApiVersion("1")]
        [SwaggerOperation("Login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(LoginResult))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType((int)HttpStatusCode.Conflict, Type = typeof(string))]
        public async Task<IActionResult> Login(User userCredentials)
        {
            User userAuthenticatedResult = await _userService.Authenticate(userCredentials);
            if (userAuthenticatedResult == null)
                return Conflict("Invalid credentials.");
            
            string secret = _configuration["AppSettings:Secret"];
            byte[] key = Encoding.ASCII.GetBytes(secret);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userCredentials.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            return Ok(new LoginResult { Token = tokenString });
        }

        [HttpPost("logout")]
        [MapToApiVersion("1")]
        [SwaggerOperation("Logout")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Authorize]
        public IActionResult Logout([FromHeader(Name = "Authorization")] string authorizationValue)
        {
            if (string.IsNullOrWhiteSpace(authorizationValue))
                return Unauthorized();

            if (HttpContext.User == null || !HttpContext.User.Identity.IsAuthenticated)
                return Unauthorized();

            string bearerToken = authorizationValue.Split(' ')[1];
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(bearerToken) as JwtSecurityToken;

            // TODO: invalidate token

            return NoContent();
        }
    }
}
