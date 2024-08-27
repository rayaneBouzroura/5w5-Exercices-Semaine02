using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.DTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser>signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpPost]
        public async Task<ActionResult> Register(RegisterDTO registerDTO)
        {
            if (registerDTO.Password != registerDTO.PasswordConfirm)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = "Le mot de passe et la confirmation ne sont pas identique" });
            }

            IdentityUser user = new IdentityUser()
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email
            };


            IdentityResult identityResult = await _userManager.CreateAsync(user, registerDTO.Password);


            if (!identityResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = identityResult.Errors });
            }

            return Ok();

        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(loginDTO.Username, loginDTO.Password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                Claim? nameIdentifierClaim = User.Claims?.FirstOrDefault(x =>
                                                                              x.Type == ClaimTypes.NameIdentifier);

                //no roles donc only add name claim
                List<Claim> authClaims = new List<Claim>();
                authClaims.Add(nameIdentifierClaim);

                //TODO : reread se bout de code after dormiring for a bit
                SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("C'est tellement la meilleure cle qui a jamais ete cree dans l'histoire de l'humanite (doit etre longue)"));

                string issuer = this.Request.Scheme + "://" + this.Request.Host;

                DateTime expirationTime = DateTime.Now.AddMinutes(30);

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: null,
                    claims: authClaims,
                    expires: expirationTime,
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
                );

                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                // On ne veut JAMAIS retouner une string directement lorsque l'on utilise Angular.
                // Angular assume que l'on retourne un objet et donne une erreur lorsque le résultat obtenu est une simple string!
                return Ok(new LoginSuccessDTO() { Token = tokenString });


            }
            return NotFound( new { Error = "L'utilisateur existe pas 😭😭 ou bien mauvais mot de pass 🤣🤣" });


        }









        [HttpGet]
        public ActionResult PublicTest()
        {
            return Ok(new string[] { "pomme", "poire", "banane" });
        }
        [Authorize]
        [HttpGet]
        public ActionResult PrivateTest()
        {
            return Ok(new string[] { "Privatepomme", "Privatepoire", "Privatebanane" });
        }
    }
}
