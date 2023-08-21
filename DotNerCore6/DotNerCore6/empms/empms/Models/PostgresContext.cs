using System;
using System.Collections.Generic;
using empms.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace empms.Models
{
    // Inherits DBContext
    public class PostgresContext : DbContext
    {
        public PostgresContext()
        {
        }
        public PostgresContext(DbContextOptions<PostgresContext> options)
           : base(options)
        {
        }

        public virtual DbSet<Department> Departments { get; set; } = null!;  // Table entity
        public virtual DbSet<Employee> Employees { get; set; } = null!;  // Table entity
        public virtual DbSet<userInfo> UserInfos { get; set; } = null!;
        // Check Function
        public async Task<Employee> DeleteEmployeeByDeptId(int deptId)
        {
			//List<Employee> employee = new List<Employee>();
			List<Employee> employee = await Employees.Where(e => e.departmentId == deptId).ToListAsync();
            /*if (result == null) { break; }*/
            foreach(var i in employee)
            {
				//var emp = await Employees.FindAsync(i.Id);
				i.Id=i.Id;
                i.address = i.address;
                i.email = i.email;
                i.phone = i.phone;
                i.employee_name = i.employee_name;
                i.departmentId = 0;
                //Employees.Remove(i);
                await SaveChangesAsync();
             }
            return null;
        }
		public bool EmployeeExists(int id)
		{
			return Employees.Any(e => e.Id == id);
		}
		public bool DepartmentExists(int id)
		{
			return Departments.Any(e => e.Id == id);
		}


        
	}

}
