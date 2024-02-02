using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoreService;
using DataAccessLayer;
using ModelsDTO;
using ModelsDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

namespace StoreAppTest
{
    [TestClass]
    public class OrderOperationsTests
    {
        const string TEST_DB_NAME = "StoreTest";


        private static StoreDbService _service;
        private static IStoreDBContext _storeDB;

        [ClassInitialize]
        public static void ConnectToTestDBAsync(TestContext testContext)
        {
            StoreDataBaseCopier.CreateStoreDbCopy();

            _storeDB = new StoreDbContext(string.Format("Data Source = localhost\\sqlexpress; Initial Catalog = {0}; Integrated Security=True", TEST_DB_NAME));

            _service = new StoreDbService(_storeDB);

        }


        [TestInitialize]
        public void InitializeDB()
        {
            ModelsDTO.Consignment newConsignment = new ModelsDTO.Consignment()
            {
                Number = 1,
                ConsignmentDate = DateTime.Now,
                SupplierIpn = 1,
                RecipientIpn = 2
            };

            _service.ProcedureExecute(true, "AddConsignment", newConsignment);

            int result = _service.AddOrder(1, 1, 1);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            RemoveAllOrders();
            RemoveAllConsignments();
        }

        private static void RemoveAllOrders()
        {
            string commandStr = "DELETE FROM Orders";
            _storeDB.Database.ExecuteSqlCommand(commandStr);
        }

        private static void RemoveAllConsignments()
        {
            string commandStr = "DELETE FROM Consignments";
            _storeDB.Database.ExecuteSqlCommand(commandStr);
        }


        [TestMethod]
        public void GetOrdersTest()
        {
            IEnumerable<ModelsDTO.Order> orders = _service.GetOrders();

            int expectedNumber = 1;
            int actualNumber = orders.Count();

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void GetConsignmentsOrdersTest()
        {
            _service.AddOrder(1, 2, 1);

            IEnumerable<ModelsDTO.Order> orders = _service.GetConsignmentsOrders(1);

            int expectedNumber = 2;
            int actualNumber = orders.Count();

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void GetOrdersFromConsignmentWithoutOrdersTest()
        {
            int consignmentNumber = 2;

            ModelsDTO.Consignment newConsignment = new ModelsDTO.Consignment()
            {
                Number = consignmentNumber,
                ConsignmentDate = DateTime.Now,
                SupplierIpn = 1,
                RecipientIpn = 2
            };

            _service.ProcedureExecute(true, "AddConsignment", newConsignment);

            IEnumerable<ModelsDTO.Order> orders = _service.GetConsignmentsOrders(consignmentNumber);

            int expectedNumber = 0;
            int actualNumber = orders.Count();

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void AddOrderWithNotExistsConsignmentTest()
        {
            int consignmentNumber = 2;
            int productId = 1;
            double productAmount = 1;

            _service.AddOrder(consignmentNumber, productId, productAmount);

            ModelsDTO.Order actualAddedOrder = _service
                                                  .GetOrders()
                                                  .Where(o => o.ConsignmentNumber == consignmentNumber)
                                                  .FirstOrDefault();

            Assert.IsNull(actualAddedOrder);
        }

        [TestMethod]
        public void AddOrderWithNotExistsConsignmentErrorCodeTest()
        {
            int consignmentNumber = 2;
            int productId = 1;
            double productAmount = 1;

            int expectedErrorCode = 1;
            int actualErrorCode = _service.AddOrder(consignmentNumber, productId, productAmount);

            Assert.AreEqual(expectedErrorCode, actualErrorCode);
        }

        [TestMethod]
        public void AddOrderWithNotExistsProductTest()
        {
            int consignmentNumber = 1;
            int productId = 10;
            double productAmount = 1;

            _service.AddOrder(consignmentNumber, productId, productAmount);

            ModelsDAL.Order actualAddedOrder = _storeDB
                                            .Orders
                                            .Where(o => o.ConsignmentNumber == consignmentNumber
                                                             && o.Product.ID == productId)
                                            .FirstOrDefault();

            Assert.IsNull(actualAddedOrder);
        }

        [TestMethod]
        public void AddOrderWithNotExistsProductErrorCodeTest()
        {
            int consignmentNumber = 1;
            int productId = 10;
            double productAmount = 1;

            int expectedErrorCode = 2;
            int actualErrorCode = _service.AddOrder(consignmentNumber, productId, productAmount);

            Assert.AreEqual(expectedErrorCode, actualErrorCode);
        }

        [TestMethod]
        public void AddOrderSuccessfullTest()
        {
            int consignmentNumber = 1;
            int productId = 1;
            double productAmount = 1;

            _service.AddOrder(consignmentNumber, productId, productAmount);

            ModelsDAL.Order actualAddedOrder = _storeDB
                                            .Orders
                                            .Where(o => o.ConsignmentNumber == consignmentNumber
                                                                     && o.Product.ID == productId)
                                            .FirstOrDefault();

            Assert.IsNotNull(actualAddedOrder);
        }

        [TestMethod]
        public void UpdateOrderWithNotExistsConsignmentTest()
        {
            int consignmentNumber = 2;
            long productId = 1;
            double productAmount = 1;
            long productIdUpdated = 2;

            int expectedResult = 1;
            int actualResult = _service.UpdateOrder(consignmentNumber, productId, productAmount, productIdUpdated);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void UpdateOrderWithNotExistsUpdatedProductTest()
        {
            int consignmentNumber = 1;
            long productId = 3;
            double productAmount = 1;
            long productIdUpdated = 10;

            int expectedResult = 2;
            int actualResult = _service.UpdateOrder(consignmentNumber, productId, productAmount, productIdUpdated);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void UpdateOrderWithNotExistsProductTest()
        {
            int consignmentNumber = 1;
            long productId = 20;
            double productAmount = 1;
            long productIdUpdated = 1;

            int expectedResult = 3;
            int actualResult = _service.UpdateOrder(consignmentNumber, productId, productAmount, productIdUpdated);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void UpdateOrderSuccessfullTest()
        {
            int consignmentNumber = 1;
            long productId = 1;
            double productAmount = 1;
            long productIdUpdated = 2;

            _service.UpdateOrder(consignmentNumber, productId, productAmount, productIdUpdated);

            ModelsDAL.Order updatedOrder = _storeDB
                                            .Orders
                                            .Where(o => o.ConsignmentNumber == consignmentNumber
                                                             && o.Product.ID == productIdUpdated)
                                            .FirstOrDefault();

            Assert.IsNotNull(updatedOrder);

        }

        [TestMethod]
        public void DeleteOrderWithIncorrectConsIdTest()
        {
            int consignmentNumber = 2;
            long productId = 4;

            int expectedResult = 1;
            int actualResult = _service.DeleteOrder(consignmentNumber, productId);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void DeleteOrderWithIncorrectProductIdTest()
        {
            int consignmentNumber = 1;
            long productId = 10;

            int expectedResult = 3;
            int actualResult = _service.DeleteOrder(consignmentNumber, productId);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void DeleteOrderSuccessfullTest()
        {
            int consignmentNumber = 1;
            long productId = 1;

            _service.DeleteOrder(consignmentNumber, productId);

            ModelsDTO.Order deletedOrder = _service
                                            .GetOrders()
                                            .Where(o => o.ConsignmentNumber == consignmentNumber 
                                             && _service.GetProductID(o.Product) == productId)
                                            .FirstOrDefault();

            Assert.IsNull(deletedOrder);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            StoreDataBaseCopier.RemoveStoreDbCopy();

        }

    }
}
