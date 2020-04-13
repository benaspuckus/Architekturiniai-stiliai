using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CarPartsShop.Models;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CarPartsShop.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }


        [HttpPost("api/Account/Register")]
        public async Task<IActionResult> Register([FromBody]RegisterUserRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new IdentityUser { UserName = model.Email, Email = model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded) return BadRequest(result.Errors);
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, false);
                var accessToken = await BuildToken(user);

                return Ok(accessToken);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("api/Account/Login")]
        public async Task<IActionResult> Login([FromBody]LoginUserRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if(!result.Succeeded)
                {
                    return BadRequest("Could not login");
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                var token = await BuildToken(user);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("api/Account/CheckStatus")]
        public IActionResult CheckStatus()
        {
            var current = User;
            var email = current.FindFirst(ClaimTypes.Email).Value;


            return Ok(email);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("api/Account/CheckAdminStatus")]
        public IActionResult CheckAdminStatus()
        {
            return Ok();
        }

        private async Task<string> BuildToken(IdentityUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = new JwtSecurityToken(_configuration["JwtToken:Issuer"],
                _configuration["JwtToken:Issuer"],
                expires: DateTime.Now.AddMinutes(30),
                claims: claims,
                signingCredentials: credentials);
            

            return tokenHandler.WriteToken(token);

           

        }
    }
}
