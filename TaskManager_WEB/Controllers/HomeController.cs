﻿using AutoMapper;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using TaskManager_WEB.Models;
using TaskManager_WEB.Services.IServices;

namespace TaskManager_WEB.Controllers
{
    public class HomeController : Controller
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public HomeController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var token = HttpContext.Request.Cookies["AuthToken"];
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            ViewBag.FullName = jwtToken.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
            ViewBag.Email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            ViewBag.DepartmentName = jwtToken.Claims.FirstOrDefault(c => c.Type == "DepartmentName")?.Value;

            var response = await _userService.GetAll<APIResponse>();

            if (response == null || !response.IsSuccess)
            {
                return NotFound();
            }

            var users = JsonConvert.DeserializeObject<List<UserResult>>(Convert.ToString(response.result));

            var viewModel = users.Select(user =>
            {
                var allTasks = user.AssignedTasks ?? new List<TaskDto>();

                return new UserViewModel
                {
                    User = _mapper.Map<UserDto>(user.User),
                    AssignedTasks = allTasks, 
                    CreatedTasks = new List<TaskDto>() 
                };
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        [Route("Profil")]
        public async Task<IActionResult> Profile(int id)
        {
            var token = HttpContext.Request.Cookies["AuthToken"];
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var fullname = jwtToken.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;

            var nameParts = fullname.Split(' ');
            ViewBag.FirstName = nameParts.Length > 0 ? nameParts[0] : string.Empty;
            ViewBag.LastName = nameParts.Length > 1 ? string.Join(" ",nameParts.Skip(1)): string.Empty;


            ViewBag.FullName = fullname;
            ViewBag.Email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            ViewBag.DepartmentName = jwtToken.Claims.FirstOrDefault(c => c.Type == "DepartmentName")?.Value;
            return View();
        }

    }
}
