using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseApplication.Controllers;
using WarehouseApplication.Dtos;
using WarehouseApplication.Data.Interfaces;
using WarehouseApplication.Services.Interfaces;

namespace WarehouseApplication.Tests.Controllers
{
    public class ContractorsControllerTests
    {
        private readonly Mock<IContractorService> _serviceMock;

        public ContractorsControllerTests()
        {
            _serviceMock = new Mock<IContractorService>();
        }

        [Fact]
        public async Task Get_ReturnsAllContractors()
        {
            // Arrange
            var contractors = new List<ContractorDto>
            {
                new ContractorDto { Id = 1, Symbol = "C1", Name = "Contractor One" },
                new ContractorDto { Id = 2, Symbol = "C2", Name = "Contractor Two" }
            };

            _serviceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(contractors);

            var controller = new ContractorsController(_serviceMock.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedContractors = Assert.IsAssignableFrom<IEnumerable<ContractorDto>>(okResult.Value);
            returnedContractors.Should().HaveCount(2);
        }

        [Fact]
        public async Task Post_AddsNewContractor()
        {
            // Arrange
            var newDto = new ContractorDto { Symbol = "C3", Name = "Contractor Three" };
            _serviceMock.Setup(s => s.CreateAsync(newDto))
                .ReturnsAsync(newDto);

            var controller = new ContractorsController(_serviceMock.Object);

            // Act
            var result = await controller.Post(newDto);

            // Assert
            var createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returned = Assert.IsType<ContractorDto>(createdAt.Value);
            Assert.Equal("C3", returned.Symbol);
        }
    }
}
