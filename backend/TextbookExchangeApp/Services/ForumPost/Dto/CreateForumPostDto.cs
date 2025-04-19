using System.ComponentModel.DataAnnotations;

namespace TextbookExchangeApp.Services.ForumPost.Dto;

public class CreateForumPostDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
}