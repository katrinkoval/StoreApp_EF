using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Models;

namespace DataAccessLayer
{
    class StoreService       // StoreService, public
    {
        private const string DB_NAME = "Store";

        private readonly StoreDbContext _context;       //TODO: внешняя интерфейсная ссылка

        public StoreService(string servername, string login, string password)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            sqlConnectionStringBuilder.DataSource = servername;
            sqlConnectionStringBuilder.InitialCatalog = DB_NAME;
            sqlConnectionStringBuilder.UserID = login;
            sqlConnectionStringBuilder.Password = password;                 //useruser
            string strConnection = sqlConnectionStringBuilder.ToString();

            _context = new StoreDbContext(strConnection);
        }

        public StoreService()
        {
            _context = null;
        }

        public IEnumerable<Order> GetOrders()
        {
            var orders = from o in _context.Orders
                         join p in _context.Products on o.ProductID equals p.ID
                         join u in _context.Units on p.UnitID equals u.ID
                         select new Order
                         {
                             ConsignmentNumber = o.ConsignmentNumber,
                             Product = p.Name,
                             Price = p.Price,
                             Amount = o.Amount,
                             UnitType = u.UnitType,
                             TotalPrice = Convert.ToDouble(p.Price) * o.Amount
                         };

            return orders;
        }

        public IEnumerable<Order> GetConsignmentsOrders(int consigmentNumber)
        {

            var orders = from o in _context.Orders
                         join p in _context.Products on o.ProductID equals p.ID
                         join u in _context.Units on p.UnitID equals u.ID
                         where o.ConsignmentNumber == consigmentNumber
                         select new Order
                         {
                             ConsignmentNumber = o.ConsignmentNumber,
                             Product = p.Name,
                             Price = p.Price,
                             Amount = o.Amount,
                             UnitType = u.UnitType,
                             TotalPrice = Convert.ToDouble(p.Price) * o.Amount
                         };

            return orders;
        }

        public IEnumerable<Consignment> GetConsignments()
        {
            var consignments = from c in _context.Consignments
                              join supplier in _context.Individuals on c.SupplierID equals supplier.IPN
                              join recipient in _context.Individuals on c.SupplierID equals recipient.IPN
                              select new Consignment
                              {
                                 Number = c.Number,
                                 ConsignmentDate = c.ConsignmentDate,
                                 SupplierName = String.Format("{0}, {1}",supplier.Name, supplier.Surname),
                                 RecipientName = String.Format("{0}, {1}", recipient.Name, recipient.Surname),
                                 SupplierIpn = supplier.IPN,
                                 RecipientIpn = recipient.IPN
                              };

            return consignments;
        }


        public IEnumerable<Product> GetProducts()
        {
            var products = from p in _context.Products
                           join u in _context.Units on p.UnitID equals u.ID
                           select new Product 
                           { 
                               Name = p.Name,
                               UnitType = u.UnitType
                           };

            return products;
        }

        public IEnumerable<string> GetProductNames()
        {
            IQueryable<string> productNames = _context.Products.Select(p => p.Name);

            return productNames;
        }

        public IEnumerable<long> GetConsignmentNumbers()
        {
            IQueryable<long> consNumbers = _context.Consignments.Select(c => c.Number);

            return consNumbers;
        }

        public IEnumerable<long> GetIndividualIDs()
        {
            IQueryable<long> ids = _context.Individuals.Select(i => i.IPN);

            return ids;
        }

        public IEnumerable<string> GetIndividualNames()
        {
            IQueryable<string> names = _context.Individuals.Select(i => i.Name);

            return names;
        }

        public long GetProductID(string name)
        {
            long productId = _context.Products
                             .Where(p => p.Name == name)
                             .Select(p => p.ID)
                             .First();

            return productId;
        }

        public string GetNameByIPN(int ipn)
        {
            string name = _context.Individuals
                          .Where(i => i.IPN == ipn)
                          .Select(i => i.Name)
                          .First();

            return name;
        }

        public long GetIPNByName(string name)
        {

            long ipn = _context.Individuals
                           .Where(i => i.Name == name)
                           .Select(i => i.IPN)
                           .First();

            return ipn;
        }

        #region  ==== CRUD Order ===
        public int AddOrder(int consNumber, long prodID, double prodAmount)
        {
            return _context.AddOrder2(consNumber, prodID, prodAmount, null, null);    
        }

        public int UpdateOrder(int consNum, long prodID, double amount, long prodtIDNew)
        {
             return _context.UpdateOrder(consNum, prodID, prodtIDNew, amount);
        }

        public int DeleteOrder(int consNumber, long prodID)
        { 
            return _context.RemoveOrder(consNumber, prodID, 1);
        }

        #endregion


        #region  ==== CRUD Consignment ===

        public int ProcedureExecute(bool _isUsingIPN, string commandText, Consignment cons)
        {
            int result = 0;

            if (_isUsingIPN)
            {
                if (commandText == "AddConsignment2")
                {
                    result = _context.AddConsignment2(cons.Number, cons.SupplierIpn, cons.RecipientIpn, DateTime.Now
                                                , null, null, null, null, null);


                }
                else if(commandText == "UpdateConsignment")
                {
                    result = _context.UpdateConsignment(cons.Number, cons.SupplierIpn, cons.RecipientIpn, DateTime.Now
                                                , null, null, null, null, null);
                }
            }

            string[] supplierNameParts = cons.SupplierName.Split(' ');
            string[] recipientNameParts = cons.RecipientName.Split(' ');

            if (commandText == "AddConsignment2")
            {
                result = _context.AddConsignment2(cons.Number, null, null, DateTime.Now, supplierNameParts[0]
                                , supplierNameParts[1], recipientNameParts[0], recipientNameParts[1], null);
            }
            else if (commandText == "UpdateConsignment")
            {
                result = _context.UpdateConsignment(cons.Number, null, null, DateTime.Now, supplierNameParts[0]
                                , supplierNameParts[1], recipientNameParts[0], recipientNameParts[1], null);
            }

            return result;

        }


        public int DeleteConsignment(int consNumber)
        {
           return _context.RemoveConsignment(consNumber, 1);
        }

        public int DeleteConsignment2(int consNumber)
        {
            SqlParameter consNumberParam = new SqlParameter("@Number", consNumber);

            int result = _context.Database
                .SqlQuery<int>("GetResultsForCampaign @Number", consNumberParam).FirstOrDefault();

            return result;
        }

        #endregion
    }
}
