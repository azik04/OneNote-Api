using BLL.Services.Interfaces;
using Domain.DTO_s.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace OneNote_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }


        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn(LogInDTO vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var data = await _service.LogIn(vm);
            if (data.StatusCode == Domain.Enum.StatusCode.OK)
                return Ok(data);

            return BadRequest(data);
        }


        [Authorize("User")]
        [HttpPut("{id}/User")]
        public async Task<IActionResult> Update(long id , UpdateUserDTO vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var data = await _service.Update(id , vm);
            if (data.StatusCode == Domain.Enum.StatusCode.OK)
                return Ok(data);

            return BadRequest(data);
        }


        [Authorize("User")]
        [HttpPut("{id}/ChangePassword")]
        public async Task<IActionResult> ChangePassword(long id, ChangePasswordDTO vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var data = await _service.ChangePassword(id, vm);
            if (data.StatusCode == Domain.Enum.StatusCode.OK)
                return Ok(data);

            return BadRequest(data);
        }
    }
}