using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Client.Dtos
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int ContractorId { get; set; }
        public ContractorDto? Contractor { get; set; }

        // List of document items (products, quantities, etc.)
        public List<DocumentItemDto> Items { get; set; } = new();
    }
}