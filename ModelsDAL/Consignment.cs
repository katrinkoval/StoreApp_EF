using System;
using System.ComponentModel.DataAnnotations;


namespace ModelsDAL
{
    public class Consignment
    {
        [Key]
        public long Number { get; set; }

        public DateTime ConsignmentDate { get; set; }

        public long SupplierID { get; set; }

        public long RecipientID { get; set; }

    }
}
