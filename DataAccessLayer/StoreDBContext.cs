using ModelsDAL;
using System.Data.Entity;

namespace DataAccessLayer
{
    public class StoreDbContext : DbContext, IStoreDBContext
    {
        //const string CONNECTION_STRING = "Data Source = localhost\\sqlexpress; Initial Catalog = Store; Integrated Security = True";

        public StoreDbContext(string connectionString)
            : base(connectionString)
        {
          
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Consignment> Consignments { get; set; }
        public DbSet<Individual> Individuals { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
