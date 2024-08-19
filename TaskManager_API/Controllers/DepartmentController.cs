using AutoMapper;
using Business;
using Business.IServices;
using Data.Entities;
using Dto.DepartmentDtos;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager_API.Controllers
{
    [Route("api/Department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentService;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;

        public DepartmentController(IDepartmentRepository departmentService, IMapper mapper)
        {
            _departmentService = departmentService;
            _mapper = mapper;
            this._response = new APIResponse();
        }

        [HttpGet("GetAllDepartments")]
        public ActionResult<APIResponse> GetAllDepartments()
        {
            try
            {
                IEnumerable<Department> departmentList = _departmentService.Getall();
                _response.Result = _mapper.Map<List<DepartmentDto>>(departmentList);
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

        [HttpGet("GetDepartment/{id}")]
        public ActionResult<APIResponse> GetDepartment(int id)
        {
            try
            {
                if (id == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return _response;
                }
                var department = _departmentService.Get(d => d.Id == id);
                if (department == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return _response;
                }
                _response.Result = _mapper.Map<DepartmentDto>(department);
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

        [HttpDelete("DeleteDepartment/{id}")]
        public ActionResult<APIResponse> DeleteDepartment(int id)
        {
            try
            {
                if (id == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return _response;
                }
                var department = _departmentService.Get(d => d.Id == id);
                if (department == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return _response;
                }
                _departmentService.Delete(department);
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

        [HttpPost("PostDepartment")]
        public ActionResult<APIResponse> PostDepartment([FromBody] DepartmentCreateDto departmentCreateDto)
        {
            try
            {
                if (_departmentService.Get(d => d.DepartmentName.ToLower() == departmentCreateDto.DepartmentName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Aynı isme sahip department sistemde mevcut");
                    return BadRequest(ModelState);
                }
                if (departmentCreateDto == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return _response;
                }
                var department = _mapper.Map<Department>(departmentCreateDto);
                _departmentService.Create(department);

                _response.Result = department;
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


        [HttpPut("PutDepartment/{id}")]
        public ActionResult<APIResponse> PutDepartment([FromBody] DepartmentUpdateDto departmentUpdateDto, int id)
        {
            try
            {
                if (departmentUpdateDto == null || departmentUpdateDto.Id != id)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var department = _departmentService.Get(d => d.Id == id);
                if (department == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                _mapper.Map(departmentUpdateDto, department);
                _departmentService.UpdatDepartment(department);
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
