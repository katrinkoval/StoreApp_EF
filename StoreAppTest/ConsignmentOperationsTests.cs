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
    class ConsignmentOperationsTests
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
        public void GetConsignmentsTest()
        {
            IEnumerable<Consignment> consigments = _service.GetConsignments();

            int expectedNumber = 10;
            int actualNumber = consigments.Count();

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void GetConsignmentNumbersTest()
        {
            IEnumerable<long> consigments = _service.GetConsignmentNumbers();

            int expectedNumber = 10;
            int actualNumber = consigments.Count();

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        
    }
}
