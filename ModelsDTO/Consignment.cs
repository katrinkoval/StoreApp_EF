using System;

namespace ModelsDTO
{
    public class Consignment
    {
        public long Number { get; set; }

        public DateTime ConsignmentDate { get; set; }

        public string SupplierName { get; set; }

        public string RecipientName { get; set; }

        public long SupplierIpn { get; set; }

        public long RecipientIpn { get; set; }
    }
}
