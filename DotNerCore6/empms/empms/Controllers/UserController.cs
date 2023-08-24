using empms.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json;
//using System.Web.Http;

namespace empms.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly PostgresContext _context;  //could be context
		private readonly IConfiguration _config;
		public UserController(PostgresContext context, IConfiguration configuration)
		{
			_context = context;   // just use this.context
			_config = configuration;
		}


		[HttpGet("")]
		public async Task<ActionResult<IEnumerable<userInfo>>> GetUser()
		{
			return await _context.UserInfos.ToListAsync();
		}

		//[Authorize(Roles ="Admin")]
		[HttpGet("login")]
		public async Task<ActionResult<userInfo>>UserLogin([FromForm] userInfo user)
		{
			if (user.password == null || (user.email == null && user.username==null)) 
				return BadRequest("Password and email/username required");
			var password = GenerateHashKey(user.password);

			var person = await _context.UserInfos.FirstOrDefaultAsync(e => e.username == user.username || e.email == user.email);

			if (person == null) return NotFound("User not found");
			if (person.Status == 0) return Unauthorized("User account deacitvated");
			if (person.password != password) return Unauthorized("Wrong username or password");
			else
			{
				user.username = person.username;
				user.email = person.email;
				user.password = password;

				string data = JsonConvert.SerializeObject(person);
				string token = GenerateToken(user);
				data=$"Login Successfull: \n {data} \n token:{token}"; 
				return Ok(data);
			}
		}

		[HttpPost("registration")]
		public async Task<ActionResult<userInfo>> CreateUser([FromForm] userInfo userinfo)
		{
			if (userinfo.email == null || userinfo.username == null || userinfo.password == null)
				return BadRequest("Email, Username, Password Required");
			var user = await _context.UserInfos.FirstOrDefaultAsync(e => e.username == userinfo.username || e.email == userinfo.email);
			if (user != null) return BadRequest("email or username already exist, try to login");

			string password = GenerateHashKey(userinfo.password);

			userinfo.email = userinfo.email;
			userinfo.password = password;
			userinfo.username = userinfo.username;
			userinfo.Status = 1;
			try
			{
				_context.UserInfos.Add(userinfo);
				await _context.SaveChangesAsync();
				return CreatedAtAction(nameof(GetUser), new { id = userinfo.Id }, userinfo);
			}
			catch
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error with User Information");
			}
		}
		public static string GenerateHashKey(string password)
		{
			//var temp1 = SHA256.Create();
			var temp2 = Encoding.UTF8.GetBytes(password);
			var hash = SHA256.Create().ComputeHash(temp2);
			return Convert.ToBase64String(hash);
		}


		private string GenerateToken(userInfo users)
		{
			try
			{
				var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
				var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
				var claims = new[] // optional
				{
				new Claim(ClaimTypes.NameIdentifier,users.email),
				//new Claim(ClaimTypes.Role,user.Role)
				new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Key"]),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				//new Claim(ClaimTypes.NameIdentifier, users.Id.ToString()),
				new Claim(ClaimTypes.NameIdentifier, users.username),
				new Claim(ClaimTypes.NameIdentifier, users.email),
				new Claim(ClaimTypes.NameIdentifier, users.password)
				//new Claim(ClaimTypes.NameIdentifier, DateTime.Now)
            };
				var token = new JwtSecurityToken(_config["Jwt:Issuer"],
					_config["Jwt:Audience"],
					null, // could be null
					expires: DateTime.Now.AddMinutes(15),
					signingCredentials: credentials);

				return new JwtSecurityTokenHandler().WriteToken(token);
			}
			catch (Exception) { return "Not Generated"; }
		}
	}
}
