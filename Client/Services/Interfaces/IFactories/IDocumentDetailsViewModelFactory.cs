using Client.Dtos;
using Client.Views.DocumentDetails;

namespace Client.Services.Interfaces.IFactories
{
    public interface IDocumentDetailsViewModelFactory
    {
        DocumentDetailsViewModel Create(DocumentDto document, Action? onDocumentUpdated = null);
    }
}
