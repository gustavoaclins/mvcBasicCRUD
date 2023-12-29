using System.ComponentModel.DataAnnotations;

namespace mvcBasicCRUD.Models
{
    public class ChoresViewModels
    {
        public int Id { get; set; }
        
        public string? Title { get; set; }

        public DateTime DueDate { get; set; }

        public string? ChoreType { get; set; }

        public string? Status { get; set; }
    }
}
