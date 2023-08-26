using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
	[Table ("users")]
	public class User
	{
		[Key]
		[Column("user_name")]
		public string? user_name { get; set; }
		[Column("email")]
		public string? email { get; set; }
		[Column("password")]
		public string? password { get; set; }
	}
}
