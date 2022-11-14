﻿namespace VeryGoodNewsPortal.Models
{
    public class ArticleEditViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
