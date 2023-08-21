using Microsoft.EntityFrameworkCore;
namespace MyStore.DAL
{
	public class MyDbContext : DbContext
	{
		
		public MyDbContext(DbContextOptions options): base(options)
		{

		}
	}
}
