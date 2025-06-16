using Client.Dtos;
using Client.Services.Interfaces;
using Client.Views.Contractors;
using Moq;
namespace ClientTests
{
    public class AddContractorViewTests
    {
        //[WpfFact] // z pakietu xUnit.Wpf
        //public async Task AddContractor_WhenValidData_CallsApiClient()
        //{
        //    // Arrange
        //    var mockClient = new Mock<IApiClient>();
        //    var view = new AddContractorView(mockClient.Object);

        //    view.Show(); // potrzebne w testach WPF
        //    view.SymbolTextBox.Text = "ABC";
        //    view.NameTextBox.Text = "Firma XYZ";

        //    // Act
        //    view.AddContractor_Click(null, null);

        //    // Wait for async to complete
        //    await Task.Delay(100); // alternatywnie użyj synchronizacji

        //    // Assert
        //    mockClient.Verify(c => c.AddContractorAsync(It.Is<ContractorDto>(
        //        dto => dto.Symbol == "ABC" && dto.Name == "Firma XYZ"
        //    )), Times.Once);
        //}

        //[WpfFact]
        //public void AddContractor_WhenSymbolIsEmpty_ShowsWarning()
        //{
        //    var mockClient = new Mock<IApiClient>();
        //    var view = new AddContractorView(mockClient.Object);
        //    view.SymbolTextBox.Text = "";
        //    view.NameTextBox.Text = "Firma XYZ";

        //    var messageShown = false;

        //    // zamien MessageBox na atrapę, jeśli chcesz testować UI w 100%
        //    MessageBoxInterceptor.Run(() =>
        //    {
        //        view.AddContractor_Click(null, null);
        //    }, msg => messageShown = msg.Contains("Symbol"));

        //    Assert.True(messageShown);
        //    mockClient.Verify(c => c.AddContractorAsync(It.IsAny<ContractorDto>()), Times.Never);
        //}
    }
}
