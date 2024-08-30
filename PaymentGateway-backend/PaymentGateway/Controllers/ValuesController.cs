using Facebook;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain.DataAccessLayer.DTO;
using PaymentGateway.Service.IService;


namespace PaymentGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(IJwtTokenService jwtTokenService, IConfiguration config, IUserService userService, ILogger<ValuesController> logger)
        {
            _jwtTokenService = jwtTokenService;

            _config = config;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] LoginDto login)
        {
            try
            {
                // Validate the Google token
                var payload = await GoogleJsonWebSignature.ValidateAsync(login.token, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _config["GoogleAuthSettings:ClientId"] }
                });



                //Save or update user in the database
                var a = new GoogleAuthDto
                {
                    googleID = payload.Subject,
                    Email = payload.Email,
                    UserName = payload.Name,

                };
                var result = await _userService.AddUser(a);

                if (result == "User already exists")
                {
                    // Generate a token for existing user
                    var token1 = _jwtTokenService.GenerateToken(payload.Subject);
                    return Ok(new { Token = token1, a.Email, a.UserName });
                }

                await _userService.AddUser(a);
                var token = _jwtTokenService.GenerateToken(payload.Subject);

                return Ok(new
                {
                    Token = token,
                    a.Email,
                    a.UserName
                });

            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                return BadRequest();
                    }
        }
        [HttpPost("facebook")]
        public async Task<IActionResult> FacebookLogin([FromBody] FbLoginDto fblogin)
        {
            try
            {
                var fbClient = new FacebookClient();

                // Validate the Facebook token
                dynamic result1 = await fbClient.GetTaskAsync("/me?fields=id,name,email&access_token=" + fblogin.accessToken);

                var fbAuthDto = new FbAuthDto
                {
                    FacebookID = result1.id,
                    Email = result1.email,
                    UserName = result1.name
                };

                var fresult = await _userService.AddUser(fbAuthDto);

                if (fresult == "User already exists")
                {
                    // Generate a token for existing user
                    var tokenf = _jwtTokenService.GenerateToken(result1.id);
                    return Ok(new { Token = tokenf, fbAuthDto.Email, fbAuthDto.UserName });
                }

                var tokenfb = _jwtTokenService.GenerateToken(result1.id);

                return Ok(new { Token = tokenfb, fbAuthDto.Email, fbAuthDto.UserName, StatusCode = 200 });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();

            }
    }
    
    }
}
