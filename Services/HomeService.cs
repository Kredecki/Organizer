﻿using Microsoft.AspNetCore.Mvc;
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
        Task<TodoItem> GetTodoById(int id);
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
        
        public async Task<List<TodoItem>> GetAllTodosList(int page, int itemsOnPage)
        {
            try
            {
                return await _homeRepository.GetAllTodosList(page, itemsOnPage);
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to get all todo items from database", ex);
            }
        }

        public async Task<int> CountAllTodos()
        {
            try
            {
                return await _homeRepository.CountAllTodos();
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to count all todo items from database", ex);
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
            catch(Exception ex)
            {
                throw new Exception("Failed to calc todo items from database", ex);
            }
        }

        public int GetAllPages(int todoListCount, int itemsOnPage)
        {
            try
            {
                return (int)Math.Ceiling((double)todoListCount / itemsOnPage) + 1;
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to count todo items from database", ex);
            }
        }

        public async Task<TodoItem> GetTodoById(int id)
        {
            try
            {
                return await _homeRepository.GetTodoById(id);
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to get todo item from database", ex);
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
                throw new Exception("Failed to add todo item to database", ex);
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
                throw new Exception("Failed to update todo item into database", ex);
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
                throw new Exception("Failed to delete todo item from database", ex);
            }
        }
    }
}
