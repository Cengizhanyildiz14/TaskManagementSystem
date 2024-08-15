using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager_WEB.Models;
using TaskManager_WEB.Services;
using TaskManager_WEB.Services.IServices;

namespace TaskManager_WEB.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;



        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }



        [HttpGet]
        [Authorize(Policy = "IK")]
        public IActionResult DepartmentCreate()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "IK")]
        public async Task<IActionResult> DepartmentCreate(DepartmentCreateDto dto)
        {
            if (dto == null)
            {
                ModelState.AddModelError("CustomErrorMessage", "Geçersiz Dto");
            }

            var departmentsResponse = await _departmentService.CreateDepartment<APIResponse>(dto);

            if (departmentsResponse == null || !departmentsResponse.IsSuccess)
            {
                ModelState.AddModelError("", "Görev oluşturulurken bir hata oluştu.");
                return View(dto);
            }

            return RedirectToAction("getallusers", "user");
        }
    }
}
