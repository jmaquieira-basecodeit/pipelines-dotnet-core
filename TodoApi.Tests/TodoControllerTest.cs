using System;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Controllers;
using TodoApi.Models;
using System.Linq;
using Moq;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace TodoApi.Tests
{
    public class TodoControllerTest
    {
        private readonly IQueryable<TodoItem> _todoItems;

        public TodoControllerTest()
        {
            _todoItems = new List<TodoItem> {
                new TodoItem {
                    Id = 1050,
                    Name = "Chemistry",
                    IsComplete = false
                },
                new TodoItem {
                    Id = 4022,
                    Name = "Microeconomics",
                    IsComplete = false
                }
            }.AsQueryable();
        }

        [Fact]
        public async Task GetTodoItemsSuccess()
        {
            // Arrange
            var mockTodoItems = new Mock<DbSet<TodoItem>>();
            mockTodoItems.As<IAsyncEnumerable<TodoItem>>().Setup(m => m.GetEnumerator()).Returns(new MockAsyncEnumerator<TodoItem>(_todoItems.GetEnumerator()));
            mockTodoItems.As<IQueryable<TodoItem>>().Setup(m => m.Provider).Returns(new MockAsyncQueryProvider<TodoItem>(_todoItems.Provider));
            mockTodoItems.As<IQueryable<TodoItem>>().Setup(m => m.Expression).Returns(_todoItems.Expression);
            mockTodoItems.As<IQueryable<TodoItem>>().Setup(m => m.ElementType).Returns(_todoItems.ElementType);
            mockTodoItems.As<IQueryable<TodoItem>>().Setup(m => m.GetEnumerator()).Returns(_todoItems.GetEnumerator());

            var mockTodoContext = new Mock<TodoContext>();
            mockTodoContext.Setup(todoContext => todoContext.TodoItems).Returns(mockTodoItems.Object);
            var controller = new TodoController(mockTodoContext.Object);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<TodoItem>>>(result);
        }
    }

    public class MockAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public MockAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public MockAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IAsyncEnumerator<T> GetEnumerator()
        {
            return new MockAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new MockAsyncQueryProvider<T>(this); }
        }
    }

    public class MockAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public MockAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public T Current
        {
            get
            {
                return _inner.Current;
            }
        }

        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }
    }

    public class MockAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        public MockAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new MockAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new MockAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return new MockAsyncEnumerable<TResult>(expression);
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }
}
