using empms.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System;
//using Employee;
//using System.Web.Http;

namespace empms.Controllers
{
    [Route("api/[controller]")]  
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly PostgresContext _context;
        private readonly PostgresContext _employee;

		public DepartmentController(PostgresContext context, PostgresContext employee)
		{
			_context = context;
			_employee = employee;
		}

		// GET: api/DepartmentDetails
		[HttpGet]             // Search by localhost/api/department
		public async Task<ActionResult<IEnumerable<Department>>> GetDepartment()
        {
            return await _context.Departments.ToListAsync();
        }

        // Get By ID

        [HttpGet("get")]    // Search by localhost/api/department/{id}
		public async Task<ActionResult<Department>> Department([FromBody] Department dept)

        {
            try
            {
				var department = await _context.Departments.FindAsync(dept.Id);

				if (department == null)
				{
					return NotFound();
				}
				return department;

			}
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retreiving data from the database");
            }
        }

        // Update Data [PUT operation]

        [HttpPut("")]
		public async Task<IActionResult> PutDepartment([FromBody] Department department)
		{
			if (!DepartmentExists(department.Id)){
				return NotFound();
			}

			_context.Entry(department).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
                return Ok("Data Update Successful");
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating Data");
			}
		}

        // Create Data [post operation]

		[HttpPost]
        public async Task<ActionResult<Department>> CreateDepartment([FromBody] Department department)
        {
            try
            {
                if(department == null)
                {
                    return BadRequest();
                }
				_context.Departments.Add(department);
				await _context.SaveChangesAsync();

				return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);

			}
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Creating Data");
            }
            
        }



        //Delete Data [DELETE operation]
        [HttpDelete("")]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteDepartment([FromBody] Department dept)
		{
			if (dept.Id == 0) { return BadRequest("No Such Department"); }
			try
			{
				var deleteDepartment = await _context.Departments.FindAsync(dept.Id);
				if (deleteDepartment == null)
				{
					return NotFound($"No Data with id:{dept.Id} found");
				}
				else
				{
					await _context.DeleteEmployeeByDeptId(dept.Id);
					_context.Departments.Remove(deleteDepartment);
					await _context.SaveChangesAsync();
					return Ok($"Data deleted {dept.Id} : {dept.DepartmentName} successful");
				}
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error with Deleting Data");
			}
		}

		// Check for Data
		private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
		}
	}
}
