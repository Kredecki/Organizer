using Microsoft.AspNetCore.Mvc;
using Organizer.Models;
using Organizer.Models.ViewModels;
using Organizer.Data;
using Organizer.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Organizer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IHomeService _homeService;
        
        public HomeController(ILogger<HomeController> logger, IHomeService homeService)
        {
            _logger = logger;
            _homeService = homeService;
        }
        
        public async Task<IActionResult> Index(int page)
        {
            var todoListViewModel = await GetAllTodos(page);
            return View(todoListViewModel);
        }

        //SELECT
        internal async Task<TodoViewModel> GetAllTodos(int page)
        {
            int itemsOnPage = 5;

            int todoListCount = await _homeService.CountAllTodos();

            int pageNumber = _homeService.PageService(page, todoListCount, itemsOnPage);

            int pageCount = _homeService.GetAllPages(todoListCount, itemsOnPage);

            List<TodoItem> todoList = await _homeService.GetAllTodosList(page, itemsOnPage);

            TodoViewModel model = new TodoViewModel
            {
                TodoList = todoList,
                PageNumber = pageNumber,
                PageCount = pageCount
            };

            return model;
        }

        //INSERT
        public async Task<IActionResult> Insert(TodoItem todo, int page)
        {
            try
            {
                await _homeService.AddTodoItem(todo);
                return RedirectToAction("Index", new { page });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to add todo item");
                return View("Error");
            }
        }
        
        //UPDATE
        [HttpGet]
        public async Task<JsonResult> PopulateForm(int id)
        {
            var todoItem = await _homeService.GetTodoById(id);
            return new JsonResult(todoItem);
        }
        
        [HttpPost]
        public async Task<IActionResult> Update(TodoItem todo, int PageNumber)
        {
            try
            {
                int page = PageNumber;
                
                var existingTodoTask = _homeService.GetTodoById(todo.Id);
                var existingTodo = await existingTodoTask;

                if (existingTodo == null)
                {
                    return NotFound();
                }
                
                existingTodo.Name = todo.Name;
                await _homeService.UpdateTodoItem(existingTodo);

                return RedirectToAction("Index", new { page });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to update todo item");
                return View("Error");
            }
        }

        //DELETE
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var existingTodoTask = _homeService.GetTodoById(id);
            var todo = await existingTodoTask;

            if (todo == null)
            {
                return NotFound();
            }

            await _homeService.DeleteTodoItem(todo);

            return RedirectToAction("Index", "Home");
        }
    }
}