﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ModelsDTO;
using DataAccessLayer;
using StoreService.Exceptions;
using System.Data;
using System.Data.SqlTypes;

namespace StoreService
{
    public class StoreDbService: IStoreService, IDisposable
    {
        private IStoreDBContext _context;       //TODO: внешняя интерфейсная ссылка!!!!!
        private bool _alreadyDisposed;

        private const string DB_NAME = "Store";

        public StoreDbService(IStoreDBContext context)
        {
            _context = context;
        }

        public StoreDbService()
        {
            _context = null;
        }

        private string GetConnectionString(string servername, string login, string password)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            sqlConnectionStringBuilder.DataSource = servername;
            sqlConnectionStringBuilder.InitialCatalog = DB_NAME;
            sqlConnectionStringBuilder.UserID = login;
            sqlConnectionStringBuilder.Password = password;                 //useruser

            return sqlConnectionStringBuilder.ToString();
        }

        private void CheckConnectionState()
        {
            if (_alreadyDisposed)
            {
                throw new ObjectDisposedException(nameof(StoreDbService));
            }
            if (_context.Database.Connection.State == System.Data.ConnectionState.Closed)
            {
                throw new SqlConnectionException("Connection is closed");
            }
        }

        public void OpenConnection(string servername = null, string login = null, string password = null)
        {
            if (_alreadyDisposed)
            {
                throw new ObjectDisposedException(nameof(StoreDbService));
            }

            if (_context == null && servername != null && login != null && password != null)
            {
                _context = new StoreDbContext(GetConnectionString(servername, login, password));
            }
            else
            {
                throw new SqlConnectionException("Incorrect connection string");
            }

            if (_context.Database.Connection.State == System.Data.ConnectionState.Closed)
            {
                _context.Database.Connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (_alreadyDisposed)
            {
                throw new ObjectDisposedException(nameof(StoreDbService));
            }

            if (_context.Database.Connection.State == System.Data.ConnectionState.Open)
            {
                _context.Database.Connection.Close();
            }
        }

        public void Dispose()
        {
            if (_alreadyDisposed || _context == null)
            {
                return;
            }

            _context.Dispose();

            _alreadyDisposed = true;

            GC.SuppressFinalize(this);
        }

        ~StoreDbService()
        {
            if (_alreadyDisposed || _context == null)
            {
                return;
            }

            Dispose();
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
                             TotalPrice = (double)p.Price * o.Amount
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
                             TotalPrice = (double)p.Price * o.Amount
                         };

            return orders;
        }

