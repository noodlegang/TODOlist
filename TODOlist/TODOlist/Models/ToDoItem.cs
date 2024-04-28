using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace TODOlist.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Enter name")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage ="Select status")]
        public string StatusId { get; set; } = string.Empty;
        [ValidateNever]
        public Status Status { get; set; } = null!;
    }
}
