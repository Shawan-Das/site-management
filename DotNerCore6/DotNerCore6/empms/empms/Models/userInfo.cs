using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace empms.Models
{
	[Table("userinfo")]
	public class userInfo
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }
		[Column("email")]
		public string? email{ get; set; }
		[Column("username")]
		public string? username { get; set; }
		[Column("password")]
		public string? password { get; set; }
		[Column("status")]
		public int Status { get; set; }
	}
}
