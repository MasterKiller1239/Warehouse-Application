using Microsoft.EntityFrameworkCore;
using Moq;

namespace WarehouseApi.Tests.Mocks
{
    public static class DbSetMockHelper
    {
        public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> elements) where T : class
        {
            var queryable = elements.AsQueryable();

            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            mockSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(elements.Add);

            return mockSet;
        }
    }
}
