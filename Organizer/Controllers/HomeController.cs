using Microsoft.AspNetCore.Mvc;
using Organizer.Models;
using Organizer.Models.ViewModels;
using Organizer.Services;

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
        
        public async Task<IActionResult> Index(int page, string searchString)
        {
            var todoListViewModel = await GetTodos(page, searchString);
            return View(todoListViewModel);
        }

        // SELECT
        public async Task<TodoViewModel> GetTodos(int page, string searchString)
        {
            if (searchString == null) searchString = "";
            int itemsOnPage = 5;

            int todoListCount = await _homeService.CountTodos(searchString);

            int pageNumber = _homeService.PageService(page, todoListCount, itemsOnPage);

            int pageCount = _homeService.GetAllPages(todoListCount, itemsOnPage);

            List<TodoItem> todoList = await _homeService.GetTodosList(page, itemsOnPage, searchString);

            List<Project> projects = await _homeService.GetProjects();

            TodoViewModel model = new TodoViewModel
            {
                TodoList = todoList,
                PageNumber = pageNumber,
                PageCount = pageCount,
                SearchString = searchString,
                Projects = projects
            };
            
            return model;
        }

        // INSERT
        public async Task<IActionResult> Insert(TodoItem todo, int page, string searchString, int ProjectType)
        {
            await _homeService.AddTodoItem(todo, ProjectType);
            return RedirectToAction("Index", new { page, searchString });
        }
        
        // UPDATE
        [HttpGet]
        public async Task<JsonResult> PopulateForm(int id)
        {
            var todoItem = await _homeService.GetTodoById(id);
            return new JsonResult(todoItem);
        }
        
        [HttpPost]
        public async Task<IActionResult> Update(TodoItem todo, int PageNumber, string searchString, int ProjectType)
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
                await _homeService.UpdateTodoItem(existingTodo, ProjectType);

                return RedirectToAction("Index", new { page, searchString });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to update todo item");
                return View("Error");
            }
        }

        // DELETE
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