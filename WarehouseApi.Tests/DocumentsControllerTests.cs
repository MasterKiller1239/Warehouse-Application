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
    }
}
