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
//using System.Web.Http;

namespace empms.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly PostgresContext _context;  //could be context

		public UserController(PostgresContext context)
		{
			_context = context;   // just use this.context
		}


		[HttpGet("login")]
		//[Authorize]
		public async Task<ActionResult<userInfo>>UserLogin([FromBody] userInfo user)
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
				return Ok($"Login Successful:{person}");
			}
		}

		[HttpPost("registration")]
		public async Task<ActionResult<userInfo>> CreateUser([FromBody] userInfo userinfo)
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
			var temp1 = SHA256.Create();
			var temp2 = Encoding.Default.GetBytes(password);
			var hash = temp1.ComputeHash(temp2);
			return Convert.ToBase64String(hash);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<userInfo>>> GetUser()
		{
			return await _context.UserInfos.ToListAsync();
		}

	}
}
