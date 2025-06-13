namespace WarehouseApplication.Models
{
    public class Document
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public required string Symbol { get; set; }

        public int ContractorId { get; set; }
        public required Contractor Contractor { get; set; }

        public List<DocumentItem> Items { get; set; } = new();
    }
}
