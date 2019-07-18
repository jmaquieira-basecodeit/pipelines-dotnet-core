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

        [Fact]
        public async Task GetTodoItemNotFound()
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
            var result = await controller.Get(null);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetTodoItemSuccess()
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
            var result = await controller.Get(1050);

            // Assert
            Assert.IsType<ActionResult<TodoItem>>(result);
        }

        [Fact]
        public async Task GetTodoItem666NotFound()
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
            var result = await controller.Get(666);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
