using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TextbookExchangeApp.Models
{
    public class reply
    {
        [Key]
        public int replyID {  get; set; }
        [Required]
        public string accountID { get; set; } = string.Empty;
        [Required]
        public string replyMsg { get; set; } = string.Empty;
        [Required]
        public TextbookListing? listing { get; set; }
        [Required]
        public string listingID { get; set; }
    }
}
