using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoreService;
using DataAccessLayer;
using ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreAppTest
{
    [TestClass]
    public class OrderOperationsTests
    {
        const string CONNECTION_STRING = "Data Source = localhost\\sqlexpress; Initial Catalog = Store; Integrated Security = True";

        private StoreDbService _service;

        [TestInitialize]
        public void SetUpConnection()
        {
            IStoreDBContext storeDB = new StoreDbContext(CONNECTION_STRING);
            _service = new StoreDbService(storeDB);
        }

        [TestMethod]
        public void GetOrdersTest()
        {
            IEnumerable<Order> orders = _service.GetOrders();

            int expectedNumber = 16;
            int actualNumber = orders.Count();

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void GetConsignmentsOrdersTest()
        {
            int consignmentNumber = 1241;

            IEnumerable<Order> orders = _service.GetConsignmentsOrders(consignmentNumber);

            int expectedNumber = 4;
            int actualNumber = orders.Count();

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void GetOrdersFromConsignmentWithoutOrdersTest()
        {
            int consignmentNumber = 1243;

            IEnumerable<Order> orders = _service.GetConsignmentsOrders(consignmentNumber);

            int expectedNumber = 0;
            int actualNumber = orders.Count();

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void AddOrderWithNotExistsConsignmentTest()
        {
            int consignmentNumber = 1277;
            int productId = 1;
            double productAmount = 1;

            int expectedResult = 1;
            int actualResult = _service.AddOrder(consignmentNumber, productId, productAmount);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void AddOrderWithNotExistsProductTest()
        {
            int consignmentNumber = 1241;
            int productId = 10;
            double productAmount = 1;

            int expectedResult = 2;
            int actualResult = _service.AddOrder(consignmentNumber, productId, productAmount);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void AddOrderSuccessfullTest()
        {
            int consignmentNumber = 1241;
            int productId = 1;
            double productAmount = 1;

            int expectedResult = 0;
            int actualResult = _service.AddOrder(consignmentNumber, productId, productAmount);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void UpdateOrderWithNotExistsConsignmentTest()
        {
            int consignmentNumber = 1277;
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
            int consignmentNumber = 1236;
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
            int consignmentNumber = 1236;
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
            int consignmentNumber = 1241;
            long productId = 1;
            double productAmount = 1;
            long productIdUpdated = 2;

            int expectedResult = 0;
            int actualResult = _service.UpdateOrder(consignmentNumber, productId, productAmount, productIdUpdated);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void DeleteOrderWithIncorrectConsIdTest()
        {
            int consignmentNumber = 1277;
            long productId = 4;

            int expectedResult = 1;
            int actualResult = _service.DeleteOrder(consignmentNumber, productId);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void DeleteOrderWithIncorrectProductIdTest()
        {
            int consignmentNumber = 1241;
            long productId = 10;

            int expectedResult = 3;
            int actualResult = _service.DeleteOrder(consignmentNumber, productId);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void DeleteOrderSuccessfullTest()
        {
            int consignmentNumber = 1241;
            long productId = 4;

            int expectedResult = 0;
            int actualResult = _service.DeleteOrder(consignmentNumber, productId);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
