using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
	[Table("employee")]
	public class Employee
	{
		[Key]
		[Column("employee_id")]
		public string? employee_id { get; set; }
		[Column("employee_name")]
		public string? employee_name { get; set; }
		[Column("email")]
		public string? email { get; set; } = null;
		[Column("phone")]
		public string? phone_number { get; set; }
		[Column("address")]
		public string? address { get; set; }
		[Column("departmentId")]
		public string? departmentId { get; set; }
		[Column("joining_year")]
		public int year { get; set; }
		[Column("status")]
		public int status { get; set; }
	}
}
