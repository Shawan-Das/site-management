using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EmployeeManagement.Models
{
	[Table("department")]
	public class Department
	{
		[Key]
		[Column("department_id")]
		public string? department_id { get; set; }
		[Column("dep[artment_name")]
		public string? department_name { get; set; }
	}
}
