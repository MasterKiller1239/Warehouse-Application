namespace WarehouseApplication.Dtos
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Symbol { get; set; }
        public int ContractorId { get; set; }
        public List<DocumentItemDto> Items { get; set; } = new();
    }

}
