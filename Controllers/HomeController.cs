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
            _db.TodoItem.Add(todo);
            _db.SaveChanges();
            return Redirect($"https://localhost:7249/?page={page}");
        }
        
        //UPDATE
        //TODO: Naprawić PopulateForm
        [HttpGet]
        public async Task<JsonResult> PopulateForm(int id)
        {
            var todoItem = await _homeService.GetTodoById(id);
            return Json(todoItem);
        }

        public RedirectResult Update(TodoItem todo)
        {
            var existingTodo = _db.TodoItem.FirstOrDefault(t => t.Id == todo.Id);
            if (existingTodo != null)
            {
                existingTodo.Name = todo.Name;
                _db.SaveChanges();
            }
            else
            {
                // handle error
            }

            return Redirect("https://localhost:7249/");
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