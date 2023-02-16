using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer.Models;

namespace Organizer.Data.Repositories
{
    public interface IHomeRepository
    {
        Task<List<TodoItem>> GetAllTodosList(int page, int itemsOnPage);
        Task<int> CountAllTodos();
        Task<TodoItem> GetTodoById(int id);
        void AddTodoItem(TodoItem todo);
        void UpdateTodoItem(TodoItem todo);
        void DeleteTodoItem(TodoItem todo);
    }

    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<TodoItem>> GetAllTodosList(int page, int itemsOnPage)
        {
            var todos = await _db.TodoItem
                .Skip((page - 1) * itemsOnPage)
                .Take(itemsOnPage)
                .ToListAsync();

            return todos;
        }
        
        public async Task<int> CountAllTodos()
        {
            var count = await _db.TodoItem.CountAsync();
            
            return count;
        }

        public async Task<TodoItem> GetTodoById(int id)
        {
            var result = await _db.TodoItem.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public void AddTodoItem(TodoItem todo)
        {
            _db.TodoItem.Add(todo);
            _db.SaveChanges();
        }

        public async void UpdateTodoItem(TodoItem todo)
        {
            _db.TodoItem.Update(todo);
            await _db.SaveChangesAsync();
        }

        public void DeleteTodoItem(TodoItem todo)
        {
            _db.TodoItem.Remove(todo);
            _db.SaveChanges();
        }
    }
}
