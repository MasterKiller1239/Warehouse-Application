namespace Client.Dtos
{
    public class DocumentItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public int DocumentId { get; set; }
    }
}
