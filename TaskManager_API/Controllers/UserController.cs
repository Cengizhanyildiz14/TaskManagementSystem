using AutoMapper;
using Business;
using Business.IServices;
using Data.Entities;
using Dto.UserDtos;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TaskManager_API.Controllers
{
    [Route("api/TaskManager")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;

        public UserController(IUserRepository userRepository, IMapper mapper, IDepartmentRepository departmentRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            this._response = new APIResponse();
            _departmentRepository = departmentRepository;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var rememberMe = loginRequestDto.RememberMe;
            var loginResponse = _userRepository.Login(loginRequestDto, rememberMe);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { "Email hatalı" };
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpGet("GetAllUsers")]
        public ActionResult<APIResponse> GetAll()
        {
            try
            {
                var users = _userRepository.GetAllUserWithDetails();
                if (users == null || !users.Any())
                {
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return NoContent();
                }

                var mappedUsers = users.Select(user => new
                {
                    User = _mapper.Map<UserDto>(user),
                    AssignedTasks = _mapper.Map<List<TaskDto>>(user.Tasks), // Atanan görevler
                    CreatedTasks = _mapper.Map<List<TaskDto>>(user.CreatedTasks) // Oluşturulan görevler
                }).ToList();

                _response.Result = mappedUsers;
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
                return _response;
            }
        }



        [HttpPost("PostUser")]
        public ActionResult<APIResponse> PostUser([FromBody] UserCreateDto userCreateDto)
        {
            try
            {
                if (_userRepository.Get(u => u.Email.ToLower() == userCreateDto.Email.ToLower()) != null)
                {
                    ModelState.AddModelError("CustomErrorMessages", "Bu mail adresine sahip kullanıcı sistemde mecvut");
                    return BadRequest(ModelState);
                }
                if (userCreateDto == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var user = _mapper.Map<User>(userCreateDto);
                var department = _departmentRepository.Get(d => d.Id == userCreateDto.DepartmentId);
                if (department == null)
                {
                    ModelState.AddModelError("CustomErrorMessage", "Böyle Bir Departman Bulunamadı");
                    return BadRequest(ModelState);
                }

                user.Department = department;
                _userRepository.Create(user);

                _response.Result = _mapper.Map<UserDto>(user);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return _response;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("PutUser/{id}")]
        public ActionResult<APIResponse> PutUser([FromBody] UserUpdateDto userUpdateDto, int id)
        {
            try
            {
                if (userUpdateDto == null || userUpdateDto.Id != id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var user = _userRepository.Get(u => u.Id == id);
                if (user == null)
                {
                    ModelState.AddModelError("CustomErrorMessages", "Kullanıcı Bulunamadı");
                    return BadRequest(ModelState);
                }

                // Gelen DepartmentDto nesnesini kullanarak departmanı güncelle
                var department = _departmentRepository.Get(d => d.Id == userUpdateDto.Department.Id);
                if (department == null)
                {
                    ModelState.AddModelError("CustomErrorMessage", "Böyle Bir Departman Bulunamadı");
                    return BadRequest(ModelState);
                }

                // User nesnesine haritalama işlemini yap
                user.Department = department;
                user.Email = userUpdateDto.Email;
                user.PhoneNumber = userUpdateDto.PhoneNumber;
                user.Education = userUpdateDto.Education;
                user.Adress = userUpdateDto.Adress;

                _userRepository.UpdateUser(user);

                _response.Result = _mapper.Map<UserDto>(user);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return _response;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
                return _response;
            }
        }


        [HttpDelete("DeleteUser/{id}")]
        public ActionResult<APIResponse> DeleteUser(int id)
        {
            try
            {
                var user = _userRepository.Get(u => u.Id == id);
                if (user == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                _userRepository.Delete(user);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("GetUser/{id}")]
        public ActionResult<APIResponse> GetUser(int id)
        {
            try
            {
                var user = _userRepository.GetUserWithDetails(id);
                if (user == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var mappUser = _mapper.Map<UserDto>(user);
                var mappedAssignedTasks = _mapper.Map<List<TaskDto>>(user.Tasks);
                var mappedCreatedTasks = _mapper.Map<List<TaskDto>>(user.CreatedTasks);

                _response.Result = new { User = mappUser, AssignedTasks = mappedAssignedTasks, CreatedTasks = mappedCreatedTasks };
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("GetUsersTasks/{id}")]
        public ActionResult<APIResponse> GetUsersTask(int id)
        {
            try
            {
                var usersTasks = _userRepository.GetUserTask(id);
                if (usersTasks == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var mapUsersTasks = _mapper.Map<List<TaskDto>>(usersTasks);
                _response.Result = mapUsersTasks;
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return _response;
        }
    }
}
