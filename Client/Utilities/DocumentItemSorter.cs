using Client.Dtos;
using Client.Utilities;
using System.ComponentModel;

public class DocumentItemSorter : IDocumentItemSorter
{
    public IEnumerable<DocumentItemDto> Sort(IEnumerable<DocumentItemDto> items, string sortColumn, ListSortDirection direction)
    {
        return (sortColumn, direction) switch
        {
            ("ProductName", ListSortDirection.Ascending) => items.OrderBy(i => i.ProductName),
            ("ProductName", ListSortDirection.Descending) => items.OrderByDescending(i => i.ProductName),
            ("Unit", ListSortDirection.Ascending) => items.OrderBy(i => i.Unit),
            ("Unit", ListSortDirection.Descending) => items.OrderByDescending(i => i.Unit),
            ("Quantity", ListSortDirection.Ascending) => items.OrderBy(i => i.Quantity),
            ("Quantity", ListSortDirection.Descending) => items.OrderByDescending(i => i.Quantity),
            _ => items
        };
    }

}
