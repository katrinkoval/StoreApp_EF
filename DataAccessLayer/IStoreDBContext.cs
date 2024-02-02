using System;
using System.Data.Entity;
using ModelsDAL;

namespace DataAccessLayer
{
    public interface IStoreDBContext: IDisposable
    {
        DbSet<Product> Products { get; set; }
        DbSet<Consignment> Consignments { get; set; }
        DbSet<Individual> Individuals { get; set; }
        DbSet<Unit> Units { get; set; }
        DbSet<Order> Orders { get; set; }
        Database Database { get; }

        int SaveChanges();
    }
}
