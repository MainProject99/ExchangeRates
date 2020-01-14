using AutoMapper;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model.Models;
using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Logging;

namespace RESTfulAPIs.Controllers
{
    [ApiController]
    [System.Web.Http.Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        /// <summary>
        /// This method allows register user
        /// </summary>
        /// <param name="registerModel">Required</param>
        /// <returns></returns>
        /// <response code="200">Register valid</response> 
        /// <response code="400">If register process failed</response>
        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.HttpPost("Register")]
        public IActionResult Register(RegisterDto registerModel)
        {
            // map authmodel to entity
            var user = _mapper.Map<User>(registerModel);

            try
            {
                // create user
                _userService.CreateUser(user, registerModel.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// This method allows to log in to the API and generate an authentication token.
        /// </summary>
        /// <param name="authModel">Required</param>
        /// <returns>UserInfo authmodel</returns>
        /// <response code="200">Return UserInfo authmodel</response>
        /// <response code="400">If login process failed</response>
        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.HttpPost("Authenticate")]//("authenticate")]
        public IActionResult Authenticate(AuthenticateDto authModel)
        {
            var user = _userService.Authenticate(authModel.Email, authModel.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            IdentityModelEventSource.ShowPII = true;
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                Username = user.Email,
                FirstName = user.Name,
                Token = tokenString
            });
        }
        /// <summary>
        /// This method is for change password
        /// </summary>
        /// <param name="updateModel">Required</param>
        /// <returns></returns>
        /// <response code="200">Data change succesful</response> 
        /// <response code="400">If data change process failed</response>
        [Microsoft.AspNetCore.Mvc.HttpPut("{id}")]
        public IActionResult Update(int id, UpdateDto updateModel)
        {
            // map updateModel to entity and set id
            var user = _mapper.Map<User>(updateModel);
            user.Id = id;

            try
            {
                // update user 
                _userService.UpdateUser(user, updateModel.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
