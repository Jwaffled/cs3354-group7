using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace TextbookExchangeApp.Models
{
    public class TextbookListing
    {
        [Key] 
        public string listingID { get; set; }

        [Required]
        public string title { get; set; } = string.Empty;

        [Required]
        public string author { get; set; } = string.Empty;

        [Required]
        public string edition { get; set; } = string.Empty;

        [Required]
        public double price { get; set; }

        [Required]
        public string condition { get; set; }

        [Required]
        public int year { get; set; }

        [Required]
        public string isbn { get; set; }

        public List<reply>? replyList { get; set; } = new List<reply>();

    }
}
