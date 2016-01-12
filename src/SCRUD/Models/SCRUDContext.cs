/*
	Created By Gene Kochanowsky	
*/
using Microsoft.Data.Entity;
using SCRUD.Models;

namespace SCRUD.Models
{
    public class SCRUDContext : DbContext
    {
        private static bool _created = false;

        public SCRUDContext()
        {
            if (!_created)
            {
                _created = true;
                Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        public DbSet<Person> Person { get; set; }

        public DbSet<Gender> Gender { get; set; }
    }
}
