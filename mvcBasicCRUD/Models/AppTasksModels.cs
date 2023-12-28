using System.ComponentModel.DataAnnotations;

namespace mvcBasicCRUD.Models
{
    public class Chore
    {
        [Key]
        public int ChoreID { get; set; }
        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Required]
        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; }

        public virtual int? ChoreTypeID { get; set; }
        public virtual ChoreType? ChoreType { get; set; }

    }

    public class ChoreType
    {
        [Key]
        public int ChoreTypeID { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        public virtual ICollection<Task>? Chores { get; set; }
    }
}
