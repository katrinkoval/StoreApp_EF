
using System.ComponentModel.DataAnnotations;

namespace ModelsDAL
{
    public class Product
    {
        [Key]
        public long ID { get; set; }

        public string Name { get; set; }

        public byte UnitID { get; set; }

        public decimal Price { get; set; }

    }
}
