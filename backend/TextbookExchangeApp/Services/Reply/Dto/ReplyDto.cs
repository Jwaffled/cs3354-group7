namespace TextbookExchangeApp.Services.Reply.Dto;

public class ReplyDto
{
    public int Id {  get; set; }
    public string Message { get; set; }
    
    public int ListingId { get; set; }
    public string CreatedById { get; set; }

    public Models.Reply ConvertToModel()
    {
        return new Models.Reply
        {
            Id = Id,
            Message = Message,
            ListingId = ListingId,
            CreatedById = CreatedById,
        };
    }
}