using Client.Dtos;
using System.ComponentModel;

public class DocumentItemViewModel : INotifyPropertyChanged
{
    public string ProductName { get; }
    public string Unit { get; }
    public int Quantity { get; }

    public DocumentItemViewModel(DocumentItemDto item)
    {
        ProductName = item.ProductName;
        Unit = item.Unit;
        Quantity = (int)item.Quantity;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}