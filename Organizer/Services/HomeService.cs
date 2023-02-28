using Organizer.Data.Repositories;
using Organizer.Models;
using System.Data.SqlClient;

namespace Organizer.Services
{
    public interface IHomeService
    {
        void HandleException(Exception ex, string errorMessage);
        Task<List<TodoItem>> GetAllTodosList(int page, int itemsOnPage, string searchString);
        Task<int> CountAllTodos(string searchString);
        int PageService(int page, int todoListCount, int itemsOnPage);
        int GetAllPages(int todoListCount, int itemsOnPage);
        Task<TodoItem?> GetTodoById(int id);
        Task AddTodoItem(TodoItem todo);
        Task UpdateTodoItem(TodoItem todo);
        Task DeleteTodoItem(TodoItem todo);
    }

    public class HomeService : IHomeService
    {
        private readonly IHomeRepository _homeRepository;

        public HomeService(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }

        public void HandleException(Exception ex, string errorMessage)
        {
            if (ex is InvalidOperationException || ex is SqlException)
            {
                throw new ArgumentNullException(errorMessage, ex);
            }
            else
            {
                throw new ArgumentNullException("An error occurred while performing the database operation", ex);
            }
        }

        public async Task<List<TodoItem>> GetAllTodosList(int page, int itemsOnPage, string searchString)
        {
            try
            {
                return await _homeRepository.GetAllTodosList(page, itemsOnPage, searchString);
            }
            catch (Exception ex)
            {
                HandleException(ex, "Failed to get all todo items from database");
                throw;
            }
        }

        public async Task<int> CountAllTodos(string searchString)
        {
            try
            {
                return await _homeRepository.CountAllTodos(searchString);
            }
            catch (Exception ex)
            {
                HandleException(ex, "Failed to count all todo items from database");
                throw;
            }
        }
        
        public int PageService(int page, int todoListCount, int itemsOnPage)
        {
            try
            {
                int pages = (int)Math.Ceiling((double)todoListCount / itemsOnPage);
                if (page <= 0) page = 1;
                if (page >= pages) page = pages;

                return page;
            }
            catch (Exception ex)
            {
                HandleException(ex, "Failed to calc todo items from database");
                throw;
            }
        }

        public int GetAllPages(int todoListCount, int itemsOnPage)
        {
            try
            {
                return (int)Math.Ceiling((double)todoListCount / itemsOnPage) + 1;
            }
            catch (Exception ex)
            {
                HandleException(ex, "Failed to count todo items from database");
                throw;
            }
        }
        
        public async Task<TodoItem?> GetTodoById(int id)
        {
            try
            {
                return await _homeRepository.GetTodoById(id);
            }
            catch (Exception ex)
            {
                HandleException(ex, "Failed to get todo item from database");
                throw;
            }
        }
        
        public async Task AddTodoItem(TodoItem todo)
        {
            try
            {
                await _homeRepository.AddTodoItem(todo);
            }
            catch (Exception ex)
            {
                HandleException(ex, "Failed to add todo item to database");
                throw;
            }
        }

        public async Task UpdateTodoItem(TodoItem todo)
        {
            try
            {
                await _homeRepository.UpdateTodoItem(todo);
            }
            catch (Exception ex)
            {
                HandleException(ex, "Failed to update todo item into database");
                throw;
            }
        }
        
        public async Task DeleteTodoItem(TodoItem todo)
        {
            try
            {
                await _homeRepository.DeleteTodoItem(todo);
            }
            catch (Exception ex)
            {
                HandleException(ex, "Failed to delete todo item from database");
                throw;
            }
        }
    }
}
