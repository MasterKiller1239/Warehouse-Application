using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoMapper;
using WarehouseApplication.Controllers;
using WarehouseApplication.Data.Interfaces;
using WarehouseApplication.Dtos;
using WarehouseApplication.Models;
using WarehouseApi.Tests.Mocks;
using FluentAssertions;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseApplication.Tests.Controllers
{
    public class DocumentItemsControllerTests
    {
        private readonly Mock<IWarehouseContext> _contextMock;
        private readonly IMapper _mapper;
        private readonly List<DocumentItem> _items;

        public DocumentItemsControllerTests()
        {
            _contextMock = new Mock<IWarehouseContext>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DocumentItem, DocumentItemDto>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            var mockDocument = new Document { Id = 1, Symbol = "D1", Contractor = new Contractor { Id = 1, Symbol = "C1", Name = "Contractor One" } };

            _items = new List<DocumentItem>
            {
                new DocumentItem { Id = 1, ProductName = "Item1", Unit = "kg", Quantity = 10, DocumentId = 1, Document = mockDocument },
                new DocumentItem { Id = 2, ProductName = "Item2", Unit = "kg", Quantity = 20, DocumentId = 1, Document = mockDocument }
            };

            var mockDbSet = DbSetMockHelper.CreateMockDbSet(_items);
            _contextMock.Setup(c => c.DocumentItems).Returns(mockDbSet.Object);
        }

        [Fact]
        public async Task Get_ReturnsAllItems()
        {
            var controller = new DocumentItemsController(_contextMock.Object, _mapper);

            var result = await controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsAssignableFrom<IEnumerable<DocumentItemDto>>(okResult.Value);
            returned.Count().Should().Be(_items.Count);
        }

        [Fact]
        public async Task Get_WithId_ReturnsCorrectItem()
        {
            var controller = new DocumentItemsController(_contextMock.Object, _mapper);

            var result = await controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<DocumentItemDto>(okResult.Value);
            returned.Id.Should().Be(1);
        }

        [Fact]
        public async Task Get_WithInvalidId_ReturnsNotFound()
        {
            var controller = new DocumentItemsController(_contextMock.Object, _mapper);

            var result = await controller.Get(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task Post_AddsNewDocumentItem()
        {
            // Arrange
            var dto = new DocumentItemDto
            {
                Id = 3,
                ProductName = "Item3",
                Unit = "pcs",
                Quantity = 5,
                DocumentId = 1
            };

            var mockDbSet = DbSetMockHelper.CreateMockDbSet(_items);
            _contextMock.Setup(c => c.DocumentItems).Returns(mockDbSet.Object);
            _contextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var controller = new DocumentItemsController(_contextMock.Object, _mapper);

            // Act
            var result = await controller.Post(dto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            mockDbSet.Verify(d => d.Add(It.IsAny<DocumentItem>()), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Put_UpdatesExistingDocumentItem()
        {
            // Arrange
            var existing = _items.First();
            var dto = new DocumentItemDto
            {
                Id = existing.Id,
                ProductName = "UpdatedName",
                Unit = "kg",
                Quantity = 123,
                DocumentId = existing.DocumentId
            };

            _contextMock.Setup(c => c.DocumentItems.FindAsync(existing.Id))
                .ReturnsAsync(existing);
            _contextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var controller = new DocumentItemsController(_contextMock.Object, _mapper);

            // Act
            var result = await controller.Put((int)existing.Id, dto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            existing.ProductName.Should().Be("UpdatedName");
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var dto = new DocumentItemDto
            {
                Id = 5,
                ProductName = "UpdatedName",
                Unit = "kg",
                Quantity = 123,
                DocumentId = 5
            };
            var controller = new DocumentItemsController(_contextMock.Object, _mapper);

            // Act
            var result = await controller.Put(999, dto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Put_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            var dto = new DocumentItemDto { Id = 999, ProductName = "X", Unit = "m", Quantity = 1, DocumentId = 1 };

            _contextMock.Setup(c => c.DocumentItems.FindAsync(999))
                .ReturnsAsync((DocumentItem)null);

            var controller = new DocumentItemsController(_contextMock.Object, _mapper);

            // Act
            var result = await controller.Put(999, dto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetByDocumentId_ReturnsFilteredItems()
        {
            // Arrange
            var controller = new DocumentItemsController(_contextMock.Object, _mapper);

            var documentId = 1;
            var expectedCount = _items.Count(i => i.DocumentId == documentId);

            // Act
            var result = await controller.GetByDocumentId(documentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsAssignableFrom<IEnumerable<DocumentItemDto>>(okResult.Value);
            returned.Should().HaveCount(expectedCount);
        }

    }
}
