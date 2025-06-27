using Client.Dtos;
using System.ComponentModel;

namespace Client.Utilities
{
    public interface IDocumentItemSorter
    {
        IEnumerable<DocumentItemDto> Sort(IEnumerable<DocumentItemDto> items, string sortColumn, ListSortDirection direction);
    }
}
