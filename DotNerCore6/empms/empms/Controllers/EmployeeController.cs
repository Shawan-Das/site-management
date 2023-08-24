using empms.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
//using Microsoft.AspNetCore.Mvc;

namespace empms.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly PostgresContext _context;  //could be context

		public EmployeeController(PostgresContext context)
		{
			_context = context;   // just use this.context
		}


		// GET: api/Employee
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
		{
			return await _context.Employees.Where(e=> e.departmentId != 0).ToListAsync();
		}

		// Get By ID

		[HttpGet("get")]    // Search by localhost/api/employee/{id}
		public async Task<ActionResult<Employee>>Employee([FromForm] Employee emp)
		{
			try
			{
				var employee = await _context.Employees.FindAsync(emp.Id);

				if (employee == null)
				{
					return NotFound($"Invalid Employee Data {emp}");
				}
				return employee;

			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving data from the database");
			}
		}

		[HttpGet("get-by-department")]    // Search by localhost/api/employee/{id}
		public async Task<ActionResult<IEnumerable<Employee>>> EmployeeByDepartment([FromForm] Employee emp)
		{
			List<Employee> employee = await _context.Employees.Where(e => e.departmentId == emp.departmentId).ToListAsync();

			if (employee == null) return NotFound("No sunch Employee");
			else return employee;
		}

		// Update Data [PUT operation]

		[HttpPut("")]
		public async Task<IActionResult> PutEmployee([FromForm] Employee employee)
		{
			var id=employee.Id;

			if (!EmployeeExists(id))
			{
				return NotFound();
			}

			_context.Entry(employee).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
				return Ok("Employee Data Update Successful");
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating Employee Data");
			}
		}

		// Create Data [post operation]

		[HttpPost]
		public async Task<ActionResult<Department>> CreateEmployee([FromForm] Employee employee)
		{
			try
			{
				if (employee == null)
				{
					return BadRequest();
				}
				_context.Employees.Add(employee);
				await _context.SaveChangesAsync();

				return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);

			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Creating Employee");
			}

		}

		[HttpDelete("")]
		public async Task<IActionResult> DeleteEmployee([FromForm] Employee emp)
		{
			try
			{
				var deleteEmployee= await _context.Employees.FindAsync(emp.Id);
				if (deleteEmployee== null)
				{
					return NotFound($"No Data with id:{emp.Id} found");
				}
				else
				{
					_context.Employees.Remove(deleteEmployee);
					await _context.SaveChangesAsync();
					return Ok($"Data delete with id:{emp.Id} successful");
				}
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error with Deleting Data");
			}
		}

		private bool EmployeeExists(int id)
		{
			return _context.Employees.Any(e => e.Id == id);
		}
	}
}



// Multi table data update
/*public async Task<Employee> UpdateEmployee(Employee employee)
{
	var result = await appDbContext.Employees
		.FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);

	if (result != null)
	{
		result.FirstName = employee.FirstName;
		result.LastName = employee.LastName;
		result.Email = employee.Email;
		result.DateOfBrith = employee.DateOfBrith;
		result.Gender = employee.Gender;
		if (employee.DepartmentId != 0)
		{
			result.DepartmentId = employee.DepartmentId;
		}
		else if (employee.Department != null)
		{
			result.DepartmentId = employee.Department.DepartmentId;
		}
		result.PhotoPath = employee.PhotoPath;

		await appDbContext.SaveChangesAsync();

		return result;
	}

	return null;
}*/