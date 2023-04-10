namespace VeryGoodNewsPortal.Core.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}