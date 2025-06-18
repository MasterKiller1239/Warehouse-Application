using Moq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using WarehouseApplication.Controllers;
using WarehouseApplication.Dtos;
using WarehouseApplication.Services.Interfaces;
using Xunit;

namespace WarehouseApplication.Tests.Controllers
{
    public class DocumentsControllerTests
    {
        private readonly Mock<IDocumentService> _documentServiceMock;
        private readonly DocumentsController _controller;

        public DocumentsControllerTests()
        {
            _documentServiceMock = new Mock<IDocumentService>();

            _controller = new DocumentsController(_documentServiceMock.Object);
        }

        [Fact]
        public async Task Get_ReturnsAllDocuments()
        {
            // Arrange
            var docs = new List<DocumentDto>
            {
                new DocumentDto { Id = 1, Symbol = "D1", Items = new List<DocumentItemDto>() },
                new DocumentDto { Id = 2, Symbol = "D2", Items = new List<DocumentItemDto>() }
            };

            _documentServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(docs);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsAssignableFrom<IEnumerable<DocumentDto>>(okResult.Value);
            returned.Count().Should().Be(2);
        }

        [Fact]
        public async Task Get_WithValidId_ReturnsCorrectDocument()
        {
            // Arrange
            var doc = new DocumentDto
            {
                Id = 1,
                Symbol = "D1",
                Items = new List<DocumentItemDto>
                {
                    new DocumentItemDto { Id = 1, ProductName = "Item1", Quantity = 10, Unit = "test" },
                    new DocumentItemDto { Id = 2, ProductName = "Item2", Quantity = 20, Unit = "test" }
                }
            };

            _documentServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(doc);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<DocumentDto>(okResult.Value);
            returned.Id.Should().Be(1);
            returned.Items.Should().HaveCount(2);
        }

        [Fact]
        public async Task Get_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _documentServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((DocumentDto?)null);

            // Act
            var result = await _controller.Get(999); // non-existent ID

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Post_ValidDocument_ReturnsCreated()
        {
            // Arrange
            var newDoc = new DocumentDto { Id = 3, Symbol = "D3", ContractorId = 1 };
            _documentServiceMock.Setup(s => s.GetBySymbolAsync("D3")).ReturnsAsync((DocumentDto?)null);
            _documentServiceMock.Setup(s => s.CreateAsync(newDoc)).ReturnsAsync(newDoc);

            // Act
            var result = await _controller.Post(newDoc);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returned = Assert.IsType<DocumentDto>(createdResult.Value);
            returned.Symbol.Should().Be("D3");
        }

        [Fact]
        public async Task Post_DuplicateSymbol_ReturnsConflict()
        {
            // Arrange
            var newDoc = new DocumentDto { Id = 3, Symbol = "D1", ContractorId = 1 };
            _documentServiceMock.Setup(s => s.GetBySymbolAsync("D1")).ReturnsAsync(newDoc);

            // Act
            var result = await _controller.Post(newDoc);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            conflictResult.Value.Should().Be("A document with symbol 'D1' already exists.");
        }

        [Fact]
        public async Task Post_InvalidContractor_ReturnsBadRequest()
        {
            // Arrange
            var newDoc = new DocumentDto { Id = 3, Symbol = "D3", ContractorId = 0 };

            // Act
            var result = await _controller.Post(newDoc);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequest.Value.Should().Be("Contractor ID must be provided");
        }

        [Fact]
        public async Task Post_ThrowsArgumentException_ReturnsBadRequest()
        {
            // Arrange
            var newDoc = new DocumentDto { Id = 3, Symbol = "D3", ContractorId = 1 };
            _documentServiceMock.Setup(s => s.GetBySymbolAsync("D3")).ReturnsAsync((DocumentDto?)null);
            _documentServiceMock.Setup(s => s.CreateAsync(newDoc)).ThrowsAsync(new ArgumentException("Invalid"));

            // Act
            var result = await _controller.Post(newDoc);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequest.Value.Should().Be("Invalid");
        }

        [Fact]
        public async Task Post_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var newDoc = new DocumentDto { Id = 3, Symbol = "D3", ContractorId = 1 };
            _documentServiceMock.Setup(s => s.GetBySymbolAsync("D3")).ReturnsAsync((DocumentDto?)null);
            _documentServiceMock.Setup(s => s.CreateAsync(newDoc)).ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.Post(newDoc);

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result.Result);
            errorResult.StatusCode.Should().Be(500);
            errorResult.Value.Should().Be("Internal server error: Something went wrong");
        }

        [Fact]
        public async Task Put_ValidUpdate_ReturnsNoContent()
        {
            // Arrange
            var dto = new DocumentDto { Id = 1, Symbol = "D1" };
            _documentServiceMock.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(true);

            // Act
            var result = await _controller.Put(1, dto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Put_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            var dto = new DocumentDto { Id = 2, Symbol = "D1" };

            // Act
            var result = await _controller.Put(1, dto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Put_NotFound_ReturnsNotFound()
        {
            // Arrange
            var dto = new DocumentDto { Id = 1, Symbol = "D1" };
            _documentServiceMock.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(false);

            // Act
            var result = await _controller.Put(1, dto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetBySymbol_Valid_ReturnsOk()
        {
            // Arrange
            var doc = new DocumentDto { Id = 1, Symbol = "D1" };
            _documentServiceMock.Setup(s => s.GetBySymbolAsync("D1")).ReturnsAsync(doc);

            // Act
            var result = await _controller.GetBySymbol("D1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<DocumentDto>(okResult.Value);
            returned.Symbol.Should().Be("D1");
        }

        [Fact]
        public async Task GetBySymbol_NotFound_ReturnsNotFound()
        {
            // Arrange
            _documentServiceMock.Setup(s => s.GetBySymbolAsync("D999")).ReturnsAsync((DocumentDto?)null);

            // Act
            var result = await _controller.GetBySymbol("D999");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
