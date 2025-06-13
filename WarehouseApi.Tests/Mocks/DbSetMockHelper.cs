using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;
using System.Threading;

namespace WarehouseApi.Tests.Mocks
{
    // Async enumerable implementation for testing async EF Core queries
    public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        // Returns an async enumerator to support EF Core async iteration
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

        // IQueryable provider required by EF Core for async query translation
        IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
    }

    // Async enumerator wrapping regular IEnumerator<T> to provide async iteration semantics
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        // Current element in iteration
        public T Current => _inner.Current;

        // Dispose enumerator asynchronously (but just disposes synchronously here)
        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return ValueTask.CompletedTask;
        }

        // Move to next element asynchronously
        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_inner.MoveNext());
        }
    }

    // Async query provider to support async LINQ methods such as FirstOrDefaultAsync, ToListAsync, etc.
    public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        public TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
            => new TestAsyncEnumerable<TEntity>(expression);

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            => new TestAsyncEnumerable<TElement>(expression);

        public object Execute(Expression expression)
            => _inner.Execute(expression);

        public TResult Execute<TResult>(Expression expression)
            => _inner.Execute<TResult>(expression);

        // Executes async LINQ expressions, EF Core expects this for async queries
        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            var result = _inner.Execute(expression);

            if (typeof(TResult).IsGenericType && typeof(TResult).GetGenericTypeDefinition() == typeof(Task<>))
            {
                var resultType = typeof(TResult).GetGenericArguments()[0];
                var taskResult = typeof(Task).GetMethod(nameof(Task.FromResult))!
                    .MakeGenericMethod(resultType)
                    .Invoke(null, new[] { result });

                return (TResult)taskResult!;
            }

            return (TResult)result!;
        }
    }

    public static class DbSetMockHelper
    {
        // Creates a mock DbSet<T> that supports async operations and LINQ queries
        public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();

            var mockSet = new Mock<DbSet<T>>();

            // Setup async enumerator for EF Core async iteration (ToListAsync, FirstOrDefaultAsync, etc)
            mockSet.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(() => new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

            // Setup the IQueryable provider to use the async query provider to support async LINQ operators
            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<T>(queryable.Provider));

            // Setup IQueryable expression tree
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            // Setup Add method to add items to the underlying list
            mockSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(data.Add);

            // Setup FindAsync to find entity by primary key (assumed property name "Id")
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .Returns((object[] ids) =>
                {
                    var id = ids[0];
                    var prop = typeof(T).GetProperty("Id");
                    var entity = data.FirstOrDefault(d => prop?.GetValue(d)?.Equals(id) == true);
                    return new ValueTask<T>(entity);
                });

            return mockSet;
        }
    }
}
