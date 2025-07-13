namespace WarehouseApplication.Dtos
{
    public class DocumentItemDto
    {
        public int? Id { get; set; }
        public required string ProductName { get; set; }
        public required string Unit { get; set; }
        public decimal Quantity { get; set; }
        public int DocumentId { get; set; }
    }
}
