namespace VeryGoodNewsPortal.WebApi.Models.Requests;

public class CreateCommentRequest
{
    public string Text { get; set; }
    public Guid UserId { get; set; }
    public Guid ArticleId { get; set; }
}