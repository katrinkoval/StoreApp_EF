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
    class ProductOperationsTests
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
        public void GetProductsTest()
        {
            IEnumerable<Product> products = _service.GetProducts();

            int expectedNumber = 6;
            int actualNumber = products.Count();

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void GetProductNamesTest()
        {
            IEnumerable<string> products = _service.GetProductNames();

            int expectedNumber = 6;
            int actualNumber = products.Count();

            Assert.AreEqual(expectedNumber, actualNumber);
        }


        [TestMethod]
        public void GetProductIdTest()
        {
            string productName = "Сement";

            long actualId = _service.GetProductID(productName);
            long expectedId = 1;

            Assert.AreEqual(expectedId, actualId);
        }

        [TestMethod]
        public void GetProductIdWithIncorrectNameTest()
        {
            string productName = "a";

            long actualId = _service.GetProductID(productName);
            long expectedId = 0;

            Assert.AreEqual(expectedId, actualId);
        }
    }   
}
