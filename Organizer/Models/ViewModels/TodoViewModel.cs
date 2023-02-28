using System.Collections.Generic;

namespace Organizer.Models.ViewModels
{
    public class TodoViewModel
    {
        public List<TodoItem>? TodoList { get; set; }
        public TodoItem? Todo { get; set; }
        public int? PageNumber { get; set; }
        public int? PageCount { get; set; }
        public string? SearchString { get; set; }
        public List<Project>? Projects { get; set; }
        public int itemsOnPage { get; set; }
    }
}