using System.ComponentModel.DataAnnotations;

namespace MyStore.Models.DBEntites
{
	public class clients
	{
		[Key]
		public int id { get; set; }
		public string name { get; set; }
		[Required]
		public string? email { get; set; }
		[Required]
		public string? phone { get; set; }
		[Required]
		public string? address { get; set; }
		[Required]
		public string? createdAt { get; set; }
		[Required]
	}
}
