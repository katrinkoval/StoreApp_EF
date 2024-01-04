
using System.ComponentModel.DataAnnotations;

namespace ModelsDAL
{
    public class Individual
    {
        [Key]
        public long IPN { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

    }
}
