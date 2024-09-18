using AutoMapper;
using Azure;
using Business;
using Business.IServices;
using Data.Entities;
using Dto.AnnouncementDtos;
using Dto.DepartmentDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager_API.Controllers
{
    [Route("api/Announcement")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;

        public AnnouncementController(IAnnouncementRepository announcementRepository, IMapper mapper)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
            this._response = new APIResponse();
        }

        [HttpGet("GetAllAnnouncements")]
        public ActionResult<APIResponse> GetAllAnnouncements()
        {
            try
            {
                IEnumerable<Announcement> announcementList = _announcementRepository.Getall();
                _response.Result = _mapper.Map<List<AnnouncementDto>>(announcementList);
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("GetAnnouncement/{id}")]
        public ActionResult<APIResponse> GetAnnouncement(Guid id)
        {
            try
            {
                if (id == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return _response;
                }
                var annoucnement = _announcementRepository.Get(d => d.Id == id);
                if (annoucnement == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return _response;
                }
                _response.Result = _mapper.Map<AnnouncementDto>(annoucnement);
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("DeleteAnnouncement/{id}")]
        public ActionResult<APIResponse> DeleteAnnouncement(Guid id)
        {
            try
            {
                if (id == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return _response;
                }
                var announcement = _announcementRepository.Get(d => d.Id == id);
                if (announcement == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return _response;
                }
                _announcementRepository.Delete(announcement);
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("PostAnnouncement")]
        public ActionResult<APIResponse> PostAnnouncement([FromBody] AnnouncementCreateDto announcementCreateDto)
        {
            try
            {
                if (_announcementRepository.Get(d => d.Title.ToLower() == announcementCreateDto.Title.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Aynı isme sahip duyuru sistemde mevcut");
                    return BadRequest(ModelState);
                }
                if (announcementCreateDto == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return _response;
                }
                var announcement = _mapper.Map<Announcement>(announcementCreateDto);
                _announcementRepository.Create(announcement);

                _response.Result = announcement;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpPut("PutAnnouncement/{id}")]
        public ActionResult<APIResponse> PutAnnouncement([FromBody] AnnouncementUpdateDto announcementUpdateDto, Guid id)
        {
            try
            {
                if (announcementUpdateDto == null || announcementUpdateDto.Id != id)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var announcement = _announcementRepository.Get(d => d.Id == id);
                if (announcement == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                _mapper.Map(announcementUpdateDto, announcement);
                _announcementRepository.UpdateAnnouncement(announcement);
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
