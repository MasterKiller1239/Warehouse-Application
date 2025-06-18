using Client.Dtos;

namespace Client.ViewModels.Contractors
{
    public interface IContractorResultProvider
    {
        ContractorDto? NewContractor { get; }
    }

}
