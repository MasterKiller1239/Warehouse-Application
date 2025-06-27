using Client.Dtos;

namespace Client.Services.Interfaces.IFactories
{
    public interface IEditDocumentViewModelFactory
    {
        EditDocumentViewModel Create(DocumentDto document);
    }
}
