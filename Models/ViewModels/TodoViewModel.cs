using System.Collections.Generic;

namespace Organizer.Models.ViewModels
{
    public class TodoViewModel
    {
        public List<TodoItem>? TodoList { get; set; }
        public TodoItem? Todo { get; set; }
        public int? PageNumber { get; set; }
    }
}