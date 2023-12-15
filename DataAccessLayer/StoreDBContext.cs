using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsDB;

namespace DataAccessLayer
{
    class StoreDbContext : DbContext
    {
        //const string CONNECTION_STRING = "Data Source = localhost\\sqlexpress; Initial Catalog = Store; Integrated Security = True";

        public StoreDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<ProductModel> Products { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<ConsignmentModel> Consignments { get; set; }
        public DbSet<IndividualModel> Individuals { get; set; }
        public DbSet<UnitModel> Units { get; set; }

        public virtual int AddConsignment2(Nullable<long> number, Nullable<long> supplierID, Nullable<long> recipientID, Nullable<System.DateTime> consigmentDate, string supplierName, string supplierSurname, string recipientName, string recipientSurname, ObjectParameter addedConsigmentNumber)
        {
            var numberParameter = number.HasValue ?
                new ObjectParameter("Number", number) :
                new ObjectParameter("Number", typeof(long));

            var supplierIDParameter = supplierID.HasValue ?
                new ObjectParameter("SupplierID", supplierID) :
                new ObjectParameter("SupplierID", typeof(long));

            var recipientIDParameter = recipientID.HasValue ?
                new ObjectParameter("RecipientID", recipientID) :
                new ObjectParameter("RecipientID", typeof(long));

            var consigmentDateParameter = consigmentDate.HasValue ?
                new ObjectParameter("ConsigmentDate", consigmentDate) :
                new ObjectParameter("ConsigmentDate", typeof(System.DateTime));

            var supplierNameParameter = supplierName != null ?
                new ObjectParameter("SupplierName", supplierName) :
                new ObjectParameter("SupplierName", typeof(string));

            var supplierSurnameParameter = supplierSurname != null ?
                new ObjectParameter("SupplierSurname", supplierSurname) :
                new ObjectParameter("SupplierSurname", typeof(string));

            var recipientNameParameter = recipientName != null ?
                new ObjectParameter("RecipientName", recipientName) :
                new ObjectParameter("RecipientName", typeof(string));

            var recipientSurnameParameter = recipientSurname != null ?
                new ObjectParameter("RecipientSurname", recipientSurname) :
                new ObjectParameter("RecipientSurname", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddConsignment2"
                , numberParameter, supplierIDParameter, recipientIDParameter
                , consigmentDateParameter, supplierNameParameter, supplierSurnameParameter
                , recipientNameParameter, recipientSurnameParameter, addedConsigmentNumber);
        }

        public virtual int AddOrder2(Nullable<long> consigmentNumber, Nullable<long> productID, Nullable<double> amount, ObjectParameter addedConsigmentNumber, ObjectParameter addedProductID)
        {
            var consigmentNumberParameter = consigmentNumber.HasValue ?
                new ObjectParameter("ConsigmentNumber", consigmentNumber) :
                new ObjectParameter("ConsigmentNumber", typeof(long));

            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(long));

            var amountParameter = amount.HasValue ?
                new ObjectParameter("Amount", amount) :
                new ObjectParameter("Amount", typeof(double));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddOrder2", consigmentNumberParameter, productIDParameter, amountParameter, addedConsigmentNumber, addedProductID);
        }

        public virtual int RemoveConsignment(Nullable<long> number, Nullable<byte> dateInterval)
        {
            var numberParameter = number.HasValue ?
                new ObjectParameter("Number", number) :
                new ObjectParameter("Number", typeof(long));

            var dateIntervalParameter = dateInterval.HasValue ?
                new ObjectParameter("DateInterval", dateInterval) :
                new ObjectParameter("DateInterval", typeof(byte));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("RemoveConsignment", numberParameter, dateIntervalParameter);
        }

        public virtual int RemoveOrder(Nullable<long> number, Nullable<long> productID, Nullable<byte> dateInterval)
        {
            var numberParameter = number.HasValue ?
                new ObjectParameter("Number", number) :
                new ObjectParameter("Number", typeof(long));

            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(long));

            var dateIntervalParameter = dateInterval.HasValue ?
                new ObjectParameter("DateInterval", dateInterval) :
                new ObjectParameter("DateInterval", typeof(byte));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("RemoveOrder", numberParameter, productIDParameter, dateIntervalParameter);
        }

        public virtual int UpdateConsignment(Nullable<long> number, Nullable<long> supplierID, Nullable<long> recipientID, Nullable<System.DateTime> consigmentDate, string supplierName, string supplierSurname, string recipientName, string recipientSurname, ObjectParameter addedConsigmentNumber)
        {
            var numberParameter = number.HasValue ?
                new ObjectParameter("Number", number) :
                new ObjectParameter("Number", typeof(long));

            var supplierIDParameter = supplierID.HasValue ?
                new ObjectParameter("SupplierID", supplierID) :
                new ObjectParameter("SupplierID", typeof(long));

            var recipientIDParameter = recipientID.HasValue ?
                new ObjectParameter("RecipientID", recipientID) :
                new ObjectParameter("RecipientID", typeof(long));

            var consigmentDateParameter = consigmentDate.HasValue ?
                new ObjectParameter("ConsigmentDate", consigmentDate) :
                new ObjectParameter("ConsigmentDate", typeof(System.DateTime));

            var supplierNameParameter = supplierName != null ?
                new ObjectParameter("SupplierName", supplierName) :
                new ObjectParameter("SupplierName", typeof(string));

            var supplierSurnameParameter = supplierSurname != null ?
                new ObjectParameter("SupplierSurname", supplierSurname) :
                new ObjectParameter("SupplierSurname", typeof(string));

            var recipientNameParameter = recipientName != null ?
                new ObjectParameter("RecipientName", recipientName) :
                new ObjectParameter("RecipientName", typeof(string));

            var recipientSurnameParameter = recipientSurname != null ?
                new ObjectParameter("RecipientSurname", recipientSurname) :
                new ObjectParameter("RecipientSurname", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateConsignment", numberParameter, supplierIDParameter, recipientIDParameter, consigmentDateParameter, supplierNameParameter, supplierSurnameParameter, recipientNameParameter, recipientSurnameParameter, addedConsigmentNumber);
        }

        public virtual int UpdateOrder(Nullable<long> number, Nullable<long> productID, Nullable<long> productIDUpdated, Nullable<double> amount)
        {
            var numberParameter = number.HasValue ?
                new ObjectParameter("Number", number) :
                new ObjectParameter("Number", typeof(long));

            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(long));

            var productIDUpdatedParameter = productIDUpdated.HasValue ?
                new ObjectParameter("ProductIDUpdated", productIDUpdated) :
                new ObjectParameter("ProductIDUpdated", typeof(long));

            var amountParameter = amount.HasValue ?
                new ObjectParameter("Amount", amount) :
                new ObjectParameter("Amount", typeof(double));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateOrder", numberParameter, productIDParameter, productIDUpdatedParameter, amountParameter);
        }
    }
}
