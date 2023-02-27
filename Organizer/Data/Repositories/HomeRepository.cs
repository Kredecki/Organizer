﻿using Microsoft.AspNetCore.Mvc;
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

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<TodoItem>> GetAllTodosList(int page, int itemsOnPage)
        {
            return await _db.TodoItem.Skip((page - 1) * itemsOnPage).Take(itemsOnPage).ToListAsync();
        }
        
        public async Task<int> CountAllTodos()
        {
            return await _db.TodoItem.CountAsync();
        }

        public async Task<TodoItem?> GetTodoById(int id)
        {
            return await _db.TodoItem.FindAsync(id);
        }

        public async Task AddTodoItem(TodoItem todo)
        {
            await _db.TodoItem.AddAsync(todo);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateTodoItem(TodoItem todo)
        {
            _db.TodoItem.Update(todo);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteTodoItem(TodoItem todo)
        {
            _db.TodoItem.Remove(todo);
            await _db.SaveChangesAsync();
        }
    }
}