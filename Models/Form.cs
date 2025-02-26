using System.ComponentModel.DataAnnotations;

namespace DynamicForm.Models
{
    public class Form
    {
        [Key]
        public int Id { get; set; }

        public string? FormKey { get; set; } 

        public string? FormSchema { get; set; }
    }
}
