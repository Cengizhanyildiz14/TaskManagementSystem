using Business.IServices;
using Business.Repository;
using Data.Context;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AnnouncementRepository : Repository<Announcement>, IAnnouncementRepository
    {
        private readonly TaskManagerContext _taskManagerContext;

        public AnnouncementRepository(TaskManagerContext taskManagerContext) : base(taskManagerContext)
        {
            _taskManagerContext = taskManagerContext;
        }

        public Announcement UpdateAnnouncement(Announcement announcement)
        {
            _taskManagerContext.Announcements.Update(announcement);
            _taskManagerContext.SaveChanges();
            return announcement;
        }
    }
}