        public IEnumerable<Consignment> GetConsignments()
        {
            var consignments = from c in _context.Consignments
                               join supplier in _context.Individuals on c.SupplierID equals supplier.IPN
                               join recipient in _context.Individuals on c.RecipientID equals recipient.IPN
                               select new ModelsDTO.Consignment
                               {
                                   Number = c.Number,
                                   ConsignmentDate = c.ConsignmentDate,
                                   SupplierName =  supplier.Name + " " + supplier.Surname,
                                   RecipientName =  recipient.Name + " " + recipient.Surname,
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
            IQueryable<string> names = _context.Individuals.Select(i => i.Name + " " + i.Surname);

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
                          .Select(i => i.Name + " " + i.Surname)
                          .First();

            return name;
        }

        public long GetIPNByName(string name)
        {
            string[] nameParts = name.Split(' ');

            string firstName = nameParts[0];
            string lastName = nameParts[1];

            long ipn = _context.Individuals
                           .Where(i => i.Name == firstName && i.Surname == lastName)
                           .Select(i => i.IPN)
                           .First();

            return ipn;
        }

        #region  ==== CRUD Order ===

        public int AddOrder(int consNumber, long prodID, double prodAmount)
        {
            SqlParameter number = new SqlParameter("@ConsigmentNumber", consNumber);
            SqlParameter productID = new SqlParameter("@ProductID", prodID);
            SqlParameter productAmount = new SqlParameter("@Amount", prodAmount);
            SqlParameter resultParameter = new SqlParameter("@ErrorCode", SqlDbType.Int);
            resultParameter.Direction = ParameterDirection.Output;

            _context.Database
                    .ExecuteSqlCommand("AddOrder @ConsigmentNumber, @ProductID, @Amount, @ErrorCode OUTPUT"
                    , number, productID, productAmount, resultParameter);

            return (int)resultParameter.Value;
        }

        public int UpdateOrder(int consNum, long prodID, double amount, long prodtIDNew)
        {
            SqlParameter number = new SqlParameter("@Number", consNum);
            SqlParameter productID = new SqlParameter("@ProductID", prodID);
            SqlParameter productIDUpdated = new SqlParameter("@ProductIDUpdated", prodtIDNew);
            SqlParameter productAmount = new SqlParameter("@Amount", amount);
            SqlParameter resultParameter = new SqlParameter("@ErrorCode", SqlDbType.Int);
            resultParameter.Direction = ParameterDirection.Output;

            _context.Database
                    .ExecuteSqlCommand("UpdateOrder @Number, @ProductID, @ProductIDUpdated" +
                    ", @Amount,  @ErrorCode OUTPUT", number, productID, productIDUpdated
                    , productAmount, resultParameter);

            return (int)resultParameter.Value;
        }

        public int DeleteOrder(int consNumber, long prodID)
        {
            SqlParameter number = new SqlParameter("@Number", consNumber);
            SqlParameter productID = new SqlParameter("@ProductID", prodID);
            SqlParameter interval = new SqlParameter("@DateInterval", 1);

            SqlParameter resultParameter = new SqlParameter("@ErrorStatus", SqlDbType.Int);
            resultParameter.Direction = ParameterDirection.Output;

             _context.Database
                     .ExecuteSqlCommand("RemoveOrder @Number, @ProductID, @DateInterval, @ErrorStatus OUTPUT"
                     , number, productID, interval, resultParameter);

            return (int)resultParameter.Value;
        }

        #endregion


        #region  ==== CRUD Consignment ===

        public int ProcedureExecute(bool _isUsingIPN, string commandText, Consignment cons)
        {
            string queryStr;
            SqlParameter number = new SqlParameter("@Number", cons.Number);
            SqlParameter date = new SqlParameter("@ConsigmentDate", cons.ConsignmentDate);
            SqlParameter resultParameter = new SqlParameter("@ErrorStatus", SqlDbType.Int);
            resultParameter.Direction = ParameterDirection.Output;

            SqlParameter supplierIPN = (_isUsingIPN) ? new SqlParameter("@SupplierID", cons.SupplierIpn) : new SqlParameter("@SupplierID", SqlInt64.Null);
            SqlParameter recipientIPN = (_isUsingIPN) ? new SqlParameter("@RecipientID", cons.RecipientIpn) : new SqlParameter("@RecipientID", SqlInt64.Null);


            string[] supplierNameParts = (_isUsingIPN) ? null : cons.SupplierName.Split(' ');
            string[] recipientNameParts = (_isUsingIPN) ? null : cons.RecipientName.Split(' ');


            SqlParameter supplierName = new SqlParameter("@SupplierName", (_isUsingIPN) ? SqlString.Null : supplierNameParts[0]);
            SqlParameter supplierSurname = new SqlParameter("@SupplierSurname", (_isUsingIPN) ? SqlString.Null : supplierNameParts[1]);
            SqlParameter recipientName = new SqlParameter("@RecipientName", (_isUsingIPN) ? SqlString.Null : recipientNameParts[0]);
            SqlParameter recipientSurname = new SqlParameter("@RecipientSurname", (_isUsingIPN) ? SqlString.Null : recipientNameParts[1]);

            queryStr = string.Format("Exec {0} @Number, @SupplierID, @RecipientID, @ConsigmentDate" +
                                        ", @SupplierName, @SupplierSurname, @RecipientName, @RecipientSurname" +
                                        ", @ErrorStatus OUTPUT", commandText);

            _context.Database.ExecuteSqlCommand(queryStr, number, supplierIPN, recipientIPN, date, supplierName, supplierSurname, recipientName, recipientSurname, resultParameter);

            return (int)resultParameter.Value;
        }

        
        public int DeleteConsignment(int consNumber)
        {
            SqlParameter consNumberParam = new SqlParameter("@Number", consNumber);
            
            SqlParameter interval = new SqlParameter("@DateInterval", 1);

            SqlParameter resultParameter = new SqlParameter("@ErrorStatus", SqlDbType.Int);
            resultParameter.Direction = ParameterDirection.Output;

             _context.Database
                     .ExecuteSqlCommand("Exec RemoveConsignment @Number, @DateInterval, @ErrorStatus OUTPUT"
                     , consNumberParam, interval, resultParameter);

            return (int)resultParameter.Value;
        }


        #endregion
    }
}
