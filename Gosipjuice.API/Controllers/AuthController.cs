using Gosipjuice.API.Models.DTO;
using Gosipjuice.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gosipjuice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        //POST : {apibaseurl}/api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDto request)
        {
            var identityUser = await userManager.FindByEmailAsync(request.Email);
            if (identityUser is not null)
            {
                //Check Password
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser,request.Password);
                
                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);
                    //Create a toke and response
                    var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());
                    var response = new LoginResponseDto()
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };
                    return Ok(response);
                }
            }
            ModelState.AddModelError("", "Email or Password is incorrect");

            return ValidationProblem(ModelState);

        }

        // POST: {apibaseurl}/api
        [HttpPost]
        [Route("register")]
       public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            // Create IdentityUser object
            var user = new IdentityUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim()
            };

            var identityResult = await userManager.CreateAsync(user, request.Password);

            if(identityResult.Succeeded)
            {
                // Add Role to user(Reader)
                identityResult = await userManager.AddToRoleAsync(user, "Reader");

                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if(identityResult.Errors.Any())
                {
                    foreach(var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }
    }
}
