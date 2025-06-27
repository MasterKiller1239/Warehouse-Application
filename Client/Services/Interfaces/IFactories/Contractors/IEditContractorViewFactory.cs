using Client.Dtos;
using Client.Views.Contractors;

namespace Client.Services.Interfaces.IFactories.Contractors
{
    public interface IEditContractorViewFactory
    {
        EditContractorView Create(ContractorDto contractor);
    }
}
