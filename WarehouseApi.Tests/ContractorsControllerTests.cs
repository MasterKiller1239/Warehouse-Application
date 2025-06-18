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
        [Fact]
        public async Task Get_ById_ReturnsContractor_WhenExists()
        {
            // Arrange
            var contractor = new ContractorDto { Id = 1, Symbol = "C1", Name = "Contractor One" };
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(contractor);
            var controller = new ContractorsController(_serviceMock.Object);

            // Act
            var result = await controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<ContractorDto>(okResult.Value);
            Assert.Equal(1, returned.Id);
        }

        [Fact]
        public async Task Get_ById_ReturnsNotFound_WhenNotExists()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((ContractorDto)null);
            var controller = new ContractorsController(_serviceMock.Object);

            // Act
            var result = await controller.Get(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Put_UpdatesContractor_WhenValid()
        {
            // Arrange
            var dto = new ContractorDto { Id = 1, Symbol = "C1", Name = "Updated" };
            _serviceMock.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(true);
            var controller = new ContractorsController(_serviceMock.Object);

            // Act
            var result = await controller.Put(1, dto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var dto = new ContractorDto { Id = 1, Symbol = "C1", Name = "Updated" };
            _serviceMock.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(false);
            var controller = new ContractorsController(_serviceMock.Object);

            // Act
            var result = await controller.Put(1, dto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetBySymbol_ReturnsContractor_WhenFound()
        {
            // Arrange
            var contractor = new ContractorDto { Id = 1, Symbol = "ABC", Name = "Some Contractor" };
            _serviceMock.Setup(s => s.GetBySymbolAsync("ABC")).ReturnsAsync(contractor);
            var controller = new ContractorsController(_serviceMock.Object);

            // Act
            var result = await controller.GetBySymbol("ABC");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<ContractorDto>(okResult.Value);
            Assert.Equal("ABC", returned.Symbol);
        }

        [Fact]
        public async Task GetBySymbol_ReturnsNotFound_WhenMissing()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetBySymbolAsync("XXX")).ReturnsAsync((ContractorDto)null);
            var controller = new ContractorsController(_serviceMock.Object);

            // Act
            var result = await controller.GetBySymbol("XXX");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
