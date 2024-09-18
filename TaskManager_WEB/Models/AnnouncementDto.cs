﻿namespace TaskManager_WEB.Models
{
    public class AnnouncementDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
