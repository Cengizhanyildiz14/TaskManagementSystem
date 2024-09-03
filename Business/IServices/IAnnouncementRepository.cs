using Business.Repository;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IAnnouncementRepository : IRepository<Announcement>
    {
        Announcement UpdateAnnouncement(Announcement announcement);
    }
}
