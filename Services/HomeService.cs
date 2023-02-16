using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Organizer.Data.Repositories;
using Organizer.Models;
using Organizer.Models.ViewModels;

namespace Organizer.Services
{
    public interface IHomeService
    {
        Task<List<TodoItem>> GetAllTodosList(int page, int itemsOnPage);
        Task<int> CountAllTodos();
        int PageService(int page, int todoListCount, int itemsOnPage);
        int GetAllPages(int todoListCount, int itemsOnPage);
        Task<string> GetTodoById(int id);
        bool AddTodoItem(TodoItem todo);
    }

    public class HomeService : IHomeService
    {
        private readonly IHomeRepository _homeRepository;

        public HomeService(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }
        
        public async Task<List<TodoItem>> GetAllTodosList(int page, int itemsOnPage)
        {
            return await _homeRepository.GetAllTodosList(page, itemsOnPage);
        }

        public async Task<int> CountAllTodos()
        {
            return await _homeRepository.CountAllTodos();
        }

        public int PageService(int page, int todoListCount, int itemsOnPage)
        {       
            int pages = (int)Math.Ceiling((double)todoListCount / itemsOnPage);
            if (page <= 0) page = 1;
            if (page >= pages) page = pages;

            return page;
        }

        public int GetAllPages(int todoListCount, int itemsOnPage)
        {
            int pages = (int)Math.Ceiling((double)todoListCount / itemsOnPage);

            return pages+1;
        }

        public async Task<string> GetTodoById(int id)
        {
            var result = await _homeRepository.GetTodoById(id);
            return result.ToString();
        }
        
        public bool AddTodoItem(TodoItem todo)
        {
            return _homeRepository.AddTodoItem(todo);
        }
    }
}
