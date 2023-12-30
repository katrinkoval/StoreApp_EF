using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreService;

namespace StoreApp_EF
{
    class Program
    {
        static void Main(string[] args)
        {
            //StoreDbContext dbContext = new StoreDbContext("Data Source = localhost\\sqlexpress; Initial Catalog = Store; Integrated Security = True");
            //StoreDbService service = new StoreDbService(dbContext);
           
            ////int s = service.DeleteConsignment(1257);

            //string name = service.GetNameByIPN(1);

            //Console.WriteLine("Result = {0}", name);
            //Console.ReadKey();

            ////using (DataContext db = new DataContext("Data Source=localhost\\sqlexpress;Initial Catalog=Store;Integrated Security=True"))
            ////{
            ////    if (db.Connection.State == System.Data.ConnectionState.Open)
            ////    {
            ////        Console.WriteLine("+");
            ////    }

            ////    var orders = db.GetTable<OrderModel>();

            ////    Console.WriteLine("Orders\n");
            ////    foreach (OrderModel o in orders)
            ////    {
            ////        //Console.WriteLine(o.ToString());
            ////    }

            ////    Console.WriteLine("-----------------------------------------------");

            ////    var consignments = db.GetTable<ConsignmentModel>();

            ////    Console.WriteLine("Consignments\n");
            ////    foreach (ConsignmentModel cons in consignments)
            ////    {
            ////        //Console.WriteLine(cons.ToString());
            ////    }

            ////    Console.WriteLine("-----------------------------------------------");

            ////    var products = db.GetTable<ProductModel>();

            ////    Console.WriteLine("Products\n");
            ////    foreach (ProductModel p in products)
            ////    {
            ////        Console.WriteLine(p.ToString());
            ////    }


            ////}

            ////Console.ReadLine();

        }
    }
}
