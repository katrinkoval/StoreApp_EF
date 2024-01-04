using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Linq.Mapping;

namespace ModelsDAL
{
    public class Order
    {
        [ForeignKey("ConsignmentNumber")]
        public virtual Consignment Consignment { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

       
        [Key, System.ComponentModel.DataAnnotations.Schema.Column(Order = 1)]
        public long ConsignmentNumber { get; set; }

        [Key, System.ComponentModel.DataAnnotations.Schema.Column(Order = 2)]
        public long ProductID { get; set; }

        public double Amount { get; set; }

    }
}