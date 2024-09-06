﻿namespace TaskManager_WEB.Models
{
    public class AnnouncementDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
