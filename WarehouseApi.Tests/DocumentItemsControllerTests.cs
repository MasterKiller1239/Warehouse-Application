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
    }
}
