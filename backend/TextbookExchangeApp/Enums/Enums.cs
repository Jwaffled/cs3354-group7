using System.ComponentModel.DataAnnotations;

namespace TextbookExchangeApp.Enums;

public enum TextbookCondition
{
    [Display(Name = "New")]
    New = 1,
    [Display(Name = "Like New")]
    LikeNew = 2,
    [Display(Name ="Slightly Used")]
    SlightlyUsed = 3,
    [Display(Name ="Heavily Used")]
    HeavilyUsed = 4,
    [Display(Name ="Damaged")]
    Damaged = 5
}