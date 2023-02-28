using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Organizer.Controllers;
using Organizer.Models;
using Organizer.Services;

namespace Organizer.UnitTests
{
    class HomeTests
    {
        private Mock<IHomeService>? _homeServiceMock;
        private Mock<ILogger<HomeController>>? _loggerMock;
        private HomeController _controller;

        [SetUp]
        public void Setup()
        {
            _homeServiceMock = new Mock<IHomeService>();
            _loggerMock = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_loggerMock.Object, _homeServiceMock.Object);
        }

        [Test]
        public async Task Index_ReturnsViewResult_WhenCalled()
        {
            // Arrange
            _homeServiceMock?.Setup(x => x.CountAllTodos("")).ReturnsAsync(10);
            _homeServiceMock?.Setup(x => x.GetAllPages(10, 5)).Returns(2);
            _homeServiceMock?.Setup(x => x.PageService(1, 10, 5)).Returns(1);
            _homeServiceMock?.Setup(x => x.GetAllTodosList(1, 5, ""))
                       .ReturnsAsync(new List<TodoItem>
                       {
                   new TodoItem {Id = 1, Name = "Zrobić zakupy"},
                   new TodoItem {Id = 2, Name = "Zrobić obiad"},
                   new TodoItem {Id = 3, Name = "Napisać testy"}
                       });

            // Act
            var result = await _controller.Index(1, "");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        // SELECT

        [Test]
        public async Task GetAllTodos_ReturnsCorrectViewModel()
        {
            // Arrange
            _homeServiceMock?.Setup(x => x.CountAllTodos("")).ReturnsAsync(10);
            _homeServiceMock?.Setup(x => x.GetAllPages(10, 5)).Returns(2);
            _homeServiceMock?.Setup(x => x.PageService(1, 10, 5)).Returns(1);
            _homeServiceMock?.Setup(x => x.GetAllTodosList(1, 5, ""))
                       .ReturnsAsync(new List<TodoItem>
                       {
                   new TodoItem {Id = 1, Name = "Zrobić zakupy"},
                   new TodoItem {Id = 2, Name = "Zrobić obiad"},
                   new TodoItem {Id = 3, Name = "Napisać testy"}
                       });

            // Act
            var result = await _controller.GetAllTodos(1, "");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.PageNumber);
            Assert.AreEqual(2, result.PageCount);
            Assert.IsNotNull(result.TodoList);
            Assert.AreEqual(3, result?.TodoList?.Count);
            Assert.AreEqual("Zrobić zakupy", result?.TodoList?[0].Name);
            Assert.AreEqual("Zrobić obiad", result?.TodoList?[1].Name);
            Assert.AreEqual("Napisać testy", result?.TodoList?[2].Name);
        }

        // INSERT

        [Test]
        public async Task Insert_ReturnsRedirectToActionResult_WhenAddTodoItemSucceeds()
        {
            // Arrange
            var todoItem = new TodoItem();
            var page = 1;

            // Act
            var result = await _controller.Insert(todoItem, page, "") as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result?.ActionName);
            Assert.AreEqual(page, result?.RouteValues?["page"]);
        }

        [Test]
        public async Task Insert_WhenCalledWithValidTodoAndPage_CallsAddTodoItemMethodOfHomeService()
        {
            // Arrange
            var todo = new TodoItem();
            var page = 1;

            // Act
            await _controller.Insert(todo, page, "");

            // Assert
            _homeServiceMock?.Verify(x => x.AddTodoItem(todo), Times.Once);
        }

        // UPDATE

        [Test]
        public async Task PopulateForm_WhenCalledWithValidId_ReturnsJsonResult()
        {
            // Arrange
            var id = 1;
            var todoItem = new TodoItem { Id = id };

            _homeServiceMock?.Setup(x => x.GetTodoById(id)).ReturnsAsync(todoItem);

            // Act
            var result = await _controller.PopulateForm(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(todoItem, result.Value);
        }

        [Test]
        public async Task Update_WhenCalledWithValidTodoAndPage_ReturnsRedirectToActionResult()
        {
            // Arrange
            var todo = new TodoItem { Id = 1, Name = "Test Todo" };
            var pageNumber = 1;

            _homeServiceMock?.Setup(x => x.GetTodoById(todo.Id)).ReturnsAsync(todo);
            _homeServiceMock?.Setup(x => x.UpdateTodoItem(todo)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(todo, pageNumber, "") as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result?.ActionName);
            Assert.AreEqual(pageNumber, result?.RouteValues?["page"]);
        }

        [Test]
        public async Task Update_WhenTodoItemNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var todo = new TodoItem { Id = 1, Name = "Test Todo" };
            var pageNumber = 1;

            _homeServiceMock?.Setup(x => x.GetTodoById(todo.Id)).ReturnsAsync(() => null);

            // Act
            var result = await _controller.Update(todo, pageNumber, "") as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Update_WhenExceptionThrown_ReturnsErrorView()
        {
            // Arrange
            var todo = new TodoItem { Id = 1, Name = "Test Todo" };
            var pageNumber = 1;

            _homeServiceMock?.Setup(x => x.GetTodoById(todo.Id)).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Update(todo, pageNumber, "") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Error", result?.ViewName);
        }

        // DELETE

        [Test]
        public async Task Delete_WhenCalledWithValidId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var todo = new TodoItem { Id = 1, Name = "Test Todo" };

            _homeServiceMock?.Setup(x => x.GetTodoById(todo.Id)).ReturnsAsync(todo);
            _homeServiceMock?.Setup(x => x.DeleteTodoItem(todo)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(todo.Id) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result?.ActionName);
        }

        [Test]
        public async Task Delete_WhenTodoItemNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            TodoItem? todo = null;
            int id = 1;

            _homeServiceMock?.Setup(x => x.GetTodoById(id)).ReturnsAsync(todo);

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
