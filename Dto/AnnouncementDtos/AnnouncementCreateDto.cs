﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.AnnouncementDtos
{
    public class AnnouncementCreateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}