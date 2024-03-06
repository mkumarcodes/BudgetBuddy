using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpanceTracker.Models
{
    public class Category
    {
        [Key]

        public int CategoryId { get; set; }

        [Column (TypeName = "nvarchar(50)")]
        [Required(ErrorMessage ="Title Is Required")]
        
        public String? Title  { get; set; }

        [Column(TypeName = "nvarchar(5)")]

        public String? Icon { get; set; } = "";
        [Column(TypeName = "nvarchar(10)")]
        public String? Type { get; set; } = "Expence";

        [NotMapped]
        public string? TitleWithIcon
        {
            get
            {
                return this.Icon +"  " +this.Title;

            }
        }


    }
}
