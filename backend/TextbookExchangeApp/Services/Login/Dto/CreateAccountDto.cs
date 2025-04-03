using System.ComponentModel.DataAnnotations;

namespace TextbookExchangeApp.Services.Login.Dto;

public class CreateAccountDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; }    
}