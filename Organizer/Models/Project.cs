﻿namespace Organizer.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<TodoItem> TodoItems { get; set; }
    }
}
