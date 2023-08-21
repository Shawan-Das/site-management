using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace WebApplication1.Models
{
	public class PostgresContext : DbContext
	{
		public PostgresContext(DbContextOptions<PostgresContext> options)
			: base(options)
		{ }
		public  DbSet<Employee> employes { get; set; }
		public  DbSet<Department> departments { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if(!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseNpgsql("Host=localhost;Database=testdb;Port=8080;Username= postgres;Password=1234");
			}
		}
	}
}
