using System;
using System.Collections.Generic;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
	public class PostgresContext : DbContext
	{
		public PostgresContext()
		{
		}
		public PostgresContext(DbContextOptions<PostgresContext> options)
		   : base(options)
		{
		}

		// Models
		public virtual DbSet<Employee> employees { get; set; } = null!;
		public virtual DbSet<Department> departments { get; set; } = null!;
		public virtual DbSet<User> users { get; set; } = null!;
		public virtual DbSet<UserModel> userModels { get; set; } = null!;

		// Department Functions
		// All departments
		public async Task<ActionResult<IEnumerable<Department>>> GetaDepartment()
		{
			return await departments.ToListAsync();
		}
		// Get Specific Department
		public async Task<ActionResult<Department>> GetDepartment(string id)
		{
			var data = await departments.FirstOrDefaultAsync(e=> e.department_id==id);
			return data!;
		}
		// Add department 
		public async Task<ActionResult<Department>> CreateDepartment(Department dept)
		{
			var result = departments.Add(dept);
			await SaveChangesAsync();
			return result.Entity;
		}
		//Update Department
		public async Task<Department> updateDepartment(Department dept)
		{
			var info = await departments.FirstOrDefaultAsync(e => e.department_id == dept.department_id);
			if (info != null) 
			{
				info.department_name = dept.department_name;
				Entry(info).State = EntityState.Modified;
				await SaveChangesAsync();
				return info;
			}
			else
			{
				return null!;
			}
		}

	// user function
		//Get all users
		public async Task<ActionResult<IEnumerable<User>>> GetaUsers()
		{
			return await users.ToListAsync();
		}
		//Get User by username or email
		public async Task<ActionResult<User>> GetaDepartment(User user)
		{
			var info = await users.FirstOrDefaultAsync(e => (e.email == user.email) || (e.user_name== user.user_name));
			return info!;
		}
		// Add User
		public async Task<ActionResult<User>> AddUser(User user)
		{
			if (user.password == null) return user;
			user.password= GenerateHashKey(user.password);
			var result = users.Add(user);
			await SaveChangesAsync();
			return result.Entity;
		}

		// Employee Functions
		// All employees
		public async Task<ActionResult<IEnumerable<Employee>>> GetaEmployee()
		{
			return await employees.ToListAsync();
		}
		// Get Specific employee
		public async Task<ActionResult<Employee>> GetEmployee(string id)
		{
			var data = await employees.FirstOrDefaultAsync(e => e.employee_id == id);
			return data!;
		}
		// Get Specific Employee By department
		public async Task<ActionResult<IEnumerable<Employee>>> GetaEmployeeByDept(string deptId)
		{
			List<Employee> emp = await employees.Where(e => e.departmentId == deptId).ToListAsync();
			return emp;
		}
		//Add Employee 
		public async Task<ActionResult<Employee>> AddEmployee(Employee emp)
		{
			if (emp.departmentId == null) emp.departmentId = "000";
			emp.employee_id = GenerateEmployeeId(emp.year, emp.departmentId);
			var result = employees.Add(emp);
			await SaveChangesAsync();
			return result.Entity;
		}
		// Delete Employee
		public async Task<Employee> InActiveEmployee(string id)
		{
			Employee result = (Employee)employees.Where(e => e.employee_id == id);
			result.status = 0;

			Entry(result).State = EntityState.Modified;
			await SaveChangesAsync();
			return result;
		}

	// Extra functions
		//password generator
		public string GenerateHashKey(string password)
		{
			//var temp1 = SHA256.Create();
			var temp2 = Encoding.UTF8.GetBytes(password);
			var hash = SHA256.Create().ComputeHash(temp2);
			return Convert.ToBase64String(hash);
		}
		public string GenerateEmployeeId(int year, string deptId)
		{
			List<Employee> emp =  employees.Where(e => e.departmentId == deptId).ToList();
			int count = emp.Count + 1;
			string temp;
			if (count < 10) temp = "00" + count.ToString();
			else if (count < 99) temp = "0" + count.ToString();
			else temp = count.ToString();

			return year.ToString() + temp + count.ToString();
		}
	
	}
}


/*
 * public async  Task<ActionResult<UserModel>> CreateEmployee(UserModel um)
		{
			// total employee count
			List<Employee> emp = await employees.Where(e => e.departmentId == um.departmentCode).ToListAsync();
			int count= emp.Count;
			string temp;
			if (count < 10) temp = "00" + count.ToString();
			else if(count<99) temp = "0" + count.ToString();
			else temp = count.ToString();

			var employee = new Employee();
			var user = new User();

			employee.employee_id = um.joiningYear.ToString() 
								+ um.departmentCode + temp;
			employee.employee_name = um.firstName+ " "+ um.lastName;
			employee.email = um.email;
			employee.address= um.address;
			employee.phone_number = um.phoneNumbner;
			employee.departmentId = um.departmentCode;
			employee.year = um.joiningYear;
			employee.status = 1;

			user.email = um.email;
			user.user_name = um.userName;
			user.password= GenerateHashKey(um.password);

			var result1 = AddEmployee(employee);
			var result2 = AddUser(user);

			
			if (result1 != null && result2 !=null) return um;
			else
			{
				if (result1 == null)
				{
					employees.Remove(employee);
					await SaveChangesAsync();
				}
				if (result2 == null)
				{
					users.Remove(user);
					await SaveChangesAsync();
				}

			}
		}
*/