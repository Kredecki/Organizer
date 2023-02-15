using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer.Models;

namespace Organizer.Data.Repositories
{
    public interface IHomeRepository
    {
        Task<List<TodoItem>> GetAllTodosList(int page, int itemsOnPage);
        Task<int> CountAllTodos();
        Task<string> GetTodoById(int id);
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

        public async Task<string> GetTodoById(int id)
        {
            var result = await _db.TodoItem.FindAsync(id);
            return result.Name.ToString();
        }
    }
}
