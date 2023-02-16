﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer.Models;

namespace Organizer.Data.Repositories
{
    public interface IHomeRepository
    {
        Task<List<TodoItem>> GetAllTodosList(int page, int itemsOnPage);
        Task<int> CountAllTodos();
        Task<TodoItem> GetTodoById(int id);
        bool AddTodoItem(TodoItem todo);
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
            var result = await _db.TodoItem.FindAsync(id);
            return result;
        }

        public bool AddTodoItem(TodoItem todo)
        {
            bool result = false;
            
            try
            {
                _db.TodoItem.Add(todo);
                _db.SaveChanges();
                result = true;
            } 
            catch(Exception ex) { }

            return result;
        }
    }
}
