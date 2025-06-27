using Client.Dtos;
using Client.ViewModels.DocumentItems;

namespace Client.Services.Interfaces.IFactories
{
    public interface IEditDocumentItemViewModelFactory
    {
        EditDocumentItemViewModel Create(DocumentItemDto item);
    }
}
