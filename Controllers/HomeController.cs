using Microsoft.AspNetCore.Mvc;
using Organizer.Models;
using Organizer.Models.ViewModels;
using Organizer.Data;
using Organizer.Services;

namespace Organizer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        private readonly IHomeService _homeService;
        
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IHomeService homeService)
        {
            _logger = logger;
            _db = db;
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
        public RedirectResult Insert(TodoItem todo, int page)
        {
            _homeService.AddTodoItem(todo);  
            return Redirect($"https://localhost:7249/?page={page}");
        }
        
        //UPDATE
        [HttpGet]
        public async Task<JsonResult> PopulateForm(int id)
        {
            var todoItem = await _homeService.GetTodoById(id);
            return new JsonResult(todoItem);
        }
        
        [HttpPost]
        public async Task<IActionResult> Update(TodoItem todo)
        {
            var existingTodoTask = _homeService.GetTodoById(todo.Id);
            var existingTodo = await existingTodoTask;

            if (existingTodo == null)
            {
                return NotFound();
            }

            existingTodo.Name = todo.Name;
            _homeService.UpdateTodoItem(existingTodo);

            return RedirectToAction("Index", "Home");
        }

        //DELETE
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var todoToDelete = _db.TodoItem.FirstOrDefault(t => t.Id == id);
            if (todoToDelete != null)
            {
                _db.TodoItem.Remove(todoToDelete);
                _db.SaveChanges();
            }
            else
            {
                // handle error
            }

            return Json(new { });
        }
    }
}