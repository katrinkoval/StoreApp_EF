using System.ComponentModel.DataAnnotations;

namespace ModelsDAL
{
    public class Unit
    {
        [Key]
        public byte ID { get; set; }

        public string UnitType { get; set; }

    }
}
