using Microsoft.EntityFrameworkCore;


namespace DictLib.Models
{
    class PromITContext:DbContext
    {
        private string _connect_string;
        public DbSet<DictElement> WordsStat { get; set; }

        public PromITContext(string connect_string)
        {
            _connect_string = connect_string;
            Database.EnsureCreated();         
        }       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connect_string);
        }

    }
}
