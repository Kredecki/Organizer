using Microsoft.AspNetCore.Mvc;
using Organizer.Models;
using Microsoft.Data.Sqlite;
using Organizer.Models.ViewModels;
using Organizer.Data;

namespace Organizer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult Index()
        {
            var todoListViewModel = GetAllTodos();
            return View(todoListViewModel);
        }

        //SELECT
        internal TodoViewModel GetAllTodos()
        {
            List<TodoItem> todoList = _db.TodoItem.ToList();

            return new TodoViewModel
            {
                TodoList = todoList
            };
        }

        //INSERT
        public RedirectResult Insert(TodoItem todo)
        {
            _db.TodoItem.Add(todo);
            _db.SaveChanges();
            return Redirect("https://localhost:7249/");
        }

        //UPDATE
        [HttpGet]
        public JsonResult PopulateForm(int id)
        {
            var todo = _db.TodoItem.FirstOrDefault(t => t.Id == id);
            return Json(todo);
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