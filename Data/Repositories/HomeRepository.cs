using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer.Models;
using Organizer.Services;
using System.Data.SqlClient;

namespace Organizer.Data.Repositories
{
    public interface IHomeRepository
    {
        Task<List<TodoItem>> GetAllTodosList(int page, int itemsOnPage);
        Task<int> CountAllTodos();
        Task<TodoItem?> GetTodoById(int id);
        Task AddTodoItem(TodoItem todo);
        Task UpdateTodoItem(TodoItem todo);
        Task DeleteTodoItem(TodoItem todo);
    }

    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHomeService _homeService;

        public HomeRepository(ApplicationDbContext db, IHomeService homeService)
        {
            _db = db;
            _homeService = homeService;
        }

        public async Task<List<TodoItem>> GetAllTodosList(int page, int itemsOnPage)
        {
            try 
            { 
                return await _db.TodoItem.Skip((page - 1) * itemsOnPage).Take(itemsOnPage).ToListAsync();
            }
            catch (Exception ex)
            {
                _homeService.HandleException(ex, "Failed to get all todo items from database");
                throw;
            }
        }
        
        public async Task<int> CountAllTodos()
        {
            try
            {
                return await _db.TodoItem.CountAsync();
            }
            catch (Exception ex)
            {
                _homeService.HandleException(ex, "Failed to count all todo items from database");
                throw;
            }
        }

        public async Task<TodoItem?> GetTodoById(int id)
        {
            try
            {
                return await _db.TodoItem.FindAsync(id);
            }
            catch (Exception ex)
            {
                _homeService.HandleException(ex, "Failed to get todo items from database");
                throw;
            }
        }

        public async Task AddTodoItem(TodoItem todo)
        {
            try
            {
                await _db.TodoItem.AddAsync(todo);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _homeService.HandleException(ex, "Failed to add todo item to database");
                throw;
            }
        }

        public async Task UpdateTodoItem(TodoItem todo)
        {
            try
            {
                _db.TodoItem.Update(todo);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _homeService.HandleException(ex, "Failed to update todo item into database");
                throw;
            }
        }

        public async Task DeleteTodoItem(TodoItem todo)
        {
            try
            {
                _db.TodoItem.Remove(todo);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _homeService.HandleException(ex, "Failed to delete todo item from database");
                throw;
            }
        }
    }
}
