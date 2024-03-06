using Microsoft.SqlServer.Server;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpanceTracker.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        // category id
        [Range(1, int.MaxValue, ErrorMessage ="Please Select a Category!!")]
        public int CategoryId { get; set; }
        public Category? Category  { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Amount Should be More Than '0' !")]

        public int Amount { get; set; }
        [Column(TypeName = "nvarchar(75)")]
        public String? Note{ get; set; }

        public DateTime Date { get; set; } = DateTime.Now;


        [NotMapped]
        public string? CategoryTitleWithIcon
        {
            get
            {
              return Category == null? "": Category.Icon + "" + Category.Title;
                    

            }
        }

        [NotMapped]
        public string? FormatedAmount
        {
            get
            {
                return ((Category == null || Category.Type == "Expence") ? "-" : "+") + Amount.ToString("c0", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            }

        }

    }
}
