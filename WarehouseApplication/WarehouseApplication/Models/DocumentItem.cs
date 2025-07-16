using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseApplication.Models
{
    [Table("document_items")]
    public class DocumentItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string ProductName { get; set; }
        public required string Unit { get; set; }
        public decimal Quantity { get; set; }

        public int DocumentId { get; set; }
        public required Document Document { get; set; }
    }
}
