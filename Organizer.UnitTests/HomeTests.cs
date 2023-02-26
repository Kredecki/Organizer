using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Organizer.Controllers;
using Organizer.Models;
using Organizer.Models.ViewModels;
using Organizer.Services;

namespace Organizer.UnitTests
{
    class HomeTests
    {
        private Mock<IHomeService>? _homeServiceMock;
        private Mock<ILogger<HomeController>>? _loggerMock;
        private HomeController? _controller;

        [SetUp]
        public void Setup()
        {
            _homeServiceMock = new Mock<IHomeService>();
            _loggerMock = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_loggerMock.Object, _homeServiceMock.Object);
        }

        // SELECT

        [Test]
        public async Task GetAllTodos_ReturnsCorrectViewModel()
        {
            // Arrange
            _homeServiceMock?.Setup(x => x.CountAllTodos()).ReturnsAsync(10);
            _homeServiceMock?.Setup(x => x.GetAllPages(10, 5)).Returns(2);
            _homeServiceMock?.Setup(x => x.PageService(1, 10, 5)).Returns(1);
            _homeServiceMock?.Setup(x => x.GetAllTodosList(1, 5))
                       .ReturnsAsync(new List<TodoItem>
                       {
                   new TodoItem {Id = 1, Name = "Zrobić zakupy"},
                   new TodoItem {Id = 2, Name = "Zrobić obiad"},
                   new TodoItem {Id = 3, Name = "Napisać testy"}
                       });

            // Act
            var result = await _controller.GetAllTodos(1);

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
            var result = await _controller.Insert(todoItem, page) as RedirectToActionResult;

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
            await _controller.Insert(todo, page);

            // Assert
            _homeServiceMock?.Verify(x => x.AddTodoItem(todo), Times.Once);
        }
    }
}
