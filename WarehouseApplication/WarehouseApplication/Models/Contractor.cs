using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseApplication.Models
{
    [Table("contractors")]
    public class Contractor
    {
        public int? Id { get; set; }
        public required string Symbol { get; set; }
        public required string Name { get; set; }
    }

}
